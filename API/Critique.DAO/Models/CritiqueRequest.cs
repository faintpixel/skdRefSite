using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkdAPI.ArtCrit.DAO.Models
{
    public class CritiqueRequest
    {
        [BsonId]
        public string Id { get; set; }

        public string ImageUrl { get; set; }
        public string RequesterUsername { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Nsfw { get; set; }

        public List<Critique> Critiques { get; set; }
    }
}
