using System;
using System.Collections.Generic;

namespace Services.PlayerResourcesService
{
    public class PlayerResourcesService
    {
        private static List<string> colors;
        private static List<int> numbers;

        private static List<Dictionary<string, object>> resources = new List<Dictionary<string, object>>();
        private static int _nrPlayers = 2;

        public static void Initialize() {
            colors = new List<string>();
            colors.Add("#f28910"); //orange
            colors.Add("#099fef"); //blue

            for (int i = 0; i < _nrPlayers; i++)
            {
                var color = getNewColor();

                var playersResources = new Dictionary<string, object>();
                playersResources.Add("color", color);

                resources.Add(playersResources);
            }
            
            InitializeNumbers();
        }

        private static string getNewColor() {
            Random randomizer = new Random();
            int idx = randomizer.Next(colors.Count);
            string result = colors[idx];
            colors.RemoveAt(idx);

            return result;
        }

        private static int getNewNumber() {
            Random randomizer = new Random();
            int idx = randomizer.Next(numbers.Count);
            int result = numbers[idx];
            numbers.RemoveAt(idx);

            if(numbers.Count == 0) {
                InitializeNumbers();
            }

            return result;
        }

        private static void InitializeNumbers() {
            numbers = new List<int>();
            for (int i = 0; i < _nrPlayers; i++)
            {
                numbers.Add(i);
            }
        }

        public static Dictionary<string, object> getPlayerResources() {
            var result = new Dictionary<string, object>();
            result.Add("allPlayersRes", resources);
            result.Add("number", getNewNumber());
            return result;
        }
    }
}