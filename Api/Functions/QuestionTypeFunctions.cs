using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RithmicSoul.Models.Survey;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoulAdminFunctionApp.Functions
{
    public class QuestionTypeFunctions
    {
        private readonly IService<QuestionType> _questionTypeService;
        private readonly ILogger _logger;

        public QuestionTypeFunctions(ILoggerFactory loggerFactory, IService<QuestionType> questionTypeService)
        {
            _questionTypeService = questionTypeService;
            _logger = loggerFactory.CreateLogger<QuestionTypeFunctions>();
        }

        [Function("QuestionTypeGetAll")]
        public async Task<HttpResponseData> GetAllAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "QuestionType/GetAll")] HttpRequestData req)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(QuestionType)}:GetAllAsync processed a request.");

            var result = await _questionTypeService.GetAllAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("QuestionTypeGetById")]
        public async Task<HttpResponseData> GetByIdAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "QuestionType/GetById/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(QuestionType)}:GetByIdAsync processed a request.");

            var result = await _questionTypeService.GetByIdAsync(id);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("QuestionTypeInsert")]
        public async Task<HttpResponseData> InsertAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "QuestionType/Insert")] HttpRequestData req, QuestionType questionType)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(QuestionType)}:InsertAsync processed a request.");

            await _questionTypeService.InsertAsync(questionType);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        [Function("QuestionTypeUpdate")]
        public async Task<HttpResponseData> UpdateAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "QuestionType/Update")] HttpRequestData req, QuestionType questionType)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(QuestionType)}:UpdateAsync processed a request.");

            await _questionTypeService.UpdateAsync(questionType);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        [Function("QuestionTypeDelete")]
        public async Task<HttpResponseData> DeleteAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "QuestionType/Delete")] HttpRequestData req, QuestionType questionType)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(QuestionType)}:DeleteAsync processed a request.");

            await _questionTypeService.DeleteAsync(questionType);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        [Function("QuestionTypeDeleteById")]
        public async Task<HttpResponseData> DeleteByIdAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "QuestionType/DeleteById/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"HTTP trigger function {nameof(QuestionType)}:DeleteAsync processed a request.");

            await _questionTypeService.DeleteAsync(id);

            var response = req.CreateResponse(HttpStatusCode.Created);

            return response;
        }
    }
}
