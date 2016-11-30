using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryboardRendering.Models;

namespace StoryboardRendering.Helper
{
    public class SectionsPager
    {
        private Logger _logger = new Logger();

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
            _logger.Log($"PREV SECTION: {_prevSection}");

            SetCurrentSectionAndRecalculate(_currentSection - 1);
            _slidesPager.Init(_currentSection);

            _logger.Log($"CURRENT SECTION AFTER CHANGE: {_currentSection}");
        }

        public void SetNextSection()
        {
            _logger.Log($"NEXT SECTOIN: {_nextSection}");

            SetCurrentSectionAndRecalculate(_currentSection + 1);
            _slidesPager.Init(_currentSection);

            _logger.Log($"CURRENT SECTION AFTER CHANGE: {_currentSection}");
        }

        public void SetPrevSlide()
        {
            _slidesPager.SetPrevSlide();
        }

        public void SetNextSlide()
        {
            _slidesPager.SetNextSlide();
        }

        public List<SlideViewModel> GetPrevSectionSlides()
        {
            return _prevSection.HasValue ? _slidesPager.GetSectionSlides((int)_prevSection) : GetEmptySection();
        }

        public List<SlideViewModel> GetCurrectSectionSlides()
        {
            return _slidesPager.GetSectionSlides(_currentSection);
        }

        public List<SlideViewModel> GetNextSectionSlides()
        {
            return _nextSection.HasValue ? _slidesPager.GetSectionSlides((int)_nextSection) : GetEmptySection();
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

        public int GetCurrentSlideIndex() => _slidesPager.CurrentSlideIndex;

        private List<SlideViewModel> GetEmptySection() => new List<SlideViewModel> { null, null, null };

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
