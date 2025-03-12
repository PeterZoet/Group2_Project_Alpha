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
                Console.WriteLine("What would you like to do (enter a number)?\n");
                Console.WriteLine($"You are at: {player.CurrentLocation.Name}.");
                Console.WriteLine($"{player.CurrentLocation.Description}\n");
                
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
            Console.Clear();
            Monster baseMonster = player.CurrentLocation.MonsterLivingHere;

            List<Monster> monstersToFight = new List<Monster>
            {
                new Monster(baseMonster.ID, baseMonster.Name + "-1", baseMonster.MaximumHitPoints, baseMonster.CurrentHitPoints),
                new Monster(baseMonster.ID, baseMonster.Name+ "-2", baseMonster.MaximumHitPoints, baseMonster.CurrentHitPoints),
                new Monster(baseMonster.ID, baseMonster.Name+ "-3", baseMonster.MaximumHitPoints, baseMonster.CurrentHitPoints)
            };

            Console.WriteLine($"You stand face-to-face with three {baseMonster.Name}s!");

            for (int i = 0; i < monstersToFight.Count; i++)
            {
                Console.WriteLine($"{baseMonster.Name}-{i + 1}: {monstersToFight[i].CurrentHitPoints}HP");
            }

            Console.WriteLine("\nDo you want to engage in battle?");
            string choice = GetValidInput(["Y", "N"]);

            if (choice == "N")
            {
                Console.WriteLine("\nYou cautiously step back into the shadows, avoiding the fight for now...");

                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Clear();

            bool allMonstersAlive = monstersToFight.All(m => m.IsAlive());

            while (allMonstersAlive)
            {
                List<string> options = new();
                List<string> optionsToValidate = new();
                for (int i = 0; i < monstersToFight.Count; i++)
                {
                    if (monstersToFight[i].IsAlive())
                    {
                        options.Add($"{monstersToFight[i].Name} ({monstersToFight[i].CurrentHitPoints}HP)");
                        optionsToValidate.Add($"{i + 1}");
                    }
                }
                
                Console.Clear();
                Console.WriteLine($"The battle rages on! Which monster will you target? {string.Join(", ", options)}");

                int selectedMonsterIndex = int.Parse(GetValidInput(optionsToValidate)) - 1;

                Monster selectedMonster = monstersToFight[selectedMonsterIndex];

                Console.Clear();
                Console.WriteLine($"You move closer to {selectedMonster.Name}. ({selectedMonster.CurrentHitPoints}HP)");
                Console.WriteLine($"The foe's prepare to strike...");

                Console.WriteLine($"\nYou have {player.CurrentHitPoints}HP");

                Console.WriteLine($"\nBe aware the {baseMonster.Name}s will retaliate and deal between 1-{selectedMonster.MaximumDamage}HP of damage each!");
                Console.WriteLine($"Would you like to strike with your {player.CurrentWeapon.Name}? (1-{player.CurrentWeapon.MaximumDamage} dmg) \nIf not the fight will be reset!");
                string attackChoice = GetValidInput(["Y", "N"]);

                if (attackChoice == "N")
                {
                    Console.WriteLine("You fled from the fight! Come back when you're stronger.");
                    return;
                }

                int damage = player.CurrentWeapon.MaximumDamage;
                int damageDealt = selectedMonster.random.Next(1, damage + 1);
                int damageDealtByMonsters = 0;

                //player strikes first
                selectedMonster.TakeDamage(damageDealt);

                if (monstersToFight.All(m => !m.IsAlive())) //all monsters not alive
                {
                    break;
                }

                //all 3 monster that are alive attack the player
                Dictionary<string, int> monsterDamageLog = new();

                foreach (Monster monster in monstersToFight)
                {
                    if (monster.IsAlive())
                    {
                        int damageDealtByMonster = selectedMonster.random.Next(1, monster.MaximumDamage + 1);
                        monsterDamageLog[$"{monster.Name}"] = damageDealtByMonster;
                        damageDealtByMonsters += damageDealtByMonster;
                    }
                }
                //1 keer takedamage callen in plaats van 3 keer
                player.TakeDamage(monsterDamageLog.Values.Sum());
                
                Console.Clear();
                Console.WriteLine($"\nYou swing your {player.CurrentWeapon.Name} with all your might!");
                Console.WriteLine($"You hit {selectedMonster.Name}, dealing {damageDealt}HP damage! {(selectedMonster.IsAlive() ? $"It lives with {selectedMonster.CurrentHitPoints}HP left!" : "It collapses, defeated!")}");

                Console.WriteLine($"\nThe monsters retaliate, hurting you for {monsterDamageLog.Values.Sum()}HP in total!");
                
                foreach (var entry in monsterDamageLog)
                {
                    Console.WriteLine($"    -{entry.Key} dealt {entry.Value}HP of damage.");
                }

                Console.WriteLine($"\nYour current HP: {player.CurrentHitPoints}");

                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();

                if (!player.IsAlive())
                {
                    List<string> deathMessages = new List<string>
                    {
                        "\nYour vision blurs... A chilling numbness creeps over you... Then, nothing.",
                        "\nYou collapse to the ground, your strength fading... Darkness consumes you.",
                        "\nA sharp pain jolts through you—then silence. The battle is over... for you.",
                        "\nYour knees buckle. The last thing you see is your foe standing victorious...",
                        "\nA cold hand seems to grasp your soul as your body gives in to exhaustion...",
                        "\nYou fought bravely, but fate had other plans. The world fades to black...",
                        "\nYour breath grows shallow. A final whisper escapes your lips before everything ends...",
                        "\nThe weight of battle is too much. You fall, unable to continue...",
                        "\nAs your consciousness drifts away, a single thought lingers... ‘Is this the end?’",
                        "\nDarkness envelops you. Your journey... has come to an end."
                    };

                    // Select a random death message
                    Random rnd = new Random();
                    string deathMessage = deathMessages[rnd.Next(deathMessages.Count)];
                    Console.Clear();
                    Console.WriteLine(deathMessage);
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    restartGame();
                    return;
                }
            }
            Console.WriteLine("\n+----------------------+");
            Console.WriteLine($"All monsters defeated! You stand victorious amidst the fallen beasts!\nYou completed the quest {PlayerQuest.ActiveQuest!.Name}");
            Console.WriteLine("+----------------------+");

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
            string input = string.Empty;

            while(input != "2")
            {
                Console.Clear();
                Console.WriteLine("----------\nInventory:\n----------\n");
                foreach (Item item in player.Inventory)
                {
                    if (item is Weapon)
                    {
                        Weapon weapon = (Weapon)item;
                        Console.WriteLine($"- {item.Name} (Damage: 1-{weapon.MaximumDamage})");
                    }
                    else
                    {
                        Console.WriteLine($"- {item.Name}");
                    }
                }
            
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("1. Use an item");
                Console.WriteLine("2. Close inventory");
            
                input = GetValidInput(["1", "2"]);
                if (input == "1")
                {
                    UseItem();
                }
            }
            
        }
        private static void UseItem()
        {
            List<string> optionsList = new();

            Console.WriteLine("\nWhich item would you like to use?");
            for (int i = 0; i < player.Inventory.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {player.Inventory[i].Name}");
                optionsList.Add($"{i + 1}");
            }

            Console.WriteLine("X to cancel");
            optionsList.Add("X");
        
            string input = GetValidInput(optionsList);

            if (input == "X")
            {
                return;
            }
        
            if (int.TryParse(input, out int index) && index > 0 && index <= player.Inventory.Count)
            {
                Item item = player.Inventory[index - 1];

                Console.WriteLine($"You used {item.Name}.");
        
                player.Inventory.RemoveAt(index - 1);
            }
            
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        }
    }