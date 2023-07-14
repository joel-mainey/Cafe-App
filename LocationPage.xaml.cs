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
using Windows.Services.Maps;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Media.SpeechSynthesis;
using System.Globalization;
#endregion Using

namespace Cafe_App
{
    public sealed partial class LocationPage : Page
    {
        // Constants & Fields

        // Push Pin Streams
        RandomAccessStreamReference mapIconStreamReference1;
        RandomAccessStreamReference mapIconStreamReference2;
        RandomAccessStreamReference mapIconStreamReference3;
        int IconUsed = 0;
        // Billboard Stream
        RandomAccessStreamReference mapBillboardStreamReference;
        // XAML controles
        bool ShowControls;
        Border aBorder = new Border();
        Image aImage = new Image();
        TextBlock aTextBlock = new TextBlock();
        // Route selection
        bool ShowRoute;
        //SpeechSynthesizer reader;


        // Constructor
        public LocationPage()
        {
            this.InitializeComponent();

            // Push Pin images
            mapIconStreamReference1 = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MapPin1.png"));
            mapIconStreamReference2 = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MapPin2.png"));
            mapIconStreamReference3 = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MapPin3.png"));
            // Billboard image
            mapBillboardStreamReference = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Waterfall.png"));

            // Controls selection
            ShowControls = true;
            // Route selection
            ShowRoute = true;
            //reader = new SpeechSynthesizer();

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

        private void GotoMainPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        #endregion Extra

        // Methods
        private void Map_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the start up position to my house
            Map.Center = new Geopoint(new BasicGeoposition { Latitude = -37.714440, Longitude = 176.331600 });
        }

        private async void SetMapStyle(int selection, MapControl aMap)
        {
            if (selection == 1) aMap.Style = MapStyle.None;
            else if (selection == 2) aMap.Style = MapStyle.Road;
            else if (selection == 3) aMap.Style = MapStyle.Aerial;
            else if (selection == 4) aMap.Style = MapStyle.AerialWithRoads;
            else if (selection == 5) aMap.Style = MapStyle.Terrain;
            else if (selection == 6)
            {
                if (aMap.Is3DSupported)
                {
                    BasicGeoposition hwGeoposition = new BasicGeoposition()
                    {
                        Latitude = aMap.Center.Position.Latitude,
                        Longitude = aMap.Center.Position.Longitude
                    };
                    Geopoint hwPoint = new Geopoint(hwGeoposition);

                    // Set the Geopoint, show the many meters around, looking at it to the North, degrees pitch
                    MapScene hwScene = MapScene.CreateFromLocationAndRadius(hwPoint, 80, 0, 60);

                    aMap.Style = MapStyle.Aerial3D;

                    // Set the 3D view with animation.
                    await aMap.TrySetSceneAsync(hwScene, MapAnimationKind.Bow);
                }
                else
                {
                    var messageDialog = new MessageDialog("Not curretly supported", "Aerial3D");
                    await messageDialog.ShowAsync();
                }
            }
            else if (selection == 7)
            {
                if (aMap.Is3DSupported)
                {
                    BasicGeoposition hwGeoposition = new BasicGeoposition()
                    {
                        Latitude = aMap.Center.Position.Latitude,
                        Longitude = aMap.Center.Position.Longitude
                    };
                    Geopoint hwPoint = new Geopoint(hwGeoposition);

                    // Set the Geopoint, show the many meters around, looking at it to the North, degrees pitch
                    MapScene hwScene = MapScene.CreateFromLocationAndRadius(hwPoint, 80, 0, 60);

                    aMap.Style = MapStyle.Aerial3DWithRoads;

                    // Set the 3D view with animation.
                    await aMap.TrySetSceneAsync(hwScene, MapAnimationKind.Bow);
                }
                else
                {
                    var messageDialog = new MessageDialog("Not currently supported", "Aerial3DWithRoads");
                    await messageDialog.ShowAsync();
                }
            }
        }

        private void ButtonStreetView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Map.CustomExperience = null;

            ShowStreetView(Map);
        }

        public async void ShowStreetView(MapControl aMap)
        {
            // Check if Streetside is supported.
            if (aMap.IsStreetsideSupported)
            {
                BasicGeoposition cityPosition = new BasicGeoposition()
                {
                    Latitude = aMap.Center.Position.Latitude,
                    Longitude = aMap.Center.Position.Longitude
                };

                // Find a panorama near Avenue Gustave Eiffel.
                Geopoint cityCenter = new Geopoint(cityPosition);
                StreetsidePanorama panoramaNearCity = await StreetsidePanorama.FindNearbyAsync(cityCenter);

                aMap.Center = cityCenter;
                aMap.Style = MapStyle.Aerial3DWithRoads;

                // Set the Streetside view if a panorama exists.
                if (panoramaNearCity != null)
                {
                    // Create the Streetside view.
                    StreetsideExperience ssView = new StreetsideExperience(panoramaNearCity);
                    ssView.OverviewMapVisible = true;
                    aMap.CustomExperience = ssView;
                }
                else
                {
                    var messageDialog = new MessageDialog("Street View is not supported", "Street views are not supported at this location.");
      
                    await messageDialog.ShowAsync();
                }
            }
            else
            {
                var messageDialog = new MessageDialog("Streetside is not supported", "Streetside views are not supported on this device.");
     
                await messageDialog.ShowAsync();
            }
        }

