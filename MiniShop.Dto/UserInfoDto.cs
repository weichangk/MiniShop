using MiniShop.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniShop.Dto
{
    public class UserInfoDto
    {
        public int Id { get; set; }
        public Guid ShopId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public EnumRole Role { get; set; }
    }
}
