using entities;
namespace _2425_OP34_Group2_Project_Alpha
{
    public class Program
    {
        static public Player player = new(World.Weapons[0], World.Locations[0]);

        static void Main()
        {
            Console.Title = "Project Alpa: a simple rpg game";

            // ASCII Art Title
            string title = @"

 _____ _                 _        ____________ _____ 
/  ___(_)               | |       | ___ \ ___ \  __ \
\ `--. _ _ __ ___  _ __ | | ___   | |_/ / |_/ / |  \/
 `--. \ | '_ ` _ \| '_ \| |/ _ \  |    /|  __/| | __ 
/\__/ / | | | | | | |_) | |  __/  | |\ \| |   | |_\ \
\____/|_|_| |_| |_| .__/|_|\___|  \_| \_\_|    \____/
                  | |                                
                  |_|                                
                                                                                    
                                                                                                    
            ";
            Console.WriteLine(title);

            Console.WriteLine("\nPress enter to start");
            Console.ReadLine();

            RunGame();
        }

        public static void RunGame()
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
            Console.WriteLine($"Balance: {player.Coins} Coins");
            
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

            bool allMonstersAlive = monstersToFight.Any(m => m.IsAlive());

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
                Console.WriteLine($"Would you like to strike with your {player.CurrentWeapon.Name}? (1-{player.CurrentWeapon.MaximumDamage} dmg)");
                Console.WriteLine($"\n(NOTE: If you dicide to flee, the current quest will be canceld and monsters reset)");
                Console.WriteLine($"\n1: Attack with your {player.CurrentWeapon.Name} (1-{player.CurrentWeapon.MaximumDamage} dmg)");
                Console.WriteLine("2: Use Item");
                Console.WriteLine("3: Flee");
        
                string actionChoice = GetValidInput(["1", "2", "3"]);

                if (actionChoice == "3") // Flee
                {
                    PlayerQuest.ActiveQuest = null;
                    Console.Clear();
                    Console.WriteLine("You fled from the fight! Come back when you're stronger.");
                    Console.WriteLine("Hint: find a place to heal!");

                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                    return;
                }
        
                int damageDealtByMonsters = 0;
                Dictionary<string, int> monsterDamageLog = new();
        
                if (actionChoice == "1") // Attack
                {
                    // Player attacks
                    int damage = player.CurrentWeapon.MaximumDamage;
                    int damageDealt = selectedMonster.random.Next(1, damage + 1);
                    selectedMonster.TakeDamage(damageDealt);
            
                    Console.Clear();
                    Console.WriteLine($"\nYou swing your {player.CurrentWeapon.Name} with all your might!");
                    Console.WriteLine($"You hit {selectedMonster.Name}, dealing {damageDealt}HP damage! {(selectedMonster.IsAlive() ? $"It lives with {selectedMonster.CurrentHitPoints}HP left!" : "It collapses, defeated!")}");
                }
                else if (actionChoice == "2") // Use Item
                {
                    // Use item logic
                    if (!UseCombatItem())
                    {
                    // If player didn't use an item or cancelled, return to monster selection
                        continue;
                    }
            
                    Console.Clear();
                    Console.WriteLine($"You've used an item to help in battle!");
                }
        
                if (monstersToFight.All(m => !m.IsAlive())) // All monsters defeated
                {
                    break;
                }

                // All surviving monsters attack the player
                foreach (Monster monster in monstersToFight)
                {
                    if (monster.IsAlive())
                    {
                        int damageDealtByMonster = monster.random.Next(1, monster.MaximumDamage + 1);
                        monsterDamageLog[$"{monster.Name}"] = damageDealtByMonster;
                        damageDealtByMonsters += damageDealtByMonster;
                    }
                }
        
                // Apply monster damage to player
                player.TakeDamage(monsterDamageLog.Values.Sum());
        
                Console.WriteLine($"\nThe monsters retaliate, hurting you for {monsterDamageLog.Values.Sum()}HP in total!");
        
