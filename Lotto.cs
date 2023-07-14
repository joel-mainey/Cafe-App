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
    internal class Lotto
    {
        // Fields
        // An array to store the generated numbers
        private readonly int[] numArray;

        // Random number generator
        private readonly Random randomNumber;

        // Constructor
        public Lotto()
        {
            numArray = new int[6];
            randomNumber = new Random(DateTime.Now.Millisecond);
        }

        // Methods
        // Generates random numbers for the lotto
        public void GenerateNumbers()
        {
            for (int i = 0; i < numArray.Length; i++)
            {
                numArray[i] = randomNumber.Next(1, 50);
            }
        }

        // Prints the generated numbers to the specified TextBlock
        public void PrintNumbers(TextBlock OutputTextBlock)
        {
            string formattedNumbers = string.Join("  ", numArray.Select(n => n.ToString("D2")));
            OutputTextBlock.Text += formattedNumbers.Replace(" ", "    ");
        }

        // Resets the generated numbers to zero
        public void SetNumbersToZero()
        {
            Array.Clear(numArray, 0, numArray.Length);
        }

        // Sorts the generated numbers in ascending order
        public void SortNumbers()
        {
            Array.Sort(numArray);
        }
    }
}
