﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class P04_QtyCartonByCustCD : Sci.Win.Subs.Base
    {
        private string styleUkey;
        public P04_QtyCartonByCustCD(string StyleUkey)
        {
            InitializeComponent();
            styleUkey = StyleUkey;
        }

        protected override void OnFormLoaded()
        {
            //撈Grid資料
            //   select sq.* ,isnull(d.Name,'') as ContinentName, isnull(c.Alias,'') as Alias, '' as CreateBy,'' as EditBy 
            //from Style_QtyCTN sq
            //left join DropDownList d on d.Type = 'Continent' and sq.Continent = d.ID
            //left join Country c on sq.CountryID = c.ID order by s.Continent,s.CountryID
           string sqlCmd = string.Format(@"
SELECT DISTINCT isnull(d.Name,'') as ContinentName,S.CountryID,isnull(c.Alias,'') as Alias,S.CustCDID,S.Qty,S.AddName,S.AddDate,S.EditName,S.EditDate, '' as CreateBy,'' as EditBy 
FROM Style_QtyCTN S WITH (NOLOCK) 
LEFT JOIN Country C WITH (NOLOCK) ON S.CountryID=C.ID
left join DropDownList d WITH (NOLOCK) on d.Type = 'Continent' and s.Continent = d.ID
where s.StyleUkey = {0}", styleUkey);

            DataTable selectDataTable;
            DualResult selectResult1 = DBProxy.Current.Select(null, sqlCmd, out selectDataTable);
            foreach (DataRow gridData in selectDataTable.Rows)
            {
                gridData["CreateBy"] = gridData["AddName"].ToString() + " " + (MyUtility.Check.Empty(gridData["AddDate"]) ? "" : ((DateTime)gridData["AddDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat)));
                gridData["EditBy"] = gridData["EditName"].ToString() + "  " + (MyUtility.Check.Empty(gridData["EditDate"]) ? "" : ((DateTime)gridData["EditDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat)));
            }
            
            listControlBindingSource1.DataSource = selectDataTable;

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("ContinentName", header: "Continent", width: Widths.AnsiChars(15))
                 .Text("CountryID", header: "Country Id", width: Widths.AnsiChars(2))
                 .Text("Alias", header: "Country", width: Widths.AnsiChars(10))
                 .Text("CustCDID", header: "CustCD", width: Widths.AnsiChars(10))
                 .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5))
                 .Text("CreateBy", header: "Create by", width: Widths.AnsiChars(28))
                 .Text("EditBy", header: "Edit by", width: Widths.AnsiChars(28));
        }
    }
}
