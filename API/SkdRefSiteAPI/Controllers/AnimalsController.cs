using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkdRefSiteAPI.DAO;
using SkdRefSiteAPI.DAO.Models;
using SkdRefSiteAPI.DAO.Models.Animals;
using SkdRefSiteAPI.DAO.Queryables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdRefSiteAPI.Controllers
{
    /// <summary>
    /// API for working with animal references
    /// </summary>
    public class AnimalsController : Controller, IReferenceController<AnimalReference, AnimalClassifications>
    {
        private ReferenceDAO<AnimalReference, AnimalClassifications> _dao;

        /// <summary>
        /// Constructor
        /// </summary>
        public AnimalsController()
        {
            var queryable = new AnimalsQueryable();
            _dao = new ReferenceDAO<AnimalReference, AnimalClassifications>(DAO.Models.ReferenceType.Animal, queryable);
        }
        /// <summary>
        /// Get animals
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Animals")]
        public async Task<List<AnimalReference>> Search([FromQuery(Name = "")]AnimalClassifications criteria, [FromQuery(Name ="")]OffsetLimit offsetLimit)
        {
            if (criteria == null)
                criteria = new AnimalClassifications();

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
        [Route("api/Animals/Next")]
        public async Task<AnimalReference> GetNext([FromQuery(Name = "")]AnimalClassifications criteria, [FromBody]List<string> excludeIds, [FromQuery]bool? onlyRecentImages = null)
        {
            if (criteria == null)
                criteria = new AnimalClassifications();
            if (excludeIds == null)
                excludeIds = new List<string>();

            var image = await _dao.Get(criteria, excludeIds, onlyRecentImages);

            return image;
        }

        /// <summary>
        /// Save animals
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Animals")]
        public async Task<List<AnimalReference>> Save([FromBody]List<AnimalReference> images)
        {
            var results = await _dao.Save(images);
            return images; // TO DO - return something better
        }

        /// <summary>
        /// Gets the number of animal references matching the criteria
        /// </summary>
        /// <param name="classifications"></param>
        /// <param name="recentImagesOnly"></param>
        /// <returns></returns>
        [HttpGet]        
        [Route("api/Animals/Count")]
        public async Task<int> Count([FromQuery(Name = "")]AnimalClassifications classifications, [FromQuery]bool? recentImagesOnly)
        {
            if (classifications == null)
                classifications = new AnimalClassifications();
            return await _dao.Count(classifications, recentImagesOnly);
        }
    }
}
