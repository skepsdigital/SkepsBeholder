using System.Text.Json;
using System.Text;

namespace SkepsBeholder.Services.Email
{
    public class Email : IEmail
    {
        private readonly string _authorizationKey;
        private readonly string _endpointUrl;
        private readonly HttpClient _httpClient;

        public Email()
        {
            _authorizationKey = "YmVob2xkZXIxOjhNakREQzBVTkxYOWxrQ0N0V3NZ";
            _endpointUrl = "https://brasgaming.http.msging.net/messages";
            _httpClient = new HttpClient();

            // Configurando o cabeçalho de autorização
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Key", _authorizationKey);
        }



        public async Task SendMessageAsync(string recipient, string content)
        {
            var message = new
            {
                to = Uri.EscapeDataString(recipient) + "@mailgun.gw.msging.net",
                type = "text/plain",
                content
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(message),
                Encoding.UTF8,
                "application/json");

            try
            {
                var response = await _httpClient.PostAsync(_endpointUrl, jsonContent);
                response.EnsureSuccessStatusCode();

                Console.WriteLine("Mensagem enviada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar mensagem: {ex.Message}");
                throw;
            }
        }
    }
}
