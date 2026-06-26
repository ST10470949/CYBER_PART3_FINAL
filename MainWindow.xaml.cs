using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CyberSecurity_Part2._2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// This class controls the main WPF window: UI events, sending messages,
    /// handling the startup overlay, playing audio, recording voice notes,
    /// and managing the right-side task panel.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Simple recorder helper. Created when user starts recording.
        private AudioRecorder? recorder;
        // Path to the temporary WAV file created when recording.
        private string? currentRecordingPath;
        // Guard to avoid re-entrant text change updates on the name box
        private bool isUpdatingNameText = false;
        // Sidebar state for collapsible topics
        private bool _sidebarOpen = true;
        // Task panel state for collapsible right panel
        private bool _taskPanelOpen = true;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;

            // ensure startup overlay blocks interaction until name provided
            StartupOverlay.IsHitTestVisible = true;
            NameTextBox.KeyDown += NameTextBox_KeyDown;
            NameTextBox.TextChanged += NameTextBox_TextChanged;
        }

        // Toggle handler for the Topics sidebar button in the header.
        private void BtnToggleTopics_Click(object sender, RoutedEventArgs e)
        {
            _sidebarOpen = !_sidebarOpen;
            var col = SidebarColumn;
            if (col == null) return;

            var anim = new GridLengthAnimation
            {
                From = col.Width,
                To = _sidebarOpen ? new GridLength(240, GridUnitType.Pixel) : new GridLength(0, GridUnitType.Pixel),
                Duration = new Duration(TimeSpan.FromMilliseconds(220)),
                EasingFunction = new System.Windows.Media.Animation.QuadraticEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
            };
            col.BeginAnimation(ColumnDefinition.WidthProperty, anim);
            if (ToggleIcon != null) ToggleIcon.Text = _sidebarOpen ? "✕" : "☰";
        }

        // Toggle handler for the Tasks panel button in the header.
        private void BtnToggleTasks_Click(object sender, RoutedEventArgs e)
        {
            _taskPanelOpen = !_taskPanelOpen;
            var col = TaskPanelColumn;
            if (col == null) return;

            var anim = new GridLengthAnimation
            {
                From = col.Width,
                To = _taskPanelOpen ? new GridLength(280, GridUnitType.Pixel) : new GridLength(0, GridUnitType.Pixel),
                Duration = new Duration(TimeSpan.FromMilliseconds(220)),
                EasingFunction = new System.Windows.Media.Animation.QuadraticEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
            };
            col.BeginAnimation(ColumnDefinition.WidthProperty, anim);
            if (TaskToggleIcon != null) TaskToggleIcon.Text = _taskPanelOpen ? "✕" : "📋";
        }

        // Toggle the sidebar visibility with a simple width animation
        private void SidebarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var gridCol = this.MainContentGrid.ColumnDefinitions[0];
                if (gridCol.Width.Value > 0)
                    gridCol.Width = new GridLength(0);
                else
                    gridCol.Width = new GridLength(240);
            }
            catch { }
        }

        // Called when the window finishes loading.
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialise the database and create all tables on first run.
            DatabaseManager.Initialise();

            // Load tasks saved from previous sessions into memory.
            TaskManager.LoadTasksFromDatabase();

            // Also try to load from JSON as a secondary sync.
            // (LoadTasksFromDatabase already syncs JSON, but this is a safety fallback.)
            // TaskManager.LoadTasksFromJson(); // uncomment if you want JSON-first loading

            // Refresh the task panel so loaded tasks appear immediately.
            RefreshTaskPanel();

            if (string.IsNullOrWhiteSpace(UserManager.CurrentUserName))
                UserManager.CurrentUserName = string.Empty;
            else
                UpdateHeaderUser();

            try
            {
                if (StartupOverlay != null)
                {
                    StartupOverlay.Visibility = Visibility.Visible;
                    StartupOverlay.IsHitTestVisible = true;
                }
                if (NameTextBox != null)
                {
                    NameTextBox.IsEnabled = true;
                    NameTextBox.Focusable = true;
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        try { NameTextBox.Focus(); } catch { }
                    }));
                }
            }
            catch { }

            NameTextBox.Focus();
        }

        // Update the small "User:" text in the header.
        private void UpdateHeaderUser()
        {
            if (HeaderUserText != null)
                HeaderUserText.Text = $"User: {UserManager.CurrentUserName}";
        }

        // When the user presses Enter inside the name textbox, treat it as 'Continue'.
        private void NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                ContinueButton_Click(this, new RoutedEventArgs());
            }
        }

        // Normalize the name to Title Case as the user types.
        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isUpdatingNameText) return;
            try
            {
                isUpdatingNameText = true;
                var tb = sender as TextBox;
                if (tb == null) return;
                var selStart = tb.SelectionStart;
                var original = tb.Text ?? string.Empty;
                var ti = CultureInfo.CurrentCulture.TextInfo;
                var lower = original.ToLower();
                var titled = ti.ToTitleCase(lower);
                if (titled != original)
                {
                    tb.Text = titled;
                    tb.SelectionStart = Math.Min(selStart + (titled.Length - original.Length), tb.Text.Length);
                }
            }
            finally
            {
                isUpdatingNameText = false;
            }
        }

        // Continue button handler on the startup overlay.
        private async void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            var name = NameTextBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                NameWarning.Visibility = Visibility.Visible;
                return;
            }

            NameWarning.Visibility = Visibility.Collapsed;
            UserManager.CurrentUserName = name;
            UpdateHeaderUser();

            var visits = UserManager.IncrementUserCount(name);

            MainContentGrid.IsEnabled = true;
            StartupOverlay.Visibility = Visibility.Collapsed;
            StartupOverlay.IsHitTestVisible = false;
            EndChatButton.Visibility = Visibility.Visible;

            DatabaseManager.SaveLogEntry("🔑", $"User logged in: {name}");

            // Refresh task panel now that we know the username (loads user's tasks)
            TaskManager.LoadTasksFromDatabase();
            RefreshTaskPanel();

            try
            {
                var path = System.IO.Path.Combine(System.AppContext.BaseDirectory, "Abos2.wav");
                if (System.IO.File.Exists(path))
                {
                    await System.Threading.Tasks.Task.Run(() =>
                    {
                        try
                        {
                            var sp = new System.Media.SoundPlayer(path);
                            sp.PlaySync();
                        }
                        catch { }
                    });
                }
            }
            catch { }

            var userGreeting = new ChatMessage
            {
                Sender = UserManager.CurrentUserName,
                Text = $"Hi, I am {UserManager.CurrentUserName}.",
                IsUser = true
            };
            ChatListBox.Items.Add(userGreeting);
            ChatListBox.ScrollIntoView(userGreeting);

            await System.Threading.Tasks.Task.Delay(400);

            ChatMessage botWelcome;
            if (visits > 2)
            {
                botWelcome = new ChatMessage
                {
                    Sender = "Bot",
                    Text = $"Welcome back {UserManager.CurrentUserName}! 👋 I remember you from your previous visits. Great to have you here again. Feel free to ask me anything about cybersecurity — passwords, phishing, malware, privacy and more.",
                    IsUser = false
                };
            }
            else
            {
                botWelcome = new ChatMessage
                {
                    Sender = "Bot",
                    Text = $"Welcome {UserManager.CurrentUserName}! 👋 I am your AI Cybersecurity Assistant. It is a pleasure to have you here. You may ask me about passwords, phishing, malware, privacy, safe browsing and much more. Select a topic on the left or type your question below to get started.",
                    IsUser = false
                };
            }

            ChatListBox.Items.Add(botWelcome);
            ChatListBox.ScrollIntoView(botWelcome);
            InputTextBox.Focus();
        }

        // If the user presses Enter in the chat input, send the message.
        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.None)
            {
                e.Handled = true;
                SendCurrentMessage();
            }
        }

        // Send button clicked.
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendCurrentMessage();
        }

        // Send the user's message and display the bot response.
        // Also refreshes the task panel after any message in case tasks changed.
        private void SendCurrentMessage()
        {
            var text = InputTextBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(text)) return;

            try
            {
                var userMsg = new ChatMessage { Sender = UserManager.CurrentUserName, Text = text, IsUser = true };
                this.Dispatcher.Invoke(() =>
                {
                    ChatListBox.Items.Add(userMsg);
                    ChatListBox.ScrollIntoView(userMsg);
                });

                string response;
                try
                {
                    response = ChatBot.GetResponse(text) ?? "I'm not sure I understand. Can you try rephrasing?";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ChatBot.GetResponse error: {ex.Message}");
                    response = "I'm having trouble processing that right now. Please try again.";
                }

                var botMsg = new ChatMessage { Sender = "Bot", Text = response, IsUser = false };
                this.Dispatcher.Invoke(() =>
                {
                    ChatListBox.Items.Add(botMsg);
                    ChatListBox.ScrollIntoView(botMsg);
                });

                this.Dispatcher.Invoke(() =>
                {
                    InputTextBox.Clear();
                    InputTextBox.Focus();
                    // Refresh the task panel so any added/completed/deleted tasks appear immediately
                    RefreshTaskPanel();
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SendCurrentMessage error: {ex.Message}");
                var err = new ChatMessage { Sender = "Bot", Text = "An unexpected error occurred. Please try again.", IsUser = false };
                this.Dispatcher.Invoke(() =>
                {
                    ChatListBox.Items.Add(err);
                    ChatListBox.ScrollIntoView(err);
                });
            }
        }

        // When a topic button in the left sidebar is clicked.
        private void TopicButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button btn && btn.Tag is string tag)
                {
                    var topicKey = tag.Trim();
                    var display = btn.Content is StackPanel sp && sp.Children.Count > 1 && sp.Children[1] is TextBlock tb ? tb.Text : topicKey;
                    var userMsg = new ChatMessage { Sender = UserManager.CurrentUserName, Text = display, IsUser = true };
                    ChatListBox.Items.Add(userMsg);
                    ChatListBox.ScrollIntoView(userMsg);

                    DatabaseManager.SaveLogEntry("📚", $"Topic viewed: {display}");

                    string response;
                    try
                    {
                        response = ChatBot.GetResponse(topicKey) ?? "I'm not sure I understand. Can you try rephrasing?";
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"ChatBot.GetResponse error: {ex.Message}");
                        response = "I'm having trouble processing that right now. Please try again.";
                    }

                    var botMsg = new ChatMessage { Sender = "Bot", Text = response, IsUser = false };
                    ChatListBox.Items.Add(botMsg);
                    ChatListBox.ScrollIntoView(botMsg);
                    InputTextBox.Focus();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"TopicButton_Click error: {ex.Message}");
            }
        }

        // Start Quiz button clicked.
        private void BtnStartQuiz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userMsg = new ChatMessage
                {
                    Sender = UserManager.CurrentUserName,
                    Text = "Start Quiz",
                    IsUser = true
                };
                ChatListBox.Items.Add(userMsg);
                ChatListBox.ScrollIntoView(userMsg);

                string response;
                try
                {
                    response = ChatBot.GetResponse("start quiz") ?? "Unable to start quiz. Please try again.";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"BtnStartQuiz_Click error: {ex.Message}");
                    response = "Unable to start the quiz right now. Please try again.";
                }

                var botMsg = new ChatMessage { Sender = "Bot", Text = response, IsUser = false };
                ChatListBox.Items.Add(botMsg);
                ChatListBox.ScrollIntoView(botMsg);
                InputTextBox.Focus();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"BtnStartQuiz_Click outer error: {ex.Message}");
            }
        }

        // Show Tasks button in sidebar clicked — sends to chat AND refreshes panel.
        private void BtnShowTasks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userMsg = new ChatMessage
                {
                    Sender = UserManager.CurrentUserName,
                    Text = "Show my tasks",
                    IsUser = true
                };
                ChatListBox.Items.Add(userMsg);
                ChatListBox.ScrollIntoView(userMsg);

                string response;
                try
                {
                    response = ChatBot.GetResponse("show tasks") ?? "Unable to retrieve tasks.";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"BtnShowTasks_Click error: {ex.Message}");
                    response = "Unable to retrieve tasks right now. Please try again.";
                }

                var botMsg = new ChatMessage { Sender = "Bot", Text = response, IsUser = false };
                ChatListBox.Items.Add(botMsg);
                ChatListBox.ScrollIntoView(botMsg);

                // Also refresh the visual task panel
                RefreshTaskPanel();

                // Open the task panel if it's collapsed
                if (!_taskPanelOpen)
                    BtnToggleTasks_Click(sender, e);

                InputTextBox.Focus();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"BtnShowTasks_Click outer error: {ex.Message}");
            }
        }

        // Activity Log button in sidebar clicked.
        private void BtnActivityLog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userMsg = new ChatMessage
                {
                    Sender = UserManager.CurrentUserName,
                    Text = "Show activity log",
                    IsUser = true
                };
                ChatListBox.Items.Add(userMsg);
                ChatListBox.ScrollIntoView(userMsg);

                string response;
                try
                {
                    response = ChatBot.GetResponse("activity log") ?? "Unable to retrieve activity log.";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"BtnActivityLog_Click error: {ex.Message}");
                    response = "Unable to retrieve the activity log right now. Please try again.";
                }

                var botMsg = new ChatMessage { Sender = "Bot", Text = response, IsUser = false };
                ChatListBox.Items.Add(botMsg);
                ChatListBox.ScrollIntoView(botMsg);
                InputTextBox.Focus();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"BtnActivityLog_Click outer error: {ex.Message}");
            }
        }

        // End Chat button handler.
        private void EndChatButton_Click(object sender, RoutedEventArgs e)
        {
            DatabaseManager.SaveLogEntry("👋", $"User ended chat: {UserManager.CurrentUserName}");
            ExitSequence();
        }

        // ExitSequence posts farewell messages and closes the window.
        private async void ExitSequence()
        {
            var lines = new string[]
            {
                $"Goodbye, {UserManager.CurrentUserName}.",
                "Thank you for using Cyber Times With Abo.",
                "Stay vigilant and stay safe online.",
                "Remember, cybersecurity is a shared responsibility. Together, we can create a safer digital world for everyone.",
                "Made by Abongile Manqana"
            };

            foreach (var line in lines)
            {
                var msg = new ChatMessage { Sender = "Bot", Text = line, IsUser = false };
                ChatListBox.Items.Add(msg);
                ChatListBox.ScrollIntoView(msg);
                await System.Threading.Tasks.Task.Delay(450);
            }

            await System.Threading.Tasks.Task.Delay(550);
            this.Close();
        }

        // RecordButton toggles recording state.
        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (recorder == null)
            {
                try
                {
                    recorder = new AudioRecorder();
                    var tmp = System.IO.Path.GetTempFileName();
                    var wav = System.IO.Path.ChangeExtension(tmp, ".wav");
                    if (System.IO.File.Exists(wav)) System.IO.File.Delete(wav);
                    currentRecordingPath = wav;
                    recorder.StartRecording(wav);
                    RecordButton.Content = "⏺";
                }
                catch
                {
                    recorder = null;
                }
            }
            else
            {
                recorder.StopRecording();
                recorder.Dispose();
                recorder = null;
                RecordButton.Content = "🎤";

                if (!string.IsNullOrEmpty(currentRecordingPath) && System.IO.File.Exists(currentRecordingPath))
                {
                    var userMsg = new ChatMessage { Sender = UserManager.CurrentUserName, Text = "[Voice message recorded]", IsUser = true };
                    ChatListBox.Items.Add(userMsg);
                    ChatListBox.ScrollIntoView(userMsg);

                    try
                    {
                        var sp = new System.Media.SoundPlayer(currentRecordingPath);
                        sp.Play();
                    }
                    catch { }
                }
            }
        }

        // Empty handler kept to avoid XAML compile error from the TextChanged_1 wire-up.
        private void NameTextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            // Intentionally empty — actual logic is in NameTextBox_TextChanged above.
        }

        // ═══════════════════════════════════════════════════════
        // TASK PANEL — Quick add from the panel's own input box
        // ═══════════════════════════════════════════════════════

        // Enter key in the quick-add task box triggers add.
        private void QuickAddTaskBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                AddQuickTask();
            }
        }

        // + button in the task panel triggers add.
        private void QuickAddTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            AddQuickTask();
        }

        // Add a task from the panel's input box directly (bypasses chat).
        private void AddQuickTask()
        {
            var title = QuickAddTaskBox?.Text?.Trim();
            if (string.IsNullOrWhiteSpace(title)) return;

            try
            {
                // Add to task manager (saves to DB + JSON automatically)
                var newTask = TaskManager.AddTask(title, "", null);

                // Show a chat bot message confirming the addition
                var botMsg = new ChatMessage
                {
                    Sender = "Bot",
                    Text = $"✅ Task added from panel: '{newTask.Title}'. Type 'show tasks' or check the Tasks panel to see all your tasks.",
                    IsUser = false
                };
                ChatListBox.Items.Add(botMsg);
                ChatListBox.ScrollIntoView(botMsg);

                // Clear the input
                QuickAddTaskBox.Clear();
                QuickAddTaskBox.Focus();

                // Refresh the visual task panel
                RefreshTaskPanel();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AddQuickTask error: {ex.Message}");
            }
        }

        // ═══════════════════════════════════════════════════════
        // TASK PANEL — Refresh the visual list of task cards
        // ═══════════════════════════════════════════════════════

        // Rebuild the task card list in the right panel from the current task list.
        public void RefreshTaskPanel()
        {
            try
            {
                if (TaskListPanel == null) return;

                // Clear all existing children except the empty-tasks text
                TaskListPanel.Children.Clear();

                var tasks = TaskManager.GetAllTasks();
                int pending = 0;
                int completed = 0;

                if (tasks.Count == 0)
                {
                    // Show the empty placeholder
                    var empty = new TextBlock
                    {
                        Name = "EmptyTasksText",
                        Text = "No tasks yet. Type above or chat: 'add task enable 2FA'",
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748B")),
                        FontSize = 11,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(4, 8, 4, 0)
                    };
                    TaskListPanel.Children.Add(empty);
                }
                else
                {
                    foreach (var task in tasks)
                    {
                        if (task.IsCompleted) completed++; else pending++;

                        // Outer card border
                        var card = new Border
                        {
                            Background = new SolidColorBrush(task.IsCompleted
                                ? (Color)ColorConverter.ConvertFromString("#0A1F12")
                                : (Color)ColorConverter.ConvertFromString("#0D1117")),
                            BorderBrush = new SolidColorBrush(task.IsCompleted
                                ? (Color)ColorConverter.ConvertFromString("#00FF9C44")
                                : (Color)ColorConverter.ConvertFromString("#1E2D3D")),
                            BorderThickness = new Thickness(1),
                            CornerRadius = new CornerRadius(6),
                            Padding = new Thickness(8, 6, 8, 6),
                            Margin = new Thickness(0, 0, 0, 6)
                        };

                        // Content grid: status icon + title + action buttons
                        var cardGrid = new Grid();
                        cardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                        cardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                        cardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                        // Status icon
                        var statusIcon = new TextBlock
                        {
                            Text = task.IsCompleted ? "✅" : "⏳",
                            FontSize = 13,
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(0, 0, 8, 0)
                        };
                        Grid.SetColumn(statusIcon, 0);

                        // Title + optional reminder
                        var titlePanel = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
                        var titleText = new TextBlock
                        {
                            Text = task.Title,
                            Foreground = new SolidColorBrush(task.IsCompleted
                                ? (Color)ColorConverter.ConvertFromString("#64748B")
                                : (Color)ColorConverter.ConvertFromString("#E2E8F0")),
                            FontSize = 12,
                            TextWrapping = TextWrapping.Wrap,
                            TextDecorations = task.IsCompleted ? TextDecorations.Strikethrough : null
                        };
                        titlePanel.Children.Add(titleText);

                        if (!string.IsNullOrEmpty(task.Reminder))
                        {
                            var reminderText = new TextBlock
                            {
                                Text = $"🔔 {task.Reminder}",
                                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FF9C")),
                                FontSize = 10,
                                Margin = new Thickness(0, 2, 0, 0)
                            };
                            titlePanel.Children.Add(reminderText);
                        }
                        Grid.SetColumn(titlePanel, 1);

                        // Action buttons panel
                        var actionPanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(6, 0, 0, 0)
                        };

                        // Complete button (only if not already complete)
                        if (!task.IsCompleted)
                        {
                            var completeBtn = new Button
                            {
                                Content = "✓",
                                Tag = task.Id,
                                ToolTip = "Mark complete",
                                Width = 26,
                                Height = 26,
                                FontSize = 12,
                                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FF9C")),
                                Background = Brushes.Transparent,
                                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E2D3D")),
                                BorderThickness = new Thickness(1),
                                Cursor = Cursors.Hand,
                                Margin = new Thickness(0, 0, 4, 0)
                            };
                            completeBtn.Click += TaskCompleteBtn_Click;

                            // Inline hover style via event (simpler than full Style in code)
                            completeBtn.MouseEnter += (s, ev) =>
                            {
                                if (s is Button b) b.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FF9C"));
                            };
                            completeBtn.MouseLeave += (s, ev) =>
                            {
                                if (s is Button b) b.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E2D3D"));
                            };

                            actionPanel.Children.Add(completeBtn);
                        }

                        // Delete button
                        var deleteBtn = new Button
                        {
                            Content = "✕",
                            Tag = task.Id,
                            ToolTip = "Delete task",
                            Width = 26,
                            Height = 26,
                            FontSize = 11,
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748B")),
                            Background = Brushes.Transparent,
                            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E2D3D")),
                            BorderThickness = new Thickness(1),
                            Cursor = Cursors.Hand
                        };
                        deleteBtn.Click += TaskDeleteBtn_Click;
                        deleteBtn.MouseEnter += (s, ev) =>
                        {
                            if (s is Button b)
                            {
                                b.Foreground = new SolidColorBrush(Colors.OrangeRed);
                                b.BorderBrush = new SolidColorBrush(Colors.OrangeRed);
                            }
                        };
                        deleteBtn.MouseLeave += (s, ev) =>
                        {
                            if (s is Button b)
                            {
                                b.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748B"));
                                b.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E2D3D"));
                            }
                        };
                        actionPanel.Children.Add(deleteBtn);

                        Grid.SetColumn(actionPanel, 2);

                        cardGrid.Children.Add(statusIcon);
                        cardGrid.Children.Add(titlePanel);
                        cardGrid.Children.Add(actionPanel);
                        card.Child = cardGrid;
                        TaskListPanel.Children.Add(card);
                    }
                }

                // Update count badge and footer
                if (TaskCountBadge != null)
                    TaskCountBadge.Text = $"  ({tasks.Count})";
                if (PendingCountText != null)
                    PendingCountText.Text = $"⏳ {pending} pending";
                if (CompletedCountText != null)
                    CompletedCountText.Text = $"✅ {completed} done";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RefreshTaskPanel error: {ex.Message}");
            }
        }

        // Handler for the ✓ (complete) button on a task card.
        private void TaskCompleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int taskId)
            {
                bool done = TaskManager.CompleteTask(taskId);
                if (done)
                {
                    var botMsg = new ChatMessage
                    {
                        Sender = "Bot",
                        Text = $"✅ Task {taskId} marked as complete from the panel. Great work!",
                        IsUser = false
                    };
                    ChatListBox.Items.Add(botMsg);
                    ChatListBox.ScrollIntoView(botMsg);
                }
                RefreshTaskPanel();
            }
        }

        // Handler for the ✕ (delete) button on a task card.
        private void TaskDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int taskId)
            {
                bool del = TaskManager.DeleteTask(taskId);
                if (del)
                {
                    var botMsg = new ChatMessage
                    {
                        Sender = "Bot",
                        Text = $"🗑 Task {taskId} deleted from the panel.",
                        IsUser = false
                    };
                    ChatListBox.Items.Add(botMsg);
                    ChatListBox.ScrollIntoView(botMsg);
                }
                RefreshTaskPanel();
            }
        }
    }
}