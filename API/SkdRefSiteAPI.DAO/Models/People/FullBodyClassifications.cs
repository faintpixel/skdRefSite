using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdRefSiteAPI.DAO.Models.People
{
    public class FullBodyClassifications
    {
        public bool? NSFW { get; set; }
        public bool? Clothing { get; set; }
        public Gender? Gender { get; set; }
        public PoseType? PoseType { get; set; }
        public ViewAngle? ViewAngle { get; set; }
    }
}
