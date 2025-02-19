namespace entities;

class Monster
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int MaximumDamage { get; set; }
    public int CurrentHitPoints { get; set; }
    public int MaximumHitPoints { get; set; }
    public Random random = new();

    public Monster(int id, string name, int maxDamage, int maxHitPoints) //defaults worden geïnitialiseerd bij het aanmaken van een object van deze class
    {
        ID = id;
        Name = name;
        MaximumDamage = maxDamage;
        MaximumHitPoints = maxHitPoints;
        CurrentHitPoints = maxHitPoints;
    }

    public int Attack()
    {
        return random.Next(1, MaximumDamage + 1);
    }

    public void TakeDamage(int damage)
    {
        CurrentHitPoints -= damage;
        if (CurrentHitPoints < 0)
        {
            CurrentHitPoints = 0;
        }
    }

    public bool IsAlive()
    {
        return CurrentHitPoints > 0;
    }
}    