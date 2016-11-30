using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace StoryboardRendering.Helper
{
    public class ScrollViewerManagement
    {
        private Logger _logger = new Logger();
        private const int HorizontalScrollViewer = 0;
        private const int VerticalScrollViewer = 1;
        private readonly List<ScrollViewer> _scrollViewers;
        private ScrollViewer VerticalScroll => _scrollViewers[VerticalScrollViewer];
        private ScrollViewer HorizontallScroll => _scrollViewers[HorizontalScrollViewer];

        public ScrollViewerManagement(List<ScrollViewer> scrollViewers)
        {
            _scrollViewers = scrollViewers;
        }

        public void UpdateVerticalLayout()
        {
            _logger.Log("Before update vertical layout\n"
                + VerticalScroll.ActualHeight + " Height \n" +
                +VerticalScroll.ScrollableHeight + " Scrollable height \n" +
                +VerticalScroll.VerticalOffset + "Verticall offset \n");

            VerticalScroll.UpdateLayout();

            _logger.Log("After update vertical layout\n"
                + VerticalScroll.ActualHeight + " Height \n" +
                +VerticalScroll.ScrollableHeight + " Scrollable height \n" +
                +VerticalScroll.VerticalOffset + "Verticall offset \n");
        }

        public void UpdateHorizontallLayout()
        {
            _logger.Log("Before update vertical layout\n"
               + HorizontallScroll.ActualWidth + " Width \n" +
               +HorizontallScroll.ScrollableWidth + " Scrollable Width \n" +
               +HorizontallScroll.HorizontalOffset + " Horizontall offset \n");

            HorizontallScroll.UpdateLayout();

            _logger.Log("Before update vertical layout\n"
               + HorizontallScroll.ActualWidth + " Width \n" +
               +HorizontallScroll.ScrollableWidth + " Scrollable Width \n" +
               +HorizontallScroll.HorizontalOffset + " Horizontall offset \n");
        }

        public bool ScrollVerticallyToPage(int page)
        {
            bool ignoreNextVerticalScrolling = true;
            bool changed = VerticalScroll.ChangeView(null,
                page * VerticalScroll.ActualHeight, null, true);
            if (!changed)
            {
                ignoreNextVerticalScrolling = false;
            }
            return ignoreNextVerticalScrolling;
        }

        public bool ScrollHorizontallyToPage(int page)
        {
            bool ignoreNextHorizontalScrolling = true;
            bool changed = HorizontallScroll.ChangeView(page * HorizontallScroll.ActualWidth, 
                null, null, true);
            if (!changed)
            {
                ignoreNextHorizontalScrolling = false;
            }
            return ignoreNextHorizontalScrolling;
        }
    }
}
