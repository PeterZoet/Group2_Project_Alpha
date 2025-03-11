namespace entities;

public class Weapon : Item
{
    public int ID { get; set; }
    public int MaximumDamage { get; set; }

    public Weapon(string name, int id, string description, int maximumDamage) 
        : base(name, description)
    {
        Name = name;
        ID = id;
        MaximumDamage = maximumDamage;
    }
}