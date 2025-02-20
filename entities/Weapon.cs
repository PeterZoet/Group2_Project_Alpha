namespace entities;

public class Weapon
{
    public int ID { get; }
    public string Name { get; }
    public int MaximumDamage { get; }

    public Weapon(int id, string name, int maximumdamage)
    {
        ID = id;
        Name = name;
        MaximumDamage = maximumdamage;
    }
}