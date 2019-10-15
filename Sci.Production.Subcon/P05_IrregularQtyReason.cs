using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{  
    public partial class P05_IrregularQtyReason : Sci.Win.Subs.Base
    {
        DataRow _masterData;
        DataTable _detailDatas;
        DataTable dtLoad;
        string _ArtWorkReq_ID = string.Empty;
        Ict.Win.UI.DataGridViewComboBoxColumn comb_SubReason;

        public P05_IrregularQtyReason(string ArtWorkReq_ID,DataRow masterData, DataTable detailDatas)
        {
            InitializeComponent();
            this.EditMode = false;
            _masterData = masterData;
            _ArtWorkReq_ID = ArtWorkReq_ID;
            _detailDatas = detailDatas;
        }

        protected override void OnFormLoaded()
        {
            Query();
            cellSubconReason comboSubReason = (cellSubconReason)cellSubconReason.GetGridCell("SQ");

            comboSubReason.EditingControlShowing += (s, e) =>
             {
                 if (s==null || e == null)
                 {
                     return;
                 }

                 var eventArgs = (Ict.Win.UI.DataGridViewEditingControlShowingEventArgs)e;
                 ComboBox cb = eventArgs.Control as ComboBox;
                 if (cb != null)
                 {
                     cb.SelectionChangeCommitted -= this.combo_SelectionChangeCommitted;
                     cb.SelectionChangeCommitted += this.combo_SelectionChangeCommitted;
                 }
             };

            this.gridIrregularQty.IsEditingReadOnly = false;
            this.gridIrregularQty.DataSource = listControlBindingSource1;

            #region Grid欄位設定

            Helper.Controls.Grid.Generator(this.gridIrregularQty)
                .Text("FactoryID", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("ArtworkTypeID", header: "Artwork Type", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("OrderID", header: "SP", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("StyleID", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("BrandID", header: "Brand", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Numeric("StandardQty", header: "Std. Qty", decimal_places: 6, iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Numeric("ReqQty", header: "Req. Qty", decimal_places: 6, iseditingreadonly: true, width: Widths.AnsiChars(10))
                .ComboBox("SubconReasonID", header: "Reason#", width: Widths.AnsiChars(15), settings: comboSubReason, iseditable: false).Get(out comb_SubReason)
                .Text("ReasonDesc", header: "Reason Desc.", iseditingreadonly: true, width: Widths.AnsiChars(15))

                .DateTime("CreateDate", header: "Create Date", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("CreateBy", header: "Create By", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .DateTime("EditBy", header: "Edit Date", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("EditDate", header: "Edit By", iseditingreadonly: true, width: Widths.AnsiChars(10))
                ;
            #endregion
            this.gridIrregularQty.Columns["SubconReasonID"].DefaultCellStyle.BackColor = Color.Pink;

            //for (int i = 0; i < this.gridIrregularQty.Columns.Count; i++)
            //{
            //    this.gridIrregularQty.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //}
        }

        private void combo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string newValue = ((Ict.Win.UI.DataGridViewComboBoxEditingControl)sender).EditingControlFormattedValue.ToString();
            DataRow dr = this.gridIrregularQty.GetDataRow(this.listControlBindingSource1.Position);
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;

            string sqlcmd = $@"select reason from SubconReason where Type='SQ' and id= '{newValue}'";
            DataRow drRow;

            if (MyUtility.Check.Seek(sqlcmd, out drRow))
            {
                dr["ReasonDesc"] = drRow["reason"];
                dr["SubconReasonID"] = newValue;
            }

            dr.EndEdit();
        }

        private void Query()
        {
            string sqlcmd = string.Empty;
            DualResult result;
            sqlcmd = strQuerySqlcmd();

            result = DBProxy.Current.Select(null, sqlcmd, out dtLoad);
            if (result == false) ShowErr(sqlcmd, result);

            listControlBindingSource1.DataSource = dtLoad;
        }

        public bool Check_Irregular_Qty()
        {
            bool Has_Irregular_Qty = false;

            string sqlcmd = string.Empty;
            DualResult result;
            sqlcmd = strQuerySqlcmd();

            result = DBProxy.Current.Select(null, sqlcmd, out dtLoad);
            if (result == false) ShowErr(sqlcmd, result);
            if (dtLoad.Rows.Count > 0)
            {
                Has_Irregular_Qty = true;
            }

            return Has_Irregular_Qty;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                DataTable ModifyTable = (DataTable)listControlBindingSource1.DataSource;

                StringBuilder sqlcmd = new StringBuilder();

                var Insert_Or_Update = dtLoad.AsEnumerable().Where(o => o.Field<string>("SubconReasonID").Trim() != "");

                var deleteTmp = dtLoad.AsEnumerable().Where(o => o.Field<string>("SubconReasonID").Trim() == "");

                foreach (var item in deleteTmp)
                {
                    string OrderID = item["OrderID"].ToString();
                    string ArtworkType = item["ArtworkTypeID"].ToString();
                    sqlcmd.Append($@"delete from ArtworkReq_IrregularQty where OrderID = '{OrderID}' and ArtworkTypeID = '{ArtworkType}'" + Environment.NewLine);
                }

                foreach (var item in Insert_Or_Update)
                {
                    string OrderID = item["OrderID"].ToString();
                    string ArtworkType = item["ArtworkTypeID"].ToString();
                    string SubconReasonID = item["SubconReasonID"].ToString();
                    decimal StandardQty = MyUtility.Convert.GetDecimal(item["StandardQty"]);
                    decimal ReqQty = MyUtility.Convert.GetDecimal(item["ReqQty"]);

                    DataTable dt;

                    DualResult result = DBProxy.Current.Select(null, $@"SELECT * FROM ArtworkReq_IrregularQty WHERE OrderID='{OrderID}' AND ArtworkTypeID='{ArtworkType}'", out dt);
                    if (result)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["SubconReasonID"].ToString() != SubconReasonID && !string.IsNullOrEmpty(SubconReasonID))
                            {
                                sqlcmd.Append($"UPDATE [ArtworkReq_IrregularQty] SET [SubconReasonID]='{SubconReasonID}',EditDate=GETDATE(),EditName='{Sci.Env.User.UserID}'" + Environment.NewLine);
                                sqlcmd.Append($"                                  WHERE OrderID='{OrderID}' AND [ArtworkTypeID]='{ArtworkType}'" + Environment.NewLine);
                            }
                        }
                        else
                        {
                            sqlcmd.Append(@"
INSERT INTO [ArtworkReq_IrregularQty]([OrderID],[ArtworkTypeID],[StandardQty],[ReqQty],[SubconReasonID],[AddDate],[AddName])" + Environment.NewLine);
                            sqlcmd.Append($@"
VALUES ('{OrderID}','{ArtworkType}',{StandardQty},{ReqQty},'{SubconReasonID}',GETDATE(),'{Sci.Env.User.UserID}')" + Environment.NewLine);
                        }
                    }
                }

                if (!MyUtility.Check.Empty(sqlcmd.ToString()))
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        DualResult upResult;
                        try
                        {
                            upResult = DBProxy.Current.Execute(null, sqlcmd.ToString());
                            if (!upResult)
                            {
                                ShowErr(sqlcmd.ToString(), upResult);
                                return;
                            }

                            scope.Complete();
                            scope.Dispose();
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            ShowErr("Commit transaction error.", ex);
                        }
                    }
                }

            }

            this.EditMode = !this.EditMode;
            if (EditMode)
            {
                comb_SubReason.IsEditable = true;
            }
            else
            {
                comb_SubReason.IsEditable = false;
            }
            btnEdit.Text = this.EditMode ? "Save" : "Edit";
            btnClose.Text = this.EditMode ? "Undo" : "Close";
        }

        private string strQuerySqlcmd()
        {
            string sqlcmd = string.Empty;
            sqlcmd = $@"
select 
o.FactoryID
,ai.ArtworkTypeID
,ai.OrderID
,o.StyleID
,o.BrandID
,ai.StandardQty
,[ReqQty] = ReqQty.value + PoQty.value
,ReqQty.value as reqqty , PoQty.value as poqty
,[SubconReasonID] =ai.SubconReasonID
,[ReasonDesc]= (select reason from SubconReason where Type='SQ' and id=ai.SubconReasonID)
,[CreateBy] = (select name from Pass1 where id = ai.AddName)
,[CreateDate] = ai.AddDate
,[EditBy] = (select name from pass1 where id=ai.EditName)
,[EditDate] = ai.EditDate
from ArtworkReq aq 
inner join ArtworkReq_Detail aqd on aq.ID=aqd.ID
inner join ArtworkReq_IrregularQty ai on ai.OrderID = aqd.OrderID and ai.ArtworkTypeID = aq.ArtworkTypeID
inner join Orders o on ai.OrderID=o.ID
outer apply(
	select value = ISNULL(sum(PoQty),0)
    from ArtworkPO_Detail AD
	where OrderID = o.ID and ad.PatternCode= aqd.PatternCode
	and ad.ArtworkId = aqd.ArtworkID
	and ad.ArtworkReqID=''
) PoQty
outer apply(
	select value = ISNULL(sum(ReqQty),0)
	from ArtworkReq_Detail aq2
	where aqd.uKey = aq2.uKey
)ReqQty
where 1=1
and aq.ID='{_ArtWorkReq_ID}'
and ai.ReqQty > ai.StandardQty
";

            return sqlcmd;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "Close")
                Close();
            else
            {
                this.EditMode = !this.EditMode;
                btnEdit.Text = "Edit";
                btnClose.Text = "Close";
                if (EditMode)
                {
                    comb_SubReason.IsEditable = true;
                }
                else
                {
                    comb_SubReason.IsEditable = false;
                }

                //回到檢視模式，並且重新取得資料
                Query();
            }
        }
    }
}
