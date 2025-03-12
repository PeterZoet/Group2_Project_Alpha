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
            Console.WriteLine("\nCompleted Quests:");
            foreach (Quest quest in PlayerQuest.CompletedQuests)
            {
                Console.WriteLine(quest.Name);
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
            Monster baseMonster = player.CurrentLocation.MonsterLivingHere;

            List<Monster> monstersToFight = new List<Monster>
            {
                new Monster(baseMonster.ID, baseMonster.Name, baseMonster.MaximumHitPoints, baseMonster.CurrentHitPoints),
                new Monster(baseMonster.ID, baseMonster.Name, baseMonster.MaximumHitPoints, baseMonster.CurrentHitPoints),
                new Monster(baseMonster.ID, baseMonster.Name, baseMonster.MaximumHitPoints, baseMonster.CurrentHitPoints)
            };

            Console.WriteLine($"\nYou are about to fight three {baseMonster.Name}s!");

            for (int i = 0; i < monstersToFight.Count; i++)
            {
                Console.WriteLine($"Monster {i + 1}: HP {monstersToFight[i].CurrentHitPoints}");
            }

            Console.WriteLine("\nDo you want to attack them? (Y/N)");
            string choice = GetValidInput(["Y", "N"]);

            if (choice == "N")
            {
                return;
            }

            Console.Clear();

            while (monstersToFight.Any(m => m.IsAlive()) && player.IsAlive())
            {
                Console.WriteLine($"Which monster do you want to attack? (1: {monstersToFight[0].Name} ({monstersToFight[0].CurrentHitPoints}), 2: {monstersToFight[1].Name} ({monstersToFight[0].CurrentHitPoints}), or 3: {monstersToFight[2].Name} ({monstersToFight[0].CurrentHitPoints}),)");
                int selectedMonsterIndex = int.Parse(GetValidInput([ "1", "2", "3"])) - 1;
                Monster selectedMonster = monstersToFight[selectedMonsterIndex];

                if (!selectedMonster.IsAlive())
                {
                    Console.WriteLine("That monster is already defeated! Choose another.");
                    continue;
                }

                Console.WriteLine($"Would you like to attack the {selectedMonster.Name} with your {player.CurrentWeapon.Name}? (Y/N)");
                Console.WriteLine($"The {selectedMonster.Name} will attack you in damage range 1-{selectedMonster.MaximumDamage}");
                string attackChoice = GetValidInput(["Y", "N"]);

                if (attackChoice == "N")
                {
                    Console.WriteLine("You fled from the fight! Come back when you're stronger.");
                    return;
                }

                int damage = player.CurrentWeapon.MaximumDamage;
                int damageDealt = selectedMonster.random.Next(1, damage + 1);
                int damageToPlayer = selectedMonster.Attack();

                player.TakeDamage(damageToPlayer);
                selectedMonster.TakeDamage(damageDealt);

                Console.WriteLine($"\nMonster {selectedMonsterIndex + 1} HP: {selectedMonster.CurrentHitPoints}");
                Console.WriteLine($"Damage dealt: {damageDealt} HP");
                Console.WriteLine($"\nYour HP: {player.CurrentHitPoints}");
                Console.WriteLine($"Monster hurt you for: {damageToPlayer} HP");

                if (!player.IsAlive())
                {
                    restartGame();
                    return;
                }
            }

            Console.WriteLine("\nAll monsters defeated!");
            PlayerQuest.CompleteQuest(PlayerQuest.ActiveQuest);
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }


        private static void restartGame()
        {
            player.CurrentLocation = World.Locations[0];
            PlayerQuest.CompletedQuests = null;
        }

        private static void TalkToWitch(Witch witch)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Talking to {witch.Name} The Witch...");

                Console.WriteLine("\nAh, a weary traveler! The winds whisper of your journey... let me mend your wounds and share my wares for just a few coins.\n");

                Console.WriteLine($"Health Points: {player.CurrentHitPoints}/{player.MaximumHitPoints}");
                Console.WriteLine($"Coin Balance: {player.Coins} 🪙\n");
                
                Console.WriteLine("1: Small Heal (30HP) - Free");
                Console.WriteLine("2: Medium heal (60HP) - 2 🪙");
                Console.WriteLine("3: Full Heal - 4 🪙");
                Console.WriteLine("4: View Shop");
                Console.WriteLine("5: Return");

                string choice = GetValidInput(["1", "2", "3", "4", "5"]);
                switch (choice)
                {
                    case "1":
                        // Small Heal
                        witch.HealToAmount(player, 30, 0);
                        break;
                    case "2":
                        witch.HealToAmount(player, 60, 2);
                        break;
                    case "3":
                        witch.FullHeal(player, 4);
                        break;
                    case "4":
                        OpenShop(witch);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, press Enter to continue...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private static void OpenShop(Witch witch)
        {
            Console.WriteLine("Witch shop");
            Thread.Sleep(2000); // Pause application for 2 sec (Remove later: using for testing purposes)
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
                Console.WriteLine($"\n-------------\nEnter a valid option: {string.Join(", ", validOptions)}");
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
        
            string input = GetValidInput(["1", "2"]);
        
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
            List<string> optionString = new();

            Console.WriteLine("Which item would you like to use?");
            for (int i = 0; i < player.Inventory.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {player.Inventory[i].Name}");
                optionString.Add($"{i + 1}");
            }
        
            string input = GetValidInput([]);
        
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