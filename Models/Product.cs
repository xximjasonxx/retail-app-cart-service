
using Newtonsoft.Json;

namespace CartApi.Models
{
    public class Product
   {
       public string ProductId { get; set;}
       
       [JsonProperty("productName")]
       public string Name { get; set; }

       [JsonProperty("productDescription")]
       public string Description { get; set; }

       [JsonProperty("price")]
       public decimal Price { get; set; }

       [JsonProperty("count")]
       public int Count { get; set; }
       public int InOrder { get; set; }
   }
}