using System;

namespace MiniShop.Dto
{
    public class ShopInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Contacts { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime ValidDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
