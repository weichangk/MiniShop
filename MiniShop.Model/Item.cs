using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniShop.Model
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ShopId { get; set; }
        [ForeignKey("CategorieId")]
        public int CategorieId { get; set; }
        [Required]
        [MaxLength(32)]
        public string Code { get; set; }
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
    }
}
