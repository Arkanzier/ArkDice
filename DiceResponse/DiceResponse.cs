using System.Reflection.PortableExecutable;

namespace ArkDice
{
    public struct DiceResponse
    {
        //A struct for sending back information about die rolls.

        //Attributes:
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Indicates success/failure.
        public bool success { get; set; }

        //Indicates the total of the roll.
        public int total { get; set; }

        //A Human-readable description of the roll to potentially display to the user.
        public string description { get; set; }

        //If we need to make any changes to the Character instance that triggered the dice rolls, put them here.
        public Dictionary<string, string> changes { get; set; }

        //Indicates nat 1/20, if present.
        //...


        //Constructor(s):
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Blank slate.
        public DiceResponse()
        {
            success = false;
            total = 0;
            description = "";
            changes = new Dictionary<string, string>();
        }

        //Just success/failure.
        public DiceResponse(bool success)
        {
            this.success = success;
            total = 0;
            description = "";
            changes = new Dictionary<string, string>();
        }

        //Just success/failure and a message.
        public DiceResponse(bool success, string description)
        {
            this.success = success;
            total = 0;
            this.description = description;
            changes = new Dictionary<string, string>();
        }

        //Everything but changes.
        public DiceResponse(bool success, int total, string description)
        {
            this.success = success;
            this.total = total;
            this.description = description;
            changes = new Dictionary<string, string>();
        }

        //Everything.
        public DiceResponse(bool success, int total, string description, Dictionary<string, string> changes)
        {
            this.success = success;
            this.total = total;
            this.description = description;
            this.changes = changes;
        }
    }
}