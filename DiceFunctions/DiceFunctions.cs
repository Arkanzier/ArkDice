namespace ArkDice
{
    //A class used as a way to get some basic functions anywhere I want in the program.
    public static class DiceFunctions
    {
        //Converts a number of rounds into a Human-readable string (ie: 10 -> "1 minute").
        public static string ConvertRoundsToDuration (int duration, bool concentration = false)
        {
            //Check for shortcuts.
            if (duration == 0)
            {
                return "0";
            }

            //First we break this out into the relevant pieces:
            //1 round = 1 round, duh
            int rounds = 0;
            //1 minute = 10 rounds
            int minutes = 0;
            //1 hour = 600 rounds
            int hours = 0;
            //1 day = 14,400 rounds
            int days = 0;
            //more?
            while (duration > 0)
            {
                if (duration >= 14400)
                {
                    duration -= 14400;
                    days++;
                }
                else if (duration >= 600)
                {
                    duration -= 600;
                    hours++;
                }
                else if (duration >= 10)
                {
                    duration -= 10;
                    minutes++;
                }
                else
                {
                    //We can just dump all the rounds into the rounds bucket.
                    rounds += duration;
                    duration = 0;
                }
            }

            //Build the response.
            string ret = "";
            if (days > 0)
            {
                ret = days + " day";

                if (days > 1)
                {
                    ret += "s";
                }
            }
            if (hours > 0)
            {
                if (ret == "")
                {
                    ret = hours + " hour";
                } else
                {
                    ret += ", " + hours + " hour";
                }

                if (hours > 1)
                {
                    ret += "s";
                }
            }
            if (minutes > 0)
            {
                if (ret == "")
                {
                    ret = minutes + " minute";
                } else
                {
                    ret += ", " + minutes + " minute";
                }

                if (minutes > 1)
                {
                    ret += "s";
                }
            }
            if (rounds > 0)
            {
                if (ret == "")
                {
                    ret = rounds + " round";
                } else
                {
                    ret += ", " + rounds + " round";
                }

                if (rounds > 1)
                {
                    ret += "s";
                }
            }

            if (concentration)
            {
                ret = "Concentration up to " + ret;
            }

            return ret;
        }
        public static string ConvertRoundsToDuration (string duration, string concentration = "")
        {
            int d;
            if (!Int32.TryParse(duration, out d))
            {
                //Complain to a log file?
                return ConvertRoundsToDuration(0, false);
            }

            bool c = (concentration == "X" || concentration == "x") ? true : false;

            return ConvertRoundsToDuration(d, c);
        }

        //Takes in a stat (the number) and spits out it's modifier.
        public static int GetModifierForStat (int stat)
        {
            decimal temp = stat - 10;
            int ret = Decimal.ToInt32(Math.Floor(temp / 2));
            return ret;
        }

        //Takes in a level and spits out the expected proficiency bonus for it.
        public static int GetProfForLevel(int level)
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
            string temp = GetStatForSkill (name);
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
        public static string GetStatForSkill(string skill)
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