using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkdRefSiteAPI.DAO;
using SkdRefSiteAPI.DAO.Models;
using SkdRefSiteAPI.DAO.Models.Vegetation;
using SkdRefSiteAPI.DAO.Queryables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdRefSiteAPI.Controllers
{
    /// <summary>
    /// API for working with vegetation references
    /// </summary>
    public class VegetationController : BaseController, IReferenceController<VegetationReference, VegetationClassifications>
    {
        private ReferenceDAO<VegetationReference, VegetationClassifications> _dao;

        /// <summary>
        /// Constructor
        /// </summary>
        public VegetationController()
        {
            var queryable = new VegetationQueryable();
            _dao = new ReferenceDAO<VegetationReference, VegetationClassifications>(ReferenceType.Vegetation, queryable);
        }

        /// <summary>
        /// Get vegetation
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="recentImagesOnly"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Vegetation")]
        public async Task<VegetationReference> Get([FromQuery(Name = "")]VegetationClassifications criteria, [FromQuery]bool? recentImagesOnly = null)
        {
            if (criteria == null)
                criteria = new VegetationClassifications();

            var image = await _dao.Get(criteria, new List<string>(), recentImagesOnly); // TO DO - get rid of this list

            return image;
        }

        /// <summary>
        /// Gets the next image for a drawing session
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="excludeIds"></param>
        /// <param name="onlyRecentImages"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Vegetation/Next")]
        public async Task<VegetationReference> GetNext([FromQuery(Name = "")]VegetationClassifications criteria, [FromBody]List<string> excludeIds, [FromQuery]bool? onlyRecentImages = null)
        {
            if (criteria == null)
                criteria = new VegetationClassifications();
            if (excludeIds == null)
                excludeIds = new List<string>();

            var image = await _dao.Get(criteria, excludeIds, onlyRecentImages);

            return image;
        }

        /// <summary>
        /// Save vegetation
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Vegetation")]
        public async Task<List<VegetationReference>> Save([FromBody]List<VegetationReference> images)
        {
            var results = await _dao.Save(images);
            return images; // TO DO - return something better
        }

        /// <summary>
        /// Gets the number of vegetation references matching the criteria
        /// </summary>
        /// <param name="classifications"></param>
        /// <param name="recentImagesOnly"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Vegetation/Count")]
        public async Task<int> Count([FromQuery(Name = "")]VegetationClassifications classifications, [FromQuery]bool? recentImagesOnly)
        {
            if (classifications == null)
                classifications = new VegetationClassifications();
            return await _dao.Count(classifications, recentImagesOnly);
        }
    }
}
