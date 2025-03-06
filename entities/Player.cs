namespace entities;

public class Player
{
    public int CurrentHitPoints;
    public int MaximumHitPoints;
    public Weapon CurrentWeapon { get; set; }
    public Location CurrentLocation { get; set; }

    public Player(Weapon currentWeapon, Location startLocation)
    {
        MaximumHitPoints = 100;
        CurrentHitPoints = MaximumHitPoints;
        CurrentWeapon = currentWeapon;
        CurrentLocation = startLocation;
    }

    public void MoveTo(Location? newLocation)
    {
        if (newLocation == null) 
        {
            Console.WriteLine("You did not move because there is no location in the chosen direction!");
            return; 
        }
        
        CurrentLocation = newLocation;
    }
}
