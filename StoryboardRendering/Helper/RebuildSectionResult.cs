namespace StoryboardRendering.Helper
{
    public class RebuildSectionResult
    {
        public RebuildSectionResult(bool prevSlideIsBuilt, bool prevSectionIsBuilt)
        {
            PrevSlideIsBuilt = prevSlideIsBuilt;
            PrevSectionIsBuilt = prevSectionIsBuilt;
        }

        public bool PrevSlideIsBuilt { get; private set; }
        public bool PrevSectionIsBuilt { get; private set; }
    }
}
