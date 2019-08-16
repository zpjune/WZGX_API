using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.BIZModule.CangChu.Models
{
    public class GCTreeNode
    {
        public string DW_CODE { get; set; }
        public string DW_NAME { get; set; }
        public List<GCTreeNode> Children { get; set; }

    }
}
