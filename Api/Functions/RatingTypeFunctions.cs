using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RithmicSoul.Models.Survey;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoulAdminFunctionApp.Functions
{
    public class RatingTypeFunctions
    {
        private readonly IService<RatingType> _ratingTypeService;
        private readonly ILogger _logger;

        public RatingTypeFunctions(ILoggerFactory loggerFactory, IService<RatingType> ratingTypeService)
        {
            _ratingTypeService = ratingTypeService;
            _logger = loggerFactory.CreateLogger<RatingTypeFunctions>();
        }

        [Function("RatingTypeGetAll")]
        public async Task<HttpResponseData> GetAllAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "RatingType/GetAll")] HttpRequestData req)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(RatingType)}:GetAllAsync processed a request.");

            var result = await _ratingTypeService.GetAllAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("RatingTypeGetById")]
        public async Task<HttpResponseData> GetByIdAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "RatingType/GetById/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(RatingType)}:GetByIdAsync processed a request.");

            var result = await _ratingTypeService.GetByIdAsync(id);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("RatingTypeInsert")]
        public async Task<HttpResponseData> InsertAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "RatingType/Insert")] HttpRequestData req, RatingType ratingType)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(ratingType)}:InsertAsync processed a request.");

            await _ratingTypeService.InsertAsync(ratingType);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        [Function("RatingTypeUpdate")]
        public async Task<HttpResponseData> UpdateAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "RatingType/Update")] HttpRequestData req, RatingType ratingType)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(ratingType)}:UpdateAsync processed a request.");

            await _ratingTypeService.UpdateAsync(ratingType);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        [Function("RatingTypeDelete")]
        public async Task<HttpResponseData> DeleteAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "RatingType/Delete")] HttpRequestData req, RatingType ratingType)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(ratingType)}:DeleteAsync processed a request.");

            await _ratingTypeService.DeleteAsync(ratingType);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        [Function("RatingTypeDeleteById")]
        public async Task<HttpResponseData> DeleteByIdAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "RatingType/DeleteById/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(RatingType)}:DeleteAsync processed a request.");

            await _ratingTypeService.DeleteAsync(id);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }
    }
}
