using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RithmicSoul.Models.Survey;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoulAdminFunctionApp.Functions
{
    public class SurveyFunctions
    {
        private readonly IService<Survey> _surveyService;
        private readonly ILogger _logger;

        public SurveyFunctions(ILoggerFactory loggerFactory, IService<Survey> surveyService)
        {
            _surveyService = surveyService;
            _logger = loggerFactory.CreateLogger<SurveyFunctions>();
        }

        [Function("SurveyGetAll")]
        public async Task<HttpResponseData> GetAllAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Survey/GetAll")] HttpRequestData req)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(Survey)}:GetAllAsync processed a request.");

            var result = await _surveyService.GetAllAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-", "application/json; charset=utf-8");

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("SurveyGetById")]
        public async Task<HttpResponseData> GetByIdAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Survey/GetById/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(Survey)}:GetByIdAsync processed a request.");

            var result = await _surveyService.GetByIdAsync(id);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-", "application/json; charset=utf-8");

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("SurveyInsert")]
        public async Task<HttpResponseData> InsertAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Survey/Insert")] HttpRequestData req, Survey survey)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(survey)}:InsertAsync processed a request.");

            await _surveyService.InsertAsync(survey);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        [Function("SurveyUpdate")]
        public async Task<HttpResponseData> UpdateAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Survey/Update")] HttpRequestData req, Survey survey)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(survey)}:UpdateAsync processed a request.");

            await _surveyService.UpdateAsync(survey);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        [Function("SurveyDelete")]
        public async Task<HttpResponseData> DeleteAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Survey/Delete")] HttpRequestData req, Survey survey)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(survey)}:DeleteAsync processed a request.");

            await _surveyService.DeleteAsync(survey);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        [Function("SurveyDeleteById")]
        public async Task<HttpResponseData> DeleteByIdAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Survey/DeleteById/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(Survey)}:DeleteAsync processed a request.");

            await _surveyService.DeleteAsync(id);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }
    }
}
