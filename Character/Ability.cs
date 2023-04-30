using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Schema;
using ArkDice;
using static System.Net.Mime.MediaTypeNames;

namespace Character
{
    public class Ability
    {
        public string ID { get; private set; }
        public string Name { get; private set; }

        public string Action { get; private set; }
        public DiceCollection Dice { get; private set; }

        //Number of currently available uses / charges / whatever.
        public int Uses{ get; private set; }
            //if max uses < 0, unlimited uses?
                //require that current uses = 0?
        //Maximum number of uses / charges / whatever.
        public int MaxUses { get; private set; }
        //How the uses change with each activation.
        public int UsesChange { get; private set; }

        //These three are used by the program to temporarily store values that get generated.
        [JsonIgnore]
        public int Total { get; private set; }
        [JsonIgnore]
        public string Description { get; private set; }
        [JsonIgnore]
        public Dictionary<string, string> Changes { get; private set; }

        public string RechargeCondition{ get; private set; }
        //consider adding a rechargeAmount variable.
            //Determines how much the thing recharges, when it recharges.
            //-1 for all? Just do 9999?
        public string Text{ get; private set; }

        //Used to group abilities together when displayed.
        //Lower-numbered tiers will be displayed first.
        public int DisplayTier { get; private set; }

        //consider adding displayTier or something similar.
            //Decide how I want sorting to work.
                //Ideally allow the user to sort things by name and probably a few other things.


        //eventually expand this to include a parsed set of data.
        //keep it for now as a backup or something.

        //I need this to hold a list of 0 or more dice strings.
            //Represent them as DiceCollections.
            //For each one, I need a list of 0 or more things to do based on the outcome of the first.
            //Maybe simplify this and go with a single string set of controls.
                //That might be too simple to be convenient.
        //I need some sort of instruction for what to do with each dice string.
            //Maybe just roll them and log the results.
            //Maybe roll them and add the total to the character's HP or something.
            //Keep the ability to have counters/whatever.

        //To hold:
        //a list(?) of 0 or more actions
            //action:
                //0 or more dice strings, probably translated to a DiceCollection.
                //for each of those dice strings, 0-1 (or 0+ ?) things to do with the result.
        //Basic things to be able to represent:
            //Here's a dice string, roll it and add the total to one of the character's stats.
            //Here's a dice string and a list of things to do based on the total, roll it and do them.
                //ie: an attack (attack roll + damage), indicate when a crit was rolled and do more damage as appropriate.
                    //if d20 is nat 1: print "natural 1" or whatever and stop.
                    //if d20 is nat 2-19: also roll damage (dice string #2 presumably) and print it.
                    //if d20 is nat 20: print "crit" or whatever and roll crit damage (dice string #3 presumably) and print it.
        //Maybe move that business about different damage types from DiceCollection to here?
            //I kind of like the ability to bake that right in to the DiceCollection.
        //Maybe swap the ability to hold multiple dice strings and call others based on the result of the first one should be replaced with the ability to point to another ability by ID and trigger that based on the result of the roll of the only dice string in here.
            //that would require hooking back into the list of abilities somehow, which i don't like.

        //conditions ideas: mostly about string representation of things
            //total x+ refers to all totals >= x
            //total x- refers to all totals <= x
            //total x-y refers to all totals >=x and <= y
            //nat whatever behaves like total whatever
            //do condition(s) : action(s)
                //i need a way to indicate when an action is always to be done. Condition = true? yes? always? 1? do?

