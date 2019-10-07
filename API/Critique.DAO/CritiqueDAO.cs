using MongoDB.Bson;
using MongoDB.Driver;
using SkdAPI.ArtCrit.DAO.Models;
using SkdAPI.Common;
using System;

namespace SkdAPI.ArtCrit.DAO
{
    public class CritiqueDAO
    {
        private MongoClient _mongoClient;
        private IMongoDatabase _db;
        private IMongoCollection<CritiqueRequest> _critiques;
        private Logger _logger;

        public CritiqueDAO()
        {
            _mongoClient = new MongoClient(AppSettings.MongoDBConnection);
            _db = _mongoClient.GetDatabase("refsite");
            _critiques = _db.GetCollection<CritiqueRequest>("critiqueRequests");
            _logger = new Logger(Databases.ARTCRIT, "CritiqueDAO");
        }

        public bool SaveRequest(CritiqueRequest request)
        {
            if (request == null)
                return false;

            try
            {
                if (string.IsNullOrEmpty(request.Id))
                    request.Id = ObjectId.GenerateNewId().ToString();
                _critiques.ReplaceOne(x => x.Id.Equals(request.Id), request, new UpdateOptions { IsUpsert = true });
            }
            catch (Exception ex)
            {
                _logger.Log("Save", ex, request);
                return false;
            }

            return true;
        }
    }
}
