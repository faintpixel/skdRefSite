﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SkdAPI.RefSite.DAO.Models
{
    public class ImageSaveResults
    {
        public List<Image> Images { get; set; }
        public string BatchId { get; set; }
        public bool Success { get; set; }
    }
}
