﻿using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using System.Transactions;
using static System.Formats.Asn1.AsnWriter;
using System.Text.Json;
using System.Collections;
using System.Text.Json.Serialization;

namespace ArkDice
{
    public class DiceCollection
    {
        public List<DicePile> Dice { get; set; }

        public List<DiceCondition> Conditions { get; set; }

        //These two are used by the program to temporarily store values that get generated.
        [JsonIgnore]
        public int Total { get; set; }
        [JsonIgnore]
        public string Description { get; set; }


        //Constructors
        [JsonConstructor]
        public DiceCollection()
        {
            Dice = new List<DicePile>();
            Conditions = new List<DiceCondition>();
            Total = 0;
            Description = "";
        }
        public DiceCollection (string diceString = "")
            : this()
        {
            //It's possible for there to be 'no dice string' in things.
            //This is represented by specifying 'false' for the dice string.
            if (diceString.ToLower() == "false")
            {
                //No dice to add, leave the list zeroed out.
            }
            else
            {
                Dice = new List<DicePile>();
                bool temp = ParseDiceCollection(diceString);
                //error checking goes here?
            }
        }

        //Public functions
        //-------- -------- -------- -------- -------- -------- -------- -------- 

        //Adds or removes one die at a time from this.
        //Arguments:
            //dieSize: the size of the dice to add / remove.
            //subtract: if true, this will remove dice instead of adding them.
            //advdis: controls whether this adds regular dice, advantage dice, or disadvantage dice.
                //<0: disadvantage
                //0: regular
                //>0: advantage
            //flatBonus: indicates that this is to add/remove a flat bonus instead of a die.
        public bool AddOneDie (int dieSize, bool subtract = false, int advdis = 0, bool flatBonus = false)
        {
            for (int a = 0; a < Dice.Count; a++)
            {
                DicePile pile = Dice[a];
                if (flatBonus && pile.DieSize == 0)
                {
                    int thisMultiplier = (subtract) ? -1 : 1;

                    dieSize *= thisMultiplier;

                    Dice[a].FlatBonus += dieSize;

                    //error checking goes here

                    //Check if this bonus is now 0. If so, get rid of it.
                    if (Dice[a].FlatBonus == 0)
                    {
                        Dice.RemoveAt(a);
                    }

                    return true;
                }
                else if (!flatBonus && pile.DieSize == dieSize)
                {
                    //Figure out whether we're adding dice or subtracting them.
                    //We add if subtract and the multiplier match.
                    int numToAdd;
                    if (!subtract && pile.Multiplier > 0 || subtract && pile.Multiplier < 0)
                    {
                        //We might be adding dice that get added to the total.
                        //We might be adding dice that get removed from the total.
                        //Either way, we want to make the number bigger.
                        numToAdd = 1;
                    } else
                    {
                        //We might be removing dice that get added to the total.
                        //We might be removing dice that get subtracted from the total.
                        //Either way, we want to make the number smaller.
                        numToAdd = -1;
                    }

                    if (advdis == 0)
                    {
                        Dice[a].NumDice += numToAdd;
                        //We don't have a way to check if that worked, so we'll assume it did.

                        //Check if this now has 0 dice. If so, get rid of it.
                        if (Dice[a].NumDice == 0)
                        {
                            Dice.RemoveAt(a);
                        }
                        return true;
                    }
                    else if (advdis > 0)
                    {
                        Dice[a].NumAdv += numToAdd;
                        //We don't have a way to check if that worked, so we'll assume it did.
                        return true;
                    }
                    else if (advdis < 0)
                    {
                        Dice[a].NumDis += numToAdd;
                        //We don't have a way to check if that worked, so we'll assume it did.
                        return true;
                    }
                }   //if die sizes match
            }   //for each pile in dice

            //If we get this far, we weren't able to find a matching pile, so we'll make a new one.
            int numDice = (advdis == 0) ? 1 : 0;
            int numAdv = (advdis > 0) ? 1 : 0;
            int numDis = (advdis < 0) ? 1 : 0;
            int multiplier = (subtract) ? -1 : 1;
            int bonus = 0;
            if (flatBonus)
            {
                bonus = dieSize;
                dieSize = 0;
            }

            DicePile newPile = new DicePile(dieSize, numDice, bonus, "", numAdv, numDis, multiplier);
            Dice.Add(newPile);

            return true;
        }

