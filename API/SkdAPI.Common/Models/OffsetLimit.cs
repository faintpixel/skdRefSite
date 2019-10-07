using System;
using System.Collections.Generic;
using System.Text;

namespace SkdAPI.Common.Models
{
    public class OffsetLimit
    {
        public int Offset { get; set; }
        public int Limit { get; set; }

        public OffsetLimit()
        {
            Offset = 0;
            Limit = int.MaxValue;
        }
    }
}
