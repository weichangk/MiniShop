using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class UnitUpdateDto
    {
        [Display(Name = "ID")]
        [Required(ErrorMessage = "{0},不能为空")]
        public int Id { get; set; }

        [Display(Name = "单位名称")]
        [Required(ErrorMessage = "{0},不能为空")]
        public string Name { get; set; }
    }
}
