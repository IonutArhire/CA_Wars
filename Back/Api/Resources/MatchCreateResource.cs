namespace Api.Resources
{
    public class MatchCreateResource
    {
        public int NrPlayers { get; set; }

        public string RuleSet { get; set; }

        public int MaxIters { get; set; }

        public int Rows { get; set; }

        public int Cols { get; set; }
    }
}