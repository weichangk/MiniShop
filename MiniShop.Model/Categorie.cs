using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniShop.Model
{
    /// <summary>
    /// 商品类别
    /// </summary>
    [Table("Categorie")]
    public class Categorie : EntityBaseNoDeletedStoreId<int>
    {
        public int Code { get; set; }

        [DefaultValue(0)]
        public int Level { get; set; }

        public int ParentCode { get; set; }
    }
}
