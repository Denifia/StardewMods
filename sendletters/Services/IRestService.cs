using RestSharp;
using System;
using System.Collections.Generic;

namespace denifia.stardew.sendletters.Services
{
    public interface IRestService
    {
        void PutRequest<T>(string resource, Dictionary<string, string> urlSegments, object jsonBody, Action<T> callback, T obj);

        void PostRequest(string resource, Dictionary<string, string> urlSegments, object jsonBody, Action callback);

        void DeleteRequest(string resource, Dictionary<string, string> urlSegments);

        void GetRequest<T>(string resource, Dictionary<string, string> urlSegments, Action<T> callback) where T : new();

        RestRequest FormStandardRequest(string resource, Dictionary<string, string> urlSegments, Method method);
    }
}