using System;
using System.Collections.Generic;

namespace Services.PlayerResourcesService
{
    public class PlayerResourcesService
    {
        private static List<string> _colors;
        private static List<int> _numbers;

        private static List<Dictionary<string, object>> _resources = new List<Dictionary<string, object>>();
        private static int _nrPlayers = 2;

        private static string getNewColor() {
            Random randomizer = new Random();
            int idx = randomizer.Next(_colors.Count);
            string result = _colors[idx];
            _colors.RemoveAt(idx);

            return result;
        }

        private static void InitializeNumbers() {
            _numbers = new List<int>();
            for (int i = 0; i < _nrPlayers; i++)
            {
                _numbers.Add(i);
            }
        }

        private static int getNewNumber() {
            Random randomizer = new Random();
            int idx = randomizer.Next(_numbers.Count);
            int result = _numbers[idx];
            _numbers.RemoveAt(idx);

            if(_numbers.Count == 0) {
                InitializeNumbers();
            }

            return result;
        }

        public static void Initialize() {
            _colors = new List<string>();
            _colors.Add("#f28910"); //orange
            _colors.Add("#099fef"); //blue

            for (int i = 0; i < _nrPlayers; i++)
            {
                var color = getNewColor();

                var playersResources = new Dictionary<string, object>();
                playersResources.Add("color", color);

                _resources.Add(playersResources);
            }
            
            InitializeNumbers();
        }
        
        public static Object getPlayerResources() {
            return new { allPlayersRes = _resources, number = getNewNumber() };
        }
    }
}