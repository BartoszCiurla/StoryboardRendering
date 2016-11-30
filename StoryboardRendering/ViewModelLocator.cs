using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryboardRendering.PageViewModels;

namespace StoryboardRendering
{
    public class ViewModelLocator
    {
        public MainPageViewModel MainPageViewModel => new MainPageViewModel();
    }
}
