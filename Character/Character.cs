using System.Collections.Immutable;
using System.ComponentModel.Design;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Xml.Schema;
using ArkDice;

namespace Character
{
    public class Character
    {
        //Basic character info
        public string ID { get; set; }
        public string Name { get; set; }
        public string Race { get; set; }
        public string Subrace { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int TempHP { get; set; }
        //temporary modifiers to max HP?

        //Class related info
        public List<ClassLevel> Classes { get; set; }

        //Added to the prof bonus calculated based on level. Can be negative.
        public int BonusToProf { get; set; }

        //Automatically managed. Stores prof bonus by level + profBonus.
        [JsonIgnore]
        public int Prof { get; set; }

        //Stats and related info
        //Starts at 0 and proceeds in the order Str, Dex, Con, Int, Wis, Cha.
        public int[] Stats { get; set; }

        public double[] Saves { get; set; }

        //Skill proficiencies
        public Dictionary<string, double> Skills { get; set; }

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
        public List<Ability> Abilities { get; set; }
        //switch from strings to objects?
        public List<Ability> BasicAbilities { get; set; }
        //Text describing passive abilities the character has.
        public List<string> Passives { get; set; }

        //The character's spell slots.
        public int[] SpellSlotsMax { get; set; }
        public int[] SpellSlotsCurrent { get; set; }
        public int[] SpellSlotsWarlockMax { get; set; }
        public int[] SpellSlotsWarlockCurrent { get; set; }

        //The character's known, prepared, and frequently used spells.
        public List<Spell> Spells { get; set; }

        //Automatically managed. Stores the location of the file this was loaded from + should be saved to.
        [JsonIgnore]
        private string FolderLocation;



        #region Constructors
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
            Saves = new double[6];
            for (int a = 0; a < 6; a++)
            {
                Stats[a] = 0;
                Saves[a] = 0;
            }
            Skills = new Dictionary<string, double>();
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

            SpellSlotsMax = new int[10];
            SpellSlotsCurrent = new int[10];
            SpellSlotsWarlockMax = new int[6];
            SpellSlotsWarlockCurrent = new int[6];
            //Note: I know I'm "wasting" index 0 here, but I'm ok with that for the convenience of index = spell slot level.
            for (int a = 1; a <= 9; a++)
            {
                SpellSlotsMax[a] = 0;
                SpellSlotsCurrent[a] = 0;
            }
            for (int a = 1; a <= 5; a++)
            {
                SpellSlotsWarlockMax[a] = 0;
                SpellSlotsWarlockCurrent[a] = 0;
            }
            Spells = new List<Spell>();

            FolderLocation = "";

            CalculateProf();
        }

        //Loads the character from a file containing JSON.
        public Character (string charID, string folderpath = "")
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

            FolderLocation = folderpath;

            var deserializerOptions = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };
            Character? copy = JsonSerializer.Deserialize<Character>(json);

            if (copy != null)
            {
                IncorporateChanges(copy);
            }

