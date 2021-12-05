using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Model
{
    public class Shop
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        [Required]
        [MaxLength(32)]
        public string Contacts { get; set; }
        [MaxLength(32)]
        public string Phone { get; set; }
        [MaxLength(32)]
        public string Email { get; set; }
        [MaxLength(64)]
        public string Address { get; set; }
        [Required]
        public DateTime ValidDate { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
    }
}
