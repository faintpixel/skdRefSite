using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkdAPI.Common.Models;
using SkdAPI.RefSite.DAO;
using SkdAPI.RefSite.DAO.Models;
using SkdAPI.RefSite.DAO.Models.People;
using SkdAPI.RefSite.DAO.Queryables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdAPI.Controllers
{
    /// <summary>
    /// API for working with body part references
    /// </summary>
    public class BodyPartsController : BaseController, IReferenceController<BodyPartReference, BodyPartClassifications>
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
        /// <returns></returns>
        [HttpGet]
        [Authorize("admin")]
        [Route("api/BodyParts")]
        public async Task<List<BodyPartReference>> Search([FromQuery(Name = "")]BodyPartClassifications criteria, [FromQuery(Name = "")]OffsetLimit offsetLimit)
        {
            if (criteria == null)
                criteria = new BodyPartClassifications();
            return await _dao.Search(criteria, offsetLimit.Offset, offsetLimit.Limit);
        }

        /// <summary>
        /// Gets the next image for a drawing session
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="excludeIds"></param>
        /// <param name="recentImagesOnly"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/BodyParts/Next")]
        public async Task<BodyPartReference> GetNext([FromQuery(Name = "")]BodyPartClassifications criteria, [FromBody]List<string> excludeIds, [FromQuery]bool? recentImagesOnly)
        {
            if (criteria == null)
                criteria = new BodyPartClassifications();
            if (excludeIds == null)
                excludeIds = new List<string>();

            var image = await _dao.Get(criteria, excludeIds, recentImagesOnly);

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
            var user = GetCurrentUser();
            var results = await _dao.Save(images, user);
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
