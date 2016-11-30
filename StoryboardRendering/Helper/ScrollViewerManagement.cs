using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace StoryboardRendering.Helper
{
    public class ScrollViewerManagement
    {
        private Logger _logger = new Logger();
        private const int HorizontalScrollViewer = 0;
        private const int VerticalScrollViewer = 1;
        private List<ScrollViewer> _scrollViewers;

        public ScrollViewerManagement(List<ScrollViewer> scrollViewers)
        {
            _scrollViewers = scrollViewers;
        }

        public void UpdateVerticalLayout()
        {
            _logger.Log("Before update vertical layout\n"
                + GetVerticalScroll().ActualHeight + " Height \n" +
                + GetVerticalScroll().ScrollableHeight + " Scrollable height \n" +
                + GetVerticalScroll().VerticalOffset + "Verticall offset \n");

            GetVerticalScroll().UpdateLayout();

            _logger.Log("After update vertical layout\n"
                + GetVerticalScroll().ActualHeight + " Height \n" +
                +GetVerticalScroll().ScrollableHeight + " Scrollable height \n" +
                +GetVerticalScroll().VerticalOffset + "Verticall offset \n");
        }

        private ScrollViewer GetVerticalScroll() => _scrollViewers[VerticalScrollViewer];

        public bool ScrollVerticallyToPage(int page)
        {
            bool ignoreNextVerticalScrolling = true;
            bool changed = GetVerticalScroll().ChangeView(null,
                page * GetVerticalScroll().ActualHeight, null, true);
            if (!changed)
            {
                ignoreNextVerticalScrolling = false;
            }
            return ignoreNextVerticalScrolling;
        }


    }
}
