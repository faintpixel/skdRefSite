using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdAPI.Controllers
{
    /// <summary>
    /// Debug methods for troubleshooting connectivity
    /// </summary>
    public class DebugController : Controller
    {
        /// <summary>
        /// Returns a string. No communication with other systems
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Debug/NoCommunication")]
        public string GetNoCommunication()
        {
            return "success";
        }
    }
}
