using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P04_Artwork
    /// </summary>
    public partial class P04_Artwork : Sci.Win.Subs.Input4
    {
        /// <summary>
        /// P04_Artwork
        /// </summary>
        /// <param name="canedit">bool canedit</param>
        /// <param name="keyvalue1">string keyvalue1</param>
        /// <param name="keyvalue2">string keyvalue2</param>
        /// <param name="keyvalue3">string keyvalue3</param>
        /// <param name="styleid">string styleid</param>
        /// <param name="seasonid">string seasonid</param>
        public P04_Artwork(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, string styleid, string seasonid)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.Text = "Artwork <" + styleid + "-" + seasonid + ">";
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            Ict.Win.DataGridViewGeneratorTextColumnSettings artworktype = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            Ict.Win.DataGridViewGeneratorTextColumnSettings cutpartdesc = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            Ict.Win.DataGridViewGeneratorTextColumnSettings patterndesc = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            Ict.Win.DataGridViewGeneratorTextColumnSettings remark = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            cutpartdesc.CharacterCasing = CharacterCasing.Normal;
            patterndesc.CharacterCasing = CharacterCasing.Normal;
            remark.CharacterCasing = CharacterCasing.Normal;
            #region Artwork Type的Right Click & Validating
            artworktype.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,ArtworkUnit from ArtworkType WITH (NOLOCK) where Junk = 0 and IsArtwork = 1", "20", dr["ArtworkTypeID"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            IList<DataRow> list = item.GetSelecteds();
                            dr["ArtworkTypeID"] = item.GetSelectedString();
                            dr["UnitID"] = list[0]["ArtworkUnit"].ToString();
                            dr["Article"] = "----";
                            dr.EndEdit();
                        }
                    }
                }
            };

            artworktype.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                    if (!string.IsNullOrWhiteSpace(e.FormattedValue.ToString()))
                    {
                        // sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@artworktype", e.FormattedValue.ToString());

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);

                        DataTable artworkType;
                        string sqlCmd = "select ArtworkUnit from ArtworkType WITH (NOLOCK) where Junk = 0 and IsArtwork = 1 and ID = @artworktype";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out artworkType);
                        if (!result || artworkType.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("< Artwork Type: {0} > not found!!!", e.FormattedValue.ToString()));
                            }

                            dr["ArtworkTypeID"] = string.Empty;
                            dr["UnitID"] = string.Empty;
                            e.Cancel = true;
                            dr.EndEdit();
                            return;
                        }
                        else
                        {
                            dr["ArtworkTypeID"] = e.FormattedValue.ToString();
                            dr["UnitID"] = artworkType.Rows[0]["ArtworkUnit"];
                            dr["Article"] = "----";
                            dr.EndEdit();
                        }
                    }
                }
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("ArtworkTypeID", header: "Artwork Type", width: Widths.AnsiChars(20), settings: artworktype)
                .Text("Article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("PatternCode", header: "Cut Part", width: Widths.AnsiChars(10))
                .Text("PatternDesc", header: "Description", width: Widths.AnsiChars(20), settings: cutpartdesc)
                .Text("ArtworkID", header: "Pattern#", width: Widths.AnsiChars(15))
                .Text("ArtworkName", header: "Pattern Description", width: Widths.AnsiChars(30), settings: patterndesc)
                .Numeric("Qty", header: string.Empty, width: Widths.AnsiChars(5))
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("TMS", header: "TMS", width: Widths.AnsiChars(5))
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(8), decimal_places: 4, integer_places: 4, maximum: 9999.9999m, minimum: 0m)
                .Numeric("Cost", header: "Cost", width: Widths.AnsiChars(8), decimal_places: 4, integer_places: 4, maximum: 9999.9999m, minimum: 0m)
                .EditText("Remark", header: "Remark", width: Widths.AnsiChars(30), settings: remark)
                .Text("CreateBy", header: "Create By", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Text("EditBy", header: "Edit By", width: Widths.AnsiChars(30), iseditingreadonly: true);

            return true;
        }

        /// <inheritdoc/>
        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);
            string sqlCmd = string.Format(
                @"select sa.ArtworkTypeID,a.ArtworkUnit 
from Style_Artwork sa WITH (NOLOCK) 
left join ArtworkType a WITH (NOLOCK) on sa.ArtworkTypeID = a.ID
where StyleUkey = {0}", this.KeyValue1);
            DataTable artworkUnit;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out artworkUnit);

            if (!result)
            {
                MyUtility.Msg.WarningBox("Query unit fail!!\r\n" + result.ToString());
            }

            datas.Columns.Add("UnitID");
            datas.Columns.Add("CreateBy");
            datas.Columns.Add("EditBy");
            foreach (DataRow gridData in datas.Rows)
            {
                gridData["CreateBy"] = gridData["AddName"].ToString() + "   " + ((DateTime)gridData["AddDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                if (gridData["EditDate"] != System.DBNull.Value)
                {
                    gridData["EditBy"] = gridData["EditName"].ToString() + "   " + ((DateTime)gridData["EditDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                }

                DataRow[] findrow = artworkUnit.Select(string.Format("ArtworkTypeID = '{0}'", gridData["ArtworkTypeID"].ToString()));
                if (findrow.Length > 0)
                {
                    gridData["UnitID"] = findrow[0]["ArtworkUnit"].ToString();
                }

                gridData.AcceptChanges();
            }
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            this.grid.ValidateControl();
            this.gridbs.EndEdit();
            DataRow[] findData = ((DataTable)this.gridbs.DataSource).Select("ArtworkTypeID = '' or PatternCode = '' or ArtworkID = ''");
            if (findData.Length > 0)
            {
                MyUtility.Msg.WarningBox("< Artwork Type > and < Cut Part > and < Pattern# > can't empty!!");
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnSavePost()
        {
            #region 更新Order_Artwork，已經有Sewing Daily Output的Order就不更新
            string sqlCmd = string.Format(
                @"declare @styleukey bigint;
set @styleukey = {0};

--撈出屬於此Style的沒有Sewing Daily Output的訂單
with NoOutputOrderID
as (select o.ID
from Orders o WITH (NOLOCK) 
where o.StyleUkey = @styleukey and not exists (select 1 from SewingOutput_Detail WITH (NOLOCK) where OrderId = o.ID)
),
--撈出沒有Sewing Daily Output的訂單的Artwork資料
OrderArtwork
as
(select oa.ID,oa.ArtworkTypeID,oa.Article,oa.PatternCode,oa.PatternDesc,oa.ArtworkID,oa.ArtworkName,oa.Qty,oa.Tms,oa.Price,oa.Cost,oa.Remark,oa.Ukey
from NoOutputOrderID o
inner join Order_Artwork oa WITH (NOLOCK) on o.ID = oa.ID
),
--撈出應該要有的全部資料
AllData
as
(select isnull(o.ID,'') as ID,sa.ArtworkTypeID,sa.Article,sa.PatternCode,sa.PatternDesc,sa.ArtworkID,sa.ArtworkName,sa.Qty,sa.Tms,sa.Price,sa.Cost,sa.Remark,0 as Ukey
from Style_Artwork sa
left join NoOutputOrderID o on 1=1
where sa.StyleUkey = @styleukey
),
--撈出要Insert的資料
InsertData
as
(select *,'I' as Status from AllData a where not exists (select 1 from OrderArtwork o where o.ID = a.ID and o.ArtworkTypeID = a.ArtworkTypeID and o.PatternCode = a.PatternCode and o.ArtworkID = a.ArtworkID)
),
--撈出要Delete的資料
DeleteData
as
(select *,'D' as Status from OrderArtwork o where not exists (select 1 from AllData a where o.ID = a.ID and o.ArtworkTypeID = a.ArtworkTypeID and o.PatternCode = a.PatternCode and o.ArtworkID = a.ArtworkID)
),
--撈出要Update的資料
UpdateData
as
(select a.ID,a.ArtworkTypeID,a.Article,a.PatternCode,a.PatternDesc,a.ArtworkID,a.ArtworkName,a.Qty,a.Tms,a.Price,a.Cost,a.Remark,(select Ukey from OrderArtwork o where o.ID = a.ID and o.ArtworkTypeID = a.ArtworkTypeID and o.Article = a.Article and o.PatternCode = a.PatternCode and o.ArtworkID = a.ArtworkID) as Ukey,'U' as Status from AllData a 
where exists (select 1 from OrderArtwork o where o.ID = a.ID and o.ArtworkTypeID = a.ArtworkTypeID and o.Article = a.Article and o.PatternCode = a.PatternCode and o.ArtworkID = a.ArtworkID)
)

select * from InsertData
union all
select * from DeleteData
union all
select * from UpdateData", this.KeyValue1);
            DataTable orderArtwork;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out orderArtwork);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query Order_Artwork fail!!\r\n" + result.ToString());
                return failResult;
            }

            IList<string> cmds = new List<string>();
            foreach (DataRow dr in orderArtwork.Rows)
            {
                if (dr["Status"].ToString() == "I")
                {
                    if (!MyUtility.Check.Empty(dr["ID"]))
                    {
                        cmds.Add(string.Format(
                            "insert into Order_Artwork(ID,ArtworkTypeID,Article,PatternCode,PatternDesc,ArtworkID,ArtworkName,Qty,Tms,Price,Cost,Remark,AddName,AddDate,Ukey) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},{8},{9},{10},'{11}','{12}',GETDATE(),{13});",
                            dr["ID"].ToString(),
                            dr["ArtworkTypeID"].ToString(),
                            dr["Article"].ToString(),
                            dr["PatternCode"].ToString(),
                            dr["PatternDesc"].ToString(),
                            dr["ArtworkID"].ToString(),
                            dr["ArtworkName"].ToString(),
                            dr["Qty"].ToString(),
                            dr["Tms"].ToString(),
                            dr["Price"].ToString(),
                            dr["Cost"].ToString(),
                            dr["Remark"].ToString(),
                            Sci.Env.User.UserID,
                            "(select min(Ukey)-1 from Order_Artwork)"));
                    }
                }
                else
                {
                    if (dr["Status"].ToString() == "U")
                    {
                        cmds.Add(string.Format(
                            "update Order_Artwork set PatternDesc = '{0}',ArtworkName = '{1}', Qty = {2}, Tms = {3}, Price = {4}, Cost = {5}, Remark = '{6}', EditName = '{7}', EditDate = GETDATE() where Ukey = {8};",
                            dr["PatternDesc"].ToString(),
                            dr["ArtworkName"].ToString(),
                            dr["Qty"].ToString(),
                            dr["Tms"].ToString(),
                            dr["Price"].ToString(),
                            dr["Cost"].ToString(),
                            dr["Remark"].ToString(),
                            Sci.Env.User.UserID,
                            dr["Ukey"].ToString()));
                    }
                    else
                    {
                        cmds.Add(string.Format("delete from Order_Artwork where Ukey = {0};", dr["Ukey"].ToString()));
                    }
                }
            }

            if (cmds.Count > 0)
            {
                result = DBProxy.Current.Executes(null, cmds);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Update Order_Artwork fail!!\r\n" + result.ToString());
                    return failResult;
                }
            }
            #endregion

            #region 更新Order_TMSCost(已經有Sewing Daily Output的Order就不更新)與Style_TMSCost
            sqlCmd = string.Format(
                @"declare @styleukey bigint;
set @styleukey = {0};
--先將要更新的Style的TMS,Qty,Cost做加總
with TMSCost
as
(select * from (
select ArtworkTypeID,isnull(sum(TMS),0) as TMS, isnull(sum(Qty),0) as Stitch, isnull(sum(Cost),0) as Cost, COUNT(ArtworkTypeID) as CNTAID
from Style_Artwork WITH (NOLOCK) where StyleUkey = @styleukey group by ArtworkTypeID) a
where a.TMS <> 0 or a.Stitch <> 0 or a.Cost <> 0
),
--撈出屬於此Style的沒有Sewing Daily Output的訂單
NoOutputOrderID
as 
(select o.ID, o.Qty
from Orders o WITH (NOLOCK) 
where o.StyleUkey = @styleukey and not exists (select 1 from SewingOutput_Detail WITH (NOLOCK) where OrderId = o.ID)
),
--撈出要更新的Order_TMSCost資料
OrderTMSCost
as 
(select 'ORDER' as TableName,t.ArtworkTypeID,iif(t.CNTAID <> 0,cast(t.TMS*n.Qty as float)/(t.CNTAID*n.Qty),0) as TMS,
iif(t.CNTAID <> 0,cast(t.Stitch*n.Qty as float)/(t.CNTAID*n.Qty),0) as Stitch,iif(t.CNTAID <> 0,cast(t.Cost*n.Qty as float)/(t.CNTAID*n.Qty),0) as Cost,
n.ID as OrderID, isnull(ot.ArtworkTypeID,'') as OrderArtWork, a.Seq, a.ArtworkUnit,n.Qty
from TMSCost t 
inner join NoOutputOrderID n on 1=1
inner join Order_TmsCost ot WITH (NOLOCK) on n.ID = ot.ID and t.ArtworkTypeID = ot.ArtworkTypeID
left join ArtworkType a WITH (NOLOCK) on ot.ArtworkTypeID = a.ID
),
--撈出要更新的Style_TMSCost資料
StyleTMSCost
as
(select 'STYLE' as TableName,t.ArtworkTypeID,iif(t.CNTAID <> 0,cast(t.TMS as float)/t.CNTAID,0) as TMS,
iif(t.CNTAID <> 0,cast(t.Stitch as float)/t.CNTAID,0) as Stitch,iif(t.CNTAID <> 0,cast(t.Cost as float)/t.CNTAID,0) as Cost,
'' as OrderID, t.ArtworkTypeID as OrderArtWork, a.Seq, a.ArtworkUnit,0 as QTY
from TMSCost t
left join Style_TmsCost st WITH (NOLOCK) on st.StyleUkey = @styleukey and st.ArtworkTypeID = t.ArtworkTypeID
left join ArtworkType a WITH (NOLOCK) on st.ArtworkTypeID = a.ID
)

select * from OrderTMSCost
union all
select * from StyleTMSCost", this.KeyValue1);

            DataTable allTMSCost;
            result = DBProxy.Current.Select(null, sqlCmd, out allTMSCost);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query TMSCost fail!!\r\n" + result.ToString());
                return failResult;
            }

            IList<string> updateCmds = new List<string>();
            foreach (DataRow dr in allTMSCost.Rows)
            {
                if (dr["TableName"].ToString() == "ORDER")
                {
                    if (dr["OrderArtWork"].ToString() == string.Empty)
                    {
                        updateCmds.Add(string.Format(
                            "insert into Order_TmsCost (ID,ArtworkTypeID,Seq,Qty,ArtworkUnit,TMS,Price,AddName,AddDate) values ('{0}','{1}','{2}',{3},'{4}',{5},{6},'{7}',GETDATE());",
                            dr["OrderID"].ToString(),
                            dr["ArtworkTypeID"].ToString(),
                            dr["Seq"].ToString(),
                            dr["Stitch"].ToString(),
                            dr["ArtworkUnit"].ToString(),
                            dr["TMS"].ToString(),
                            dr["Cost"].ToString(),
                            Sci.Env.User.UserID));
                    }
                    else
                    {
                        updateCmds.Add(string.Format(
                            "update Order_TmsCost set Qty = {0}, TMS = {1}, Price = {2}, EditName = '{3}', EditDate = GETDATE() where ID = '{4}' and ArtworkTypeID = '{5}';",
                            dr["Stitch"].ToString(),
                            dr["TMS"].ToString(),
                            dr["Cost"].ToString(),
                            Sci.Env.User.UserID,
                            dr["OrderID"].ToString(),
                            dr["ArtworkTypeID"].ToString()));
                    }
                }
                else
                {
                    if (dr["OrderArtWork"].ToString() == string.Empty)
                    {
                        updateCmds.Add(string.Format(
                            "insert into Style_TmsCost (StyleUkey,ArtworkTypeID,Seq,Qty,ArtworkUnit,TMS,Price,AddName,AddDate) values ({0},'{1}','{2}',{3},'{4}',{5},{6},'{7}',GETDATE());",
                            this.KeyValue1,
                            dr["ArtworkTypeID"].ToString(),
                            dr["Seq"].ToString(),
                            dr["Stitch"].ToString(),
                            dr["ArtworkUnit"].ToString(),
                            dr["TMS"].ToString(),
                            dr["Cost"].ToString(),
                            Sci.Env.User.UserID));
                    }
                    else
                    {
                        updateCmds.Add(string.Format(
                            "update Style_TmsCost set Qty = {0}, TMS = {1}, Price = {2}, EditName = '{3}', EditDate = GETDATE() where StyleUkey = {4} and ArtworkTypeID = '{5}';",
                            dr["Stitch"].ToString(),
                            dr["TMS"].ToString(),
                            dr["Cost"].ToString(),
                            Sci.Env.User.UserID,
                            this.KeyValue1,
                            dr["ArtworkTypeID"].ToString()));
                    }
                }
            }

            if (updateCmds.Count > 0)
            {
                result = DBProxy.Current.Executes(null, updateCmds);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Update TMSCost fail!!\r\n" + result.ToString());
                    return failResult;
                }
            }
            #endregion

            return Result.True;
        }
    }
}
