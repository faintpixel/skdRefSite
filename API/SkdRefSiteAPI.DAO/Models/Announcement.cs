﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkdAPI.RefSite.DAO.Models
{
    public class Announcement
    {
        [BsonId]
        public string Id { get; set; }
        public string Value { get; set; }
    }
}
