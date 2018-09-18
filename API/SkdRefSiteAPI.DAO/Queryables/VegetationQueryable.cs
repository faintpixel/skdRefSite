using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SkdRefSiteAPI.DAO.Models;
using SkdRefSiteAPI.DAO.Models.Vegetation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkdRefSiteAPI.DAO.Queryables
{
    public class VegetationQueryable : IQueryable<VegetationReference, VegetationClassifications>
    {
        public IMongoQueryable<VegetationReference> GetQueryable(IMongoCollection<VegetationReference> collection, VegetationClassifications classifications, bool? recentImagesOnly)
        {
            var query = collection.AsQueryable();
            if (classifications.PhotoType.HasValue)
                query = query.Where(x => x.Classifications.PhotoType == classifications.PhotoType);
            if (classifications.VegetationType.HasValue)
                query = query.Where(x => x.Classifications.VegetationType == classifications.VegetationType);
            if (recentImagesOnly == true)
            {
                query = query.OrderByDescending(x => x.UploadDate);
                query = query.Take(50);
            }

            query = query.Where(x => x.Status == Status.Active);

            return query;
        }
    }
}
