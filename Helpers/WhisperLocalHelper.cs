using System.Diagnostics;
using System.Text;

namespace VoxBrief.Helpers
{
    public static class WhisperLocalHelper
    {
        public static string GetTranscript(string audioPath)
        {
            var psi = new ProcessStartInfo
            {
                FileName = @"C:\Users\[USER]\AppData\Local\Programs\Python\Python313\python.exe",
                Arguments = $"Scripts/whisper_transcribe.py \"{audioPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            using var process = Process.Start(psi)!;
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(error) &&
                !error.ToLower().Contains("fp16 is not supported") &&
                !error.ToLower().Contains("download") &&
                !error.ToLower().Contains("model"))
            {
                throw new Exception("Whisper Gerçek Hatası: " + error);
            }


            return output;
        }
    }
}
