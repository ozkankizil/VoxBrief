using Newtonsoft.Json.Linq;
using RestSharp;

namespace VoxBrief.Helpers
{
    public class OpenAiWhisperHelper
    {
        private readonly string _apiKey;

        public OpenAiWhisperHelper(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<string> GetTranscriptAsync(string filePath)
        {
            var client = new RestClient("https://api.openai.com/v1/audio/transcriptions");
            var request = new RestRequest();
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            request.AlwaysMultipartFormData = true;

            request.AddFile("file", filePath);
            request.AddParameter("model", "whisper-1");
            request.AddParameter("language", "tr");

            var response = await client.PostAsync(request);
            if (!response.IsSuccessful)
                throw new Exception("Transkripsiyon alınamadı: " + response.Content);

            var json = JObject.Parse(response.Content!);
            return json["text"]?.ToString() ?? "";
        }
    }
}
