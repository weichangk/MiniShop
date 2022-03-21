// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.ComponentModel.DataAnnotations;

namespace IdentityServerHost.Quickstart.UI
{
    public class LoginInputModel
    {
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "{0}，不能为空")]
        public string Username { get; set; }
        [Display(Name = "密码")]
        [Required(ErrorMessage = "{0}，不能为空")]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}