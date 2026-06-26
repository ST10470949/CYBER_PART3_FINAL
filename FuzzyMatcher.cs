using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurity_Part2._2
{
    // FuzzyMatcher contains simple functions to help match topic words
    // even when the user makes small typos or uses slightly different words.
    // This is useful to suggest topics when the user's input is close to
    // one of the known topic keys.
    public static class FuzzyMatcher
    {
        // LevenshteinDistance computes how different two words are.
        // A result of 0 means the words are the same, higher numbers mean more changes needed.
        // Inputs: a, b - two strings to compare.
        // Output: an integer distance value.
        public static int LevenshteinDistance(string a, string b)
        {
            // If first string is empty, the distance is the length of the second.
            if (string.IsNullOrEmpty(a)) return string.IsNullOrEmpty(b) ? 0 : b.Length;
            // If second string is empty, the distance is the length of the first.
            if (string.IsNullOrEmpty(b)) return a.Length;

            // Create a 2D array to store distances between all prefixes.
            int[,] d = new int[a.Length + 1, b.Length + 1];
            // Initialize first column: distance from empty string a to prefixes of b.
            for (int i = 0; i <= a.Length; i++) d[i, 0] = i;
            // Initialize first row: distance from empty string b to prefixes of a.
            for (int j = 0; j <= b.Length; j++) d[0, j] = j;

            // Fill the table using dynamic programming.
            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    // cost is 0 when characters are the same, otherwise 1 for a substitution.
                    int cost = a[i - 1] == b[j - 1] ? 0 : 1;
                    // compute the minimum among deletion, insertion, and substitution.
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            // The bottom-right cell contains the full distance between the two strings.
            return d[a.Length, b.Length];
        }

        // GetSuggestions finds topic keys that are close to any token the user typed.
        // Inputs: topicKeys - a list of topic strings (like "phishing", "malware").
        //         tokens - words from the user's input.
        // Output: a list of topic keys that are similar to any token.
        public static List<string> GetSuggestions(IEnumerable<string> topicKeys, List<string> tokens)
        {
            var suggestions = new List<string>();
            // For each known topic key, split it into words and compare to each token.
            foreach (var kv in topicKeys)
            {
                var keyTokens = kv.Split(' ');
                foreach (var kt in keyTokens)
                {
                    foreach (var it in tokens)
                    {
                        // compute distance between the topic token and the input token
                        int dist = LevenshteinDistance(kt, it);
                        // threshold decides how many edits are allowed; small words have low threshold
                        int threshold = Math.Max(1, kt.Length / 3);
                        // if the distance is small enough, suggest this topic
                        if (dist <= threshold)
                        {
                            if (!suggestions.Contains(kv)) suggestions.Add(kv);
                        }
                    }
                }
            }
            return suggestions;
        }
    }
}
