using System.ComponentModel.DataAnnotations.Schema;

namespace MiniShop.Model
{
    /// <summary>
    /// 商品类别
    /// </summary>
    [Table("Categorie")]
    public class Categorie : EntityBaseNoDeletedStoreId<int>
    {
        public string Code { get; set; }
    }
}
