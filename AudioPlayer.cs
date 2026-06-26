using System;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Threading.Tasks;

namespace CyberTimes_ChatBot_Part2
{
    // AudioPlayer provides small helpers to play WAV files from the application folder.
    // It does not block the UI and it logs simple errors using Debug.WriteLine.
    public static class AudioPlayer
    {
        // Play the default welcome sound file 'Abos.wav' if it exists.
        public static void PlayWelcomeSound()
        {
            PlayIfExists("Abos2.wav");
        }

        // Play a WAV file in the application folder if it exists (non-blocking).
        // Input: filename - name of the wav file (for example "sound.wav").
        // Output: none. Method will return immediately and audio plays in background.
        public static void PlayIfExists(string filename)
        {
            try
            {
                var path = Path.Combine(AppContext.BaseDirectory, filename ?? string.Empty);
                if (!File.Exists(path))
                {
                    Debug.WriteLine($"Audio file not found: {path}");
                    return;
                }

                // Use SoundPlayer.Play for non-blocking playback
                var player = new SoundPlayer(path);
                player.Play();
            }
            catch (Exception ex)
            {
                // Do not throw from UI helper; log for diagnostics
                Debug.WriteLine($"Audio playback error: {ex.Message}");
            }
        }

        // Optional: asynchronous wrapper if caller prefers awaiting
        public static Task PlayIfExistsAsync(string filename)
        {
            return Task.Run(() => PlayIfExists(filename));
        }
    }
}
