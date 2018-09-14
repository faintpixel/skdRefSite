using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdRefSiteAPI.DAO.Models.Animals
{
    public class AnimalReference : Image
    {
        public AnimalClassifications Classifications { get; set; }
    }
}
