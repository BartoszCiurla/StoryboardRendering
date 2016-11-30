using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace StoryboardRendering.Helper
{
    public class ScrollViewerManager
    {
        private const int HorizontalScrollViewer = 0;
        private const int VerticalScrollViewer = 1;
        private readonly List<ScrollViewer> _scrollViewers;
        private ScrollViewer VerticalScroll => _scrollViewers[VerticalScrollViewer];
        private ScrollViewer HorizontallScroll => _scrollViewers[HorizontalScrollViewer];

        public ScrollViewerManager(List<ScrollViewer> scrollViewers)
        {
            _scrollViewers = scrollViewers;
        }

        public void UpdateVerticalLayout()
        {
            VerticalScroll.UpdateLayout();
        }

        public void UpdateHorizontallLayout()
        {
            HorizontallScroll.UpdateLayout();
        }

        public bool ScrollVerticallyToPage(int page)
        {
            bool ignoreNextVerticalScrolling = true;
            bool changed = VerticalScroll.ChangeView(null, page * VerticalScroll.ActualHeight, null, true);
            if (!changed)
            {
                ignoreNextVerticalScrolling = false;
            }
            return ignoreNextVerticalScrolling;
        }

        public bool ScrollHorizontallyToPage(int page)
        {
            bool ignoreNextHorizontalScrolling = true;
            bool changed = HorizontallScroll.ChangeView(page * HorizontallScroll.ActualWidth,null, null, true);
            if (!changed)
            {
                ignoreNextHorizontalScrolling = false;
            }
            return ignoreNextHorizontalScrolling;
        }
    }
}
