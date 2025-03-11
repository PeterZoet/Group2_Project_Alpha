namespace entities;

public class Weapon : Item
{
    public int MaximumDamage { get; set; }
    public int ID { get; set; }

    public Weapon(int id, string name, int maximumDamage) 
        : base(name)
    {
        ID = id;
        MaximumDamage = maximumDamage;
    }
}