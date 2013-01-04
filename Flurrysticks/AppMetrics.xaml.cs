﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using System.Xml.Linq;
using Telerik.UI.Xaml.Controls.Chart;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Flurrystics
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class AppMetrics : Flurrystics.Common.LayoutAwarePage
    {

        string apiKey;
        string appapikey = ""; // initial apikey of the app
        string appName = "";   // appName
        string platform = "";  // platform
        string[] AppMetricsNames = { "ActiveUsers", "ActiveUsersByWeek", "ActiveUsersByMonth", "NewUsers", "MedianSessionLength", "AvgSessionLength", "Sessions", "RetainedUsers" };
        string EndDate;
        string StartDate;
        int actualMetricsIndex = 0;
        DownloadHelper dh = new DownloadHelper();

        public AppMetrics()
        {
            this.InitializeComponent();
        }

        private void ParseXML(XDocument what, int i)
        {
            Debug.WriteLine("Processing..." + what);
            RadCartesianChart[] targetCharts = { radChart1, radChart2, radChart3, radChart4, radChart5, radChart6, radChart7, radChart8 };
            bool result = false;
                IEnumerable<ChartDataPoint> ChartData;
                   ChartData = from query in what.Descendants("day")
                               select new ChartDataPoint
                               {
                                   Value = (double)query.Attribute("value"),
                                   Label = (string)query.Attribute("date")
                               };
           targetCharts[i].DataContext = ChartData;          
        } // ParseXML

        private async void loadData(int metricsIndex)
        {
            Debug.WriteLine("loadData() " + metricsIndex);
            EndDate   = String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-1));
            StartDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddMonths(-1));
            string metrics = AppMetricsNames[metricsIndex]; // this will be selectable
            if (ProgressBar1 == null) {return;}
            ProgressBar1.Visibility = Windows.UI.Xaml.Visibility.Visible;
            // check if it's loaded, if not - load it up
            bool success;
            XDocument result = null;
                string callURL = "http://api.flurry.com/appMetrics/" + metrics + "?apiAccessCode=" + apiKey + "&apiKey=" + appapikey + "&startDate=" + StartDate + "&endDate=" + EndDate;
                Debug.WriteLine(callURL);
                try
                {
                    result = await dh.DownloadXML(callURL); // load it   
                    success = true;
                }
                catch (System.Net.Http.HttpRequestException)
                {   // load failed
                    success = false;
                }
                Debug.WriteLine("Success:" + success);
                if (success) { ParseXML(result, metricsIndex); }
                if (App.taskCount==0) {
                    ProgressBar1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }

        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            Debug.WriteLine("AppMetrics - LoadState");
            CallApp what = (CallApp)navigationParameter;
            pageTitle.Text = what.Name;
            apiKey = what.ApiKey;
            appapikey = what.AppApiKey;
            loadData(actualMetricsIndex); 
        }
     
        /*
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Debug.WriteLine("AppMetrics - OnNavigatedTo");
            CallApp what = (CallApp)e.Parameter; // navigationParameter;
            pageTitle.Text = what.Name;
            apiKey = what.ApiKey;
            appapikey = what.AppApiKey;
            loadData(actualMetricsIndex);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }        
        */

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        public class ChartDataPoint
        {
            public string Label { get; set; }
            public double Value { get; set; } // appmetrics
            public double Value1 { get; set; } // Unique Users
            public double Value2 { get; set; } // Total Sessions
            public double Value3 { get; set; } //
            public double Value4 { get; set; }
        }

        private void flipView1_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("flipView1 SelectionChanged");
            actualMetricsIndex = ((FlipView)sender).SelectedIndex;
            loadData(actualMetricsIndex);
        }

        private void ChartTrackBallBehavior_TrackInfoUpdated_1(object sender, TrackBallInfoEventArgs e)
        {

        }

    }
}