using System;
using System.Collections.Generic;

namespace CyberSecurity_Part2._2
{
    // QuizManager holds all quiz questions and tracks the current quiz session.
    // It provides methods to start, answer and score the quiz.
    public static class QuizManager
    {
        // Represents a single quiz question with options and the correct answer.
        public class QuizQuestion
        {
            // The question text shown to the user.
            public string Question { get; set; }
            // The list of answer options e.g. A, B, C, D or True/False.
            public List<string> Options { get; set; }
            // The correct answer text — must match one of the Options exactly.
            public string CorrectAnswer { get; set; }
            // A brief explanation shown after the user answers.
            public string Explanation { get; set; }
            // The type of question: "multiple" or "truefalse".
            public string Type { get; set; }
        }

        // All quiz questions. More than 10 as required by the POE.
        public static readonly List<QuizQuestion> AllQuestions = new List<QuizQuestion>
        {
            // --- MULTIPLE CHOICE QUESTIONS ---
            new QuizQuestion
            {
                Type = "multiple",
                Question = "What should you do if you receive an email asking for your password?",
                Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" },
                CorrectAnswer = "C",
                Explanation = "You should always report phishing emails. Reporting helps your email provider block similar attacks for everyone."
            },
            new QuizQuestion
            {
                Type = "multiple",
                Question = "Which of the following is the strongest password?",
                Options = new List<string> { "A) password123", "B) John1990", "C) T!g3r#Sky92@Blue", "D) 123456" },
                CorrectAnswer = "C",
                Explanation = "T!g3r#Sky92@Blue is strong because it uses uppercase, lowercase, numbers and symbols — making it extremely difficult to crack."
            },
            new QuizQuestion
            {
                Type = "multiple",
                Question = "What does HTTPS in a website address mean?",
                Options = new List<string> { "A) The site is fast", "B) The connection is encrypted and secure", "C) The site is owned by the government", "D) The site has no ads" },
                CorrectAnswer = "B",
                Explanation = "HTTPS means the data between your browser and the website is encrypted, protecting it from interception."
            },
            new QuizQuestion
            {
                Type = "multiple",
                Question = "What is phishing?",
                Options = new List<string> { "A) A type of firewall", "B) A fishing game on the internet", "C) A cyberattack that tricks you into revealing personal information", "D) A method of backing up data" },
                CorrectAnswer = "C",
                Explanation = "Phishing is when attackers impersonate trusted organisations to steal your credentials or personal information."
            },
            new QuizQuestion
            {
                Type = "multiple",
                Question = "What is two-factor authentication (2FA)?",
                Options = new List<string> { "A) Using two different browsers", "B) A second layer of security beyond just a password", "C) Having two email accounts", "D) Logging in from two devices" },
                CorrectAnswer = "B",
                Explanation = "2FA requires a second verification step — like a code sent to your phone — making it much harder for attackers to access your account even if they have your password."
            },
            new QuizQuestion
            {
                Type = "multiple",
                Question = "Which of these is a sign that your device may have malware?",
                Options = new List<string> { "A) Your device runs faster than usual", "B) Pop-up ads appear constantly even without a browser open", "C) Your wallpaper changes to something you chose", "D) Your battery lasts longer" },
                CorrectAnswer = "B",
                Explanation = "Constant pop-up ads, slow performance and programs opening on their own are common signs of a malware infection."
            },
            new QuizQuestion
            {
                Type = "multiple",
                Question = "What is ransomware?",
                Options = new List<string> { "A) Software that speeds up your computer", "B) A type of antivirus", "C) Malware that encrypts your files and demands payment", "D) A backup tool" },
                CorrectAnswer = "C",
                Explanation = "Ransomware locks your files and demands cryptocurrency as payment. You should never pay — restore from backups instead."
            },
            new QuizQuestion
            {
                Type = "multiple",
                Question = "What is the 3-2-1 backup rule?",
                Options = new List<string> { "A) Back up 3 times a day, 2 days a week, 1 month", "B) 3 copies of data, on 2 different media, with 1 copy offsite", "C) 3 passwords, 2 devices, 1 cloud account", "D) Back up every 3 hours" },
                CorrectAnswer = "B",
                Explanation = "The 3-2-1 rule is the gold standard for backups — 3 copies, 2 different storage types, 1 stored offsite or offline."
            },
            new QuizQuestion
            {
                Type = "multiple",
                Question = "What should you do before connecting to public Wi-Fi?",
                Options = new List<string> { "A) Nothing — public Wi-Fi is always safe", "B) Share your location with friends", "C) Use a VPN to encrypt your connection", "D) Disable your antivirus to speed things up" },
                CorrectAnswer = "C",
                Explanation = "A VPN encrypts your internet traffic on public Wi-Fi, preventing attackers from intercepting your data."
            },
            new QuizQuestion
            {
                Type = "multiple",
                Question = "What does a firewall do?",
                Options = new List<string> { "A) Speeds up your internet connection", "B) Monitors and filters network traffic based on security rules", "C) Backs up your files automatically", "D) Removes viruses from your device" },
                CorrectAnswer = "B",
                Explanation = "A firewall acts as a gatekeeper, blocking unauthorised network traffic while allowing legitimate communication."
            },
            new QuizQuestion
            {
                Type = "multiple",
                Question = "Which of the following is an example of social engineering?",
                Options = new List<string> { "A) Installing antivirus software", "B) A hacker calling you pretending to be IT support to get your password", "C) Updating your operating system", "D) Using a strong password" },
                CorrectAnswer = "B",
                Explanation = "Social engineering manipulates people rather than systems. Attackers use trust, urgency and authority to trick victims into handing over credentials."
            },
            new QuizQuestion
            {
                Type = "multiple",
                Question = "What is end-to-end encryption?",
                Options = new List<string> { "A) Encryption that only protects files on your hard drive", "B) Encryption where only the sender and recipient can read the messages", "C) A firewall feature", "D) A type of antivirus scan" },
                CorrectAnswer = "B",
                Explanation = "End-to-end encryption means only you and the person you are communicating with can read the messages — not even the service provider."
            },

            // --- TRUE / FALSE QUESTIONS ---
            new QuizQuestion
            {
                Type = "truefalse",
                Question = "True or False: It is safe to use the same password for all your accounts.",
                Options = new List<string> { "A) True", "B) False" },
                CorrectAnswer = "B",
                Explanation = "False! Using the same password everywhere means one breach exposes all your accounts. Always use unique passwords."
            },
            new QuizQuestion
            {
                Type = "truefalse",
                Question = "True or False: A padlock icon in your browser means the website is completely safe and trustworthy.",
                Options = new List<string> { "A) True", "B) False" },
                CorrectAnswer = "B",
                Explanation = "False! The padlock only means the connection is encrypted. Phishing sites can also use HTTPS and show a padlock. Always verify the full URL."
            },
            new QuizQuestion
            {
                Type = "truefalse",
                Question = "True or False: Software updates should be installed as soon as they are available.",
                Options = new List<string> { "A) True", "B) False" },
                CorrectAnswer = "A",
                Explanation = "True! Updates patch security vulnerabilities. Delaying updates leaves known gaps that attackers actively exploit."
            },
            new QuizQuestion
            {
                Type = "truefalse",
                Question = "True or False: You should pay a ransomware demand to get your files back.",
                Options = new List<string> { "A) True", "B) False" },
                CorrectAnswer = "B",
                Explanation = "False! Paying does not guarantee file recovery and funds criminal activity. Always restore from backups and check nomoreransom.org for free tools."
            },
            new QuizQuestion
            {
                Type = "truefalse",
                Question = "True or False: A VPN makes you completely anonymous and secure online.",
                Options = new List<string> { "A) True", "B) False" },
                CorrectAnswer = "B",
                Explanation = "False! A VPN encrypts your traffic but does not make you fully anonymous. You still need good passwords, 2FA and safe browsing habits."
            }
        };

