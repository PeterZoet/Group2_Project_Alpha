using _2425_OP34_Group2_Project_Alpha;

namespace entities;
public class Witch
{
    public string Name;
    public List<(Item Item, int Price)> ItemsForSale;

    public Witch()
    {
        Name = "Ennadiz";

        Item healthPotion = new("Health Potion");
        Item shieldPotion = new("Shield Potion");

        ItemsForSale = new List<(Item, int)>
        {
            (healthPotion, 10),
            (shieldPotion, 15)
        };
    }

    public void TalkToPlayer(Player player)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Talking to {Name} The Witch...");
            Console.WriteLine("\nAh, a weary traveler! The winds whisper of your journey... let me mend your wounds and share my wares for just a few coins.\n");

            Console.WriteLine($"Health Points: {player.CurrentHitPoints}/{player.MaximumHitPoints}");
            Console.WriteLine($"Coin Balance: {player.Coins} 🪙\n");

            Console.WriteLine("1: Small Heal (30HP) - Free");
            Console.WriteLine("2: Medium heal (60HP) - 2 Coins");
            Console.WriteLine("3: Full Heal - 4 Coins");
            Console.WriteLine("4: View Shop");
            Console.WriteLine("5: Return");

            string choice = Program.GetValidInput(["1", "2", "3", "4", "5"]);
            switch (choice)
            {
                case "1":
                    HealToAmount(player, 30, 0);
                    break;
                case "2":
                    HealToAmount(player, 60, 2);
                    break;
                case "3":
                    FullHeal(player, 4);
                    break;
                case "4":
                    OpenShop(player);
                    break;
                case "5":
                    return;
            }
        }
    }

    private void OpenShop(Player player)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Talking to {Name} The Witch...");
            Console.WriteLine("\nAh, so you've an eye for my wares! Shields strengthened, wounds mended—if you’ve the coin, I’ve the magic. What shall it be, traveler?\n");

            Console.WriteLine($"Health Points: {player.CurrentHitPoints}/{player.MaximumHitPoints}");
            Console.WriteLine($"Coin Balance: {player.Coins} Coins\n");

            DisplayItemsForSale();

            Console.WriteLine($"{ItemsForSale.Count + 1}: Return");
            Console.Write("Enter the number of the item you want to buy: ");

            List<string> optionString = new();
            for (int i = 0; i < ItemsForSale.Count; i++)
            {
                optionString.Add($"{i}");
            }         

            if (int.TryParse(Program.GetValidInput(optionString), out int itemNumber))
            {
                if (itemNumber == ItemsForSale.Count + 1)
                {
                    return;
                }
                SellItem(player, itemNumber);
            }
        }
    }

    public void DisplayItemsForSale()
    {
        Console.WriteLine("Items for sale:");
        for (int i = 0; i < ItemsForSale.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {ItemsForSale[i].Item.Name} - {ItemsForSale[i].Price} coins");
        }
    }

    public void SellItem(Player player, int itemNumber)
    {
        if (itemNumber < 1 || itemNumber > ItemsForSale.Count)
        {
            Console.WriteLine("Invalid option.");
            return;
        }

        var (item, price) = ItemsForSale[itemNumber - 1];

        if (player.Coins >= price)
        {
            player.Coins -= price;
            Console.WriteLine($"You bought {item.Name} for {price} coins.");
        }
        else
        {
            Console.WriteLine($"Not enough coins to buy {item.Name}.");
        }
        Thread.Sleep(1500);
    }

    public void HealToAmount(Player player, int targetHealth, int price)
    {
        if (player.CurrentHitPoints < targetHealth)
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