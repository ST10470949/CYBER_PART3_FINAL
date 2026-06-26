using System;
using System.Collections.Generic;

namespace CyberTimes_ChatBot_Part2
{
    // ExternalChatbot provides a simple static menu of topics.
    // This was used originally for console scenarios and can be used by the UI
    // to show a numbered list of topic options.
    public static class ExternalChatbot
    {
        // GetMenuLines returns a small list of menu entries for display.
        // Each entry is a string describing a topic.
        public static List<string> GetMenuLines()
        {
            return new List<string>
            {
                "0 — Return to Main Menu",
                "1 — Password Safety",
                "2 — Phishing",
                "3 — Malware",
                "4 — Social Engineering",
                "5 — Two-Factor Authentication",
                "6 — Safe Browsing",
                "7 — Ransomware",
                "8 — Public Wi-Fi Safety",
                "9 — Identity Theft",
                "10 — Software Updates",
                "11 — Encryption",
                "12 — Firewalls",
                "13 — Backup & Recovery",
                "14 — Incident Response",
                "15 — Privacy Settings",
                "16 — IoT Security",
                "17 — Mobile Security",
                "18 — Secure Coding",
                "19 — Network Segmentation",
                "20 — Access Control",
                "21 — Security Policies"
            };
        }
    }
}