        // Current question index during an active quiz session.
        public static int CurrentQuestionIndex { get; private set; } = 0;
        // Number of correct answers in the current session.
        public static int Score { get; private set; } = 0;
        // Whether a quiz is currently in progress.
        public static bool IsActive { get; private set; } = false;
        // The shuffled list of questions for the current session.
        private static List<QuizQuestion> _sessionQuestions = new List<QuizQuestion>();

        // Start a new quiz session. Shuffles questions and resets score.
        public static QuizQuestion StartQuiz()
        {
            IsActive = true;
            CurrentQuestionIndex = 0;
            Score = 0;
            // Shuffle questions for variety each session
            _sessionQuestions = new List<QuizQuestion>(AllQuestions);
            var rng = new Random();
            for (int i = _sessionQuestions.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                var temp = _sessionQuestions[i];
                _sessionQuestions[i] = _sessionQuestions[j];
                _sessionQuestions[j] = temp;
            }
            return _sessionQuestions[0];
        }

        // Get the current question without advancing.
        public static QuizQuestion GetCurrentQuestion()
        {
            if (!IsActive || CurrentQuestionIndex >= _sessionQuestions.Count) return null;
            return _sessionQuestions[CurrentQuestionIndex];
        }

        // Submit an answer. Returns true if correct.
        // Advances to the next question automatically.
        public static bool SubmitAnswer(string answerLetter)
        {
            if (!IsActive || CurrentQuestionIndex >= _sessionQuestions.Count) return false;
            var q = _sessionQuestions[CurrentQuestionIndex];
            bool correct = string.Equals(answerLetter.Trim().ToUpper(), q.CorrectAnswer.Trim().ToUpper());
            if (correct) Score++;
            CurrentQuestionIndex++;
            if (CurrentQuestionIndex >= _sessionQuestions.Count) IsActive = false;
            return correct;
        }

        // Check if there are more questions remaining.
        public static bool HasNextQuestion()
        {
            return IsActive && CurrentQuestionIndex < _sessionQuestions.Count;
        }

        // Get the total number of questions in the session.
        public static int TotalQuestions => _sessionQuestions.Count;

        // Get a feedback message based on the final score.
        public static string GetFinalFeedback()
        {
            double percent = TotalQuestions > 0 ? (double)Score / TotalQuestions * 100 : 0;
            if (percent == 100)
                return $"🏆 Perfect score! {Score}/{TotalQuestions} — You are a true cybersecurity expert! Outstanding work!";
            if (percent >= 80)
                return $"🌟 Great job! {Score}/{TotalQuestions} — You have a strong understanding of cybersecurity. Keep it up!";
            if (percent >= 60)
                return $"👍 Good effort! {Score}/{TotalQuestions} — You know the basics well. Review the topics you missed to strengthen your knowledge.";
            if (percent >= 40)
                return $"📚 Keep learning! {Score}/{TotalQuestions} — You are on the right track. Explore the topic buttons on the left to improve.";
            return $"💪 Don't give up! {Score}/{TotalQuestions} — Cybersecurity takes practice. Read through the topics and try again — you will improve!";
        }

        // Reset the quiz completely.
        public static void Reset()
        {
            IsActive = false;
            CurrentQuestionIndex = 0;
            Score = 0;
            _sessionQuestions.Clear();
        }
    }
}
