using System.ComponentModel.DataAnnotations;

namespace VINATECH.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }
    }
}
