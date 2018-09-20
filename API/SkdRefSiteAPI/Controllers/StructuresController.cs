using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkdRefSiteAPI.DAO;
using SkdRefSiteAPI.DAO.Models;
using SkdRefSiteAPI.DAO.Models.Structures;
using SkdRefSiteAPI.DAO.Queryables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdRefSiteAPI.Controllers
{
    /// <summary>
    /// API for working with structure references
    /// </summary>
    public class StructuresController: BaseController, IReferenceController<StructureReference, StructureClassifications>
    {
        private ReferenceDAO<StructureReference, StructureClassifications> _dao;

        /// <summary>
        /// Constructor
        /// </summary>
        public StructuresController()
        {
            var queryable = new StructuresQueryable();
            _dao = new ReferenceDAO<StructureReference, StructureClassifications>(ReferenceType.Structure, queryable);
        }

        /// <summary>
        /// Get Structures
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Structures")]
        public async Task<List<StructureReference>> Search([FromQuery(Name = "")]StructureClassifications criteria, [FromQuery(Name = "")]OffsetLimit offsetLimit)
        {
            if (criteria == null)
                criteria = new StructureClassifications();

            var image = await _dao.Search(criteria, offsetLimit.Offset, offsetLimit.Limit);

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
        [Route("api/Structures/Next")]
        public async Task<StructureReference> GetNext([FromQuery(Name = "")]StructureClassifications criteria, [FromBody]List<string> excludeIds, [FromQuery]bool? onlyRecentImages = null)
        {
            if (criteria == null)
                criteria = new StructureClassifications();
            if (excludeIds == null)
                excludeIds = new List<string>();

            var image = await _dao.Get(criteria, excludeIds, onlyRecentImages);

            return image;
        }

        /// <summary>
        /// Save structures
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Structures")]
        public async Task<List<StructureReference>> Save([FromBody]List<StructureReference> images)
        {
            var results = await _dao.Save(images);
            return images; // TO DO - return something better
        }

        /// <summary>
        /// Gets the number of structure references matching the criteria
        /// </summary>
        /// <param name="classifications"></param>
        /// <param name="recentImagesOnly"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Structures/Count")]
        public async Task<int> Count([FromQuery(Name = "")]StructureClassifications classifications, [FromQuery]bool? recentImagesOnly)
        {
            if (classifications == null)
                classifications = new StructureClassifications();
            return await _dao.Count(classifications, recentImagesOnly);
        }
    }
}
