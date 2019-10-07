using System;
using System.Collections.Generic;
using System.Text;

namespace SkdAPI.RefSite.DAO.Models
{
    public class Report
    {
        public string ImageId { get; set; }
        public string Comment { get; set; }
        public User User { get; set; }
        public ReportType ReportType { get; set; }
        public DateTime Date { get; set; }
        public string ReferenceType { get; set; }
    }
}
