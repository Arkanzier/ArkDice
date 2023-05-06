﻿using System.Collections.Immutable;
using System.ComponentModel.Design;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Xml.Schema;
using ArkDice;

namespace Character //change to ArkDice?
{
    public class Character
    {
        //Basic info
        public string ID { get; private set; }
        public string Name { get; private set; }
        public string Race { get; private set; }
        public string Subrace { get; private set; }
        public int MaxHP { get; private set; }
        public int CurrentHP { get; private set; }
        public int TempHP { get; private set; }
        //temporary modifiers to max HP?

        //Class related info
        public List<ClassLevel> Classes { get; private set; }
        //to do: consider setting something up to support gestalt rules.
        //just set up a multiplier for levels or 'class levels per character level' type thing?
        //the character's level equals total class level divided by that number

        //Added to the prof bonus calculated based on level. Can be negative.
        public int BonusToProf { get; private set; }

        //Automatically managed. Stores prof bonus by level + profBonus.
        public int Prof { get; private set; }

        //Stats and related info
        //Starts at 0 and proceeds in the order Str, Dex, Con, Int, Wis, Cha.
        public int[] Stats { get; private set; }
        //convert these over to loose ints/bools?
        //there are only 6 of each, and it would cut down a little on the work involved.

        public int[] Saves { get; private set; }
        //to do: rename to saves?

        //how to handle weapon and armor profs?
        //armor can probably just be 3 bools/decimals
        //weapons can almost just be a couple bools/floats, but we might need to address each weapon individually
        //do something with hardcoded categories?
        //write a function or two to convert between weapon categories and specific weapons?
        //how to handle language profs?
        //just make them bools, or build in houserules for learning them fractionally?
        //need something for tool profs
        //just put in an 'other' since there are so many types of tools?
        //this would just be an array of strings, in that case.
        //lump tools and languages together, since they're just going to be lists anyway?

        //Skill proficiencies
        public Dictionary<string, int> Skills { get; private set; }

        //Skills by stat:
        /*
         *  Strength:
         *      Athletics
         *  Dexterity:
         *      Acrobatics
         *      Sleight of Hand
         *      Stealth
         *   Constitution:
         *      none
         *   Intelligence:
         *      Arcana
         *      History
         *      Investigation
         *      Nature
         *      Religion
         *   Wisdom:
         *      Animal Handling
         *      Insight
         *      Medicine
         *      Perception
         *      Survival
         *   Charisma:
         *      Deception
         *      Intimidation
         *      Performance
         *      Persuasion
         */

        //Abilities and related info.
        public List<Ability> Abilities { get; private set; }
        //switch from strings to objects?
        public List<Ability> BasicAbilities { get; private set; }
        //Text describing passive abilities the character has.
        public List<string> Passives { get; private set; }

        public List<Spell> Spells { get; private set; }

        //Constructor(s):
        //-------- -------- -------- -------- -------- -------- -------- -------- 

        //Creates a blank character.
        public Character()
        {
            //Basic info.
            ID = "";
            Name = "";
            Race = "";
            Subrace = "";
            MaxHP = 0;
            CurrentHP = 0;
            TempHP = 0;

            //Class and level based info.
            Classes = new List<ClassLevel>();
            BonusToProf = 0;

            //Stats and related info.
            Stats = new int[6];
            Saves = new int[6];
            for (int a = 0; a < 6; a++)
            {
                Stats[a] = 0;
                Saves[a] = 0;
            }
            Skills = new Dictionary<string, int>();
            Skills["Athletics"] = 0;
            Skills["Acrobatics"] = 0;
            Skills["Sleight of Hand"] = 0;
            Skills["Stealth"] = 0;
            Skills["Arcana"] = 0;
            Skills["History"] = 0;
            Skills["Investigation"] = 0;
            Skills["Nature"] = 0;
            Skills["Religion"] = 0;
            Skills["Animal Handling"] = 0;
            Skills["Insight"] = 0;
            Skills["Medicine"] = 0;
            Skills["Perception"] = 0;
            Skills["Survival"] = 0;
            Skills["Deception"] = 0;
            Skills["Intimidation"] = 0;
            Skills["Performance"] = 0;
            Skills["Persuasion"] = 0;

            //Ability info
            Abilities = new List<Ability>();
            BasicAbilities = new List<Ability>();
            Passives = new List<string>();
            Spells = new List<Spell>();

            CalculateProf();
        }

