using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using SkdAPI.Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SkdAPI.Common
{
    public class Logger
    {
        private readonly string APPLICATION_NAME;
        private MongoClient _mongoClient;
        private IMongoDatabase _db;
        private IMongoCollection<Log> _collection;

        public Logger(string databaseName, string applicationName)
        {
            APPLICATION_NAME = applicationName;
            _mongoClient = new MongoClient(AppSettings.MongoDBConnection);
            _db = _mongoClient.GetDatabase(databaseName);
            _collection = _db.GetCollection<Log>("logs");
        }

        public void Log(string source, Exception ex, params object[] parameters)
        {
            var serializedParameters = GetSerializedParameters(parameters);
            PerformLog(source, "Error", serializedParameters, LogType.Error, ex).Wait();
        }

        public void Log(string source, string message, LogType type, params object[] parameters)
        {
            var serializedParameters = GetSerializedParameters(parameters);
            PerformLog(source, message, serializedParameters, type).Wait();
        }

        private async Task PerformLog(string source, string message, string parameters, LogType type, Exception exception = null)
        {
            try
            {
                var log = new Log();
                if (exception != null)
                    log.Exception = exception.ToString();
                log.Message = message;
                log.Parameters = parameters;
                log.Source = APPLICATION_NAME + " " + source;
                log.Time = DateTime.Now;
                log.Type = type;

                var replaceResult = await _collection.ReplaceOneAsync(
                            filter: new BsonDocument("_id", log.Id),
                            options: new UpdateOptions { IsUpsert = true },
                            replacement: log);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error logging: " + ex.ToString());
            }
        }

        public async Task<int> CountLogs()
        {
            var data = _collection.AsQueryable();
            var results = await data.CountAsync();
            return results;
        }

        public async Task<List<Log>> GetAllLogs()
        {
            var data = _collection.AsQueryable();
            var results = await data.ToListAsync();
            return results;
        }

        public bool DeleteLog(string id)
        {
            try
            {
                _collection.DeleteOne(filter: new BsonDocument("_id", id));
                return true;
            }
            catch (Exception ex)
            {
                Log("DeleteLog", ex, id);
                return false;
            }
        }

        private string GetSerializedParameters(params object[] parameters)
        {
            var serializedParameters = new List<string>();
            foreach (var parameter in parameters)
                serializedParameters.Add(JsonConvert.SerializeObject(parameter));
            var allParameters = String.Join("; ", serializedParameters);
            return allParameters;
        }
    }
}
