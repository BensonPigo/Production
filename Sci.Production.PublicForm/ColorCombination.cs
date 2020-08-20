using Ict;
using Ict.Win;
using System;
using System.Data;
using System.Drawing;
using Sci.Data;

namespace Sci.Production.PublicForm
{
    /// <inheritdoc/>
    public partial class ColorCombination : Win.Subs.Base
    {
        private string cutid;
        private string Styleukey;
        private DataTable colortb;
        private DataTable dtQTWith;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorCombination"/> class.
        /// </summary>
        public ColorCombination()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorCombination"/> class.
        /// </summary>
        /// <param name="cID">cID</param>
        /// <param name="styleukey">styleukey</param>
        public ColorCombination(string cID, string styleukey)
        {
            this.InitializeComponent();
            this.cutid = cID;
            this.Styleukey = styleukey;

            this.GetQTWith();  // 參考TRADE的方式抓[QT WITH]資料
            this.Requery();
            this.Color();
        }

        private void Requery()
        {
            this.Helper.Controls.Grid.Generator(this.gridFabric)
            .Text("FabricCode", header: "Fabric#", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("Refno", header: "Refno", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Description", header: "Description", width: Widths.AnsiChars(55), iseditingreadonly: true);

            this.Helper.Controls.Grid.Generator(this.gridColCombin)
            .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true);

            this.Helper.Controls.Grid.Generator(this.gridColorDesc)
            .Text("ID", header: "Color ID", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Name", header: "Color Decription", width: Widths.AnsiChars(18), iseditingreadonly: true);
            this.gridColorDesc.Font = new Font("Arial", 9);

            #region Create header
            string createheader = "Select Article";
            string headername;

            // Order_Fabric
            string headersql = string.Format("Select distinct FabricCode,PatternPanel,FabricPanelCode from Order_FabricCode WITH (NOLOCK) where id = '{0}' order by PatternPanel,FabricCode", this.cutid);
            DataTable headertb0, headertb;
            DualResult sqlresult = DBProxy.Current.Select(null, headersql, out headertb0);
            if (!sqlresult)
            {
                this.ShowErr(headersql, sqlresult);
                return;
            }

            foreach (DataRow dr in headertb0.Rows)
            {
                headername = dr["FabricPanelCode"].ToString().Trim();
                createheader = createheader + string.Format(",case when a.FabricPanelCode='{0}' then Colorid end '{0}' ", dr["FabricPanelCode"].ToString().Trim());

                this.Helper.Controls.Grid.Generator(this.gridColCombin)
                .Text(headername, header: headername, width: Widths.AnsiChars(8), iseditingreadonly: true);
            }

            // BOA
            headersql = string.Format("Select distinct PatternPanel from Order_BOA WITH (NOLOCK) where id = '{0}' and patternpanel!='' order by PatternPanel", this.cutid);
            sqlresult = DBProxy.Current.Select(null, headersql, out headertb);
            if (!sqlresult)
            {
                this.ShowErr(headersql, sqlresult);
                return;
            }

            foreach (DataRow dr in headertb.Rows)
            {
                headername = dr["PatternPanel"].ToString().Trim();
                createheader = createheader + string.Format(",case when a.PatternPanel='{0}' then Colorid end '{0}' ", headername);
                this.Helper.Controls.Grid.Generator(this.gridColCombin)
                .Text(headername, header: headername, width: Widths.AnsiChars(8), iseditingreadonly: true);
            }

            string createtable = createheader + string.Format(" From Order_ColorCombo a WITH (NOLOCK) where id ='{0}' ", this.cutid); // create data
            createheader = createheader + " From Order_ColorCombo a WITH (NOLOCK) where 1=0"; // empty table

            // BOF
            string cmd = $@"
SELECT o.FabricCode,f.Refno,f.Description
FROM Order_BOF o with(nolock) 
LEFT JOIN Fabric f with(nolock) ON o.SCIRefno=f.SCIRefno
where o.id = '{this.cutid}'
ORDER BY o.FabricCode
";

            DataTable gridtb, datatb, gridFabric;
            sqlresult = DBProxy.Current.Select(null, createheader, out gridtb);
            if (!sqlresult)
            {
                this.ShowErr(createheader, sqlresult);
                return;
            }

            sqlresult = DBProxy.Current.Select(null, cmd, out gridFabric);
            if (!sqlresult)
            {
                this.ShowErr(createheader, sqlresult);
                return;
            }
            #endregion

            #region QT 塞資料
            string qtsql = string.Format("Select * from order_FabricCode_Qt a WITH (NOLOCK) where a.id = '{0}'", this.cutid);
            DataTable qttb;
            sqlresult = DBProxy.Current.Select(null, qtsql, out qttb);
            DataTable qttb2 = qttb.Copy();
            if (!sqlresult)
            {
                this.ShowErr(qtsql, sqlresult);
                return;
            }

            // 加入前3列,第一欄設定名稱
            DataRow ndr = gridtb.NewRow();
            ndr["Article"] = "PatternPanel";
            gridtb.Rows.Add(ndr);
            ndr = gridtb.NewRow();
            ndr["Article"] = "Fabric";
            gridtb.Rows.Add(ndr);
            ndr = gridtb.NewRow();
            ndr["Article"] = "QT With";
            gridtb.Rows.Add(ndr);
            foreach (DataRow dr in headertb0.Rows)
            {
                string lc = dr["FabricPanelCode"].ToString();
                string pp = dr["PatternPanel"].ToString();
                string fc = dr["FabricCode"].ToString();
                DataRow[] tbdr = gridtb.Select("Article = 'PatternPanel'");
                tbdr[0][lc] = pp;
                DataRow[] tbdr2 = gridtb.Select("Article = 'Fabric'");
                tbdr2[0][lc] = fc;

                // 產生[QT With]資料
                DataRow[] tbdr3 = gridtb.Select("Article = 'QT With'");
                DataRow[] drQTWith = this.dtQTWith.Select(string.Format("FabricPanelCode = '{0}'", lc));
                if (drQTWith.Length > 0)
                {
                    tbdr3[0][lc] = drQTWith[0]["IsQT"].ToString();
                }
            }

            #endregion

            #region 建立Data
            sqlresult = DBProxy.Current.Select(null, createtable, out datatb);
            if (!sqlresult)
            {
                this.ShowErr(createtable, sqlresult);
                return;
            }

            foreach (DataRow dr in datatb.Rows)
            {
                DataRow[] dr2 = gridtb.Select(string.Format("Article = '{0}'", dr["Article"]));
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

            this.listControlBindingSource1.DataSource = gridtb;
            this.listControlBindingSource3.DataSource = gridFabric;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Color()
        {
            string brandid = MyUtility.GetValue.Lookup("Brandid", this.cutid, "Orders", "ID");
            #region SQL CMD 串出ColorComb 所有Color, 含在Color_Mutiple
            string colorsql = string.Format(
                            @"select distinct * from 
                            (
                                /* 1. 抓 Order_ColorCombo 表層的顏色, 名稱 */
                                Select d.id,d.name
                                from Color d WITH (NOLOCK) , 
                                        (
                                        select distinct a.colorid
	                                        from Order_ColorCombo a WITH (NOLOCK) 
                                            where a.id='{0}'
                                        ) c 
	                            where d.id = c.colorid and d.BrandId = '{1}' 
	                           
	                            union all

                                /* 2. 加上 Order_ColorCombo 裡面有複合色的話, 就再往下帶出內含每個顏色的 id, 名稱  */
                                select g.id,g.name
                                from Color g WITH (NOLOCK) ,
                                (
	                                Select mid,f.colorid,f.brandid 
                                    from Color_Multiple f WITH (NOLOCK) ,
	                                (
                                        Select  d.id as mid,d.ukey 
                                        from Color d WITH (NOLOCK) , 
                                        (
                                            select distinct a.colorid 
                                            from Order_ColorCombo a WITH (NOLOCK) 
                                            where a.id='{0}' 
                                        ) c 
	                                    where d.id = c.colorid and d.BrandId = '{1}' 
	                                    and d.ukey in 
                                        (
                                            Select ColorUkey 
                                            from Color_Multiple WITH (NOLOCK) 
                                            where brandid = '{1}'
                                        )
                                    ) e
                                    where e.ukey = f.ColorUkey and f.Brandid = '{1}' 
                                ) h 
                                where g.brandid = h.brandid and g.id = h.ColorID and g.BrandId = '{1}' 
                            ) ord order by id ",
                            this.cutid,
                            brandid); // 串出所有Colorid
            #endregion

            DualResult sqlresult = DBProxy.Current.Select(null, colorsql, out this.colortb);
            if (!sqlresult)
            {
                this.ShowErr(sqlresult);
                return;
            }

            this.listControlBindingSource2.DataSource = this.colortb;
        }

        private void GetQTWith()
        {
            string sql = string.Format(
                @"with 
	            fabericCode as (Select StyleUkey as myKey, Style_BOFUkey as parentKey, * From Style_FabricCode WITH (NOLOCK) where StyleUkey = '{0}'),
	            bof as (Select StyleUkey as myKey, * From Style_BOF WITH (NOLOCK) where StyleUkey = '{0}'),
	            boa as (Select StyleUkey as myKey, * From Style_BOA WITH (NOLOCK) where StyleUkey = '{0}'),
	            article as (Select StyleUkey as myKey, * From Style_Article WITH (NOLOCK) where StyleUKey = '{0}'),
	            colorCombo as (Select StyleUkey as myKey, * From Style_ColorCombo WITH (NOLOCK) where StyleUkey = '{0}'),
	            qt as (Select StyleUkey as myKey, * From Style_FabricCode_QT WITH (NOLOCK) where StyleUkey = '{0}')

            Select f.myKey, f.StyleUkey, f.FabricPanelCode, f.PatternPanel, f.FabricCode, IsNull(q.IsQt, '') as IsQT
            from bof b
            Inner join fabericCode f on f.parentKey = b.Ukey
            Outer Apply (
                Select Top 1 Concat(qt.QTFabricCode, qt.QTPatternPanel) as IsQT
                From qt
                Where qt.myKey = f.myKey
                and qt.FabricPanelCode = f.FabricPanelCode
	            and qt.SeqNO > (Select distinct SeqNO From qt QR where QR.myKey = qt.myKey and QR.QTFabricPanelCode = f.FabricPanelCode)
	            Order by qt.SeqNO
            ) as q
            Order by f.PatternPanel, f.FabricCode, f.FabricPanelCode;", this.Styleukey);
            DualResult sqlresult = DBProxy.Current.Select(null, sql, out this.dtQTWith);
            if (!sqlresult)
            {
                this.ShowErr(sqlresult);
                return;
            }
        }
    }
}