        //Loads the character from a file containing JSON.
        public Character (string charID, Dictionary<string, Spell> library, string folderpath = "")
            : this()
        {
            if (folderpath != "" && folderpath.Last() != '\\')
            {
                folderpath += "\\";
            }
            string filepath = folderpath + "Characters\\" + charID + ".char";

            if (!File.Exists (filepath))
            {
                //Complain to a log file.
                return;
            }

            string json = File.ReadAllText (filepath);

            if (json == "" || json == null)
            {
                //Complain to a log file.
                return;
            }

            //to do: look into making this work with deserialize.
            //will i have to make a struct with the same attributes and then write a function to copy stuff from there to here?

            //Temporarily removed.
            //Doesn't work, I'm guessing it requires public setters to work but I don't want those.
            
            /*
            try
            {
                var deserializerOptions = new JsonSerializerOptions
                {
                    NumberHandling = JsonNumberHandling.AllowReadingFromString
                };
                //Character? copy = JsonSerializer.Deserialize<Character>(json);
                CharacterCopy? copy = JsonSerializer.Deserialize<CharacterCopy> (json);
                //CharacterCopy? copy = JsonSerializer.Deserialize<CharacterCopy> (json, deserializerOptions);

                if (copy == null)
                {
                    //is this even possible?
                    //complain to a log
                    return;
                }

                //copy copy's stuff to here.
                //do some error checking first?

                ID = copy.ID;
                Name = copy.Name;
                Race = copy.Race;
                MaxHP = copy.MaxHP;
                CurrentHP = copy.CurrentHP;
                Classes = copy.Classes;
                Prof = copy.Prof;
                BonusToProf = copy.BonusToProf;
                Stats = copy.Stats;
                Saves = copy.Saves;
                Skills = copy.Skills;
                Abilities = copy.Abilities;
                BasicAbilities = copy.BasicAbilities;
                Passives = copy.Passives;
                return;
            }
            catch
            {
                //complain to a log?
                Name = "Error";
                return;
            }
            */
            

            //Pull info from the JSON.
            try
            {
                JsonDocument doc = JsonDocument.Parse(json);
                JsonElement root = doc.RootElement;

                JsonElement temp = new JsonElement();
                int tempint = 0;

                //Basic attributes
                if (root.TryGetProperty("ID", out temp))
                {
                    ID = temp.ToString();
                }
                if (root.TryGetProperty("Name", out temp))
                {
                    Name = temp.ToString();
                }
                if (root.TryGetProperty("Race", out temp))
                {
                    Race = temp.ToString();
                }
                if (root.TryGetProperty("Subrace", out temp))
                {
                    Subrace = temp.ToString();
                }
                if (root.TryGetProperty("MaxHP", out temp))
                {
                    if (Int32.TryParse(temp.ToString(), out tempint))
                    {
                        MaxHP = tempint;
                    }
                }
                if (root.TryGetProperty("CurrentHP", out temp))
                {
                    if (Int32.TryParse(temp.ToString(), out tempint))
                    {
                        CurrentHP = tempint;
                    }
                }
                if (root.TryGetProperty("TempHP", out temp))
                {
                    if (Int32.TryParse(temp.ToString(), out tempint))
                    {
                        TempHP = tempint;
                    }
                }
                if (root.TryGetProperty("BonusToProf", out temp))
                {
                    //not currently in the file
                    if (Int32.TryParse(temp.ToString(), out tempint))
                    {
                        BonusToProf = tempint;
                    }
                }

                //Classes and levels
                if (root.TryGetProperty("Classes", out temp))
                {
                    Classes = new List<ClassLevel>();
                    JsonElement temp2 = new JsonElement();

                    for (int a = 0; a < temp.GetArrayLength(); a++)
                    {
                        JsonElement thisclass = temp[a];
                        ClassLevel newclass = new ClassLevel();

                        newclass.Name = "";
                        if (thisclass.TryGetProperty("Name", out temp2))
                        {
                            newclass.Name = temp2.ToString();
                        }
                        else
                        {
                            //complain to a log file?
                            continue;
                        }
                        newclass.Subclass = "";
                        if (thisclass.TryGetProperty("Subclass", out temp2))
                        {
                            newclass.Subclass = temp2.ToString();
                        }
                        //Subclass is not required, so we don't quit when it's not present.
                        newclass.Level = 0;
                        if (thisclass.TryGetProperty("Level", out temp2))
                        {
                            if (Int32.TryParse(temp2.ToString(), out tempint))
                            {
                                newclass.Level = tempint;
                            }
                        }
                        else
                        {
                            //complain to a log file?
                            continue;
                        }
                        newclass.HDSize = 0;
                        if (thisclass.TryGetProperty("HDSize", out temp2))
                        {
                            if (Int32.TryParse(temp2.ToString(), out tempint))
                            {
                                newclass.HDSize = tempint;
                            }
                        }
                        newclass.CurrentHD = 0;
                        if (thisclass.TryGetProperty("CurrentHD", out temp2))
                        {
                            if (Int32.TryParse(temp2.ToString(), out tempint))
                            {
                                newclass.CurrentHD = tempint;
                            }
                        }

                        Classes.Add(newclass);
                    }
                }

                //Stats
                //to do: this is now a purely numeric array, adjust this to compensate
                if (root.TryGetProperty("Stats", out temp))
                {
                    //We don't technically need to do this here, but it won't hurt.
                    Stats = new int[6];
                    //JsonElement temp2 = new JsonElement();

                    //to do: check if array length is 6 and complain otherwise?
                    for (int a = 0; a < temp.GetArrayLength() && a < 6; a++)
                    {
                        JsonElement thisstat = temp[a];

                        if (thisstat.TryGetInt32(out tempint))
                        {
                            Stats[a] = tempint;
                        }
                        else
                        {
                            Stats[a] = 0;
                            //complain to a log?
                        }
                    }
                }

                //Saves
                if (root.TryGetProperty("Saves", out temp))
                {
                    //We don't technically need to do this here, but it won't hurt.
                    Saves = new int[6];
                    //JsonElement temp2 = new JsonElement();

                    //to do: check if array length is 6 and complain otherwise?
                    for (int a = 0; a < temp.GetArrayLength() && a < 6; a++)
                    {
                        JsonElement thissave = temp[a];

                        if (thissave.TryGetInt32(out tempint))
                        {
                            Saves[a] = tempint;
                        }
                        else
                        {
                            Saves[a] = 0;
                            //complain to a log?
                        }
                    }
                }

                //Skills
                //to do: set up a way to automatically get these indices.
                //just do a foreach through the existing dictionary, since they should all be in there now?
                if (root.TryGetProperty("Skills", out temp))
                {
                    Skills = new Dictionary<string, int>();
                    JsonElement temp2 = new JsonElement();

                    Skills["Athletics"] = 0;
                    if (temp.TryGetProperty("Athletics", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            Skills["Athletics"] = tempint;
                        }
                    }
                    Skills["Acrobatics"] = 0;
                    if (temp.TryGetProperty("Acrobatics", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            Skills["Acrobatics"] = tempint;
                        }
                        else
                        {
                            Skills["Acrobatics"] = 0;
                        }
                    }
                    Skills["Sleight of Hand"] = 0;
                    if (temp.TryGetProperty("Sleight of Hand", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            Skills["Sleight of Hand"] = tempint;
                        }
                    }
                    Skills["Stealth"] = 0;
                    if (temp.TryGetProperty("Stealth", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            Skills["Stealth"] = tempint;
                        }
                    }
                    Skills["Arcana"] = 0;
                    if (temp.TryGetProperty("Arcana", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            Skills["Arcana"] = tempint;
                        }
                    }
                    Skills["History"] = 0;
                    if (temp.TryGetProperty("History", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            Skills["History"] = tempint;
                        }
                    }
                    Skills["Investigation"] = 0;
                    if (temp.TryGetProperty("Investigation", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            Skills["Investigation"] = tempint;
                        }
                    }
                    Skills["Nature"] = 0;
                    if (temp.TryGetProperty("Nature", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            Skills["Nature"] = tempint;
                        }
                    }
                    Skills["Religion"] = 0;
                    if (temp.TryGetProperty("Religion", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            Skills["Religion"] = tempint;
                        }
                    }
                    Skills["Animal Handling"] = 0;
                    if (temp.TryGetProperty("Animal Handling", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            Skills["Animal Handling"] = tempint;
                        }
                    }
                    Skills["Insight"] = 0;
                    if (temp.TryGetProperty("Insight", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            Skills["Insight"] = tempint;
                        }
                    }
                    Skills["Medicine"] = 0;
                    if (temp.TryGetProperty("Medicine", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            Skills["Medicine"] = tempint;
                        }
                    }
                    Skills["Perception"] = 0;
                    if (temp.TryGetProperty("Perception", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            Skills["Perception"] = tempint;
                        }
                    }
                    Skills["Survival"] = 0;
                    if (temp.TryGetProperty("Survival", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            Skills["Survival"] = tempint;
                        }
                    }
                    Skills["Deception"] = 0;
                    if (temp.TryGetProperty("Deception", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            Skills["Deception"] = tempint;
                        }
                    }
                    Skills["Intimidation"] = 0;
                    if (temp.TryGetProperty("Intimidation", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            Skills["Intimidation"] = tempint;
                        }
                    }
                    Skills["Performance"] = 0;
                    if (temp.TryGetProperty("Performance", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            Skills["Performance"] = tempint;
                        }
                    }
                    Skills["Persuasion"] = 0;
                    if (temp.TryGetProperty("Persuasion", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            Skills["Persuasion"] = tempint;
                        }
                    }
                }

                //Abilities
                if (root.TryGetProperty("Abilities", out temp))
                {
                    //Contains an array of objects.
                    //Pass each object to the Ability class' constructor.

                    Abilities = new List<Ability>();

                    for (int a = 0; a < temp.GetArrayLength(); a++)
                    {
                        Ability thisAbility = new Ability(temp[a].ToString());
                        Abilities.Add(thisAbility);
                    }
                }

                //basic abilities goes here

                //passives goes here

                //Spells
                if (root.TryGetProperty ("Spells", out temp))
                {
                    Spells = new List<Spell>();

                    for (int a = 0; a < temp.GetArrayLength(); a++)
                    {
                        //Spell thisSpell = new Spell(temp[a].ToString());
                        Spell thisSpell = new Spell(temp[a]);

                        //We might need to load the spell from the library.
                        if (!thisSpell.IsLoaded() && thisSpell.ID != "")
                        {
                            //We got an ID but that's it. Let's try to load it by ID.
                            if (library.ContainsKey(thisSpell.ID))
                            {
                                thisSpell.Copy(library[thisSpell.ID]);
                                Spells.Add(thisSpell);
                            } else if (thisSpell.LoadFromFile(folderpath))
                            {
                                //We were able to load it from a file.
                                Spells.Add(thisSpell);
                            }
                        } else
                        {
                            Spells.Add(thisSpell);
                        }
                    }
                }

            }
            catch
            {
                //complain to a log file?
                //Name = "Error";
                return;
            }

            CalculateProf();
            SortAbilities();
            SortSpells();
        }

        


