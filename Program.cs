using entities;
namespace _2425_OP34_Group2_Project_Alpha
{
    class Program
    {
        static Player player = new(World.Weapons[0], World.Locations[0]);

        static void Main()
        {
            // DisplayWorldAttributes(); //dev-only
            bool gameRunning = true;

            while (gameRunning)
            {
                Console.WriteLine("What would you like to do (enter a number)?");
                Console.WriteLine(" 1: See game stats \n 2: Move \n 3: Fight \n 4: Quit\n");

                string option = GetValidInput(["1", "2", "3", "4"]);

                gameRunning = HandleUserChoice(option);
            }
        }

        private static bool HandleUserChoice(string option)
        {
            switch (option)
            {
                case "1":
                    SeeGameStats();
                    break;
                case "2":
                    Move();
                    break;
                case "3":
                    Fight();
                    break;
                case "4":
                    Console.WriteLine("--------------------\nSee you next time!\n--------------------");
                    return false;
            }
            return true;
        }

        private static void SeeGameStats()
        {
            Console.WriteLine("Displaying stats...");
        }

        private static void Move()
        {
            /* The map
             * +------+
             * |  p   |
             * |  AW  |
             * |VFTGBS|
             * |  H   |
             * +------+
            */
            Console.WriteLine("\nWhere would you like to go? (N/E/S/W)");
            Console.WriteLine($"You are at: {player.CurrentLocation.Name}.");
            player.CurrentLocation.Compass();
            
            foreach (string direction in new[] { "N", "E", "S", "W" })
            {
                Location? location = player.CurrentLocation.GetLocationAt(direction);
                if (location != null)
                {
                    Console.WriteLine($"Location to the {direction}: {location.Name}");
                }
            } 
            Console.WriteLine();

            string movingTo = GetValidInput(["N", "E", "S", "W"]);
            player.MoveTo(player.CurrentLocation.GetLocationAt(movingTo));

            Console.WriteLine($"\nYou are now at: {player.CurrentLocation.GetLocationAt(movingTo)?.Name} \n");
        }

        private static void Fight()
        {
            Console.WriteLine("Fighting a monster if there is one...");
        }
    
        private static bool ValidateInput(string? input, List<string> options)
        {
            return input != null && options.Contains(input);
        }

        private static string GetValidInput(List<string> validOptions)
        {
            string? input;
            do
            {
                Console.WriteLine($"Enter a valid option: {string.Join(", ", validOptions)}");
                input = Console.ReadLine()?.ToUpper();
            } while (!ValidateInput(input, validOptions));

            return input!;
        }
        
        public static void DisplayWorldAttributes()
        {
            Console.WriteLine("\nAvailable Weapons:");
            foreach (var weapon in World.Weapons)
            {
                Console.WriteLine($"{weapon.ID}: {weapon.Name} (Damage: {weapon.MaximumDamage})");
            }

            Console.WriteLine("\nAvailable Locations:\n");
            foreach (var location in World.Locations)
            {
                Console.WriteLine($"{location.ID}: {location.Name} - {location.Description}");
                Console.WriteLine($"   Quest: {(location.QuestAvailableHere != null ? $"{location.QuestAvailableHere.Name} - id {location.QuestAvailableHere.ID}" : "None")}");
                Console.WriteLine($"   Monster: {(location.MonsterLivingHere != null ? $"{location.MonsterLivingHere.Name} - id {location.MonsterLivingHere.ID}" : "None")}\n");
            }

        }
    }
}
