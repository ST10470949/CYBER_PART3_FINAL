using System;

namespace CyberSecurity_Part2._2
{
    // ChatMessage is a simple model that represents one chat line.
    // It stores who sent the message, the text, and a flag whether it's a user message.
    public class ChatMessage
    {
        // Name of the sender, e.g. "Bot" or the user's name.
        public string Sender { get; set; } = string.Empty;
        // The message text to display in the chat UI.
        public string Text { get; set; } = string.Empty;
        // IsUser is true when the message came from the user.
        public bool IsUser { get; set; }

        public ChatMessage() { }

        public ChatMessage(string sender, string text, bool isUser = false)
        {
            Sender = sender;
            Text = text;
            IsUser = isUser;
        }
    }
}