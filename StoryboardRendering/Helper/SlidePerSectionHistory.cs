using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryboardRendering.Helper
{
    public class SlidePerSectionHistory
    {
        private readonly int?[] _history;
        public SlidePerSectionHistory(int sectionsCount)
        {
            _history = new int?[sectionsCount];
        }

        public void SetHistory(int sectionIndex, int slideIndex) => _history[sectionIndex] = slideIndex;

        public int GetHistory(int sectionIndex) => _history[sectionIndex] ?? 0;
    }
}
