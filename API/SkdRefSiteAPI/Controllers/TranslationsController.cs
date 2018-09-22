using Microsoft.AspNetCore.Mvc;
using SkdRefSiteAPI.DAO;
using SkdRefSiteAPI.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdRefSiteAPI.Controllers
{
    /// <summary>
    /// API for working with translations
    /// </summary>
    public class TranslationsController : BaseController
    {
        /// <summary>
        /// Submit a translation
        /// </summary>
        [HttpPost]
        [Route("api/Translations")]
        public bool Save([FromBody]Translation translation)
        {
            var logger = new Logger("Translations");
            logger.Log("Translation Submission", $"A new translation has been submitted for language '{translation.Language}' by author '{translation.Author}'", LogType.Translation, translation.Comments, translation.TranslationFile);
            return true;
        }
    }
}
