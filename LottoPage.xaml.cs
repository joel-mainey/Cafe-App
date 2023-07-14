#region Using
using System;
using System.Text;
using System.Windows.Input;
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
    public sealed partial class LottoPage : Page
    {
        #region Constructor
        // Constructor
        public LottoPage()
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
        // Handle the click event of the ButtonSelect
        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            Lotto row = new Lotto(); // Create a new Lotto instance

            int luckyDips = int.TryParse(TextBoxTicket.Text, out luckyDips) ? luckyDips : 0; // Parse the entered text as an integer, defaulting to 0 if parsing fails

            TextBlockTicket.Text = "";
            TextBlockTicket.Text = "-------------------------- Lotto Ticket --------------------------\n";

            if (luckyDips >= 1 && luckyDips <= 20) // Check if the entered value is between 1 and 20
            {
                for (int i = 0; i < luckyDips; i++) // Generate lotto numbers for the specified number of lucky dips
                {
                    TextBlockError.Text = "";
                    TextBlockTicket.Text += "-----------    ";
                    row.SetNumbersToZero(); // Set the lotto numbers to zero
                    row.GenerateNumbers(); // Generate random lotto numbers
                    row.SortNumbers(); // Sort the lotto numbers in ascending order
                    row.PrintNumbers(TextBlockTicket); // Print the lotto numbers to the TextBlockTicket
                    TextBlockTicket.Text += "    -----------\n";
                }
            }
            else
            {
                TextBlockError.Text = "Enter a number between 1 and 20"; // Display an error message if the entered value is not within the specified range
            }

            TextBlockTicket.Text += "------------------------------------------------------------------";
        }
        #endregion Methods
    }
}

