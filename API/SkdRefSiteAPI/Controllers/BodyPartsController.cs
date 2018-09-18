using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkdRefSiteAPI.DAO;
using SkdRefSiteAPI.DAO.Models;
using SkdRefSiteAPI.DAO.Models.People;
using SkdRefSiteAPI.DAO.Queryables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdRefSiteAPI.Controllers
{
    /// <summary>
    /// API for working with body part references
    /// </summary>
    public class BodyPartsController : Controller, IReferenceController<BodyPartReference, BodyPartClassifications>
    {
        private ReferenceDAO<BodyPartReference, BodyPartClassifications> _dao;

        /// <summary>
        /// Constructor
        /// </summary>
        public BodyPartsController()
        {
            var queryable = new BodyPartsQueryable();
            _dao = new ReferenceDAO<BodyPartReference, BodyPartClassifications>(ReferenceType.BodyPart, queryable);
        }

        /// <summary>
        /// Get body parts
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="recentImagesOnly"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/BodyParts")]
        public async Task<BodyPartReference> Get([FromQuery(Name = "")]BodyPartClassifications criteria, [FromQuery]bool? recentImagesOnly)
        {
            if (criteria == null)
                criteria = new BodyPartClassifications();
            return await _dao.Get(criteria, new List<string>(), recentImagesOnly); // TO DO - remove list
        }

        /// <summary>
        /// Gets the next image for a drawing session
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="excludeIds"></param>
        /// <param name="onlyRecentImages"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/BodyParts/Next")]
        public async Task<BodyPartReference> GetNext([FromQuery(Name = "")]BodyPartClassifications criteria, [FromBody]List<string> excludeIds, [FromQuery]bool? onlyRecentImages)
        {
            if (criteria == null)
                criteria = new BodyPartClassifications();
            if (excludeIds == null)
                excludeIds = new List<string>();

            var image = await _dao.Get(criteria, excludeIds, onlyRecentImages);

            return image;
        }

        /// <summary>
        /// Save body parts
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/BodyParts")]
        public async Task<List<BodyPartReference>> Save([FromBody]List<BodyPartReference> images)
        {
            var results = await _dao.Save(images);
            return images; // TO DO - return something better
        }

        /// <summary>
        /// Gets the number of body part references matching the criteria
        /// </summary>
        /// <param name="classifications"></param>
        /// <param name="recentImagesOnly"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/BodyParts/Count")]
        public async Task<int> Count([FromQuery(Name = "")]BodyPartClassifications classifications, [FromQuery]bool? recentImagesOnly)
        {
            if(classifications == null)
                classifications = new BodyPartClassifications();
            return await _dao.Count(classifications, recentImagesOnly);
        }
    }
}
