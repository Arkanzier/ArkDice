using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character
{
    internal class CharacterCopy
    {
        //This class exists solely to deserialize characters into.
        //Deserializing seems to require public setters, which I'm unwilling to give the Character class.

        public string ID { get; set; }
        public string Name { get; set; }
        public string Race { get; set; }
        public string Subrace { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int TempHP { get; set; }
        public List<ClassLevel> Classes { get; private set; }
        public int BonusToProf { get; private set; }
        public int Prof { get; private set; }
        public int[] Stats { get; private set; }
        public int[] Saves { get; private set; }
        public Dictionary<string, int> Skills { get; private set; }
        public List<Ability> Abilities { get; private set; }
        public List<Ability> BasicAbilities { get; private set; }
        public List<string> Passives { get; private set; }

        public CharacterCopy()
        {
            ID = "";
            Name = "";
            Race = "";
            Subrace = "";
            MaxHP = 0;
            CurrentHP = 0;
            TempHP = 0;
            Classes = new List<ClassLevel>();
            BonusToProf = 0;
            Prof = 0;
            Stats = new int[6];
            Saves = new int[6];
            //for (int a = 0; a < 6; a++)
            //{
            //    Stats[a] = 0;
            //    Saves[a] = 0;
            //}
            Skills = new Dictionary<string, int>();
            Abilities = new List<Ability>();
            BasicAbilities = new List<Ability>();
            Passives = new List<string>();
        }
    }

}
