using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.BIZModule.Modules
{
    public class JYWZ
    {
        public string WERKS { get; set; }
        public string WERKS_NAME { get; set; }
        public string MATKL { get; set; }
        public string MATNR { get; set; }
        public string MAKTX { get; set; }
        public string MEINS { get; set; }
        public Decimal GESME { get; set; }
        public string ZSTATUS { get; set; }
        public string LGPLA { get; set; }//货位号
        public string KCSTATUS { get; set; }//库存状态 是判断出来的 上面的字段全是直接查询出来的  00正常 01积压
        public string BUDAT_MKPF { get; set; } //过账日期
    }
}
