using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using StoryboardRendering.Models;

namespace StoryboardRendering.Helper
{
    public class SectionsManager
    {
        private readonly SectionsPager _sectionsPager;
        private ScrollViewerManager _scrollViewerManagement;
        private SectionsBuilder _sectionsBuilder;

        public SectionsManager(List<SectionViewModel> sections, string slideId = null)
        {
            _sectionsPager = new SectionsPager(sections, slideId);
        }

        public bool IgnoreNextVerticalScrolling { get; set; }

        public bool IgnoreNextHorizontalScrolling { get; set; }

        public void Init(List<Border> sectionsPlaceHoldersContainer,
            List<Border> slidesPlaceHoldersContainer,
            List<ScrollViewer> scrollViewers)
        {
            _sectionsBuilder = new SectionsBuilder(sectionsPlaceHoldersContainer, slidesPlaceHoldersContainer);
            _scrollViewerManagement = new ScrollViewerManager(scrollViewers);
            InitSections();
        }

        private void InitSections()
        {
            _scrollViewerManagement.UpdateVerticalLayout();

            bool prevSectionIsBuilt = _sectionsBuilder
                .BuildPrevSection(_sectionsPager.GetPrevSectionSlide());

            _sectionsBuilder
               .BuildNextSection(_sectionsPager.GetNextSectionSlide());

            bool prevSlideIsBuilt =
                _sectionsBuilder
                .BuildCurrentSectionPrevSlide(_sectionsPager.GetCurrentSectionPrevSlide());

            _sectionsBuilder
            .BuildCurrentSectionCurrentSlide(_sectionsPager.GetCurrentSectionCurrentSlide());

            _sectionsBuilder
            .BuildCurrentSectionNextSlide(_sectionsPager.GetCurrentSectionNextSlide());


            if (prevSlideIsBuilt)
                IgnoreNextVerticalScrolling = _scrollViewerManagement.ScrollVerticallyToPage(1);

            if (prevSectionIsBuilt)
                IgnoreNextHorizontalScrolling = _scrollViewerManagement.ScrollHorizontallyToPage(1);
        }

        public void SetPrevSlide()
        {
            _sectionsPager.SetPrevSlide();
            bool prevSlideIsBuilt = _sectionsBuilder.RebuildCurrentSectionAfterSetPrevSlide(_sectionsPager.GetCurrentSectionPrevSlide());

            if (prevSlideIsBuilt)
            {
                IgnoreNextVerticalScrolling = _scrollViewerManagement.ScrollVerticallyToPage(1);
            }
        }

        public void SetNextSlide()
        {
            _sectionsPager.SetNextSlide();

            _sectionsBuilder.RebuildCurrentSectionAfterSetNextSlide(_sectionsPager.GetCurrentSectionNextSlide());

            IgnoreNextVerticalScrolling = _scrollViewerManagement.ScrollVerticallyToPage(1);
        }

        public void SetPrevSection()
        {
            _sectionsPager.SetPrevSection();

            RebuildSectionResult rebuildSectionResult = _sectionsBuilder.RebuildSectionsAfterSetPrevSection(
                _sectionsPager.GetCurrentSectionPrevSlide(),
                _sectionsPager.GetCurrentSectionNextSlide(), 
                _sectionsPager.GetPrevSectionSlide());

            _scrollViewerManagement.UpdateHorizontallLayout();
            

            if (rebuildSectionResult.PrevSectionIsBuilt)
                IgnoreNextHorizontalScrolling = _scrollViewerManagement.ScrollHorizontallyToPage(1);

            _scrollViewerManagement.UpdateVerticalLayout();

            if (rebuildSectionResult.PrevSlideIsBuilt)
                IgnoreNextVerticalScrolling = _scrollViewerManagement.ScrollVerticallyToPage(1);
            else
            {
                IgnoreNextVerticalScrolling = _scrollViewerManagement.ScrollVerticallyToPage(0);
            }
        }

        public void SetNextSection()
        {
            _sectionsPager.SetNextSection();

            RebuildSectionResult rebuildSectionResult = _sectionsBuilder.RebuildSectionsAfterSetNextSection(
                _sectionsPager.GetCurrentSectionPrevSlide(),
                _sectionsPager.GetCurrentSectionNextSlide(),
                _sectionsPager.GetNextSectionSlide());


            if (rebuildSectionResult.PrevSectionIsBuilt)
                IgnoreNextHorizontalScrolling = _scrollViewerManagement.ScrollHorizontallyToPage(1);

            _scrollViewerManagement.UpdateVerticalLayout();

            if (rebuildSectionResult.PrevSlideIsBuilt)
                IgnoreNextVerticalScrolling = _scrollViewerManagement.ScrollVerticallyToPage(1);
            else
            {
                IgnoreNextVerticalScrolling = _scrollViewerManagement.ScrollVerticallyToPage(0);
            }
        }
    }
}
