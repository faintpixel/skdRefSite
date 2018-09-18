using System;
using System.Collections.Generic;
using System.Text;

namespace SkdRefSiteAPI.DAO.Models.Structures
{
    public class StructureReference : Image
    {
        public StructureClassifications Classifications { get; set; }
    }
}
