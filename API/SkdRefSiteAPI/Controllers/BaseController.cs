using Microsoft.AspNetCore.Mvc;
using SkdRefSiteAPI.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdRefSiteAPI.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Gets the current user
        /// </summary>
        /// <returns></returns>
        protected User GetCurrentUser()
        {
            var user = new User();

            user.Name = HttpContext.User.Claims.First(c => c.Type == "nickname" && c.Issuer == @"https://sketchdaily.auth0.com/").Value;
            user.Email = HttpContext.User.Claims.First(c => c.Type == "name" && c.Issuer == @"https://sketchdaily.auth0.com/").Value;

            var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == @"https://reference.sketchdaily.net/roles" && c.Issuer == @"https://sketchdaily.auth0.com/").Value;
            user.IsAdmin = role == "admin";

            return user;
        }
    }
}
