﻿using Fiscalapi.XmlDownloader.Builder;

namespace Fiscalapi.XmlDownloader.SoapClient
{
    public static class InternalHttpClient
    {
        private static readonly HttpClient httpClient = new HttpClient();


        public static async Task<InternalResponse> SendAsync(InternalRequest internalRequest)
        {
            using var request = MessageBuilder.BuildHttpRequestMessage(internalRequest);

            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);

            var internalResponse = await MessageBuilder.BuildInternalResponseMessage(internalRequest, response);


            return internalResponse;
        }
    }
}