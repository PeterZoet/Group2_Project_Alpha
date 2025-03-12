using entities;

public class Witch
{
    public string Name;
    public int RestoresHpTo;
    public List<string> ShopItems = ["Shield", "Health Potion"];

    public Witch()
    {
        Name = "Ennadiz";
    }

    public void HealToAmount(Player player, int targetHealth, int price)
    {
        if (player.CurrentHitPoints < targetHealth) // Only heal if health is below the target
        {
            if (player.Coins >= price)
            {
                player.Coins -= price;
                player.CurrentHitPoints = Math.Min(targetHealth, player.MaximumHitPoints);
                Console.WriteLine($"You have been healed to {player.CurrentHitPoints}HP.");
            }
            else
            {
                Console.WriteLine("Not enough coins to heal.");
            }
        }
        else
        {
            Console.WriteLine($"Your health is already above {targetHealth}!");
        }
        Thread.Sleep(1500);
    }

    public void FullHeal(Player player, int price)
    {
        if (player.CurrentHitPoints == player.MaximumHitPoints)
        {
            Console.WriteLine("Health is already full!");
            Thread.Sleep(1500);
            return;
        }

        if (player.Coins >= price)
        {
            player.Coins -= price;
            player.CurrentHitPoints = player.MaximumHitPoints;
            Console.WriteLine("You have been fully healed!");
        }
        else
        {
            Console.WriteLine("Not enough coins to heal.");
        }
        Thread.Sleep(1500);
    }
    
}