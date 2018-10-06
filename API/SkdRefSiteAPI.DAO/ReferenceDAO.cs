using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SkdRefSiteAPI.DAO.Models;
using SkdRefSiteAPI.DAO.Models.Animals;
using SkdRefSiteAPI.DAO.Models.People;
using SkdRefSiteAPI.DAO.Queryables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace SkdRefSiteAPI.DAO
{
    public class ReferenceDAO<TReference, TClassifications> 
        where TReference : Image 
        where TClassifications : BaseClassifications
    {
        public MongoClient _mongoClient;
        private IMongoDatabase _db;
        private IMongoCollection<TReference> _collection;
        private Logger _logger;
        private IQueryable<TReference, TClassifications> _queryable;
        private readonly ReferenceType REFERENCE_TYPE;

        public ReferenceDAO(ReferenceType type, IQueryable<TReference, TClassifications> queryable)
        {
            string collectionName = GetCollectionName(type);

            _mongoClient = new MongoClient(AppSettings.MongoDBConnection);
            _db = _mongoClient.GetDatabase("refsite");
            _collection = _db.GetCollection<TReference>(collectionName);
            _logger = new Logger("ReferenceDAO");
            _queryable = queryable;
            REFERENCE_TYPE = type;
        }

        private string GetCollectionName(ReferenceType type)
        {
            string collectionName;
            if (type == ReferenceType.Animal)
                collectionName = "animalReferences";
            else if (type == ReferenceType.BodyPart)
                collectionName = "bodyPartReferences";
            else if (type == ReferenceType.FullBody)
                collectionName = "fullBodyReferences";
            else if (type == ReferenceType.Structure)
                collectionName = "structureReferences";
            else if (type == ReferenceType.Vegetation)
                collectionName = "vegetationReferences";
            else
                throw new Exception("Collection not specified");

            return collectionName;
        }

        public async Task<List<ReplaceOneResult>> Save(List<TReference> references, User user)
        {
            await CheckSavePermissions(references, user);
            try
            {
                if (user.IsAdmin == false)
                    foreach (var reference in references)
                    {
                        if (reference.Status == Status.Deleted)
                            reference.Status = Status.DeleteRequested;
                        else
                            reference.Status = Status.Pending;
                    }


                var results = new List<ReplaceOneResult>();

                foreach (var reference in references)
                {
                    Image refImage = (Image)reference;
                    if (refImage.Status == Status.Deleted)
                        DeleteReference(refImage);
                    else
                    {
                        var replaceResult = await _collection.ReplaceOneAsync(
                            filter: new BsonDocument("_id", refImage.Id),
                            options: new UpdateOptions { IsUpsert = true },
                            replacement: reference);
                        results.Add(replaceResult);
                    }
                }

                if (user.IsAdmin == false)
                    _logger.Log("Reference Added", $"User {user.Name} ({user.Email}) has added {references.Count} new {REFERENCE_TYPE} images", LogType.Image,  user);

                return results;
            }
            catch (Exception ex)
            {
                _logger.Log("Save", ex, references);
                throw;
            }
        }

        private async Task CheckSavePermissions(List<TReference> references, User user)
        {
            if (user.IsAdmin)
                return;

            var batchIds = references.Select(x => x.BatchId).Distinct().ToList();
            var batchDAO = new BatchDAO();
            foreach(var id in batchIds)
            {
                var batch = await batchDAO.Get(id);
                if (batch.User != user.Email)
                    throw new Exception("Unauthorized");
            }
        }

        public void DeleteReference(Image reference)
        {
            File.Delete(reference.Location);
            _collection.DeleteOne(filter: new BsonDocument("_id", reference.Id));
        }

        public void DeleteReferences(List<TReference> references)
        {
            foreach(var reference in references)
            {
                try
                {
                    File.Delete(reference.Location);
                    _collection.DeleteOne(filter: new BsonDocument("_id", reference.Id));
                }
                catch(Exception ex)
                {
                    _logger.Log("DeleteReferences", ex, reference);
                }
            }            
        }

        public async Task<int> Count(TClassifications classifications, bool? recentImagesOnly)
        {
            classifications.Status = Status.Active;
            var data = GetQueryable(classifications, recentImagesOnly);
            var results = await data.CountAsync();

            return results;
        }

        public async Task<TReference> Get(TClassifications classifications, List<string> excludeIds, bool? recentImagesOnly)
        {
            classifications.Status = Status.Active;
            var data = GetQueryable(classifications, recentImagesOnly);
            data = data.Where(x => !excludeIds.Contains(x.Id));
            var recordCount = await data.CountAsync();
            if (recordCount == 0)
                return null;

            var random = RandomGenerator.Next(0, recordCount);

            var result = data.Skip(random).First();

            return result;
        }

        public async Task<List<TReference>> Search(TClassifications classifications, int offset = 0, int limit = int.MaxValue)
        {
            var query = _queryable.GetQueryable(_collection, classifications, false);
            if(string.IsNullOrWhiteSpace(classifications.BatchId) == false)
                query = query.Where(x => x.BatchId == classifications.BatchId);
            if (classifications.UploadDateStart.HasValue)
                query = query.Where(x => x.UploadDate >= classifications.UploadDateStart);
            if (classifications.UploadDateEnd.HasValue)
                query = query.Where(x => x.UploadDate <= classifications.UploadDateEnd);
            if (classifications.Status.HasValue)
                query = query.Where(x => x.Status == classifications.Status);
            if (string.IsNullOrWhiteSpace(classifications.UploadedBy) == false)
                query = query.Where(x => x.UploadedBy.Contains(classifications.UploadedBy));
            if (string.IsNullOrWhiteSpace(classifications.FileName) == false)
                query = query.Where(x => x.File.Contains(classifications.FileName));
            if (string.IsNullOrWhiteSpace(classifications.Photographer) == false)
                query = query.Where(x => x.Photographer != null && x.Photographer.Name.ToLower().Contains(classifications.Photographer.ToLower()));
            if (string.IsNullOrWhiteSpace(classifications.Model) == false)
                query = query.Where(x => x.Model != null && x.Model.Name.ToLower().Contains(classifications.Model.ToLower()));
            if (string.IsNullOrWhiteSpace(classifications.ImageId) == false)
                query = query.Where(x => x.Id == classifications.ImageId);

            query = query.Skip(offset);
            query = query.Take(limit);

            return await query.ToListAsync();
        }

        private IMongoQueryable<TReference> GetQueryable(TClassifications classifications, bool? recentImagesOnly)
        {
            return _queryable.GetQueryable(_collection, classifications, recentImagesOnly);
        }

    }
}