        //Public functions:
        //-------- -------- -------- -------- -------- -------- -------- -------- 

        //Add or remove HD to the specified class.
        public bool AddOrSubtractHDForClass (string className, int amount)
        {
            for (int a = 0; a < Classes.Count; a++)
            {
                if (Classes[a].Name == className)
                {
                    if (amount >= 0)
                    {
                        if (Classes[a].CurrentHD + amount <= Classes[a].Level)
                        {
                            //We can just add them.
                            Classes[a].CurrentHD += amount;
                        } else
                        {
                            //We'll set them to full and let some be wasted.
                            Classes[a].CurrentHD = Classes[a].Level;
                        }
                    } else
                    {
                        if (Classes[a].CurrentHD + amount >= 0)
                        {
                            //We can just subtract them.
                            //Remember that amount is negative, so we add.
                            Classes[a].CurrentHD += amount;
                        } else
                        {
                            //We don't have enough to spend.
                            return false;
                        }
                    }

                    return true;
                }
            }

            //If we got this far, that class isn't in the list.
            return false;
        }

        //Checks if there's a spell under the specified ID and, if so, adds it to Spells.
        public bool AddSpellByID (string id, Dictionary<string, Spell> library, string folderpath)
        {
            //This constructor does all the work for us, we'll just see if it works.
            Spell temp = new Spell(id, library, folderpath);
            if (temp.IsLoaded())
            {
                Spells.Add(temp);
                return true;
            }

            //Apparently it didn't work.
            return false;
        }

