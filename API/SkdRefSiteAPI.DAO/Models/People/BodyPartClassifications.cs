using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdAPI.RefSite.DAO.Models.People
{
    public class BodyPartClassifications : BaseClassifications
    {
        public BodyPart? BodyPart { get; set; }
        public ViewAngle? ViewAngle { get; set; }
        public Gender? Gender { get; set; }
    }
}
