﻿using Microsoft.AspNetCore.Http;
using SkdRefSiteAPI.DAO.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SkdRefSiteAPI.DAO
{
    public class FileDAO
    {
        private readonly string WEB_ROOT_PATH;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="webRootPath"></param>
        public FileDAO(string webRootPath)
        {
            WEB_ROOT_PATH = webRootPath;
        }

        public void Delete(string file)
        {
            File.Delete(file);
        }

        public List<Image> Upload(IFormFileCollection files, Batch batch)
        {
            var images = new List<Image>();

            var batchId = CreateOrUpdateBatch(batch);

            foreach (var file in files)
            {
                // Validate the file is a jpg/png

                // Validate the dimensions of the image - maybe reject if the file is too big. 

                // save the file somewhere accessible
                var uniqueId = Guid.NewGuid().ToString();
                var timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                if (fileName.Length > 50)
                    fileName = fileName.Substring(0, 75);
                fileName = fileName.Replace("_", ""); // replacing these characters so we can parse out the original file name (or close to it) easily if needed.
                var extension = Path.GetExtension(file.FileName);

                var newFileName = $"{batchId}-{fileName}_{timestamp}-{uniqueId}{extension}";
                var imagePath = Path.Combine(WEB_ROOT_PATH, "images");
                var fileLocation = Path.Combine(imagePath, newFileName);
                var stream = new FileStream(fileLocation, FileMode.CreateNew);
                file.CopyTo(stream);
                stream.Close();

                // create the Image record
                var image = new Image();
                image.File = $"http://localhost:15285/images/{newFileName}"; // TO DO - switch to / or the url for when we actually release it
                image.Model = new Contact();
                image.Photographer = new Contact();
                image.Source = UploadType.USER;
                image.Status = Status.Pending;
                image.TermsOfUse = "";
                image.UploadDate = DateTime.Now;
                image.BatchId = batchId;
                image.Location = fileLocation;
                //save

                images.Add(image);
            }

            return images;
        }

        private string CreateOrUpdateBatch(Batch batch)
        {
            var batchDAO = new BatchDAO();
            batch.Id = batchDAO.GetExistingBatchId(batch.User, batch.Name);
            if(string.IsNullOrEmpty(batch.Id))
            {
                batch.Id = Guid.NewGuid().ToString();
                batch.CreationDate = DateTime.Now;
                batchDAO.Save(batch);
            }            

            return batch.Id;
        }
    }
}
