using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniShop.Model
{
    [Table("Item")]
    public class Item : EntityBaseNoDeleted
    {
        /// <summary>
        /// 商店Id
        /// </summary>
        [Required]
        public Guid ShopId { get; set; }

        /// <summary>
        /// 类别Id
        /// </summary>
        public int CategorieId { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [ForeignKey("CategorieId")]
        public virtual Categorie Categorie { get; set; }

        /// <summary>
        /// 商品条码
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        [Required]
        public decimal Price { get; set; }
    }
}
