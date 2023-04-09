using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character
{
    //A simple little struct so we can easily associate class + subclass + level as a single data point.

    public struct ClassLevel
    {
        public string name { get; set; }
        public string subclass { get; set; }
        public int level { get; set; }

        public int HDSize { get; set; }
        public int currentHD { get; set; }

        //hd size?
        //current number of hd?

        public ClassLevel (string name = "", string subclass = "", int level = 0, int HDSize = -1, int currentHD = -1)
        {
            this.name = name;
            this.subclass = subclass;
            this.level = level;

            if (HDSize > 0)
            {
                this.HDSize = HDSize;
            }
            else
            {
                //look it up by class name
                //to do
                this.HDSize = 0;
            }

            if (currentHD > 0)
            {
                this.currentHD = currentHD;
            } else
            {
                //Default to full HD.
                this.currentHD = level;
            }
        }
    }
}
