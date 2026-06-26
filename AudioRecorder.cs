using System;
using System.Threading.Tasks;

namespace CyberSecurity_Part2._2
{
    public class AudioRecorder : IDisposable
    {
        private bool _isRecording;
        public string? OutputFile { get; private set; }

        public bool IsRecording => _isRecording;

        public Task StartRecordingAsync(string outputFile)
        {
            OutputFile = outputFile;
            _isRecording = true;
            // placeholder: actual recording not implemented
            Console.WriteLine($"(Stub) Start recording to {outputFile}");
            return Task.CompletedTask;
        }

        // Synchronous wrapper used by existing code
        public void StartRecording(string outputFile)
        {
            _ = StartRecordingAsync(outputFile);
        }

        public Task StopRecordingAsync()
        {
            _isRecording = false;
            Console.WriteLine("(Stub) Stop recording");
            return Task.CompletedTask;
        }

        // Synchronous wrapper used by existing code
        public void StopRecording()
        {
            _ = StopRecordingAsync();
        }

        public void Dispose()
        {
            _ = StopRecordingAsync();
        }
    }
}
