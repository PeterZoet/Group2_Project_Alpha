namespace entities;

public class Quest
{
    public int ID { get; }
    public string Name { get; }
    public string Description { get; }
    public bool IsActive { get; private set; }
    public bool IsCompleted { get; private set; }
    public bool WasStarted { get; private set; }

    public Quest(int id, string name, string description)
    {
        ID = id;
        Name = name;
        Description = description;
        IsActive = false;
        IsCompleted = false;
        WasStarted = false;
    }

    public void StartQuest()
    {
        IsActive = true;
        WasStarted = true;
        Console.WriteLine($"Quest started: {Name}");
        Console.WriteLine(Description);
    }

    public void FleeQuest()
    {
        if (IsActive)
        {
            IsActive = false;
            Console.WriteLine($"You fled from the quest: {Name}");
        }
        else
        {
            Console.WriteLine("No active quest to flee from.");
        }
    }

    public void CompleteQuest()
    {
        if (IsActive)
        {
            IsActive = false;
            IsCompleted = true;
            Console.WriteLine($"Quest completed: {Name}");
        }
    }
}