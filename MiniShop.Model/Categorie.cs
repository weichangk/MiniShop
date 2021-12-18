using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniShop.Model
{
    [Table("Categorie")]
    public class Categorie : EntityBaseNoDeleted
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        [Required]
        public Guid ShopId { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
