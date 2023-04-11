using System.ComponentModel.Design;
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
        public string id { get; private set; }
        public string name { get; private set; }
        public string race { get; private set; }
        //subrace as separate field?
        public int maxHP { get; private set; }
        public int currentHP { get; private set; }
        public int tempHP { get; private set; }
        //temporary modifiers to max HP?

        //Class related info
        public List<ClassLevel> classes { get; private set; }
        //to do: consider setting something up to support gestalt rules.
            //just set up a multiplier for levels or 'class levels per character level' type thing?
                //the character's level equals total class level divided by that number

        //Added to the prof bonus calculated based on level. Can be negative.
        public int profBonus { get; private set; }

        //Automatically managed. Stores prof bonus by level + profBonus.
        public int prof { get; private set; }

        //Stats and related info
        //Starts at 0 and proceeds in the order Str, Dex, Con, Int, Wis, Cha.
        public int[] stats { get; private set; }
            //convert these over to loose ints/bools?
            //there are only 6 of each, and it would cut down a little on the work involved.

        public int[] saveProfs { get; private set; }
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
        public Dictionary<string, int> skills{ get; private set; }

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
        public List<Ability> abilities{ get; private set; }
            //switch from strings to objects?
        public List<Ability> basicAbilities { get; private set; }
        //Text describing passive abilities the character has.
        public List<string> passives { get; private set; }

        //Constructor(s):
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        
        //Creates a blank character.
        public Character ()
        {
            //Basic info.
            id = "";
            name = "";
            race = "";
            maxHP = 0;
            currentHP = 0;
            tempHP = 0;

            //Class and level based info.
            classes = new List<ClassLevel>();
            profBonus = 0;

            //Stats and related info.
            stats = new int[6];
            saveProfs = new int[6];
            for (int a = 0; a < 6; a++)
            {
                stats[a] = 0;
                saveProfs[a] = 0;
            }
            skills = new Dictionary<string, int>();
            skills["Athletics"] = 0;
            skills["Acrobatics"] = 0;
            skills["Sleight of Hand"] = 0;
            skills["Stealth"] = 0;
            skills["Arcana"] = 0;
            skills["History"] = 0;
            skills["Investigation"] = 0;
            skills["Nature"] = 0;
            skills["Religion"] = 0;
            skills["Animal Handling"] = 0;
            skills["Insight"] = 0;
            skills["Medicine"] = 0;
            skills["Perception"] = 0;
            skills["Survival"] = 0;
            skills["Deception"] = 0;
            skills["Intimidation"] = 0;
            skills["Performance"] = 0;
            skills["Persuasion"] = 0;

            //Ability info
            abilities = new List<Ability>();
            basicAbilities = new List<Ability>();
            passives = new List<string>();

            CalculateProf();
        }

        //public Character (string filename)
        //{
        //    //load "/Characters/" + filename
        //    //parse it's contents
        //    //load the stuff into this
        //}
        //take the 'load from json' stuff from the other constructor, make it a function, and then call it here and there?

        //Load a character from a JSON string.
        public Character (string json)
            : this()
        {
            //to do: look into making this work with deserialize.
            //will i have to make a struct with the same attributes and then write a function to copy stuff from there to here?

            //Temporarily removed.
            //Doesn't work, I'm guessing it requires public setters to work but I don't want those.
            /*
            try
            {
                Character copy = JsonSerializer.Deserialize<Character>(json);

                if (copy == null)
                {
                    //is this even possible?
                    //complain to a log
                    return;
                }

                //copy copy's stuff to here.
                //do some error checking first?

                id = copy.id;
                name = copy.name;
                race = copy.race;
                maxHP = copy.maxHP;
                currentHP = copy.currentHP;
                classes = copy.classes;
                prof = copy.prof;
                profBonus = copy.profBonus;
                stats = copy.stats;
                saveProfs = copy.saveProfs;
                skills = copy.skills;
                abilities = copy.abilities;
                basicAbilities = copy.basicAbilities;
                passives = copy.passives;
                return;
            }
            catch
            {
                //complain to a log?
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
                if (root.TryGetProperty("id", out temp))
                {
                    id = temp.ToString();
                }
                if (root.TryGetProperty("name", out temp))
                {
                    name = temp.ToString();
                }
                if (root.TryGetProperty("race", out temp))
                {
                    //not currently in the file
                    race = temp.ToString();
                }
                if (root.TryGetProperty("maxHP", out temp))
                {
                    if (Int32.TryParse(temp.ToString(), out tempint))
                    {
                        maxHP = tempint;
                    }
                }
                if (root.TryGetProperty("currentHP", out temp))
                {
                    if (Int32.TryParse(temp.ToString(), out tempint))
                    {
                        currentHP = tempint;
                    }
                }
                if (root.TryGetProperty("profBonus", out temp))
                {
                    //not currently in the file
                    if (Int32.TryParse(temp.ToString(), out tempint))
                    {
                        profBonus = tempint;
                    }
                }

                //Classes and levels
                if (root.TryGetProperty("classes", out temp))
                {
                    classes = new List<ClassLevel>();
                    JsonElement temp2 = new JsonElement();

                    for (int a = 0; a < temp.GetArrayLength(); a++)
                    {
                        JsonElement thisclass = temp[a];
                        ClassLevel newclass = new ClassLevel();

                        newclass.name = "";
                        if (thisclass.TryGetProperty("name", out temp2))
                        {
                            newclass.name = temp2.ToString();
                        }
                        else
                        {
                            //complain to a log file?
                            continue;
                        }
                        newclass.subclass = "";
                        if (thisclass.TryGetProperty("subclass", out temp2))
                        {
                            newclass.subclass = temp2.ToString();
                        }
                        //Subclass is not required, so we don't quit when it's not present.
                        newclass.level = 0;
                        if (thisclass.TryGetProperty("level", out temp2))
                        {
                            if (Int32.TryParse(temp2.ToString(), out tempint))
                            {
                                newclass.level = tempint;
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
                        newclass.currentHD = 0;
                        if (thisclass.TryGetProperty("currentHD", out temp2))
                        {
                            if (Int32.TryParse(temp2.ToString(), out tempint))
                            {
                                newclass.currentHD = tempint;
                            }
                        }

                        classes.Add(newclass);
                    }
                }

                //Stats
                //to do: this is now a purely numeric array, adjust this to compensate
                if (root.TryGetProperty("stats", out temp))
                {
                    //We don't technically need to do this here, but it won't hurt.
                    stats = new int[6];
                    //JsonElement temp2 = new JsonElement();

                    //to do: check if array length is 6 and complain otherwise?
                    for (int a = 0; a < temp.GetArrayLength() && a < 6; a++)
                    {
                        JsonElement thisstat = temp[a];

                        if (thisstat.TryGetInt32(out tempint))
                        {
                            stats[a] = tempint;
                        }
                        else
                        {
                            stats[a] = 0;
                            //complain to a log?
                        }
                    }
                    /*
                    if (temp.TryGetProperty("strength", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            stats[0] = tempint;
                        }
                    }
                    if (temp.TryGetProperty("dexterity", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            stats[1] = tempint;
                        }
                    }
                    if (temp.TryGetProperty("constitution", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            stats[2] = tempint;
                        }
                    }
                    if (temp.TryGetProperty("intelligence", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            stats[3] = tempint;
                        }
                    }
                    if (temp.TryGetProperty("wisdom", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            stats[4] = tempint;
                        }
                    }
                    if (temp.TryGetProperty("charisma", out temp2))
                    {
                        //if (Int32.TryParse(temp2.ToString(), out tempint))
                        if (temp2.TryGetInt32(out tempint))
                        {
                            stats[5] = tempint;
                        }
                    }
                    */
                }

                //Saves
                //to do: this is now a purely numeric array, adjust this to compensate
                if (root.TryGetProperty("saves", out temp))
                {
                    saveProfs = new int[6];
                    JsonElement temp2 = new JsonElement();
                    if (temp.TryGetProperty("strength", out temp2))
                    {
                        //saveProfs[0] = temp2.GetBoolean();
                        bool tempbool = temp2.GetBoolean();
                        if (tempbool)
                        {
                            saveProfs[0] = 1;
                        }
                        else
                        {
                            saveProfs[0] = 0;
                        }
                    }
                    if (temp.TryGetProperty("dexterity", out temp2))
                    {
                        //saveProfs[1] = temp2.GetBoolean();
                        bool tempbool = temp2.GetBoolean();
                        if (tempbool)
                        {
                            saveProfs[1] = 1;
                        }
                        else
                        {
                            saveProfs[1] = 0;
                        }
                    }
                    if (temp.TryGetProperty("constitution", out temp2))
                    {
                        //saveProfs[2] = temp2.GetBoolean();
                        bool tempbool = temp2.GetBoolean();
                        if (tempbool)
                        {
                            saveProfs[2] = 1;
                        }
                        else
                        {
                            saveProfs[2] = 0;
                        }
                    }
                    if (temp.TryGetProperty("intelligence", out temp2))
                    {
                        //saveProfs[3] = temp2.GetBoolean();
                        bool tempbool = temp2.GetBoolean();
                        if (tempbool)
                        {
                            saveProfs[3] = 1;
                        }
                        else
                        {
                            saveProfs[3] = 0;
                        }
                    }
                    if (temp.TryGetProperty("wisdom", out temp2))
                    {
                        //saveProfs[4] = temp2.GetBoolean();
                        bool tempbool = temp2.GetBoolean();
                        if (tempbool)
                        {
                            saveProfs[4] = 1;
                        }
                        else
                        {
                            saveProfs[4] = 0;
                        }
                    }
                    if (temp.TryGetProperty("charisma", out temp2))
                    {
                        //saveProfs[5] = temp2.GetBoolean();
                        bool tempbool = temp2.GetBoolean();
                        if (tempbool)
                        {
                            saveProfs[5] = 1;
                        }
                        else
                        {
                            saveProfs[5] = 0;
                        }
                    }
                }

                //Skills
                //to do: set up a way to automatically get these indices.
                    //just do a foreach through the existing dictionary, since they should all be in there now?
                if (root.TryGetProperty("skills", out temp))
                {
                    skills = new Dictionary<string, int>();
                    JsonElement temp2 = new JsonElement();

                    skills["Athletics"] = 0;
                    if (temp.TryGetProperty("Athletics", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            skills["Athletics"] = tempint;
                        }
                    }
                    skills["Acrobatics"] = 0;
                    if (temp.TryGetProperty("Acrobatics", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            skills["Acrobatics"] = tempint;
                        }
                        else
                        {
                            skills["Acrobatics"] = 0;
                        }
                    }
                    skills["Sleight of Hand"] = 0;
                    if (temp.TryGetProperty("Sleight of Hand", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            skills["Sleight of Hand"] = tempint;
                        }
                    }
                    skills["Stealth"] = 0;
                    if (temp.TryGetProperty("Stealth", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            skills["Stealth"] = tempint;
                        }
                    }
                    skills["Arcana"] = 0;
                    if (temp.TryGetProperty("Arcana", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            skills["Arcana"] = tempint;
                        }
                    }
                    skills["History"] = 0;
                    if (temp.TryGetProperty("History", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            skills["History"] = tempint;
                        }
                    }
                    skills["Investigation"] = 0;
                    if (temp.TryGetProperty("Investigation", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            skills["Investigation"] = tempint;
                        }
                    }
                    skills["Nature"] = 0;
                    if (temp.TryGetProperty("Nature", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            skills["Nature"] = tempint;
                        }
                    }
                    skills["Religion"] = 0;
                    if (temp.TryGetProperty("Religion", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            skills["Religion"] = tempint;
                        }
                    }
                    skills["Animal Handling"] = 0;
                    if (temp.TryGetProperty("Animal Handling", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            skills["Animal Handling"] = tempint;
                        }
                    }
                    skills["Insight"] = 0;
                    if (temp.TryGetProperty("Insight", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            skills["Insight"] = tempint;
                        }
                    }
                    skills["Medicine"] = 0;
                    if (temp.TryGetProperty("Medicine", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            skills["Medicine"] = tempint;
                        }
                    }
                    skills["Perception"] = 0;
                    if (temp.TryGetProperty("Perception", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            skills["Perception"] = tempint;
                        }
                    }
                    skills["Survival"] = 0;
                    if (temp.TryGetProperty("Survival", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            skills["Survival"] = tempint;
                        }
                    }
                    skills["Deception"] = 0;
                    if (temp.TryGetProperty("Deception", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            skills["Deception"] = tempint;
                        }
                    }
                    skills["Intimidation"] = 0;
                    if (temp.TryGetProperty("Intimidation", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            skills["Intimidation"] = tempint;
                        }
                    }
                    skills["Performance"] = 0;
                    if (temp.TryGetProperty("Performance", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            skills["Performance"] = tempint;
                        }
                    }
                    skills["Persuasion"] = 0;
                    if (temp.TryGetProperty("Persuasion", out temp2))
                    {
                        if (temp2.TryGetInt32(out tempint))
                        {
                            skills["Persuasion"] = tempint;
                        }
                    }
                }

                //Abilities
                if (root.TryGetProperty ("abilities", out temp))
                {
                    //Contains an array of objects.
                    //Pass each object to the Ability class' constructor.

                    abilities = new List<Ability>();

                    for (int a = 0; a < temp.GetArrayLength(); a++)
                    {
                        Ability thisAbility = new Ability(temp[a].ToString());
                        abilities.Add(thisAbility);
                    }
                }

                //basic abilities goes here

                //passives goes here

            }
            catch
            {
                //complain to a log file?
                return;
            }

            CalculateProf();
        }

        //Currently only used by abilities for sending back lists of changes.
        public Character (string id, string name)
            : this()
        {
            this.id = id;
            this.name= name;

            ////Everything else is baseline / false / 0 / etc.
            //race = "";
            //maxHP = 0;
            //currentHP = 0;
            //classes = new List<ClassLevel> ();
            //prof = 0;
            //profBonus = 0;
            //stats = new int[6];
            //saveProfs = new int[6];
            //for (int a = 0; a < 6; a++)
            //{
            //    stats[a] = 0;
            //    saveProfs[a] = 0;
            //}
            //abilities= new List<Ability> ();
            //passives= new List<string> ();
        }


        //Public functions:
        //-------- -------- -------- -------- -------- -------- -------- -------- 

        //Deal damage to the character.
        public bool Damage(int amount, bool allowNegative = false)
        {
            //We need to account for temp HP.
            int totalHP = currentHP + tempHP;

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
                    if (amount <= tempHP)
                    {
                        tempHP -= amount;
                        return true;
                    } else
                    {
                        amount -= tempHP;
                        tempHP = 0;
                        currentHP -= amount;
                        return true;
                    }
                }
                else
                {
                    //We're going to die.
                    if (allowNegative)
                    {
                        amount -= tempHP;
                        tempHP = 0;
                        currentHP -= amount;
                        return true;
                    } else
                    {
                        tempHP = 0;
                        currentHP = 0;
                        return true;
                    }
                }
            }
        }

        //Returns the sum of the character's levels in each of their classes.
        public int GetCharacterLevel ()
        {
            int ret = 0;

            foreach (var classEntry in classes)
            {
                ret += classEntry.level;
            }

            return ret;
        }

        //Returns a list of the character's basic numeric stats for use with abilities.
        public Dictionary<string, int> GetGeneralStatistics()
        {
            Dictionary<string, int> ret = new Dictionary<string, int>();

            //Stats and modifiers
            ret["strmod"] = DiceFunctions.getModifierForStat(stats[0]);
            ret["dexmod"] = DiceFunctions.getModifierForStat(stats[1]);
            ret["conmod"] = DiceFunctions.getModifierForStat(stats[2]);
            ret["intmod"] = DiceFunctions.getModifierForStat(stats[3]);
            ret["wismod"] = DiceFunctions.getModifierForStat(stats[4]);
            ret["chamod"] = DiceFunctions.getModifierForStat(stats[5]);

            ret["strscore"] = stats[0];
            ret["dexscore"] = stats[1];
            ret["conscore"] = stats[2];
            ret["intscore"] = stats[3];
            ret["wisscore"] = stats[4];
            ret["chascore"] = stats[5];

            //General info
            ret["prof"] = prof;
            ret["level"] = GetCharacterLevel();
            //toss in current and max hp?

            //Save profs
            ret["strsave"] = saveProfs[0];
            ret["dexsave"] = saveProfs[1];
            ret["consave"] = saveProfs[2];
            ret["intsave"] = saveProfs[3];
            ret["wissave"] = saveProfs[4];
            ret["chasave"] = saveProfs[5];

            //Skill profs
            ret["athletics"]        = skills["Athletics"];
            ret["acrobatics"]       = skills["Acrobatics"];
            ret["sleightofhand"]    = skills["Sleight of Hand"];
            ret["stealth"]          = skills["Stealth"];
            ret["arcana"]           = skills["Arcana"];
            ret["history"]          = skills["History"];
            ret["investigation"]    = skills["Investigation"];
            ret["nature"]           = skills["Nature"];
            ret["religion"]         = skills["Religion"];
            ret["animalhandling"]   = skills["Animal Handling"];
            ret["insight"]          = skills["Insight"];
            ret["medicine"]         = skills["Medicine"];
            ret["perception"]       = skills["Perception"];
            ret["survival"]         = skills["Survival"];
            ret["deception"]        = skills["Deception"];
            ret["intimidation"]     = skills["Intimidation"];
            ret["performance"]      = skills["Performance"];
            ret["persuasion"]       = skills["Persuasion"];

            //other profs here

            return ret;
        }

        //Heal the character
        public bool Heal (int amount)
        {
            if (amount + currentHP > maxHP)
            {
                currentHP = maxHP;
            } else
            {
                currentHP += amount;
            }

            return true;
        }

        //modifyTempHP?

        //Makes a roll using the character's stats and proficiencies.
        public DiceResponse RollForCharacter (DiceCollection dc)
        {
            Dictionary<string, int> stats = this.GetGeneralStatistics();

            DiceResponse resp = dc.roll(stats);

            return resp;
        }

        //Saves the character and all it's abilities to a file.
        public bool Save (string filepath)
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
                string backupFilepath = backupFolder + name + "_" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".char";
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
        public bool SetTempHP (int amount, bool onlyIncrease = true) {
            if (amount > tempHP)
            {
                tempHP = amount;
                return true;
            } else
            {
                if (onlyIncrease)
                {
                    return true;
                } else
                {
                    tempHP = amount;
                    return true;
                }
            }
        }

        //Converts the character and all of it's stuff to a JSON string, presumably for saving to a file.
        public override string ToString()
        {
            string ret = JsonSerializer.Serialize<Character>(this);
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
            Ability ability = abilities[abilityNum];

            DiceResponse resp = ability.use(GetGeneralStatistics());

            IncorporateChanges(resp.changes);

            return resp;
        }


        //Private functions:
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Calculates the character's proficiency bonus and loads it into the prof variable.
        private void CalculateProf ()
        {
            int charLevel = GetCharacterLevel();

            prof = DiceFunctions.getProfForLevel(charLevel) + profBonus;
        }

        //Calculates the numeric index of the ability with the specified ID.
        private int GetAbilityIndexByID (string id)
        {
            string compare = id.ToLower();
            for (int a = 0; a < abilities.Count; a++)
            {
                string compare2 = abilities[a].getID().ToLower();
                if (compare == compare2)
                {
                    return a;
                }
            }

            //There is no such ability on this character.
            return -1;
        }

        //Takes in some changes and applies them to the character.
        private bool IncorporateChanges (Dictionary<string, string> changes)
        {
            //to do: consider having this return false when it can't parse something.

            //If there is an index in the dictionary, assume that's meant to be a change and set the corresponding attribute here to that value.
            //We will need to do some sanity checking, though (ie: current HP cannot be more than max HP).

            int tempint = 0;

            //Consider removing support for changing the character's name or ID.
            if (changes.ContainsKey ("id")) { id = changes["id"]; }
            if (changes.ContainsKey ("name")) { name = changes["name"]; }
            if (changes.ContainsKey ("race")) { race = changes["race"]; }

            if (changes.ContainsKey("maxHP") && Int32.TryParse(changes["maxHP"], out tempint))
            {
                maxHP = tempint;
            }
            if (changes.ContainsKey("currentHP") && Int32.TryParse(changes["currentHP"], out tempint))
            {
                currentHP = tempint;
            }

            //...

            return true;
        }

        //Sets everything to a zeroed out / default state.
        private void ZeroEverything()
        {
            //Basic info.
            id = "";
            name = "";
            race = "";
            maxHP = 0;
            currentHP = 0;

            //Class and level based info.
            classes = new List<ClassLevel>();
            profBonus = 0;

            //Stats and related info.
            stats = new int[6];
            saveProfs = new int[6];
            for (int a = 0; a < 6; a++)
            {
                stats[a] = 0;
                saveProfs[a] = 0;
            }
            skills = new Dictionary<string, int>();
            skills["Athletics"] = 0;
            skills["Acrobatics"] = 0;
            skills["Sleight of Hand"] = 0;
            skills["Stealth"] = 0;
            skills["Arcana"] = 0;
            skills["History"] = 0;
            skills["Investigation"] = 0;
            skills["Nature"] = 0;
            skills["Religion"] = 0;
            skills["Animal Handling"] = 0;
            skills["Insight"] = 0;
            skills["Medicine"] = 0;
            skills["Perception"] = 0;
            skills["Survival"] = 0;
            skills["Deception"] = 0;
            skills["Intimidation"] = 0;
            skills["Performance"] = 0;
            skills["Persuasion"] = 0;

            //Ability info
            abilities = new List<Ability>();
            basicAbilities = new List<Ability>();
            passives = new List<string>();
        }

        //Getters and Setters:
        //-------- -------- -------- -------- -------- -------- -------- -------- 

        //to do: how many of these are actually needed?
        //Get stats.
        public int GetStrength()
        {
            return this.stats[0];
        }
        public int GetStr()
        {
            return this.stats[0];
        }
        public int GetDexterity()
        {
            return this.stats[1];
        }
        public int GetDex()
        {
            return this.stats[1];
        }
        public int GetConstitution()
        {
            return this.stats[2];
        }
        public int GetCon()
        {
            return this.stats[2];
        }
        public int GetIntelligence()
        {
            return this.stats[3];
        }
        public int GetInt()
        {
            return this.stats[3];
        }
        public int GetWisdom()
        {
            return this.stats[4];
        }
        public int GetWis()
        {
            return this.stats[4];
        }
        public int GetCharisma()
        {
            return this.stats[5];
        }
        public int GetCha()
        {
            return this.stats[5];
        }
        public int[] GetStats()
        {
            return this.stats;
        }
        public int GetProf()
        {
            return (this.prof);
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
                return this.saveProfs[save];
            }
        }

        public List<Ability> GetAbilities ()
        {
            //to do: perhaps switch this over to something that just outputs data needed to render the things for the abilities.
            //then switch ability class over to internal only again.
            return this.abilities;
        }

        public List<Ability> GetBasicAbilities ()
        {
            return this.basicAbilities;
        }
    }
}