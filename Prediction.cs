#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
#endregion Using

namespace Cafe_App
{
    internal class Prediction
    {
        // Fields
        // Random number generator instance
        private Random randomInstance = new Random();

        // Arrays
        // Array of time periods
        string[] timePeriods = new string[] { "thirty minutes", "an hour", "eight hours", "tewlve hours", "a day", "a week", "a month", "a year", "a decade" };

        // Array of aspects
        string[] aspects = new string[] { "finances", "lover life", "career prospects", "travel plans", "relationships" };

        // Array of effects
        string[] effects = new string[] { "fall apart", "exceed your expectation", "become awkward in an unexpected way", "become manageable", "become spectacular", "come to a positive outcome" };

        // Array of personas
        string[] personas = new string[] { "man", "boy", "woman", "girl", "dog", "bird", "hedehog", "singer", "relative" };

        // Array of features
        string[] features = new string[] { "pink hair", "a broken golden chain", "scary eyes", "long blond nose hair", "very red lips", "silver feet" };

        // Array of consequences
        string[] consequences = new string[] { "avoid looking at directly", "sing a sad song with", "stop and talk to", "dance with", "tell a secret", "buy a coffee" };

        // Methods
        // Generates a sentence with random elements from the arrays
        public string GetSentence()
        {
            return $"Over a period of {timePeriods[randomInstance.Next(timePeriods.Length)]}, " +
                   $"your {aspects[randomInstance.Next(aspects.Length)]} will {effects[randomInstance.Next(effects.Length)]}. " +
                   $"This will come to pass after you meet a {personas[randomInstance.Next(personas.Length)]} " +
                   $"with {features[randomInstance.Next(features.Length)]}, " +
                   $"who, for some reason, you find yourself obliged to {consequences[randomInstance.Next(consequences.Length)]}.";
        }
    }
}
