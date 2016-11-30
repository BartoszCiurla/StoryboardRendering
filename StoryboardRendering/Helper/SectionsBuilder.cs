using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using StoryboardRendering.Controls;
using StoryboardRendering.Models;

namespace StoryboardRendering.Helper
{
    public class SectionsBuilder
    {
        private readonly List<Border> _slidesPlaceHoldersContainer;
        private const int PrevSectionCurSlide = 0;
        private const int CurSectionPrevSlide = 1;
        private const int CurSectionCurSlide = 2;
        private const int CurSectionNextSlide = 3;
        private const int NextSectionCurrSlide = 4;

        private readonly List<Border> _sectionsPlaceHoldersContainer;
        private const int PrevSection = 0;
        private const int NextSection = 2;

        public SectionsBuilder(List<Border> sectionsPlaceHoldersContainer, List<Border> slidesPlaceHoldersContainer)
        {
            _sectionsPlaceHoldersContainer = sectionsPlaceHoldersContainer;
            _slidesPlaceHoldersContainer = slidesPlaceHoldersContainer;
        }

        public bool BuildPrevSection(SlideViewModel slide)
            => SetVisibilityToSectionPlaceHolder(PrevSection, AddSlideToPlaceHolder(slide, PrevSectionCurSlide));

        public bool BuildNextSection(SlideViewModel slide)
            => SetVisibilityToSectionPlaceHolder(NextSection, AddSlideToPlaceHolder(slide, NextSectionCurrSlide));

        public bool BuildCurrentSectionPrevSlide(SlideViewModel slide)
            => SetVisibilityToSlidePlaceHolder(CurSectionPrevSlide, AddSlideToPlaceHolder(slide, CurSectionPrevSlide));

        public bool BuildCurrentSectionCurrentSlide(SlideViewModel slide)
            => SetVisibilityToSlidePlaceHolder(CurSectionCurSlide, AddSlideToPlaceHolder(slide, CurSectionCurSlide));

        public bool BuildCurrentSectionNextSlide(SlideViewModel slide)
            => SetVisibilityToSlidePlaceHolder(CurSectionNextSlide, AddSlideToPlaceHolder(slide, CurSectionNextSlide));

        public bool RebuildCurrentSectionAfterSetPrevSlide(SlideViewModel prevSlide)
        {
            MoveCurrentSlideContentToNextSlide();

            MovePrevSlideContentToCurrentSlide();

            return BuildCurrentSectionPrevSlide(prevSlide);
        }

        private bool MoveCurrentSlideContentToNextSlide() =>
            MoveContent(_slidesPlaceHoldersContainer[CurSectionCurSlide], _slidesPlaceHoldersContainer[CurSectionNextSlide]);

        private bool MovePrevSlideContentToCurrentSlide()
            => MoveContent(_slidesPlaceHoldersContainer[CurSectionPrevSlide], _slidesPlaceHoldersContainer[CurSectionCurSlide]);

        public bool RebuildCurrentSectionAfterSetNextSlide(SlideViewModel nextSlide)
        {
            MoveCurrentSlideContentToPrevSlide();

            MoveNextSlideContentToCurrentSlide();

            return BuildCurrentSectionNextSlide(nextSlide);
        }

        private bool MoveCurrentSlideContentToPrevSlide()
            => MoveContent(_slidesPlaceHoldersContainer[CurSectionCurSlide], _slidesPlaceHoldersContainer[CurSectionPrevSlide]);

        private bool MoveNextSlideContentToCurrentSlide()
            => MoveContent(_slidesPlaceHoldersContainer[CurSectionNextSlide], _slidesPlaceHoldersContainer[CurSectionCurSlide]);

