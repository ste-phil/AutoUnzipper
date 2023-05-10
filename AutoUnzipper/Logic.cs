using Microsoft.UI.Xaml;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using System;
using System.IO.Compression;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace AutoUnzipper
{
    public class Logic
    {
        private readonly string fileFilter = "*.zip";
        private FileSystemWatcher watcher;
        private Configuration config;

        private bool enabled = true;
        private bool enabledNotifications = true;
        private bool enabledDeleteZip = true;

        public bool Enabled 
        { 
            get => enabled; 
            set 
            { 
                enabled = value;
                config.AppSettings.Settings["Enabled"].Value = value.ToString();
                SaveSettings();
            }
        }

        public bool EnabledNotifications 
        { 
            get => enabledNotifications; 
            set
            { 
                enabledNotifications = value;
                config.AppSettings.Settings["EnabledNotifications"].Value = value.ToString();
                SaveSettings();
            }
        }

        public bool EnabledDeleteZip 
        { 
            get => enabledDeleteZip; 
            set 
            { 
                enabledDeleteZip = value;
                config.AppSettings.Settings["EnabledDeleteZip"].Value = value.ToString();
                SaveSettings();
            } 
        }

        public Logic()
        {
            OpenSettings();

            watcher = new FileSystemWatcher();
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            watcher.Path = downloadsPath;
            watcher.Filter = fileFilter;
            watcher.EnableRaisingEvents = true;
            //watcher.Created += OnFileCreated;
            watcher.Changed += OnFileChanged;
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;

            Enabled = Convert.ToBoolean(config.AppSettings.Settings["Enabled"].Value);
            EnabledNotifications = Convert.ToBoolean(config.AppSettings.Settings["EnabledNotifications"].Value);
            EnabledDeleteZip = Convert.ToBoolean(config.AppSettings.Settings["EnabledDeleteZip"].Value);
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            if (!Enabled) return;

            var fileName = Path.GetFileNameWithoutExtension(e.FullPath);
            var path = Path.GetDirectoryName(e.FullPath);
            var extractDir = Path.Combine(path, fileName);

            if (Directory.Exists(extractDir)) return;
            try
            {
                ZipFile.ExtractToDirectory(e.FullPath, extractDir);
                ShowNotification(extractDir, fileName);
                DeleteZipFile(e.FullPath);
            }
            catch { }
        }

        //private void OnFileCreated(object sender, FileSystemEventArgs e)
        //{
        //    return;

        //    if (!Enabled) return;

        //    var fileName = Path.GetFileNameWithoutExtension(e.FullPath);
        //    var path = Path.GetDirectoryName(e.FullPath);
        //    var extractDir = Path.Combine(path, fileName);
        //    if (!Directory.Exists(extractDir))
        //    {
        //        Directory.CreateDirectory(extractDir);
        //    }
        //}

        private void ShowNotification(string path, string fileName)
        {
            if (!EnabledNotifications) return;


            var builder = new AppNotificationBuilder()
                .AddArgument("notification-folder", "\"" + path + "\"")
                .AddText($"Unzipped {fileName}")
                .AddButton(
                    new AppNotificationButton("Show in Folder")
                        .AddArgument("notification-folder", "\"" + path + "\"")
                );

            var variable = Environment.GetEnvironmentVariable("Path");
            if (variable != null && variable.Contains("Microsoft VS Code"))
            {
                builder.AddButton(
                   new AppNotificationButton("Show in VSCode")
                       .AddArgument("notification-code", "\"" + path + "\"")
                       .SetIcon(new Uri("ms-appx:///Assets/icon_vscode.png"))
               );
            }
           

            //.AddButton(new AppNotificationButton("Delete zip & show")
            //    .AddArgument("notificationX", "\"" + path + "\""));

            var notificationManager = AppNotificationManager.Default;
            var notification = builder.BuildNotification();

            Debug.WriteLine("Not payload:" + notification.Payload);

            notificationManager.Show(notification);
        }

        private void DeleteZipFile(string zipPath)
        {
            if (!EnabledDeleteZip) return;

            File.Delete(zipPath);
        }

        private void OpenSettings()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        private void SaveSettings()
        {
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
