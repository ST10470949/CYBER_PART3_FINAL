using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace CyberSecurity_Part2._2
{
    public class TaskStorageHelper
    {
        private const string FilePath = "tasks.json";

        // LoadTasks(): reads tasks.json and deserialises to List<CyberTask>.
        // If the file does not exist, returns an empty list.
        public List<CyberTask> LoadTasks()
        {
            try
            {
                if (!File.Exists(FilePath))
                    return new List<CyberTask>();

                string json = File.ReadAllText(FilePath);

                if (string.IsNullOrWhiteSpace(json))
                    return new List<CyberTask>();

                var tasks = JsonConvert.DeserializeObject<List<CyberTask>>(json);
                return tasks ?? new List<CyberTask>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"TaskStorageHelper.LoadTasks error: {ex.Message}");
                return new List<CyberTask>();
            }
        }

        // SaveTasks(List<CyberTask> tasks): serialises the list to JSON
        // and writes the result to tasks.json.
        public void SaveTasks(List<CyberTask> tasks)
        {
            try
            {
                string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"TaskStorageHelper.SaveTasks error: {ex.Message}");
            }
        }

        // AddTask(string title, string description, string reminder):
        // loads the current list, creates a new CyberTask, adds it and saves.
        public CyberTask AddTask(string title, string description, string reminder)
        {
            try
            {
                var tasks = LoadTasks();

                // Generate new Id as last Id + 1, or 1 if the list is empty
                int newId = tasks.Count > 0 ? tasks[tasks.Count - 1].Id + 1 : 1;

                var newTask = new CyberTask
                {
                    Id = newId,
                    Title = title?.Trim() ?? "Untitled Task",
                    Description = description?.Trim() ?? string.Empty,
                    Reminder = reminder?.Trim() ?? string.Empty,
                    IsCompleted = false,
                    CreatedAt = DateTime.Now
                };

                tasks.Add(newTask);
                SaveTasks(tasks);

                System.Diagnostics.Debug.WriteLine($"TaskStorageHelper.AddTask: Added '{newTask.Title}' with Id {newTask.Id}");
                return newTask;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"TaskStorageHelper.AddTask error: {ex.Message}");
                return null;
            }
        }

        // MarkAsComplete(int id): loads tasks, finds the task by Id,
        // sets IsCompleted = true and saves the updated list.
        public bool MarkAsComplete(int id)
        {
            try
            {
                var tasks = LoadTasks();
                var task = tasks.Find(t => t.Id == id);

                if (task == null)
                {
                    System.Diagnostics.Debug.WriteLine($"TaskStorageHelper.MarkAsComplete: Task {id} not found.");
                    return false;
                }

                task.IsCompleted = true;
                SaveTasks(tasks);

                System.Diagnostics.Debug.WriteLine($"TaskStorageHelper.MarkAsComplete: Task {id} marked complete.");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"TaskStorageHelper.MarkAsComplete error: {ex.Message}");
                return false;
            }
        }

        // DeleteTask(int id): loads tasks, removes the task with the given Id
        // and saves the updated list.
        public bool DeleteTask(int id)
        {
            try
            {
                var tasks = LoadTasks();
                var task = tasks.Find(t => t.Id == id);

                if (task == null)
                {
                    System.Diagnostics.Debug.WriteLine($"TaskStorageHelper.DeleteTask: Task {id} not found.");
                    return false;
                }

                tasks.Remove(task);
                SaveTasks(tasks);

                System.Diagnostics.Debug.WriteLine($"TaskStorageHelper.DeleteTask: Task {id} deleted.");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"TaskStorageHelper.DeleteTask error: {ex.Message}");
                return false;
            }
        }
    }
}