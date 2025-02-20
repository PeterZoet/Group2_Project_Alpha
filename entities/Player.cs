namespace entities;

public class Player
{
    public string Name { get; set; }
    public int CurrentHitPoints { get; set; }
    public int MaximumHitPoints { get; set; }
    public Weapon CurrentWeapon { get; set; }
    public Location CurrentLocation { get; set; }

    public Player(string name, int maxHitPoints, Weapon currentWeapon, Location currentLocation)
    {
        Name = name;
        MaximumHitPoints = maxHitPoints;
        CurrentHitPoints = maxHitPoints;
        CurrentWeapon = currentWeapon;
        CurrentLocation = currentLocation;
    }
}
