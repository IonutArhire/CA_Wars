using System.Collections.Generic;

namespace Services.Models
{
    public class GameResultModel
    {
        public GameResultModel(List<float[,]> generations, int winner) 
        {
            this.Generations = generations;
            this.Winner = winner;
        }

        public List<float[,]> Generations { get; set; }

        public int Winner { get; set; }
    }
}