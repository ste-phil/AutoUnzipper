using Microsoft.UI.Xaml;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using System;
using System.IO.Compression;
using System.IO;
using System.Diagnostics;

namespace AutoUnzipper
{
    public class Logic
    {
        private readonly string fileFilter = "*.zip";
        private FileSystemWatcher watcher;

        public bool Enabled { get; set; } = true;
        public bool EnabledNotifications { get; set; } = true;
        public bool EnableDeleteZip { get; set; } = true;


        public Logic()
        {
            watcher = new FileSystemWatcher();

            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            watcher.Path = downloadsPath;
            watcher.Filter = fileFilter;
            watcher.EnableRaisingEvents = true;
            watcher.Created += OnFileCreated;
            watcher.Changed += OnFileChanged;
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
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
            } catch { }
        }

        

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            return;

            if (!Enabled) return;

            var fileName = Path.GetFileNameWithoutExtension(e.FullPath);
            var path = Path.GetDirectoryName(e.FullPath);
            var extractDir = Path.Combine(path, fileName);
            if (!Directory.Exists(extractDir))
            {
                Directory.CreateDirectory(extractDir);
            }
        }

        private void ShowNotification(string path, string fileName)
        {
            if (!EnabledNotifications) return;


            var builder = new AppNotificationBuilder()
            .AddArgument("notification", "\"" + path + "\"")
            .AddText($"Unzipped {fileName}")
            .AddButton(new AppNotificationButton("Show in Folder")
                .AddArgument("notification", "\"" + path + "\""));
            //.AddButton(new AppNotificationButton("Delete zip & show")
            //    .AddArgument("notificationX", "\"" + path + "\""));

            var notificationManager = AppNotificationManager.Default;
            var notification = builder.BuildNotification();
            notificationManager.Show(notification);
        }

        private void DeleteZipFile(string zipPath)
        {
            if (!EnableDeleteZip) return;

            File.Delete(zipPath);
        }
    }
}
