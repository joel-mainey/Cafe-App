#region Using
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
#endregion Using

namespace Cafe_App
{
    public sealed partial class PredictionPage : Page
    {
        #region Constants & Fields
        // Fields
        private Prediction predictionInstance = new Prediction(); // Create an instance of the Prediction class      
        #endregion Constants & Fields

        #region Constructor
        // Constructor
        public PredictionPage()
        {
            this.InitializeComponent();

            #region Device sizing
            ApplicationView.GetForCurrentView().TryResizeView(new Size(App.DeviceScreenWidth, App.DeviceScreenHeight));
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(App.DeviceMinimumScreenWidth, App.DeviceMinimumScreenHeight));
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            #endregion Device sizing
        }
        #endregion Constructor

        #region Extra
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ApplicationView.GetForCurrentView().TryResizeView(new Size(App.DeviceScreenWidth, App.DeviceScreenHeight));
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Lock the device sizing for the application
            // ApplicationView.GetForCurrentView().TryResizeView(new Size(App.DeviceScreenWidth, App.DeviceScreenHeight));
        }

        private void GotoMainPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        #endregion Extra

        #region Methods
        // Methods
        // Handle the click event of the ButtonPrediction
        private void ButtonPrediction_Click(object sender, RoutedEventArgs e)
        {
            TextBlockPrediction.Text = predictionInstance.GetSentence(); // Get a prediction sentence from the predictionInstance and display it in the TextBlockPrediction
        }
        #endregion Methods
    }
}

