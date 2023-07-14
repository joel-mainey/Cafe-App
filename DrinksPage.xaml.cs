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
using Windows.UI.Xaml.Media.Imaging;
using Windows.Media.SpeechSynthesis;
using Windows.Media;
#endregion Using

namespace Cafe_App
{
    public sealed partial class DrinksPage : Page
    {
        #region Fields & Constants
        // Fields
        MediaElement media = new MediaElement(); // MediaElement for speech synthesis
        #endregion Fiels & Constants

        #region Dictionary
        // Dictionary to store drink information
        Dictionary<string, Drink> drinkDictionary = new Dictionary<string, Drink>()
        {
            // Key-value pairs representing drinks and their details
            {"Espresso", new Drink() { Name = "Espresso", ImageName = "espresso.png", About = "If you’re looking for a hard-hitting cup of coffee, or if you want a strong base for another coffee drink, espresso is for you. In short, espresso is a coffee-making method that pressurizes near-boiling water through finely-ground coffee beans. Because of how fine the grinds are, shots of espresso are some of the strongest, most aromatic, and caffeine-packed types of coffee available. You can order a shot of espresso as a single drink for a quick pick-me-up. Oftentimes, though, espresso shots are the coffee base for flavored beverages." } },
            {"Americano", new Drink() { Name = "Americano", ImageName = "americano.png", About = "If the flavor of espresso is just a bit too strong for your taste but you’re still looking for an unsweet beverage, try an Americano. This drink is simply a combination of espresso and hot water, creating a more mild, less intense taste. The ratio of water to espresso can vary based on how strong you want your coffee, but a typical Americano contains one or two shots of espresso with the rest of the cup being filled by hot water." } },
            {"Macchiato", new Drink() { Name = "Macchiato", ImageName = "macchiato.png", About = "A macchiato is a nice mix for coffee drinkers who like a touch of sweetness without losing the dominant flavor of espresso. This drink consists of a mixture of espresso and frothy milk, creating a noticeable layer of foam on top of the drink. In most cases, a small amount of foam is added to the espresso base. However, there are some variations of the coffee beverage in which the foam dominates the espresso." } },
            {"Cappuccino", new Drink() { Name = "Cappuccino", ImageName = "cappuccino.png", About = "This drink can be viewed as a slightly tamer version of a macchiato as steamed milk is added to the espresso and milk foam. The additional milk tames the bold flavor of the espresso a bit without completely removing it. Ideally, a cappuccino will consist of even amounts of all three ingredients and can be topped with cinnamon or another sweet addition." } },
            {"Latte", new Drink() { Name = "Latte", ImageName = "latte.png", About = "Upon first taste, you might mistake a latte for a cappuccino. And while the two coffee drinks do use the exact same ingredients, the ratios of each one cause some noticeable differences. Rather than including equal amounts of each ingredient like a cappuccino, the main element of a latte is the steamed milk. Depending on desired strength, mix one to two shots of espresso with six ounces of steamed milk, top it with a thin layer of foam, and add your desired flavor of syrup for a sweeter touch." } },
            {"Mocha", new Drink() { Name = "Mocha", ImageName = "mocha.png", About = "If you’re in the market for a coffee drink that barely tastes like coffee, mochas are for you! While most mochas do still contain two shots of espresso, the coffee flavor is largely masked by steamed milk, some form of chocolate syrup, and whipped cream. Mochas can be made with white chocolate, dark chocolate, and more, and they’re often mixed with other flavors for a sweet sensation." } }
        };
        #endregion Dictionary

        #region Constructor
        // Constructor
        public DrinksPage()
        {
            this.InitializeComponent();

            DrinksListBox.ItemsSource = drinkDictionary.Keys; // Set the item source for the list box

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
        // Perform speech synthesis for the given message using the provided MediaElement
        private async void Talk(string message, MediaElement media)
        {
            using (var reader = new SpeechSynthesizer())
            {
                var stream = await reader.SynthesizeTextToStreamAsync(message);
                media.SetSource(stream, stream.ContentType);
                media.Play();
            }
        }

        // Handle the selection change event of the DrinksListBox
        private void DrinksListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string itemSelected = DrinksListBox.SelectedItem.ToString();

            if (!drinkDictionary.TryGetValue(itemSelected, out var theDrink))
            {
                TextBoxName.Text += $"\nkey {itemSelected} not found";
                ImageDrink.Source = new BitmapImage(new Uri("ms-appx:///Assets/drinks/Empty Glass.png"));
            }
            else
            {
                TextBoxName.Text = theDrink.Name;
                ImageDrink.Source = new BitmapImage(new Uri($"ms-appx:///Assets/drinks/{theDrink.ImageName}"));
                TextBlockAbout.Text = theDrink.About;
            }
        }

        // Handle the double tap event of the DrinksListBox
        private void DrinksListBox_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var Drinkstring = $"{TextBoxName.Text} {TextBlockAbout.Text} ";
            Talk(Drinkstring, media);
        }
        #endregion Methods

    }
}