        //Changes the number of uses remaining of the specified ability, by index.
        public bool ChangeAbilityUses (int abilityIndex, int change)
        {
            if (abilityIndex >= Abilities.Count)
            {
                //Complain to a log file?
                return false;
            }
            Ability ability = Abilities[abilityIndex];

            return ability.ChangeUses(change);
        }

        //Changes the number of uses remaining of the specified ability, by ID.
        public bool ChangeAbilityUses (string abilityID, int change)
        {
            int index = GetAbilityIndexByID (abilityID);

            return ChangeAbilityUses(index, change);
        }

        //Deal damage to the character.
        public bool Damage(int amount, bool allowNegative = false)
        {
            //We need to account for temp HP.
            int totalHP = CurrentHP + TempHP;

            if (amount == 0)
            {
                //We're done.
                return true;
            }
            else if (amount < 0)
            {
                //We're not going to allow negative damage in this function. Use Heal().
                return false;
            } else
            {
                //Actually take damage.
                if (amount <= totalHP)
                {
                    //We're going to survive
                    if (amount <= TempHP)
                    {
                        TempHP -= amount;
                        return true;
                    } else
                    {
                        amount -= TempHP;
                        TempHP = 0;
                        CurrentHP -= amount;
                        return true;
                    }
                }
                else
                {
                    //We're going to die.
                    if (allowNegative)
                    {
                        amount -= TempHP;
                        TempHP = 0;
                        CurrentHP -= amount;
                        return true;
                    } else
                    {
                        TempHP = 0;
                        CurrentHP = 0;
                        return true;
                    }
                }
            }
        }

        //Returns an array of the available unspent HD.
        public int[] GetAvailableHD()
        {
            //We'll use a full array out to 12 for convenience.
            //ret[x] refers to HD of size dx.
            int[] ret = new int[13];

            for (int a = 0; a < Classes.Count; a++)
            {
                int dieSize = Classes[a].HDSize;
                int numDice = Classes[a].CurrentHD;
                ret[dieSize] += numDice;
            }

            return ret;
        }

        //Returns the number of HD available of the specified size.
        public int GetAvailableHD(int size)
        {
            int ret = 0;
            for (int a = 0; a < Classes.Count; a++)
            {
                if (Classes[a].HDSize == size)
                {
                    ret += Classes[a].CurrentHD;
                }
            }

            return ret;
        }

        //Returns the sum of the character's levels in each of their classes.
        public int GetCharacterLevel()
        {
            int ret = 0;

            foreach (var classEntry in Classes)
            {
                ret += classEntry.Level;
            }

            return ret;
        }

