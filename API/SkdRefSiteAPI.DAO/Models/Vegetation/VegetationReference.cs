using System;
using System.Collections.Generic;
using System.Text;

namespace SkdAPI.RefSite.DAO.Models.Vegetation
{
    public class VegetationReference : Image
    {
        public VegetationClassifications Classifications { get; set; }
    }
}
