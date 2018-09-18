using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkdRefSiteAPI.DAO.Queryables
{
    public interface IQueryable<TReference, TClassifications>
    {
        IMongoQueryable<TReference> GetQueryable(IMongoCollection<TReference> collection, TClassifications classifications, bool? recentImagesOnly);
    }
}
