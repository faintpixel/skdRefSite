using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkdAPI.ArtCrit.DAO.Models
{
    public class Critique
    {
        [BsonId]
        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public string Username { get; set; }
        public string Comment { get; set; }
        public List<Indicator> Indicators { get; set; }
        public string PaintoverUrl { get; set; }
        public List<Box> Boxes { get; set; }
    }
}
