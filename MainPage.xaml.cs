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
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            #region Device sizing
            ApplicationView.GetForCurrentView().TryResizeView(new Size(App.DeviceScreenWidth, App.DeviceScreenHeight));
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(App.DeviceMinimumScreenWidth, App.DeviceMinimumScreenHeight));
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            #endregion Device sizing
        }

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

        private void GotoSlotPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SlotPage));
        }

        private void GotoDicePageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DicePage));
        }

        private void GotoPredictionPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PredictionPage));
        }

        private void GotoLottoPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LottoPage));
        }

        private void GotoDrinksPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DrinksPage));
        }

        private void GotoCompanyPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CompanyDetailsPage));
        }

        private void GotoLocationPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LocationPage));
        }
        #endregion Extra
    }
}

