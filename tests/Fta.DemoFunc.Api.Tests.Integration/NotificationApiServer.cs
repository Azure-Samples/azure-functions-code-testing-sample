using System;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Fta.DemoFunc.Api.Tests.Integration
{
    public class NotificationApiServer : IDisposable
    {
        private WireMockServer? _server;

        public string Url => _server!.Url!;

        public void Start()
        {
            _server = WireMockServer.Start();
        }

        public void SetupRequestDetails()
        {
            _server!.Given(Request.Create()
                .WithPath($"/notes")
                .UsingPost()
                .WithBody(GenerateRequestBody()))
                .RespondWith(Response.Create()
                    .WithBody(GenerateResponseBody())
                    .WithHeader("content-type", "application/json; charset=utf-8")
                    .WithStatusCode(HttpStatusCode.Created));
        }

        public void Dispose()
        {
            _server!.Stop();
            _server.Dispose();
        }

        private static string GenerateRequestBody()
        {
            return $@"{{""title"":""Test note title"",""description"":""Test note description""}}";
        }

        private static string GenerateResponseBody()
        {
            return $@"{{""title"":""Test note title"",""description"":""Test note description"",""id"":""999""}}";
        }
    }
}
