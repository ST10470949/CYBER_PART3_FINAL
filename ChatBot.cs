using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurity_Part2._2
{
    public static class ChatBot
    {
        // simple in-memory memories (session-only)
        private static string? memoryName = null;
        private static string? memoryMood = null;
        private static List<string> memoryPreferences = new List<string>();
        // store a single favourite cybersecurity topic (session-only)
        private static string? memoryFavoriteTopic = null;

        // Main entry for getting a response from the chatbot engine
        public static string GetResponse(string input)
        {
            // Ensure input is non-null and prepare helpers used throughout
            input ??= string.Empty;
            var user = string.IsNullOrWhiteSpace(UserManager.CurrentUserName) ? "User" : UserManager.CurrentUserName;
            if (string.IsNullOrWhiteSpace(input)) return "Please type a question or select a topic.";
            var lower = (input ?? string.Empty).ToLower();

            // ═══════════════════════════════════════════════════════
            // TASK MANAGEMENT COMMANDS
            // ═══════════════════════════════════════════════════════

            // Show all tasks
            if (lower.Contains("show tasks") || lower.Contains("my tasks") || lower.Contains("view tasks") || lower.Contains("list tasks") || lower.Contains("what are my tasks"))
            {
                ActivityLog.Add("👁", "User viewed their task list");
                // Also save this action to the database
                DatabaseManager.SaveLogEntry("👁", "User viewed their task list");
                return TaskManager.GetFormattedTaskList();
            }

            // Complete a task by number
            if ((lower.Contains("complete task") || lower.Contains("mark task") || lower.Contains("done task") || lower.Contains("finish task")))
            {
                // try to extract a number from the input
                var words = lower.Split(' ');
                foreach (var w in words)
                {
                    if (int.TryParse(w, out int tid))
                    {
                        // Complete in memory
                        bool memDone = TaskManager.CompleteTask(tid);
                        // Also complete in database
                        DatabaseManager.MarkTaskComplete(tid);
                        if (memDone)
                            return $"✅ Great work, {user}! Task {tid} has been marked as complete. Keep up the good cybersecurity habits!";
                        else
                            return $"I could not find task {tid}. Type 'show tasks' to see your current task list.";
                    }
                }
                return $"Please tell me which task number to complete, {user}. For example: 'complete task 1'. Type 'show tasks' to see your list.";
            }

            // Delete a task by number
            if (lower.Contains("delete task") || lower.Contains("remove task"))
            {
                var words = lower.Split(' ');
                foreach (var w in words)
                {
                    if (int.TryParse(w, out int tid))
                    {
                        // Delete from memory
                        bool memDel = TaskManager.DeleteTask(tid);
                        // Also delete from database
                        DatabaseManager.DeleteTask(tid);
                        if (memDel)
                            return $"🗑 Task {tid} has been deleted, {user}.";
                        else
                            return $"I could not find task {tid}. Type 'show tasks' to see your current task list.";
                    }
                }
                return $"Please tell me which task number to delete. For example: 'delete task 2'. Type 'show tasks' to see your list.";
            }

            // Add a task — NLP handles many phrasings
            if (lower.Contains("add task") || lower.Contains("create task") || lower.Contains("new task") ||
                lower.Contains("set task") || lower.Contains("add a task") || lower.Contains("create a task") ||
                lower.Contains("add reminder") || lower.Contains("set reminder") || lower.Contains("remind me to") ||
                lower.Contains("don't let me forget") || lower.Contains("dont let me forget") || lower.Contains("remember to"))
            {
                // Extract task title from the input after the command keyword
                string taskTitle = input.Trim();
                string[] taskPrefixes = new[] { "add task", "create task", "new task", "set task", "add a task",
                                     "create a task", "add reminder", "set reminder", "remind me to",
                                     "don't let me forget", "dont let me forget", "remember to" };
                foreach (var p in taskPrefixes)
                {
                    int idx = taskTitle.ToLower().IndexOf(p);
                    if (idx >= 0)
                    {
                        taskTitle = taskTitle.Substring(idx + p.Length).Trim();
                        break;
                    }
                }

                // Extract reminder if mentioned e.g. "in 3 days" or "tomorrow"
                string? reminder = null;
                string[] reminderKeywords = new[] { "in 1 day", "in 2 days", "in 3 days", "in 4 days", "in 5 days",
                                         "in 6 days", "in 7 days", "in a week", "tomorrow", "next week",
                                         "in one day", "in two days", "in three days" };
                foreach (var rk in reminderKeywords)
                {
                    if (lower.Contains(rk))
                    {
                        reminder = rk;
                        taskTitle = taskTitle.Replace(rk, "").Trim().TrimEnd(',').Trim();
                        break;
                    }
                }

                if (string.IsNullOrWhiteSpace(taskTitle))
                    taskTitle = "General cybersecurity task";

                // Add to in-memory task list (TaskManager also saves to DB and JSON)
                var newTask = TaskManager.AddTask(taskTitle, "", reminder);

                // Also save the task to the database for persistence
                DatabaseManager.SaveLogEntry("📝", $"Task saved to DB: '{newTask.Title}'");

                string reminderMsg = string.IsNullOrEmpty(reminder) ? "" : $" I have set a reminder for {reminder}.";
                return $"✅ Task added, {user}! '{newTask.Title}'.{reminderMsg} " +
                       $"Would you like to set a description? You can also type 'show tasks' to see all your tasks.";
            }

            // ═══════════════════════════════════════════════════════
            // ACTIVITY LOG COMMANDS
            // ═══════════════════════════════════════════════════════

            // Show MORE of the activity log (full history)
            if (lower.Contains("show more log") || lower.Contains("show full log") || lower.Contains("full activity") ||
                lower.Contains("show all log") || lower.Contains("more log") || lower.Contains("full history") ||
                lower.Contains("show more history") || lower.Contains("all activity"))
            {
                ActivityLog.Add("📋", "User viewed the full activity log");
                DatabaseManager.SaveLogEntry("📋", "User viewed the full activity log");
                return ActivityLog.GetFullFormattedLog();
            }

            // Show the activity log (default — last 5 entries with show-more hint)
            if (lower.Contains("activity log") || lower.Contains("show log") || lower.Contains("what have you done") ||
                lower.Contains("what have i done") || lower.Contains("recent actions") || lower.Contains("show history") ||
                lower.Contains("what did i do") || lower.Contains("my history"))
            {
                ActivityLog.Add("📋", "User viewed the activity log");
                DatabaseManager.SaveLogEntry("📋", "User viewed the activity log");
                return ActivityLog.GetFormattedLog();
            }

            // Show quiz history from database
            if (lower.Contains("quiz history") || lower.Contains("my quiz results") || lower.Contains("past quiz") || lower.Contains("previous quiz"))
            {
                return DatabaseManager.GetQuizHistory();
            }

            // ═══════════════════════════════════════════════════════
            // QUIZ COMMANDS — NLP handles many ways to start the quiz
            // ═══════════════════════════════════════════════════════

            // Answer a quiz question that is currently active
            if (QuizManager.IsActive)
            {
                // Check if user typed A, B, C, D or True/False as an answer
                string trimmed = lower.Trim();
                bool isAnswer = trimmed == "a" || trimmed == "b" || trimmed == "c" || trimmed == "d" ||
                                trimmed == "true" || trimmed == "false" || trimmed == "1" || trimmed == "2" ||
                                trimmed == "3" || trimmed == "4";

                // Also accept "A)" or "B) ..." format
                if (!isAnswer && trimmed.Length >= 1)
                {
                    char first = char.ToUpper(trimmed[0]);
                    isAnswer = first >= 'A' && first <= 'D';
                }

                if (isAnswer)
                {
                    // Map "true" to "A" and "false" to "B" for true/false questions
                    string answerLetter = trimmed.ToUpper();
                    if (answerLetter == "TRUE") answerLetter = "A";
                    if (answerLetter == "FALSE") answerLetter = "B";
                    // Take only first character
                    answerLetter = answerLetter.Substring(0, 1);

                    var currentQ = QuizManager.GetCurrentQuestion();
                    bool correct = QuizManager.SubmitAnswer(answerLetter);
                    string resultMsg = correct
                        ? $"✅ Correct! Well done, {user}!\n💡 {currentQ.Explanation}"
                        : $"❌ Not quite. The correct answer was {currentQ.CorrectAnswer}.\n💡 {currentQ.Explanation}";

                    ActivityLog.Add(correct ? "✅" : "❌", $"Quiz answer: {answerLetter} — {(correct ? "Correct" : "Incorrect")}");
                    DatabaseManager.SaveLogEntry(correct ? "✅" : "❌", $"Quiz answer: {answerLetter} — {(correct ? "Correct" : "Incorrect")}");

                    if (QuizManager.HasNextQuestion())
                    {
                        var next = QuizManager.GetCurrentQuestion();
                        string optionsText = string.Join("\n", next.Options);
                        return $"{resultMsg}\n\n" +
                               $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                               $"Question {QuizManager.CurrentQuestionIndex} of {QuizManager.TotalQuestions}:\n\n" +
                               $"❓ {next.Question}\n\n{optionsText}\n\n" +
                               $"Type A, B, C or D to answer.";
                    }
                    else
                    {
                        // Quiz finished — save result to database
                        string feedback = QuizManager.GetFinalFeedback();
                        ActivityLog.Add("🏆", $"Quiz completed — Score: {QuizManager.Score}/{QuizManager.TotalQuestions}");
                        DatabaseManager.SaveLogEntry("🏆", $"Quiz completed — Score: {QuizManager.Score}/{QuizManager.TotalQuestions}");
                        // Save the quiz result to the database for history tracking
                        DatabaseManager.SaveQuizResult(QuizManager.Score, QuizManager.TotalQuestions);
                        QuizManager.Reset();
                        return $"{resultMsg}\n\n" +
                               $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                               $"🎉 Quiz Complete!\n\n{feedback}\n\n" +
                               $"Type 'start quiz' to play again or select a topic on the left to keep learning!";
                    }
                }
            }

            // Start the quiz — NLP recognises many ways to ask
            if (lower.Contains("start quiz") || lower.Contains("begin quiz") || lower.Contains("take quiz") ||
                lower.Contains("play quiz") || lower.Contains("quiz time") || lower.Contains("test me") ||
                lower.Contains("quiz me") || lower.Contains("test my knowledge") || lower.Contains("i want to take a quiz") ||
                lower.Contains("let's do a quiz") || lower.Contains("lets do a quiz") || lower.Contains("do a quiz") ||
                lower.Contains("start the quiz") || lower.Contains("begin the quiz"))
            {
                var firstQ = QuizManager.StartQuiz();
                string optionsText = string.Join("\n", firstQ.Options);
                ActivityLog.Add("🎮", "Quiz session started");
                DatabaseManager.SaveLogEntry("🎮", "Quiz session started");
                return $"🎮 Welcome to the Cybersecurity Quiz, {user}!\n\n" +
                       $"There are {QuizManager.TotalQuestions} questions. Type A, B, C or D to answer each one.\n\n" +
                       $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                       $"Question 1 of {QuizManager.TotalQuestions}:\n\n" +
                       $"❓ {firstQ.Question}\n\n{optionsText}\n\n" +
                       $"Type A, B, C or D to answer.";
            }

            // Stop the quiz mid-session
            if (lower.Contains("stop quiz") || lower.Contains("exit quiz") || lower.Contains("quit quiz") || lower.Contains("end quiz"))
            {
                if (QuizManager.IsActive)
                {
                    int score = QuizManager.Score;
                    int total = QuizManager.CurrentQuestionIndex;
                    QuizManager.Reset();
                    ActivityLog.Add("🛑", $"Quiz stopped early — Score so far: {score}/{total}");
                    DatabaseManager.SaveLogEntry("🛑", $"Quiz stopped early — Score so far: {score}/{total}");
                    return $"Quiz stopped, {user}. You answered {total} questions and got {score} correct. Type 'start quiz' whenever you want to try again!";
                }
                return $"There is no active quiz to stop, {user}. Type 'start quiz' to begin one!";
            }

            // Quick help
            if (lower.Contains("what can i ask") || lower.Contains("what can i ask you") || lower.Contains("what can i ask about") || lower.Contains("what can you answer"))
            {
                return $"{user}, You can ask me about: " + string.Join(", ", TopicManager.Topics.Keys);
            }

            // First process memory commands / personal phrases (priority #1)
            var mem = ProcessMemoryInput(input, lower);
            if (!string.IsNullOrEmpty(mem))
            {
                // Memory responses have top priority and should be returned immediately
                return mem;
            }

            // These are checked before topic detection so they feel natural

            // CONVERSATION 1: Interest in a specific topic combination
            if ((lower.Contains("interested in") || lower.Contains("want to learn") || lower.Contains("want to know about") || lower.Contains("curious about") || lower.Contains("tell me about")) && lower.Contains("malware") && lower.Contains("phishing"))
            {
                return $"{user}, great choice! Both malware and phishing are among the most common cyber threats today. " +
                       $"Phishing tricks you into revealing sensitive information through fake emails or websites, while malware is malicious software that infects your device to steal data or cause damage. " +
                       $"A great tip: never click links in unexpected emails, and always keep your antivirus software updated. Would you like me to go deeper into either topic?";
            }

            // CONVERSATION 2: Interest in a single topic casually
            if ((lower.Contains("interested in") || lower.Contains("want to learn") || lower.Contains("curious about") || lower.Contains("tell me more about")) && lower.Contains("malware"))
            {
                return $"{user}, malware is a fascinating and important topic! 🛡 Malware stands for malicious software and includes viruses, trojans, spyware, ransomware and worms. " +
                       $"Each type works differently — for example, a virus attaches itself to files and spreads, while spyware silently monitors your activity. " +
                       $"The best defence is a combination of good antivirus software, regular updates and being careful about what you download. Want to know about a specific type of malware?";
            }

            // CONVERSATION 3: Interest in phishing casually
            if ((lower.Contains("interested in") || lower.Contains("want to learn") || lower.Contains("curious about") || lower.Contains("tell me more about")) && lower.Contains("phishing"))
            {
                return $"{user}, phishing is one of the sneakiest cyber threats out there! 🎣 Attackers craft convincing fake emails, messages or websites that look like they come from trusted sources like your bank or a popular service. " +
                       $"They want you to click a link or enter your personal details without realising it is fake. " +
                       $"Always check the sender's email address carefully, look for spelling mistakes, and never enter your password on a site you reached through an email link. Stay sharp!";
            }

            // CONVERSATION 4: How are you / casual greeting
            if (lower.Contains("how are you") || lower.Contains("how are you doing") || lower.Contains("how is it going") || lower.Contains("how do you do"))
            {
                return $"I am doing great, {user}, thank you for asking! 😊 I am always ready to help you stay safe online. " +
                       $"Is there anything about cybersecurity you would like to explore today?";
            }

            // CONVERSATION 5: What is your favourite topic
            if (lower.Contains("favourite topic") || lower.Contains("favorite topic") || lower.Contains("what do you like") || lower.Contains("what topics do you enjoy"))
            {
                return $"Great question, {user}! If I had to pick, I would say password security is my favourite topic because it is the foundation of everything. " +
                       $"A strong, unique password is your first line of defence against almost every cyber threat. What topic interests you the most?";
            }

            // CONVERSATION 6: Am I safe online
            if (lower.Contains("am i safe") || lower.Contains("is my device safe") || lower.Contains("am i protected") || lower.Contains("how safe am i"))
            {
                return $"{user}, that is a very important question! Your safety online depends on several things: " +
                       $"whether you use strong passwords, whether you have two-factor authentication enabled, whether your software is up to date, and whether you are careful about the links you click. " +
                       $"If you do all of these things consistently, you are in a much stronger position than most people. Would you like tips on improving any of these areas?";
            }

            // CONVERSATION 7: What is the biggest cyber threat
            if (lower.Contains("biggest threat") || lower.Contains("most dangerous") || lower.Contains("worst cyber") || lower.Contains("most common threat") || lower.Contains("biggest risk"))
            {
                return $"{user}, according to cybersecurity experts, phishing and ransomware are consistently ranked as the biggest threats to everyday users and organisations alike. " +
                       $"Phishing is the entry point for many attacks, while ransomware can cause devastating financial and data loss. " +
                       $"Staying educated about these threats is one of the most powerful things you can do. Would you like to learn more about either of them?";
            }

            // CONVERSATION 8: Can I get hacked
            if (lower.Contains("can i get hacked") || lower.Contains("will i get hacked") || lower.Contains("am i being hacked") || lower.Contains("could i be hacked"))
            {
                return $"{user}, honestly — yes, anyone can be targeted by hackers, but your risk is greatly reduced when you practise good cybersecurity habits. " +
                       $"Using strong unique passwords, enabling 2FA, keeping software updated and avoiding suspicious links are your best defences. " +
                       $"Think of cybersecurity like locking your front door — you can never be 100% guaranteed safe, but good habits make you a much harder target. 🔐";
            }

            // CONVERSATION 9: What should I do first to stay safe
            if (lower.Contains("where do i start") || lower.Contains("what should i do first") || lower.Contains("how do i start") || lower.Contains("getting started") && lower.Contains("safe"))
            {
                return $"Great question, {user}! Here is a simple starting checklist: " +
                       $"1️⃣ Use a strong, unique password for every account. " +
                       $"2️⃣ Enable two-factor authentication wherever possible. " +
                       $"3️⃣ Keep your operating system and apps updated. " +
                       $"4️⃣ Be cautious about clicking links in emails or messages. " +
                       $"5️⃣ Back up your important data regularly. " +
                       $"Start with these five and you will already be far safer than most people online!";
            }

            // CONVERSATION 10: Is social media safe
            if (lower.Contains("social media") && (lower.Contains("safe") || lower.Contains("dangerous") || lower.Contains("risk") || lower.Contains("secure")))
            {
                return $"{user}, social media can be a significant cybersecurity risk if not used carefully. " +
                       $"Oversharing personal information gives attackers the tools they need for social engineering attacks. " +
                       $"Make sure your privacy settings are set to the most restrictive options, avoid sharing your location in real time, " +
                       $"use a strong unique password for each platform and enable two-factor authentication. Stay smart on social media! 📱";
            }

            // CONVERSATION 11: What is a VPN
            if (lower.Contains("what is a vpn") || lower.Contains("vpn") && (lower.Contains("what") || lower.Contains("how") || lower.Contains("should i use") || lower.Contains("do i need")))
            {
                return $"{user}, a VPN — Virtual Private Network — is a tool that encrypts your internet connection and hides your IP address. " +
                       $"It is especially useful when using public Wi-Fi, as it prevents attackers from intercepting your data. " +
                       $"Think of it as a secure tunnel between your device and the internet. " +
                       $"While a VPN is a great privacy tool, it is not a complete security solution — you still need good passwords and safe browsing habits!";
            }

            // CONVERSATION 12: What is a data breach
            if (lower.Contains("data breach") || lower.Contains("my data was leaked") || lower.Contains("my information was stolen") || lower.Contains("got breached"))
            {
                return $"{user}, a data breach happens when unauthorised people gain access to confidential information — like usernames, passwords or financial details. " +
                       $"If you think your data has been breached, act quickly: change your passwords immediately, enable 2FA on affected accounts, " +
                       $"monitor your bank statements for suspicious activity and consider using a service like HaveIBeenPwned.com to check if your email was exposed. " +
                       $"Speed is critical when dealing with a breach! 🚨";
            }

            // CONVERSATION 13: Compliment the bot
            if (lower.Contains("you are great") || lower.Contains("you are amazing") || lower.Contains("you are helpful") || lower.Contains("love this") || lower.Contains("this is great") || lower.Contains("well done"))
            {
                return $"Thank you so much, {user}! 😊 That really means a lot. My goal is to make cybersecurity easy to understand and accessible for everyone. " +
                       $"Is there anything else I can help you with today?";
            }

            // CONVERSATION 14: I got a suspicious email
            if ((lower.Contains("suspicious email") || lower.Contains("strange email") || lower.Contains("weird email") || lower.Contains("got an email")) && (lower.Contains("what do i do") || lower.Contains("help") || lower.Contains("suspicious") || lower.Contains("strange")))
            {
                return $"{user}, do not click any links or download any attachments from that email! Here is what to do: " +
                       $"1️⃣ Do not reply to the sender. " +
                       $"2️⃣ Check the sender's email address carefully for misspellings. " +
                       $"3️⃣ Report it as phishing/spam in your email client. " +
                       $"4️⃣ If it claims to be from your bank or a service, go directly to their official website — do not use links in the email. " +
                       $"5️⃣ Delete the email after reporting it. Trust your instincts — if it feels suspicious, it probably is! 🎣";
            }

            // CONVERSATION 15: Someone is asking for my password
            if ((lower.Contains("asking for my password") || lower.Contains("wants my password") || lower.Contains("asked for my password") || lower.Contains("give my password")))
            {
                return $"Stop right there, {user}! 🚨 No legitimate company, bank, IT support team or government agency will ever ask you for your password. " +
                       $"This is a classic social engineering tactic. Never share your password with anyone — not even someone claiming to be from technical support. " +
                       $"If you already shared it, change it immediately and enable two-factor authentication on that account right away!";
            }

            // CONVERSATION 16: What is two factor authentication simply
            if ((lower.Contains("explain") || lower.Contains("simple") || lower.Contains("simply") || lower.Contains("easy")) && (lower.Contains("two factor") || lower.Contains("2fa") || lower.Contains("two-factor")))
            {
                return $"Sure {user}! Think of 2FA like your front door having two locks instead of one. 🔐 " +
                       $"The first lock is your password — something you know. " +
                       $"The second lock is a code sent to your phone or generated by an app — something you have. " +
                       $"Even if a hacker steals your password, they still cannot get in without that second code. " +
                       $"It is one of the easiest and most effective ways to protect your accounts. Enable it wherever you can!";
            }

            // CONVERSATION 17: My phone got hacked
            if ((lower.Contains("phone") || lower.Contains("mobile")) && (lower.Contains("hacked") || lower.Contains("compromised") || lower.Contains("virus") || lower.Contains("infected")))
            {
                return $"{user}, if you suspect your phone has been compromised, here is what to do immediately: " +
                       $"1️⃣ Disconnect from Wi-Fi and mobile data to stop any ongoing data theft. " +
                       $"2️⃣ Run a reputable mobile antivirus scan. " +
                       $"3️⃣ Change passwords for your important accounts from a different, safe device. " +
                       $"4️⃣ Check which apps have been installed recently and remove anything suspicious. " +
                       $"5️⃣ If the problem persists, consider a factory reset as a last resort — but back up important data first if you can safely do so. 📱🚨";
            }

            // CONVERSATION 18: How do hackers think
            if (lower.Contains("how do hackers think") || lower.Contains("how do hackers work") || lower.Contains("hacker mindset") || lower.Contains("how hackers operate") || lower.Contains("what do hackers do"))
            {
                return $"Really interesting question, {user}! 🧠 Hackers typically look for the easiest path of least resistance. " +
                       $"They scan for weak passwords, unpatched software, careless users and poorly configured systems. " +
                       $"They often do reconnaissance first — gathering information about their target from social media and public records. " +
                       $"Understanding how attackers think is actually one of the best ways to defend yourself. " +
                       $"The key insight: most attacks succeed not because of technical genius, but because of human error. Stay alert!";
            }

            // CONVERSATION 19: Is it safe to use public wifi
            if ((lower.Contains("public wifi") || lower.Contains("public wi-fi") || lower.Contains("free wifi") || lower.Contains("coffee shop wifi") || lower.Contains("airport wifi")) && (lower.Contains("safe") || lower.Contains("okay") || lower.Contains("use") || lower.Contains("connect")))
            {
                return $"{user}, public Wi-Fi is convenient but risky! ⚠ Attackers on the same network can potentially intercept your data using a technique called a man-in-the-middle attack. " +
                       $"Here is how to stay safer: use a VPN to encrypt your traffic, avoid logging into banking or sensitive accounts, " +
                       $"make sure websites use HTTPS before entering any information, and turn off automatic Wi-Fi connection on your device. " +
                       $"Better safe than sorry when it comes to public networks!";
            }

            // CONVERSATION 20: What is ransomware simply explained
            if ((lower.Contains("explain") || lower.Contains("simple") || lower.Contains("simply") || lower.Contains("easy") || lower.Contains("what is")) && lower.Contains("ransomware"))
            {
                return $"Of course, {user}! Imagine coming home and finding all your belongings locked in a box, and someone slipped a note under your door saying 'Pay me to get the key.' 🔒 " +
                       $"That is exactly what ransomware does to your files. It is malware that encrypts all your data and demands payment — usually cryptocurrency — to unlock it. " +
                       $"The best protection is regular backups stored offline, so even if ransomware hits, you can restore your files without paying. Never pay the ransom if you can avoid it!";
            }

            // CONVERSATION 21: Motivational / encouragement about learning cybersecurity
            if (lower.Contains("is it hard") || lower.Contains("cybersecurity is difficult") || lower.Contains("too complicated") || lower.Contains("hard to understand") || lower.Contains("difficult to learn"))
            {
                return $"{user}, I completely understand how overwhelming cybersecurity can feel at first! 💪 " +
                       $"But here is the good news: you do not need to be a technical expert to stay safe online. " +
                       $"Most attacks succeed because of simple mistakes that are easy to avoid once you know about them — like weak passwords or clicking suspicious links. " +
                       $"You are already doing the right thing by learning. Every topic you explore here makes you significantly safer. Keep going!";
            }

            // CONVERSATION 22: What happens if I click a phishing link
            if ((lower.Contains("clicked") || lower.Contains("i clicked") || lower.Contains("accidentally clicked") || lower.Contains("what happens if i click")) && (lower.Contains("phishing") || lower.Contains("suspicious link") || lower.Contains("bad link") || lower.Contains("weird link")))
            {
                return $"{user}, do not panic — but act quickly! 🚨 If you clicked a phishing link: " +
                       $"1️⃣ Disconnect from the internet immediately. " +
                       $"2️⃣ Run a full antivirus scan on your device. " +
                       $"3️⃣ Change the passwords of any accounts you may have entered details for. " +
                       $"4️⃣ Enable 2FA on those accounts. " +
                       $"5️⃣ Check your bank statements for unauthorised transactions. " +
                       $"6️⃣ Report the phishing link to your email provider or IT team if at work. " +
                       $"Quick action can significantly limit the damage. You've got this!";
            }

            // CONVERSATION 23: Should I use the same password everywhere
            if ((lower.Contains("same password") || lower.Contains("reuse password") || lower.Contains("use one password") || lower.Contains("one password for everything")))
            {
                return $"Please do not, {user}! 🚫 Using the same password everywhere is one of the most dangerous habits online. " +
                       $"If just one site you use gets breached, attackers will try that same password on your email, bank and social media accounts — a technique called credential stuffing. " +
                       $"Use a unique password for every account. I know that sounds hard, but a password manager makes it effortless. " +
                       $"It stores all your passwords securely so you only need to remember one master password. Highly recommended!";
            }

            // numeric shortcuts map to topics
            if (int.TryParse(lower, out int n))
            {
                var keys = TopicManager.Topics.Keys.ToList();
                if (n >= 1 && n <= keys.Count)
                {
                    var key = keys[n - 1];
                    return user + ", " + TopicManager.Topics[key];
                }
            }

            // direct topic name match (highest priority) - collect all direct matches in the full input
            var matched = new List<string>();
            foreach (var kv in TopicManager.Topics)
            {
                if (lower.Contains(kv.Key) && !matched.Contains(kv.Key)) matched.Add(kv.Key);
            }

            // Split input into segments by common separators and conjunctions so multiple topics/questions are detected
            var segments = System.Text.RegularExpressions.Regex.Split(lower, "\\?|!|;|\\.|,|\\band\\b|\\balso\\b|\\bthen\\b|\\bplus\\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                .Select(s => s.Trim()).Where(s => s.Length > 0).ToList();

            foreach (var seg in segments)
            {
                // tokens per segment
                var segTokens = new List<string>();
                foreach (var t in System.Text.RegularExpressions.Regex.Split(seg, "[^a-z0-9]+"))
                {
                    if (!string.IsNullOrEmpty(t)) segTokens.Add(t);
                }

                // check direct phrase match within the segment first
                foreach (var kv in TopicManager.Topics)
                {
                    if (seg.Contains(kv.Key) && !matched.Contains(kv.Key)) matched.Add(kv.Key);
                }

                // token-level matching
                foreach (var kv in TopicManager.Topics)
                {
                    if (matched.Contains(kv.Key)) continue;
                    var keyTokens = kv.Key.Split(' ');
                    foreach (var kt in keyTokens)
                    {
                        if (segTokens.Contains(kt)) { matched.Add(kv.Key); break; }
                    }
                }
            }

            // fuzzy suggestions across full input tokens as a last attempt to catch misspellings
            var fullTokens = new List<string>();
            foreach (var t in System.Text.RegularExpressions.Regex.Split(lower, "[^a-z0-9]+")) if (!string.IsNullOrEmpty(t)) fullTokens.Add(t);
            if (matched.Count == 0)
            {
                var suggestions = FuzzyMatcher.GetSuggestions(TopicManager.Topics.Keys, fullTokens);
                foreach (var s in suggestions)
                {
                    if (!matched.Contains(s)) matched.Add(s);
                }
            }

            if (matched.Count > 0)
            {
                // build combined response for all matched topics, preserving order and avoiding duplicates
                var parts = new List<string>();
                foreach (var key in matched)
                {
                    if (TopicManager.Topics.TryGetValue(key, out var text)) parts.Add(text);
                }

                // include memory response at top if it exists and is not redundant
                if (!string.IsNullOrEmpty(mem))
                {
                    parts.Insert(0, mem);
                }

                return user + ", " + string.Join("\n\n", parts);
            }

            // Sentiment detection: if user expresses an emotion, reply empathetically and give a tip
            var sentimentReply = DetectSentimentAndTip(lower);
            if (!string.IsNullOrEmpty(sentimentReply))
            {
                if (!string.IsNullOrEmpty(memoryFavoriteTopic))
                {
                    sentimentReply += "\n\n" + $"Since you are interested in {memoryFavoriteTopic}, remember to use secure passwords and avoid sharing personal information on public websites.";
                }
                var knownName = !string.IsNullOrEmpty(memoryName) ? memoryName : (!string.IsNullOrWhiteSpace(UserManager.CurrentUserName) ? UserManager.CurrentUserName : null);
                if (!string.IsNullOrEmpty(knownName)) sentimentReply = knownName + ", " + sentimentReply;
                return sentimentReply;
            }

            // Date/time detection: only trigger on explicit date/time queries
            if (IsDateQuestion(lower))
                return "Today's date is " + DateTime.Now.ToShortDateString() + ".";

            if (IsTimeQuestion(lower))
                return "The current time is " + DateTime.Now.ToShortTimeString() + ".";

            // Greetings and small talk
            if (lower.Contains("hello") || lower.Contains("hi") || lower.Contains("hey"))
                return "Hello! How can I help you with cybersecurity today?";

            if (lower.Contains("thank") || lower.Contains("thanks"))
                return "You're welcome — happy to help. Ask me anything about online safety.";

            if (lower.Contains("your name") || lower.Contains("who are you") || lower.Contains("what are you"))
                return "I am Cyber Times With Abo, your cybersecurity assistant. I can explain threats and give practical advice.";

            if (lower.Contains("purpose") || lower.Contains("what do you do") || lower.Contains("about you"))
                return "I help people learn about online risks and how to protect themselves — passwords, phishing, malware, privacy and more.";

            if (lower.Contains("weather"))
                return "I can't check live weather, but remember to avoid clicking unknown links in weather alerts — they can be phishing.";

            if (lower.Contains("bye") || lower.Contains("goodbye") || lower.Contains("see you"))
                return "Goodbye — stay safe online!";

            if (lower.Contains("help") || lower.Contains("how do i") || lower.Contains("how to"))
                return "Tell me the topic you need help with, for example 'passwords', 'phishing', or 'safe browsing'.";

            if (lower.Contains("joke") || lower.Contains("funny"))
                return "Why did the computer get cold? Because it left its Windows open. 😄";

            return "I'm not sure I understand. Can you try rephrasing? You can also type a topic name like 'phishing' or 'passwords', or use the sidebar buttons.";
        }

        // ProcessMemoryInput looks for phrases that set or request remembered values.
        // Returns a reply when a memory action is handled, otherwise null.
        public static string ProcessMemoryInput(string rawInput, string lower)
        {
            if (string.IsNullOrWhiteSpace(rawInput)) return null;

            // Set name (avoid misclassifying short emotion phrases like "I am scared")
            string[] namePrefixes = new[] { "my name is ", "call me " };
            foreach (var p in namePrefixes)
            {
                int idx = lower.IndexOf(p, StringComparison.Ordinal);
                if (idx == 0)
                {
                    string value = rawInput.Substring(p.Length).Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        memoryName = value;
                        // also update shared user store so other parts of app use the name
                        UserManager.CurrentUserName = memoryName;
                        return $"Nice to meet you, {memoryName}. I'll remember your name.";
                    }
                }
            }

            // Also accept "I am <name>" or "I'm <name>" but only when the remainder looks like a name (no emotion words)
            if (lower.StartsWith("i am ") || lower.StartsWith("i'm ") || lower.StartsWith("im "))
            {
                var candidate = rawInput.Substring(rawInput.IndexOf(' ') + 1).Trim();
                // simple heuristic: treat as name if it's 1-3 words and not an emotion keyword
                var tokens = candidate.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var emotionWords = new[] { "scared", "afraid", "anxious", "nervous", "unsafe", "sad", "happy", "upset", "angry" };
                if (tokens.Length > 0 && tokens.Length <= 3 && !tokens.Any(t => emotionWords.Contains(t.ToLower())))
                {
                    memoryName = candidate;
                    UserManager.CurrentUserName = memoryName;
                    return $"Nice to meet you, {memoryName}. I'll remember your name.";
                }
            }

            // Ask for name
            if (lower.Contains("what is my name") || lower.Contains("what's my name") || lower.Contains("do you know my name"))
            {
                if (!string.IsNullOrEmpty(memoryName)) return $"Your name is {memoryName}.";
                if (!string.IsNullOrEmpty(UserManager.CurrentUserName)) return $"Your name is {UserManager.CurrentUserName}.";
                return "I don't know your name yet. You can tell me by saying 'my name is ...'";
            }

            // Set mood
            string[] moodPrefixes = new[] { "my mood is ", "i feel ", "i'm feeling ", "i am feeling " };
            foreach (var p in moodPrefixes)
            {
                int idx = lower.IndexOf(p, StringComparison.Ordinal);
                if (idx == 0)
                {
                    string value = rawInput.Substring(p.Length).Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        memoryMood = value;
                        return $"Thanks for telling me — I have noted that you're feeling {memoryMood}.";
                    }
                }
            }

            // Ask for mood
            if (lower.Contains("what is my mood") || lower.Contains("how am i feeling") || lower.Contains("what am i feeling"))
            {
                if (!string.IsNullOrEmpty(memoryMood)) return $"You told me you're feeling {memoryMood}.";
                return "I don't know how you're feeling yet. You can tell me by saying 'I feel ...'";
            }

            // Preferences: set simple preferences
            string[] prefPrefixes = new[] { "i prefer ", "my preference is ", "i like " };
            foreach (var p in prefPrefixes)
            {
                int idx = lower.IndexOf(p, StringComparison.Ordinal);
                if (idx == 0)
                {
                    string value = rawInput.Substring(p.Length).Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        memoryPreferences.Add(value);
                        // If the preference looks like a cybersecurity topic, store as favorite topic for personalization
                        foreach (var k in TopicManager.Topics.Keys)
                        {
                            if (value.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                memoryFavoriteTopic = k;
                                break;
                            }
                        }
                        return $"Noted. I'll remember that you prefer {value}.";
                    }
                }
            }

            // Ask for preferences
            if (lower.Contains("what are my preferences") || lower.Contains("what do i prefer") || lower.Contains("what do i like"))
            {
                if (memoryPreferences.Count > 0) return "You told me you prefer: " + string.Join(", ", memoryPreferences) + ".";
                return "You haven't told me any preferences yet. Say 'I prefer ...' to set one.";
            }

            // Forget commands
            if (lower.Contains("forget my name") || lower.Contains("clear my name"))
            {
                memoryName = null;
                return "Okay, I have forgotten your name.";
            }

            if (lower.Contains("forget my mood") || lower.Contains("clear my mood"))
            {
                memoryMood = null;
                return "Okay, I have forgotten your mood.";
            }

            // not a memory-related input
            return null;
        }

        // Detect if the user is explicitly asking for the date
        private static bool IsDateQuestion(string lower)
        {
            if (string.IsNullOrWhiteSpace(lower)) return false;
            var patterns = new[] { "what is today's date", "what is today", "what is the date", "tell me the date", "today's date", "current date", "what date is it", "what's the date" };
            foreach (var p in patterns)
            {
                if (lower.Contains(p)) return true;
            }
            if (lower.Trim() == "date" || lower.Trim() == "today") return true;
            return false;
        }

        // Detect common sentiments from the user's input and return an empathetic reply with a cybersecurity tip.
        // Returns null when no sentiment is detected.
        private static string DetectSentimentAndTip(string lower)
        {
            if (string.IsNullOrWhiteSpace(lower)) return null;

            // Worried
            var worried = new[] { "scared", "afraid", "anxious", "nervous", "unsafe" };
            foreach (var w in worried) if (lower.Contains(w)) return "I understand that this situation can feel stressful. A good cybersecurity practice is to enable two-factor authentication and change your passwords regularly.";

            // Curious
            var curious = new[] { "wondering", "interested", "want to know", "how does" };
            foreach (var c in curious) if (lower.Contains(c)) return "That is a great question. Learning about cybersecurity is very important. A useful tip is to avoid clicking unknown links in emails or messages.";

            // Frustrated
            var frustrated = new[] { "annoyed", "confused", "don't understand", "dont understand", "do not understand" };
            foreach (var f in frustrated) if (lower.Contains(f)) return "I understand your frustration. Cybersecurity topics can sometimes be confusing. One useful tip is to always use strong and unique passwords for different accounts.";

            // Happy
            var happy = new[] { "great", "thanks", "helpful", "awesome", "love it" };
            foreach (var h in happy) if (lower.Contains(h)) return "I am glad you found it helpful. A good habit is to keep your software updated to stay protected from security threats.";

            return null;
        }

        // Detect if the user is explicitly asking for the time
        private static bool IsTimeQuestion(string lower)
        {
            if (string.IsNullOrWhiteSpace(lower)) return false;
            var patterns = new[] { "what time is it", "current time", "tell me the time", "what is the time", "time now", "what's the time" };
            foreach (var p in patterns)
            {
                if (lower.Contains(p)) return true;
            }
            if (lower.Trim() == "time") return true;
            return false;
        }
    }
}