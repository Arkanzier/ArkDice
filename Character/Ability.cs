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
        //Behind the scenes identifier.
        public string ID { get; set; }

        //Name that's displayed to the user.
        public string Name { get; set; }

        //What gets done when this is triggered.
        public string Action { get; set; }
        
        //The dice / whatever that gets rolled when this is triggered.
        public DiceCollection Dice { get; set; }

        //Number of currently available uses / charges / whatever.
        public int Uses{ get; set; }

        //Maximum number of uses / charges / whatever.
        public int MaxUses { get; set; }

        //How the uses change with each activation.
        public int UsesChange { get; set; }

        //These three are used by the program to temporarily store values that get generated.
        [JsonIgnore]
        public int Total { get; set; }
        [JsonIgnore]
        public string Description { get; set; }
        [JsonIgnore]
        public Dictionary<string, string> Changes { get; set; }

        //The condition for when this ability recharges it's uses.
        public string RechargeCondition{ get; set; }

        //How many uses this ability recharges each time it does so.
        //Generally expected to be a dice string.
            //Can also be "all" to recharge fully.
            //Can also be "false" or "none" to not recharge automatically.
        public string RechargeAmount { get; set; }

        //The text displayed to the user to explain what this ability does.
        public string Text{ get; set; }

        //Used to group abilities together when displayed.
        //Lower-numbered tiers will be displayed first.
        public int DisplayTier { get; set; }


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
            RechargeAmount = "all";

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
                if (root.TryGetProperty("RechargeAmount", out temp))
                {
                    RechargeAmount = temp.ToString();
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
        /*
            Possible actions:
                false - do nothing
                roll - roll some dice (as appropriate) and put a thing in the log.
                counter - adds the result of the roll to the remaining uses.
                heal - heals the current character based on the roll.
                temphp - gives the current character temp HP based on the roll.
        */
        public DiceResponse Use (Dictionary<string, double> stats)
        {
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
                    //usesUsed = Uses;
                    //Uses = 0;
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

                return new DiceResponse(true, Name + ": " + " used");
            }

            DiceResponse resp = RollDice(stats);

            string lowerAction = Action.ToLower();
            switch (lowerAction)
            {
                case "roll":
                    //We just do the roll and let it get logged.
                    break;
                case "counter":
                    //We add the result of the roll to the remaining uses.
                    Uses += resp.Total;
                    if (Uses > MaxUses)
                    {
                        Uses = MaxUses;
                    }
                    break;
                case "heal":
                    int newHP = (int)stats["CurrentHP"] + resp.Total;
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
                    break;
                case "temphp":
                    resp.Changes["TempHP"] = resp.Total.ToString();
                    break;
                default:
                    //There's apparently nothing to do, so we're done.
                    return new DiceResponse(true);
            }

            return resp;
        }

        //Triggers the appropriate dice roll(s) for an ability activating.
        private DiceResponse RollDice(Dictionary<string, double> stats)
        {
            Total = 0;
            Description = Name+": ";
            Changes = new Dictionary<string, string>();

                DiceResponse resp = Dice.Roll(stats);
                if (resp.Success == false)
                {
                    //Complain to a log file?
                    Description += "error rolling dice";
                }
                else
                {
                    Total += resp.Total;
                    Description += resp.Description;
                }

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
                Recharge();
                return true;
            }

            return false;
        }
        
        //Recharges the ability.
        public void Recharge()
        {
            if (RechargeAmount.ToLower() == "all")
            {
                //Full recharge.
                Uses = MaxUses;
            }
            else if (RechargeAmount.ToLower() == "false" || RechargeAmount.ToLower() == "none")
            {
                //No recharge.
            } else
            {
                //Roll dice to determine recharge amount.
                DiceCollection dc = new DiceCollection (RechargeAmount);
                DiceResponse resp = dc.Roll();
                if (resp.Total >= 0)
                {
                    //We're increasing the number of uses.
                    if (Uses + resp.Total <= MaxUses)
                    {
                        Uses += resp.Total;
                    }
                    else
                    {
                        Uses = MaxUses;
                    }
                } else
                {
                    //We're decreasing the number of uses.
                    if (Uses + resp.Total >= 0)
                    {
                        Uses += resp.Total;
                    } else
                    {
                        Uses = 0;
                    }
                }
            }
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
            return Dice.GetDiceString();
        }

        public string getUsesString ()
        {
            return Uses.ToString() + " / " + MaxUses.ToString();
        }
        
    }
}
