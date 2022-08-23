using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz.Examples.AspNetCore.CRC_API.DTO;
using RestSharp;
using System;
using CRCModels = Quartz.Examples.AspNetCore.CRC_API.DTO;

namespace Quartz.Examples.AspNetCore.CRC_API
{
    public class CRCAPI
    {
        private readonly RestClient Client;
        private readonly CRCOptions Options;

        public CRCAPI(IOptions<CRCOptions> options)
        {
            Options = options.Value;
            if (!string.IsNullOrEmpty(Options.BaseURI))
                Client = new RestClient(Options.BaseURI);
            else
                Client = new RestClient("https://crcindustries.em-hosting.be/swagger");
        }

        public Result<CRCModels.Token> Authenticate()
        {
            Result<CRCModels.Token> result = new Result<CRCModels.Token>();

            RestRequest request = new RestRequest("/token", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept", "application/json");
            //request.AddParameter("grant_type", "password", ParameterType.RequestBody);
#pragma warning disable CS8604 // Possible null reference argument.
            //request.AddParameter("username", Options.Username, ParameterType.RequestBody);
            //request.AddParameter("password", Options.Password, ParameterType.RequestBody);
#pragma warning restore CS8604 // Possible null reference argument.
            request.AddParameter("application/x-www-form-urlencoded", $"grant_type=password&username={Options.Username}&password={Options.Password}", ParameterType.RequestBody);
            //request.MultipartFormQuoteParameters = true;
            //request.AlwaysMultipartFormData = true;

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
                var token = JsonConvert.DeserializeObject<CRCModels.Token>(response.Content);
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
                result.Error = "Unknown error, request result is null";
                return result;
            }

            if (!string.IsNullOrEmpty(requestResult.Error))
            {
                result.Error = requestResult.Error;
                return result;
            }

            if (string.IsNullOrEmpty(requestResult.Error) && requestResult.Value == null)
            {
                result.Error = "Unknown error, request result value is null";
                return result;
            }

            return requestResult;
        }

        public Result<IEnumerable<EmployeePresenceDTO>> GetEmployeePresences()
        {
            Result<IEnumerable<EmployeePresenceDTO>> result = new Result<IEnumerable<EmployeePresenceDTO>>();

            var requestResult = Execute<IEnumerable<EmployeePresenceDTO>>("/api/public/employee/presence", Method.Get);

            if (requestResult == null)
            {
                result.Error = "Unknown error, request result is null";
                return result;
            }

            if (!string.IsNullOrEmpty(requestResult.Error))
            {
                result.Error = requestResult.Error;
                return result;
            }

            if (string.IsNullOrEmpty(requestResult.Error) && requestResult.Value == null)
            {
                result.Error = "Unknown error, request result value is null";
                return result;
            }

            return requestResult;
        }
    }
}
