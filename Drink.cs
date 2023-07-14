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
    internal class Drink
    {
        // Fields
        // The name of the drink
        public string Name { get; set; }

        // The file name of the drink image
        public string ImageName { get; set; }

        // Information about the drink
        public string About { get; set; }

        // Constructors
        // Default constructor
        public Drink() { }

        // Parameterized constructor
        public Drink(string name, string imageName, string about)
        {
            Name = name;
            ImageName = imageName;
            About = about;
        }
    }
}
