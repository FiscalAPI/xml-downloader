﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlDownloader.Core.Common;

namespace XmlDownloader.Core.Services.Download
{
    public class DownloadResult : Result, IHasSuccessResponse
    {
        public bool IsSuccess { get; set; }

        public string? PackageId { get; set; }
        public string? PackageBase64 { get; set; }
    }
}