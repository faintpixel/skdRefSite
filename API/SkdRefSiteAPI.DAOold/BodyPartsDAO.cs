using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SkdRefSiteAPI.DAO.Models;
using SkdRefSiteAPI.DAO.Models.People;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SkdRefSiteAPI.DAO
{
    public class BodyPartsDAO
    {
        public MongoClient _mongoClient;
        private IMongoDatabase _db;
        private IMongoCollection<BodyPartReference> _collection;

        public BodyPartsDAO()
        {
            _mongoClient = new MongoClient(AppSettings.MongoDBConnection);
            _db = _mongoClient.GetDatabase("refsite");
            _collection = _db.GetCollection<BodyPartReference>("bodyPartReferences");
        }

        public BodyPartsDAO(string connectionString)
        {
            _mongoClient = new MongoClient(connectionString);
            _db = _mongoClient.GetDatabase("refsite");
            _collection = _db.GetCollection<BodyPartReference>("bodyPartReferences");
        }

        public async Task<List<ReplaceOneResult>> Save(List<BodyPartReference> references)
        {
            try
            {
                var results = new List<ReplaceOneResult>();

                foreach (var reference in references)
                {
                    if (reference.Status == Status.Deleted)
                        DeleteReference(reference);
                    else
                    {
                        var replaceResult = await _collection.ReplaceOneAsync(
                            filter: new BsonDocument("_id", reference.Id),
                            options: new UpdateOptions { IsUpsert = true },
                            replacement: reference);
                        results.Add(replaceResult);
                    }
                }

                return results;
            }
            catch(Exception ex)
            {
                throw;
            }            
        }

        public void DeleteReference(BodyPartReference reference)
        {
            File.Delete(reference.Location);
            _collection.DeleteOne(filter: new BsonDocument("_id", reference.Id));
        }

        public async Task<int> Count(BodyPartClassifications classifications, bool? recentImagesOnly)
        {
            var data = GetQueryable(classifications, recentImagesOnly);
            var results = await data.CountAsync();

            return results;
        }

        public async Task<BodyPartReference> Get(BodyPartClassifications classifications, List<string> excludeIds, bool? recentImagesOnly)
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

        public async Task<List<BodyPartReference>> Search(string batchId)
        {
            var query = _collection.AsQueryable();
            query = query.Where(x => x.BatchId == batchId);

            return await query.ToListAsync();
        }

        private IMongoQueryable<BodyPartReference> GetQueryable(BodyPartClassifications classifications, bool? recentImagesOnly)
        {
            var query = _collection.AsQueryable();
            if (classifications.Gender.HasValue)
                query = query.Where(x => x.Classifications.Gender == classifications.Gender);
            if (classifications.BodyPart.HasValue)
                query = query.Where(x => x.Classifications.BodyPart == classifications.BodyPart);
            if (classifications.ViewAngle.HasValue)
                query = query.Where(x => x.Classifications.ViewAngle == classifications.ViewAngle);
            if (recentImagesOnly == true)
            {
                // get the most recent upload, then include anything within 30 days of that
            }

            query = query.Where(x => x.Status == Status.Active);

            return query;
        }
    }
}