        //Retrieves the description from a previously-done roll.
        //Note that this will return empty string if no roll has been done yet.
        public string GetDescription()
        {
            return this.Description;
        }

        //Retrieves the total from a previously-done roll.
        //Note that this will return 0 if no roll has been done yet.
        public int GetTotal()
        {
            return this.Total;
        }

        //Rolls the provided dice and plugs the total into the total variable for retrieval later.
        //Also puts a text description of the roll into the description variable.
        public DiceResponse Roll ()
        {
            return Roll (new Dictionary<string, double>());
        }
        public DiceResponse Roll (Dictionary<string, double> stats)
        {
            int total = 0;
            string desc = "";

            foreach (var dp in Dice)
            {
                DiceResponse roll = dp.Roll(stats);
                int tempTot = roll.Total;
                string tempDesc = roll.Description;

                total += tempTot;
                if (desc.Length > 0)
                {
                    //Something is already here, we'll add a + or -.
                    string symbol = "+";
                    if (dp.Multiplier == -1)
                    {
                        symbol = "-";
                    }

                    desc += " " + symbol + tempDesc;
                }
                else
                {
                    desc = tempDesc;
                }
            }

            this.Total = total;
            this.Description = "Rolled: " + total + ": " + desc;

            return new DiceResponse(true, total, Description);
        }

        //Generates a dice string that will be usable to recreate this instance of this class.
        public string GetDiceString()
        {
            string ret = "";

            for (int a = 0; a < Dice.Count; a++)
            {
                DicePile dp = Dice[a];
                int numDice = dp.NumDice;
                int dieSize = dp.DieSize;
                int bonus = dp.FlatBonus;
                string dynamicBonus = dp.DynamicBonus;

                int numAdv = dp.NumAdv;
                int numDis = dp.NumDis;
                int multiplier = dp.Multiplier;

                //+ or -, if necessary.
                if (multiplier < 0)
                {
                    ret += " - ";
                } else
                {
                    if (ret == "")
                    {
                        //We don't need to put a plus here.
                    } else
                    {
                        ret += " + ";
                    }
                }

                //This will have dice or a flat bonus or a dynamic bonus, but only one.
                if (dieSize > 0)
                {
                    ret += numDice + "d" + dieSize + " ";
                }
                else if (dynamicBonus != "")
                {
                    ret += dynamicBonus + " ";
                } else
                {
                    //assume this is here for the flat bonus.
                    ret += bonus + " ";
                }

                //Advantage and disadvantage.
                if (numAdv > 0)
                {
                    ret += "adv" + numAdv + " ";
                }
                if (numDis > 0)
                {
                    ret += "dis" + numDis + " ";
                }

                //type or whatever I'll call it goes here when I've added it.
            }

            return ret;
        }


        //Private functions
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        /*
         *  Acceptable dice string chunk formats:
         *      xdy
         *      x
         *      xdy adv or xdy dis
         *      xdy advn disz
         *      something valid followed by [text]
         *      negative versions of everything valid
         *      more?
        */

        //The function we'll use to convert a string of dice values into an instance of this class.
        //Returns true on success, false on failure.
        private bool ParseDiceCollection (string diceString)
        {
            List<DicePile> renameme = ParseDiceString (diceString);

            //error checking goes here.

            foreach (DicePile dp in renameme)
            {
                //Create a duplicate of the DicePile and add it to the list.
                //We won't do anything to combine stuff under the assumption that the user probably wants things the way they wrote it.

                DicePile temp = dp.CopyMe();

                this.Dice.Add(temp);
            }

            return true;
        }

