using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SkdAPI.RefSite.DAO.Models;
using SkdAPI.RefSite.DAO.Models.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkdAPI.RefSite.DAO.Queryables
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
                var mostRecentUpload = GetMostRecentImageUploadDate(collection);
                query = query.Where(x => x.UploadDate >= mostRecentUpload.AddDays(-30));
            }
            if (classifications.Status.HasValue)
                query = query.Where(x => x.Status == classifications.Status);

            return query;
        }

        private DateTime GetMostRecentImageUploadDate(IMongoCollection<StructureReference> collection)
        {
            var query = collection.AsQueryable().OrderByDescending(x => x.UploadDate).Take(1);
            var item = query.First();
            return item.UploadDate;
        }
    }
}
