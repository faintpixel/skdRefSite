using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SkdRefSiteAPI.DAO.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SkdRefSiteAPI.DAO
{
    public class BatchDAO
    {
        public MongoClient _mongoClient;
        private IMongoDatabase _db;
        private IMongoCollection<Batch> _collection;

        public BatchDAO()
        {
            _mongoClient = new MongoClient(AppSettings.MongoDBConnection);
            _db = _mongoClient.GetDatabase("refsite");
            _collection = _db.GetCollection<Batch>("batches");
        }

        public BatchDAO(string connectionString)
        {
            _mongoClient = new MongoClient(connectionString);
            _db = _mongoClient.GetDatabase("refsite");
            _collection = _db.GetCollection<Batch>("batches");
        }

        public void Save(Batch batch)
        {
            _collection.InsertOne(batch);
        }

        public async Task DeleteReference(string id, ReferenceType type, User user)
        {
            var batch = await Get(id);
            if (batch.User != user.Email)
                throw new Exception("Access to delete batch denied");
            _collection.DeleteOne(filter: new BsonDocument("_id", id));

            if(type == ReferenceType.Animal)
            {
                var dao = ReferenceDAOFactory.GetAnimalsDAO();
                var images = await dao.Search(new Models.Animals.AnimalClassifications { BatchId = id });
                dao.DeleteReferences(images);
            }

        }

        public async Task<List<Batch>> GetUserBatches(string user)
        {
            var query = _collection.AsQueryable();
            query = query.Where(x => x.User == user).OrderByDescending(x => x.CreationDate);

            return await query.ToListAsync();
        }

        public async Task<Batch> Get(string id)
        {
            var query = _collection.AsQueryable();
            query = query.Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public string GetExistingBatchId(string user, string batchName)
        {
            var query = _collection.AsQueryable();
            var id = query.Where(x => x.User == user && x.Name == batchName).Select(x => x.Id).FirstOrDefault();

            return id;
        }
    }
}
