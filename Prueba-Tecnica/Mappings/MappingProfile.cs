using AutoMapper;
using Prueba_Tecnica.Models;
using Prueba_Tecnica.DTOs.Products;
using Prueba_Tecnica.DTOs.Categories;
using Prueba_Tecnica.DTOs.Warehouses;
using Prueba_Tecnica.DTOs.Inventory;
using Prueba_Tecnica.DTOs.Reports;

namespace Prueba_Tecnica.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<CreateCategoryDTO, Category>();

        CreateMap<Warehouse, WarehouseDTO>().ReverseMap();
        CreateMap<CreateWarehouseDTO, Warehouse>();

        CreateMap<Product, ProductListDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<CreateProductDTO, Product>();
        CreateMap<UpdateProductDTO, Product>();

        CreateMap<MovementRequestDTO, InventoryMovements>()
            .ForMember(dest => dest.MovementDate, opt => opt.Ignore());

        CreateMap<InventoryMovements, InventoryReportDTO>()
            .ForMember(dest => dest.ProductCode, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Product.Description ?? src.Product.Name))
            .ForMember(dest => dest.MovementNumber, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Warehouse, opt => opt.MapFrom(src => src.Warehouse.Name));

        CreateMap<ProductWarehouse, LowStockProductDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.MinStock, opt => opt.MapFrom(src => src.Product.MinStock));
    }
}