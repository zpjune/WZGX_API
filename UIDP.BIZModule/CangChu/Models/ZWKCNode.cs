using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.BIZModule.CangChu.Models
{
    public class ZWKCNode
    {
        public string ID { get; set; }//节点ID
        public string CODE { get; set; }//节点编码
        public string NAME { get; set; }//节点名称
        public decimal SALK3 { get; set; }//金额数量
        public int level { get; set; }//节点等级
        public bool hasChildren { get; set; }//是否有子节点

    }
}
