using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVGuide.App
{
    class TVGuideChannel
    {
        public string Name { get; set; }
        
        private List<TVGuideChannelProgram> programs = new List<TVGuideChannelProgram>();
        public List<TVGuideChannelProgram> Programs {
            get { return programs; }
        }

        public TVGuideChannel()
        {
        }
    }
}
