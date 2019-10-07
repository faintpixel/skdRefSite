using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SkdAPI.Common;
using SkdAPI.Common.Models;
using SkdAPI.RefSite.DAO.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SkdAPI.RefSite.DAO
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
            _logger = new Logger(Databases.REFSITE, "NewsDAO");
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
                _logger.Log("SaveAnnouncement", ex, announcement);
                return false;
            }

            return true;
        }

        public List<News> Get(OffsetLimit offsetLimit)
        {
            var query = _news.AsQueryable();
            query = query.OrderByDescending(x => x.Date);
            query = query.Skip(offsetLimit.Offset);
            query = query.Take(offsetLimit.Limit);
            var results = query.ToList();
            return results;
        }

        public Announcement GetAnnouncement()
        {
            var results = _announcement.Find(_ => true).FirstOrDefault();
            return results;
        }
    }
}
