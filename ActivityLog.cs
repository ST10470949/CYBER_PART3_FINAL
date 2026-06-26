using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurity_Part2._2
{
    // ActivityLog records all significant actions taken during the session.
    // It stores entries and provides formatted display text with show-more support.
    public static class ActivityLog
    {
        // A single log entry with a timestamp and description.
        public class LogEntry
        {
            // When the action happened.
            public DateTime Timestamp { get; set; }
            // A short description of what happened.
            public string Description { get; set; }
            // An emoji icon to make the log more readable.
            public string Icon { get; set; }
        }

        // Internal list of log entries — kept in full, display is capped.
        private static List<LogEntry> _entries = new List<LogEntry>();

        // Maximum entries to store in memory (raised to allow show-more).
        private const int MaxEntries = 50;

        // Default number of entries shown before "show more" is needed.
        private const int DefaultDisplay = 5;

        // Add a new entry to the activity log.
        // Input: icon - an emoji e.g. "📝", description - what happened.
        public static void Add(string icon, string description)
        {
            _entries.Add(new LogEntry
            {
                Timestamp = DateTime.Now,
                Icon = icon ?? "•",
                Description = description ?? string.Empty
            });
            // Keep only the most recent MaxEntries entries
            if (_entries.Count > MaxEntries)
                _entries = _entries.Skip(_entries.Count - MaxEntries).ToList();
        }

        // Get all current log entries in reverse order (most recent first).
        public static List<LogEntry> GetEntries()
        {
            var copy = new List<LogEntry>(_entries);
            copy.Reverse();
            return copy;
        }

        // Get a formatted string of the last DefaultDisplay entries.
        // Shows a "show more" hint if there are more entries available.
        public static string GetFormattedLog()
        {
            if (_entries.Count == 0)
                return "No activity recorded yet. Start chatting, take the quiz or use a topic button to log actions here.";

            var lines = new List<string>();
            lines.Add("📋 Here is a summary of your recent activity:\n");
            var recent = GetEntries();
            int toShow = Math.Min(DefaultDisplay, recent.Count);
            for (int i = 0; i < toShow; i++)
            {
                var entry = recent[i];
                lines.Add($"{i + 1}. {entry.Icon} {entry.Description}  [{entry.Timestamp:HH:mm:ss}]");
            }
            if (recent.Count > DefaultDisplay)
            {
                int remaining = recent.Count - DefaultDisplay;
                lines.Add($"\n... and {remaining} more {(remaining == 1 ? "entry" : "entries")}. Type 'show more log' to see the full history.");
            }
            return string.Join("\n", lines);
        }

        // Get a formatted string of ALL entries (used when user asks for full history).
        public static string GetFullFormattedLog()
        {
            if (_entries.Count == 0)
                return "No activity recorded yet.";

            var lines = new List<string>();
            lines.Add($"📋 Full Activity History ({_entries.Count} {(_entries.Count == 1 ? "entry" : "entries")}):\n");
            var recent = GetEntries();
            for (int i = 0; i < recent.Count; i++)
            {
                var entry = recent[i];
                lines.Add($"{i + 1}. {entry.Icon} {entry.Description}  [{entry.Timestamp:HH:mm:ss}]");
            }
            return string.Join("\n", lines);
        }

        // Total number of entries currently stored.
        public static int Count => _entries.Count;

        // Clear all log entries.
        public static void Clear()
        {
            _entries.Clear();
        }
    }
}