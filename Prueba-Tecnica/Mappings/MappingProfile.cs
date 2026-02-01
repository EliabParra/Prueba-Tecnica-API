using AutoMapper;
using Prueba_Tecnica.Models;
using Prueba_Tecnica.DTOs.Products;
using Prueba_Tecnica.DTOs.Categories;
using Prueba_Tecnica.DTOs.Warehouses;

namespace Prueba_Tecnica.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Categorías
        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<CreateCategoryDTO, Category>();

        // Almacenes
        CreateMap<Warehouse, WarehouseDTO>().ReverseMap();

        // Productos
        // Aquí hacemos un mapeo especial para mostrar el nombre de la categoría en lugar del ID
        CreateMap<Product, ProductListDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<CreateProductDTO, Product>();
    }
}