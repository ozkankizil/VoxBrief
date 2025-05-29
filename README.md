# VoxBrief

A project designed to upload Turkish .mp3 audio files and generate transcripts.
The project was built using ASP.NET 8 Core with the default frontend. Uploaded audio files were saved to the server and initially transcribed using OpenAI’s Whisper API. However, to pursue a free and local solution, transcription was later handled using Python and Whisper locally.

As the raw transcription output was not satisfactory, an improvement phase was initiated using a local LLM. LM Studio along with the Mistral 7B model was integrated to enhance the transcription text.

Although the project was technically successful, it was concluded that the solution was not feasible in terms of time, resources, and overall requirements. Therefore, the project has been discontinued.
