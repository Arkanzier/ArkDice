using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character
{
    //A list of ClassLevel objects.
    //This exists mostly to allow automatic deserialization of lists of ClassLevels, and partially for a couple convenience functions.
    #region ClassLevelList Class

    public class ClassLevelList
    {
        public List<ClassLevel> Levels { get; set; }

        public ClassLevelList()
        {
            Levels = new List<ClassLevel>();
        }

        public ClassLevelList(List<ClassLevel> levels)
        {
            Levels = levels;
        }

        public void Sort ()
        {
            Levels.Sort();
        }
    }

    #endregion

    //A simple little struct so we can easily associate class + subclass + level as a single data point.
    //It also handles HD while we're at it because that's convenient.
    #region ClassLevel Class

    public class ClassLevel
    {
        public string Name { get; set; }
        public string Subclass { get; set; }
        public int Level { get; set; }

        public int HDSize { get; set; }
        public int CurrentHD { get; set; }

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

        //Used for sorting.
        public int Compare (ClassLevel other)
        {
            //First we compare based on class name.
            int comparison = String.Compare(this.Name, other.Name, StringComparison.OrdinalIgnoreCase);
            if (comparison < 0)
            {
                return -1;
            } else if (comparison > 0)
            {
                return 1;
            }

            //Now we need to break ties by subclass name.
            comparison = String.Compare(this.Subclass, other.Subclass, StringComparison.OrdinalIgnoreCase);
            if (comparison < 0)
            {
                return -1;
            }
            else if (comparison > 0)
            {
                return 1;
            }

            //I guess we'll compare levels, in case that matters.
            if (this.Level < other.Level)
            {
                return -1;
            }
            else if (this.Level > other.Level)
            {
                return 1;
            }

            //They're identical in all ways we could reasonably justify sorting by.
            return 0;
        }

        //Updates this object to match whatever is passed in.
        //Leaves CurrentHD alone, except that it enforces a maximum for it of Level.
        public void IncorporateChanges (ClassLevel newInfo)
        {
            this.Name = newInfo.Name;
            this.Subclass = newInfo.Subclass;
            this.Level = newInfo.Level;
            this.HDSize = newInfo.HDSize;
            this.CurrentHD = newInfo.CurrentHD;

            if (CurrentHD > Level)
            {
                CurrentHD = Level;
            }
        }
    }

    #endregion
}
