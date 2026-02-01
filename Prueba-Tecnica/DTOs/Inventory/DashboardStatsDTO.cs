namespace Prueba_Tecnica.DTOs.Inventory;

public class DashboardStatsDTO
{
    public int TotalProducts { get; set; }
    public int TotalStock { get; set; }
    public int LowStockCount { get; set; }
    public List<LowStockProductDTO> LowStockProducts { get; set; } = [];
}

public class LowStockProductDTO
{
    public string Name { get; set; } = null!;
    public int CurrentStock { get; set; }
    public int MinStock { get; set; }
}