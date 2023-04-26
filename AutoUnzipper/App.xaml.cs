﻿using H.NotifyIcon;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AutoUnzipper
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Logic logic;
        private TaskbarIcon taskbarIcon;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            var cmdArgs = Environment.GetCommandLineArgs();
            LaunchApp(cmdArgs);
        }

        private void LaunchApp(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Contains("notification"))
                {
                    var path = args[i].Split("=")[1];

                    Process.Start("explorer.exe", "/select," + path);
                    return;
                }
                else if (args[i].Contains("notification2"))
                {
                    var path = args[i].Split("=")[1];
                    var zipPath = path + ".zip";
                    if (File.Exists(zipPath))
                        File.Delete(zipPath);
                    
                    Process.Start("explorer.exe", "/select," + path);
                    return;
                }
            }


            logic = new Logic();

            taskbarIcon = Resources["TrayIcon"] as TaskbarIcon;
            taskbarIcon.ForceCreate(false);

            var enableCmd = Resources["EnableCmd"] as XamlUICommand;
            var enableNotificationCmd = Resources["EnableNotificationsCmd"] as XamlUICommand;
            var enableDeleteZipCmd = Resources["EnableDeleteZipCmd"] as XamlUICommand;
            var exitCmd = Resources["ExitCmd"] as XamlUICommand;

            enableCmd.ExecuteRequested += Cmd_EnabledToggle;
            enableNotificationCmd.ExecuteRequested += Cmd_EnabledNotificationToggle;
            enableDeleteZipCmd.ExecuteRequested += Cmd_EnableDeleteZipToggle;
            exitCmd.ExecuteRequested += Cmd_Exit;
        }

        private void Cmd_Exit(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            taskbarIcon.Dispose();
        }

        private void Cmd_EnabledToggle(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            logic.Enabled = !logic.Enabled;
        }

        private void Cmd_EnabledNotificationToggle(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            logic.EnabledNotifications = !logic.EnabledNotifications;
        }

        private void Cmd_EnableDeleteZipToggle(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            logic.EnableDeleteZip = !logic.EnableDeleteZip;
        }
    }
}
