using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryboardRendering.Models;

namespace StoryboardRendering.PageViewModels
{
    public class MainPageViewModel
    {
        private readonly Storyboard storyboard;
        public MainPageViewModel()
        {
            storyboard = Storyboard.Create(10);
        }

        public List<SectionViewModel> SectionList => storyboard.Sections;
    }
}
