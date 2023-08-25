namespace OOP_Restaurant_Controll_System.Models.Constructors
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool IsDrink { get; set; }//jei drink saugoti i drink faila jei nedrink saugoti i food faila
        public DateTime CreatedDate { get; set; } //sita naudot tik kuriant nauja produkta

        public MenuItem(int id, string name, string description, double price, DateTime createdDate, bool isDrink)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            IsDrink = isDrink;
            CreatedDate = createdDate;
        }

        public string FoodFileFormat()
        {
            return $"{Id};{Name};{Description};{Price};{CreatedDate};{IsDrink}"; 
        }

        public string GetFoodItem()
        {
            return $"| {Id,-3} | {Name,-20} | {Description,-50} | {Price,-6:F2} |";
        }

        public string GetFoodItemForReceipt()
        {
            return $"| {Name,-20} | {Price,-6:F2} |";
        }
    }
}
