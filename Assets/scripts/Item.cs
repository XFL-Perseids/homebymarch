[System.Serializable]
public class Item
{
    public string itemName;
    public int price;

    public Item(string name, int cost)
    {
        itemName = name;
        price = cost;
    }
}
