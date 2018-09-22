using System;
using System.Collections.Generic;
using System.Text;

namespace SkdRefSiteAPI.DAO.Models
{
    public class BaseClassifications
    {
        public string BatchId { get; set; }
        public string ImageId { get; set; }
        public DateTime? UploadDateStart { get; set; }
        public DateTime? UploadDateEnd { get; set; }
        public Status? Status { get; set; }
        public string UploadedBy { get; set; }
        public string FileName { get; set; }
        public string Photographer { get; set; }
        public string Model { get; set; }
    }
}
