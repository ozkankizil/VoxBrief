using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VoxBrief.Helpers
{
    public static class LmStudioClient
    {
        public static async Task<string> FixTranscriptAsync(string inputText)
        {
            using var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(10)
            };
            httpClient.BaseAddress = new Uri("http://localhost:1234");

            var modelName = "mistral-7b-instruct-v0.2.Q4_K_M";

            var requestBody = new
            {
                model = modelName,
                messages = new[]
                {
            new {
                role = "user",
                content = "Aşağıdaki Türkçe metni Türkçe dil bilgisine göre düzelt ve çıktı da Türkçe olmalı. Örneğin eksik kelimeler veya cümlenin anlam yapısına aykırı olan kelimeleri düzelt. Bunu yaparken metnin orijinalliğini asla bozma. Sadece metinde yanlış yazılmış olan kelimeleri düzelteceksin:\n\n" + inputText
            }
        },
                temperature = 0.7,
                stream = false
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(requestBody, options);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("/v1/chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"LM Studio Hatası: {response.StatusCode}\n{error}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseString);

            return doc.RootElement
                      .GetProperty("choices")[0]
                      .GetProperty("message")
                      .GetProperty("content")
                      .GetString() ?? "";
        }

    }
}
