
using System.ComponentModel.DataAnnotations;

namespace CartApi.Requests
{
    public class NewItemQuantityRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be grater than or equal to one")]
        public int Quantity { get; set; }
    }
}