
using System.Threading.Tasks;
using CartApi.Models;

namespace CartApi.Services
{
    public interface IProductService
    {
        Task<Product> GetProductAsync(string id);
    }
}