namespace entities;
public class PlayerQuest
{
    public static List<Quest> ActiveQuests { get; } = new List<Quest>();
    public static List<Quest> CompletedQuests { get; } = new List<Quest>();

    public static void StartQuest(Quest quest)
    {
        if (quest != null && !ActiveQuests.Contains(quest) && !CompletedQuests.Contains(quest))
        {
            quest.StartQuest();
            ActiveQuests.Add(quest);
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
        if (quest != null && ActiveQuests.Contains(quest))
        {
            quest.FleeQuest();
            ActiveQuests.Remove(quest);
        }
    }
}