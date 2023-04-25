using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RithmicSoul.Models.Survey;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoulAdminFunctionApp.Functions
{
    public class SurveyQuestionnaireFunctions
    {
        private readonly IService<SurveyQuestionnaire> _surveyQuestionnaireService;
        private readonly ILogger _logger;

        public SurveyQuestionnaireFunctions(ILoggerFactory loggerFactory, IService<SurveyQuestionnaire> surveyQuestionnaireService)
        {
            _surveyQuestionnaireService = surveyQuestionnaireService;
            _logger = loggerFactory.CreateLogger<SurveyQuestionnaireFunctions>();
        }

        [Function("SurveyQuestionnaireGetAll")]
        public async Task<HttpResponseData> GetAllAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SurveyQuestionnaire/GetAll")] HttpRequestData req)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(SurveyQuestionnaire)}:GetAllAsync processed a request.");

            var result = await _surveyQuestionnaireService.GetAllAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Questionnaire", "application/json; charset=utf-8");

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("SurveyQuestionnaireGetById")]
        public async Task<HttpResponseData> GetByIdAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SurveyQuestionnaire/GetById/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(SurveyQuestionnaire)}:GetByIdAsync processed a request.");

            var result = await _surveyQuestionnaireService.GetByIdAsync(id);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Questionnaire", "application/json; charset=utf-8");

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("SurveyQuestionnaireInsert")]
        public async Task<HttpResponseData> InsertAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SurveyQuestionnaire/Insert")] HttpRequestData req, SurveyQuestionnaire surveyQuestionnaire)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(surveyQuestionnaire)}:InsertAsync processed a request.");

            await _surveyQuestionnaireService.InsertAsync(surveyQuestionnaire);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        [Function("SurveyQuestionnaireUpdate")]
        public async Task<HttpResponseData> UpdateAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SurveyQuestionnaire/Update")] HttpRequestData req, SurveyQuestionnaire surveyQuestionnaire)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(surveyQuestionnaire)}:UpdateAsync processed a request.");

            await _surveyQuestionnaireService.UpdateAsync(surveyQuestionnaire);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        [Function("SurveyQuestionnaireDelete")]
        public async Task<HttpResponseData> DeleteAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SurveyQuestionnaire/Delete")] HttpRequestData req, SurveyQuestionnaire surveyQuestionnaire)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(surveyQuestionnaire)}:DeleteAsync processed a request.");

            await _surveyQuestionnaireService.DeleteAsync(surveyQuestionnaire);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        [Function("SurveyQuestionnaireDeleteById")]
        public async Task<HttpResponseData> DeleteByIdAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SurveyQuestionnaire/DeleteById/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(SurveyQuestionnaire)}:DeleteAsync processed a request.");

            await _surveyQuestionnaireService.DeleteAsync(id);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }
    }
}
