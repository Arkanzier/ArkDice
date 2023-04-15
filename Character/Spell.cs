namespace Spell
{
    //Stores one spell.
    public class Spell
    {
        //Basic information
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //An identifier for behind-the-scenes use.
        public string id { get; private set; }
        //The name to display to the user.
        public string name { get; private set; }
        //The spell's level, 0 for cantrip.
        public int level { get; private set; }

        //Components
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Whether the spell has a vocal component.
        public bool vocal { get; private set; }
        //Whether the spell has a somatic component.
        public bool somatic { get; private set; }
        //Whether the spell has a material component.
        public bool material { get; private set; }
        //to do: consider switching this to something other than a bool.
        //or use this for no-price material components only?

        //Basic Information
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Which type of action the spell takes.
        public string action { get; private set; }
        //Proper description
        public string description { get; private set; }
        //What benefit, if any, the spell gets for being cast at a higher level.
        //Empty string for none.
        public string upcastingBenefit { get; private set; }
        //The spell's range.
        public string range { get; private set; }
        //Duration in rounds.
        public int duration { get; private set; }
        //or do i want to make this a string and just let the player deal with it?
        //Whether the spell requires concentration.
        public bool concentration { get; private set; }

        //Meta information
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Which book the spell is from.
        public string book { get; private set; }
        //Which page number of the book, if appropriate. 0 for none.
        public int page { get; private set; }

        //to add:
        //something to store dice collections + descriptors for them.
            //ie: for Fireball, store 8d6 with the descriptor "Fire damage"


        //Constructors
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        public Spell()
        {
            id = "";
            name = "";
            level = 0;
            vocal = false;
            somatic = false;
            material = false;
            action = "";
            description = "";
            upcastingBenefit = "";
            range = "";
            duration = 0;
            concentration = false;
            book = "";
            page = 0;
        }

        public Spell(string id, string name, int level, bool vocal, bool somatic, bool material, string action, string description, string upcastingBenefit, string range, int duration, bool concentration = false, string book = "", int page = 0)
        {
            //to do: decide how many of these i want to make optional
            this.id = id;
            this.name = name;
            this.level = level;
            this.vocal = vocal;
            this.somatic = somatic;
            this.material = material;
            this.action = action;
            this.description = description;
            this.upcastingBenefit = upcastingBenefit;
            this.range = range;
            this.duration = duration;
            this.concentration = concentration;
            this.book = book;
            this.page = page;
        }

        public Spell (string id, string name)
            : this()
        {
            this.id = id;
            this.name= name;
        }
    }
}