        //Parses the condition portion of a dice string.
        //to do: set something up somewhere to be able to handle dicepile level stuff and dicecollection level
            //probably adjust the regex that splits off individual dicepile chunks to only include conditions starting with "(if:"
        private List<DiceCondition> ParseDiceCondition (string condition)
        {
            //Expected formats:
                //(if: scope operator targetnumber dicestring)
                //(iftotal: operator targetnumber dicestring)
                    //this one has no scope because the scope must be "total"

            List<DiceCondition> ret = new List<DiceCondition> ();

            string regex = "^\\s*\\(\\s*(if|iftotal)\\s*:\\s*([a-z]+)?\\s*(<|<=|>|>=|=)\\s*([0-9]+)\\s*([^\\(\\)]*)\\s*\\)\\s*$";

            //Split them based on parentheses so we can process them individually.
            var pieces = condition.Split(new char[] { '(', ')' });
            foreach (var piece in pieces)
            {
                DiceCondition temp = new DiceCondition ();

                Match matches = Regex.Match(piece, regex, RegexOptions.IgnoreCase);
                //Matches:
                    //0: full match
                    //1: if / iftotal
                    //2: scope, if present
                    //3: operator
                    //4: target number
                    //5: dice string

                if (!matches.Success)
                {
                    //Complain to a log file?
                    continue;
                }

                //Make sure everything that we expect is present.
                //Remember that index 2 is only expected when index 1 is "if"
                if (matches.Groups[1].Value.Length == 0 || matches.Groups[3].Value.Length == 0 || matches.Groups[3].Value.Length == 0 || matches.Groups[4].Value.Length == 0 || matches.Groups[5].Value.Length == 0)
                {
                    //Complain in a log file?
                    continue;
                }

                string iftemp = matches.Groups[1].Value.Trim().ToLower();
                string scope = "";
                if (matches.Groups[2].Value.Length > 0)
                {
                    scope = matches.Groups[2].Value.Trim().ToLower();
                }
                if (iftemp == "if")
                {
                    //We require a scope
                    if (scope == "")
                    {
                        //complain or something
                        continue;
                    }
                }
                else if (iftemp == "iftotal")
                {
                    //We require that there not be a scope.
                    if (scope != "" && scope != "total")
                    {
                        //complain or something?
                    }
                    scope = "total";
                }
                else
                {
                    //Unsupported option
                    //Complain to a log file or something
                    continue;
                }
                string comparator = matches.Groups[3].Value.Trim().ToLower();
                string numberString = matches.Groups[4].Value.Trim().ToLower();
                //parse to int
                int targetNumber;
                if (!Int32.TryParse (numberString, out targetNumber))
                {
                    //complain to a log
                    continue;
                }
                string diceString = matches.Groups[5].Value.Trim().ToLower();
                //remove the tolower from this one?

                //slot the stuff into temp
                temp.scope = scope;
                temp.targetNumber = targetNumber;
                temp.comparator= comparator;
                temp.diceString= diceString;
                //roll label

                //Add this to the list.
                ret.Add (temp);
            }

            return ret;
        }

