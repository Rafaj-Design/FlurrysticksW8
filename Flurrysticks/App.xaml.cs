﻿using Flurrystics.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using BugSense;
using Windows.UI.ApplicationSettings;
using Windows.System;
using Flurrysticks.DataModel;
using Windows.ApplicationModel.Background;

// The Split App template is documented at http://go.microsoft.com/fwlink/?LinkId=234228

// TO:DO - implement Flurry analytics when becomes available

namespace Flurrystics
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

        private const string TASK_NAME = "Flurrystics Secondary tiles updater";
        private const string TASK_ENTRY = "FlurrysticsBackgroundTask.BackgroundTask";

        public static int taskCount = 0;

        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Debug.WriteLine("init App");
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            // Initialize BugSense

            //BugSenseHandler.Instance.Init(this, "w8c5d386");
            BugSenseHandler.Instance.Init(this, "w8c5d386", new NotificationOptions() { HandleWhileDebugging = true });
     
        }

        public static void CheckIfBackgroundTaskExist()
        {
            if (BackgroundTaskRegistration.AllTasks.Count < 1)
            {
                RegisterBackgroundTask();
            }
        }

        //Registering the maintenance trigger background task      
        public async static void RegisterBackgroundTask()
        {
            BackgroundAccessStatus result = BackgroundAccessStatus.Denied;
            try
            {
                result = await BackgroundExecutionManager.RequestAccessAsync();
            }
            catch (Exception)
            {
                Debug.WriteLine("RequestAccessAsync not supported"); // probably running on simulator
            }
            if (result == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                result == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {

                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == TASK_NAME)
                        task.Value.Unregister(true);
                }

                BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
                builder.Name = TASK_NAME;
                builder.TaskEntryPoint = TASK_ENTRY;
                builder.SetTrigger(new TimeTrigger(15, false));
                var registration = builder.Register();

            }
        }


        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {

            // try to register background task
            CheckIfBackgroundTaskExist();

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }

                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter

                    if (args.Arguments.Contains("|"))
                    { // called from secondary tile
                        string[] p = args.Arguments.Split('|'); // appName + "|" + appPlatform + "|" + apiKey + "|" + appApiKey;
                        CallApp what = new CallApp();
                        what.AppApiKey = p[3];
                        what.ApiKey = p[2];
                        what.Name = p[0];
                        what.Platform = p[1];
                        rootFrame.Navigate(typeof(AppMetrics), what);
                    }
                    else
                        if (!rootFrame.Navigate(typeof(AccountApplicationsPage), args.Arguments))
                        {
                            throw new Exception("Failed to create initial page");
                        }

                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            else
            {

                if (args.Arguments.Contains("|"))
                { // called from secondary tile
                    string[] p = args.Arguments.Split('|'); // appName + "|" + appPlatform + "|" + apiKey + "|" + appApiKey;
                    CallApp what = new CallApp();
                    what.AppApiKey = p[3];
                    what.ApiKey = p[2];
                    what.Name = p[0];
                    what.Platform = p[1];
                    rootFrame.Navigate(typeof(AppMetrics), what);
                }

            }
            
            // Ensure the current window is active
            Window.Current.Activate();

            // integrate mandatory settings items

            SettingsPane.GetForCurrentView().CommandsRequested += OnSettingsPaneCommandRequested;

        }

        private void OnSettingsPaneCommandRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            // Add the commands one by one to the settings panel

            args.Request.ApplicationCommands.Add(new SettingsCommand("support", "Support e-mail", SupportEmailOperation));
            args.Request.ApplicationCommands.Add(new SettingsCommand("privacy", "Privacy statement", PrivacyPolicyOperation));
        }

        private async void PrivacyPolicyOperation(Windows.UI.Popups.IUICommand command)
        {
            Uri uri = new Uri("http://www.fuerteint.com/mobile-project-16-flurrystics-windows-8/");
            await Launcher.LaunchUriAsync(uri);
        }

        private async void SupportEmailOperation(Windows.UI.Popups.IUICommand command)
        {
            var mailto = new Uri("mailto:info@fuerteint.com");
            await Windows.System.Launcher.LaunchUriAsync(mailto);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}
