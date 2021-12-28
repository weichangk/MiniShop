﻿using System.Collections.Generic;

namespace MiniShop.Mvc.Models
{
    public class Table
    {
        public int code { get; set; } = 0;
        public string msg { get; set; }
        public int count { get; set; }
        public dynamic data { get; set; }
    }

    public class Tree
    {
        public int id { get; set; }
        public string label { get; set; }
        public bool isLeaf { get; set; }
        public List<Tree> children { get; set; }

    }

    public class Result
    {
        public bool success { get; set; } = true;
        public string msg { get; set; } = "成功！";
        public dynamic data { get; set; }
        public int status { get; set; } = 200;
    }
}
