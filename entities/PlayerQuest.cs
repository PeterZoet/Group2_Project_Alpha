using _2425_OP34_Group2_Project_Alpha;

namespace entities;
public class PlayerQuest
{
    public static Quest? ActiveQuest { get; set; }
    public static List<Quest> CompletedQuests { get; set; } = new List<Quest>();

    public static void StartQuest(Quest quest)
    {
        if (quest != null && !CompletedQuests.Contains(quest))
        {
            ActiveQuest = quest;
            quest.StartQuest();
        }
        else if (CompletedQuests.Contains(quest))
        {
            Console.WriteLine($"You've already completed the quest: {quest.Name}");
        }
        else
        {
            Console.WriteLine($"The quest is already active: {quest.Name}");
        }
    }

    public static void FleeQuest(Quest quest)
    {
        if (quest != null)
        {
            quest.FleeQuest();
        }
    }

    public static void CompleteQuest(Quest quest, Player player)
    {
       Console.WriteLine("\n+------------------------------------------------+");
        Console.WriteLine("QUEST COMPLETE!");  
        Console.WriteLine("You stand triumphant over the fallen monsters!");  
        Console.WriteLine($"Mission Accomplished: {PlayerQuest.ActiveQuest!.Name}");
        Console.WriteLine("+------------------------------------------------+");
        Console.WriteLine("You gained 10 Coins!");
        if (quest?.ID == 1)
        {
            Console.WriteLine($"You obtained: {World.WeaponByID(2).Name} " +  
            $"(Deals 1-{World.WeaponByID(2).MaximumDamage} HP damage)!");
        }

            
        if (quest != null)
        {
            ActiveQuest = null;
            CompletedQuests.Add(quest);
            quest.CompleteQuest();
        }

        if (CompletedQuests.Count == 2)
        {
            Location guardPost = World.LocationByID(3);
            Location bridge = World.LocationByID(8);
            guardPost.LocationToEast = bridge;
        }

        if(PlayerQuest.CompletedQuests.Count == 3)
        {
            PlayerQuest.WinGame();
        }

        player.Coins += 10;

        if(quest?.ID == 1)
        {
            player.Inventory.Add(World.WeaponByID(2));
        }
    }

    public static void WinGame()
    {
        Program.restartGame();
        Console.Clear();
        string youWonASCII = @"
__   __           __        __          _ 
\ \ / /__  _   _  \ \      / /__  _ __ | |
 \ V / _ \| | | |  \ \ /\ / / _ \| '_ \| |
  | | (_) | |_| |   \ V  V / (_) | | | |_|
  |_|\___/ \__,_|    \_/\_/ \___/|_| |_(_)                                                         
        ";

        Console.WriteLine(youWonASCII);
        Console.WriteLine("Congratulations on completing the game!");
        Console.WriteLine("Press enter to play again...");
        Console.ReadLine();
    }
    
}
