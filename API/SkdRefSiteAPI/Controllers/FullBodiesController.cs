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
    /// API for working with full body references
    /// </summary>
    public class FullBodiesController : BaseController, IReferenceController<FullBodyReference, FullBodyClassifications>
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
        /// <returns></returns>
        [HttpGet]
        [Authorize("admin")]
        [Route("api/FullBodies")]
        public async Task<List<FullBodyReference>> Search([FromQuery(Name = "")]FullBodyClassifications criteria, [FromQuery(Name = "")]OffsetLimit offsetLimit)
        {
            if (criteria == null)
                criteria = new FullBodyClassifications();
            if (offsetLimit == null)
                offsetLimit = new OffsetLimit();

            return await _dao.Search(criteria, offsetLimit.Offset, offsetLimit.Limit);
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
            var user = GetCurrentUser();
            var result = await _dao.Save(images, user);
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