        //Returns a list of the character's basic numeric stats for use with abilities.
        public Dictionary<string, int> GetGeneralStatistics()
        {
            Dictionary<string, int> ret = new Dictionary<string, int>();

            //Stats and modifiers
            ret["strmod"] = DiceFunctions.getModifierForStat(Stats[0]);
            ret["dexmod"] = DiceFunctions.getModifierForStat(Stats[1]);
            ret["conmod"] = DiceFunctions.getModifierForStat(Stats[2]);
            ret["intmod"] = DiceFunctions.getModifierForStat(Stats[3]);
            ret["wismod"] = DiceFunctions.getModifierForStat(Stats[4]);
            ret["chamod"] = DiceFunctions.getModifierForStat(Stats[5]);

            ret["strscore"] = Stats[0];
            ret["dexscore"] = Stats[1];
            ret["conscore"] = Stats[2];
            ret["intscore"] = Stats[3];
            ret["wisscore"] = Stats[4];
            ret["chascore"] = Stats[5];

            //General info
            ret["prof"] = Prof;
            ret["level"] = GetCharacterLevel();
            //toss in current and max hp?

            //Save profs
            ret["strsave"] = Saves[0];
            ret["dexsave"] = Saves[1];
            ret["consave"] = Saves[2];
            ret["intsave"] = Saves[3];
            ret["wissave"] = Saves[4];
            ret["chasave"] = Saves[5];

            //Skill profs
            ret["athletics"] = Skills["Athletics"];
            ret["acrobatics"] = Skills["Acrobatics"];
            ret["sleightofhand"] = Skills["Sleight of Hand"];
            ret["stealth"] = Skills["Stealth"];
            ret["arcana"] = Skills["Arcana"];
            ret["history"] = Skills["History"];
            ret["investigation"] = Skills["Investigation"];
            ret["nature"] = Skills["Nature"];
            ret["religion"] = Skills["Religion"];
            ret["animalhandling"] = Skills["Animal Handling"];
            ret["insight"] = Skills["Insight"];
            ret["medicine"] = Skills["Medicine"];
            ret["perception"] = Skills["Perception"];
            ret["survival"] = Skills["Survival"];
            ret["deception"] = Skills["Deception"];
            ret["intimidation"] = Skills["Intimidation"];
            ret["performance"] = Skills["Performance"];
            ret["persuasion"] = Skills["Persuasion"];

            //other profs here

            return ret;
        }

        //Returns an array of the maximum HD of each size.
        public int[] GetMaximumHD()
        {
            //We'll use a full array out to 12 for convenience.
            //ret[x] refers to HD of size dx.
            int[] ret = new int[13];

            for (int a = 0; a < Classes.Count; a++)
            {
                int dieSize = Classes[a].HDSize;
                int numDice = Classes[a].Level;
                ret[dieSize] += numDice;
            }

            return ret;
        }

        //Heal the character
        public bool Heal(int amount)
        {
            if (amount + CurrentHP > MaxHP)
            {
                CurrentHP = MaxHP;
            } else
            {
                CurrentHP += amount;
            }

            return true;
        }

        //Replenishes abilities and, on a long rest, also replenishes HD.
        //to do: find a better name for this?
        public bool RechargeAbilities (string condition)
        {
            condition = condition.ToLower();
            if (condition == "short rest")
            {
                //do short rest stuff
            }
            else if (condition == "long rest")
            {
                //do short + long rest stuff
                RegainHD("half");
            }

            //Replenish abilities as appropriate.
            for (int a = 0; a < Abilities.Count; a++)
            {
                Abilities[a].MaybeRecharge(condition);
            }

            return true;
        }

        //Replenishes the character's spent HD.
        public bool RegainHD (string amount)
        {
            amount = amount.ToLower();

            if (amount == "half")
            {
                //Regain 1/2 of all available HD.
                int toRegain = Decimal.ToInt32(Math.Floor((decimal)GetCharacterLevel() / 2));

                int[] max = GetMaximumHD();
                int[] current = GetAvailableHD();

                for (int size = 12; size > 0; size--)
                {
                    if (max.Length - 1 < size || current.Length - 1 < size)
                    {
                        continue;
                    }
                    if (max[size] > current[size])
                    {
                        for (int a = 0; a < Classes.Count; a++)
                        {
                            if (Classes[a].HDSize == size)
                            {
                                int toAdd;
                                int missingHere = Classes[a].Level - Classes[a].CurrentHD;
                                if (missingHere >= toRegain)
                                {
                                    //We can add all.
                                    Classes[a].CurrentHD += toRegain;
                                } else
                                {
                                    //We can only add some.
                                    Classes[a].CurrentHD = Classes[a].Level;
                                    toRegain -= missingHere;
                                }
                            }
                        }
                    }
                }


                //need a decently fast way to skip to the largest hd sizes
                //write a function to get the max hd of each size, like the one that gets current hd?
                    //or modify that one with another mode.
            }

            return true;
        }

        //Makes a roll using the character's stats and proficiencies.
        public DiceResponse RollForCharacter(DiceCollection dc)
        {
            Dictionary<string, int> stats = this.GetGeneralStatistics();

            DiceResponse resp = dc.Roll(stats);

            return resp;
        }

