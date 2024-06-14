using SQLite;

public class Item
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    public string ItemPrice { get; set; }
    public byte[] ItemImage { get; set; }
    public bool IsButton { get; set; } 
}