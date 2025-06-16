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

namespace XmlDownloader.Common.Http;

/// <summary>
/// Abstract base HTTP client for sending soap requests.
/// </summary>
public abstract class SatService : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly bool _ownsHttpClient;
    private bool _disposed;

    protected bool IsDebugEnabled { get; set; }

    /// <summary>
    /// Constructor for dependency injection scenarios
    /// </summary>
    protected SatService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _ownsHttpClient = false;
    }

    /// <summary>
    /// Constructor for direct instantiation scenarios
    /// </summary>
    protected SatService()
    {
        _httpClient = new HttpClient();
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
            // Log request 
            if (IsDebugEnabled)
                LogRequest(request, url, action, payload);

            using var response =
                await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            // Log response
            if (IsDebugEnabled)
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
            if (IsDebugEnabled)
                LogError(ex, url, action);

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
    private static void LogRequest(HttpRequestMessage request, string url, string soapAction, string payload)
    {
        try
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== SOAP REQUEST ===");
            sb.AppendLine($"HTTP Method: {request.Method}");
            sb.AppendLine($"URL: {url}");
            sb.AppendLine($"SOAP Action: {soapAction}");
            sb.AppendLine($"Timestamp: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");

            // Headers
            sb.AppendLine("Headers:");
            foreach (var header in request.Headers)
            {
                sb.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
            }

            // Content headers
            if (request.Content?.Headers != null)
            {
                sb.AppendLine("Content Headers:");
                foreach (var header in request.Content.Headers)
                {
                    sb.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                }
            }

            sb.AppendLine("Raw Request Body:");
            sb.AppendLine(payload);
            sb.AppendLine("=== END REQUEST ===");

            Console.WriteLine(sb.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging request: {ex.Message}");
        }
    }

    /// <summary>
    /// Logs the raw HTTP response details
    /// </summary>
    /// <param name="response">HttpResponseMessage to log</param>
    /// <param name="responseContent">Response content</param>
    /// <param name="url">Original request URL</param>
    /// <param name="soapAction">Original SOAP action</param>
    private static void LogResponse(HttpResponseMessage response, string responseContent, string url,
        string soapAction)
    {
        try
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== SOAP RESPONSE ===");
            sb.AppendLine($"URL: {url}");
            sb.AppendLine($"SOAP Action: {soapAction}");
            sb.AppendLine($"HTTP Status Code: {(int)response.StatusCode} {response.StatusCode}");
            sb.AppendLine($"Reason Phrase: {response.ReasonPhrase}");
            sb.AppendLine($"Success: {response.IsSuccessStatusCode}");
            sb.AppendLine($"Timestamp: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");

            // Response headers
            sb.AppendLine("Response Headers:");
            foreach (var header in response.Headers)
            {
                sb.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
            }

            // Content headers
            if (response.Content?.Headers != null)
            {
                sb.AppendLine("Content Headers:");
                foreach (var header in response.Content.Headers)
                {
                    sb.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                }
            }

            sb.AppendLine("Raw Response Body:");
            sb.AppendLine(string.IsNullOrEmpty(responseContent) ? "[Empty Response]" : responseContent);
            sb.AppendLine("=== END RESPONSE ===");

            Console.WriteLine(sb.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging response: {ex.Message}");
        }
    }

    /// <summary>
    /// Logs error details when an exception occurs
    /// </summary>
    /// <param name="ex">Exception that occurred</param>
    /// <param name="url">Request URL</param>
    /// <param name="soapAction">SOAP action</param>
    private static void LogError(Exception ex, string url, string soapAction)
    {
        try
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== SOAP ERROR ===");
            sb.AppendLine($"URL: {url}");
            sb.AppendLine($"SOAP Action: {soapAction}");
            sb.AppendLine($"Timestamp: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");
            sb.AppendLine($"Exception Type: {ex.GetType().Name}");
            sb.AppendLine($"Message: {ex.Message}");

            if (ex.InnerException != null)
            {
                sb.AppendLine($"Inner Exception: {ex.InnerException.Message}");
            }

            sb.AppendLine("Stack Trace:");
            sb.AppendLine(ex.StackTrace);
            sb.AppendLine("=== END ERROR ===");

            Console.WriteLine(sb.ToString());
        }
        catch (Exception logEx)
        {
            Console.WriteLine($"Error logging exception: {logEx.Message}");
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