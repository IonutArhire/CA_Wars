using System.Collections.Generic;

namespace Services.Models
{
    public class InitialConfigModel
    {
        public InitialConfigModel(float[,] initialConfig, int playerId) 
        {
            this.InitialConfig = initialConfig;
            this.PlayerId = playerId;
        }

        public float[,] InitialConfig { get; private set; }
        public int PlayerId { get; private set; }
    }
}