namespace ArkDice
{
    //A class used as a way to get some basic functions anywhere I want in the program.
    public static class DiceFunctions
    {
        //Takes in a stat (the number) and spits out it's modifier.
        public static int getModifierForStat (int stat)
        {
            decimal temp = stat - 10;
            int ret = Decimal.ToInt32(Math.Floor(temp / 2));
            return ret;
        }

        //Takes in a level and spits out the expected proficiency bonus for it.
        public static int getProfForLevel(int level)
        {
            switch (level)
            {
                case <= 0: return 0;
                case < 5: return 2;
                case < 9: return 3;
                case < 13: return 4;
                case < 17: return 5;
                case >= 17: return 6;

            }
        }

        //Takes in the name of a skill and spits out the name of the stat typically associated to it.
        //to do: load this from a file
        //to do: create a version that returns an int?
        public static string getStatForSkill(string skill)
        {
            string moddedSkill = skill.ToLower();
            switch (moddedSkill)
            {
                case "athletics":
                    return "strength";
                case "acrobatics":
                case "sleight of hand":
                case "stealth":
                    return "dexterity";
                case "arcana":
                case "history":
                case "investigation":
                case "nature":
                case "religion;":
                    return "intelligence";
                case "animal handling":
                case "insight":
                case "medicine":
                case "perception":
                case "survival":
                    return "wisdom";
                case "deception":
                case "intimidation":
                case "performance":
                case "persuasion":
                    return "charisma";
                default:
                    return "";
            }
        }
    }
}