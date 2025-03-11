namespace entities;
public class Player
{
    public int CurrentHitPoints;
    public int MaximumHitPoints;
    public Weapon CurrentWeapon { get; set; }
    public Location CurrentLocation { get; set; }
    private Location _previousLocation;
    public List<Item> Inventory { get; set; }
    

    public Player(Weapon currentWeapon, Location startLocation)
    {
        MaximumHitPoints = 100;
        CurrentHitPoints = MaximumHitPoints;
        CurrentWeapon = currentWeapon;
        CurrentLocation = startLocation;
        _previousLocation = startLocation;
        Inventory = new List<Item>();
        Inventory.Add(new Item("Healing Potion", "Restores 10 health"));
        Inventory.Add(new Weapon(1, "Rusty Sword", "A rusty sword", 5));
        
    }

    public void MoveTo(Location? newLocation)
    {
        if (newLocation == null)
        {
            Console.WriteLine("You did not move because there is no location in the chosen direction!");
            return;
        }

        // Cancel active quests if leaving the area
        if (CurrentLocation.QuestAvailableHere != null &&
            PlayerQuest.ActiveQuests.Contains(CurrentLocation.QuestAvailableHere))
        {
            Quest questToCancel = CurrentLocation.QuestAvailableHere;
            PlayerQuest.FleeQuest(questToCancel);
        }

        _previousLocation = CurrentLocation;
        CurrentLocation = newLocation;
    }
}