using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using StoryboardRendering.Helper;
using StoryboardRendering.Models;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace StoryboardRendering.Controls
{
    public sealed partial class SlidesListViewer : UserControl
    {
        private SectionsManager _sectionsManager;
        private Size _viewPortSize;
        private readonly List<Border> _sectionsPlaceHoldersContainer;
        private readonly List<Border> _slidesPlaceHoldersContainer;
        private readonly List<ScrollViewer> _scrollViewers;
        private double _lastOffsetY = 0.0;
        private double _lastOffsetX = 0.0;
        public SlidesListViewer()
        {
            this.InitializeComponent();
            _slidesPlaceHoldersContainer = new List<Border>
            {
                PrevSectionCurSlide,
                CurSectionPrevSlide,
                CurSectionCurSlide,
                CurSectionNextSlide,
                NextSectionCurrSlide
            };

            _sectionsPlaceHoldersContainer = new List<Border>
            {
                PrevSection,
                CurSection,
                NextSection
            };

            _scrollViewers = new List<ScrollViewer>
            {
                HorizontalScrollViewer,
                VerticalScrollViewer
            };

            Loaded += SlidesListControl_Loaded;
            FillPlaceholdersSizes();
            HorizontalScrollViewer.ViewChanged += HorizontalScrollViewer_ViewChanged;
            VerticalScrollViewer.ViewChanged += VerticalScrollViewer_ViewChanged;
        }

        private void VerticalScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (!e.IsIntermediate)
            {
                var currentOffsetY = VerticalScrollViewer.VerticalOffset;
                var dir = GetScrollingDirection(_lastOffsetY, currentOffsetY);
                _lastOffsetY = currentOffsetY;

                if (!_sectionsManager.IgnoreNextVerticalScrolling)
                {

                    if (dir == 0)
                    {
                        return;
                    }

                    if (dir < 0)
                    {
                        _sectionsManager.SetPrevSlide();
                    }
                    else
                    {
                        _sectionsManager.SetNextSlide();
                    }

                }
                else
                {
                    _sectionsManager.IgnoreNextVerticalScrolling = false;
                }
            }
        }

        private void HorizontalScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (!e.IsIntermediate)
            {
                var currentOffetX = HorizontalScrollViewer.HorizontalOffset;
                var dir = GetScrollingDirection(_lastOffsetX, currentOffetX);
                _lastOffsetX = currentOffetX;

                if (!_sectionsManager.IgnoreNextHorizontalScrolling)
                {
                    if (dir == 0)
                        return;

                    if (dir < 0)
                    {
                        _sectionsManager.SetPrevSection();
                    }
                    else
                    {
                        _sectionsManager.SetNextSection();
                    }
                }
                else
                {
                    _sectionsManager.IgnoreNextHorizontalScrolling = false;
                }
            }
        }

        private double GetScrollingDirection(double lastOffsetY, double currentOffsetY)
        {
            if (lastOffsetY == currentOffsetY)
                return 0;

            var offset = currentOffsetY - lastOffsetY;

            return offset < 0 ? -1 : 1;
        }

        private void SlidesListControl_Loaded(object sender, RoutedEventArgs e)
        {
            #region todo remove after tests

            string slideId = "Section 2, slide 9";
            string otherVersion = "Section 2, slide 1";
            string otherVersion2 = "Section 2, slide 5";
            string otherVersion3 = "Section 2, slide 2";
            string otherVersion4 = "Section 5, slide 0";

            string otherVersion5 = "Section 9, slide 9";

            string emptySlideId = "";
            #endregion todo remove after tests    

            FillPlaceholdersSizes();
            _sectionsManager = new SectionsManager(SectionsList, emptySlideId);
            _sectionsManager
                .Init(_sectionsPlaceHoldersContainer,
                _slidesPlaceHoldersContainer,
                _scrollViewers);
        }

        private void FillPlaceholdersSizes()
        {
            _viewPortSize = new Size(HorizontalScrollViewer.ActualWidth, HorizontalScrollViewer.ActualHeight);

            _slidesPlaceHoldersContainer
                .ForEach(SetSizeForPlaceholder);

            _sectionsPlaceHoldersContainer
                .ForEach(SetSizeForPlaceholder);
        }

        private void SetSizeForPlaceholder(Border placeHolder)
        {
            placeHolder.Width = _viewPortSize.Width;
            placeHolder.Height = _viewPortSize.Height;
        }

        #region SectionsList DP
        public const string SectionsListPropertyName = "SectionsList";
        private static void SectionsListPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slidesListViewer = d as SlidesListViewer;
            var sectionsList = e.NewValue as List<SectionViewModel>;                     
        }

        public List<SectionViewModel> SectionsList
        {
            get
            {
                return (List<SectionViewModel>)GetValue(SectionsListProperty);
            }
            set
            {
                SetValue(SectionsListProperty, value);
            }
        }

        public static readonly DependencyProperty SectionsListProperty = DependencyProperty.Register(
            SectionsListPropertyName,
            typeof(List<SectionViewModel>),
            typeof(SlidesListViewer),
            new PropertyMetadata(null, SectionsListPropertyChanged));

        #endregion SectionsListDP
    }
}
