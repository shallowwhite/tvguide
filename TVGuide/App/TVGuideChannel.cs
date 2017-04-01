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
        public List<TVGuideChannelProgram> Programs {
            get { return programs; }
        }

        private List<TVGuideChannelProgram> programs = new List<TVGuideChannelProgram>();

        public TVGuideChannel()
        {
        }
    }
}
