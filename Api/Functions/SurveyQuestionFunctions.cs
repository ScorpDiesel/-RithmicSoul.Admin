using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RithmicSoul.Models.Survey;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoulAdminFunctionApp.Functions
{
    public class SurveyQuestionFunctions
    {
        private readonly IService<SurveyQuestion> _surveyQuestionService;
        private readonly ILogger _logger;

        public SurveyQuestionFunctions(ILoggerFactory loggerFactory, IService<SurveyQuestion> surveyQuestionService)
        {
            _surveyQuestionService = surveyQuestionService;
            _logger = loggerFactory.CreateLogger<SurveyQuestionFunctions>();
        }

        [Function("SurveyQuestionGetAll")]
        public async Task<HttpResponseData> GetAllAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SurveyQuestion/GetAll")] HttpRequestData req)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(SurveyQuestion)}:GetAllAsync processed a request.");

            var result = await _surveyQuestionService.GetAllAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Question", "application/json; charset=utf-8");

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("SurveyQuestionGetById")]
        public async Task<HttpResponseData> GetByIdAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SurveyQuestion/GetById/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(SurveyQuestion)}:GetByIdAsync processed a request.");

            var result = await _surveyQuestionService.GetByIdAsync(id);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Question", "application/json; charset=utf-8");

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("SurveyQuestionInsert")]
        public async Task<HttpResponseData> InsertAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SurveyQuestion/Insert")] HttpRequestData req, SurveyQuestion surveyQuestion)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(surveyQuestion)}:InsertAsync processed a request.");

            await _surveyQuestionService.InsertAsync(surveyQuestion);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        [Function("SurveyQuestionUpdate")]
        public async Task<HttpResponseData> UpdateAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SurveyQuestion/Update")] HttpRequestData req, SurveyQuestion surveyQuestion)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(surveyQuestion)}:UpdateAsync processed a request.");

            await _surveyQuestionService.UpdateAsync(surveyQuestion);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        [Function("SurveyQuestionDelete")]
        public async Task<HttpResponseData> DeleteAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SurveyQuestion/Delete")] HttpRequestData req, SurveyQuestion surveyQuestion)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(surveyQuestion)}:DeleteAsync processed a request.");

            await _surveyQuestionService.DeleteAsync(surveyQuestion);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        [Function("SurveyQuestionDeleteById")]
        public async Task<HttpResponseData> DeleteByIdAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SurveyQuestion/DeleteById/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(SurveyQuestion)}:DeleteAsync processed a request.");

            await _surveyQuestionService.DeleteAsync(id);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }
    }
}
