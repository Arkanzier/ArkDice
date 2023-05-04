using System.Text.Json;
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

        //to add:
        //something to store dice collections + descriptors for them.
        //ie: for Fireball, store 8d6 with the descriptor "Fire damage"


        //Constructors
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        [JsonConstructor]
        public Spell()
        {
            ID = "";
            Name = "";
            Level = 0;
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
        public Spell(string id)
            : this()
        {
            ID = id;
        }
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
            //to do: decide how many of these i want to make optional
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

        //Add one that takes a json string?

        //
        //-------- -------- -------- -------- -------- -------- -------- -------- 

        public bool Copy (Spell other)
        {
            //Check for a few key fields and return false if they're not present?
            
            ID = other.ID;
            Name = other.Name;
            Level = other.Level;
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

        //Attempts to load this spell from a file.
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