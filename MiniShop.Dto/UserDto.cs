﻿using MiniShop.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class UserDto
    {
        [Display(Name = "用户ID")]
        public int Id { get; set; }

        [Display(Name = "商店ID")]
        public Guid ShopId { get; set; }

        [Display(Name = "用户名")]
        public string Name { get; set; }

        [Display(Name = "手机号")]
        public string Phone { get; set; }

        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Display(Name = "角色")]
        public EnumRole Role { get; set; }
    }
}