                foreach (var entry in monsterDamageLog)
                {
                    Console.WriteLine($"    -{entry.Key} dealt {entry.Value}HP of damage.");
                }

                Console.WriteLine($"\nYour current HP: {player.CurrentHitPoints}");
        
                if (!player.IsAlive())
                {
                    List<string> deathMessages = new List<string>
                    {
                        "Your vision blurs... A chilling numbness creeps over you... Then, nothing.",
                        "You collapse to the ground, your strength fading... Darkness consumes you.",
                       "A sharp pain jolts through you—then silence. The battle is over... for you.",
                        "Your knees buckle. The last thing you see is your foe standing victorious...",
                        "A cold hand seems to grasp your soul as your body gives in to exhaustion...",
                        "You fought bravely, but fate had other plans. The world fades to black...",
                        "Your breath grows shallow. A final whisper escapes your lips before everything ends...",
                        "The weight of battle is too much. You fall, unable to continue...",
                        "As your consciousness drifts away, a single thought lingers... 'Is this the end?'",
                        "Darkness envelops you. Your journey... has come to an end."
                    };

                    // Select a random death message
                    Random rnd = new Random();
                    string deathMessage = deathMessages[rnd.Next(deathMessages.Count)];
                    Console.Clear();
                    Console.WriteLine(deathMessage);
                    YouDied();
                    Console.ReadLine();
                    restartGame();
                    return;
                }

                // Check if any monsters are still alive
                allMonstersAlive = monstersToFight.Any(m => m.IsAlive());

                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
            PlayerQuest.CompleteQuest(PlayerQuest.ActiveQuest, player);

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        private static bool UseCombatItem()
        {
            // Filter inventory to only show consumable items (not weapons)
            List<Item> consumables = new List<Item>();
            List<int> itemIndices = new List<int>();
    
            for (int i = 0; i < player.Inventory.Count; i++)
            {
                if (!(player.Inventory[i] is Weapon))
                {
                    consumables.Add(player.Inventory[i]);
                    itemIndices.Add(i);
                }
            }
    
            if (consumables.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("You don't have any usable items in your inventory!");
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
                return false;
            }
    
            Console.Clear();
            Console.WriteLine("\nSelect an item to use during this round:");
    
            for (int i = 0; i < consumables.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {consumables[i].Name}");
            }
    
            Console.WriteLine($"{consumables.Count + 1}. Cancel");
    
            // Create valid options list
            List<string> validOptions = new List<string>();
            for (int i = 1; i <= consumables.Count + 1; i++)
            {
                validOptions.Add(i.ToString());
            }
    
            string input = GetValidInput(validOptions);
            int selectedIndex = int.Parse(input) - 1;
    
            // Check if user selected cancel
            if (selectedIndex >= consumables.Count)
            {
                return false;
            }
    
            // Use the selected item
            Item selectedItem = consumables[selectedIndex];
            int inventoryIndex = itemIndices[selectedIndex];
    
            // Item effect logic based on item name
            Console.WriteLine($"You used {selectedItem.Name}!");
    
            // Apply healing effects based on item name
            if (selectedItem.Name.Contains("Health Potion"))
            {
                Console.Clear();
                int healAmount = 40;
                player.CurrentHitPoints = Math.Min(player.MaximumHitPoints, player.CurrentHitPoints + healAmount);
                Console.WriteLine($"You healed for {healAmount} HP!");
                Console.WriteLine($"Your current HP now: {player.CurrentHitPoints}\n");
            }
            else if (selectedItem.Name.Contains("Shield Potion"))
            {
                Console.Clear();
                player.MaximumHitPoints = 130;
                int healAmount = 30;
                player.CurrentHitPoints = Math.Min(player.MaximumHitPoints, player.CurrentHitPoints + healAmount);
                Console.WriteLine($"You have used a Shield Potion! Your maximum HP is now {player.MaximumHitPoints}!");
                Console.WriteLine($"You fully healed for {healAmount} HP!");
                Console.WriteLine($"Your current HP now: {player.CurrentHitPoints}\n");
            }

            // Remove the item from inventory after use
           player.Inventory.RemoveAt(inventoryIndex);
    
           Console.WriteLine("Press enter to continue...");
           Console.ReadLine();
    
            return true;
        }


