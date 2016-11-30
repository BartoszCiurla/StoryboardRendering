using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryboardRendering.Models
{
    public class Storyboard
    {
        public List<SectionViewModel> Sections { get; private set; }

        public static Storyboard Create(int numberOfSections)
        {
            var storyboard = new Storyboard { Sections = new List<SectionViewModel>() };

            for (int i = 0; i < numberOfSections; ++i)
                storyboard.Sections.Add(SectionViewModel.Create($"Section {i}",10));
            return storyboard;
        }

        public List<SectionViewModel> GetSection()
        {
            return Sections;
        }
    }

    public class SectionViewModel
    {
        public string Name { get; private set; }
        public List<SlideViewModel> Slides { get; private set; }

        public static SectionViewModel Create(string name, int numberOfSlides)
        {
            var section = new SectionViewModel();
            section.Name = name;
            section.Slides = new List<SlideViewModel>();

            for (int i = 0; i < numberOfSlides; ++i)
                section.Slides.Add(SlideViewModel.Create($"{section.Name}, slide {i}"));

            return section;
        }

        public SlideViewModel GetSlide(int index)
        {
            if (index < 0 || index >= Slides.Count)
                return null;

            return Slides[index];
        }
    }

    public class SlideViewModel
    {
        public string Id { get; private set; }

        public static SlideViewModel Create(string id)
        {
            var slide = new SlideViewModel { Id = id };
            return slide;
        }
    }
}