        public RebuildSectionResult RebuildSectionsAfterSetPrevSection(SlideViewModel currentSectionPrevSlide,
            SlideViewModel currentSectionNextSlide,
            SlideViewModel prevSectionSlide)
        {
            bool contentIsBuilt = MoveCurrentSlideContentToNextSection();

            SetVisibilityToSectionPlaceHolder(NextSection, contentIsBuilt);

            MovePrevSectionContentToCurrentSlide();

            bool prevSlideIsBuilt = BuildCurrentSectionPrevSlide(currentSectionPrevSlide);

            BuildCurrentSectionNextSlide(currentSectionNextSlide);

            bool prevSectionWasBuild = BuildPrevSection(prevSectionSlide);

            return new RebuildSectionResult(prevSlideIsBuilt, prevSectionWasBuild);
        }

        private bool MoveCurrentSlideContentToNextSection()
            => MoveContent(_slidesPlaceHoldersContainer[CurSectionCurSlide], _slidesPlaceHoldersContainer[NextSectionCurrSlide]);

        private bool MovePrevSectionContentToCurrentSlide()
            => MoveContent(_slidesPlaceHoldersContainer[PrevSectionCurSlide], _slidesPlaceHoldersContainer[CurSectionCurSlide]);

        public RebuildSectionResult RebuildSectionsAfterSetNextSection(SlideViewModel currentSectionPrevSlide,
            SlideViewModel currentSectionNextSlide,
            SlideViewModel nextSectionSlide)
        {
            bool prevSectionIsBuilt = MoveCurrentSlideContentToPrevSection();

            SetVisibilityToSectionPlaceHolder(PrevSection, prevSectionIsBuilt);

            MoveNextSectionSlideToCurrentSlide();

            bool prevSlideIsBuilt = BuildCurrentSectionPrevSlide(currentSectionPrevSlide);

            BuildCurrentSectionNextSlide(currentSectionNextSlide);

            BuildNextSection(nextSectionSlide);

            return new RebuildSectionResult(prevSlideIsBuilt, prevSectionIsBuilt);     
        }

        private bool MoveCurrentSlideContentToPrevSection()
            => MoveContent(_slidesPlaceHoldersContainer[CurSectionCurSlide], _slidesPlaceHoldersContainer[PrevSectionCurSlide]);

        private bool MoveNextSectionSlideToCurrentSlide()
            => MoveContent(_slidesPlaceHoldersContainer[NextSectionCurrSlide], _slidesPlaceHoldersContainer[CurSectionCurSlide]);

        private bool MoveContent(Border source, Border target)
        {
            bool contentIsFull = false;
            var content = source.Child as SlideViewer;
            source.Child = null;

            if (content != null)
                contentIsFull = true;

            target.Child = null;
            target.Visibility = contentIsFull ? Visibility.Visible : Visibility.Collapsed;
            target.Child = content;

            return contentIsFull;
        }   

        private bool AddSlideToPlaceHolder(SlideViewModel slide, int placeHolderIndex)
        {
            if (slide == null)
                return false;

            var slidePlaceHolder = _slidesPlaceHoldersContainer[placeHolderIndex];
            if (slidePlaceHolder != null)
                slidePlaceHolder.Child = new SlideViewer { SlideModel = slide };

            return true;
        }

        private bool SetVisibilityToSectionPlaceHolder(int placeHolderIndex, bool itIsBuilt)
        {
            var sectionPlaceHolder = _sectionsPlaceHoldersContainer[placeHolderIndex];

            return SetVisibility(sectionPlaceHolder, itIsBuilt);
        }

        private bool SetVisibilityToSlidePlaceHolder(int placeHolderIndex, bool itIsBuilt)
        {
            var slidePlaceHolder = _slidesPlaceHoldersContainer[placeHolderIndex];

            return SetVisibility(slidePlaceHolder, itIsBuilt);
        }

        private bool SetVisibility(Border placeHolder, bool itIsBuilt)
        {
            if (placeHolder == null)
                return false;
            placeHolder.Visibility = itIsBuilt
              ? Visibility.Visible
              : Visibility.Collapsed;

            return itIsBuilt;
        }
    }

   
}
