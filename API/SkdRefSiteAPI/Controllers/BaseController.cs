using Microsoft.AspNetCore.Mvc;
using SkdAPI.RefSite.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkdAPI.Controllers
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

            try
            {
                user.Name = HttpContext.User.Claims.First(c => c.Type == "nickname" && c.Issuer == @"https://sketchdaily.auth0.com/").Value;
                user.Email = HttpContext.User.Claims.First(c => c.Type == @"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress" && c.Issuer == @"https://sketchdaily.auth0.com/").Value;

                var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == @"https://reference.sketchdaily.net/roles" && c.Issuer == @"https://sketchdaily.auth0.com/").Value;
                user.IsAdmin = role == "admin";
            }
            catch(Exception)
            {
                user.IsAdmin = false;
                user.Name = "Unknown";
                user.Email = "Unknown";
            }
            return user;
        }
    }
}
