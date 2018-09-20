using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkdRefSiteAPI.DAO.Models;
using SkdRefSiteAPI.DAO.Models.Animals;

namespace SkdRefSiteAPI.Controllers
{
    public interface IReferenceController<TReference, TClassifications>
    {
        Task<int> Count([FromQuery(Name = "")] TClassifications classifications, [FromQuery] bool? recentImagesOnly);
        Task<List<TReference>> Search([FromQuery(Name = "")] TClassifications criteria, [FromQuery(Name = "")]OffsetLimit offsetLimit);
        Task<TReference> GetNext([FromQuery(Name = "")] TClassifications criteria, [FromBody] List<string> excludeIds, [FromQuery] bool? onlyRecentImages = null);
        Task<List<TReference>> Save([FromBody] List<TReference> images);
    }
}