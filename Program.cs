using entities;
namespace _2425_OP34_Group2_Project_Alpha
{
    class Program
    {
        static public Player player = new(World.Weapons[0], World.Locations[0]);

        static void Main()
        {
            bool gameRunning = true;

            while (gameRunning)
            {
                Console.Clear();
                Console.WriteLine("What would you like to do (enter a number)?");
                Console.WriteLine($"You are at: {player.CurrentLocation.Name}.");
                Console.WriteLine(player.CurrentLocation.Description);
                
                List<string> options = new List<string>
                {
                    "See game stats",
                    "Move",
                    "Open inventory"
                };
                
                if (player.CurrentLocation.Killable && player._previousLocation.QuestAvailableHere == PlayerQuest.ActiveQuest)
                {
                    options.Add("Fight");
                }
                
                // If current location has a witch npc
                if (player.CurrentLocation.WitchAvailableHere != null)
                {
                    options.Add("Talk to Witch");
                }
                 
                Quest locationQuest = player.CurrentLocation.QuestAvailableHere;
                if (locationQuest != null &&  (PlayerQuest.ActiveQuest == null) && !PlayerQuest.CompletedQuests.Contains(locationQuest))
                {
                    options.Add("Start Quest");
                }
                
                if (locationQuest != null && (PlayerQuest.ActiveQuest != null))
                {
                    options.Add("Flee Quest");
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

                gameRunning = HandleUserChoice(choice, options);
            }
        }


        private static bool HandleUserChoice(int choice, List<string> options)
        {
            string selectedOption = options[choice - 1];
            
            switch (selectedOption)
            {
                case "See game stats":
                    SeeGameStats();
                    break;
                case "Move":
                    Move();
                    break;
                case "Fight":
                    Fight();
                    break;
                case "Start Quest":
                    PlayerQuest.StartQuest(player.CurrentLocation.QuestAvailableHere);
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                    break;
                case "Flee Quest":
                    PlayerQuest.FleeQuest(player.CurrentLocation.QuestAvailableHere);
                    break;
                case "Open inventory":
                    OpenInventory();
                    break;
                case "Talk to Witch":
                    TalkToWitch(player.CurrentLocation.WitchAvailableHere);
                    break;
                case "Quit":
                    Console.WriteLine("--------------------\nSee you next time!\n--------------------");
                    return false;
            }
            return true;
        }

        private static void SeeGameStats()
        {
            Console.Clear();
            Console.WriteLine("Displaying stats...");
            Console.WriteLine($"HP: {player.CurrentHitPoints}/{player.MaximumHitPoints}");
            Console.WriteLine($"Weapon: {player.CurrentWeapon.Name} (Max Damage: {player.CurrentWeapon.MaximumDamage})");
            
            // Display active quest
            Console.WriteLine("\nActive Quests:");
            if (PlayerQuest.ActiveQuest == null)
            {
                Console.WriteLine("None");
            }
            else
            {
                Console.WriteLine($"- {PlayerQuest.ActiveQuest?.Name}: {PlayerQuest.ActiveQuest?.Description}");                
            }

            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
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
            Console.Clear();
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
            Monster monsterToFight = player.CurrentLocation.MonsterLivingHere;
            Console.WriteLine($"\nYou are about to fight a: {player.CurrentLocation.MonsterLivingHere.Name}");
            Console.WriteLine($"\nBoss HP: {monsterToFight.CurrentHitPoints}");
            Console.WriteLine("\nDo you want to attack it?\n");
            string choice = GetValidInput(["Y","N"]);

            if (choice == "N")
            {
                return;
            }

            Console.Clear();

            while (monsterToFight.IsAlive())
            {
                Console.WriteLine($"Would you like to attack the {monsterToFight.Name} with your {player.CurrentWeapon.Name}?");
                string conChoiche = GetValidInput(["Y","N"]);

                if (conChoiche == "N")
                {
                    Console.WriteLine($"You fled from the fight, come back if you are stronger/healed!");
                    return;
                }

                int Attack = player.CurrentWeapon.MaximumDamage;
                int damage = monsterToFight.random.Next(1, Attack + 1);
                int damageToPlayer = monsterToFight.Attack();

                player.TakeDamage(damageToPlayer);
                monsterToFight.TakeDamage(damage);


                Console.WriteLine($"\nBoss HP: {monsterToFight.CurrentHitPoints}");
                Console.WriteLine($"Damage done to boss: {damage}hp");
                Console.WriteLine($"\nYour HP: {player.CurrentHitPoints}");
                Console.WriteLine($"boss hurt you for: {damageToPlayer}hp");
            }
            PlayerQuest.CompleteQuest(PlayerQuest.ActiveQuest);
            Console.WriteLine("\nBoss defeated!");
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
            
        }

        private static void TalkToWitch(Witch witch)
        {
            witch.TalkToPlayer(player);
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
        private static void OpenInventory()
        {
            Console.WriteLine("Inventory:");
            foreach (Item item in player.Inventory)
            {
                if (item is Weapon)
                {
                    Weapon weapon = (Weapon)item;
                    Console.WriteLine($"{weapon.ID}. {item.Name} - (Damage: {weapon.MaximumDamage})");
                }
                else
                {
                    Console.WriteLine($"{item.Name}");
                }
            }
        
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1. Use an item");
            Console.WriteLine("2. Close inventory");
        
            string input = Console.ReadLine();
        
            switch (input)
            {
                case "1":
                    UseItem();
                    break;
                case "2":
                    break;
                default:
                    Console.WriteLine("Invalid input. Please try again.");
                    OpenInventory();
                    break;
            }
        }
        private static void UseItem()
        {
            Console.WriteLine("Which item would you like to use?");
            for (int i = 0; i < player.Inventory.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {player.Inventory[i].Name}");
            }
        
            string input = Console.ReadLine();
        
            if (int.TryParse(input, out int index) && index > 0 && index <= player.Inventory.Count)
            {
                Item item = player.Inventory[index - 1];

                Console.WriteLine($"You used {item.Name}.");
        

                player.Inventory.RemoveAt(index - 1);
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again.");
                UseItem();
            }
        }

        }
    }