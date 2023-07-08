using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using AzureTokenApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureTokenApi.Controllers;

[ApiController]
[Route("deviceauth")]
public class DeviceAuthController : ControllerBase
{
    /// <summary>
    /// Get a device code for Azure AD authentication.
    /// </summary>
    /// <remarks>See https://learn.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-device-code for more info</remarks>
    /// <response code="200">New user code was issued</response>
    [HttpGet("get-code")]
    [ProducesResponseType(typeof(DeviceAuthResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCode()
    {
        using var client = CreateHttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/organizations/oauth2/v2.0/devicecode");
        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("client_id", "04b07795-8ddb-461a-bbee-02f9e1bf7b46"),
            new KeyValuePair<string, string>("scope", "https://management.core.windows.net//.default offline_access openid profile"),
        });

        var response = await client.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();
        var authResponse = JsonSerializer.Deserialize<DeviceAuthResponse>(body);
        return Ok(authResponse);
    }

    /// <summary>
    /// Redeem a device code for an access token.
    /// </summary>
    /// <remarks>Complete login specified in the previous step</remarks>
    /// <param name="deviceCode">Device code, returned from "get-code" endpoint</param>
    /// <response code="200">Successfully obtained access token</response>
    /// <response code="400">Code is invalid, or user has not complete auth</response>
    [HttpGet("get-token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetToken([FromQuery(Name = "device_code"), Required] string deviceCode)
    {
        using var client = CreateHttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/organizations/oauth2/v2.0/token");
        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("client_id", "04b07795-8ddb-461a-bbee-02f9e1bf7b46"),
            new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:device_code"),
            new KeyValuePair<string, string>("client_info", "1"),
            new KeyValuePair<string, string>("code", deviceCode),
            new KeyValuePair<string, string>("device_code", deviceCode),
            new KeyValuePair<string, string>("claims", "{\"access_token\": {\"xms_cc\": {\"values\": [\"CP1\"]}}}"),
        });

        var response = await client.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            return BadRequest(JsonSerializer.Deserialize<ErrorResponse>(body));
        }

        return Ok(JsonSerializer.Deserialize<TokenResponse>(body));
    }

    private static HttpClient CreateHttpClient()
    {
        return new HttpClient
        {
            DefaultRequestHeaders =
            {
                { "user-agent", "python-requests/2.26.0" },
                { "x-client-cpu", "x86" },
                { "x-client-os", "x-client-os" },
                { "x-client-sku", "MSAL.Python" },
                { "x-client-ver", "1.20.0" },
                { "x-ms-lib-capability", "retry-after, h429" },
            }
        };
    }
}