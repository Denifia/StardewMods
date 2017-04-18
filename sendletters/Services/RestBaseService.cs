using Denifia.Stardew.SendLetters.Domain;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendLetters.Services
{
    public class RestService : IRestService
    {

        private RestClient RestClient { get; set; }
        internal IConfigurationService _configService;

        public RestService(IConfigurationService configService)
        {
            _configService = configService;
            RestClient = new RestClient(_configService.GetApiUri());
        }

        public void PutRequest<T>(string resource, Dictionary<string, string> urlSegments, object jsonBody, Action<T> callback, T obj)
        {
            var request = FormStandardRequest(resource, urlSegments, Method.PUT);
            request.AddJsonBody(jsonBody);

            RestClient.ExecuteAsync(request, response => {
                callback(obj);
            });
        }

        public void PostRequest(string resource, Dictionary<string, string> urlSegments, object jsonBody, Action callback)
        {
            var request = FormStandardRequest(resource, urlSegments, Method.POST);
            request.AddJsonBody(jsonBody);

            RestClient.ExecuteAsync(request, response => {
                callback();
            });
        }

        public void DeleteRequest(string resource, Dictionary<string, string> urlSegments)
        {
            var request = FormStandardRequest(resource, urlSegments, Method.DELETE);
            RestClient.ExecuteAsync(request, response => {
                var x = response;
            });
        }

        public void GetRequest<T>(string resource, Dictionary<string, string> urlSegments, Action<T> callback)
            where T : new()
        {
            var request = FormStandardRequest(resource, urlSegments, Method.GET);
            RestClient.ExecuteAsync<T>(request, response => {
                callback(response.Data);
            });
        }

        public RestRequest FormStandardRequest(string resource, Dictionary<string, string> urlSegments, Method method)
        {
            var request = new RestRequest(resource, method);
            request.AddHeader("Content-type", "application/json; charset=utf-8");
            foreach (var urlSegment in urlSegments)
            {
                request.AddUrlSegment(urlSegment.Key, urlSegment.Value);
            }

            return request;
        }
    }
}
