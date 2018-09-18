using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkdRefSiteAPI.DAO.Models.Animals;

namespace SkdRefSiteAPI.Controllers
{
    public interface IReferenceController<TReference, TClassifications>
    {
        Task<int> Count([FromQuery(Name = "")] TClassifications classifications, [FromQuery] bool? recentImagesOnly);
        Task<TReference> Get([FromQuery(Name = "")] TClassifications criteria, [FromQuery] bool? recentImagesOnly = null);
        Task<TReference> GetNext([FromQuery(Name = "")] TClassifications criteria, [FromBody] List<string> excludeIds, [FromQuery] bool? onlyRecentImages = null);
        Task<List<TReference>> Save([FromBody] List<TReference> images);
    }
}