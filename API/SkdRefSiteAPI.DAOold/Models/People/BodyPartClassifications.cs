using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdRefSiteAPI.DAO.Models.People
{
    public class BodyPartClassifications
    {
        public BodyPart? BodyPart { get; set; }
        public ViewAngle? ViewAngle { get; set; }
        public Gender? Gender { get; set; }
    }
}