        //Constructor(s):
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        public Ability ()
        {
            ID = "";
            Action = "";
            Name = "";

            Uses = 0;
            MaxUses = 0;
            UsesChange = 0;

            Text = "";
            Dice = new DiceCollection();
            RechargeCondition = "";

            Total = 0;
            Description = "";
            Changes = new Dictionary<string, string>();

            DisplayTier = 10;
        }
        public Ability(string id, string text)
            : this ()
        {
            this.ID = id;
            this.Text = text;
        }
        public Ability(string json)
            : this()
        {
            try
            {
                JsonDocument doc = JsonDocument.Parse(json);
                JsonElement root = doc.RootElement;

                JsonElement temp = new JsonElement();
                int tempint = 0;

                if (root.TryGetProperty("ID", out temp))
                {
                    ID = temp.ToString();
                }
                if (root.TryGetProperty("Name", out temp))
                {
                    Name = temp.ToString();
                }
                if (root.TryGetProperty("Text", out temp))
                {
                    Text = temp.ToString();
                }
                if (root.TryGetProperty("RechargeCondition", out temp))
                {
                    RechargeCondition = temp.ToString();
                }
                if (root.TryGetProperty("Action", out temp))
                {
                    Action = temp.ToString();
                }

                if (root.TryGetProperty("MaxUses", out temp))
                {
                    if (Int32.TryParse(temp.ToString(), out tempint))
                    {
                        MaxUses = tempint;
                    }
                }
                if (root.TryGetProperty("Uses", out temp))
                {
                    if (Int32.TryParse(temp.ToString(), out tempint))
                    {
                        Uses = tempint;
                    }
                }
                if (root.TryGetProperty("UsesChange", out temp))
                {
                    if (Int32.TryParse(temp.ToString(), out tempint))
                    {
                        UsesChange = tempint;
                    }
                }

                if (root.TryGetProperty("Dice", out temp))
                {
                    string temp2 = temp.ToString();

                    //temporary, lazy way of checking if this is a dice string or some json:
                    //to do: make this better
                    //replace this with a function that checks if something is a valid dice string?
                    //replace this with checking stuff about the dice collection object?
                        //can't rely on any given set of attributes not being what someone wants, give it a 'loaded successfully' attribute?

                    if (temp2.Substring(0, 1) == "\"" || temp2.Substring(0, 1) == "{")
                    {
                        //Assume this is a serialized Ability object.
                        DiceCollection? aaa = JsonSerializer.Deserialize<DiceCollection>(temp2);
                        if (aaa == null)
                        {
                            //We can't parse this.
                            Dice = new DiceCollection();
                        }
                        else
                        {
                            Dice = aaa;
                        }
                    } else
                    {
                        //Assume this is a dice string and treat it as such.
                        DiceCollection aaa = new DiceCollection(temp2);
                        Dice =  aaa;
                    }
                }

                if (root.TryGetProperty("DisplayTier", out temp))
                {
                    if (Int32.TryParse(temp.ToString(), out tempint))
                    {
                        DisplayTier = tempint;
                    }
                }
            }
            catch
            {
                //Complain to a log file?
            }
        }


