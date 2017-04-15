using denifia.stardew.sendletters.Domain;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Services
{
    public class RestBaseService
    {
        internal Repository Repo;// = Repository.Instance;
        private RestClient RestClient { get; set; }

        public RestBaseService(Uri api)
        {
            RestClient = new RestClient(api);
        }

        internal void PutRequest<T>(string resource, Dictionary<string, string> urlSegments, object jsonBody, Action<T> callback, T obj)
        {
            var request = FormStandardRequest(resource, urlSegments, Method.PUT);
            request.AddJsonBody(jsonBody);

            RestClient.ExecuteAsync(request, response => {
                callback(obj);
            });
        }

        internal void PostRequest(string resource, Dictionary<string, string> urlSegments, object jsonBody, Action callback)
        {
            var request = FormStandardRequest(resource, urlSegments, Method.POST);
            request.AddJsonBody(jsonBody);

            RestClient.ExecuteAsync(request, response => {
                callback();
            });
        }

        internal void DeleteRequest(string resource, Dictionary<string, string> urlSegments)
        {
            var request = FormStandardRequest(resource, urlSegments, Method.DELETE);
            RestClient.ExecuteAsync(request, response => {
                var x = response;
            });
        }

        internal void GetRequest<T>(string resource, Dictionary<string, string> urlSegments, Action<T> callback)
            where T : new()
        {
            var request = FormStandardRequest(resource, urlSegments, Method.GET);
            RestClient.ExecuteAsync<T>(request, response => {
                callback(response.Data);
            });
        }

        internal RestRequest FormStandardRequest(string resource, Dictionary<string, string> urlSegments, Method method)
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
