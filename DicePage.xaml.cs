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
using System.Windows;
using Windows.UI.ViewManagement;
#endregion Using

namespace Cafe_App
{
    public sealed partial class DicePage : Page
    {
        #region Constants & Fields
        // Constants
        private const int NumDice = 5; // Number of dice used for player 1
        private const int NumDice2 = 5; // Number of dice used for player 2
        private const int DiceCost = 20; // Cost to roll the dice
        private const int InitialCash = 0; // Initital cash amount
        private const int AddCashAmount = 100; // Amount of cash added when the "Add Cash" button is clicked

        // Fields
        private Random random = new Random(); // Random number generator for rolling the dice
        private int cash = InitialCash; // Current cash amount
        #endregion Constants & Fields

        #region Constructor
        // Constructor
        public DicePage()
        {
            this.InitializeComponent();

            player1Roll.Visibility = Visibility.Collapsed;  
            player2Roll.Visibility = Visibility.Collapsed;  
            UpdateCash();

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
        // Update the displayed cash amount
        private void UpdateCash()
        {
            textBlockDollars.Text = "You have $" + cash;
        }

        // Get the dice image control based on the index for player 1
        private Image GetDiceImage(int index)
        {
            switch (index)
            {
                case 0: return imageDice1;
                case 1: return imageDice2;
                case 2: return imageDice3;
                case 3: return imageDice4;
                case 4: return imageDice5;
                default: throw new ArgumentException("Invalid dice index");
            }
        }

        // Get the dice image control based on the index for player 2
        private Image GetDiceImage2(int index)
        {
            switch (index)
            {
                case 0: return imageDice6;
                case 1: return imageDice7;
                case 2: return imageDice8;
                case 3: return imageDice9;
                case 4: return imageDice10;
                default: throw new ArgumentException("Invalid dice index");
            }
        }

        // Add cash to the player's balance
        private void buttonAddCash_Click(object sender, RoutedEventArgs e)
        {
            cash += AddCashAmount;                                                                    
            UpdateCash(); // Update the displayed cash amount                                                                             
            player1Roll.Visibility = cash > 0 ? Visibility.Visible : Visibility.Collapsed; // Show or hide the player 1 play button based on the cash amount            
            player2Roll.Visibility = cash > 0 ? Visibility.Visible : Visibility.Collapsed; // Show or hide the player 2 play button based on the cash amount
        }

        // Roll the dice for player 1
        private void player1Roll_Click(object sender, RoutedEventArgs e)
        {
            int[] rolls = new int[NumDice]; // Array to store the rolled dice values

            for (int i = 0; i < NumDice; i++)
            {
                rolls[i] = random.Next(1, 7); // Roll the dice and get a random value

                cash -= DiceCost; // Subtract the dice cost from the player's cash

                Image diceImage = GetDiceImage(i); // Get the dice image control
                diceImage.Source = new BitmapImage(new Uri($"ms-appx:///Assets/dice/dice_{rolls[i]}.png", UriKind.RelativeOrAbsolute)); // Set the image source based on the rolled value
            }

            UpdateCash(); // Update the displayed cash amount

            if (cash <= 0)
            {
                cash = 0;
                player1Roll.Visibility = Visibility.Collapsed; // Hide the roll button if the player has no cash left
            }

            #region Rules
            if (rolls[0] == rolls[1] && rolls[1] == rolls[2] && rolls[2] == rolls[3] && rolls[3] == rolls[4])
            {
                // Five of a kind: Win $5000.
                cash += 5000;
            }
            else if (rolls.Contains(1) && rolls.Contains(2) && rolls.Contains(3) && rolls.Contains(4) && rolls.Contains(5))
            {
                // 1 – 2 – 3 – 4 – 5: Win $1500.
                cash += 1500;
            }
            else if (rolls.Contains(2) && rolls.Contains(3) && rolls.Contains(4) && rolls.Contains(5) && rolls.Contains(6))
            {
                // 2 – 3 – 4 – 5 – 6: Win $1500.
                cash += 1500;
            }
            else if (rolls[0] == rolls[1] && rolls[1] == rolls[2] && rolls[3] == rolls[4])
            {
                // Three of a kind and a pair: Win $1000.
                cash += 1000;
            }
            else if (rolls[0] == rolls[1] && rolls[1] == rolls[2])
            {
                // Three of a kind: Win $500.
                cash += 500;
            }
            else if (rolls[0] == rolls[1] && rolls[2] == rolls[3] && rolls[3] == rolls[4])
            {
                // Pair and three of a kind: Win $500.
                cash += 500;
            }
            else if (rolls[0] == rolls[1] && rolls[2] == rolls[3] && rolls[3] == rolls[4])
            {
                // Pair and three of a kind: Win $500.
                cash += 500;
            }
            else if (rolls[0] == rolls[2] && rolls[2] == rolls[4])
            {
                // Three of a kind: Win $500.
                cash += 500;
            }

            UpdateCash();
            #endregion Rules
        }

        //Roll the dice for player 2
        private void player2Roll_Click(object sender, RoutedEventArgs e)
        {
            int[] rolls = new int[NumDice2]; // Array to store the rolled dice values

            for (int i = 0; i < NumDice2; i++)
            {
                rolls[i] = random.Next(1, 7); // Roll the dice and get a random value

                cash -= DiceCost; // Subtract the dice cost from the player's cash

                Image diceImage = GetDiceImage2(i); // Get the dice image control
                diceImage.Source = new BitmapImage(new Uri($"ms-appx:///Assets/dice/dice_{rolls[i]}.png", UriKind.RelativeOrAbsolute)); // Set the image source based on the rolled value
            }

            UpdateCash(); // Update the displayed cash amount

            if (cash <= 0)
            {
                cash = 0;
                player2Roll.Visibility = Visibility.Collapsed; // Hide the roll button if the player has no cash left
            }

            #region Rules
            if (rolls[0] == rolls[1] && rolls[1] == rolls[2] && rolls[2] == rolls[3] && rolls[3] == rolls[4])
            {
                // Five of a kind: Win $5000.
                cash += 5000;
            }
            else if (rolls.Contains(1) && rolls.Contains(2) && rolls.Contains(3) && rolls.Contains(4) && rolls.Contains(5))
            {
                // 1 – 2 – 3 – 4 – 5: Win $1500.
                cash += 1500;
            }
            else if (rolls.Contains(2) && rolls.Contains(3) && rolls.Contains(4) && rolls.Contains(5) && rolls.Contains(6))
            {
                // 2 – 3 – 4 – 5 – 6: Win $1500.
                cash += 1500;
            }
            else if (rolls[0] == rolls[1] && rolls[1] == rolls[2] && rolls[3] == rolls[4])
            {
                // Three of a kind and a pair: Win $1000.
                cash += 1000;
            }
            else if (rolls[0] == rolls[1] && rolls[1] == rolls[2])
            {
                // Three of a kind: Win $500.
                cash += 500;
            }
            else if (rolls[0] == rolls[1] && rolls[2] == rolls[3] && rolls[3] == rolls[4])
            {
                // Pair and three of a kind: Win $500.
                cash += 500;
            }
            else if (rolls[0] == rolls[1] && rolls[2] == rolls[3] && rolls[3] == rolls[4])
            {
                // Pair and three of a kind: Win $500.
                cash += 500;
            }
            else if (rolls[0] == rolls[2] && rolls[2] == rolls[4])
            {
                // Three of a kind: Win $500.
                cash += 500;
            }

            UpdateCash();
            #endregion Rules
        }
        #endregion Methods 

    }
}

