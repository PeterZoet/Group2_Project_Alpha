namespace entities;

class Quest
{
    public int ID { get; }
    public string Name { get; }
    public string Description { get; }

    public Quest(int id, string name, string description)
    {
        ID = id;
        Name = name;
        Description = description;
    }
}