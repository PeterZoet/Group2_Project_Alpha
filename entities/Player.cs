namespace entities;
public class Player
{
    public int CurrentHitPoints;
    public int MaximumHitPoints;
    public int Coins;
    public Weapon CurrentWeapon { get; set; }
    public Location CurrentLocation { get; set; }
    public Location _previousLocation { get; set; }
    public List<Item> Inventory { get; set; }
    

    public Player(Weapon currentWeapon, Location startLocation)
    {
        MaximumHitPoints = 100;
        CurrentHitPoints = 10;
        CurrentWeapon = currentWeapon;
        CurrentLocation = startLocation;
        _previousLocation = startLocation;
        Coins = 4;
        Inventory = new List<Item>();
        Inventory.Add(new Item("Healing Potion"));
        Inventory.Add(new Weapon(World.WEAPON_ID_RUSTY_SWORD,"Rusty Sword", 5));
        
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

    public void MoveTo(Location? newLocation)
    {
        if (newLocation == null)
        {
            Console.WriteLine("You did not move because there is no location in the chosen direction!");
            return;
        }
        
        // Cancel active quests if leaving the area
        if (CurrentLocation.QuestAvailableHere != null && PlayerQuest.ActiveQuest == CurrentLocation.QuestAvailableHere)
        {
            Quest questToCancel = CurrentLocation.QuestAvailableHere;
            PlayerQuest.FleeQuest(questToCancel);
            Console.WriteLine($"You left the area, so the quest '{questToCancel.Name}' has been cancelled.");
        }
        
        _previousLocation = CurrentLocation;
        CurrentLocation = newLocation;
    }
}