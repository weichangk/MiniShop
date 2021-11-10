using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniShop.Model
{
    public class Shop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        [Required]
        [MaxLength(32)]
        public string Contacts { get; set; }
        [Required]
        [MaxLength(32)]
        public string Phone { get; set; }
        [Required]
        [MaxLength(32)]
        public string Email { get; set; }
        [Required]
        [MaxLength(64)]
        public string Address { get; set; }
        [Required]
        public DateTime ValidDate { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
    }
}
