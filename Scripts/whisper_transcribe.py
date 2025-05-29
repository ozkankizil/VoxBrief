import whisper
import sys
import sys
sys.stdout.reconfigure(encoding='utf-8')

audio_path = sys.argv[1]

print("MODEL YÜKLENİYOR...")
model = whisper.load_model("tiny")

print("TRANSCRIBE BAŞLIYOR...")
result = model.transcribe(audio_path, language="Turkish")

print("İŞLEM TAMAMLANDI.")
print(result["text"])
