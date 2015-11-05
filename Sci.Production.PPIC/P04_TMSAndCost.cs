using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class P04_TMSAndCost : Sci.Win.Subs.Input4
    {
        Ict.Win.UI.DataGridViewNumericBoxColumn colQty = new Ict.Win.UI.DataGridViewNumericBoxColumn();
        Ict.Win.UI.DataGridViewNumericBoxColumn colTms = new Ict.Win.UI.DataGridViewNumericBoxColumn();
        Ict.Win.UI.DataGridViewNumericBoxColumn colPrice = new Ict.Win.UI.DataGridViewNumericBoxColumn();
        private decimal stdTMS;

        public P04_TMSAndCost(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, string styleid)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            this.Text = "TMS & Cost (" + styleid + ")";
            stdTMS = Convert.ToDecimal(MyUtility.GetValue.Lookup("select StdTMS from System"));
        }

        protected override bool OnGridSetup()
        {
            Ict.Win.DataGridViewGeneratorNumericColumnSettings qty = new DataGridViewGeneratorNumericColumnSettings();
            Ict.Win.DataGridViewGeneratorNumericColumnSettings tms = new DataGridViewGeneratorNumericColumnSettings();
            Ict.Win.DataGridViewGeneratorNumericColumnSettings price = new DataGridViewGeneratorNumericColumnSettings();
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
                    dr["Price"] = MyUtility.Check.Empty(e.FormattedValue.ToString()) ? 0 : MyUtility.Math.Round(Convert.ToDecimal(e.FormattedValue.ToString()) / stdTMS, 3);
                    dr.EndEdit();
                    CalculateTtlTMS();
                }
            };
            #endregion

            Helper.Controls.Grid.Generator(this.grid)
                .Text("Seq", header: "Seq#", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("ArtworkTypeID", header: "Artwork Type", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), settings: qty).Get(out colQty)
                .Text("ArtworkUnit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("TMS", header: "TMS", width: Widths.AnsiChars(5), settings: tms).Get(out colTms)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(6), decimal_places: 3, integer_places: 3, maximum: 999.999m, minimum: 0m, settings: price).Get(out colPrice)
                .Text("IsTtlTMS", header: "Ttl TMS", width: Widths.AnsiChars(8), iseditingreadonly: true);

            return true;
        }

        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);
            string sqlCmd = "select ID,IsTMS,IsPrice,IsTtlTMS,Classify from ArtworkType";
            DataTable ArtworkType;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out ArtworkType);

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
                DataRow[] findrow = ArtworkType.Select(string.Format("ID = '{0}'", gridData["ArtworkTypeID"].ToString()));
                if (findrow.Length > 0)
                {
                    gridData["isTms"] = findrow[0]["isTms"].ToString();
                    gridData["isPrice"] = findrow[0]["isPrice"].ToString();
                    gridData["IsTtlTMS"] = findrow[0]["IsTtlTMS"].ToString().ToUpper() == "TRUE" ? "Y" : "N";
                    gridData["Classify"] = findrow[0]["Classify"].ToString();
                    gridData["NewData"] = 0;
                }
                gridData.AcceptChanges();
            }

            #region 計算Ttl TMS
            sqlCmd = string.Format(@"select isnull(sum(TMS),0) as TtlTMS from Style_TmsCost st, ArtworkType at
where st.ArtworkTypeID = at.ID
and at.IsTtlTMS = 1
and st.StyleUkey = {0}",KeyValue1);
            numericBox1.Value = Convert.ToDecimal(MyUtility.GetValue.Lookup(sqlCmd));
            #endregion

            #region 撈新增的ArtworkType
            sqlCmd = string.Format(@"select a.* from (
select a.ID,a.Classify,a.Seq,a.ArtworkUnit,a.IsTMS,a.IsPrice,a.IsTtlTMS,isnull(st.StyleUkey,'') as StyleUkey
from ArtworkType a
left join Style_TmsCost st on a.ID = st.ArtworkTypeID and st.StyleUkey = {0}
where a.SystemType = 'T' and a.Junk = 0) a
where a.StyleUkey = ''", KeyValue1);

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
                newdr["StyleUkey"] = KeyValue1;
                newdr["ArtworkTypeID"] = dr["ID"].ToString();
                newdr["Seq"] = dr["Seq"].ToString();
                newdr["Qty"] = 0;
                newdr["ArtworkUnit"] = dr["ArtworkUnit"].ToString();
                newdr["TMS"] = 0;
                newdr["Price"] = 0;
                newdr["AddName"] = Sci.Env.User.UserID;
                newdr["AddDate"] = DateTime.Now;
                newdr["isTms"] = dr["isTms"].ToString();
                newdr["isPrice"] = dr["isPrice"].ToString();
                newdr["IsTtlTMS"] = dr["IsTtlTMS"].ToString().ToUpper() == "TRUE" ? "Y" : "N";
                newdr["Classify"] = dr["Classify"].ToString();
                newdr["NewData"] = 1;
                datas.Rows.Add(newdr);
            }
            #endregion
            
            datas.DefaultView.Sort = "Seq";
        }

        protected override void OnGridRowChanged()
        {
            base.OnGridRowChanged();
            #region -- 控制Qty,Tms,price 是否允許修改.
            if (EditMode)
            {
                int rowid = grid.GetSelectedRowIndex();

                DataRowView dr = grid.GetData<DataRowView>(rowid);
                if (dr != null)
                {
                    grid.Rows[rowid].Cells[colQty.Index].ReadOnly = MyUtility.Check.Empty(dr["ArtworkUnit"]);
                    grid.Rows[rowid].Cells[colTms.Index].ReadOnly = dr["isTms"].ToString().ToUpper() != "TRUE";
                    grid.Rows[rowid].Cells[colPrice.Index].ReadOnly = dr["isPrice"].ToString().ToUpper() != "TRUE";
                }
            }
            #endregion
        }

        protected override void OnUIConvertToMaintain()
        {
            base.OnUIConvertToMaintain();
            append.Visible = false;
            revise.Visible = false;
            delete.Visible = false;
        }

        protected override void OnMaintainEntered()
        {
            base.OnMaintainEntered();
            foreach (DataRow dr in ((DataTable)gridbs.DataSource).Rows)
            {
                if (dr["NewData"].ToString() == "1" && dr.RowState == DataRowState.Unchanged)
                {
                    dr.SetAdded();
                }
            }
        }

        //計算Ttl TMS
        private void CalculateTtlTMS()
        {
            if ((DataTable)gridbs.DataSource == null)
            {
                numericBox1.Value = 0;
            }
            else
            {
                Object ttlTMS = ((DataTable)gridbs.DataSource).Compute("Sum(Tms)", "IsTtlTMS = 'Y'");
                numericBox1.Value = Convert.ToDecimal(ttlTMS);
            }
        }

        protected override bool OnSaveBefore()
        {
            this.grid.ValidateControl();
            gridbs.EndEdit();
            DataRow[] findData = ((DataTable)gridbs.DataSource).Select("Classify = 'I' and Qty > 0 and Tms = 0");
            if (findData.Length > 0)
            {
                StringBuilder errMsg = new StringBuilder();
                for (int i = 0; i < findData.Length ; i++)
                {
                    errMsg.Append(string.Format("[{0} TMS] is empty!\r\n", findData[i]["ArtworkTypeID"].ToString()));
                }
                MyUtility.Msg.WarningBox(errMsg.ToString());
            }

            foreach (DataRow dr in Datas)
            {
                if (dr["NewData"].ToString() == "1" && MyUtility.Check.Empty(dr["Qty"]) && MyUtility.Check.Empty(dr["TMS"]) && MyUtility.Check.Empty(dr["Price"]))
                {
                    dr.Delete();
                }
            }
            
            return true;
        }

        protected override DualResult OnSavePost()
        {
            string sqlCmd = string.Format(@"select isnull(sum(TMS),0) as TtlTMS from Style_TmsCost st, ArtworkType at
where st.ArtworkTypeID = at.ID
and at.IsTtlTMS = 1
and st.StyleUkey = {0}", KeyValue1);
            decimal cpu = MyUtility.Math.Round(Convert.ToDecimal(MyUtility.GetValue.Lookup(sqlCmd)) / stdTMS, 3);
            IList<String> updateCmds = new List<String>();
            updateCmds.Add(string.Format("update Style set CPU = {0} where Ukey = {1};", Convert.ToString(cpu), KeyValue1));
            updateCmds.Add(string.Format("update Orders set CPU = {0}, CMPPrice = {0}*12 where StyleUkey = {0} and not exists (select 1 from SewingOutput_Detail where OrderId = Orders.ID);", Convert.ToString(cpu), KeyValue1));
            #region 組要更新Order_TMSCost的SQL，已經有Sewing Daily Output的Order就不更新
            sqlCmd = string.Format(@"declare @styleukey bigint;
set @styleukey = {0};

--撈出屬於此Style的沒有Sewing Daily Output的訂單
with NoOutputOrderID
as (select o.ID
from Orders o
where o.StyleUkey = @styleukey and not exists (select 1 from SewingOutput_Detail where OrderId = o.ID)
),
--撈出沒有Sewing Daily Output的訂單的TMS & Cost資料
OrderTMSCost
as
(select ot.ID,ot.ArtworkTypeID
from NoOutputOrderID o
inner join Order_TmsCost ot on o.ID = ot.ID
inner join ArtworkType a on ot.ArtworkTypeID = a.ID and a.IsArtwork = 0
),
--撈出應該要有的全部資料
AllData
as
(select isnull(o.ID,'') as ID,st.ArtworkTypeID,st.Seq,st.Qty,st.ArtworkUnit,st.Tms,st.Price
from Style_TmsCost st
inner join ArtworkType a on st.ArtworkTypeID = a.ID and a.IsArtwork = 0
left join NoOutputOrderID o on 1=1
where st.StyleUkey = @styleukey
),
--撈出要Insert的資料
InsertData
as
(select *,'I' as Status from AllData a where not exists (select 1 from OrderTMSCost o where o.ID = a.ID and o.ArtworkTypeID = a.ArtworkTypeID)
),
--撈出要Update的資料
UpdateData
as
(select *,'U' as Status from AllData a 
where exists (select 1 from OrderTMSCost o where o.ID = a.ID and o.ArtworkTypeID = a.ArtworkTypeID)
)

select * from InsertData
union all
select * from UpdateData", KeyValue1);

            DataTable OrderTMSCost;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out OrderTMSCost);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query Order_TMSCost fail!!\r\n" + result.ToString());
                return failResult;
            }

            foreach (DataRow dr in OrderTMSCost.Rows)
            {
                if (dr["Status"].ToString() == "I")
                {
                    if (!MyUtility.Check.Empty(dr["ID"]))
                    {
                        updateCmds.Add(string.Format("insert into Order_TmsCost(ID,ArtworkTypeID,Seq,Qty,ArtworkUnit,TMS,Price,AddName,AddDate) values ('{0}','{1}','{2}',{3},'{4}',{5},{6},'{7}',GETDATE());",
                            dr["ID"].ToString(), dr["ArtworkTypeID"].ToString(), dr["Seq"].ToString(), dr["Qty"].ToString(), dr["ArtworkUnit"].ToString(), dr["TMS"].ToString(), dr["Price"].ToString(), Sci.Env.User.UserID));
                    }
                }
                else
                {
                    if (dr["Status"].ToString() == "U")
                    {
                        updateCmds.Add(string.Format("update Order_TmsCost set Seq = '{0}', Qty = {1}, ArtworkUnit = '{2}', TMS = {3}, Price = {4}, EditName = '{5}', EditDate= GETDATE() where ID = '{6}' and ArtworkTypeID = '{7}';",
                            dr["Seq"].ToString(), dr["Qty"].ToString(), dr["ArtworkUnit"].ToString(), dr["TMS"].ToString(), dr["Price"].ToString(), Sci.Env.User.UserID, dr["ID"].ToString(), dr["ArtworkTypeID"].ToString()));
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

            return Result.True;
        }
    }
}
