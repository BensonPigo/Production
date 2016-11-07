using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.PublicForm
{
    public partial class ColorCombination : Sci.Win.Subs.Base
    {
        private string cutid;
        private DataTable colortb;
        public ColorCombination()
        {
            InitializeComponent();
        }

        public ColorCombination(string cID)
        {
            InitializeComponent();
            cutid = cID;
            
            requery();
            color();
        }
        private void requery()
        {
           Helper.Controls.Grid.Generator(this.gridFab)
           .Text("Article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true);

           Helper.Controls.Grid.Generator(this.gridColorDesc)
          .Text("ID", header: "Color ID", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Name", header: "Color Decription", width: Widths.AnsiChars(20), iseditingreadonly: true);

            #region Create header
            string createheader = "Select Article";
            string headername;
            //Order_Fabric
            string headersql = string.Format("Select distinct FabricCode,PatternPanel,LectraCode from Order_FabricCode where id = '{0}' order by PatternPanel,FabricCode", cutid);
            DataTable headertb;
            DualResult sqlresult = DBProxy.Current.Select(null, headersql, out headertb);
            if(!sqlresult)
            {
                ShowErr(headersql,sqlresult);
                return;
            }
            foreach (DataRow dr in headertb.Rows)
            {
                headername = dr["LectraCode"].ToString().Trim();
                createheader = createheader + string.Format(",case when a.Lectracode='{0}' then Colorid end '{0}' ", dr["LectraCode"].ToString().Trim());
               
            Helper.Controls.Grid.Generator(this.gridFab)
            .Text(headername, header: headername, width: Widths.AnsiChars(8), iseditingreadonly: true);
            }
            //BOA
            headersql = string.Format("Select distinct PatternPanel from Order_BOA where id = '{0}' and patternpanel!='' order by PatternPanel", cutid);
            sqlresult = DBProxy.Current.Select(null, headersql, out headertb);
            if(!sqlresult)
            {
                ShowErr(headersql,sqlresult);
                return;
            }

            foreach (DataRow dr in headertb.Rows)
            {
                headername = dr["PatternPanel"].ToString().Trim();
                createheader = createheader + string.Format(",case when a.PatternPanel='{0}' then Colorid end '{0}' ", headername );
            Helper.Controls.Grid.Generator(this.gridFab)
            .Text(headername, header: headername, width: Widths.AnsiChars(8), iseditingreadonly: true);
            }

            string createtable = createheader + string.Format(" From Order_ColorCombo a where id ='{0}' ", cutid); //create data
            createheader = createheader + " From Order_ColorCombo a where 1=0"; //empty table

            DataTable gridtb,datatb;
            sqlresult = DBProxy.Current.Select(null, createheader, out gridtb);
            if (!sqlresult)
            {
                ShowErr(createheader, sqlresult);
                return;
            }

            #endregion
            #region QT
            string qtsql = string.Format("Select * from order_FabricCode_Qt a where a.id = '{0}'", cutid);
            DataTable qttb;
            sqlresult = DBProxy.Current.Select(null, qtsql, out qttb);
            DataTable qttb2 = qttb.Copy();
            if (!sqlresult)
            {
                ShowErr(qtsql, sqlresult);
                return;
            }
            DataRow ndr = gridtb.NewRow();
            ndr["Article"] = "PatternPanel";
            gridtb.Rows.Add(ndr);
            ndr = gridtb.NewRow();
            ndr["Article"] = "QT FabricCode";
            gridtb.Rows.Add(ndr);
            ndr = gridtb.NewRow();
            ndr["Article"] = "QT Width";
            gridtb.Rows.Add(ndr);
            foreach (DataRow dr in qttb.Rows)
            {
                if (dr["seqno"].ToString() != "")
                {
                    string seqno = MyUtility.Convert.NTOC((MyUtility.Convert.GetInt(dr["SEQNo"])-1),2);
                    DataRow[] drqt = qttb2.Select(string.Format("LectraCode ='{0}' and SEQNO = '{1}'", dr["LecreaCode"],seqno));
                    string patternfab = drqt[0]["LectraCode"].ToString().Trim();
                    string qtpatternfab = drqt[0]["QtWidth"].ToString().Trim();
                    decimal qtpatternwidrh = MyUtility.Convert.GetDecimal(drqt[0]["QtWidth"]);
                    DataRow[] tbdr = gridtb.Select("Article = 'PatternPanel'");
                    tbdr[0][1] = patternfab;
                    tbdr = gridtb.Select("Article = 'LectraCode'");
                    tbdr[0][1] = qtpatternwidrh;
                    tbdr = gridtb.Select("Article = 'QT Width'");
                    tbdr[0][1] = qtpatternwidrh;
                }
            }
            #endregion
            #region 建立Data
           
            sqlresult = DBProxy.Current.Select(null, createtable, out datatb);
            if (!sqlresult)
            {
                ShowErr(createtable, sqlresult);
                return;
            }
            foreach (DataRow dr in datatb.Rows)
            {
                DataRow[] dr2 = gridtb.Select(string.Format("Article = '{0}'",dr["Article"]));
                if (dr2.Length == 0)
                {
                    ndr = gridtb.NewRow();
                    ndr["Article"] = dr["article"];
                    gridtb.Rows.Add(ndr);
                    for (int i = 1; i < datatb.Columns.Count; i++)
                    {
                        if (!MyUtility.Check.Empty(dr[i]))
                        {
                            ndr[i] = dr[i];
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < datatb.Columns.Count; i++)
                    {
                        if (!MyUtility.Check.Empty(dr[i]))
                        {
                            dr2[0][i] = dr[i];
                        }
                    }
                }
                    
                
            }
            #endregion 
            
            
            //gridFab.DataSource = gridtb;
            this.listControlBindingSource1.DataSource = gridtb;
            //gridFab.CellEnter += (s, e) =>
            //{
            //    if (e.RowIndex == -1) return;
            //    DataRow coldr = gridFab.GetDataRow(e.RowIndex); //取得資料列
            //    string str= string.Empty;
            //    string propertyname = gridFab.Columns[e.ColumnIndex].DataPropertyName;//取得欄位名稱
            //    str = coldr[propertyname].ToString();
            //    colortb.DefaultView.RowFilter = string.Format("mid = '{0}'", str);
            //};
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void color()
        {
            string brandid = MyUtility.GetValue.Lookup("Brandid", cutid, "Orders", "ID");
            #region SQL CMD 串出ColorComb 所有Color, 含在Color_Mutiple
            string colorsql = string.Format(
                            @"select distinct * from 
                            (
                                Select d.id as mid, d.id,d.name
                                from Color d, 
                                        (
                                        select distinct a.colorid
	                                        from Order_ColorCombo a 
                                            where a.id='{0}'
                                        ) c 
	                            where d.id = c.colorid and d.BrandId = 'ADIDAS' 
	                            and d.ukey not in 
                                    (
                                        Select ColorUkey 
                                        from Color_Multiple 
                                        where brandid = '{1}'
                                    )
	                            union all
                                select mid,g.id,g.name
                                from Color g,
                                (
	                                Select mid,f.colorid,f.brandid 
                                    from Color_Multiple f,
	                                (
                                        Select  d.id as mid,d.ukey 
                                        from Color d, 
                                        (
                                            select distinct a.colorid 
                                            from Order_ColorCombo a 
                                            where a.id='{0}' 
                                        ) c 
	                                    where d.id = c.colorid and d.BrandId = '{1}' 
	                                    and d.ukey in 
                                        (
                                            Select ColorUkey 
                                            from Color_Multiple 
                                            where brandid = '{1}'
                                        )
                                    ) e
                                    where e.ukey = f.ColorUkey and f.Brandid = '{1}' 
                                ) h 
                                where g.brandid = h.brandid and g.id = h.ColorID and g.BrandId = '{1}' 
                            ) ord order by id ", cutid,brandid); //串出所有Colorid
            #endregion

            DualResult sqlresult = DBProxy.Current.Select(null, colorsql, out colortb);
            if (!sqlresult)
            {
                ShowErr( sqlresult);
                return;
            }
            //gridColorDesc.DataSource = colortb;
            this.listControlBindingSource2.DataSource = colortb;
        }
    }
}
