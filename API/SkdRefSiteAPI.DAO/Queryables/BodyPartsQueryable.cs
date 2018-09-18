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
    public class BodyPartsQueryable : IQueryable<BodyPartReference, BodyPartClassifications>
    {
        public IMongoQueryable<BodyPartReference> GetQueryable(IMongoCollection<BodyPartReference> collection, BodyPartClassifications classifications, bool? recentImagesOnly)
        {
            var query = collection.AsQueryable();
            if (classifications.Gender.HasValue)
                query = query.Where(x => x.Classifications.Gender == classifications.Gender);
            if (classifications.BodyPart.HasValue)
                query = query.Where(x => x.Classifications.BodyPart == classifications.BodyPart);
            if (classifications.ViewAngle.HasValue)
                query = query.Where(x => x.Classifications.ViewAngle == classifications.ViewAngle);
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
