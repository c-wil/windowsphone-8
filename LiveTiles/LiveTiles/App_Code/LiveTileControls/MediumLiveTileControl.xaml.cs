/// <copyright file="MediumLiveTileControl.xaml.cs">
/// Copyright (c) 2013 All Rights Reserved
/// </copyright>
/// 
/// <author>Christopher Wilson</author>
/// <version>1.0</version>
/// <date>12/23/2013</date>
/// 
/// <summary>
/// Code behind for MediumLiveTileControl.xaml
/// 
/// This class has one overloaded function that
/// sets the background and text of the control.
/// </summary>

using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LiveTiles.App_Code.LiveTileControls
{
    public partial class MediumLiveTileControl : UserControl
    {
        // Default Constructor
        public MediumLiveTileControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Overloaded constructor
        /// 
        /// Sets the background image and text for the control
        /// </summary>
        /// <param name="tileImage"></param>
        /// <param name="content"></param>
        public MediumLiveTileControl(BitmapImage tileImage, string content)
        {
            InitializeComponent();

            // set the page contents
            TileImage.ImageSource = tileImage;
            TileContent.Text = content;
        }
    }
}
