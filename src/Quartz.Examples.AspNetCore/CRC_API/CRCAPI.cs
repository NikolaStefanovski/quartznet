using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz.Examples.AspNetCore.CRC_API.Models;
using RestSharp;
using System;

namespace Quartz.Examples.AspNetCore.CRC_API
{
    public class CRCAPI
    {
        private readonly RestClient Client;
        private readonly CRCOptions Options;

        public CRCAPI(CRCOptions options)
        {
            Options = options;
            if (!string.IsNullOrEmpty(Options.BaseURI))
                Client = new RestClient(Options.BaseURI);
            else
                Client = new RestClient("https://crcindustries.em-hosting.be/swagger");
        }

        public Result<Token> Authenticate()
        {
            Result<Token> result = new Result<Token>();

            RestRequest request = new RestRequest("/token", Method.Post);
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", Options.Username);
            request.AddParameter("password", Options.Password);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            var response = Client.Execute(request);

            if (response == null)
            {
                result.Error = "Unknown error, authentication response is null";
                return result;
            }

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                result.Error = $"Error message: {response.ErrorMessage}";

                if (response.ErrorException != null && !string.IsNullOrEmpty(response.ErrorException.Message))
                    result.Error += $";exception message: {response.ErrorException.Message}";

                return result;
            }

            if (response.ResponseStatus == ResponseStatus.Completed && string.IsNullOrEmpty(response.Content))
            {
                result.StatusCode = response.StatusCode;
                result.Error = response.ErrorMessage;
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                    result.Error = "Response content is empty";
                return result;
            }

            if (response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
            {
                var token = JsonConvert.DeserializeObject<Token>(response.Content);
                result.Value = token;
            }

            return result;
        }

        public Result<T> Execute<T>(string? resource, Method method, IEnumerable<Parameter>? parameters = null)
        {
            Result<T> result = new Result<T>();
            string bearerToken = string.Empty;

            var authResult = Authenticate();

            if (authResult == null)
            {
                result.Error = "Unknown error, authentication result is null";
                return result;
            }

            if (!string.IsNullOrEmpty(authResult.Error))
            {
                result.Error = authResult.Error;
                return result;
            }

            if (string.IsNullOrEmpty(authResult.Error) && authResult.Value != null && !string.IsNullOrEmpty(authResult.Value.AccessToken))
            {
                bearerToken = authResult.Value.AccessToken;
            }

            RestRequest request = new RestRequest(resource, method);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {bearerToken}");
            if (parameters != null)
                request.Parameters.AddParameters(parameters);

            var response = Client.Execute(request);

            if (response == null)
            {
                result.Error = "Unknown error, response is null";
                return result;
            }

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                result.Error = $"Error message: {response.ErrorMessage}";

                if (response.ErrorException != null && !string.IsNullOrEmpty(response.ErrorException.Message))
                    result.Error += $";exception message: {response.ErrorException.Message}";

                return result;
            }

            if (response.ResponseStatus == ResponseStatus.Completed && string.IsNullOrEmpty(response.Content))
            {
                result.StatusCode = response.StatusCode;
                result.Error = response.ErrorMessage;
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                    result.Error = "Response content is empty";
                return result;
            }

            if (response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
            {
                var contentObject = JsonConvert.DeserializeObject<T>(response.Content);
                result.Value = contentObject;
            }

            return result;
        }

        public Result<IEnumerable<EmployeeDTO>> GetEmployees()
        {
            Result<IEnumerable<EmployeeDTO>> result = new Result<IEnumerable<EmployeeDTO>>();

            var requestResult = Execute<IEnumerable<EmployeeDTO>>("/api/public/employee", Method.Get);

            if (requestResult == null)
            {
                result.Error = "Unknown error, result is null";
                return result;
            }

            if (!string.IsNullOrEmpty(requestResult.Error))
            {
                result.Error = requestResult.Error;
                return result;
            }

            throw new NotImplementedException();
        }
    }
}
