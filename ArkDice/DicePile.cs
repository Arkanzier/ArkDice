using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace ArkDice
{
    //This class represents one or more dice of the same size, or a non-die bonus to a roll.
    public class DicePile
    {
        //Number of sides per die.
        public int DieSize { get; set; }
        //The number of dice.
        public int NumDice { get; set; }
        //This many additional dice will be rolled, and then this many of the lowest rolls will be dropped.
        public int NumAdv { get; set; }
        //This many additional dice will be rolled, and then this many of the highest rolls will be dropped.
        public int NumDis { get; set; }
        //A flat, unchanging bonus to a roll (ie: +4)
        public int FlatBonus { get; set; }
        //A bonus to a roll based on some external condition, like a character's stats.
        public string DynamicBonus { get; set; }
        //The total of the roll will be multiplied by this number.
        //Should generally be either 1 or -1 to represent positive or negative, but other values can be used if necessary.
        public int Multiplier { get; set; }
        //change to float/double so we can do division with it?
            //at that point, it might be better to find another way.

        //For when we want dice that aren't simply numbered 1-whatever.
        //Expected to have a number of indices equal to dieSize + 1.
            //Index n corresponds to the side that would normally be numbered n.
        //private int[] mapping;
        //still to be done.

        //Coming soon?
        public List<DiceCondition> Conditions { get; set; }

        //These two are used by the program to temporarily store values that get generated.
        //These theoretically aren't needed anymore, but I'll leave them in for now because they're not really hurting anything.
        public int Total { get; set; }
        public string Description { get; set; }

        //consider dropping bonus and finding a different way to represent it.
            //if die size is 0, treat num dice as the bonus?
            //We're generally only going to need it once or twice in a given string, even if it's very long.
            //It makes sense to be able to split on every + and almost every -

        //possibly add a descriptor
            //so we can have things like 1d8 + 1 slashing + 3d6 fire and it'll output something like "4 slashing + 10 fire = 14"
            //just have it be applied to everything before it but after the previous descriptor.
                //if I do this, add an option for a 'no descriptor' keyword. "nodescriptor" ?
            //how to identify descriptors and separate them from other stuff?
                //maybe require that they be wrapped in square brackets or something.
                    //manually specify no type by adding [] after a chunk.
                //maybe require that the dice chunk(s) and the descriptor both be wrapped in the same square brackets.
                    //ie: 4d6 + [2d6 + 1 fire] + [1d8 slashing]
                    //then expect that the type will be the last part
            //have it be smart enough to combine like types at some point in the process?
                //probably after rolling, so the description contains the dice in the order they were specified.
                //set up an object or something and set up an index in it for each type specified?


        //Constructor(s)
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        public DicePile (int dieSize, int numDice, int bonus, string dynamicBonus, int numAdv, int numDis, int multiplier, List<DiceCondition> conditions)
        {
            this.DieSize = dieSize;
            this.NumDice = numDice;
            this.FlatBonus = bonus;
            this.DynamicBonus = dynamicBonus;
            this.NumAdv = numAdv;
            this.NumDis = numDis;
            this.Multiplier = multiplier;
            this.Conditions = conditions;

            this.Total = 0;
            this.Description = "";
        }
        public DicePile(int dieSize, int numDice = 0, int bonus = 0, string dynamicBonus = "", int numAdv = 0, int numDis = 0, int multiplier = 1)
        {
            this.DieSize = dieSize;
            this.NumDice = numDice;
            this.FlatBonus = bonus;
            this.DynamicBonus = dynamicBonus;
            this.NumAdv = numAdv;
            this.NumDis = numDis;
            this.Multiplier = multiplier;
            this.Conditions = new List<DiceCondition>();

            this.Total = 0;
            this.Description = "";
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
        public DicePile CopyMe()
        {
            DicePile ret = new DicePile(this.DieSize, this.NumDice, this.FlatBonus, this.DynamicBonus, this.NumAdv, this.NumDis, this.Multiplier);

            return ret;
        }

        //Wrapper for the other roll() function for when we have no stats to pass in.
        public DiceResponse Roll ()
        {
            Dictionary<string, int> empty = new Dictionary<string, int>();
            return Roll(empty);
        }

        //Rolls this collection of dice and returns the total.
        //stats is expected to receive data in the format outputted by Character.GetGeneralStatistics()
        public DiceResponse Roll (Dictionary<string, int> stats)
        {
            int numToRoll = this.NumDice + this.NumAdv + this.NumDis;

            //to do: check stats before returning.
            //revamp function so the dice section merely gets skipped, rather than exiting early?

            string desc = GetDiceString();
            int ret = 0;

            if (DieSize > 0)
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
                    if (DieSize > 1)
                    {
                        roll = rand.Next(1, DieSize +1);
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
                int start = this.NumAdv;
                int end = this.NumDice + this.NumAdv;

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
            ret += FlatBonus;
            if (DynamicBonus.Length> 0)
            {
                //Check the character's stats that were passed in.
                if (stats.ContainsKey(DynamicBonus))
                {
                    ret += stats[DynamicBonus];
                }
                else if (DynamicBonus.Substring(0, 10) == "profifprof")
                {
                    //First we'll pull out the thing we're supposed to check for.
                    //This will be from the 12th character (index 11) and go strlen-12 characters
                    string thing = DynamicBonus.Substring(11, DynamicBonus.Length - 12);

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

            ret *= this.Multiplier;

            //Calculate the description for non-dice options.
            if (DieSize == 0)
            {
                //We're not including dice, list whatever we are using.
                if (DynamicBonus.Length > 0)
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

            this.Total = ret;

            this.Description = desc;

            return new DiceResponse(true, this.Total, this.Description);
        }

        //Returns a string that, if fed into the appropriate function in a DiceCollection instance, will get a duplicate of this.
        public string GetDiceString ()
        {
            string ret = "";

            if (NumDice > 0)
            {
                ret = NumDice.ToString() + "d" + DieSize.ToString();
                if (NumAdv> 0)
                {
                    ret += " adv" + NumAdv;
                }
                if (NumDis > 0)
                {
                    ret += " dis" + NumDis;
                }
            }
            else if (DynamicBonus.Length > 0)
            {
                ret = DynamicBonus;
            }
            else
            {
                ret = FlatBonus.ToString();
            }

            return ret;
        }


        //Overloading operators, and related functions.
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Combines this object with another DicePile.
        public DicePile Combine (DicePile other)
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
            if (this.NumAdv > 0 || other.NumAdv > 0)
            {
                return false;
            }
            if (this.NumDis > 0 || other.NumDis > 0)
            {
                return false;
            }

            //Obviously the two piles need to be the same size of dice.
            if (this.DieSize != other.DieSize)
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
        //Does this even need any?
    }
}