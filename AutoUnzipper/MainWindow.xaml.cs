using H.NotifyIcon;
using H.NotifyIcon.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace AutoUnzipper
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            //var enabledToggle = taskbarIcon.FindName("EnabledToggle") as ToggleMenuFlyoutItem;
            //var enabledNotificationToggle = taskbarIcon.FindName("EnabledNotificationsToggle") as ToggleMenuFlyoutItem;
            //var exitBtn = TrayIcon4.FindName("ExitBtn") as MenuFlyoutItem;

            //enabledToggle.Click += EnabledToggle_Clicked;
            //enabledNotificationToggle.Click += EnabledNotificationToggle_Clicked;
            //exitBtn.Click += ExitBtn_Click;

        }

        //private void ExitBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    myButton.Content = "Clicked";
        //}

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
        }
    }
}
