﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Character
{
    //Stores one spell.
    public class Spell
    {
        //Basic information
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //An identifier for behind-the-scenes use.
        public string ID { get; set; }
        //The name to display to the user.
        public string Name { get; set; }
        //The spell's level, 0 for cantrip.
        public int Level { get;  set; }
        //The spell's school
        public string School { get; set; }
        //Whether the spell is prepared/known or not.
        public bool Prepared { get; set; }

        //Components
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Whether the spell has a vocal component.
        public bool Vocal { get; set; }
        //Whether the spell has a somatic component.
        public bool Somatic { get; set; }
        //Whether the spell has a material component with no cost listed.
        public bool Material { get; set; }
        //Whether the spell has an expensive material component.
        public string ExpensiveMaterial { get; set; }

        //Basic Information
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Which type of action the spell takes.
        public string Action { get; set; }
        //Proper description
        public string Description { get; set; }
        //What benefit, if any, the spell gets for being cast at a higher level.
        //Empty string for none.
        public string UpcastingBenefit { get; set; }
        //The spell's range.
        public string Range { get; set; }
        //Duration in rounds.
        public int Duration { get; set; }
        //or do i want to make this a string and just let the player deal with it?
        //Whether the spell requires concentration.
        public bool Concentration { get; set; }

        //Meta information
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Which book the spell is from.
        public string Book { get; set; }
        //Which page number of the book, if appropriate. 0 for none.
        public int Page { get; set; }


        //Constructors
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        [JsonConstructor]
        public Spell()
        {
            ID = "";
            Name = "";
            Level = 0;
            School = "";
            Prepared = false;
            Vocal = false;
            Somatic = false;
            Material = false;
            ExpensiveMaterial = "";
            Action = "";
            Description = "";
            UpcastingBenefit = "";
            Range = "";
            Duration = 0;
            Concentration = false;
            Book = "";
            Page = 0;
        }

        //This one just loads the ID, for use with things like LoadFromFile().
        public Spell(string id)
            : this()
        {
            ID = id;
        }

        //These three load from either the spell library, a file, or both.
        public Spell(string id, string folderpath)
            : this()
        {
            ID = id;
            //We'll attempt to load this spell from the library on disk.
            string filepath = folderpath + id + ".dat";

            if (folderpath.Last() != '\\')
            {
                folderpath += "\\";
            }
            folderpath += "dat\\spells\\";
            this.LoadFromFile(folderpath);
        }
        public Spell (string id, Dictionary<string, Spell> library)
            : this()
        {
            //We attempt to load the spell from the library.
            if (library.ContainsKey (id))
            {
                this.Copy (library[id]);
            }
        }
        public Spell(string id, Dictionary<string, Spell> library, string folderpath)
            : this()
        {
            //We attempt to load the spell from the library.
            if (library.ContainsKey(id))
            {
                if (this.Copy(library[id]))
                {
                    return;
                }
            }

            //Check if there's a file for this spell specifically.
            if (folderpath.Last() != '\\')
            {
                folderpath += "\\";
            }
            folderpath += "dat\\spells\\";
            this.LoadFromFile(folderpath);
        }

        public Spell(string id, string name, int level, bool vocal, bool somatic, bool material, string action, string description, string upcastingBenefit, string range, int duration, bool concentration = false, string book = "", int page = 0)
            : this()
        {
            this.ID = id;
            this.Name = name;
            this.Level = level;
            this.Vocal = vocal;
            this.Somatic = somatic;
            this.Material = material;
            //ExpensiveMaterial
            this.Action = action;
            this.Description = description;
            this.UpcastingBenefit = upcastingBenefit;
            this.Range = range;
            this.Duration = duration;
            this.Concentration = concentration;
            this.Book = book;
            this.Page = page;
        }

        public Spell (JsonElement json)
            : this()
        {
            //Convert the JSON to a Spell object.
            var deserializerOptions = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };

            Spell? deserialized = JsonSerializer.Deserialize<Spell>(json, deserializerOptions);

            if (deserialized != null)
            {
                this.Copy(deserialized);
            }
        }

        //Public functions
        //-------- -------- -------- -------- -------- -------- -------- -------- 

        //Used for sorting.
        //Arguments:
            //other: another Spell object to compare against.
            //column: which attribute of the Spell class should be used to compare the two.
        //Return values:
            //If this is to appear before the other one, returns -1.
            //If this is to appear after the other one, returns 1.
            //If this is equal to the other one, including with tiebreakers, returns 0.
            //Ties are broken by Level, then Name.
        //Note: for the boolean columns, false will be considered to be less than true.
        public int Compare(Spell other, string column = "level")
        {
            column = column.ToLower();
            int comparison;

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
                case "level":
                    if (Level < other.Level)
                    {
                        return -1;
                    }
                    else if (Level > other.Level)
                    {
                        return 1;
                    }
                    else
                    {
                        //We need to do more comparisons.
                        break;
                    }
                case "school":
                    comparison = String.Compare(this.School, other.School, StringComparison.OrdinalIgnoreCase);
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
                case "vocal":
                    if (!Vocal && other.Vocal)
                    {
                        return -1;
                    }
                    else if (Vocal && !other.Vocal)
                    {
                        return 1;
                    } else
                    {
                        //We need to do more comparisons.
                        break;
                    }
                case "somatic":
                    if (!Somatic && other.Somatic)
                    {
                        return -1;
                    }
                    else if (Somatic && !other.Somatic)
                    {
                        return 1;
                    }
                    else
                    {
                        //We need to do more comparisons.
                        break;
                    }
                case "material":
                    if (!Material && other.Material)
                    {
                        return -1;
                    }
                    else if (Material && !other.Material)
                    {
                        return 1;
                    }
                    else
                    {
                        //We need to do more comparisons.
                        break;
                    }
                case "expensivematerial":
                    comparison = String.Compare(this.ExpensiveMaterial, other.ExpensiveMaterial, StringComparison.OrdinalIgnoreCase);
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
                case "materialcolumn":
                    //to do: just sort alphabetically?
                    break;
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
                case "upcastingbenefit":
                    comparison = String.Compare(this.UpcastingBenefit, other.UpcastingBenefit, StringComparison.OrdinalIgnoreCase);
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
                case "range":
                    comparison = String.Compare(this.Range, other.Range, StringComparison.OrdinalIgnoreCase);
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
                case "duration":
                    if (Duration < other.Duration)
                    {
                        return -1;
                    }
                    else if (Duration > other.Duration)
                    {
                        return 1;
                    }
                    else
                    {
                        //We need to do more comparisons.
                        break;
                    }
                case "concentration":
                    if (!Concentration && other.Concentration)
                    {
                        return -1;
                    }
                    else if (Concentration && !other.Concentration)
                    {
                        return 1;
                    }
                    else
                    {
                        //We need to do more comparisons.
                        break;
                    }
                case "book":
                    comparison = String.Compare(this.Book, other.Book, StringComparison.OrdinalIgnoreCase);
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
                case "page":
                    if (Page < other.Page)
                    {
                        return -1;
                    }
                    else if (Page > other.Page)
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

            //Break ties by level.
            if (Level < other.Level)
            {
                return -1;
            }
            else if (Level > other.Level)
            {
                return 1;
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

            //If we get here, we weren't able to see a difference between these two.
            return 0;
        }

        //Copies the contents of another spell into this one.
        public bool Copy (Spell other)
        {
            //Check for a few key fields and return false if they're not present?
            
            ID = other.ID;
            Name = other.Name;
            Level = other.Level;
            School = other.School;
            Prepared = other.Prepared;
            Vocal = other.Vocal;
            Somatic = other.Somatic;
            Material = other.Material;
            ExpensiveMaterial = other.ExpensiveMaterial;
            Action = other.Action;
            Description = other.Description;
            UpcastingBenefit = other.UpcastingBenefit;
            Range = other.Range;
            Duration = other.Duration;
            Concentration = other.Concentration;
            Book = other.Book;
            Page = other.Page;

            return true;
        }

        //Checks if the spell has been loaded.
        public bool IsLoaded ()
        {
            if (ID == "" || Name == "")
            {
                return false;
            }

            //If we get here, it's probably been loaded.
            return true;
        }

        //Attempts to load this spell from a file.
        //This is it's own function so that we can call it later and get a boolean response.
        public bool LoadFromFile (string folderpath)
        {
            char lastchar = folderpath.Last();
            if (lastchar != '\\')
            {
                folderpath += "\\";
            }
            string filepath = folderpath + ID;

            //Check if the file even exists.
            if (!File.Exists (filepath))
            {
                return false;
            }

            //Attempt to load the file.
            string jsonstring = File.ReadAllText (filepath);

            //Convert the JSON to a Spell object.
            var deserializerOptions = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };

            Spell? deserialized = JsonSerializer.Deserialize<Spell>(jsonstring, deserializerOptions);

            if (deserialized == null)
            {
                return false;
            }

            this.Copy(deserialized);

            return true;
        }
    }
}