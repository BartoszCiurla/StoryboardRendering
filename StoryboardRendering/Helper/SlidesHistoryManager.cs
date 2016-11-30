namespace StoryboardRendering.Helper
{
    public class SlidesHistoryManager
    {
        private readonly int?[] _history;
        public SlidesHistoryManager(int sectionsCount)
        {
            _history = new int?[sectionsCount];
        }

        public void SetHistory(int sectionIndex, int slideIndex) => _history[sectionIndex] = slideIndex;

        public int GetHistory(int sectionIndex) => _history[sectionIndex] ?? 0;
    }
}
