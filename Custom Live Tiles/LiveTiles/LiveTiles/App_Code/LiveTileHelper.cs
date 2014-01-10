/// <copyright file="LiveTileHelper.cs">
/// Copyright (c) 2013 All Rights Reserved
/// </copyright>
/// 
/// <author>Christopher Wilson</author>
/// <version>1.2</version>
/// <date>12/23/2013</date>
/// 
/// <summary>
/// This helper class can create a medium live tile 
/// image and create new pinned tiles.
/// </summary>

// libraries
using Microsoft.Phone.Shell;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LiveTiles.App_Code
{
    public class LiveTileHelper
    {
        protected const string MEDIUM_LIVETILE_URI = "/Shared/ShellContent/LiveTile_MED_"; // standard file string for a live tile
        public int MAX_CONTENT_LENGTH = 60; // the max amount of characters on a medium tile

        // Default Constructor
        public LiveTileHelper() { }

        /// <summary>
        /// This function creates a new pinned tile.
        /// The only modified tile is the medium tile, which is based off 
        /// the file name provided.
        /// </summary>
        /// <param name="mediumTileUri">(string) the file name of the medium tile</param>
        public void UpdateLiveTile(string mediumTileUri)
        {
            // get the total count of pinned tiles
            int liveTileCount = ShellTile.ActiveTiles.Count() + 1;

            /* Check if the pinned tile already exists, create a 
             * new one if it doesn't.
             */
            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("ref=" + liveTileCount.ToString()));
            if (tile == null)
            {
                // set up the tile data based on the isolated storage file name provided
                FlipTileData tileData = SetTileData(new Uri("isostore:" + mediumTileUri, UriKind.Absolute), new Uri("/Assets/Tiles/FlipCycleTileLarge.png", UriKind.Relative));

                // create the live tile
                // NOTE: query string is needed in this case because only 1 pinned tile can reference an exact Uri
                ShellTile.Create((new Uri(("/MainPage.xaml?ref=" + liveTileCount), UriKind.Relative)), tileData, true);
            }
        }

        /// <summary>
        /// Sets up a new flip tile data object.
        /// This function can be exapnded to include more options for a tile
        /// such as modifiying other tile sizes
        /// </summary>
        /// <param name="mediumBackgroundImage">The uri for the medium tile image</param>
        /// <param name="wideBackgroundImage">The uri for the wide tile image</param>
        /// <returns>A FlipTileData object to be used for pinning a new tile</returns>
        protected FlipTileData SetTileData(Uri mediumBackgroundImage, Uri wideBackgroundImage)
        {
            FlipTileData tileData = new FlipTileData
            {
                SmallBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileSmall.png", UriKind.Relative),
                BackgroundImage = mediumBackgroundImage,
                WideBackgroundImage = wideBackgroundImage
            };

            return tileData;
        }

        /// <summary>
        /// Creates a bitmap image based on the parameters given 
        /// and the Medium Tile control.
        /// 
        /// NOTE: Current use is only creating pinned tile images.
        /// </summary>
        /// <param name="tileContent">The text on the image/tile</param>
        /// <param name="tileBackground">The background image of the tile</param>
        /// <returns>The file name of the image (to be used in a Uri object)</returns>
        public string RenderMediumLiveTile(string tileContent, BitmapImage tileBackground)
        {
            // trim the string to prevent the text from running off the tile
            if (tileContent.Length > MAX_CONTENT_LENGTH)
            {
                char[] newContent = tileContent.Take(MAX_CONTENT_LENGTH).ToArray();
                newContent[MAX_CONTENT_LENGTH - 3] = '.';
                newContent[MAX_CONTENT_LENGTH - 2] = '.';
                newContent[MAX_CONTENT_LENGTH - 1] = '.';
                tileContent = new string(newContent);
            }


            // Create a framework element based on the medium tile control
            var customTile = new App_Code.LiveTileControls.MediumLiveTileControl(tileBackground, tileContent);

            // set the measurements for element
            customTile.Measure(new Size(336, 336));
            customTile.Arrange(new Rect(0, 0, 336, 336));

            var bmp = new WriteableBitmap(336, 336);
            // render the bitmap
            bmp.Render(customTile, null);
            bmp.Invalidate();

            // update/create the image and store it in the users isolated storage within the app
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string filename = MEDIUM_LIVETILE_URI + (ShellTile.ActiveTiles.Count() + 1);
                if (store.FileExists(filename))
                    store.DeleteFile(filename);

                using (var stream = store.OpenFile(filename, FileMode.OpenOrCreate))
                {
                    // create the image
                    bmp.SaveJpeg(stream, 336, 336, 0, 100);
                }

                return filename;
            }
        }

    }
}
