using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SkdRefSiteAPI.DAO.Models;
using SkdRefSiteAPI.DAO.Models.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkdRefSiteAPI.DAO.Queryables
{
    public class StructuresQueryable : IQueryable<StructureReference, StructureClassifications>
    {
        public IMongoQueryable<StructureReference> GetQueryable(IMongoCollection<StructureReference> collection, StructureClassifications classifications, bool? recentImagesOnly)
        {
            var query = collection.AsQueryable();
            if (classifications.StructureType.HasValue)
                query = query.Where(x => x.Classifications.StructureType == classifications.StructureType);
            if (recentImagesOnly == true)
            {
                query = query.OrderByDescending(x => x.UploadDate);
                query = query.Take(50);
            }
            if (classifications.Status.HasValue)
                query = query.Where(x => x.Status == classifications.Status);

            return query;
        }
    }
}
