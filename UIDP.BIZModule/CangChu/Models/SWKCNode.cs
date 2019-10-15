using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.BIZModule.CangChu.Models
{
    public class SWKCNode
    {
        public string ID { get; set; }//节点ID
        public string Code { get; set; }//节点编码
        public string Name { get; set; }//节点名称
        public string Unit { get; set; }//单位
        public decimal Number { get; set; }//数量
        public string Location { get; set; }//位置
        public bool hasChildren { get; set; }//是否有子节点
    }
}
