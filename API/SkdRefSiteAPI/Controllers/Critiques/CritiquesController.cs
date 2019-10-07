using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkdAPI.ArtCrit.DAO;
using SkdAPI.ArtCrit.DAO.Models;
using SkdAPI.Common.Models;
using SkdAPI.RefSite.DAO;
using SkdAPI.RefSite.DAO.Models;
using SkdAPI.RefSite.DAO.Models.Animals;
using SkdAPI.RefSite.DAO.Queryables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdAPI.Controllers.Critiques
{
    /// <summary>
    /// API for working with critiques
    /// </summary>
    public class CritiquesController
    {
        private CritiqueDAO _dao;

        /// <summary>
        /// Constructor
        /// </summary>
        public CritiquesController()
        {
            _dao = new CritiqueDAO();
        }

        ///// <summary>
        ///// Get critiques
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("api/ArtCrit/Critiques")]
        //public List<News> Get([FromQuery(Name = "")]OffsetLimit offsetLimit)
        //{
        //    return _dao.Get(offsetLimit);
        //}

        /// <summary>
        /// Save critique request
        /// </summary>
        /// <param name="critiqueRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/ArtCrit/CritiqueRequests")]
        public bool Save([FromBody]CritiqueRequest critiqueRequest)
        {
            return _dao.SaveRequest(critiqueRequest);
        }
    }
}
