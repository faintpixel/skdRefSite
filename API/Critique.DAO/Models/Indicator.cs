using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkdAPI.ArtCrit.DAO.Models
{
    public class Indicator
    {
        [BsonId]
        public string Id { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