        //Functions relating to triggering the ability:
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Triggers this ability.
        public DiceResponse Use (Dictionary<string, int> stats)
        {
            //actions:
            //roll - put the total in the log
            //heal - increase current hp, to a max of max hp
            //temphp - set temp hp to the roll, unless it's already higher
            //counter - count up/down
            //add a variable to store a counter, so I don't have to use current uses?
            //potentially add one for recharging/whatever - increase/decrease current uses by the number rolled.
            //do i need a special case for when there's no action?

            //to do: add an option for putting in a log message with the die roll, but nothing else.

            //to do: put in some way to do 'one of these plus also decrement uses.'
            //just put in a 'UsesChange' attribute or somesuch?
            //ignore this / set it to 0 when the action is counter
            //How feasible would it be to allow multiple different types of options operating off the same pool of uses / charges / whatever?
            //Would it be easier to link multiple abilities to the same pool, or to have multiple entries for the same ability?
            //If shared pools, I could just have names for some and stick a List of them on the character.

            //string logString = name+": ";

            //First we'll mess with the remaining uses.
            //Let's store how many we use in case that's relevant at some point.
            int usesUsed;
            if (UsesChange == 0)
            {
                //We're done with this part, that was easy.
                usesUsed = 0;
            }
            else if (UsesChange < 0)
            {
                //Attempt to reduce the number of uses remaining.
                if (Uses + UsesChange < 0)
                {
                    //There aren't enough uses left to use this ability.
                    return new DiceResponse(false, "Not enough uses for " + Name);
                    usesUsed = Uses;
                    Uses = 0;
                } else
                {
                    Uses += UsesChange;
                    usesUsed = UsesChange;
                }
            } else if (UsesChange > 0)
            {
                //Attempt to increase the number of uses / charges.
                if (Uses + UsesChange > MaxUses)
                {
                    usesUsed = MaxUses - Uses;
                    Uses = MaxUses;
                } else
                {
                    Uses += UsesChange;
                    usesUsed = UsesChange;
                }
            }

            if (Action.ToLower() == "false")
            {
                //There's nothing to do, therefore we're already done.
                //We'll send back a log message saying the ability was used.

                //logString += "used";
                //return new DiceResponse(true, logString);
                return new DiceResponse(true, Name + ": " + " used");
            }

            DiceResponse resp = RollDice(stats);

            switch (Action)
            {
                case "roll":
                    //We just do the roll and let it get logged.
                    break;
                case "counter":
                    //We add the result of the roll to the remaining uses.
                    //to do: consider splitting counters off to another variable, and adding "recharge" or something as an action.
                    Uses += resp.Total;
                    if (Uses > MaxUses)
                    {
                        Uses = MaxUses;
                    }
                    break;
                case "heal":
                    int newHP = stats["CurrentHP"] + resp.Total;
                    if (newHP > stats["MaxHP"])
                    {
                        resp.Changes["CurrentHP"] = stats["MaxHP"].ToString();
                    }
                    else if (newHP < 0)
                    {
                        resp.Changes["CurrentHP"] = "0";
                    }
                    else
                    {
                        resp.Changes["CurrentHP"] = (stats["CurrentHP"] + resp.Total).ToString();
                    }
                    //to do: should changes actually be string,int?
                        //will i ever actually change a non-int variable with this?
                        //abilities can change themselves now, so nbd there.
                    //to do: trigger a reload after changes are made.
                        //or maybe set up some way to indicate that that's necessary.
                            //another bool in DiceResponse?
                    break;
                case "temphp":
                    resp.Changes["TempHP"] = resp.Total.ToString();
                    //to do: check if there's already more
                    break;
                default:
                    //There's apparently nothing to do, so we're done.
                    return new DiceResponse(true);
            }

            //resp.description = name + "     " + resp.description;

            return resp;


            //DiceResponse respaaa = rollDice();
            //switch (this.action)
            //{
            //    case "roll":
            //        return justRoll();
            //    case "counter":
            //        //write a function for this
            //            //include the new total in the description?
            //        DiceResponse resp = justRoll();
            //        if (resp.success == true)
            //        {
            //            uses += resp.total;
            //        }
            //        else
            //        {
            //            //Do nothing?
            //        }
            //        return resp;
            //        
            //    case "heal":
            //        return heal();
            //    default:
            //        //complain to a log file or something
            //        return new DiceResponse (false, "Unsupported action: " + action);
            //}
        }

        //For when this ability triggers a simple roll with no additional effects.
        //private DiceResponse justRoll ()
        //{
        //    foreach (DiceCollection d in this.dice)
        //    {
        //        DiceResponse resp = d.roll();
        //        if (resp.success == false)
        //        {
        //            //something
        //        }
        //        //total += d.getTotal();
        //        total += resp.total;
        //        //description+= d.getDescription();
        //        description += resp.description;
        //    }
        //
        //    return new DiceResponse (true, total, description);
        //    //idea: change the returned boolean in these from success/failure to indicating whether or not there is a description to display.
        //}

        private DiceResponse RollDice(Dictionary<string, int> stats)
        {
            Total = 0;
            Description = Name+": ";
            Changes = new Dictionary<string, string>();

                DiceResponse resp = Dice.Roll(stats);
                if (resp.Success == false)
                {
                    //to do: decide what I want to do here.
                    Description += "error rolling dice";
                }
                else
                {
                    Total += resp.Total;
                    Description += resp.Description;
                }

            return new DiceResponse(true, Total, Description);
        }

