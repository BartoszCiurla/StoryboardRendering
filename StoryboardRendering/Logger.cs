using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryboardRendering
{
    public class Logger
    {
        public void Log(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
