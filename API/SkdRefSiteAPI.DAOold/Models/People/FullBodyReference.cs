using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdRefSiteAPI.DAO.Models.People

{
    public class FullBodyReference : Image
    {
        public FullBodyClassifications Classifications { get; set; }
    }
}
