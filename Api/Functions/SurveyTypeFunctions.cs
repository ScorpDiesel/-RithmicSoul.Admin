using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RithmicSoul.Models.Survey;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoulAdminFunctionApp.Functions
{
    public class SurveyTypeFunctions
    {
        private readonly IService<SurveyType> _surveyTypeService;
        private readonly ILogger _logger;

        public SurveyTypeFunctions(ILoggerFactory loggerFactory, IService<SurveyType> surveyTypeService)
        {
            _surveyTypeService = surveyTypeService;
            _logger = loggerFactory.CreateLogger<SurveyTypeFunctions>();
        }

        [Function("SurveyTypeGetAll")]
        public async Task<HttpResponseData> GetAllAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SurveyType/GetAll")] HttpRequestData req)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(SurveyType)}:GetAllAsync processed a request.");

            var result = await _surveyTypeService.GetAllAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("SurveyTypeGetById")]
        public async Task<HttpResponseData> GetByIdAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SurveyType/GetById/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(SurveyType)}:GetByIdAsync processed a request.");

            var result = await _surveyTypeService.GetByIdAsync(id);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("SurveyTypeInsert")]
        public async Task<HttpResponseData> InsertAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SurveyType/Insert")] HttpRequestData req, SurveyType surveyType)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(surveyType)}:InsertAsync processed a request.");

            await _surveyTypeService.InsertAsync(surveyType);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        [Function("SurveyTypeUpdate")]
        public async Task<HttpResponseData> UpdateAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SurveyType/Update")] HttpRequestData req, SurveyType surveyType)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(surveyType)}:UpdateAsync processed a request.");

            await _surveyTypeService.UpdateAsync(surveyType);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        [Function("SurveyTypeDelete")]
        public async Task<HttpResponseData> DeleteAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SurveyType/Delete")] HttpRequestData req, SurveyType surveyType)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(surveyType)}:DeleteAsync processed a request.");

            await _surveyTypeService.DeleteAsync(surveyType);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        [Function("SurveyTypeDeleteById")]
        public async Task<HttpResponseData> DeleteByIdAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SurveyType/DeleteById/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(SurveyType)}:DeleteAsync processed a request.");

            await _surveyTypeService.DeleteAsync(id);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }
    }
}