        private void TrafficFlowVisible_Checked(object sender, RoutedEventArgs e)
        {
            // Set the traffic flow On on the Left map
            Map.TrafficFlowVisible = true;
        }

        private void trafficFlowVisibleCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Set the traffic flow Off on the Left map
            Map.TrafficFlowVisible = false;
        }

        private void InnerFlyoutButton_Click(object sender, RoutedEventArgs e)
        {
            MapIcon mapIcon1 = new MapIcon();
            mapIcon1.Location = Map.Center;
            mapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon1.Title = LocationData.Text;

            // If none selected icon defaults to the circle
            if (IconUsed == 1) mapIcon1.Image = mapIconStreamReference1;
            else if (IconUsed == 2) mapIcon1.Image = mapIconStreamReference2;
            else if (IconUsed == 3) mapIcon1.Image = mapIconStreamReference3;

            mapIcon1.ZIndex = 0;
            Map.MapElements.Add(mapIcon1);

            PinFlyout.Hide();

            Icon1.Opacity = 0.2;
            Icon2.Opacity = 0.2;
            Icon3.Opacity = 0.2;

            LocationData.Text = "My Location";

            IconUsed = 0;
        }

        private void Icon1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Icon1.Opacity = 1.0;
            Icon2.Opacity = 0.2;
            Icon3.Opacity = 0.2;
            IconUsed = 1;
        }

        private void Icon2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Icon1.Opacity = 0.2;
            Icon2.Opacity = 1.0;
            Icon3.Opacity = 0.2;
            IconUsed = 2;
        }


        private void Icon3_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Icon1.Opacity = 0.2;
            Icon2.Opacity = 0.2;
            Icon3.Opacity = 1.0;
            IconUsed = 3;
        }

        private void mapBillboardAddButton_Click(object sender, RoutedEventArgs e)
        {
            // Create MapBillboard
            // MapBillboard scales with respect to the perspective projection of the 3D camera.
            // The reference camera determines at what view distance the billboard image appears at 1x scale.
            MapBillboard mapBillboard = new MapBillboard(Map.ActualCamera);
            mapBillboard.Location = Map.Center;
            mapBillboard.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapBillboard.Image = mapBillboardStreamReference;
            Map.MapElements.Add(mapBillboard);
        }

        private async void FindRouteButton_Click(object sender, RoutedEventArgs e)
        {
            // If the street view is not on then display
            if (Map.CustomExperience == null)
            {
                // Toggle the button to add or remove the Route elements from the map
                if (ShowRoute == true)
                {
                    // Note: Routs must have a START and END point located on a STREET and have a driveable route.

                    // Start at the top of the North Island and end at the bottom of the South Island
                    BasicGeoposition startLocation = new BasicGeoposition()
                    {
                        Latitude = -34.42988,
                        Longitude = 172.681964
                    };
                    BasicGeoposition endLocation = new BasicGeoposition()
                    {
                        Latitude = -46.57722,
                        Longitude = 168.8155613
                    };

                    // Start at the poly and end at the Mount if you want to test another route
                    // BasicGeoposition startLocation = new BasicGeoposition() { Latitude = -37.73158, Longitude = 176.147048023350 };

                    //BasicGeoposition endLocation = new BasicGeoposition() { Latitude = -37.630435, Longitude = 176.17597 };

            // Get the route between the points.
            MapRouteFinderResult routeResult = await MapRouteFinder.GetDrivingRouteAsync(
                                                                                       new Geopoint(startLocation),
                                                                                       new Geopoint(endLocation),
                                                                                       MapRouteOptimization.Time,
                                                                                       MapRouteRestrictions.None);
            // If we retrieved a route
            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                System.Text.StringBuilder routeInfo = new System.Text.StringBuilder();

                // Display summary info about the route.
                routeInfo.Append("Estimated time: ");
                routeInfo.Append(routeResult.Route.EstimatedDuration.Days.ToString());
                routeInfo.Append(" d, ");
                routeInfo.Append(routeResult.Route.EstimatedDuration.Hours.ToString());
                routeInfo.Append(" h, ");
                routeInfo.Append(routeResult.Route.EstimatedDuration.Minutes.ToString());
                routeInfo.Append(" m. ");
                routeInfo.Append("\nLength: ");
                routeInfo.Append((routeResult.Route.LengthInMeters / 1000).ToString());
                routeInfo.Append(" kilometres");

                // Display the directions.
                routeInfo.Append("\n\nDIRECTIONS\n");

                foreach (MapRouteLeg leg in routeResult.Route.Legs)
                {
                    foreach (MapRouteManeuver maneuver in leg.Maneuvers)
                    {
                        routeInfo.AppendLine(maneuver.InstructionText);
                    }
                }

                // Load the text box.
                FindRouteTextBlock.Text = routeInfo.ToString();

                // Can also output to a message dialog if you want (could also add text-to-speech here)
                //var messageDialog = new MessageDialog(routeInfo.ToString(), "Route");
                //await messageDialog.ShowAsync();

                // Use the route to initialize a MapRouteView.
                MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                viewOfRoute.RouteColor = Colors.Yellow;
                viewOfRoute.OutlineColor = Colors.Black;

                // Add the new MapRouteView to the Routes collection of the MapControl.
                Map.Routes.Add(viewOfRoute);

                // Fit the MapControl to the route.
                await Map.TrySetViewBoundsAsync(routeResult.Route.BoundingBox, null, MapAnimationKind.None);

                // Reset the button
                FindRouteButton.Content = "Remove Route";
                ShowRoute = false;

                // Play  the text message
                //Talk(FindRouteTextBlock.Text, 2);
            }
            else
            {
                var messageDialog = new MessageDialog("The START and END points must be located on a STREET and the route is driveable.", "Route Not found");

                await messageDialog.ShowAsync();
            }
            }
            else
            {
               // Remove the Route elements from the map
               Map.Routes.Clear();

               // Reset the Route Information
               FindRouteTextBlock.Text = "Route Information";

               // Reset the button
               FindRouteButton.Content = "Add Route";
               ShowRoute = true;

               // Stop  the text message
               //Talk(FindRouteTextBlock.Text, 0);               
            }
            }
            else
            {
                // We are in Street view so do not display the route elements
                FindRouteButton.Opacity = 0.2;
                var messageDialog = new MessageDialog("Exit Streetside views to access this option.",
                                                        "Streetside is displayed");
                await messageDialog.ShowAsync();
                FindRouteButton.Opacity = 1.0;
            }
            }

        private void FindRouteScrollViewer_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //    media.Play();
        }

        private void FindRouteScrollViewer_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //    media.Pause();
        }

        ///// <summary>
        ///// Play a message
        ///// </summary>
        ///// <param name="message"></param> The message to play
        ///// <param name="PlayMode"></param> 0 = stop playback, 1 = play playback
        //private async void Talk(string message, int PlayMode)
        //{
        //    // Get the text
        //    var stream = await reader.SynthesizeTextToStreamAsync(message);
        //    // Setup the stream for the player
        //    media.SetSource(stream, stream.ContentType);

        //    // Play or stop the message
        //    if(PlayMode == 1)   media.Play();
        //    else                media.Stop();
        //}

        private void ClearMapButton_Click(object sender, RoutedEventArgs e)
        {
            Map.MapElements.Clear();
        }

        private async void ButtonFinfAddress_Click(object sender, RoutedEventArgs e)
        {
            // The address or business to geocode.
            string addressToGeocode = TextBoxInputAddress.Text;
            // Test Strings
            //string addressToGeocode = "46 Lagoon Place, Tauranga 3112, New Zealand";
            //string addressToGeocode = "48 Queen Street, Auckland 1010, New Zealand";

            // The nearby location to use as a query hint - We will use the current center of the map.
            BasicGeoposition queryHint = new BasicGeoposition();
            queryHint.Latitude = Map.Center.Position.Latitude;
            queryHint.Longitude = Map.Center.Position.Longitude;
            Geopoint hintPoint = new Geopoint(queryHint);

            // Geocode the specified address, using the specified reference point
            // as a query hint. Return no more than 3 results.
            MapLocationFinderResult result = await MapLocationFinder.FindLocationsAsync(addressToGeocode, hintPoint, 3);

            // If the query returns results, display the coordinates of the first result.
            if (result.Status == MapLocationFinderStatus.Success)
            {
                try
                {
                    Map.DesiredPitch = 0;
                    Map.ZoomLevel = 19;

                    Map.Center = new Geopoint(new BasicGeoposition
                    {
                        Latitude = result.Locations[0].Point.Position.Latitude,
                        Longitude = result.Locations[0].Point.Position.Longitude
                    });
                }
                catch (ArgumentOutOfRangeException exc)
                {
                    var messageDialog = new MessageDialog("This is not a valid address, please use the following formats:\n15 Palm Springs Boulevard, Papamoa 3118, New Zealand\n48 Queen Street, Auckland 1010, New Zealand\n\nAlso please centre the map on the country where the address is located.", "Address Retrieval");
     
                    await messageDialog.ShowAsync();
                }
            }
        }



        #region GUI Interaction methods
        private void LocationListStackPanel_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            LocationListStackPanel.Opacity = 1.0;
        }

        private void LocationListStackPanel_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            LocationListStackPanel.Opacity = 0.5;
        }

        private void PinStackPanel_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            PinStackPanel.Opacity = 1.0;
        }

        private void PinStackPanel_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            PinStackPanel.Opacity = 0.5;
        }

        private void ComplexStackPanel_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ComplexStackPanel.Opacity = 1.0;
        }

        private void ComplexStackPanel_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ComplexStackPanel.Opacity = 0.5;
        }

        private void AddressInputStackPanel_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            AddressInputStackPanel.Opacity = 1.0;
        }

        private void AddressInputStackPanel_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            AddressInputStackPanel.Opacity = 0.5;
        }
        #endregion GUI Interaction methods
    }
}

