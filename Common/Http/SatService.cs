/*
 * ============================================================================
 * Mozilla Public License 2.0 (MPL-2.0)
 * Autor: FISCAL API S. DE R.L. DE C.V. - https://fiscalapi.com
 * ============================================================================
 *
 * Este código está sujeto a los términos de la Mozilla Public License v2.0.
 * Licencia completa: https://mozilla.org/MPL/2.0
 *
 * AVISO: Este software se proporciona "tal como está" sin garantías de ningún
 * tipo. Al usar, modificar o distribuir este código debe mantener esta
 * atribución y las referencias al autor.
 *
 * ============================================================================
 */

using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Fiscalapi.XmlDownloader.Common.Http;

/// <summary>
/// Abstract base HTTP client for sending soap requests.
/// </summary>
public abstract class SatService : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly bool _ownsHttpClient;
    private readonly ILogger _logger;
    private bool _disposed;

    protected bool IsDebugEnabled { get; set; }

    /// <summary>
    /// Constructor for dependency injection scenarios
    /// </summary>
    protected SatService(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _ownsHttpClient = false;
    }

    /// <summary>
    /// Constructor for direct instantiation scenarios
    /// </summary>
    protected SatService(ILogger logger)
    {
        _httpClient = new HttpClient();
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _ownsHttpClient = true;
    }

    /// <summary>
    /// Sends a soap XML request to the specified endpoint and action with the provided payload and optional auth token.
    /// </summary>
    /// <param name="url">Endpoint URL</param>
    /// <param name="action">SOAP action</param>
    /// <param name="payload">XML content to send</param>
    /// <param name="token">Authorization token</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>SatResponse from server</returns>
    protected async Task<SatResponse> SendRequestAsync(string url, string action, string payload,
        string? token = null, CancellationToken cancellationToken = default)
    {
        var content = new StringContent(payload, Encoding.UTF8, "text/xml");
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = content
        };

        request.Headers.Add("SOAPAction", action);

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Add("Authorization", $@"WRAP access_token=""{token}""");
        }

        try
        {
            LogRequest(request, url, action, payload);

            using var response =
                await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            LogResponse(response, responseContent, url, action);

            return new SatResponse
            {
                IsSuccessStatusCode = response.IsSuccessStatusCode,
                HttpStatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                RawResponse = responseContent,
                RawRequest = payload
            };
        }
        catch (Exception ex)
        {
            LogError(ex, url, action);
            
            _logger.LogError(ex, "Error sending SOAP request to {Url} with action {Action}", url, action);

            return new SatResponse
            {
                IsSuccessStatusCode = false,
                HttpStatusCode = HttpStatusCode.InternalServerError,
                ReasonPhrase = ex.Message,
                RawRequest = payload
            };
        }
    }

    /// <summary>
    /// Logs the raw HTTP request details
    /// </summary>
    /// <param name="request">HttpRequestMessage to log</param>
    /// <param name="url">Request URL</param>
    /// <param name="soapAction">SOAP action header</param>
    /// <param name="payload">Request payload</param>
    private void LogRequest(HttpRequestMessage request, string url, string soapAction, string payload)
    {
        try
        {
            var serverDateTime = DateTime.Now;
            var serverTimeZone = TimeZoneInfo.Local;
            var timestamp = $"{serverDateTime:yyyy-MM-dd HH:mm:ss} {serverTimeZone.Id}";
            
            _logger.LogInformation("SOAP Request - Endpoint: {Url}, HTTP Method: {Method}, Timestamp: {Timestamp}, RawRequest: {RawRequest}",
                url, request.Method, timestamp, payload);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging request");
        }
    }

    /// <summary>
    /// Logs the raw HTTP response details
    /// </summary>
    /// <param name="response">HttpResponseMessage to log</param>
    /// <param name="responseContent">Response content</param>
    /// <param name="url">Original request URL</param>
    /// <param name="soapAction">Original SOAP action</param>
    private void LogResponse(HttpResponseMessage response, string responseContent, string url,
        string soapAction)
    {
        try
        {
            var serverDateTime = DateTime.Now;
            var serverTimeZone = TimeZoneInfo.Local;
            var timestamp = $"{serverDateTime:yyyy-MM-dd HH:mm:ss} {serverTimeZone.Id}";
            var statusCode = (int)response.StatusCode;
            var rawResponse = string.IsNullOrEmpty(responseContent) ? "[Empty Response]" : responseContent;
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("SOAP Response - Endpoint: {Url}, HTTP Status Code: {StatusCode}, Timestamp: {Timestamp}, RawResponse: {RawResponse}",
                    url, statusCode, timestamp, rawResponse);
            }
            else
            {
                _logger.LogError("SOAP Response - Endpoint: {Url}, HTTP Status Code: {StatusCode}, Timestamp: {Timestamp}, RawResponse: {RawResponse}",
                    url, statusCode, timestamp, rawResponse);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging response");
        }
    }

    private void LogError(Exception ex, string url, string soapAction)
    {
        try
        {
            var serverDateTime = DateTime.Now;
            var serverTimeZone = TimeZoneInfo.Local;
            var timestamp = $"{serverDateTime:yyyy-MM-dd HH:mm:ss} {serverTimeZone.Id}";
            var innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : null;
            
            _logger.LogError(ex, "SOAP Error - Endpoint: {Url}, SOAP Action: {SoapAction}, Timestamp: {Timestamp}, Exception Type: {ExceptionType}, Message: {Message}, Inner Exception: {InnerException}",
                url, soapAction, timestamp, ex.GetType().Name, ex.Message, innerExceptionMessage);
        }
        catch (Exception logEx)
        {
            _logger.LogError(logEx, "Error logging exception");
        }
    }

    /// <summary>
    /// Disposes the service, releasing resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the service, releasing resources.
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing && _ownsHttpClient)
        {
            _httpClient?.Dispose();
        }

        _disposed = true;
    }
}