/// <copyright file="MainPage.xaml.cs">
/// Copyright (c) 2014 All Rights Reserved
/// </copyright>
/// 
/// <author>Christopher Wilson</author>
/// <version>1.0</version>
/// <date>1/10/2014</date>
/// 
/// <summary>
/// Controls the events triggered from MainPage.xaml.
/// This class controls when the user selects a new image 
/// and when the user wants to create a new pinned tile.
/// </summary>

using LiveTiles.App_Code;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LiveTiles
{
    public partial class MainPage : PhoneApplicationPage
    {
        readonly PhotoChooserTask photoChooserTask = new PhotoChooserTask();
       
        /// <summary>
        /// Holds the user's selected image
        /// </summary>
        BitmapImage TileImage { get; set; }

        /// <summary>
        /// Used for handling the creation of pinned tiles.
        /// </summary>
        LiveTileHelper helper { get; set; }

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // initialize the helper class
            helper = new LiveTileHelper();

            // register the photo chooser event handler
            photoChooserTask.Completed += new EventHandler<PhotoResult>(HandlePhotoChooserTaskCompleted);
        }

        /// <summary>
        /// Creates a new pinned tile to start based on the content
        /// in the text box and the image in the TileImage property.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PinButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // create the medium tile image and set the file name
            string liveTileFileName = helper.RenderMediumLiveTile(TileContentTextbox.Text, TileImage);
            
            // create the live tile and pin it to start
            helper.UpdateLiveTile(liveTileFileName);
        }

        /// <summary>
        /// Opens the photo chooser task so the user can 
        /// select a new image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PicturePickerButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            photoChooserTask.Show();
        }

        /// <summary>
        /// Handler to handle when the user has finished with the photo chooser.
        /// If the user selects a new image, the TileImage property is set and 
        /// the page background is updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandlePhotoChooserTaskCompleted(object sender, PhotoResult e)
        {
            // check if the user picked an image
            if (e.TaskResult == TaskResult.OK)
            {
                if (TileImage == null)
                    TileImage = new BitmapImage();

                // set the image source of the property
                TileImage.SetSource(e.ChosenPhoto);

                // update the page background
                BackgroundImage.ImageSource = TileImage;
            }
        }
    }
}