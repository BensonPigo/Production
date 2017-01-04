using Ict;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win;
using Ict.Win;

namespace Sci.Production.Cutting
{
    public partial class P02_PackingMethod : Sci.Win.Subs.Input4
    {
        private DataTable packingTb;
        private string cuttingid;
        public P02_PackingMethod(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            cuttingid = keyvalue1;

        }
        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
                .Text("ID", header: "SP#", width: Widths.AnsiChars(16));
            return true;
        }
        protected override DualResult OnRequery(out DataTable datas)
        {
            string cmdsql = string.Format(
                @"Select a.id,a.ctnqty,isnull(b.id,'0') as signalcolor,a.Packing,
                case ctntype when 1 then 'Solid Color/Size' 
                when 2 then 'Solid Color/Assorted Size' 
                when 3 then 'Assorted Color/Solid Size'  
                when 4 then 'Assorted Color/Size'  
                when 5 then 'Other' 
                End 'Packingmethod'   
                From orders a 
                left join order_QtyCTN b on b.id =a.id  
                Where cuttingsp = '{0}'", cuttingid);
            DualResult dr = DBProxy.Current.Select(null, cmdsql, out datas);
            if (!dr)
            {
                ShowErr(cmdsql, dr);
                return dr;
            }
            return Result.True;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Waiting for Yinmei PPIC Carton Qty Breakdown
        }
    }
}
