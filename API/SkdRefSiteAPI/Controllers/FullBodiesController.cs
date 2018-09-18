using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkdRefSiteAPI.DAO;
using SkdRefSiteAPI.DAO.Models;
using SkdRefSiteAPI.DAO.Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdRefSiteAPI.Controllers
{
    /// <summary>
    /// API for working with full body references
    /// </summary>
    public class FullBodiesController : Controller
    {
        private ReferenceDAO<FullBodyReference, FullBodyClassifications> _dao;

        /// <summary>
        /// Constructor
        /// </summary>
        public FullBodiesController()
        {
            var queryable = new FullBodiesQueryable();
            _dao = new ReferenceDAO<FullBodyReference, FullBodyClassifications>(ReferenceType.FullBody, queryable);
        }

        /// <summary>
        /// Gets full bodies
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="recentImagesOnly"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/FullBodies")]
        public async Task<FullBodyReference> Get([FromQuery(Name = "")]FullBodyClassifications criteria, [FromQuery]bool? recentImagesOnly)
        {
            if (criteria == null)
                criteria = new FullBodyClassifications();

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
        [Route("api/FullBodies/Next")]
        public async Task<FullBodyReference> GetNext([FromQuery(Name = "")]FullBodyClassifications criteria, [FromBody]List<string> excludeIds, [FromQuery]bool? onlyRecentImages)
        {
            if (criteria == null)
                criteria = new FullBodyClassifications();
            if (excludeIds == null)
                excludeIds = new List<string>();

            var image = await _dao.Get(criteria, excludeIds, onlyRecentImages);

            return image;
        }

        /// <summary>
        /// Saves full bodies
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/FullBodies")]
        public async Task<List<FullBodyReference>> Save([FromBody]List<FullBodyReference> images)
        {
            var result = await _dao.Save(images);
            return images; // TO DO - return something better
        }

        /// <summary>
        /// Gets the number of full body references matching the criteria
        /// </summary>
        /// <param name="classifications"></param>
        /// <param name="recentImagesOnly"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/FullBodies/Count")]
        public async Task<int> Count([FromQuery(Name = "")]FullBodyClassifications classifications, [FromQuery]bool? recentImagesOnly)
        {
            if (classifications == null)
                classifications = new FullBodyClassifications();
            return await _dao.Count(classifications, recentImagesOnly);
        }
    }
}
