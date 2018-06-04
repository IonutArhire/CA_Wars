namespace Services.Models
{
    public class PlayerModel
    {
        public PlayerModel(string color, int wins) 
        {
            this.Color = color;
            this.Wins = wins;
        }

        public string Color { get; private set; }

        public int Wins { get; private set; }
    }
}