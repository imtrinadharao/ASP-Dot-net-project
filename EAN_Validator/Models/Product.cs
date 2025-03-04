using System.ComponentModel.DataAnnotations;

namespace EANValidator.Models
{
    public class Product
    {
        [Key]
        public string EAN { get; set; }
        public string ProductName { get; set; }
        public string Brand { get; set; }
        public string Manufacturer { get; set; }
    }

}
