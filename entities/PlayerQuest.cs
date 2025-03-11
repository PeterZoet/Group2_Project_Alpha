namespace entities;
public class PlayerQuest
{
    public static Quest? ActiveQuest { get; set; }
    public static List<Quest> CompletedQuests { get; } = new List<Quest>();

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
}