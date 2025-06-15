namespace LaptopSG.Models
{
    public interface IStoreRepository 
    { 
        IQueryable<Product> Products { get; } 
    }
}