﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SkdRefSiteAPI.DAO;
using SkdRefSiteAPI.DAO.Models;
using SkdRefSiteAPI.DAO.Models.Animals;
using SkdRefSiteAPI.DAO.Models.People;
using SkdRefSiteAPI.DAO.Models.Structures;
using SkdRefSiteAPI.DAO.Models.Vegetation;
using SkdRefSiteAPI.DAO.Queryables;
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
        public ImageSaveResults UploadImage([FromForm]string batch)
        {
            var results = new ImageSaveResults();
            Batch deserializedBatch = null;
            try
            {
                var converter = new StringEnumConverter();
                deserializedBatch = JsonConvert.DeserializeObject<Batch>(batch, converter);
                deserializedBatch.User = GetCurrentUser().Email;
                var files = Request.Form.Files;
                var images = _fileDAO.Upload(files, ref deserializedBatch, GetCurrentUser());
                results.Images = images;
                results.BatchId = deserializedBatch.Id;
                results.Success = true;
            }
            catch(Exception ex)
            {
                if(batch != null)
                    results.BatchId = deserializedBatch.Id;
                results.Success = false;
                _logger.Log("UploadImage", ex, batch);
            }
            return results;
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
        /// Report image
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Images/{id}/Report")]
        public bool ReportImage(string id, [FromBody] Report report)
        {
            report.ImageId = id;
            report.User = GetCurrentUser();
            report.Date = DateTime.Now;
            _logger.Log("Image Report", $"User has reported image", LogType.Report, report);
            return true;
        }

        [Authorize]
        [HttpDelete]
        [Route("api/Batches/{id}")]
        public async Task<bool> DeleteBatch(string id)
        {
            var user = GetCurrentUser();
            await _batchDAO.DeleteReference(id, user);
            return true;
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
                var queryable = new AnimalsQueryable();
                var animalsDAO = new ReferenceDAO<AnimalReference, AnimalClassifications>(ReferenceType.Animal, queryable);
                var images = await animalsDAO.Search(new AnimalClassifications() { BatchId = id });
                results.Images = images.ToList<object>();
            }
            else if (results.Batch.Type == ReferenceType.BodyPart)
            {
                var queryable = new BodyPartsQueryable();
                var bodyPartsDAO = new ReferenceDAO<BodyPartReference, BodyPartClassifications>(ReferenceType.BodyPart, queryable);
                var images = await bodyPartsDAO.Search(new BodyPartClassifications { BatchId = id });
                results.Images = images.ToList<object>();
            }
            else if (results.Batch.Type == ReferenceType.FullBody)
            {
                var queryable = new FullBodiesQueryable();
                var fullBodiesDAO = new ReferenceDAO<FullBodyReference, FullBodyClassifications>(ReferenceType.FullBody, queryable);
                var images = await fullBodiesDAO.Search(new FullBodyClassifications { BatchId = id });
                results.Images = images.ToList<object>();
            }
            else if (results.Batch.Type == ReferenceType.Vegetation)
            {
                var queryable = new VegetationQueryable();
                var vegetationDAO = new ReferenceDAO<VegetationReference, VegetationClassifications>(ReferenceType.Vegetation, queryable);
                var images = await vegetationDAO.Search(new VegetationClassifications { BatchId = id });
                results.Images = images.ToList<object>();
            }
            else if (results.Batch.Type == ReferenceType.Structure)
            {
                var queryable = new StructuresQueryable();
                var structuresDAO = new ReferenceDAO<StructureReference, StructureClassifications>(ReferenceType.Structure, queryable);
                var images = await structuresDAO.Search(new StructureClassifications { BatchId = id });
                results.Images = images.ToList<object>();
            }

            return results;
        }
    }
}
