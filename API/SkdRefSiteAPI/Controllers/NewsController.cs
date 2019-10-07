using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkdAPI.Common.Models;
using SkdAPI.RefSite.DAO;
using SkdAPI.RefSite.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdAPI.Controllers
{
    /// <summary>
    /// API for working with news and announcements
    /// </summary>
    public class NewsController: Controller
    {
        private NewsDAO _dao;

        /// <summary>
        /// Constructor
        /// </summary>
        public NewsController()
        {
            _dao = new NewsDAO();
        }

        /// <summary>
        /// Get news
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/News")]
        public List<News> Get([FromQuery(Name = "")]OffsetLimit offsetLimit)
        {
            return _dao.Get(offsetLimit);
        }

        /// <summary>
        /// Save news
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("admin")]
        [Route("api/News")]
        public bool Save([FromBody]News news)
        {
            return _dao.Save(news);
        }

        /// <summary>
        /// Saves announcement
        /// </summary>
        /// <param name="announcement"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("admin")]
        [Route("api/Announcement")]
        public bool SaveAnnouncement([FromBody]Announcement announcement)
        {
            return _dao.SaveAnnouncement(announcement);
        }

        /// <summary>
        /// Gets announcement
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Announcement")]
        public Announcement GetAnnouncement()
        {
            return _dao.GetAnnouncement();
        }
    }
}
