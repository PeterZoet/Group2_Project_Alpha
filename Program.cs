using entities;
namespace _2425_OP34_Group2_Project_Alpha
{
    class Program
    {
        static Player player = new(World.Weapons[0], World.Locations[0]);

        static void Main()
        {
            Console.WriteLine(World.Locations[0].Name);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Console.WriteLine($"Location to the north: {player.CurrentLocation.GetLocationAt("N").Name}");
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // DisplayWorldAttributes(); //dev-only
            bool gameRunning = true;

            while (gameRunning)
            {
                Console.WriteLine("What would you like to do (enter a number)?");
                Console.WriteLine($"You are at: {player.CurrentLocation.Name}.");
                
                List<string> options = new List<string>
                {
                    "See game stats",
                    "Move"
                };
                
                if (player.CurrentLocation.Killable)
                {
                    options.Add("Fight");
                }
                if (player.CurrentLocation.Interactable)
                {
                    options.Add("Interact");
                }
                options.Add("Quit");

                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine($" {i + 1}: {options[i]}");
                }

                List<string> optionNumbers = new List<string>();
                for (int i = 0; i < options.Count; i++)
                {
                    optionNumbers.Add((i + 1).ToString());
                }
                
                // Get valid input from the user.
                string option = GetValidInput(optionNumbers);
                int choice = int.Parse(option);

                gameRunning = HandleUserChoice(choice);
            }
        }


        private static bool HandleUserChoice(int option)
        {
            switch (option)
                {
                    case 1:
                        SeeGameStats();
                        break;
                    case 2:
                        Move();
                        break;
                    case 3:
                        if (player.CurrentLocation.Killable && !player.CurrentLocation.Interactable)
                        {
                            Fight();
                        }
                        else if (player.CurrentLocation.Interactable)
                        {
                            Interact();
                        }
                        break;
                    case 4:
                        if (player.CurrentLocation.Killable && player.CurrentLocation.Interactable)
                        {
                            Fight();
                        }
                        else
                        {
                            Console.WriteLine("--------------------\nSee you next time!\n--------------------");
                            return false;
                        }
                        break;
                    case 5:
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
        }

        private static void Fight()
        {
            Console.WriteLine("Fighting a monster if there is one...");
        }

        private static void Interact()
        {
            Console.WriteLine("Interacting with something...");
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
