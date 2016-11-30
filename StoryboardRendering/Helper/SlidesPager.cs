using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryboardRendering.Models;

namespace StoryboardRendering.Helper
{
    public class SlidesPager
    {
        private Logger _logger = new Logger();

        private readonly SlidePerSectionHistory _history;
        private readonly List<SectionViewModel> _sections;
        public int? PrevSlideIndex { get; private set; }
        public int? NextSlideIdex { get; private set; }
        public int CurrentSlideIndex { get; private set; }
        private int _slidesCount;
        private int _currentSection;

        public SlidesPager(List<SectionViewModel> sections)
        {
            _sections = sections;
            _history = new SlidePerSectionHistory(_sections.Count);
        }

        public void InitWithSelectedSlide(int currentSection, int slideIndex)
        {
            _currentSection = currentSection;
            _slidesCount = _sections[currentSection].Slides.Count;

            SetCurrectSlideAndRecalculate(slideIndex);
        }

        public void Init(int currentSection)
        {
            _currentSection = currentSection;
            _slidesCount = _sections[currentSection].Slides.Count;

            SetCurrectSlideAndRecalculate(_history.GetHistory(_currentSection));
        }

        public void SetPrevSlide()
        {
            SetCurrectSlideAndRecalculate(CurrentSlideIndex - 1);
        }
        public void SetNextSlide()
        {
            SetCurrectSlideAndRecalculate(CurrentSlideIndex + 1);
        }

        public List<SlideViewModel> GetSectionSlides(int sectionIndex)
        {
            int currentSlideIndexInSection = _history.GetHistory(sectionIndex);
            _logger.Log("************FROM HISTORY************* \n" +
                $"Section index : {sectionIndex} \n" +
                $"Slide index : {currentSlideIndexInSection} \n" +
                "************END HISTORY************* \n");
            return new List<SlideViewModel>
            {
                //TODO DO IT BETTER
                GetSlide(sectionIndex,currentSlideIndexInSection - 1),//prev slide
                GetSlide(sectionIndex,currentSlideIndexInSection),//curr slide
                GetSlide(sectionIndex, currentSlideIndexInSection + 1)// next slide 
            };
        }

        public SlideViewModel GetSectionSlide(int sectionIndex)
        {
            int currentSlideIndexInSection = _history.GetHistory(sectionIndex);
            _logger.Log("************FROM HISTORY************* \n" +
                $"Section index : {sectionIndex} \n" +
                $"Slide index : {currentSlideIndexInSection} \n" +
                "************END HISTORY************* \n");
            return GetSlide(sectionIndex, currentSlideIndexInSection);
        }

        private bool SlideIndexInSectionIsValid(int sectionIndex, int index)
        {
            if (index < 0)
                return false;
            if (index > _sections[sectionIndex].Slides.Count - 1)
                return false;
            return true;
        }

        private SlideViewModel GetSlide(int sectionIndex, int slideIndex)
        {
            bool slideIndexIsValid = SlideIndexInSectionIsValid(sectionIndex, slideIndex);
            return slideIndexIsValid ? _sections[sectionIndex].Slides[slideIndex] : ReturnEmptySlide();
        }


        public SlideViewModel GetCurrentSectionPrevSlide()
        {
            return PrevSlideIndex.HasValue ? _sections[_currentSection].Slides[(int)PrevSlideIndex] : null;
        }

        public SlideViewModel GetCurrentSectionCurrentSlide()
        {
            return _sections[_currentSection].Slides[CurrentSlideIndex];
        }

        public SlideViewModel GetCurrentSectionNextSlide()
        {
            return NextSlideIdex.HasValue ? _sections[_currentSection].Slides[(int)NextSlideIdex] : null;
        }

        private SlideViewModel ReturnEmptySlide() => null;

        private void SetCurrectSlideAndRecalculate(int current)
        {
            current = Math.Max(current, 0);
            current = Math.Min(current, _slidesCount - 1);

            var newPrev = current - 1;
            var newCurrent = current;
            var newNext = current + 1;

            PrevSlideIndex = newPrev >= 0 ? newPrev : (int?)null;
            CurrentSlideIndex = newCurrent;
            NextSlideIdex = newNext < _slidesCount ? newNext : (int?)null;
            _logger.Log($" ****************************** \n " +
                        $"Current section index: {_currentSection} " +
                        $"\n Current slide idex: {CurrentSlideIndex} " +
                        $"\n ******************************");
            _history.SetHistory(_currentSection, CurrentSlideIndex);
        }
    }
}
