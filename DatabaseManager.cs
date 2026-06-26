using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace CyberSecurity_Part2._2
{
    /// <summary>
    /// DatabaseManager handles all database operations for the CyberTimes chatbot.
    /// It uses SQLite via the Microsoft.Data.Sqlite package so no external MySQL
    /// server is required — the database is a single file stored next to the app.
    /// 
    /// To add the SQLite package:
    /// 1. Right-click your project in Solution Explorer
    /// 2. Click "Manage NuGet Packages"
    /// 3. Search for "Microsoft.Data.Sqlite"
    /// 4. Click Install
    /// </summary>
    public static class DatabaseManager
    {
        // ── Path to the SQLite database file ────────────────────────────────────
        // The file is stored in the same folder as the running executable.
        // It is created automatically on first run if it does not exist.
        private static readonly string DatabasePath =
            Path.Combine(AppContext.BaseDirectory, "CyberTimes.db");

        // ── Connection string used by every database operation ──────────────────
        private static string ConnectionString =>
            $"Data Source={DatabasePath}";

        // ════════════════════════════════════════════════════════════════════════
        // INITIALISATION
        // ════════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Initialise the database and create all required tables if they do not
        /// already exist. Call this once from MainWindow_Loaded.
        /// </summary>
        public static void Initialise()
        {
            try
            {
                using var connection = new Microsoft.Data.Sqlite.SqliteConnection(ConnectionString);
                connection.Open();

                // ── Tasks table ─────────────────────────────────────────────────
                // Stores cybersecurity tasks created by the user.
                var createTasks = connection.CreateCommand();
                createTasks.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Tasks (
                        Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserName    TEXT    NOT NULL,
                        Title       TEXT    NOT NULL,
                        Description TEXT    NOT NULL DEFAULT '',
                        Reminder    TEXT    NOT NULL DEFAULT '',
                        IsCompleted INTEGER NOT NULL DEFAULT 0,
                        CreatedAt   TEXT    NOT NULL
                    );";
                createTasks.ExecuteNonQuery();

                // ── QuizResults table ───────────────────────────────────────────
                // Stores a record of every completed quiz session.
                var createQuiz = connection.CreateCommand();
                createQuiz.CommandText = @"
                    CREATE TABLE IF NOT EXISTS QuizResults (
                        Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserName    TEXT    NOT NULL,
                        Score       INTEGER NOT NULL,
                        TotalQ      INTEGER NOT NULL,
                        PlayedAt    TEXT    NOT NULL
                    );";
                createQuiz.ExecuteNonQuery();

                // ── ActivityLog table ───────────────────────────────────────────
                // Persists the activity log across sessions.
                var createLog = connection.CreateCommand();
                createLog.CommandText = @"
                    CREATE TABLE IF NOT EXISTS ActivityLog (
                        Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserName    TEXT    NOT NULL,
                        Icon        TEXT    NOT NULL DEFAULT '•',
                        Description TEXT    NOT NULL,
                        LoggedAt    TEXT    NOT NULL
                    );";
                createLog.ExecuteNonQuery();

                System.Diagnostics.Debug.WriteLine("DatabaseManager: Initialised successfully.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DatabaseManager.Initialise error: {ex.Message}");
            }
        }

        // ════════════════════════════════════════════════════════════════════════
        // TASK OPERATIONS
        // ════════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Save a new task to the database.
        /// Returns the auto-generated database ID, or -1 on failure.
        /// </summary>
        public static int SaveTask(CyberTask task)
        {
            try
            {
                using var connection = new Microsoft.Data.Sqlite.SqliteConnection(ConnectionString);
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO Tasks (UserName, Title, Description, Reminder, IsCompleted, CreatedAt)
                    VALUES ($user, $title, $desc, $reminder, $completed, $created);
                    SELECT last_insert_rowid();";

                cmd.Parameters.AddWithValue("$user", UserManager.CurrentUserName ?? "Unknown");
                cmd.Parameters.AddWithValue("$title", task.Title ?? string.Empty);
                cmd.Parameters.AddWithValue("$desc", task.Description ?? string.Empty);
                cmd.Parameters.AddWithValue("$reminder", task.Reminder ?? string.Empty);
                cmd.Parameters.AddWithValue("$completed", task.IsCompleted ? 1 : 0);
                cmd.Parameters.AddWithValue("$created", task.CreatedAt.ToString("o"));

                // last_insert_rowid() returns the new row's ID
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DatabaseManager.SaveTask error: {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// Load all tasks for the current user from the database.
        /// Returns an empty list on failure.
        /// </summary>
        public static List<CyberTask> LoadTasks()
        {
            var tasks = new List<CyberTask>();
            try
            {
                using var connection = new Microsoft.Data.Sqlite.SqliteConnection(ConnectionString);
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    SELECT Id, Title, Description, Reminder, IsCompleted, CreatedAt
                    FROM   Tasks
                    WHERE  UserName = $user
                    ORDER  BY Id ASC;";
                cmd.Parameters.AddWithValue("$user", UserManager.CurrentUserName ?? "Unknown");

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tasks.Add(new CyberTask
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2),
                        Reminder = reader.GetString(3),
                        IsCompleted = reader.GetInt32(4) == 1,
                        CreatedAt = DateTime.Parse(reader.GetString(5))
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DatabaseManager.LoadTasks error: {ex.Message}");
            }
            return tasks;
        }

        /// <summary>
        /// Mark a task as complete in the database by its ID.
        /// Returns true if the update affected at least one row.
        /// </summary>
        public static bool MarkTaskComplete(int taskId)
        {
            try
            {
                using var connection = new Microsoft.Data.Sqlite.SqliteConnection(ConnectionString);
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    UPDATE Tasks
                    SET    IsCompleted = 1
                    WHERE  Id = $id AND UserName = $user;";
                cmd.Parameters.AddWithValue("$id", taskId);
                cmd.Parameters.AddWithValue("$user", UserManager.CurrentUserName ?? "Unknown");

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DatabaseManager.MarkTaskComplete error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Delete a task from the database by its ID.
        /// Returns true if the deletion affected at least one row.
        /// </summary>
        public static bool DeleteTask(int taskId)
        {
            try
            {
                using var connection = new Microsoft.Data.Sqlite.SqliteConnection(ConnectionString);
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    DELETE FROM Tasks
                    WHERE  Id = $id AND UserName = $user;";
                cmd.Parameters.AddWithValue("$id", taskId);
                cmd.Parameters.AddWithValue("$user", UserManager.CurrentUserName ?? "Unknown");

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DatabaseManager.DeleteTask error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Update the title, description and reminder of an existing task.
        /// Returns true on success.
        /// </summary>
        public static bool UpdateTask(int taskId, string title, string description, string reminder)
        {
            try
            {
                using var connection = new Microsoft.Data.Sqlite.SqliteConnection(ConnectionString);
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    UPDATE Tasks
                    SET    Title       = $title,
                           Description = $desc,
                           Reminder    = $reminder
                    WHERE  Id = $id AND UserName = $user;";
                cmd.Parameters.AddWithValue("$title", title ?? string.Empty);
                cmd.Parameters.AddWithValue("$desc", description ?? string.Empty);
                cmd.Parameters.AddWithValue("$reminder", reminder ?? string.Empty);
                cmd.Parameters.AddWithValue("$id", taskId);
                cmd.Parameters.AddWithValue("$user", UserManager.CurrentUserName ?? "Unknown");

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DatabaseManager.UpdateTask error: {ex.Message}");
                return false;
            }
        }

        // ════════════════════════════════════════════════════════════════════════
        // QUIZ RESULT OPERATIONS
        // ════════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Save a completed quiz result to the database.
        /// </summary>
        public static void SaveQuizResult(int score, int totalQuestions)
        {
            try
            {
                using var connection = new Microsoft.Data.Sqlite.SqliteConnection(ConnectionString);
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO QuizResults (UserName, Score, TotalQ, PlayedAt)
                    VALUES ($user, $score, $total, $played);";
                cmd.Parameters.AddWithValue("$user", UserManager.CurrentUserName ?? "Unknown");
                cmd.Parameters.AddWithValue("$score", score);
                cmd.Parameters.AddWithValue("$total", totalQuestions);
                cmd.Parameters.AddWithValue("$played", DateTime.Now.ToString("o"));

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DatabaseManager.SaveQuizResult error: {ex.Message}");
            }
        }

        /// <summary>
        /// Load all quiz results for the current user.
        /// Returns a formatted summary string ready for display in chat.
        /// </summary>
        public static string GetQuizHistory()
        {
            try
            {
                using var connection = new Microsoft.Data.Sqlite.SqliteConnection(ConnectionString);
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    SELECT Score, TotalQ, PlayedAt
                    FROM   QuizResults
                    WHERE  UserName = $user
                    ORDER  BY Id DESC
                    LIMIT  10;";
                cmd.Parameters.AddWithValue("$user", UserManager.CurrentUserName ?? "Unknown");

                var lines = new List<string>();
                lines.Add("🏆 Your Quiz History:\n");

                using var reader = cmd.ExecuteReader();
                int count = 1;
                while (reader.Read())
                {
                    int score = reader.GetInt32(0);
                    int total = reader.GetInt32(1);
                    string date = DateTime.Parse(reader.GetString(2)).ToString("dd MMM yyyy HH:mm");
                    double pct = total > 0 ? Math.Round((double)score / total * 100) : 0;
                    lines.Add($"{count}. Score: {score}/{total} ({pct}%)  —  {date}");
                    count++;
                }

                if (count == 1)
                    return "You have not completed any quizzes yet. Type 'start quiz' to begin!";

                return string.Join("\n", lines);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DatabaseManager.GetQuizHistory error: {ex.Message}");
                return "Unable to load quiz history right now.";
            }
        }

        // ════════════════════════════════════════════════════════════════════════
        // ACTIVITY LOG OPERATIONS
        // ════════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Persist a single activity log entry to the database.
        /// </summary>
        public static void SaveLogEntry(string icon, string description)
        {
            try
            {
                using var connection = new Microsoft.Data.Sqlite.SqliteConnection(ConnectionString);
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO ActivityLog (UserName, Icon, Description, LoggedAt)
                    VALUES ($user, $icon, $desc, $logged);";
                cmd.Parameters.AddWithValue("$user", UserManager.CurrentUserName ?? "Unknown");
                cmd.Parameters.AddWithValue("$icon", icon ?? "•");
                cmd.Parameters.AddWithValue("$desc", description ?? string.Empty);
                cmd.Parameters.AddWithValue("$logged", DateTime.Now.ToString("o"));

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DatabaseManager.SaveLogEntry error: {ex.Message}");
            }
        }

        /// <summary>
        /// Load the last 10 activity log entries for the current user from the database.
        /// Returns a formatted string ready for display in chat.
        /// </summary>
        public static string LoadFormattedLog()
        {
            try
            {
                using var connection = new Microsoft.Data.Sqlite.SqliteConnection(ConnectionString);
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    SELECT Icon, Description, LoggedAt
                    FROM   ActivityLog
                    WHERE  UserName = $user
                    ORDER  BY Id DESC
                    LIMIT  10;";
                cmd.Parameters.AddWithValue("$user", UserManager.CurrentUserName ?? "Unknown");

                var lines = new List<string>();
                lines.Add("📋 Your Recent Activity (from database):\n");

                using var reader = cmd.ExecuteReader();
                int count = 1;
                while (reader.Read())
                {
                    string icon = reader.GetString(0);
                    string desc = reader.GetString(1);
                    string time = DateTime.Parse(reader.GetString(2)).ToString("HH:mm:ss dd MMM");
                    lines.Add($"{count}. {icon} {desc}  [{time}]");
                    count++;
                }

                if (count == 1)
                    return "No activity has been logged to the database yet for this session.";

                return string.Join("\n", lines);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DatabaseManager.LoadFormattedLog error: {ex.Message}");
                return "Unable to load activity log from database right now.";
            }
        }

        // ════════════════════════════════════════════════════════════════════════
        // UTILITY
        // ════════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Returns true if the database file exists on disk.
        /// Useful for diagnostic checks on startup.
        /// </summary>
        public static bool DatabaseExists()
        {
            return File.Exists(DatabasePath);
        }

        /// <summary>
        /// Returns the full path to the database file.
        /// Useful for showing the user where data is stored.
        /// </summary>
        public static string GetDatabasePath()
        {
            return DatabasePath;
        }
    }
}
