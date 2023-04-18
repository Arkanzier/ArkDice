namespace Spell
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
            //look up the spell by ID within a db of spells, then load it's info.
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

        public Spell (string id, string name)
            : this()
        {
            this.ID = id;
            this.Name= name;
        }

        //
        //-------- -------- -------- -------- -------- -------- -------- -------- 
    }
}