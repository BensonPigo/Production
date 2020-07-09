using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P04_TMSAndCost
    /// </summary>
    public partial class P04_TMSAndCost : Win.Subs.Input4
    {
        private Ict.Win.UI.DataGridViewNumericBoxColumn colQty = new Ict.Win.UI.DataGridViewNumericBoxColumn();
        private Ict.Win.UI.DataGridViewNumericBoxColumn colTms = new Ict.Win.UI.DataGridViewNumericBoxColumn();
        private Ict.Win.UI.DataGridViewNumericBoxColumn colPrice = new Ict.Win.UI.DataGridViewNumericBoxColumn();
        private decimal stdTMS;

        /// <summary>
        /// P04_TMSAndCost
        /// </summary>
        /// <param name="canedit">bool canedit</param>
        /// <param name="keyvalue1">string keyvalue1</param>
        /// <param name="keyvalue2">string keyvalue2</param>
        /// <param name="keyvalue3">string keyvalue3</param>
        /// <param name="styleid">string styleid</param>
        public P04_TMSAndCost(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, string styleid)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.Text = "TMS & Cost (" + styleid + ")";
            this.stdTMS = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup("select StdTMS from System WITH (NOLOCK) "));
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorNumericColumnSettings qty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings tms = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings price = new DataGridViewGeneratorNumericColumnSettings();
            qty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            tms.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            price.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;

            #region 輸入TMS後自動算出Price與TTL TMS
            tms.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                    dr["TMS"] = e.FormattedValue.ToString();
                    dr["Price"] = MyUtility.Check.Empty(e.FormattedValue.ToString()) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(e.FormattedValue.ToString()) / this.stdTMS, 3);
                    dr.EndEdit();
                    this.CalculateTtlTMS();
                }
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("Seq", header: "Seq#", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("ArtworkTypeID", header: "Artwork Type", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), settings: qty).Get(out this.colQty)
                .Text("ArtworkUnit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("TMS", header: "TMS", width: Widths.AnsiChars(5), settings: tms).Get(out this.colTms)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(6), decimal_places: 3, integer_places: 3, maximum: 999.999m, minimum: 0m, settings: price).Get(out this.colPrice)
                .Text("IsTtlTMS", header: "Ttl TMS", width: Widths.AnsiChars(8), iseditingreadonly: true);

            return true;
        }

        /// <inheritdoc/>
        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);
            string sqlCmd = "select ID,IsTMS,IsPrice,IsTtlTMS,Classify from ArtworkType WITH (NOLOCK) ";
            DataTable artworkType;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out artworkType);

            if (!result)
            {
                MyUtility.Msg.WarningBox("Query ArtworkType fail!!\r\n" + result.ToString());
            }

            datas.Columns.Add("isTms");
            datas.Columns.Add("isPrice");
            datas.Columns.Add("IsTtlTMS");
            datas.Columns.Add("Classify");
            datas.Columns.Add("NewData");
            foreach (DataRow gridData in datas.Rows)
            {
                DataRow[] findrow = artworkType.Select(string.Format("ID = '{0}'", gridData["ArtworkTypeID"].ToString()));
                if (findrow.Length > 0)
                {
                    gridData["isTms"] = findrow[0]["isTms"];
                    gridData["isPrice"] = findrow[0]["isPrice"];
                    gridData["IsTtlTMS"] = findrow[0]["IsTtlTMS"].ToString().ToUpper() == "TRUE" ? "Y" : "N";
                    gridData["Classify"] = findrow[0]["Classify"];
                    gridData["NewData"] = 0;
                }

                gridData.AcceptChanges();
            }

            #region 計算Ttl TMS
            sqlCmd = string.Format(
                @"select isnull(sum(TMS),0) as TtlTMS from Style_TmsCost st WITH (NOLOCK) , ArtworkType at WITH (NOLOCK) 
where st.ArtworkTypeID = at.ID
and at.IsTtlTMS = 1
and st.StyleUkey = {0}", this.KeyValue1);
            this.numTTLTMS.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlCmd));
            #endregion

            #region 撈新增的ArtworkType
            sqlCmd = string.Format(
                @"select a.* from (
select a.ID,a.Classify,a.Seq,a.ArtworkUnit,a.IsTMS,a.IsPrice,a.IsTtlTMS,isnull(st.StyleUkey,'') as StyleUkey
from ArtworkType a WITH (NOLOCK) 
left join Style_TmsCost st WITH (NOLOCK) on a.ID = st.ArtworkTypeID and st.StyleUkey = {0}
where a.SystemType = 'T' and a.Junk = 0) a
where a.StyleUkey = ''", this.KeyValue1);

            DataTable tmpArtworkType;
            result = DBProxy.Current.Select(null, sqlCmd, out tmpArtworkType);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query LackArtworkType fail!!\r\n" + result.ToString());
            }

            DataRow newdr;
            foreach (DataRow dr in tmpArtworkType.Rows)
            {
                newdr = datas.NewRow();
                newdr["StyleUkey"] = this.KeyValue1;
                newdr["ArtworkTypeID"] = dr["ID"];
                newdr["Seq"] = dr["Seq"];
                newdr["Qty"] = 0;
                newdr["ArtworkUnit"] = dr["ArtworkUnit"];
                newdr["TMS"] = 0;
                newdr["Price"] = 0;
                newdr["AddName"] = Env.User.UserID;
                newdr["AddDate"] = DateTime.Now;
                newdr["isTms"] = dr["isTms"];
                newdr["isPrice"] = dr["isPrice"];
                newdr["IsTtlTMS"] = dr["IsTtlTMS"].ToString().ToUpper() == "TRUE" ? "Y" : "N";
                newdr["Classify"] = dr["Classify"];
                newdr["NewData"] = 1;
                datas.Rows.Add(newdr);
            }
            #endregion

            datas.DefaultView.Sort = "Seq";
        }

        /// <inheritdoc/>
        protected override void OnGridRowChanged()
        {
            base.OnGridRowChanged();
            #region 控制Qty,Tms,price 是否允許修改.
            if (this.EditMode)
            {
                int rowid = this.grid.GetSelectedRowIndex();

                DataRowView dr = this.grid.GetData<DataRowView>(rowid);
                if (dr != null)
                {
                    this.grid.Rows[rowid].Cells[this.colQty.Index].ReadOnly = MyUtility.Check.Empty(dr["ArtworkUnit"]);
                    this.grid.Rows[rowid].Cells[this.colTms.Index].ReadOnly = dr["isTms"].ToString().ToUpper() != "TRUE";
                    this.grid.Rows[rowid].Cells[this.colPrice.Index].ReadOnly = dr["isPrice"].ToString().ToUpper() != "TRUE";
                }
            }
            #endregion
        }

        /// <inheritdoc/>
        protected override void OnUIConvertToMaintain()
        {
            base.OnUIConvertToMaintain();
            this.append.Visible = false;
            this.revise.Visible = false;
            this.delete.Visible = false;
        }

        /// <inheritdoc/>
        protected override void OnMaintainEntered()
        {
            base.OnMaintainEntered();
            foreach (DataRow dr in ((DataTable)this.gridbs.DataSource).Rows)
            {
                if (dr["NewData"].ToString() == "1" && dr.RowState == DataRowState.Unchanged)
                {
                    dr.SetAdded();
                }
            }
        }

        // 計算Ttl TMS
        private void CalculateTtlTMS()
        {
            if ((DataTable)this.gridbs.DataSource == null)
            {
                this.numTTLTMS.Value = 0;
            }
            else
            {
                object ttlTMS = ((DataTable)this.gridbs.DataSource).Compute("Sum(Tms)", "IsTtlTMS = 'Y'");
                this.numTTLTMS.Value = MyUtility.Convert.GetDecimal(ttlTMS);
            }
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            this.grid.ValidateControl();
            this.gridbs.EndEdit();
            DataRow[] findData = ((DataTable)this.gridbs.DataSource).Select("Classify = 'I' and Qty > 0 and Tms = 0");
            if (findData.Length > 0)
            {
                StringBuilder errMsg = new StringBuilder();
                for (int i = 0; i < findData.Length; i++)
                {
                    errMsg.Append(string.Format("[{0} TMS] is empty!\r\n", findData[i]["ArtworkTypeID"].ToString()));
                }

                MyUtility.Msg.WarningBox(errMsg.ToString());
            }

            foreach (DataRow dr in this.Datas)
            {
                if (dr["NewData"].ToString() == "1" && MyUtility.Check.Empty(dr["Qty"]) && MyUtility.Check.Empty(dr["TMS"]) && MyUtility.Check.Empty(dr["Price"]))
                {
                    dr.Delete();
                }
            }

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnSavePost()
        {
            string sqlCmd = string.Format(
                @"select isnull(sum(TMS),0) as TtlTMS from Style_TmsCost st WITH (NOLOCK) , ArtworkType at WITH (NOLOCK) 
where st.ArtworkTypeID = at.ID
and at.IsTtlTMS = 1
and st.StyleUkey = {0}", this.KeyValue1);
            decimal cpu = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlCmd)) / this.stdTMS, 3);
            IList<string> updateCmds = new List<string>();
            updateCmds.Add(string.Format("update Style set CPU = {0} where Ukey = {1};", MyUtility.Convert.GetString(cpu), this.KeyValue1));
            updateCmds.Add(string.Format("update Orders set CPU = {0}, CMPPrice = {0}*12 where StyleUkey = {1} and not exists (select 1 from SewingOutput_Detail WITH (NOLOCK) where OrderId = Orders.ID);", MyUtility.Convert.GetString(cpu), this.KeyValue1));
            #region 組要更新Order_TMSCost的SQL，已經有Sewing Daily Output的Order就不更新
            sqlCmd = string.Format(
                @"declare @styleukey bigint;
set @styleukey = {0};

--撈出屬於此Style的沒有Sewing Daily Output的訂單
with NoOutputOrderID
as (select o.ID
from Orders o WITH (NOLOCK) 
where o.StyleUkey = @styleukey and not exists (select 1 from SewingOutput_Detail WITH (NOLOCK) where OrderId = o.ID)
),
--撈出沒有Sewing Daily Output的訂單的TMS & Cost資料
OrderTMSCost
as
(select ot.ID,ot.ArtworkTypeID
from NoOutputOrderID o
inner join Order_TmsCost ot WITH (NOLOCK) on o.ID = ot.ID
inner join ArtworkType a WITH (NOLOCK) on ot.ArtworkTypeID = a.ID 
),
--撈出應該要有的全部資料
AllData
as
(select isnull(o.ID,'') as ID,st.ArtworkTypeID,st.Seq,st.Qty,st.ArtworkUnit,st.Tms,st.Price
from Style_TmsCost st WITH (NOLOCK) 
inner join ArtworkType a WITH (NOLOCK) on st.ArtworkTypeID = a.ID
left join NoOutputOrderID o on 1=1
where st.StyleUkey = @styleukey
),
--撈出要Insert的資料
InsertData
as
(select *,'I' as Status from AllData a where not exists (select 1 from OrderTMSCost o WITH (NOLOCK) where o.ID = a.ID and o.ArtworkTypeID = a.ArtworkTypeID)
),
--撈出要Update的資料
UpdateData
as
(select *,'U' as Status from AllData a 
where exists (select 1 from OrderTMSCost o WITH (NOLOCK) where o.ID = a.ID and o.ArtworkTypeID = a.ArtworkTypeID)
)

select * from InsertData
union all
select * from UpdateData", this.KeyValue1);

            DataTable orderTMSCost;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out orderTMSCost);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query Order_TMSCost fail!!\r\n" + result.ToString());
                return failResult;
            }

            foreach (DataRow dr in orderTMSCost.Rows)
            {
                if (dr["Status"].ToString() == "I")
                {
                    if (!MyUtility.Check.Empty(dr["ID"]))
                    {
                        updateCmds.Add(string.Format(
                            "insert into Order_TmsCost(ID,ArtworkTypeID,Seq,Qty,ArtworkUnit,TMS,Price,AddName,AddDate) values ('{0}','{1}','{2}',{3},'{4}',{5},{6},'{7}',GETDATE());",
                            dr["ID"].ToString(),
                            dr["ArtworkTypeID"].ToString(),
                            dr["Seq"].ToString(),
                            dr["Qty"].ToString(),
                            dr["ArtworkUnit"].ToString(),
                            dr["TMS"].ToString(),
                            dr["Price"].ToString(),
                            Env.User.UserID));
                    }
                }
                else
                {
                    if (dr["Status"].ToString() == "U")
                    {
                        updateCmds.Add(string.Format(
                            "update Order_TmsCost set Seq = '{0}', Qty = {1}, ArtworkUnit = '{2}', TMS = {3}, Price = {4}, EditName = '{5}', EditDate= GETDATE() where ID = '{6}' and ArtworkTypeID = '{7}';",
                            dr["Seq"].ToString(),
                            dr["Qty"].ToString(),
                            dr["ArtworkUnit"].ToString(),
                            dr["TMS"].ToString(),
                            dr["Price"].ToString(),
                            Env.User.UserID,
                            dr["ID"].ToString(),
                            dr["ArtworkTypeID"].ToString()));
                    }
                }
            }
            #endregion
            result = DBProxy.Current.Executes(null, updateCmds);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Update Order_TMSCost fail!!\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override void OnSaveAfter()
        {
            base.OnSaveAfter();
            this.NoConfirmInClose = true;
            this.Close();
        }

        private bool NoConfirmInClose = false;

        /// <inheritdoc/>
        protected override bool NeedUserClosingConfirm()
        {
            return this.NoConfirmInClose ? false : base.NeedUserClosingConfirm();
        }
    }
}