        //Saves the character and all it's abilities to a file.
        public bool Save(string filepath)
        {
            string folderpath = "C:\\Users\\david\\Programs\\Simple Dice Roller\\";

            //First we'll move the old file, if there is one, to a folder just in case something goes wrong and we want it back.
            if (System.IO.File.Exists(filepath))
            {
                //Start by making sure the backups folder exists.
                string backupFolder = folderpath + "save backups\\";
                if (!System.IO.Directory.Exists(backupFolder))
                {
                    System.IO.Directory.CreateDirectory(backupFolder);
                }

                //Now move the old file.
                string backupFilepath = backupFolder + Name + "_" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".char";
                //to do: consider leaving the original filename intact and just adding the stuff to that.
                //there's probably a function or two that can split the base name and the extension for us.
                //or a regex would work.
                try
                {
                    System.IO.File.Move(filepath, backupFilepath);
                    //touch the file?
                }
                catch
                {
                    //Complain to a log file?
                    return false;
                }
            }   //If old version exists.

            //Now we write the new file into the specified spot.
            string characterString = this.ToString();

            File.WriteAllText(filepath, characterString);
            //returns void so we don't know if it says it succeeded.

            if (System.IO.File.Exists(filepath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Sets the user's temporary HP to some number.
        //If the onlyIncrease argument is true, this will only set the character's temp HP if the character doesn't already have more.
        //Otherwise, it will be willing to reduce the character's temp HP.
        public bool SetTempHP(int amount, bool onlyIncrease = true) {
            if (amount > TempHP)
            {
                TempHP = amount;
                return true;
            } else
            {
                if (onlyIncrease)
                {
                    return true;
                } else
                {
                    TempHP = amount;
                    return true;
                }
            }
        }

        //Spends one or more HD of the specified size.
        //to do: consider putting in an option to spend the largest size repeatedly until HP is full.
        //maybe make that a different function
        public bool SpendHDBySize(int size, int quantity = 1)
        {
            //Make sure there are enough HD of the specified size before we start spending them.
            int numAvailable = GetAvailableHD(size);
            if (numAvailable < quantity)
            {
                return false;
            }

            while (quantity > 0)
            {
                bool resp = SpendOneHDBySize(size);
                if (!resp)
                {
                    //This shouldn't happen.
                    return false;
                }
            }

            //If we got here, we spent all the HD we were told to.
            return true;
        }

        //Spends HD from the specified class, if able.
        public DiceResponse SpendHDByClass(string className, int quantity = 1, bool ignoreCon = false)
        {
            for (int a = 0; a < Classes.Count; a++)
            {
                if (Classes[a].Name == className)
                {
                    if (Classes[a].CurrentHD >= quantity)
                    {
                        //The HD are available, now we spend them.
                        Classes[a].CurrentHD -= quantity;

                        DiceResponse resp = HealingForHD (quantity, Classes[a].HDSize, ignoreCon);
                        int healed = resp.Total;

                        if (healed < 0)
                        {
                            return new DiceResponse(false);
                        }
                        else
                        {
                            //set description or something?
                            return resp;
                        }
                    } else
                    {
                        //Complain to a log?
                        return new DiceResponse(false, "Class " + className + " does not have " + quantity + " HD to spend.");
                    }   //If the class has enough HD / else
                }   //If the class is found
            }   //For each class

            //If we get here, we couldn't find that class.
            return new DiceResponse(false, "Could not find class "+className);
        }

        //Converts the character and all of it's stuff to a JSON string, presumably for saving to a file.
        public override string ToString()
        {
            var serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string ret = JsonSerializer.Serialize<Character>(this, serializerOptions);
            return ret;
        }

        //Uses the specified ability.
        public DiceResponse UseAbility(string abilityID)
        {
            int index = GetAbilityIndexByID(abilityID);
            if (index < 0)
            {
                //return new DiceResponse (false);
                return new DiceResponse(false, "Could not find ability " + abilityID);
            }

            return UseAbility(index);
        }
        public DiceResponse UseAbility(int abilityNum)
        {
            Ability ability = Abilities[abilityNum];

            DiceResponse resp = ability.Use(GetGeneralStatistics());

            IncorporateChanges(resp.Changes);

            return resp;
        }


        //Private functions:
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Calculates the character's proficiency bonus and loads it into the prof variable.
        private void CalculateProf ()
        {
            int charLevel = GetCharacterLevel();

            Prof = DiceFunctions.getProfForLevel(charLevel) + BonusToProf;
        }

        //Calculates the numeric index of the ability with the specified ID.
        private int GetAbilityIndexByID (string abilityID)
        {
            string compare = abilityID.ToLower();
            for (int a = 0; a < Abilities.Count; a++)
            {
                string compare2 = Abilities[a].ID.ToLower();
                if (compare == compare2)
                {
                    return a;
                }
            }

            //There is no such ability on this character.
            return -1;
        }

        //Handles the healing portion of spending HD.
        //Returns the amount healed, or -1 if there was an error.
        private DiceResponse HealingForHD (int num, int size, bool ignoreCon = false)
        {
            int totalHealing = 0;

            for (int a = 0; a < num; a++)
            {
                DicePile HD = new DicePile(size, 1);
                DiceResponse resp = HD.Roll();
                int amount = resp.Total;

                if (!ignoreCon)
                {
                    int conMod = GetConMod();
                    amount += conMod;
                }

                //5e doesn't allow negative healing from HD.
                if (amount < 0) { amount = 0; }

                totalHealing += amount;
            }

            //Do the healing.
            bool success = Heal(totalHealing);

            if (success)
            {
                string description = "HD: " + num + "d" + size + "+Con: healed " + totalHealing;
                return new DiceResponse (true, totalHealing, description);
            } else
            {
                return new DiceResponse (false);
            }
        }

        //Takes in some changes and applies them to the character.
        private bool IncorporateChanges (Dictionary<string, string> changes)
        {
            //to do: consider having this return false when it can't parse something.

            //If there is an index in the dictionary, assume that's meant to be a change and set the corresponding attribute here to that value.
            //We will need to do some sanity checking, though (ie: current HP cannot be more than max HP).

            int tempint = 0;

            //Consider removing support for changing the character's name or ID.
            if (changes.ContainsKey ("ID")) { ID = changes["ID"]; }
            if (changes.ContainsKey ("Name")) { Name = changes["Name"]; }
            if (changes.ContainsKey ("Race")) { Race = changes["Race"]; }

            if (changes.ContainsKey("MaxHP") && Int32.TryParse(changes["MaxHP"], out tempint))
            {
                MaxHP = tempint;
            }
            if (changes.ContainsKey("CurrentHP") && Int32.TryParse(changes["CurrentHP"], out tempint))
            {
                CurrentHP = tempint;
            }
            if (changes.ContainsKey("TempHP") && Int32.TryParse(changes["TempHP"], out tempint))
            {
                TempHP = tempint;
            }

            //...

            return true;
        }

        //Sorts the List of Abilities into the default order, so we can just fetch it as is later.
        private void SortAbilities ()
        {
            Abilities.Sort(CompareAbilities);
        }

        //Sorts the List of Spells into the default order, so we can just fetch it as is later.
        private void SortSpells()
        {
            Spells.Sort(CompareSpells);
        }

        //Used for sorting Abilities.
        private static int CompareAbilities (Ability one, Ability two)
        {
            return one.Compare(two);
        }

        //Used for sorting Spells.
        private static int CompareSpells(Spell one, Spell two)
        {
            return one.Compare(two);
        }

        private bool SpendOneHDBySize (int size, bool ignoreCon = false)
        {
            for (int a = 0; a < Classes.Count; a++)
            {
                if (Classes[a].HDSize == size && Classes[a].CurrentHD > 0)
                {
                    //Do the healing.
                    DiceResponse resp = HealingForHD(1, size, ignoreCon);
                    int healed = resp.Total;

                    if (healed < 0)
                    {
                        return false;
                    } else
                    {
                        return true;
                    }
                }
            }

            //Apparently this character doesn't have any HD of that size available.
            return false;
        }

        //Sets everything to a zeroed out / default state.
        private void ZeroEverything()
        {
            //Basic info.
            ID = "";
            Name = "";
            Race = "";
            MaxHP = 0;
            CurrentHP = 0;

            //Class and level based info.
            Classes = new List<ClassLevel>();
            BonusToProf = 0;

            //Stats and related info.
            Stats = new int[6];
            Saves = new int[6];
            for (int a = 0; a < 6; a++)
            {
                Stats[a] = 0;
                Saves[a] = 0;
            }
            Skills = new Dictionary<string, int>();
            Skills["Athletics"] = 0;
            Skills["Acrobatics"] = 0;
            Skills["Sleight of Hand"] = 0;
            Skills["Stealth"] = 0;
            Skills["Arcana"] = 0;
            Skills["History"] = 0;
            Skills["Investigation"] = 0;
            Skills["Nature"] = 0;
            Skills["Religion"] = 0;
            Skills["Animal Handling"] = 0;
            Skills["Insight"] = 0;
            Skills["Medicine"] = 0;
            Skills["Perception"] = 0;
            Skills["Survival"] = 0;
            Skills["Deception"] = 0;
            Skills["Intimidation"] = 0;
            Skills["Performance"] = 0;
            Skills["Persuasion"] = 0;

            //Ability info
            Abilities = new List<Ability>();
            BasicAbilities = new List<Ability>();
            Passives = new List<string>();
        }

        //Getters and Setters:
        //-------- -------- -------- -------- -------- -------- -------- -------- 

        //to do: how many of these are actually needed?
        //Get stats.
        public int GetStrength()
        {
            return this.Stats[0];
        }
        public int GetStr()
        {
            return this.Stats[0];
        }
        public int GetStrMod()
        {
            return DiceFunctions.getModifierForStat(this.Stats[0]);
        }
        public int GetDexterity()
        {
            return this.Stats[1];
        }
        public int GetDex()
        {
            return this.Stats[1];
        }
        public int GetDexMod()
        {
            return DiceFunctions.getModifierForStat(this.Stats[1]);
        }
        public int GetConstitution()
        {
            return this.Stats[2];
        }
        public int GetCon()
        {
            return this.Stats[2];
        }
        public int GetConMod()
        {
            return DiceFunctions.getModifierForStat(this.Stats[2]);
        }
        public int GetIntelligence()
        {
            return this.Stats[3];
        }
        public int GetInt()
        {
            return this.Stats[3];
        }
        public int GetIntMod()
        {
            return DiceFunctions.getModifierForStat(this.Stats[3]);
        }
        public int GetWisdom()
        {
            return this.Stats[4];
        }
        public int GetWis()
        {
            return this.Stats[4];
        }
        public int GetWisMod()
        {
            return DiceFunctions.getModifierForStat(this.Stats[4]);
        }
        public int GetCharisma()
        {
            return this.Stats[5];
        }
        public int GetCha()
        {
            return this.Stats[5];
        }
        public int GetChaMod()
        {
            return DiceFunctions.getModifierForStat(this.Stats[5]);
        }
        public int[] GetStats()
        {
            return this.Stats;
        }
        public int GetProf()
        {
            return (this.Prof);
        }

        //Get whether or not the character is proficient in any given save.
        public int GetSaveProf(string save)
        {
            //convert save to all lower case here
            switch (save)
            {
                case "strength":
                case "str": return GetSaveProf(0);
                case "dexterity":
                case "dex": return GetSaveProf(1);
                case "constitution":
                case "con": return GetSaveProf(2);
                case "intelligence":
                case "int": return GetSaveProf(3);
                case "wisdom":
                case "wis": return GetSaveProf(4);
                case "charisma":
                case "cha": return GetSaveProf(5);
                default:
                    //complain to a log file?
                    return 0;
            }
        }
        public int GetSaveProf(int save)
        {
            if (save < 0 || save > 5)
            {
                //complain in a log?
                return 0;
            }
            else
            {
                return this.Saves[save];
            }
        }

        //Returns a list of all abilities, in the default order.
        public List<Ability> GetAbilities ()
        {
            //to do: perhaps switch this over to something that just outputs data needed to render the things for the abilities.
            //then switch ability class over to internal only again.
            return this.Abilities;
        }

        //
        public Ability GetAbilityByID (string abilityID)
        {
            for (int a = 0; a <  this.Abilities.Count; a++)
            {
                if (Abilities[a].ID == abilityID)
                {
                    return Abilities[a];
                }
            }

            //what to return when it's not found?
            //change it to returning the ability by reference?
            //return new Ability();
            return null;
        }

        //
        public List<string> GetAbilityRechargeConditions ()
        {
            List<string> temp = new List<string> ();

            for (int a = 0; a < Abilities.Count; a++)
            {
                string recharge = Abilities[a].RechargeCondition;
                if (recharge.ToLower() == "long rest" || recharge.ToLower() == "short rest")
                {
                    //These are being hardcoded elsewhere
                    continue;
                } else if (recharge.ToLower() == "false" || recharge.ToLower() == "none" || recharge == "")
                {
                    //This ability has no recharge condition, exclude it from the list.
                    continue;
                }

                if (temp.Contains(recharge)) {
                    continue;
                }

                temp.Add (recharge);
            }

            //To do: how to make sure this ignores capitalization?
            temp.Sort();

            //We want to hardcode long and short rests to the beginning of the list.
            List<string> ret = new List<string>();
            ret.Add("Long Rest");
            ret.Add("Short Rest");
            for (int a = 0; a < temp.Count; a++)
            {
                ret.Add(temp[a]);
            }
            return ret;
        }

        public List<Ability> GetBasicAbilities ()
        {
            return this.BasicAbilities;
        }

        //Calculates and returns the flat bonus the character gets on the specified roll.
        public int GetBonusForRoll (string roll)
        {
            int ret = 0;

            //If this is a skill, this will tell us it's stat.
            //If this isn't a skill, this will send back empty string.
            string stat = DiceFunctions.GetStatForRoll(roll);

            //Add appropriate stat mod.
            if (stat == "strength")
            {
                ret += GetStrMod();
            }
            else if (stat == "dexterity")
            {
                ret += GetDexMod();
            }
            else if (stat == "constitution")
            {
                //There are no Con skills, but we'll leave that in just in case of houserules.
                ret += GetConMod();
            }
            else if (stat == "intelligence")
            {
                ret += GetIntMod();
            }
            else if (stat == "wisdom")
            {
                ret += GetWisMod();
            }
            else if (stat == "charisma")
            {
                ret += GetChaMod();
            } else
            {
                //Did someone forgot to add something to GetStatForRoll()?
                //Don't add any stat.
            }

            //Add prof as appropriate.
            decimal profMult = GetProfForRoll(roll);
            ret += (int)Math.Floor((Prof + BonusToProf) * profMult);

            //Misc bonuses not currently supported.

            return ret;
        }

        public string GetBonusForRollAsString (string roll)
        {
            int bonus = GetBonusForRoll(roll);

            if (bonus < 0)
            {
                return bonus.ToString();
            } else
            {
                return "+" + bonus.ToString();
            }
        }

        //Calculates and returns the multiplier that should be used for the character's proficiency bonus on a roll.
        //Non-proficiency returns 0, proficiency returns 1, and expertise returns 2.
        //This is decimal instead of int because 0.5x proficiency might pop up in houserules.
        public decimal GetProfForRoll (string roll)
        {
            string lower = roll.ToLower();

            //Stats are always 0.

            //Saves:
            if (lower == "strsave")
            {
                return Saves[0];
            }
            else if (lower == "dexsave")
            {
                return Saves[1];
            }
            else if (lower == "consave" || lower == "concentration")
            {
                return Saves[2];
            }
            else if (lower == "intsave")
            {
                return Saves[3];
            }
            else if (lower == "wissave")
            {
                return Saves[4];
            } else if (lower == "chasave")
            {
                return Saves[5];
            }
            //Death saves are always 0.

            //Skills:
            if (Skills.TryGetValue(roll, out var skill))
            {
                return skill;
            }
            //to do: consider replacing this with a for loop comparing ToLower() versions against each other.
                //Is there a case insensitive version of TryGetValue()?

            //Weapons go here.

            //Armor goes here?

            //Misc profs go here.
                //Musical instruments, tools, etc.

            //If we get this far, assume no proficiency.
            return 0;
        }

        //Returns the specified spell, or null if the spell is not found.
        public Spell GetSpellByID(string spellID)
        {
            for (int a = 0; a < this.Spells.Count; a++)
            {
                if (Spells[a].ID == spellID)
                {
                    return Spells[a];
                }
            }

            //what to return when it's not found?
            //return new Spell();
            return null;
        }
    }
}