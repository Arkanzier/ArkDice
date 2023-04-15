using System.Reflection.PortableExecutable;

namespace ArkDice
{
    public struct DiceResponse
    {
        //A struct for sending back information about die rolls.

        //Attributes:
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Indicates success/failure.
        public bool Success { get; set; }

        //Indicates the total of the roll.
        public int Total { get; set; }

        //A Human-readable description of the roll to potentially display to the user.
        public string Description { get; set; }

        //If we need to make any changes to the Character instance that triggered the dice rolls, put them here.
        public Dictionary<string, string> Changes { get; set; }

        //Indicates nat 1/20, if present.

        //Array of rolls, in case they're relevant?

        //...


        //Constructor(s):
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Blank slate.
        public DiceResponse()
        {
            Success = false;
            Total = 0;
            Description = "";
            Changes = new Dictionary<string, string>();
        }

        //Just success/failure.
        public DiceResponse(bool success)
            : this()
        {
            this.Success = success;
        }

        //Just success/failure and a message.
        public DiceResponse(bool success, string description)
            :this ()
        {
            this.Success = success;
            this.Description = description;
        }

        //Everything but changes.
        public DiceResponse(bool success, int total, string description)
            : this ()
        {
            this.Success = success;
            this.Total = total;
            this.Description = description;
        }

        //Everything.
        public DiceResponse(bool success, int total, string description, Dictionary<string, string> changes)
        {
            this.Success = success;
            this.Total = total;
            this.Description = description;
            this.Changes = changes;
        }
    }
}