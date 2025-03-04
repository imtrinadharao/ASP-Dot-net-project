using System.ComponentModel.DataAnnotations;

namespace EANValidator.Models
{
    public class Item
    {
        [Key]
        public string UID { get; set; }
        
    }
}

