using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdAPI.RefSite.DAO.Models.People
{
    public class FullBodyClassifications : BaseClassifications
    {
        public bool? NSFW { get; set; }
        public bool? Clothing { get; set; }
        public Gender? Gender { get; set; }
        public PoseType? PoseType { get; set; }
        public ViewAngle? ViewAngle { get; set; }
    }
}
