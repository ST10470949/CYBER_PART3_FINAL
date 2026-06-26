using System;
using System.Collections.Generic;

namespace CyberSecurity_Part2._2
{
    // CyberTask represents a single cybersecurity task created by the user.
    // Tasks are stored in memory for this session and also persisted to the database.
    public class CyberTask
    {
        // Unique identifier for the task.
        public int Id { get; set; }
        // Short title of the task e.g. "Enable 2FA".
        public string Title { get; set; }
        // Longer description of what the task involves.
        public string Description { get; set; }
        // Optional reminder text e.g. "in 3 days" or a specific date.
        public string Reminder { get; set; }
        // Whether the task has been marked as completed.
        public bool IsCompleted { get; set; }
        // When the task was created.
        public DateTime CreatedAt { get; set; }
    }

    // TaskManager handles all in-memory CRUD operations for CyberTask objects.
    // All operations also call DatabaseManager to persist changes.
    public static class TaskManager
    {
        // In-memory list of all tasks for this session.
        private static List<CyberTask> _tasks = new List<CyberTask>();
        // Auto-incrementing ID counter.
        private static int _nextId = 1;

        // Add a new task and return it.
        // Input: title, description, optional reminder string.
        // The task is saved to both the in-memory list and the database.
        public static CyberTask AddTask(string title, string description, string reminder = null)
        {
            var task = new CyberTask
            {
                Id = _nextId++,
                Title = title?.Trim() ?? "Untitled Task",
                Description = description?.Trim() ?? string.Empty,
                Reminder = reminder?.Trim(),
                IsCompleted = false,
                CreatedAt = DateTime.Now
            };
            // Add to in-memory list
            _tasks.Add(task);
            // Log the action in the in-memory activity log
            ActivityLog.Add("📝", $"Task added: '{task.Title}'" + (string.IsNullOrEmpty(task.Reminder) ? "" : $" (Reminder: {task.Reminder})"));
            // Persist to database so tasks survive across sessions
            DatabaseManager.SaveTask(task);
            DatabaseManager.SaveLogEntry("📝", $"Task saved to DB: '{task.Title}'");
            return task;
        }

        // Load tasks from the database into memory.
        // Call this on startup after DatabaseManager.Initialise() so the user
        // sees their tasks from previous sessions immediately.
        public static void LoadTasksFromDatabase()
        {
            try
            {
                var dbTasks = DatabaseManager.LoadTasks();
                _tasks.Clear();
                foreach (var t in dbTasks)
                {
                    _tasks.Add(t);
                    // Keep the ID counter ahead of the highest loaded ID
                    if (t.Id >= _nextId) _nextId = t.Id + 1;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"TaskManager.LoadTasksFromDatabase error: {ex.Message}");
            }
        }

        // Get all tasks.
        public static List<CyberTask> GetAllTasks()
        {
            return new List<CyberTask>(_tasks);
        }

        // Get only incomplete tasks.
        public static List<CyberTask> GetPendingTasks()
        {
            return _tasks.FindAll(t => !t.IsCompleted);
        }

        // Mark a task as complete by its ID.
        // Updates both in-memory list and database.
        // Returns true if the task was found.
        public static bool CompleteTask(int id)
        {
            var task = _tasks.Find(t => t.Id == id);
            if (task == null) return false;
            task.IsCompleted = true;
            // Log in memory
            ActivityLog.Add("✅", $"Task completed: '{task.Title}'");
            // Update in database
            DatabaseManager.MarkTaskComplete(id);
            DatabaseManager.SaveLogEntry("✅", $"Task completed in DB: '{task.Title}'");
            return true;
        }

        // Delete a task by its ID.
        // Removes from both in-memory list and database.
        // Returns true if the task was found and removed.
        public static bool DeleteTask(int id)
        {
            var task = _tasks.Find(t => t.Id == id);
            if (task == null) return false;
            _tasks.Remove(task);
            // Log in memory
            ActivityLog.Add("🗑", $"Task deleted: '{task.Title}'");
            // Delete from database
            DatabaseManager.DeleteTask(id);
            DatabaseManager.SaveLogEntry("🗑", $"Task deleted from DB: '{task.Title}'");
            return true;
        }

        // Format all tasks as a readable string for chat display.
        public static string GetFormattedTaskList()
        {
            if (_tasks.Count == 0)
                return "You have no tasks yet. Add one by typing 'add task' followed by what you want to do.";

            var lines = new List<string>();
            lines.Add("📋 Your Cybersecurity Tasks:\n");
            foreach (var t in _tasks)
            {
                string status = t.IsCompleted ? "✅" : "⏳";
                string reminder = string.IsNullOrEmpty(t.Reminder) ? "" : $"  🔔 Reminder: {t.Reminder}";
                lines.Add($"{status} [{t.Id}] {t.Title}{reminder}");
                if (!string.IsNullOrEmpty(t.Description))
                    lines.Add($"      {t.Description}");
            }
            return string.Join("\n", lines);
        }

        // Clear all tasks from memory.
        public static void ClearAll()
        {
            _tasks.Clear();
        }
    }
}