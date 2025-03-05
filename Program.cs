using entities;
namespace _2425_OP34_Group2_Project_Alpha;

class Program
{
    static void Main()
    {
        Console.WriteLine("Hello, World!");

        DisplayWorldAttributes();
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
}
