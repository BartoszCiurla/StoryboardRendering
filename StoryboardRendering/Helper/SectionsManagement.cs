using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using StoryboardRendering.Models;

namespace StoryboardRendering.Helper
{
    public class SectionsManagement
    {
        private Logger _logger = new Logger();
        private readonly SectionsPager _sectionsPager;
        private ScrollViewerManagement _scrollViewerManagement;
        private SectionsBuilder _sectionsBuilder;

        public SectionsManagement(List<SectionViewModel> sections, string slideId = null)
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
            _scrollViewerManagement = new ScrollViewerManagement(scrollViewers);
            InitSections();
        }

        private void InitSections()
        {
            _scrollViewerManagement.UpdateVerticalLayout();
            bool prevSectionWasBuilt = _sectionsBuilder
                .BuildPrevSection(_sectionsPager.GetPrevSectionSlide());

            bool nextSectionWasBuilt = _sectionsBuilder
                .BuildNextSection(_sectionsPager.GetNextSectionSlide());

            bool prevSlideWasBuild =
                _sectionsBuilder
                .BuildCurrentSectionPrevSlide(_sectionsPager.GetCurrentSectionPrevSlide());

            bool curSlideWasBuild =
                _sectionsBuilder
                .BuildCurrentSectionCurrentSlide(_sectionsPager.GetCurrentSectionCurrentSlide());

            bool nextSlideWasBuild =
                _sectionsBuilder
                .BuildCurrentSectionNextSlide(_sectionsPager.GetCurrentSectionNextSlide());


            if (prevSlideWasBuild)
                IgnoreNextVerticalScrolling = _scrollViewerManagement.ScrollVerticallyToPage(1);
        }

        public void SetPrevSlide()
        {
            _logger.Log("Try set prev slide");
            _sectionsPager.SetPrevSlide();
            bool sectionisRebuild = _sectionsBuilder.RebuildCurrentSectionAfterSetPrevSlide(_sectionsPager.GetCurrentSectionPrevSlide());

            if (sectionisRebuild)
            {
                IgnoreNextVerticalScrolling = _scrollViewerManagement.ScrollVerticallyToPage(1);
            }
        }

        public void SetNextSlide()
        {
            _logger.Log("Try set next slide");
            _sectionsPager.SetNextSlide();
      
            _sectionsBuilder.RebuildCurrentSectionAfterSetNextSlide(_sectionsPager.GetCurrentSectionNextSlide());

            IgnoreNextVerticalScrolling = _scrollViewerManagement.ScrollVerticallyToPage(1);
        }

        public void SetPrevSection()
        {

        }

        public void SetNextSection()
        {

        }
    }
}
