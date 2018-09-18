using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SkdRefSiteAPI.DAO.Models;
using SkdRefSiteAPI.DAO.Models.Animals;
using SkdRefSiteAPI.DAO.Models.People;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SkdRefSiteAPI.DAO.Queryables
{
    public class AnimalsQueryable : IQueryable<AnimalReference, AnimalClassifications>
    {
        public IMongoQueryable<AnimalReference> GetQueryable(IMongoCollection<AnimalReference> collection, AnimalClassifications classifications, bool? recentImagesOnly)
        {
            var query = collection.AsQueryable();
            if (classifications.Category.HasValue)
                query = query.Where(x => x.Classifications.Category == classifications.Category);
            if (classifications.Species.HasValue)
                query = query.Where(x => x.Classifications.Species == classifications.Species);
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
