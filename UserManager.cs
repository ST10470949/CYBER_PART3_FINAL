using System;
using System.Collections.Generic;
using System.IO;

namespace CyberSecurity_Part2._2
{
    // UserManager stores simple session and persistent user data.
    // This is a small helper to remember the current user's name
    // and to count how many times a user has used the app.
    public static class UserManager
    {
        // The username for the current session. Other classes read this value.
        public static string CurrentUserName { get; set; } = string.Empty;

        // File where we persist simple user visit counts.
        // Using a human-readable file next to the application so it survives restarts.
        private static string CountsFile => Path.Combine(AppContext.BaseDirectory, "users.txt");

        // Read counts from disk into a dictionary (name -> count).
        // Returns an empty dictionary if the file does not exist.
        public static Dictionary<string, int> LoadUserCounts()
        {
            var dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            if (!File.Exists(CountsFile)) return dict;
            foreach (var line in File.ReadAllLines(CountsFile))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var idx = line.LastIndexOf(':');
                if (idx <= 0) continue;
                var n = line.Substring(0, idx).Trim();
                var s = line.Substring(idx + 1).Trim();
                if (int.TryParse(s, out int v)) dict[n] = v;
            }
            return dict;
        }

        // Save counts dictionary to disk in a simple text format.
        // Each line looks like: Name:Count
        public static void SaveUserCounts(Dictionary<string, int> dict)
        {
            try
            {
                var lines = new List<string>();
                foreach (var kv in dict) lines.Add($"{kv.Key}:{kv.Value}");
                File.WriteAllLines(CountsFile, lines);
            }
            catch
            {
                // ignore
            }
        }

        // Increase the stored count for a given user name by 1 and return the new count.
        // This persists the count to disk so it can be read on future runs.
        public static int IncrementUserCount(string name)
        {
            try
            {
                var counts = LoadUserCounts();
                counts.TryGetValue(name, out int prev);
                var next = prev + 1;
                counts[name] = next;
                SaveUserCounts(counts);
                return next;
            }
            catch
            {
                // on error return 1 as a safe default
                return 1;
            }
        }

        // Get the stored visit count for a user. Returns 0 if not found.
        public static int GetUserCount(string name)
        {
            try
            {
                var counts = LoadUserCounts();
                counts.TryGetValue(name, out int prev);
                return prev;
            }
            catch
            {
                return 0;
            }
        }

        // Return true when a user is considered a returning user (more than 2 visits).
        public static bool IsReturningUser(string name)
        {
            return GetUserCount(name) > 2;
        }
    }
}
