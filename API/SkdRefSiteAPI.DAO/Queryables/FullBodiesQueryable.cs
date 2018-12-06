using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SkdRefSiteAPI.DAO.Models;
using SkdRefSiteAPI.DAO.Models.People;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SkdRefSiteAPI.DAO.Queryables
{
    public class FullBodiesQueryable : IQueryable<FullBodyReference, FullBodyClassifications>
    {
        public IMongoQueryable<FullBodyReference> GetQueryable(IMongoCollection<FullBodyReference> collection, FullBodyClassifications classifications, bool? recentImagesOnly)
        {
            var query = collection.AsQueryable();
            if (classifications.Clothing.HasValue)
                query = query.Where(x => x.Classifications.Clothing.Value == classifications.Clothing);
            if (classifications.Gender.HasValue)
                query = query.Where(x => x.Classifications.Gender == classifications.Gender);
            if (classifications.Clothing.HasValue && classifications.Clothing.Value && (classifications.NSFW.HasValue == false || classifications.NSFW == false))
                query = query.Where(x => x.Classifications.NSFW.Value == false);
            if (classifications.PoseType.HasValue)
                query = query.Where(x => x.Classifications.PoseType == classifications.PoseType);
            if (classifications.ViewAngle.HasValue)
                query = query.Where(x => x.Classifications.ViewAngle == classifications.ViewAngle);
            if (recentImagesOnly == true)
            {
                var mostRecentUpload = GetMostRecentImageUploadDate(collection);
                query = query.Where(x => x.UploadDate >= mostRecentUpload.AddDays(-30));
            }
            if (classifications.Status.HasValue)
                query = query.Where(x => x.Status == classifications.Status);

            return query;
        }

        private DateTime GetMostRecentImageUploadDate(IMongoCollection<FullBodyReference> collection)
        {
            var query = collection.AsQueryable().OrderByDescending(x => x.UploadDate).Take(1);
            var item = query.First();
            return item.UploadDate;
        }
    }
}