        //Underlying function: translates one chunk of a dice string (ie: "+XdY") into a DicePile instance.
        private DicePile ParseDicePile (string dicePile)
        {
            DicePile ret = new DicePile(0);

            string temp = dicePile.Trim();

            //Expected format:
                //+, -, or nothing (implied +)
                //1 of:
                    //dice
                    //flat bonus
                    //dynamic bonus
                //adv and/or dis or nothing
                    //how to get adv and dis without requiring a specific order?

            //Don't bother to validate correctness in the regex level, we'll check it in the code later.
            string plusSection = "(\\+|-|)";
            string diceSection = "(?:([0-9]*)d([0-9]+))";
            string flatBonusSection = "([0-9]+)";
            string dynamicBonusSection = "([a-zA-Z0-9]+\\s*(?:\\s*\\([a-zA-Z0-9]+\\)\\s*)?)";
            string advDisSection = "(?:(adv[0-9]*)?\\s*(dis[0-9]*)?|(dis[0-9]*)?\\s*(adv[0-9]*)?)";
            string ifSection = "(\\(\\s*if:[^\\(\\)]+\\))*";

            string regex = "^" + plusSection + "\\s*(?:" + diceSection + "|" + flatBonusSection + "|" + dynamicBonusSection + ")\\s*" + advDisSection + "\\s*" + ifSection + "$";
            //Matches:
                //0: full match
                //1: + or - or nothing
                //2: number of dice
                //3: die size
                //4: flat bonus
                //5: dynamic bonus
                //6: advantage
                //7: disadvantage
                //8: disadvantage
                //9: advantage
                //10: if clauses (grouped)
            Match matches = Regex.Match (temp, regex, RegexOptions.IgnoreCase);
            if (!matches.Success)
            {
                //Complain to a log file.
                return ret;
            }

            //Minus sign, if present.
            if (matches.Groups[1].Value.Length > 0)
            {
                int multiplier = 1;
                if (matches.Groups[1].Value == "-") {
                    multiplier = -1;
                }
                ret.Multiplier = multiplier;
            }

            //Number of dice, if present.
            if (matches.Groups[2].Value.Length > 0)
            {
                int numDice = Int32.Parse(matches.Groups[2].Value);
                ret.NumDice = numDice;
            }
            else if (matches.Groups[3].Value.Length > 0)
            {
                //It looks like we got something in the format "dX" so we'll set the number of dice to 1.
                ret.NumDice = 1;
            }

            //Die size, if present.
            if (matches.Groups[3].Value.Length > 0)
            {
                int dieSize = Int32.Parse(matches.Groups[3].Value);
                ret.DieSize = dieSize;
            }

            //Flat bonus, if present.
            if (matches.Groups[4].Value.Length > 0)
            {
                int bonus = Int32.Parse(matches.Groups[4].Value);
                ret.FlatBonus = bonus;
            }

            //Dynamic bonus, if present
            if (matches.Groups[5].Value.Length > 0)
            {
                string dynamicBonus = matches.Groups[5].Value;
                ret.DynamicBonus = dynamicBonus;
            }

            //Adv/Dis, if present.
            //First we get the values.
            string advString = "";
            string disString = "";
            int numAdv = 0;
            int numDis = 0;
            //Advantage, if present, will be index 6 or 9.
            if (matches.Groups[6].Value.Length > 0)
            {
                advString = matches.Groups[6].Value;
            }
             else if (matches.Groups[9].Value.Length > 0)
            {
                advString = matches.Groups[9].Value;
            }
            //Disadvantage, if present, will be index 7 or 8.
            if (matches.Groups[7].Value.Length > 0)
            {
                disString = matches.Groups[7].Value;
            }
            else if (matches.Groups[8].Value.Length > 0)
            {
                disString = matches.Groups[8].Value;
            }
            //Now we parse whatever we got.
            string advDisRegex = "^(adv|dis)([0-9]*)$";
            if (advString.Length > 0)
            {
                Match tempmatches = Regex.Match(advString, advDisRegex);

                //Double check that we're looking at advantage here.
                //skip this step?

                //Pull out the number, if present.
                if (tempmatches.Groups[2].Value.Length > 0)
                {
                    numAdv = Int32.Parse(tempmatches.Groups[2].Value);
                }
                else
                {
                    numAdv = 1;
                }
            }
            if (disString.Length > 0)
            {
                Match tempmatches = Regex.Match(disString, advDisRegex);

                //Double check that we're looking at advantage here.
                //skip this step?

                //Pull out the number, if present.
                if (tempmatches.Groups[2].Value.Length > 0)
                {
                    numDis = Int32.Parse(tempmatches.Groups[2].Value);
                }
                else
                {
                    numDis = 1;
                }
            }
            //Now we actually set the numbers we found.
            ret.NumAdv = numAdv;
            ret.NumDis = numDis;

            //Handle any conditions specified.
            if (matches.Groups[10].Value.Length > 0)
            {
                List<DiceCondition> conditions = ParseDiceCondition(matches.Groups[10].Value);
                ret.Conditions = conditions;
            }

            return ret;
        }

        //Underlying function: splits a dice string into chunks that can each be converted into one single DicePile instance.
        private List<DicePile> ParseDiceString (string diceString)
        {
            List<DicePile> ret = new List<DicePile>();

            //It's possible for stuff to not have a dice string at all. In this case, the string should be "false".
            if (diceString.ToLower() == "false")
            {
                return ret;
            }

            //Split off any DiceCollection level conditions.
            //Expected format: (iftotal: [^()]+)
            //string conditionsRegex = "\\s*\\(iftotal:\\s*[^\\(\\)]+\\)\\s*$";
            //to do
            //split that stuff into a different string
            //can we just search for the first instance of "(iftotal:" and just take a substring of everythin from there to the end?
                //and turn the original into a substring until that point

            //Split the string into chunks that each correspond to a DicePile.
            string regex = "\\s*([+-]?[^+-]+)";

            string[] chunks = Regex.Split(diceString, regex);

            foreach (string chunk in chunks)
            {
                if (chunk.Trim() == "")
                {
                    continue;
                }
                DicePile temp = ParseDicePile(chunk);

                ret.Add(temp);
            }

            return ret;
        }
    }
}