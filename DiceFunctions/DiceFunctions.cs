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

        //Takes in a string referring to a type of roll and returns the stat used for it.
        public static string GetStatForRoll (string name)
        {
            name = name.ToLower();

            //If this is a skill, we can take the easy way out.
            string temp = getStatForSkill (name);
            if (temp != "")
            {
                //This is a skill, we're done.
                return temp;
            }

            //We're dealing with something else.
            switch (name)
            {
                case "str":
                case "strength":
                case "strsave":
                    return "strength";
                case "dex":
                case "dexterity":
                case "dexsave":
                    return "dexterity";
                case "con":
                case "constitution":
                case "consave":
                case "concentration":
                    return "constitution";
                case "int":
                case "intelligence":
                case "intsave":
                    return "intelligence";
                case "wis":
                case "wisdom":
                case "wissave":
                    return "wisdom";
                case "cha":
                case "charisma":
                case "chasave":
                    return "charisma";
                default:
                    return "";
            }
        }

        //Takes in the name of a skill and spits out the name of the stat typically associated to it.
        //to do: load this from a file
        //to do: create a version that returns an int?
        public static string getStatForSkill(string skill)
        {
            skill = skill.ToLower();
            switch (skill)
            {
                case "athletics":
                    return "strength";
                case "acrobatics":
                case "sleight of hand":
                case "sleightofhand":
                case "stealth":
                    return "dexterity";
                case "arcana":
                case "history":
                case "investigation":
                case "nature":
                case "religion":
                    return "intelligence";
                case "animal handling":
                case "animalhandling":
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