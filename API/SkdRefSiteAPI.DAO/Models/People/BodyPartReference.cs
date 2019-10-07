using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdAPI.RefSite.DAO.Models.People
{
    public class BodyPartReference : Image
    {
        public BodyPartClassifications Classifications { get; set; }
    }
}
