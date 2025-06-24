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

using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Fiscalapi.XmlDownloader.Auth;
using Fiscalapi.XmlDownloader.Common.Attributes;
using Fiscalapi.XmlDownloader.Download;
using Fiscalapi.XmlDownloader.FileStorage;
using Fiscalapi.XmlDownloader.Query;
using Fiscalapi.XmlDownloader.Verify;
using Microsoft.Extensions.DependencyInjection;

namespace Fiscalapi.XmlDownloader;

public static class DownloaderExtensions
{
    private const string SatFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

    /// <summary>
    /// Converts a DateTime to the SAT format (yyyy-MM-ddTHH:mm:ss)
    /// </summary>
    /// <param name="dateTime">Date</param>
    /// <returns>Formatted Date</returns>
    public static string ToSatFormat(this DateTime dateTime)
    {
        return dateTime.ToString(SatFormat);
    }

    /// <summary>
    /// Converts the current date to the start of the day (00:00:00.000)
    /// </summary>
    /// <param name="dateTime">The date to convert</param>
    /// <returns>DateTime with the same date but with time set to 00:00:00.000</returns>
    public static DateTime ToStartOfDay(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
    }

    /// <summary>
    /// Converts the current date to the end of the day (23:59:59)
    /// </summary>
    /// <param name="dateTime">The date to convert</param>
    /// <returns>DateTime with the same date but with time set to 23:59:59</returns>
    public static DateTime ToEndOfDay(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
    }

    /// <summary>
    /// Subtracts a specific number of seconds from the date
    /// </summary>
    /// <param name="dateTime">The date from which to subtract seconds</param>
    /// <param name="seconds">Number of seconds to subtract</param>
    /// <returns>DateTime with the seconds subtracted</returns>
    public static DateTime MinusSeconds(this DateTime dateTime, int seconds)
    {
        return dateTime.AddSeconds(-seconds);
    }

    /// <summary>
    /// Cleans an XML string by formatting it properly according to SAT standards.
    /// </summary>
    /// <param name="xml">Dirty XML</param>
    /// <returns>Cleaned XML</returns>
    public static string Clean(this string xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
            return string.Empty;


        var element = XElement.Parse(xml);
        var settings = new XmlWriterSettings
        {
            ConformanceLevel = ConformanceLevel.Auto,
            Encoding = Encoding.UTF8,
            Indent = false,
            OmitXmlDeclaration = true,
            NewLineHandling = NewLineHandling.None,
            NamespaceHandling = NamespaceHandling.OmitDuplicates,
            NewLineOnAttributes = false,
            WriteEndDocumentOnClose = false
        };

        var sb = new StringBuilder();
        using (var writer = XmlWriter.Create(sb, settings))
            element.Save(writer);

        return sb.ToString();
    }


    /// <summary>
    /// Cache for mapping enum types to their string codes and vice versa.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, Dictionary<string, object>> CodeToEnumCache = new();

    private static readonly ConcurrentDictionary<Enum, string> EnumToCodeCache = new();

    public static string ToEnumCode<TEnum>(this TEnum value) where TEnum : Enum
    {
        if (EnumToCodeCache.TryGetValue(value, out var cachedCode))
            return cachedCode;

        var type = typeof(TEnum);
        var memberInfo = type.GetMember(value.ToString()).FirstOrDefault();

        var code = memberInfo?
            .GetCustomAttribute<EnumCodeAttribute>()?
            .Code ?? string.Empty;


        EnumToCodeCache[value] = code;
        return code;
    }

    /// <summary>
    /// Converts a string code to its corresponding enum element using the EnumCodeAttribute.
    /// </summary>
    /// <typeparam name="TEnum">Enum type</typeparam>
    /// <param name="code">Enum code</param>
    /// <returns>Enum element</returns>
    /// <exception cref="ArgumentException"></exception>
    public static TEnum ToEnumElement<TEnum>(this string code) where TEnum : struct, Enum
    {
        var type = typeof(TEnum);

        var map = CodeToEnumCache.GetOrAdd(type, _ =>
        {
            return Enum.GetValues<TEnum>()
                .Cast<Enum>()
                .Select(e =>
                {
                    var memberInfo = type.GetMember(e.ToString()).FirstOrDefault();
                    var attr = memberInfo?.GetCustomAttribute<EnumCodeAttribute>();
                    return (Enum: e, attr?.Code);
                })
                .Where(x => x.Code != null)
                .ToDictionary(x => x.Code!, x => (object)x.Enum);
        });

        if (map.TryGetValue(code, out var value))
            return (TEnum)value;

        throw new ArgumentException($"No enum value of type '{type.Name}' has an EnumCodeAttribute with code '{code}'.",
            nameof(code));
    }


    /// <summary>
    /// Adds Xml downloader services to the dependency injection container
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddXmlDownloader(this IServiceCollection services)
    {
        // Register the services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IQueryService, QueryService>();
        services.AddScoped<IVerifyService, VerifyService>();
        services.AddScoped<IDownloadService, DownloadService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IXmlDownloaderService, XmlDownloaderService>();

        return services;
    }
}