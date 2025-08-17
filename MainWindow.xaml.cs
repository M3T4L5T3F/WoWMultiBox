using System;
using System.Windows;
using System.Windows.Input;

namespace WOB2025
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Optional: Add window-specific event handlers here
            Loaded += OnMainWindowLoaded;
            Closing += OnMainWindowClosing;
            
            // Optional: Handle system tray minimize
            StateChanged += OnStateChanged;
        }

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Optional: Perform any initialization after window loads
            // For example, restore window position from settings
            RestoreWindowPosition();
        }

        private void OnMainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Optional: Save window position before closing
            SaveWindowPosition();
            
            // Optional: Minimize to tray instead of closing
            var settings = App.ServiceProvider?.GetService(typeof(AppSettings)) as AppSettings;
            if (settings?.MinimizeToTray == true)
            {
                e.Cancel = true;
                WindowState = WindowState.Minimized;
            }
        }

        private void OnStateChanged(object sender, EventArgs e)
        {
            // Optional: Hide to system tray when minimized
            var settings = App.ServiceProvider?.GetService(typeof(AppSettings)) as AppSettings;
            if (settings?.MinimizeToTray == true && WindowState == WindowState.Minimized)
            {
                Hide();
                // Show system tray icon (implementation needed)
            }
        }

        private void RestoreWindowPosition()
        {
            try
            {
                // Restore from saved settings if available
                var settingsFile = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "WOB2025",
                    "window_position.json"
                );

                if (System.IO.File.Exists(settingsFile))
                {
                    var json = System.IO.File.ReadAllText(settingsFile);
                    dynamic settings = System.Text.Json.JsonSerializer.Deserialize<dynamic>(json);
                    
                    // Apply saved position if on screen
                    if (settings != null)
                    {
                        // Validate position is on screen before applying
                        // Implementation depends on your needs
                    }
                }
            }
            catch
            {
                // Ignore errors and use default position
            }
        }

        private void SaveWindowPosition()
        {
            try
            {
                var settings = new
                {
                    Left = Left,
                    Top = Top,
                    Width = Width,
                    Height = Height,
                    WindowState = WindowState.ToString()
                };

                var settingsDir = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "WOB2025"
                );
                
                System.IO.Directory.CreateDirectory(settingsDir);
                
                var settingsFile = System.IO.Path.Combine(settingsDir, "window_position.json");
                var json = System.Text.Json.JsonSerializer.Serialize(settings);
                System.IO.File.WriteAllText(settingsFile, json);
            }
            catch
            {
                // Ignore save errors
            }
        }

        // Optional: Handle ESC key to minimize
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            
            if (e.Key == Key.Escape)
            {
                WindowState = WindowState.Minimized;
            }
        }
    }
}
