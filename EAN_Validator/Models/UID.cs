using System.ComponentModel.DataAnnotations;

namespace EANValidator.Models
{
    public class Nitem
    {
        [Key]
        public String UID { get; set; }
        public string EAN { get; set; }
    }
}
