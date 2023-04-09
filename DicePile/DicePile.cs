using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace ArkDice
{
    public class DicePile
        //to do: consider changing this to a struct, since it's unlikely to change after the initial setup.
    {
        private int dieSize;
        private int numDice;
        private int numAdv;
        private int numDis;
        private int flatBonus;
        private string dynamicBonus;
        private int multiplier;
        //change to bool positive?
        //change to float/double so we can do division with it?
            //at that point, it might be better to find another way.

        //For when we want dice that aren't simply numbered 1-whatever.
        //Expected to have a number of indices equal to dieSize + 1.
            //Index n corresponds to the side that would normally be numbered n.
        //private int[] mapping;
        //still to be done.

        private List<DiceCondition> conditions;

        //These two are used by the program to temporarily store values that get generated.
        //These aren't needed anymore, but I'll leave them in for now because they're not really hurting anything.
        private int total;
        private string description;

        //potential bonuses to be able to handle:
            //statmod
            //statscore
            //prof
                //just prof or make it so it'll calculate prof/no and add prof/0?
                    //and expertise?
                    //probably just prof, certainly for now.
            //level?
            //use some type of array, probably of strings, and parse them later.
                //make the roll() function take an optional dictionary of character details.

        //consider dropping bonus and finding a different way to represent it.
            //if die size is 0, treat num dice as the bonus?
            //We're generally only going to need it once or twice in a given string, even if it's very long.
            //It makes sense to be able to split on every + and almost every -

        //possibly add a descriptor
            //so we can have things like 1d8 + 1 slashing + 3d6 fire and it'll output something like "4 slashing + 10 fire = 14"
            //just have it be applied to everything before it.
            //how to identify descriptors and separate them from other stuff?
                //maybe require that they be wrapped in square brackets or something.
                    //manually specify no type by adding [] after a chunk.
                //maybe require that the dice chunk(s) and the descriptor both be wrapped in the same square brackets.
                    //ie: 4d6 + [2d6 + 1 fire] + [1d8 slashing]
                    //then expect that the type will be the last part
            //have it be smart enough to combine like types at some point in the process.
                //probably after rolling, so the description contains the dice in the order they were specified.
                //set up an object or something and set up an index in it for each type specified?


        //Constructor(s)
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        public DicePile (int dieSize, int numDice, int bonus, string dynamicBonus, int numAdv, int numDis, int multiplier, List<DiceCondition> conditions)
        {
            this.dieSize = dieSize;
            this.numDice = numDice;
            this.flatBonus = bonus;
            this.dynamicBonus = dynamicBonus;
            this.numAdv = numAdv;
            this.numDis = numDis;
            this.multiplier = multiplier;
            this.conditions = conditions;

            this.total = 0;
            this.description = "";
        }
        public DicePile(int dieSize, int numDice = 0, int bonus = 0, string dynamicBonus = "", int numAdv = 0, int numDis = 0, int multiplier = 1)
        {
            this.dieSize = dieSize;
            this.numDice = numDice;
            this.flatBonus = bonus;
            this.dynamicBonus = dynamicBonus;
            this.numAdv = numAdv;
            this.numDis = numDis;
            this.multiplier = multiplier;
            this.conditions = new List<DiceCondition>();

            this.total = 0;
            this.description = "";
        }

        //public DicePile (string dieSize, int numDice = 0, int bonus = 0, int numAdv = 0, int numDis = 0)
        //{
        //    //to do
        //
        //    //Check if this is d or D and then a number.
        //    //If not, fail and quit.
        //    //If yes, pull out that number and set it into dieSize
        //}


        //Public functions
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Returns a duplicate of this DicePile.
        public DicePile copyMe()
        {
            DicePile ret = new DicePile(this.dieSize, this.numDice, this.flatBonus, this.dynamicBonus, this.numAdv, this.numDis, this.multiplier);

            return ret;
        }

        //Rolls this collection of dice and returns the total.
        //stats contains:
            //[stat]mod     ie strmod
            //[stat]score   ie strscore
            //prof
            //level
        public DiceResponse roll (Dictionary<string, int> stats)
        {
            int numToRoll = this.numDice + this.numAdv + this.numDis;

            //to do: check stats before returning.
            //revamp function so the dice section merely gets skipped, rather than exiting early?

            string desc = getDiceString();
            int ret = 0;

            if (dieSize > 0)
            {
                desc += " (";

                //Actually roll dice.
                int[] rolls = new int[numToRoll];
                Random rand = new Random();
                
                for (int d = 0; d < numToRoll; d++)
                {
                    //Generate a random number from 1 to dieSize.
                    //If we get this far, dieSize should be at least 1.
                    int roll = 1;
                    if (dieSize > 1)
                    {
                        roll = rand.Next(1, dieSize+1);
                    }

                    //error checking? logging?

                    if (d > 0)
                    {
                        desc += ", ";
                    }
                    desc += roll;

                    rolls[d] = roll;
                }

                Array.Sort(rolls);

                //Calculate the total, excluding those dropped for adv/dis.
                int start = this.numAdv;
                int end = this.numDice + this.numAdv;

                for (int d = start; d < end; d++)
                {
                    ret += rolls[d];
                }

                desc += "="+ret+")";
            } else
            {
                //Just add bonuses.
            }

            //Add flat bonus
            ret += flatBonus;
            if (dynamicBonus.Length> 0)
            {
                //Check the character's stats that were passed in.
                if (stats.ContainsKey(dynamicBonus))
                {
                    ret += stats[dynamicBonus];
                }
                else if (dynamicBonus.Substring(0, 10) == "profifprof")
                {
                    //First we'll pull out the thing we're supposed to check for.
                    //This will be from the 12th character (index 11) and go strlen-12 characters
                    string thing = dynamicBonus.Substring(11, dynamicBonus.Length - 12);

                    //desc += "Adding " + thing;

                    //Now we look up the thing and see if we have it.
                    if (stats.ContainsKey(thing))
                    {
                        ret += (stats["prof"] * stats[thing]);
                        //to do: translate thing into a more human readable version
                            //just write a function for it with a switch statement with hardcoded values?
                            //write a function that checks a hardcoded dictionary?
                        desc = "prof("+thing+")";
                    }
                    else if (false)
                    {
                        //use this one for checking the list of misc profs, once i add it
                    }
                    else
                    {
                        //complain to a log?
                    }
                }   //if dynamic bonus is profifprof(whatever)
            }   //if there's a dynamic bonus

            ret *= this.multiplier;

            //Calculate the description for non-dice options.
            if (dieSize== 0)
            {
                //We're not including dice, list whatever we are using.
                if (dynamicBonus.Length > 0)
                {
                    desc += " (" + ret + ")";
                }
                else
                {
                    //this isn't properly taking negatives into account
                    desc = ret.ToString();
                }
            }

            //Possibly make it negative.
            //ret *= this.multiplier;

            /*
            if (dieSize <= 0)
            {
                this.total = this.flatBonus * this.multiplier;
                this.description = "bonus only: " + (this.flatBonus * this.multiplier) + " ";
                return new DiceResponse(true, this.total, this.description);
            } else if (dieSize == 1)
            {
                this.total = (this.numDice + this.flatBonus) * this.multiplier;
                this.description = "bonus+num dice: "+this.numDice + "d1+" + this.flatBonus + " times " + this.multiplier + " ";
                return new DiceResponse(true, this.total, this.description);
            }
            
            int[] rolls = new int[numToRoll];
            Random rand = new Random();
            string desc = numToRoll + "d" + this.dieSize + " adv"+this.numAdv + " dis"+this.numDis + " + " + this.flatBonus + " (";
            
            for (int d = 0; d < numToRoll; d++)
            {
                //Generate a random number from 1 to dieSize.
                //If we get this far, dieSize should be at least 2.
            
                int roll = rand.Next(1, dieSize);
                
                //error checking? logging?
            
                if (d > 0)
                {
                    desc += ", ";
                }
                desc += roll;
            
                rolls[d] = roll;
            }
            
            //Sort the rolls from lowest -> highest.
            Array.Sort (rolls);
            
            //We want to count all rolls except the numAdv lowest (first) and the numDis highest (last).
            int start = this.numAdv;
            int end = this.numDice + this.numAdv;
            int ret = 0;
            
            for (int d = start; d < end; d++)
            {
                ret += rolls[d];
                //List the rolls here, eventually?
            }
            
            //Don't forget the bonus and multiplier.
            ret += this.flatBonus;
            ret *= this.multiplier;
            */

            this.total = ret;

            this.description = desc;

            return new DiceResponse(true, this.total, this.description);
        }

        public string getDiceString ()
        {
            string ret = "";

            if (numDice > 0)
            {
                ret = numDice.ToString() + "d" + dieSize.ToString();
                if (numAdv> 0)
                {
                    ret += " adv" + numAdv;
                }
                if (numDis > 0)
                {
                    ret += " dis" + numDis;
                }
            }
            else if (dynamicBonus.Length > 0)
            {
                ret = dynamicBonus;
            }
            else
            {
                ret = flatBonus.ToString();
            }

            return ret;
        }

        public int getTotal()
        {
            return this.total;
        }

        public string getDescription()
        {
            return this.description;
        }


        //Overloading operators, and related functions.
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        public DicePile combine (DicePile other)
        {
            //
            return this;
            //maybe change to bool and have it just change this in place.
        }

        //Checks if two instances of this class are able to have their quantities combined.
        //to do: rename to compatible() or somesuch?
        public bool sameType (DicePile other)
        {
            //I don't think there are any circumstances where it would be appropriate to combine two piles with adv and/or dis.
            if (this.numAdv > 0 || other.getNumAdv() > 0)
            {
                return false;
            }
            if (this.numDis > 0 || other.getNumDis() > 0)
            {
                return false;
            }

            //Obviously the two piles need to be the same size of dice.
            if (this.dieSize != other.getDieSize())
            {
                return false;
            }
            //Compare damage type or whatever, when I add that.

            //We ignore the number of dice, since that's not what this function is about.

            //If we get this far, they're identical for our purposes.
            return true;
        }

        ////We'll overload == and != to compare dice types only, not quantities.
        //public static bool operator ==(DicePile one, DicePile two)
        //{
        //    return one.sameType(two);
        //}
        //
        //public static bool operator !=(DicePile one, DicePile two)
        //{
        //    return !one.sameType (two);
        //}

        //overload + (and -?)
        //adds quantities together
        //use sameType or whatever I rename it to to make sure they're compatible first, and just return the first one if not?


        //Private functions
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Does this even need any?


        //Simple getter/setter functions.
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        public int getBonus()
        {
            return this.flatBonus;
        }

        public string getDynamicBonus()
        {
            return this.dynamicBonus;
        }

        public int getDieSize()
        {
            return this.dieSize;
        }

        public int getMultiplier()
        {
            return this.multiplier;
        }

        public int getNumAdv()
        {
            return this.numAdv;
        }

        public int getNumDice()
        {
            return this.numDice;
        }

        public int getNumDis()
        {
            return this.numDis;
        }

        public void setBonus(int bonus)
        {
            this.flatBonus = bonus;
        }

        public void setDynamicBonus(string bonus)
        {
            this.dynamicBonus = bonus;
        }

        public void setDieSize(int dieSize)
        {
            this.dieSize = dieSize;
        }

        public void setMultiplier(int multiplier)
        {
            this.multiplier = multiplier;
        }

        public void setNumAdv(int numAdv)
        {
            this.numAdv = numAdv;
        }

        public void setNumDice(int numDice)
        {
            this.numDice = numDice;
        }

        public void setNumDis(int numDis)
        {
            this.numDis = numDis;
        }

        public void setConditions (List<DiceCondition> conditions)
        {
            this.conditions = conditions;
        }
    }
}