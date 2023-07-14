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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.ViewManagement;
#endregion Using

namespace Cafe_App
{
    public sealed partial class SlotPage : Page
    {
        #region Constants & Fields
        // Constants
        private const int NumWheel = 4; // Number of wheels in the slot machine
        private const int WheelCost = 5; // Cost to play each wheel of the slot machine
        private const int InitialCash = 0; // Initial cash amount
        private const int AddCashAmount = 100; // Amount of cash added when the "Add Cash" button is clicked

        // Fields
        private Random random = new Random(); // Random number generator
        private int cash = InitialCash; // Current cash amount
        #endregion Constants & Fields

        #region Constructor
        // Constructor
        public SlotPage()
        {
            this.InitializeComponent();

            buttonPlay.Visibility = Visibility.Collapsed; // Hide the play button
            UpdateCash(); // Update the displayed cash amount

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
        #endregion extra

        #region Methods
        // Methods
        // Update the displayed cash amount
        private void UpdateCash()
        {
            textBlockDollars.Text = "You have $" + cash;
        }

        // Get the image associated with a specific wheel index
        private Image GetWheelImage(int index)
        {
            switch (index)
            {
                case 0: return imageWheel1;
                case 1: return imageWheel2;
                case 2: return imageWheel3;
                case 3: return imageWheel4;
                default: throw new ArgumentException("Invalid wheel index");
            }
        }

        // Handle the click event of the buttonAddCash
        private void buttonAddCash_Click(object sender, RoutedEventArgs e)
        {
            cash += AddCashAmount; // Add cash to the current amount
            UpdateCash(); // Update the displayed cash amount
            buttonPlay.Visibility = cash > 0 ? Visibility.Visible : Visibility.Collapsed; // Show or hide the play button based on the cash amount
        }

        // Handle the click event of the buttonPlay
        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            int[] rolls = new int[NumWheel]; // Array to store the rolled values of each wheel

            for (int i = 0; i < NumWheel; i++)
            {
                 rolls[i] = random.Next(1, 7); // Generate a random number for each wheel
                GetWheelImage(i).Source = new BitmapImage(new Uri($"ms-appx:///Assets/slots/slots_{rolls[i]}.png", UriKind.RelativeOrAbsolute)); // Set the image source for the wheel based on the rolled number
                
                cash -= WheelCost; // Deduct the wheel cost from the cash amount
            }

            UpdateCash(); // Update the displayed cash amount

            if (cash <= 0)
            {
                cash = 0;
                buttonPlay.Visibility = Visibility.Collapsed; // Hide the play button if the cash amount is zero or negative
            }

            #region Rules
            if (rolls[0] == rolls[1] && rolls[1] == rolls[2] && rolls[2] == rolls[3])
            {
                // Four of a kind: Win $4000 - Display win image.
                cash += 4000;
                imageWinLose.Source = new BitmapImage(new Uri($"ms-appx:///Assets/slots/win.png", UriKind.RelativeOrAbsolute));
            }
            if (rolls.Contains(1))
            {
                // Roll a one: Lose $10 - Display lose image.
                cash -= 10;
                imageWinLose.Source = new BitmapImage(new Uri($"ms-appx:///Assets/slots/lose.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                imageWinLose.Source = new BitmapImage(new Uri($"ms-appx:///Assets/slots/empty.png", UriKind.RelativeOrAbsolute));
            }

            UpdateCash();
            #endregion Rules
        }
        #endregion Methods

    }
}