        public static void restartGame()
        {
            player.CurrentLocation = World.Locations[0];
            PlayerQuest.CompletedQuests = [];
            player = new(World.Weapons[0], World.Locations[0]);
        }

        private static void TalkToWitch(Witch witch)
        {
            witch.TalkToPlayer(player);
        }
    
        public static bool ValidateInput(string? input, List<string> options)
        {
            return input != null && options.Contains(input);
        }

        public static string GetValidInput(List<string> validOptions)
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
            Console.Clear();
            Console.WriteLine("----------\nInventory:\n----------\n");
            
            if (player.Inventory.Count == 0)
            {
                Console.WriteLine("Your inventory is empty.");
            }
            else
            {
                for (int i = 0; i < player.Inventory.Count; i++)
                {
                    Item item = player.Inventory[i];
                    if (item is Weapon weapon)
                    {
                        string equippedStatus = (player.CurrentWeapon == weapon) ? " [EQUIPPED]" : "";
                        Console.WriteLine($"{i + 1}. {item.Name} - (Damage: {weapon.MaximumDamage}){equippedStatus}");
                    }
                    else
                    {
                        Console.WriteLine($"{i + 1}. {item.Name}");
                    }
                }
            
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("1. Use/equip an item");
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
                        Console.WriteLine("Press enter to continue...");
                        Console.ReadLine();
                        OpenInventory();
                        break;
                }
            }
            
        }
        
        private static void UseItem()
        {
            List<string> optionsList = new();

            Console.WriteLine("\nWhich item would you like to use?");
            for (int i = 0; i < player.Inventory.Count; i++)
            {
                Item item = player.Inventory[i];
                if (item is Weapon weapon)
                {
                    Console.WriteLine($"{i + 1}. {item.Name} (Weapon - Damage: {weapon.MaximumDamage})");
                }
                else
                {
                    Console.WriteLine($"{i + 1}. {item.Name}");
                }
            }
            
            string input = Console.ReadLine();
            
            if (int.TryParse(input, out int index) && index > 0 && index <= player.Inventory.Count)
            {
                Item item = player.Inventory[index - 1];
                
                if (item is Weapon weapon)
                {
                    player.CurrentWeapon = weapon;
                    Console.WriteLine($"You equipped {weapon.Name}.");
                }
                else
                {
                    Console.WriteLine($"You used {item.Name}.");
                    player.Inventory.RemoveAt(index - 1);
                }
                
            }
            
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        public static void YouDied()
        {
            Console.Title = "Players healt <= 0";

            // ASCII Art Title
            string title = @"



            
          _______               ______  _________ _______  ______   _ 
|\     /|(  ___  )|\     /|    (  __  \ \__   __/(  ____ \(  __  \ ( )
( \   / )| (   ) || )   ( |    | (  \  )   ) (   | (    \/| (  \  )| |
 \ (_) / | |   | || |   | |    | |   ) |   | |   | (__    | |   ) || |
  \   /  | |   | || |   | |    | |   | |   | |   |  __)   | |   | || |
   ) (   | |   | || |   | |    | |   ) |   | |   | (      | |   ) |(_)
   | |   | (___) || (___) |    | (__/  )___) (___| (____/\| (__/  ) _ 
   \_/   (_______)(_______)    (______/ \_______/(_______/(______/ (_)
                                                                      
            ";
            Console.WriteLine(title);

            Console.WriteLine("\nPress enter to restart");
            Console.ReadLine();
        }
    }
}