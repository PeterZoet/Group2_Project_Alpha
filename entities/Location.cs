namespace entities;

public class Location
{
    // Required properties (set during construction)
    public int ID { get; }
    public string Name { get; }
    public string Description { get; }
    public bool Killable { get; set; } //In this location you can kill something
    public Quest QuestAvailableHere { get; set; }
    public Monster MonsterLivingHere { get; set; }
    public Witch WitchAvailableHere { get; set; }

    // Neighboring locations (can be assigned after creation)
    public Location? LocationToNorth { get; set; }
    public Location? LocationToEast { get; set; }
    public Location? LocationToSouth { get; set; }
    public Location? LocationToWest { get; set; }

    // Constructor to initialize required fields
    public Location(int id, string name, string description, bool interactable, bool killable, Quest questAvailableHere, Monster monsterLivingHere, Witch witchAvailableHere)
    {
        ID = id;
        Name = name;
        Description = description;
        Killable = killable;
        QuestAvailableHere = questAvailableHere;
        MonsterLivingHere = monsterLivingHere;
        WitchAvailableHere = witchAvailableHere;
    }

    public void Compass()
    {
        Console.WriteLine("\nFrom here you can go:\n"); 
        if (LocationToNorth != null) Console.WriteLine("    N");
        if (LocationToNorth != null) Console.WriteLine("    |");
        Console.WriteLine($"{(LocationToWest != null ? "W---" : "    ")}|{(LocationToEast != null ? "---E" : "")}");
        if (LocationToSouth != null) Console.WriteLine("    |");
        if (LocationToSouth != null) Console.WriteLine("    S");
        Console.WriteLine();
    }

    public Location? GetLocationAt(string location) => location switch
    {
        "N" => LocationToNorth,
        "E" => LocationToEast,
        "S" => LocationToSouth,
        "W" => LocationToWest,
        _ => null
    };
}
