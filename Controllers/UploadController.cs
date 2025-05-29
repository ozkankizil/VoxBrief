using Microsoft.AspNetCore.Mvc;
using VoxBrief.Helpers;

namespace VoxBrief.Controllers
{
    public class UploadController : Controller
    {
        private readonly IConfiguration _configuration;

        public UploadController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadAudio(IFormFile audioFile)
        {
            string apiKey = _configuration["OpenAi:ApiKey"];

            if (audioFile == null || audioFile.Length == 0)
            {
                ViewBag.Message = "Ses dosyası seçilmedi.";
                return View("Index");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, Path.GetFileName(audioFile.FileName));
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await audioFile.CopyToAsync(stream);
            }

            var transcript = WhisperLocalHelper.GetTranscript(filePath);

            string fixedTranscript = await LmStudioClient.FixTranscriptAsync(transcript);

            ViewBag.FixedTranscript = fixedTranscript;
            ViewBag.Transcript = transcript;
            ViewBag.Message = "Dosya başarıyla yüklendi ve transkript oluşturuldu.";
            ViewBag.FileName = audioFile.FileName;
            ViewBag.Transcript = transcript;

            return View("Index");
        }


    }
}
