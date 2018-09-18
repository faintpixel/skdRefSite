using System;
using System.Collections.Generic;
using System.Text;

namespace SkdRefSiteAPI.DAO.Models.Vegetation
{
    public class VegetationReference : Image
    {
        public VegetationClassifications Classifications { get; set; }
    }
}
