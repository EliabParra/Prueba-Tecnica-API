namespace Prueba_Tecnica.DTOs.Products
{
    public class ProductListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string UnitOfMeasure { get; set; } = null!;
        public int MinStock { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
