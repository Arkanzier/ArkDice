using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Schema;
using ArkDice;

namespace Character
{
    public class Ability
    {
        public string id { get; private set; }
        public string name { get; private set; }

        public string action { get; private set; }
        public List<DiceCollection> dice { get; private set; }

        public int uses{ get; private set; }
            //if max uses < 0, unlimited uses?
                //require that current uses = 0?
        public int maxUses { get; private set; }
        public int counter { get; private set; }

        //These two are used by the program to temporarily store values that get generated.
        public int total { get; private set; }
        public string description { get; private set; }
        public Dictionary<string, string> changes { get; private set; }
        //private something changes;
        //create a Character class instance and add a function to add two together?

        public string recharge{ get; private set; }
        //consider adding a rechargeAmount variable.
            //Determines how much the thing recharges, when it recharges.
            //-1 for all? Just do 9999?
        public string text{ get; private set; }

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
        public Ability(string id, string text)
        {
            this.id = id;
            this.action = "";
            this.name = "";

            this.uses = 0;
            this.maxUses = 0;
            this.counter = 0;

            this.text = text;
            this.dice = new List<DiceCollection>();
            this.recharge = "";

            this.total = 0;
            this.description = "";
            this.changes = new Dictionary<string, string>();
        }

        public Ability(string json)
        {
            //Start with things zeroed out.
            this.id = "";
            this.name = "";
            this.text = "";
            this.dice = new List<DiceCollection>();
            this.action = "";
            this.recharge = "";
            this.uses = 0;
            this.maxUses = 0;
            this.counter = 0;

            this.total = 0;
            this.description = "";
            this.changes = new Dictionary<string, string>();

            try
            {
                JsonDocument doc = JsonDocument.Parse(json);
                JsonElement root = doc.RootElement;

                JsonElement temp = new JsonElement();
                int tempint = 0;

                if (root.TryGetProperty("id", out temp))
                {
                    id = temp.ToString();
                }
                if (root.TryGetProperty("name", out temp))
                {
                    name = temp.ToString();
                }
                if (root.TryGetProperty("text", out temp))
                {
                    text = temp.ToString();
                }
                if (root.TryGetProperty("recharges", out temp))
                {
                    recharge = temp.ToString();
                }
                if (root.TryGetProperty("action", out temp))
                {
                    action = temp.ToString();
                }

                if (root.TryGetProperty("maxUses", out temp))
                {
                    if (Int32.TryParse(temp.ToString(), out tempint))
                    {
                        maxUses = tempint;
                    }
                }
                if (root.TryGetProperty("currentUses", out temp))
                {
                    if (Int32.TryParse(temp.ToString(), out tempint))
                    {
                        uses = tempint;
                    }
                }
                if (root.TryGetProperty("counter", out temp))
                {
                    if (Int32.TryParse(temp.ToString(), out tempint))
                    {
                        counter = tempint;
                    }
                }

                if (root.TryGetProperty("dice", out temp))
                {
                    string temp2 = temp.ToString();
                    DiceCollection aaa = new DiceCollection(temp2);
                    dice = new List<DiceCollection> { aaa };
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
        public DiceResponse use (Dictionary<string, int> stats)
        {
            //to do: make sure this can properly pass along the stats list.
            //general structure:
                //always roll?
                //then apply the total/etc to the specified action.
            //to do: should using this decrement it's uses remaining?
                //only do this for some actions.
                    //not counter, for example.

            //actions:
                //just roll - put the total in the log
                //heal - increase current hp, to a max of max hp
                //temphp - set temp hp to the roll, unless it's already higher
                //counter - count up/down
                    //add a variable to store a counter, so I don't have to use current uses?
                //potentially add one for recharging/whatever - increase/decrease current uses by the number rolled.
                //do i need a special case for when there's no action?

            //string logString = name+": ";

            if (action.ToLower() == "false")
            {
                //There's nothing to do, therefore we're already done.
                //We'll list the ability as having been used.

                //logString += "used";
                //return new DiceResponse(true, logString);
                return new DiceResponse(true, name + ": " + "used");
            }

            DiceResponse resp = rollDice(stats);

            switch (action)
            {
                case "roll":
                    //We just do the roll and let it get logged.
                    break;
                case "counter":
                    //We add the result of the roll to the remaining uses.
                    //to do: consider splitting counters off to another variable, and adding "recharge" or something as an action.
                    uses += resp.total;
                    if (uses > maxUses)
                    {
                        uses = maxUses;
                    }
                    break;
                case "heal":
                    int newHP = stats["health"] + resp.total;
                    if (newHP > stats["maxHP"])
                    {
                        resp.changes["health"] = stats["maxHP"].ToString();
                    }
                    else if (newHP < 0)
                    {
                        resp.changes["health"] = "0";
                    }
                    else
                    {
                        resp.changes["health"] = (stats["health"] + resp.total).ToString();
                    }
                    //to do: check to make sure these indices actually exist.
                    //to do: should changes actually be string,int?
                        //will i ever actually change a non-int variable with this?
                        //abilities can change themselves now, so nbd there.
                    //to do: trigger a reload after changes are made.
                        //or maybe set up some way to indicate that that's necessary.
                            //another bool in DiceResponse?
                    break;
                case "temphp":
                    resp.changes["temphp"] = resp.total.ToString();
                    //to do: check if there's already more
                    break;
                default:
                    //There's apparently nothing to do, so we're done.
                    return new DiceResponse(true);
            }

            //MessageBox.Show(resp.description);

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

        private DiceResponse rollDice(Dictionary<string, int> stats)
        {
            total = 0;
            description = name+": ";
            changes = new Dictionary<string, string>();

            foreach (DiceCollection d in this.dice)
            {
                DiceResponse resp = d.roll(stats);
                if (resp.success == false)
                {
                    //to do: decide what I want to do here.
                    description += "error rolling dice";
                }
                else
                {
                    total += resp.total;
                    description += resp.description;
                }
            }

            return new DiceResponse(true, total, description);
        }

        private DiceResponse heal ()
        {
            //Name and ID must be changes for this to work.
            //changes = new Character("changes", "changes");
            changes = new Dictionary<string, string> ();
            int totalHealed = 0;

            foreach (DiceCollection d in this.dice)
            {
                DiceResponse resp = d.roll();
                total += resp.total;
                description += resp.description;

                totalHealed += total;
            }

            changes["currentHP"] = totalHealed.ToString();

            //return new DiceResponse(true, total, description, changes);
            //still deciding how to handle changes in DiceResponse, add this when I have that nailed down.
            return new DiceResponse(true, total, description);
        }


        //Getters and setters:
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        public string getID ()
        {
            return id;
        }
        public string getName()
        {
            return name;
        }
        public string getText()
        {
            return text;
        }
        public int getTotal()
        {
            return total;
        }
        public string getDescription()
        {
            return description;
        }
        public string getRecharge()
        {
            return recharge;
        }

        public string getDiceString()
        {
            //We're going to temporarily just list the first dice string.
            if (dice.Count > 0)
            {
                return dice[0].getDiceString();
            } else
            {
                return "none";
            }
        }

        public string getUsesString ()
        {
            return uses.ToString() + " / " + maxUses.ToString();
        }
    }
}
