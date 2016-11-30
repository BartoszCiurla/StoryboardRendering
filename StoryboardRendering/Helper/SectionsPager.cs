using System;
using System.Collections.Generic;
using System.Linq;
using StoryboardRendering.Models;

namespace StoryboardRendering.Helper
{
    public class SectionsPager
    {        
        private readonly SlidesPager _slidesPager;
        private readonly List<SectionViewModel> _sections;
        private int? _prevSection;
        private int _currentSection;
        private int? _nextSection;
        private readonly int _sectionCount;

        public SectionsPager(List<SectionViewModel> sections, string slideId)
        {
            _slidesPager = new SlidesPager(sections);
            _sections = sections;
            _sectionCount = _sections.Count;
            SetCurrentSectionAndRecalculate(GetSectionIndexBySlideId(slideId));
            _slidesPager.InitWithSelectedSlide(_currentSection, GetSlideIndexBySlideId(slideId));
        }

        private int GetSectionIndexBySlideId(string slideId)
            => string.IsNullOrWhiteSpace(slideId) ? 0 : _sections.FindIndex(0, section => section.Slides.Any(slide => slide.Id == slideId));

        private int GetSlideIndexBySlideId(string slideId)
            => string.IsNullOrWhiteSpace(slideId) ? 0 : _sections[_currentSection].Slides.FindIndex(0, x => x.Id == slideId);

        public void SetPrevSection()
        {
            SetCurrentSectionAndRecalculate(_currentSection - 1);
            _slidesPager.Init(_currentSection);            
        }

        public void SetNextSection()
        {
            SetCurrentSectionAndRecalculate(_currentSection + 1);
            _slidesPager.Init(_currentSection);
        }

        public void SetPrevSlide()
        {
            _slidesPager.SetPrevSlide();
        }

        public void SetNextSlide()
        {
            _slidesPager.SetNextSlide();
        }

        public SlideViewModel GetPrevSectionSlide()
        {
            return _prevSection.HasValue ? _slidesPager.GetSectionSlide((int)_prevSection) : null;
        }

        public SlideViewModel GetNextSectionSlide()
        {
            return _nextSection.HasValue ? _slidesPager.GetSectionSlide((int)_nextSection) : null;
        }

        public SlideViewModel GetCurrentSectionPrevSlide() => _slidesPager.GetCurrentSectionPrevSlide();

        public SlideViewModel GetCurrentSectionCurrentSlide() => _slidesPager.GetCurrentSectionCurrentSlide();


        public SlideViewModel GetCurrentSectionNextSlide() => _slidesPager.GetCurrentSectionNextSlide();

        private void SetCurrentSectionAndRecalculate(int current)
        {
            current = Math.Max(current, 0);
            current = Math.Min(current, _sectionCount - 1);

            var newPrev = current - 1;
            var newCurrent = current;
            var newNext = current + 1;

            _prevSection = newPrev >= 0 ? newPrev : (int?)null;
            _currentSection = newCurrent;
            _nextSection = newNext < _sectionCount ? newNext : (int?)null;
        }
    }
}
