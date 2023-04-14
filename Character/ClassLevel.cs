using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character
{
    //A simple little struct so we can easily associate class + subclass + level as a single data point.

    public class ClassLevel
    {
        public string Name { get; set; }
        public string Subclass { get; set; }
        public int Level { get; set; }

        public int HDSize { get; set; }
        public int CurrentHD { get; set; }

        //hd size?
        //current number of hd?

        public ClassLevel (string name = "", string subclass = "", int level = 0, int HDSize = -1, int currentHD = -1)
        {
            this.Name = name;
            this.Subclass = subclass;
            this.Level = level;

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
                this.CurrentHD = currentHD;
            } else
            {
                //Default to full HD.
                this.CurrentHD = level;
            }
        }
    }
}
