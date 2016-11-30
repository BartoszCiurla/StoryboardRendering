﻿using System.Collections.Generic;
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
            => SetVisibilityToSectionPlaceHolder(PrevSection, ItWasBuilt(slide, PrevSectionCurSlide));

        public bool BuildNextSection(SlideViewModel slide)
            => SetVisibilityToSectionPlaceHolder(NextSection, ItWasBuilt(slide, NextSectionCurrSlide));

        public bool BuildCurrentSectionPrevSlide(SlideViewModel slide)
            => SetVisibilityToSlidePlaceHolder(CurSectionPrevSlide, ItWasBuilt(slide, CurSectionPrevSlide));

        public bool BuildCurrentSectionCurrentSlide(SlideViewModel slide)
            => SetVisibilityToSlidePlaceHolder(CurSectionCurSlide, ItWasBuilt(slide, CurSectionCurSlide));

        public bool BuildCurrentSectionNextSlide(SlideViewModel slide)
            => SetVisibilityToSlidePlaceHolder(CurSectionNextSlide, ItWasBuilt(slide, CurSectionNextSlide));

        public bool RebuildCurrentSectionAfterSetPrevSlide(SlideViewModel prevSlide)
        {
            // move curent -> next
            MoveContent(_slidesPlaceHoldersContainer[CurSectionCurSlide], _slidesPlaceHoldersContainer[CurSectionNextSlide]);

            //move prev -> current 
            MoveContent(_slidesPlaceHoldersContainer[CurSectionPrevSlide], _slidesPlaceHoldersContainer[CurSectionCurSlide]);

            //build prev
            return BuildCurrentSectionPrevSlide(prevSlide);
        }

        public bool RebuildCurrentSectionAfterSetNextSlide(SlideViewModel nextSlide)
        {
            //move current -> prev
            MoveContent(_slidesPlaceHoldersContainer[CurSectionCurSlide], _slidesPlaceHoldersContainer[CurSectionPrevSlide]);

            //move next -> current
            MoveContent(_slidesPlaceHoldersContainer[CurSectionNextSlide], _slidesPlaceHoldersContainer[CurSectionCurSlide]);

            //build next
            return BuildCurrentSectionNextSlide(nextSlide);
        }

        public RebuildResult RebuildSectionsAfterSetPrevSection(SlideViewModel currentSectionPrevSlide,
        SlideViewModel currentSectionNextSlide,
        SlideViewModel prevSectionSlide)
        {
            //current -> next section 
            bool contentIsFull = MoveContent(_slidesPlaceHoldersContainer[CurSectionCurSlide],
                _slidesPlaceHoldersContainer[NextSectionCurrSlide]);

            SetVisibilityToSectionPlaceHolder(NextSection, contentIsFull);

            // prev section -> current
            MoveContent(_slidesPlaceHoldersContainer[PrevSectionCurSlide], _slidesPlaceHoldersContainer[CurSectionCurSlide]);

            //build prev slide
            bool prevSlideExist = BuildCurrentSectionPrevSlide(currentSectionPrevSlide);

            //build next slide
            BuildCurrentSectionNextSlide(currentSectionNextSlide);

            // build prev section 
            bool prevSectionExist = BuildPrevSection(prevSectionSlide);
            return new RebuildResult { PrevSectionWasBuild = prevSectionExist, PrevSlideWasBuild = prevSlideExist };
        }

        public RebuildResult RebuildSectionsAfterSetNextSection(SlideViewModel currentSectionPrevSlide, 
            SlideViewModel currentSectionNextSlide, 
            SlideViewModel nextSectionSlide)
        {
            //current -> prev section
            bool contentIsFull = MoveContent(_slidesPlaceHoldersContainer[CurSectionCurSlide],
                _slidesPlaceHoldersContainer[PrevSectionCurSlide]);

            SetVisibilityToSectionPlaceHolder(PrevSection, contentIsFull);

            //next section -> current
            MoveContent(_slidesPlaceHoldersContainer[NextSectionCurrSlide],
                _slidesPlaceHoldersContainer[CurSectionCurSlide]);

            //build prev slide
            bool prevSlideExist = BuildCurrentSectionPrevSlide(currentSectionPrevSlide);

            //build next slide
            BuildCurrentSectionNextSlide(currentSectionNextSlide);

            //build next section
            bool nextSectionExist = BuildNextSection(nextSectionSlide);
            return new RebuildResult {PrevSectionWasBuild = contentIsFull,PrevSlideWasBuild = prevSlideExist};
        }

        private bool MoveContent(Border from, Border to)
        {
            bool contentIsFull = false;
            var content = from.Child as SlideViewer;
            from.Child = null;

            if (content != null)
                contentIsFull = true;

            to.Child = null;
            to.Visibility = contentIsFull ? Visibility.Visible : Visibility.Collapsed;
            to.Child = content;

            return contentIsFull;
        }

        

        private bool ItWasBuilt(SlideViewModel slide, int placeHolderIndex)
        {
            if (slide == null)
            {
                return false;
            }
            BuildSection(slide, placeHolderIndex);
            return true;
        }

        private void BuildSection(SlideViewModel slide, int placeHolderIndex)
        {
            var slidePlaceHolder = _slidesPlaceHoldersContainer[placeHolderIndex];
            if (slidePlaceHolder != null)
                slidePlaceHolder.Child = new SlideViewer { SlideModel = slide };
        }

        private bool SetVisibilityToSectionPlaceHolder(int placeHolderIndex, bool itWasBuilt)
        {
            var sectionPlaceHolder = _sectionsPlaceHoldersContainer[placeHolderIndex];
            if (sectionPlaceHolder == null)
                return false;

            sectionPlaceHolder.Visibility = itWasBuilt
                ? Visibility.Visible
                : Visibility.Collapsed;
            return itWasBuilt;
        }

        private bool SetVisibilityToSlidePlaceHolder(int placeHolderIndex, bool itWasBuilt)
        {
            var slidePlaceHolder = _slidesPlaceHoldersContainer[placeHolderIndex];
            if (slidePlaceHolder == null)
                return false;

            slidePlaceHolder.Visibility = itWasBuilt
                ? Visibility.Visible
                : Visibility.Collapsed;
            return itWasBuilt;
        }
    }

    public class RebuildResult
    {
        public bool PrevSlideWasBuild { get; set; }
        public bool PrevSectionWasBuild { get; set; }
    }
}
