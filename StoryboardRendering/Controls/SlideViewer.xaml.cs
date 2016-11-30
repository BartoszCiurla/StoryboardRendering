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
using StoryboardRendering.Models;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace StoryboardRendering.Controls
{
    public sealed partial class SlideViewer : UserControl
    {
        public SlideViewer()
        {
            this.InitializeComponent();
        }

        #region SlideModel DP

        /// <summary>
        /// The <see cref="SlideModel" /> dependency property's name.
        /// </summary>
        public const string SlideModelPropertyName = "SlideModel";

        /// <summary>
        /// Gets or sets the value of the <see cref="SlideModel" />
        /// property. This is a dependency property.
        /// </summary>
        public SlideViewModel SlideModel
        {
            get
            {
                return (SlideViewModel)GetValue(SlideModelProperty);
            }
            set
            {
                SetValue(SlideModelProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="SlideModel" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty SlideModelProperty = DependencyProperty.Register(
            SlideModelPropertyName,
            typeof(SlideViewModel),
            typeof(SlideViewer),
            new PropertyMetadata(null, SlideModelPropertyChanged));

        #endregion SlideModel DP

        private static void SlideModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slideViewer = d as SlideViewer;
            var slide = e.NewValue as SlideViewModel;
            slideViewer.SlideNameTextBlock.Text = slide?.Id ?? "{BINDING ERROR}";
        }
    }
}
