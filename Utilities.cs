using System;
using System.Threading.Tasks;

namespace CyberTimes_ChatBot_Part2
{
    // Utilities contains small helper functions used across the app.
    // Keeping these helpers simple makes the rest of the code easier to read.
    public static class Utilities
    {
        // DelayTypingAsync is a tiny helper that waits asynchronously for the given ms.
        // Input: ms - number of milliseconds to wait.
        // Output: a Task that completes after the delay.
        public static async Task DelayTypingAsync(int ms)
        {
            await Task.Delay(ms);
        }

        // CleanInput trims whitespace and protects against null strings.
        // Input: s - raw input string.
        // Output: trimmed safe string.
        public static string CleanInput(string s)
        {
            return (s ?? string.Empty).Trim();
        }
    }
}
