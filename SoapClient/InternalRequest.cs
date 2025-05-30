﻿using System.Text;
using Fiscalapi.XmlDownloader.Services.Authenticate;
using Fiscalapi.XmlDownloader.Services.Common;

namespace Fiscalapi.XmlDownloader.SoapClient
{
    public class InternalRequest
    {
        public string? Url { get; set; }
        public string? SoapAction { get; set; }
        public string? RawRequest { get; set; }
        public EndPointName EndPointName { get; set; }
        public HttpMethod HttpMethod { get; set; } = HttpMethod.Post;
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        public string MediaType { get; set; } = "text/xml";

        public List<KeyValuePair<string, string>> Headers { get; set; } = new();

        public AuthenticateResult? Token { get; set; }

        public void AddHeader(string key, string value)
        {
            Headers.Add(new KeyValuePair<string, string>(key, value));
        }
    }
}