using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.BIZModule.CangChu.Models
{
    public class WLZTreeNode
    {
        public string id { get; set; }
        public string label { get; set; }
        public string Code { get; set; }
        public bool hasChildren { get; set; }
        public List<WLZTreeNode> children { get; set; }
        public string FlagID { get; set; }
        public string DLCODE { get; set; }
        public string ZLCODE { get; set; }
        public string XLCODE { get; set; }
        public bool IsLoading { get; set; }
    }
}
