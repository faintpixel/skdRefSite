using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkdRefSiteAPI.DAO.Models
{
    public class Log
    {
        [BsonId]
        public string Id { get; set; }
        public DateTime Time { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string Parameters { get; set; }
        public string Exception { get; set; }
        
        public Log()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}
