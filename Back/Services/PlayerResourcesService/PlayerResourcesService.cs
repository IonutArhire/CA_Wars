using System;
using System.Collections.Generic;

namespace Services.PlayerResourcesService
{
    public class PlayerResourcesService
    {
        private static List<string> colors;
        private static List<int> numbers;

        public static void Initialize() {
            colors = new List<string>();
            colors.Add("#f28910"); //orange
            colors.Add("#099fef"); //blue

            numbers = new List<int>();
            numbers.Add(1);
            numbers.Add(2);
        }

        private static string getNewColor() {
            Random randomizer = new Random();
            int idx = randomizer.Next(colors.Count);
            string result = colors[idx];
            colors.RemoveAt(idx);

            if(colors.Count == 0) {
                Initialize();
            }

            return result;
        }

        private static int getNewNumber() {
            Random randomizer = new Random();
            int idx = randomizer.Next(numbers.Count);
            int result = numbers[idx];
            numbers.RemoveAt(idx);

            if(numbers.Count == 0) {
                Initialize();
            }

            return result;
        }

        public static Dictionary<string, object> getPlayerResources() {
            var result = new Dictionary<string, object>();
            result.Add("color", getNewColor());
            result.Add("number", getNewNumber());
            return result;
        }
    }
}