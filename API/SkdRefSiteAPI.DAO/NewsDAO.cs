using MongoDB.Bson;
using MongoDB.Driver;
using SkdRefSiteAPI.DAO.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SkdRefSiteAPI.DAO
{
    public class NewsDAO
    {
        private MongoClient _mongoClient;
        private IMongoDatabase _db;
        private IMongoCollection<News> _news;
        private IMongoCollection<Announcement> _announcement;
        private Logger _logger;

        public NewsDAO()
        {
            _mongoClient = new MongoClient(AppSettings.MongoDBConnection);
            _db = _mongoClient.GetDatabase("refsite");
            _news = _db.GetCollection<News>("news");
            _announcement = _db.GetCollection<Announcement>("announcement");
            _logger = new Logger("NewsDAO");
        }

        public bool Save(News news)
        {
            if (news == null)
                return false;

            try
            {
                if (string.IsNullOrEmpty(news.Id))
                    news.Id = ObjectId.GenerateNewId().ToString();
                //_news.ReplaceOne(new BsonDocument("Id", news.Id), news, new UpdateOptions { IsUpsert = true });
                _news.ReplaceOne(x => x.Id.Equals(news.Id), news, new UpdateOptions { IsUpsert = true });
            }
            catch(Exception ex)
            {
                _logger.Log("Save", ex, news);
                return false;
            }
            
            return true;
        }

        public bool SaveAnnouncement(Announcement announcement)
        {
            try
            {
                if (string.IsNullOrEmpty(announcement.Id))
                    announcement.Id = ObjectId.GenerateNewId().ToString();

                _announcement.DeleteMany(_ => true);
                _announcement.InsertOne(announcement);
            }
            catch (Exception ex)
            {
                throw; // TO DO - log
                return false;
            }

            return true;
        }

        public List<News> Get()
        {
            var results =_news.Find(_ => true).ToList();
            return results;
        }

        public Announcement GetAnnouncement()
        {
            var results = _announcement.Find(_ => true).FirstOrDefault();
            return results;
        }
    }
}
