namespace entities;
public class PlayerQuest
{
    public static Quest? ActiveQuest { get; set; }
    public static List<Quest> CompletedQuests { get; set;} = new List<Quest>();

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

    public static void CompleteQuest(Quest quest)
    {
        if (quest != null)
        {
            ActiveQuest = null;
            CompletedQuests.Add(quest);
            quest.CompleteQuest();
        }

        if (PlayerQuest.CompletedQuests.Count == 2)
        {
            Location guardPost = World.LocationByID(3);
            Location bridge = World.LocationByID(8);
            guardPost.LocationToEast = bridge;
        }
    }
}