using Microsoft.Graph;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FitManager.Webapi.Services
{
    /// <summary>
    /// AD Client to authenticate non-interactive in a background service.
    /// See https://learn.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-auth-code-flow
    /// </summary>
    public class AzureAdClient
    {
        private readonly string _tenantId;
        private readonly string _clientId;
        private readonly string _redirectUrl;
        private readonly string _clientSecret;
        private readonly string _scope;

        public AzureAdClient(string tenantId, string clientId, string redirectUrl, string clientSecret, string scope)
        {
            _tenantId = tenantId;
            _clientId = clientId;
            _redirectUrl = redirectUrl;
            _clientSecret = clientSecret;
            _scope = scope;
        }

        public string LoginUrl => $"https://login.microsoftonline.com/{_tenantId}/oauth2/v2.0/authorize?client_id={_clientId}&response_type=code&redirect_uri={HttpUtility.UrlEncode(_redirectUrl)}&prompt=consent&response_mode=query&scope=user.read offline_access mail.send";

        public async Task<(string authToken, string? refreshToken)> GetToken(string code)
        {
            var formdata = $"client_id={_clientId}&scope={_scope}&code={code}&redirect_uri={_redirectUrl}&grant_type=authorization_code&client_secret={_clientSecret}";
            return await RequestTokens(formdata);
        }

        public async Task<(string authToken, string? refreshToken)> GetNewToken(string refreshToken)
        {
            var formdata = $"client_id={_clientId}&scope={_scope}&refresh_token={refreshToken}&grant_type=refresh_token&client_secret={_clientSecret}";
            return await RequestTokens(formdata);
        }

        public GraphServiceClient GetGraphServiceClientFromToken(string token)
        {
            var authProvider = new DelegateAuthenticationProvider(request =>
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
                return Task.CompletedTask;
            });

            return new GraphServiceClient(authProvider);
        }

        private async Task<(string authToken, string? refreshToken)> RequestTokens(string formdata)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri($"https://login.microsoftonline.com/{_tenantId}/oauth2/v2.0/");
            var response = await client.PostAsync("token", new StringContent(formdata, Encoding.UTF8, "application/x-www-form-urlencoded"));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) { throw new ApplicationException(content); }

            var data = System.Text.Json.JsonDocument.Parse(content).RootElement;
            var authToken = data.GetProperty("access_token").GetString()
                ?? throw new ApplicationException("Missing auth token in response.");
            var refreshToken = data.TryGetProperty("refresh_token", out var val)
                ? val.GetString() : null;
            return (authToken, refreshToken);
        }
    }
}