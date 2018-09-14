using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SkdRefSiteAPI.DAO;
using SkdRefSiteAPI.DAO.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SkdRefSiteAPI.Controllers
{
    /// <summary>
    /// API for working with images
    /// </summary>
    public class ImageController : BaseController
    {
        private FileDAO _fileDAO;
        private BatchDAO _batchDAO;
        private Logger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        public ImageController(IHostingEnvironment hostingEnvironment)
        {
            _fileDAO = new FileDAO(hostingEnvironment.WebRootPath);
            _batchDAO = new BatchDAO();
            _logger = new Logger("ImageController");
        }

        /// <summary>
        /// Upload image
        /// </summary>
        /// <returns>Batch id</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Image")]
        public List<Image> UploadImage([FromForm]string batch)
        {
            try
            {
                var converter = new StringEnumConverter();
                Batch deserializedBatch = JsonConvert.DeserializeObject<Batch>(batch, converter);
                deserializedBatch.User = GetCurrentUser().Email;
                var files = Request.Form.Files;
                var images = _fileDAO.Upload(files, deserializedBatch);
                return images;
            }
            catch(Exception ex)
            {
                _logger.Log("UploadImage", ex, batch);
                throw;
            }
        }

        /// <summary>
        /// Gets batches
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Batches")]
        public async Task<List<Batch>> GetBatches()
        {
            var user = GetCurrentUser().Email;
            return await _batchDAO.GetUserBatches(user);
        }

        /// <summary>
        /// Gets images from batch
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Batches/{id}/Images")]
        public async Task<BatchImages> GetBatchImages(string id)
        {
            var results = new BatchImages();

            var currentUser = GetCurrentUser();
            results.Batch = await _batchDAO.Get(id);

            if (results.Batch == null)
                return results;

            if (!currentUser.IsAdmin && currentUser.Email != results.Batch.User)
                throw new Exception("Unauthorized");

            if(results.Batch.Type == ReferenceType.Animal)
            {
                var animalsDAO = new AnimalsDAO();
                var images = await animalsDAO.Search(id);
                results.Images = images.ToList<object>();
            }
            else if (results.Batch.Type == ReferenceType.BodyPart)
            {
                var bodyPartsDAO = new BodyPartsDAO();
                var images = await bodyPartsDAO.Search(id);
                results.Images = images.ToList<object>();
            }
            else if (results.Batch.Type == ReferenceType.FullBody)
            {
                var fullBodiesDAO = new FullBodiesDAO();
                var images = await fullBodiesDAO.Search(id);
                results.Images = images.ToList<object>();
            }

            return results;
        }
    }
}