            CalculateProf();
        }

        //Loads the character from a JSON file, then compares the character's spells and abilities against the provided libraries looking for updates.
        public Character (string charID, string folderpath, ref Dictionary<string, Ability> abilitiesLibrary, ref Dictionary<string, Spell> spellsLibrary)
            : this (charID, folderpath)
        {
            //Check through the list of abilities.
            abilitiesLibrary = UpdateAllAbilities(abilitiesLibrary);

            //Then do the same for spells
            spellsLibrary = UpdateAllSpells(spellsLibrary);
        }

        #endregion


        #region Public Functions: General

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

        //Attempts to cast a spell.
        //Returns true/false to indicate success.
        //Arguments:
            //spellID is expected to be the ID of the spell being cast.
            //level is the spell level at which to cast it.
                //If level is <0, the default spell level will be used.
        public bool CastSpell (string spellID, int level = -1)
        {
            //First we check if this character even has the specified spell.
            Spell? spell = GetSpellByID(spellID);
            if (spell == null)
            {
                //This character does not know that spell / does not have it prepared.
                return false;
            }

            //Let's calculate the spell level to use.
            if (level < 0)
            {
                level = spell.Level;
            }

            //If this is a cantrip there are no slots required, so we're done.
            if (level == 0)
            {
                return true;
            }

            //Now it comes down to whether this character has a suitable spell slot.
            return TrySpendSpellSlot(level);
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

        //Changes the number of remaining spell slots of the specified level and type.
        //Returns true when it was able to do so and false when it was unable.
        public bool ChangeSpellSlots (int level, string type, int change)
        {
            type = type.ToLower();
            if (level < 1 || level > 9)
            {
                return false;
            }
            if (type == "warlock" && level > 5)
            {
                return false;
            }

            if (change == 0)
            {
                //We're already done.
                return true;
            }

            if (type == "standard")
            {
                if (SpellSlotsCurrent[level] + change < 0)
                {
                    //We can't remove this many slots.
                    return false;
                }
                else if (SpellSlotsCurrent[level] + change > SpellSlotsMax[level])
                {
                    //We can't add this many slots.
                    return false;
                } else
                {
                    //We can add/remove this many slots.
                    SpellSlotsCurrent[level] += change;
                    return true;
                }
            }
            else if (type == "warlock")
            {
                if (SpellSlotsWarlockCurrent[level] + change < 0)
                {
                    //We can't remove this many slots.
                    return false;
                }
                else if (SpellSlotsWarlockCurrent[level] + change > SpellSlotsWarlockMax[level])
                {
                    //We can't add this many slots.
                    return false;
                }
                else
                {
                    //We can add/remove this many slots.
                    SpellSlotsWarlockCurrent[level] += change;
                    return true;
                }
            } else
            {
                //We got a bad type.
                //Complain to a log?
                return false;
            }
        }

        //Returns the number of abilities that don't match those in the library provided.
        public int CheckForOutdatedAbilities(Dictionary<string, Ability> library)
        {
            int numDifferent = 0;

            for (int a = 0; a < Abilities.Count; a++)
            {
                Ability localAbility = Abilities[a];
                Ability libraryAbility = library[localAbility.ID];

                int resp = localAbility.Compare(libraryAbility);
                //Returns 0 if identical, 1 or -1 if different.
                if (resp != 0)
                {
                    numDifferent++;
                }
            }

            return numDifferent;
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
        public Dictionary<string, double> GetGeneralStatistics()
        {
            Dictionary<string, double> ret = new Dictionary<string, double>();

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

        //Takes in some changes and applies them to the character.
        public bool IncorporateChanges (Character copy)
        {
            ID = copy.ID;
            Name = copy.Name;
            Race = copy.Race;
            Subrace = copy.Subrace;
            MaxHP = copy.MaxHP;
            CurrentHP = copy.CurrentHP;
            TempHP = copy.TempHP;

            Classes = copy.Classes;

            BonusToProf = copy.BonusToProf;
            Prof = copy.Prof;
            Stats = copy.Stats;
            Saves = copy.Saves;
            Skills = copy.Skills;

            Abilities = copy.Abilities;
            BasicAbilities = copy.BasicAbilities;
            Passives = copy.Passives;

            SpellSlotsCurrent = copy.SpellSlotsCurrent;
            SpellSlotsMax = copy.SpellSlotsMax;
            SpellSlotsWarlockCurrent = copy.SpellSlotsWarlockCurrent;
            SpellSlotsWarlockMax = copy.SpellSlotsWarlockMax;
            Spells = copy.Spells;

            SortAbilities();
            SortSpells();

            CalculateProf();

            return true;
        }
        public bool IncorporateChanges(Dictionary<string, string> changes)
        {
            //If there is an index in the dictionary, assume that's meant to be a change and set the corresponding attribute here to that value.
            //We will need to do some sanity checking, though (ie: current HP cannot be more than max HP).

            int tempint = 0;

            //Consider removing support for changing the character's name or ID.
            if (changes.ContainsKey("ID")) { ID = changes["ID"]; }
            if (changes.ContainsKey("Name")) { Name = changes["Name"]; }
            if (changes.ContainsKey("Race")) { Race = changes["Race"]; }

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

            //Stats
            if (changes.ContainsKey("Str") && Int32.TryParse(changes["Str"], out tempint))
            {
                Stats[0] = tempint;
            }
            if (changes.ContainsKey("Dex") && Int32.TryParse(changes["Dex"], out tempint))
            {
                Stats[1] = tempint;
            }
            if (changes.ContainsKey("Con") && Int32.TryParse(changes["Con"], out tempint))
            {
                Stats[2] = tempint;
            }
            if (changes.ContainsKey("Int") && Int32.TryParse(changes["Int"], out tempint))
            {
                Stats[3] = tempint;
            }
            if (changes.ContainsKey("Wis") && Int32.TryParse(changes["Wis"], out tempint))
            {
                Stats[4] = tempint;
            }
            if (changes.ContainsKey("Cha") && Int32.TryParse(changes["Cha"], out tempint))
            {
                Stats[5] = tempint;
            }

            //saves go here

            //Skills
            double tempdouble;
            foreach (KeyValuePair<string, double> kvp in Skills) {
                string key = kvp.Key;
                //double value = kvp.Value;

                if (changes.TryGetValue(key, out string? valstring))
                {
                    if (valstring != null)
                    {
                        if (double.TryParse(valstring, out tempdouble))
                        {
                            Skills[key] = tempdouble;
                        }
                    }
                }
            }

            if (changes.ContainsKey("ProfBonusBonus") && Int32.TryParse(changes["ProfBonusBonus"], out tempint))
            {
                BonusToProf = tempint;
            }
            else if (changes.ContainsKey("BonusToProf") && Int32.TryParse(changes["BonusToProf"], out tempint))
            {
                BonusToProf = tempint;
            }

            //Standard Spell Slots
            for (int a = 1; a <= 9; a++)
            {
                string index = "SpellsLvl" + a;
                if (changes.ContainsKey (index) && Int32.TryParse(changes[index], out tempint))
                {
                    SpellSlotsMax[a] = tempint;

                    //Make sure this spell level doesn't have more current slots than maximum.
                    if (SpellSlotsCurrent[a] > SpellSlotsMax[a])
                    {
                        SpellSlotsCurrent[a] = SpellSlotsMax[a];
                    }
                }
            }

            //Warlock Spell Slots
            for (int a = 1; a <= 5; a++)
            {
                string index = "SpellsWarlockLvl" + a;
                if (changes.ContainsKey(index) && Int32.TryParse(changes[index], out tempint))
                {
                    SpellSlotsWarlockMax[a] = tempint;

                    //Make sure this spell level doesn't have more current slots than maximum.
                    if (SpellSlotsWarlockCurrent[a] > SpellSlotsWarlockMax[a])
                    {
                        SpellSlotsWarlockCurrent[a] = SpellSlotsWarlockMax[a];
                    }
                }
            }

            //Class Levels
            if (changes.ContainsKey("Classes"))
            {
                var deserializerOptions = new JsonSerializerOptions
                {
                    NumberHandling = JsonNumberHandling.AllowReadingFromString
                };

                try
                {
                    var decoded = JsonSerializer.Deserialize<ClassLevelList>(changes["Classes"]);

                    if (decoded != null)
                    {
                        //We have a list of zero or more ClassLevel objects, incorporate them into the character.

                        //Everything should be present, so we can just overwrite what's already there.
                        Classes = new List<ClassLevel>();
                        foreach (ClassLevel level in decoded.Levels)
                        {
                            Classes.Add(level);
                        }
                    }
                } catch (Exception ex)
                {
                    //Complain and don't do anything with the classes.
                }
                

                
            }

            //...

            CalculateProf();

            return true;
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
        public bool RechargeAbilities (string condition)
        {
            condition = condition.ToLower();
            if (condition == "short rest")
            {
                //Do short rest stuff.
            }
            else if (condition == "long rest")
            {
                //Do short + long rest stuff.
                RegainHD("half");
            }

            //Replenish abilities as appropriate.
            for (int a = 0; a < Abilities.Count; a++)
            {
                Abilities[a].MaybeRecharge(condition);
            }

            return true;
        }

        //Recharges spell slots (or not) on long / short rest.
        public void RechargeSpellSlots (string condition)
        {
            //Standard spell slots.
            if (condition.ToLower() == "long rest")
            {
                for (int a = 1; a <= 9; a++)
                {
                    SpellSlotsCurrent[a] = SpellSlotsMax[a];
                }
            }

            //Warlock spell slots.
            if (condition.ToLower() == "long rest" || condition.ToLower() == "short rest")
            {
                for (int a = 1; a <= 5; a++)
                {
                    SpellSlotsWarlockCurrent[a] = SpellSlotsWarlockMax[a];
                }
            }
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
            } else if (amount == "all")
            {
                //Regain all available HD.
                //We don't need to do any fancy math for this one, just set everything to the maximum.
                for (int a = 0; a < Classes.Count; a++)
                {
                    Classes[a].CurrentHD = Classes[a].Level;
                }
            }

            return true;
        }

        //Removes the specified ability, if present.
        public void RemoveAbility(string id)
        {
            int index = GetAbilityIndexByID(id);
            if (index >= 0)
            {
                Abilities.RemoveAt(index);
            }

            SortAbilities();
        }

        //Removes the specified spell, if present.
        public void RemoveSpell(string id)
        {
            int index = GetSpellIndexByID(id);
            if (index >= 0)
            {
                Spells.RemoveAt(index);
            }

            SortSpells();
        }

        //Makes a roll using the character's stats and proficiencies.
        public DiceResponse RollForCharacter(DiceCollection dc)
        {
            Dictionary<string, double> stats = this.GetGeneralStatistics();

            DiceResponse resp = dc.Roll(stats);

            return resp;
        }

        //Saves the character and all it's abilities to a file.
        public bool Save(Settings.Settings settings)
        {
            string backupFolder = settings.GetCharacterBackupFolderpath();
            string filepath = settings.GetCharacterFilepath(ID);
            string backupFilepath = backupFolder + ID + "_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".char";

            if (System.IO.File.Exists(filepath))
            {
                //The file exists, so we need to make a backup.
                if (!System.IO.Directory.Exists(backupFolder))
                {
                    //Make sure the backup folder exists.
                    System.IO.Directory.CreateDirectory(backupFolder);
                }

                //Now move the old file.
                try
                {
                    System.IO.File.Move(filepath, backupFilepath);
                    //touch the file?
                    if (System.IO.File.Exists(filepath))
                    {
                        //We were unable to move the file for some reason.
                        return false;
                    }
                }
                catch
                {
                    //Complain to a log file?
                    return false;
                }
            }

            //Now we write the new file into the specified spot.
            string characterString = this.ToString();
            File.WriteAllText(filepath, characterString);

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

        //Attempts to spend one spell slot of the specified level.
        public bool TrySpendSpellSlot (int level, string type = "")
        {
            //Do some basic error checking.
            if (level < 0)
            {
                return false;
            }

            //Check for Warlock slots first.
            if (level <= 5 && (type == "" || type.ToLower() == "warlock"))
            {
                if (SpellSlotsWarlockCurrent[level] > 0)
                {
                    //We have one to spend, use it.
                    SpellSlotsWarlockCurrent[level]--;
                    return true;
                }
            }

            //Then check standard slots.
            if (level <= 5 && (type == "" || type.ToLower() == "standard"))
            {
                if (SpellSlotsCurrent[level] > 0)
                {
                    //We have one to spend, use it.
                    SpellSlotsCurrent[level]--;
                    return true;
                }
            }

            //If we get this far, there are no suitable spell slots of this level.
            return false;
        }

        //Converts the character and all of it's stuff to a JSON string, presumably for saving to a file.
        public override string ToString()
        {
            var serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string ret = JsonSerializer.Serialize<Character>(this, serializerOptions);
            //error checking here?

            return ret;
        }

        //Compares all the character's abilities against those in a library.
        //Also updates any abilities here to match those in the library, when there's a conflict.
        //Returns a modified library in case abilities on the character were added to it.
        public Dictionary<string,Ability> UpdateAllAbilities(Dictionary<string, Ability> library)
        {
            Dictionary<string, Ability> ret = library;

            for (int a = 0; a < Abilities.Count; a++)
            {
                Ability localAbility = Abilities[a];
                string id = localAbility.ID;
                if (id == "")
                {
                    //This shouldn't happen, but we'll check for it just in case.
                    //complain to a log file?
                    continue;
                }

                if (!library.ContainsKey(id))
                {
                    //This ability does not exist in the library, add it.
                    ret.Add(id, localAbility);
                }
                else
                {
                    //This ability does exist in the library, grab it and compare it to the local copy.
                    Ability libraryAbility = library[id];

                    int resp = localAbility.Compare(libraryAbility);
                    //Returns 0 if identical, 1 or -1 if different.
                    if (resp != 0)
                    {
                        //These two abilities are not identical.
                        //Overwrite the local version with the library version.
                        Abilities[a] = libraryAbility;
                        Abilities[a].Uses = localAbility.Uses;
                        if (Abilities[a].Uses > Abilities[a].MaxUses)
                        {
                            //The max uses have lowered, fix this.
                            Abilities[a].Uses = Abilities[a].MaxUses;
                        }
                        //Copy over anything else?
                    }
                }
            }

            return ret;
        }

        //Compares all the character's spells against those in a library.
        //Also updates any spells here to match those in the library, when there's a conflict.
        //Returns a modified library in case spells on the character were added to it.
        public Dictionary<string, Spell> UpdateAllSpells(Dictionary<string, Spell> library)
        {
            Dictionary<string, Spell> ret = library;

            for (int a = 0; a < Spells.Count; a++)
            {
                Spell localSpell = Spells[a];
                string id = localSpell.ID;
                if (id == "")
                {
                    //This shouldn't happen, but we'll check for it just in case.
                    //complain to a log file?
                    continue;
                }

                if (!library.ContainsKey(id))
                {
                    //This spell does not exist in the library, add it.
                    ret.Add(id, localSpell);
                }
                else
                {
                    //This spell does exist in the library, grab it and compare it to the local copy.
                    Spell librarySpell = library[id];

                    int resp = localSpell.Compare(librarySpell);
                    //Returns 0 if identical, 1 or -1 if different.
                    if (resp != 0)
                    {
                        //These two spells are not identical.
                        //Overwrite the local version with the library version.
                        Spells[a] = librarySpell;
                        //Copy over anything else?
                    }
                }
            }

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

        #endregion


        #region Public Functions: Getters and Setters for Stats

        //Getters for stats, to obfuscate the array that's actually used.
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

        //Setters for stats, to obfuscate the array that's actually used.
        public void SetStr(int newVal)
        {
            Stats[0] = newVal;
        }
        public void SetDex(int newVal)
        {
            Stats[1] = newVal;
        }
        public void SetCon(int newVal)
        {
            Stats[2] = newVal;
        }
        public void SetInt(int newVal)
        {
            Stats[3] = newVal;
        }
        public void SetWis(int newVal)
        {
            Stats[4] = newVal;
        }
        public void SetCha(int newVal)
        {
            Stats[5] = newVal;
        }

        #endregion


        #region Public Functions: Other Getters and Setters

        //Adds the specified ability if the character doesn't have it, or updates the character's version of the ability if they do have it.
        public void AddOrUpdateAbility(Ability ability, bool add = true, bool update = true)
        {
            int index = GetAbilityIndexByID(ability.ID);
            if (index >= 0)
            {
                if (update)
                {
                    //This character has the ability, we want to update what's there.
                    Abilities[index] = ability;
                    //Run some logic to make sure the available uses are still within bounds?
                }
            }
            else
            {
                if (add)
                {
                    //This character does not have the ability, add it.
                    Abilities.Add(ability);
                }
            }

            SortAbilities();
        }

        //Adds the specified spell if the character doesn't have it, or updates the character's version of the spell if they do have it.
        public void AddOrUpdateSpell(Spell spell, bool add = true, bool update = true)
        {
            int index = GetSpellIndexByID(spell.ID);
            if (index >= 0)
            {
                if (update)
                {
                    Spells[index] = spell;
                }
            }
            else
            {
                if (add)
                {
                    Spells.Add(spell);
                }
            }

            SortSpells();
        }

        //Indicates whether this character has a class with the provided name.
        //Also checks the subclass if a name is provided.
        public bool CharacterHasClassWithName (string className, string subclassName = "")
        {
            int index = GetClassIndexByName(className, subclassName);
            if (index < 0)
            {
                return false;
            } else
            {
                return true;
            }
        }

        //Get whether or not the character is proficient in any given save.
        //Returns the multiple of the character's prof bonus that gets used (0 for no prof, 1 for proficient, etc).
        public double GetSaveProf(string save)
        {
            save = save.ToLower();
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
        public double GetSaveProf(int save)
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

        //Get whether or not the character is proficient in any given skill.
        //Returns the multiple of the character's prof bonus that gets used (0 for no prof, 1 for proficient, etc).
        public double GetSkillProf(string skill)
        {
            if (Skills.ContainsKey(skill))
            {
                return Skills[skill];
            }
            else
            {
                //complain to a log file.
                return 0;
            }
        }

        //Returns a list of all abilities, in the default order.
        public List<Ability> GetAbilities()
        {
            return this.Abilities;
        }

        //Returns an Ability with the specified ID, if this character has one.
        //Returns null if the character doesn't have a matching Ability.
        public Ability GetAbilityByID(string abilityID)
        {
            for (int a = 0; a < this.Abilities.Count; a++)
            {
                if (Abilities[a].ID == abilityID)
                {
                    return Abilities[a];
                }
            }

            return null;
        }

        //Returns a list of recharge conditions for all of the character's abilities.
        //Skips those that are short/long rest or with several flavors of "none."
        public List<string> GetAbilityRechargeConditions()
        {
            List<string> temp = new List<string>();

            for (int a = 0; a < Abilities.Count; a++)
            {
                string recharge = Abilities[a].RechargeCondition;
                if (recharge.ToLower() == "long rest" || recharge.ToLower() == "short rest")
                {
                    //These are being hardcoded elsewhere
                    continue;
                }
                else if (recharge.ToLower() == "false" || recharge.ToLower() == "none" || recharge == "")
                {
                    //This ability has no recharge condition, exclude it from the list.
                    continue;
                }

                if (temp.Contains(recharge))
                {
                    continue;
                }

                temp.Add(recharge);
            }

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

        //Returns a list of the character's basic abilities.
        public List<Ability> GetBasicAbilities()
        {
            return this.BasicAbilities;
        }

        //Calculates and returns the flat bonus the character gets on the specified roll.
        public int GetBonusForRoll(string roll)
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
            }
            else
            {
                //Did someone forget to add something to GetStatForRoll()?
                //Don't add any stat.
            }

            //Add prof as appropriate.
            double profMult = GetProfForRoll(roll);
            ret += (int)Math.Floor((Prof + BonusToProf) * profMult);

            //Misc bonuses not currently supported.

            return ret;
        }

        //It does GetBonusForRoll() but converts the response to a string.
        public string GetBonusForRollAsString(string roll)
        {
            int bonus = GetBonusForRoll(roll);

            if (bonus < 0)
            {
                return bonus.ToString();
            }
            else
            {
                return "+" + bonus.ToString();
            }
        }

        //Returns the class matching the specified name.
        public ClassLevel GetClassByName (string className, string subclassName = "")
        {
            int index = GetClassIndexByName(className, subclassName);

            if (index >= 0)
            {
                return Classes[index];
            } else
            {
                //to do: what to send back?
                //Just return a blank ClassLevel for now.
                return new ClassLevel();
            }
        }

        //Returns the index for any class matching the specified name.
        //If subclassName is also specified, it will require that that matches as well.
        //Returns -1 if there is no matching class.
        public int GetClassIndexByName (string className, string subclassName = "")
        {
            for (int a = 0; a < Classes.Count; a++)
            {
                string thisClass = Classes[a].Name;
                string thisSubclass = Classes[a].Subclass;

                if (thisClass != className)
                {
                    //This isn't the one we're looking for, we can move on.
                    continue;
                }

                if (subclassName != "" && subclassName != thisSubclass)
                {
                    //This isn't the one we're looking for, we can move on.
                    continue;
                }

                //If we get this far, they match to the level of information we were given.
                return a;
            }

            //If we get this far, the specified class doesn't exist in the list.
            return -1;
        }

        //Calculates and returns the multiplier that should be used for the character's proficiency bonus on a roll.
        //Non-proficiency returns 0, proficiency returns 1, and expertise returns 2.
        //This is decimal instead of int because 0.5x proficiency might pop up in houserules.
        public double GetProfForRoll(string roll)
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
            }
            else if (lower == "chasave")
            {
                return Saves[5];
            }
            //Death saves are always 0.

            //Skills:
            if (Skills.TryGetValue(roll, out var skill))
            {
                return (int)skill;
            }

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

        //Returns a list of all spells, in the default order.
        public List<Spell> GetSpells()
        {
            return this.Spells;
        }

        //Returns the number of available spell slots of the specified level and type.
        //fetchCurrent controls whether this returns the currently-available number of spell slots (true) or the maximum (false).
        //type controls whether this returns all spell slots of the specified level ("") or just those of a particular type (ie: "warlock"). Case insensitive.
        public int GetSpellSlotsForLevel(int level, bool fetchCurrent = true, string type = "")
        {
            int ret = 0;

            //Maybe add standard spell slots.
            if (type == "" || type.ToLower() == "standard")
            {
                if (level >= 1 && level <= 9)
                {
                    ret += (fetchCurrent) ? SpellSlotsCurrent[level] : SpellSlotsMax[level];
                }
            }

            //Maybe add warlock spell slots.
            if (type == "" || type.ToLower() == "warlock")
            {
                if (level >= 1 && level <= 5)
                {
                    ret += (fetchCurrent) ? SpellSlotsWarlockCurrent[level] : SpellSlotsWarlockMax[level];
                }
            }

            return ret;
        }

        #endregion


        #region Private Functions
        //Calculates the character's proficiency bonus and loads it into the prof variable.
        private void CalculateProf ()
        {
            int charLevel = GetCharacterLevel();

            Prof = DiceFunctions.getProfForLevel(charLevel) + BonusToProf;
        }

        //Calculates the numeric index of the ability with the specified ID.
        //Returns -1 if not found.
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

        //Calculates the numeric index of the spell with the specified ID.
        //Returns -1 if not found.
        private int GetSpellIndexByID(string spellID)
        {
            string compare = spellID.ToLower();
            for (int a = 0; a < Spells.Count; a++)
            {
                string compare2 = Spells[a].ID.ToLower();
                if (compare == compare2)
                {
                    return a;
                }
            }

            //There is no such spell on this character.
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

        //Sorts the List of Abilities into the default order, so we can just fetch it as is later.
        private void SortAbilities ()
        {
            Abilities.Sort(CompareAbilities);
        }

        //Sorts the list of classes into alphabetical order, with ties broken by subclass.
        private void SortClasses ()
        {
            Classes.Sort();
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

        //Spends one HD of the specified size, if possible, and does the appropriate healing.
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
            Saves = new double[6];
            for (int a = 0; a < 6; a++)
            {
                Stats[a] = 0;
                Saves[a] = 0;
            }
            Skills = new Dictionary<string, double>();
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

        #endregion

    }
}