        private DiceResponse Heal ()
        {
            //Name and ID must be changes for this to work.
            //changes = new Character("changes", "changes");
            Changes = new Dictionary<string, string> ();
            int totalHealed = 0;

                DiceResponse resp = Dice.Roll();
                Total += resp.Total;
                Description += resp.Description;

                totalHealed += Total;

            Changes["CurrentHP"] = totalHealed.ToString();

            //return new DiceResponse(true, total, description, changes);
            //still deciding how to handle changes in DiceResponse, add this when I have that nailed down.
            return new DiceResponse(true, Total, Description);
        }

        //Other public functions
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Changes the number of uses remaining, within the bounds of 0 -> maximum.
        public bool ChangeUses (int change)
        {
            if (change == 0)
            {
                return true;
            }
            else if (change < 0)
            {
                //We are reducing the number of uses remaining, 0 is our limit.
                if (Uses + change >= 0)
                {
                    //We can make this change.
                    Uses += change;
                    return true;
                } else
                {
                    //We can make some of this change.
                    Uses = 0;
                    return true;
                }
            } else if (change > 0)
            {
                //We are increasing the number of uses remaining, MaxUses is our limit.
                if (Uses + change <= MaxUses)
                {
                    //We can make this change.
                    Uses += change;
                    return true;
                } else
                {
                    //We can make some of this change.
                    Uses = MaxUses;
                    return true;
                }
            }

            //We shouldn't be able to get here, but Visual Studio seems to think we can.
            //I'll include this to shut it up.
            return false;
        }

