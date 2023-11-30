using Fta.DemoFunc.Api.Interfaces;
using Fta.DemoFunc.Api.Models;
using Fta.DemoFunc.Api.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Fta.DemoFunc.Api.Services
{
    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(HttpClient httpClient, ILogger<NotificationService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task SendNoteCreatedEventAsync(CreateNoteOptions createNoteOptions, CancellationToken ct = default)
        {
            try
            {
                var noteCreatedRequest = new NoteCreatedRequest
                {
                    Title = createNoteOptions.Title,
                    Description = createNoteOptions.Body
                };
                var json = JsonSerializer.Serialize(noteCreatedRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var httpResponseMessage = await _httpClient.PostAsync("notes", data, ct);

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    var responseContentStr = await httpResponseMessage.Content.ReadAsStringAsync(ct);

                    _logger.LogError("The HTTP request failed with the following error: {ResponseContent}", responseContentStr);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in {ClassName} -> {MethodName} method.", nameof(NotificationService), nameof(SendNoteCreatedEventAsync));
            }
        }
    }
}
