using System.Collections.Generic;

namespace MiniShop.Mvc.Models
{
    public class Table
    {
        public int Code { get; set; } = 0;
        public string Msg { get; set; }
        public int Count { get; set; }
        public dynamic Data { get; set; }
    }

    public class Tree
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public bool IsLeaf { get; set; }
        public List<Tree> Children { get; set; }

    }

    public class Result
    {
        public bool Success { get; set; } = true;
        public string Msg { get; set; } = "成功！";
        public dynamic Data { get; set; }
        public int Status { get; set; } = 200;
    }
}
