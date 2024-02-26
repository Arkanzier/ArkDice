using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Text.Json.Serialization;

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

        //Add eventually:
        //For when we want dice that aren't simply numbered 1-whatever.
        //Expected to have a number of indices equal to dieSize + 1.
            //Index n corresponds to the side that would normally be numbered n.
        //private int[] mapping;

        //Coming soon?
        public List<DiceCondition> Conditions { get; set; }


        //Constructors
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        [JsonConstructor]
        public DicePile ()
        {
            DieSize = 0;
            NumDice = 0;
            NumAdv = 0;
            NumDis = 0;
            FlatBonus = 0;
            DynamicBonus = "";
            Multiplier = 0;
            Conditions = new List<DiceCondition> ();
        }
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
        }


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
            Dictionary<string, double> empty = new Dictionary<string, double>();
            return Roll(empty);
        }

        //Rolls this collection of dice and returns the total.
        //stats is expected to receive data in the format outputted by Character.GetGeneralStatistics()
        public DiceResponse Roll (Dictionary<string, double> stats)
        {
            int numToRoll = this.NumDice + this.NumAdv + this.NumDis;

            //to do: check stats before returning.

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
                    ret += (int)stats[DynamicBonus];
                }
                else if (DynamicBonus.Substring(0, 10) == "profifprof")
                {
                    //First we'll pull out the thing we're supposed to check for.
                    //This will be from the 12th character (index 11) and go strlen-12 characters
                    string thing = DynamicBonus.Substring(11, DynamicBonus.Length - 12);

                    //Now we look up the thing and see if we have it.
                    if (stats.ContainsKey(thing))
                    {
                        ret += (int)((stats["prof"] * stats[thing]));
                        desc = "prof("+thing+")";
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

            return new DiceResponse(true, ret, desc);
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
        //Coming soon?
        //Combines this object with another DicePile.
        public DicePile Combine (DicePile other)
        {
            //
            return this;
            //maybe change to bool and have it just change this in place.
        }

        //Coming soon?
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
    }
}