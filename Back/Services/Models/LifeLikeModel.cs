using System.Collections.Generic;

namespace Services.Models
{
    public class LifeLikeModel
    {
        public LifeLikeModel(string name, List<byte> forSurvival, List<byte> forBirth, string character) 
        {
            this.Name = name;
            this.ForSurvival = forSurvival;
            this.ForBirth = forBirth;
            this.Character = character;
        }

        public string Name { get; set; }

        public List<byte> ForSurvival { get; set; }

        public List<byte> ForBirth { get; set; }

        public string Character { get; set; }
    }
}