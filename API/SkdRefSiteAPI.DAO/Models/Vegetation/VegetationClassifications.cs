using System;
using System.Collections.Generic;
using System.Text;

namespace SkdAPI.RefSite.DAO.Models.Vegetation
{
    public class VegetationClassifications : BaseClassifications
    {
        public VegetationType? VegetationType { get; set; }
        public PhotoType? PhotoType { get; set; }
    }
}
