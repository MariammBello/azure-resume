using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text;
using System.Net.Http;

namespace Company.Function
{
    public static class GetResumeCounter
    {
        [FunctionName("GetResumeCounter")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Get MongoDB connection details from environment variables
            var mongoConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
            var mongoDatabaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME");

            // Create a MongoClient
            var client = new MongoClient(mongoConnectionString);
            var database = client.GetDatabase(mongoDatabaseName);
            var collection = database.GetCollection<Counter>("Counter");

            // Fetch the current counter (assuming a single document with a known ID)
            var filter = Builders<Counter>.Filter.Eq("_id", "1");
            var counter = await collection.Find(filter).FirstOrDefaultAsync();

            if (counter == null)
            {
                counter = new Counter { Id = "1", Count = 1 };
                await collection.InsertOneAsync(counter);
            }
            else
            {
                counter.Count += 1;
                await collection.ReplaceOneAsync(filter, counter);
            }

            var jsonToReturn = JsonConvert.SerializeObject(counter);
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
            };
        }
    }

    public class Counter
    {
        public string Id { get; set; }
        public int Count { get; set; }
    }
}
