﻿using Fiscalapi.XmlDownloader.Services.Common;
using Fiscalapi.XmlDownloader.SoapClient;

namespace Fiscalapi.XmlDownloader.Services.Download
{
    public class DownloadResult : Result, IHasSuccessResponse, IHasInternalRequestResponse
    {
        public bool IsSuccess { get; set; }

        public string? PackageId { get; set; }
        public string? PackageBase64 { get; set; }
        public InternalRequest? InternalRequest { get; set; }
        public InternalResponse? InternalResponse { get; set; }
    }
}