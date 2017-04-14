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
        internal Repository Repo = Repository.Instance;
        private RestClient RestClient { get; set; }

        public RestBaseService(Uri api)
        {
            RestClient = new RestClient(api);
        }

        internal void PutRequest(string resource, Dictionary<string, string> urlSegments, object jsonBody, Action callback)
        {
            var request = new RestRequest(resource, Method.PUT);
            request.AddHeader("Content-type", "application/json; charset=utf-8");
            foreach (var urlSegment in urlSegments)
            {
                request.AddUrlSegment(urlSegment.Key, urlSegment.Value);
            }
            request.AddJsonBody(jsonBody);

            RestClient.ExecuteAsync(request, response => {
                callback();
            });
        }

        internal void GetRequest<T>(string resource, Dictionary<string, string> urlSegments, Action<T> callback)
            where T : new()
        {
            var request = new RestRequest(resource, Method.GET);
            request.AddHeader("Content-type", "application/json; charset=utf-8");
            foreach (var urlSegment in urlSegments)
            {
                request.AddUrlSegment(urlSegment.Key, urlSegment.Value);
            }

            RestClient.ExecuteAsync<T>(request, response => {
                callback(response.Data);
            });
        }

    }
}
