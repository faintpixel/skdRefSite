using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SkdRefSiteAPI.DAO.Models;
using SkdRefSiteAPI.DAO.Models.Animals;
using SkdRefSiteAPI.DAO.Models.People;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SkdRefSiteAPI.DAO
{
    public class ReferenceDAO<TReference, TClassifications> where TReference : Image
    {
        public MongoClient _mongoClient;
        private IMongoDatabase _db;
        private IMongoCollection<TReference> _collection;
        private Logger _logger;
        private IQueryable<TReference, TClassifications> _queryable;

        public ReferenceDAO(ReferenceType type, IQueryable<TReference, TClassifications> queryable)
        {
            string collectionName = GetCollectionName(type);

            _mongoClient = new MongoClient(AppSettings.MongoDBConnection);
            _db = _mongoClient.GetDatabase("refsite");
            _collection = _db.GetCollection<TReference>(collectionName);
            _logger = new Logger("ReferenceDAO");
            _queryable = queryable;
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
                collectionName = "vegetationStructure";
            else
                throw new Exception("Collection not specified");

            return collectionName;
        }

        public async Task<List<ReplaceOneResult>> Save(List<TReference> references)
        {
            try
            {
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

                return results;
            }
            catch (Exception ex)
            {
                _logger.Log("Save", ex, references);
                throw;
            }
        }

        public void DeleteReference(Image reference)
        {
            File.Delete(reference.Location);
            _collection.DeleteOne(filter: new BsonDocument("_id", reference.Id));
        }

        public async Task<int> Count(TClassifications classifications, bool? recentImagesOnly)
        {
            var data = GetQueryable(classifications, recentImagesOnly);
            var results = await data.CountAsync();

            return results;
        }

        public async Task<TReference> Get(TClassifications classifications, List<string> excludeIds, bool? recentImagesOnly)
        {
            var data = GetQueryable(classifications, recentImagesOnly);
            data = data.Where(x => !excludeIds.Contains(x.Id));
            var recordCount = await data.CountAsync();
            if (recordCount == 0)
                return null;

            var random = RandomGenerator.Next(0, recordCount);

            var result = data.Skip(random).First();

            return result;
        }

        public async Task<List<TReference>> Search(string batchId)
        {
            var query = _collection.AsQueryable();
            query = query.Where(x => x.BatchId == batchId);

            return await query.ToListAsync();
        }

        private IMongoQueryable<TReference> GetQueryable(TClassifications classifications, bool? recentImagesOnly)
        {
            return _queryable.GetQueryable(_collection, classifications, recentImagesOnly);
        }

    }
}
