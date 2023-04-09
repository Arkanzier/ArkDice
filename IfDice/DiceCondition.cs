using System;
using System.Reflection;

namespace ArkDice
{
    //This is used to store conditionals in the dice related logic.
    //Things like 'if d20 roll is nat 20, roll crit damage instead of regular damage'
    public struct DiceCondition
    {
        //These two define the condition: the roll must be greater than / less than / whatever targetNumber.
        public int targetNumber;
        public string comparator;
            //Can be:
                //<, <=, >, >=, =

        //This defines whether we're looking at the roll's total, the highest natural roll, or the lowest natural roll.
        public string scope;
            //Can be:
                //"total"
                //"nat1"
                //"natAll"
            //to do: consider changing to total / natHigh / natLow

        public string diceString;
            //Is there a way to make this a DiceCollection instead?
                //It will be generated from a dice string anyway, so it's not a big deal.

        //If the reroll is triggered and this isn't empty string, this label will be used to describe that the new roll is being done.
        string rollLabel;

        //to do: I'm going to need some way to get the die rolls to feed into this.
            //include one for all the die rolls, and one for only the d20 die rolls.
            //don't include dice that get dropped for adv/dis in this.
            //getDieRolls() and getD20Rolls() ?
            //Should they return int arrays or just lists?
        //to do: testing
            //death save: d20 (plus modifier from item?), if >= 10 label is "success", if < 10 label is "failure"
            //attack roll: if nat 1, don't roll damage. If nat 20, roll crit damage. Otherwise, roll regular damage.
        //to do: attacks
            //consider setting something up to auto generate attacks based on weapon stats (auto use the higher of Str/Dex)
                //Have buttons or something to add adv/+d4/whatever to the attack roll.
            //consider just using this but having some way to either:
                //automatically apply adv / bless / whatever to the attack roll when the appropriate button is pressed.
                    //maybe some sort of category variable on the basic ability?
                //automatically show/hide different versions of the basic abilities based on whether there's supposed to be adv/bless/etc.

        public DiceCondition()
        {
            targetNumber = 0;
            comparator = "";
            scope = "";
            diceString = "";
            rollLabel = "";
        }
        public DiceCondition (int targetNumber, string comparator, string scope, string diceString, string rollLabel = "")
        {
            this.targetNumber = targetNumber;
            this.comparator = comparator;
            this.scope = scope;
            this.diceString = diceString;
            this.rollLabel = rollLabel;
        }

        //Public functions:
        //-------- -------- -------- -------- -------- -------- -------- -------- 

        //Checks this struct's conditions against a roll to see if it's dice should be rolled.
        public bool shouldRoll(int total, List<int> rolls)
        {
            //First we calculate which number to use in the comparison.
            int c = getComparisonNumber(total, rolls);

            if (comparator == ">")
            {
                if (c > targetNumber) { return true; } else { return false; }
            }
            else if (comparator == ">=")
            {
                if (c >= targetNumber) { return true; } else { return false; }
            }
            else if (comparator == "<")
            {
                if (c < targetNumber) { return true; } else { return false; }
            }
            else if (comparator == "<=")
            {
                if (c <= targetNumber) { return true; } else { return false; }
            }
            else if (comparator == "=")
            {
                if (c == targetNumber) { return true; } else { return false; }
            }
            else
            {
                return false;
            }
        }


        //Private functions:
        //-------- -------- -------- -------- -------- -------- -------- -------- 

        //A central place for interpreting rolls and the scope variable to get the number to compare against targetNumber.
        int getComparisonNumber (int total, List<int> rolls)
        {
            //We'll use this for defining the default number that will be sent back when we don't have the info to actually calculate the correct number.
            int def = 0;

            if (scope == "total")
            {
                //We just use the total.
                return total;
            }
            else if (scope == "nat1")
            {
                //We look at the highest die roll.
                int highest = def;
                for (int a = 0; a < rolls.Count; a++)
                {
                    if (rolls[a] > highest) { highest = rolls[a]; }
                }
                return highest;
            }
            else if (scope == "natAll")
            {
                //All rolls must be >= whatever our target number is, so we'll return the lowest roll.
                int lowest = def;
                if (rolls.Count > 0)
                {
                    lowest = rolls[0];
                }
                for (int a = 1; a < rolls.Count; a++)
                {
                    if (rolls[a] < lowest) { lowest = rolls[a]; }
                }
                //to do: if there are no rolls, this returns 0. Is that what I want?
                return lowest;
            }
            else
            {
                //to do: decide how to handle this.
                return def;
            }
        }
    }
}