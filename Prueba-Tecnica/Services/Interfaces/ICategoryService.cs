namespace Prueba_Tecnica.Services.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<CategoriesDTO> GetAllAsync();

    }
}
