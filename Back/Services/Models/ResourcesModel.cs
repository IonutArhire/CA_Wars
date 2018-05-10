namespace Services.Models
{
    public class ResourcesModel
    {
        public ResourcesModel(GameModel game, int assignedNumber) 
        {
            this.Game = game;
            this.AssignedNumber = assignedNumber;
        }

        public GameModel Game { get; set; }

        public int AssignedNumber { get; set; }
    }
}