        //Used for sorting.
        //Arguments:
            //other: another Ability object to compare against.
            //column: which attribute of the Ability class should be used to compare the two.
            //reverse: used to indicate whether the thing in question is being sorted in ascending or descending order.
                //This is so that Display Tiers can be sorted to the top / wherever as appropriate.
            //DisplayTierPriority: disable this to make Display Tiers no longer override other sorting.
        //Return values:
            //If this is to appear before the other one, returns -1.
            //If this is to appear after the other one, returns 1.
            //If this is equal to the other one, including with tiebreakers, returns 0.
        //Ties are broken by Name.
        public int Compare (Ability other, string column = "DisplayTier", bool reverse = false, bool DisplayTierPriority = true)
        {
            column = column.ToLower();
            int comparison;

            if (DisplayTierPriority)
            {
                //We'll check display tiers to see if we can cut out early.
                int temp = 0;
                if (DisplayTier < other.DisplayTier)
                {
                    temp = -1;
                } else if (DisplayTier > other.DisplayTier)
                {
                    temp = 1;
                }

                //If this is being sorted in reverse order we must manually reverse the display tiers.
                if (reverse && temp != 0)
                {
                    return temp * -1;
                }
                else if (temp != 0)
                {
                    return temp;
                }
            }

            //If we made it this far, these two abilities are the same display tier.
            //We're going to have to actually compare them now.
            //Note that the reverse argument is no longer relevant here.
            switch (column)
            {
                case "":
                    break;
                case "id":
                    comparison = String.Compare(this.ID, other.ID, StringComparison.OrdinalIgnoreCase);
                    if (comparison < 0)
                    {
                        return -1;
                    }
                    else if (comparison > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        //We need to do more comparisons.
                        break;
                    }
                case "name":
                    comparison = String.Compare(this.Name, other.Name, StringComparison.OrdinalIgnoreCase);
                    if (comparison < 0)
                    {
                        return -1;
                    }
                    else if (comparison > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        //We need to do more comparisons.
                        break;
                    }
                case "action":
                    comparison = String.Compare(this.Action, other.Action, StringComparison.OrdinalIgnoreCase);
                    if (comparison < 0)
                    {
                        return -1;
                    }
                    else if (comparison > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        //We need to do more comparisons.
                        break;
                    }
                case "uses":
                    if (Uses < other.Uses)
                    {
                        return -1;
                    } else if (Uses > other.Uses)
                    {
                        return 1;
                    } else
                    {
                        //We need to do more comparisons.
                        break;
                    }
                case "maxuses":
                    if (MaxUses < other.MaxUses)
                    {
                        return -1;
                    }
                    else if (MaxUses > other.MaxUses)
                    {
                        return 1;
                    }
                    else
                    {
                        //We need to do more comparisons.
                        break;
                    }
                case "useschange":
                    if (UsesChange < other.UsesChange)
                    {
                        return -1;
                    }
                    else if (UsesChange > other.UsesChange)
                    {
                        return 1;
                    }
                    else
                    {
                        //We need to do more comparisons.
                        break;
                    }
                case "usescol":
                    //Used for the Uses column on the Abilities list.
                    //Start with current uses.
                    if (Uses < other.Uses)
                    {
                        return -1;
                    }
                    else if (Uses > other.Uses)
                    {
                        return 1;
                    }
                    //Continue with max uses.
                    if (MaxUses < other.MaxUses)
                    {
                        return -1;
                    }
                    else if (MaxUses > other.MaxUses)
                    {
                        return 1;
                    }
                    else
                    {
                        //We need to do more comparisons.
                        break;
                    }
                case "text":
                    comparison = String.Compare(this.Text, other.Text, StringComparison.OrdinalIgnoreCase);
                    if (comparison < 0)
                    {
                        return -1;
                    }
                    else if (comparison > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        //We need to do more comparisons.
                        break;
                    }
                case "dice":
                    comparison = String.Compare(this.Dice.ToString(), other.Dice.ToString(), StringComparison.OrdinalIgnoreCase);
                    if (comparison < 0)
                    {
                        return -1;
                    }
                    else if (comparison > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        //We need to do more comparisons.
                        break;
                    }
                case "rechargecondition":
                    comparison = String.Compare(this.RechargeCondition, other.RechargeCondition, StringComparison.OrdinalIgnoreCase);
                    if (comparison < 0)
                    {
                        return -1;
                    }
                    else if (comparison > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        //We need to do more comparisons.
                        break;
                    }
                case "description":
                    comparison = String.Compare(this.Description, other.Description, StringComparison.OrdinalIgnoreCase);
                    if (comparison < 0)
                    {
                        return -1;
                    }
                    else if (comparison > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        //We need to do more comparisons.
                        break;
                    }
                case "displaytier":
                    if (DisplayTier < other.DisplayTier)
                    {
                        return -1;
                    }
                    else if (DisplayTier > other.DisplayTier)
                    {
                        return 1;
                    }
                    else
                    {
                        //We need to do more comparisons.
                        break;
                    }
                default:
                    break;
            }

            //Break ties by name.
            comparison = String.Compare(this.Name, other.Name, StringComparison.OrdinalIgnoreCase);
            if (comparison < 0)
            {
                return -1;
            }
            else if (comparison > 0)
            {
                return 1;
            }

            //add more tiebreakers here
                //dice string (standard string sort)?
                //current uses?
                //max uses?
                    //before current uses?

            //If we get here, we weren't able to see a difference between these two.
            return 0;
        }

        //Recharges the ability or not based on the specified event.
        public bool MaybeRecharge (string renameme)
        {
            if (ShouldRecharge (renameme))
            {
                return Recharge();
            }

            return false;
        }
        
        //Recharges the ability.
        //to do: add more complexity than just 'all' for amount to recharge.
        public bool Recharge()
        {
            Uses = MaxUses;
            return true;
        }

        //Returns true if the ability should be recharged under the specified event, or false otherwise.
        public bool ShouldRecharge (string renameme) {
            renameme = renameme.ToLower();

            if (renameme == RechargeCondition.ToLower())
            {
                return true;
            } else if (renameme == "long rest" && RechargeCondition.ToLower() == "short rest")
            {
                return true;
            }

            return false;
        }

        //Getters and setters:
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        public string getDiceString()
        {
            //We're going to temporarily just list the first dice string.
            return Dice.GetDiceString();
        }

        public string getUsesString ()
        {
            return Uses.ToString() + " / " + MaxUses.ToString();
        }
        
    }
}
