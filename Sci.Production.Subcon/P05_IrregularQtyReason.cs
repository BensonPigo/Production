using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using Sci.Win.UI;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class P05_IrregularQtyReason : Win.Subs.Base
    {
        private DataRow _masterData;
        private DataTable _detailDatas;
        private DataTable dtLoad;
        private string _ArtWorkReq_ID = string.Empty;
        private Ict.Win.UI.DataGridViewTextBoxColumn txt_SubReason;
        private P05 p05;
        private Func<string, string> sqlGetBuyBackDeduction;

        /// <inheritdoc/>
        public P05_IrregularQtyReason(string artWorkReq_ID, DataRow masterData, DataTable detailDatas, Func<string, string> sqlGetBuyBackDeduction)
        {
            this.InitializeComponent();
            this.EditMode = false;
            this._masterData = masterData;
            this._ArtWorkReq_ID = artWorkReq_ID;
            this._detailDatas = detailDatas;
            this.sqlGetBuyBackDeduction = sqlGetBuyBackDeduction;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.p05 = (P05)this.ParentIForm;
            TxtSubconReason.CellSubconReason txtSubReason = (TxtSubconReason.CellSubconReason)TxtSubconReason.CellSubconReason.GetGridtxtCell("SQ");

            // comboSubReason.EditingControlShowing += (s, e) =>
            // {
            //     if (s==null || e == null)
            //     {
            //         return;
            //     }

            // var eventArgs = (Ict.Win.UI.DataGridViewEditingControlShowingEventArgs)e;
            //     ComboBox cb = eventArgs.Control as ComboBox;
            //     if (cb != null)
            //     {
            //         cb.SelectionChangeCommitted -= this.combo_SelectionChangeCommitted;
            //         cb.SelectionChangeCommitted += this.combo_SelectionChangeCommitted;
            //     }
            // };
            this.Query();
            this.gridIrregularQty.IsEditingReadOnly = false;
            this.gridIrregularQty.DataSource = this.listControlBindingSource1;

            #region Grid欄位設定
            DataGridViewGeneratorNumericColumnSettings tsStdQty = new DataGridViewGeneratorNumericColumnSettings();

            tsStdQty.CellMouseDoubleClick += (s, e) =>
            {
                DataTable dtMsg = ((DataTable)this.listControlBindingSource1.DataSource).Clone();
                dtMsg.ImportRow(this.gridIrregularQty.GetDataRow(e.RowIndex));
                MsgGridForm msgGridForm = new MsgGridForm(dtMsg, "Buy Back Qty", "Buy Back Qty", "orderID,StandardQty,BuyBackArtworkReq");
                msgGridForm.grid1.Columns[0].HeaderText = "SP";
                msgGridForm.grid1.Columns[1].HeaderText = "Order\r\nQty";
                msgGridForm.grid1.Columns[2].HeaderText = "Buy Back\r\nQty";
                msgGridForm.grid1.AutoResizeColumns();
                msgGridForm.grid1.Columns[0].Width = 120;
                msgGridForm.ShowDialog();
            };

            this.Helper.Controls.Grid.Generator(this.gridIrregularQty)
                .Text("FactoryID", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("ArtworkTypeID", header: "Artwork Type", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("OrderID", header: "SP", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("StyleID", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("BrandID", header: "Brand", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Numeric("StandardQty", header: "Std. Qty", decimal_places: 0, iseditingreadonly: true, width: Widths.AnsiChars(10), settings: tsStdQty)
                .Numeric("ReqQty", header: "Req. Qty", decimal_places: 0, iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("SubconReasonID", header: "Reason# ", width: Widths.AnsiChars(15), iseditable: false, settings: txtSubReason).Get(out this.txt_SubReason)
                .Text("ReasonDesc", header: "Reason Desc.", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .DateTime("CreateDate", header: "Create" + Environment.NewLine + "Date", iseditingreadonly: true, width: Widths.AnsiChars(16))
                .Text("CreateBy", header: "Create" + Environment.NewLine + "By", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .DateTime("EditBy", header: "Edit" + Environment.NewLine + "Date", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("EditDate", header: "Edit" + Environment.NewLine + "By", iseditingreadonly: true, width: Widths.AnsiChars(10))
                ;
            #endregion
            this.gridIrregularQty.Columns["SubconReasonID"].DefaultCellStyle.BackColor = Color.Pink;

            for (int i = 0; i < this.gridIrregularQty.Columns.Count; i++)
            {
                this.gridIrregularQty.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void Combo_SelectionChangeCommitted(object sender, EventArgs e)
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
            this.listControlBindingSource1.DataSource = this.GetData();
        }

        /// <inheritdoc/>
        public DataTable Check_Irregular_Qty()
        {
            DataTable dt = this.GetData();
            this.listControlBindingSource1.DataSource = dt;

            return dt;
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                DataTable modifyTable = (DataTable)this.listControlBindingSource1.DataSource;

                StringBuilder sqlcmd = new StringBuilder();

                var insert_Or_Update = this.dtLoad.AsEnumerable().Where(o => o.Field<string>("SubconReasonID").Trim() != string.Empty);

                var deleteTmp = this.dtLoad.AsEnumerable().Where(o => o.Field<string>("SubconReasonID").Trim() == string.Empty);

                foreach (var item in deleteTmp)
                {
                    string orderID = item["OrderID"].ToString();
                    string artworkType = item["ArtworkTypeID"].ToString();
                    sqlcmd.Append($@"delete from ArtworkReq_IrregularQty where OrderID = '{orderID}' and ArtworkTypeID = '{artworkType}'" + Environment.NewLine);
                }

                foreach (var item in insert_Or_Update)
                {
                    string orderID = item["OrderID"].ToString();
                    string artworkType = item["ArtworkTypeID"].ToString();
                    string subconReasonID = item["SubconReasonID"].ToString();
                    decimal standardQty = MyUtility.Convert.GetDecimal(item["StandardQty"]);
                    decimal reqQty = MyUtility.Convert.GetDecimal(item["ReqQty"]);

                    DataTable dt;

                    DualResult result = DBProxy.Current.Select(null, $@"SELECT * FROM ArtworkReq_IrregularQty WHERE OrderID='{orderID}' AND ArtworkTypeID='{artworkType}'", out dt);
                    if (result)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["SubconReasonID"].ToString() != subconReasonID && !string.IsNullOrEmpty(subconReasonID))
                            {
                                sqlcmd.Append($"UPDATE [ArtworkReq_IrregularQty] SET [SubconReasonID]='{subconReasonID}',EditDate=GETDATE(),EditName='{Env.User.UserID}'" + Environment.NewLine);
                                sqlcmd.Append($"                                  WHERE OrderID='{orderID}' AND [ArtworkTypeID]='{artworkType}'" + Environment.NewLine);
                            }
                        }
                        else
                        {
                            sqlcmd.Append(@"
INSERT INTO [ArtworkReq_IrregularQty]([OrderID],[ArtworkTypeID],[StandardQty],[ReqQty],[SubconReasonID],[AddDate],[AddName])" + Environment.NewLine);
                            sqlcmd.Append($@"
VALUES ('{orderID}','{artworkType}',{standardQty},{reqQty},'{subconReasonID}',GETDATE(),'{Env.User.UserID}')" + Environment.NewLine);
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
                                scope.Dispose();
                                this.ShowErr(sqlcmd.ToString(), upResult);
                                return;
                            }

                            scope.Complete();
                            scope.Dispose();
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            this.ShowErr("Commit transaction error.", ex);
                        }
                    }
                }
            }

            this.Query();
            this.EditMode = !this.EditMode;
            if (this.EditMode)
            {
                this.txt_SubReason.IsEditable = true;
            }
            else
            {
                this.txt_SubReason.IsEditable = false;
            }

            this.btnEdit.Text = this.EditMode ? "Save" : "Edit";
            this.btnClose.Text = this.EditMode ? "Undo" : "Close";
        }

        /// <summary>
        /// GetData
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetData()
        {
            string sqlcmd = string.Empty;

            DualResult result;
            sqlcmd = $@"
alter table #tmp alter column OrderID varchar(13)
alter table #tmp alter column ArtworkID varchar(20)
alter table #tmp alter column PatternCode varchar(20)
alter table #tmp alter column PatternDesc varchar(40)

select  t.OrderID,
        [Article] = '',
        [SizeCode] = '',
        t.ArtworkID,
        t.PatternCode,
        t.PatternDesc,
        [LocalSuppID] = '',
        [OrderQty] = o.Qty,
        [ReqQty] = sum(t.ReqQty)
into #FinalArtworkReq
from #tmp t
inner join orders o with (nolock) on o.ID = t.OrderID
group by    t.OrderID,
            t.PatternCode,
            t.PatternDesc,
            t.ArtworkID,
            o.Qty

{this.sqlGetBuyBackDeduction(this._masterData["artworktypeid"].ToString())}

-- exists DB
select distinct
o.FactoryID
,ArtworkTypeID = '{this._masterData["ArtworkTypeID"]}'
,ai.OrderID
,o.StyleID
,o.BrandID
,ai.StandardQty
,ai.ReqQty
,ai.SubconReasonID
,[ReasonDesc] = (select reason from SubconReason where Type='SQ' and id=ai.SubconReasonID)
,[CreateBy] = (select name from Pass1 where id = ai.AddName)
,[CreateDate] = ai.AddDate
,[EditBy] = (select name from pass1 where id=ai.EditName)
,[EditDate] = ai.EditDate
,ArtworkID=''
,PatternCode=''
,PatternDesc=''
,[NeedUpdate] = 0
,[NeedDelete] = 0
into #tmpDB
from ArtworkReq_IrregularQty ai
inner join Orders o on o.ID = ai.OrderID
where   ai.ArtworkTypeID like '{this._masterData["ArtworkTypeID"]}%' and
        exists(select 1 from #FinalArtworkReq s where s.OrderID = ai.OrderID )

-- not exists DB
select 
o.FactoryID
,ArtworkTypeID = '{this._masterData["ArtworkTypeID"]}'
,[OrderID] = o.ID
,o.StyleID
,o.BrandID
,[StandardQty] = sum(oq.Qty)
,[ReqQty] = ReqQty.value + PoQty.value + s.ReqQty + isnull(tbbd.BuyBackArtworkReq,0)
,[SubconReasonID] = ''
,[ReasonDesc] = ''
,[CreateBy] = ''
,[CreateDate] = null
,[EditBy] = ''
,[EditDate] = null
,s.ArtworkID
,s.PatternCode
,s.PatternDesc
,[BuyBackArtworkReq] = isnull(tbbd.BuyBackArtworkReq,0)
into #tmpCurrent
from  orders o WITH (NOLOCK) 
inner join order_qty oq WITH (NOLOCK) on oq.id = o.ID
inner join #FinalArtworkReq s  on s.OrderID = o.ID 
left join #tmpBuyBackDeduction tbbd on  tbbd.OrderID = s.OrderID       and
                                        tbbd.Article = s.Article       and
                                        tbbd.SizeCode = s.SizeCode     and
                                        tbbd.PatternCode = s.PatternCode   and
                                        tbbd.PatternDesc = s.PatternDesc   and
                                        tbbd.ArtworkID = s.ArtworkID and
										tbbd.LocalSuppID = ''
outer apply(
	select value = ISNULL(sum(PoQty),0)
    from ArtworkPO_Detail ad, ArtworkPO a
	where ad.ID = a.ID
    and OrderID = o.ID 
    and ad.PatternCode = isnull(s.PatternCode,'')
	and ad.PatternDesc = isnull(s.PatternDesc,'') 
    and ad.ArtworkID = iif(s.ArtworkID is null,'{this._masterData["ArtworkTypeID"]}',s.ArtworkID)
	and ad.ArtworkReqID =''
	and a.ArtworkTypeID = '{this._masterData["ArtworkTypeID"]}'
) PoQty
outer apply(
	select value = ISNULL(sum(ReqQty),0)
	from ArtworkReq_Detail ad , ArtworkReq a
	where ad.ID = a.ID
	and OrderID = o.ID and ad.PatternCode= isnull(s.PatternCode,'')
	and ad.PatternDesc = isnull(s.PatternDesc,'') 
    and ad.ArtworkID = iif(s.ArtworkID is null,'{this._masterData["ArtworkTypeID"]}',s.ArtworkID)
	and ad.id !=  '{this._ArtWorkReq_ID}'
	and a.ArtworkTypeID = '{this._masterData["ArtworkTypeID"]}'
    and a.status != 'Closed'
)ReqQty
group by o.FactoryID,o.ID,o.StyleID,o.BrandID,ReqQty.value,PoQty.value,s.ReqQty
,s.ArtworkID,s.PatternCode,s.PatternDesc,isnull(tbbd.BuyBackArtworkReq,0)
having ReqQty.value + PoQty.value + s.ReqQty + isnull(tbbd.BuyBackArtworkReq,0) > sum(oq.Qty) 

select  FactoryID,
        ArtworkTypeID,
        OrderID,
        StyleID,
        BrandID,
        StandardQty,
        [ReqQty] = Max(ReqQty)
into #tmpCurrentFinal
from #tmpCurrent
group by    FactoryID,
            ArtworkTypeID,
            OrderID,
            StyleID,
            BrandID,
            StandardQty


MERGE #tmpDB AS T
USING #tmpCurrentFinal AS S
ON (T.ArtworkTypeID = S.ArtworkTypeID and T.OrderID = S.OrderID) 
WHEN NOT MATCHED BY TARGET 
    THEN INSERT(FactoryID, ArtworkTypeID, OrderID, StyleID, BrandID, StandardQty, ReqQty, SubconReasonID, ReasonDesc, CreateBy, CreateDate, EditBy, EditDate, ArtworkID, PatternCode, PatternDesc, NeedUpdate, NeedDelete) 
            VALUES(S.FactoryID, S.ArtworkTypeID, S.OrderID, S.StyleID, S.BrandID, S.StandardQty, S.ReqQty, '', '', '', getdate(), '', null, '', '', '', 0, 0)
WHEN MATCHED 
    THEN UPDATE SET T.StandardQty = S.StandardQty,
                    T.ReqQty = S.ReqQty,
                    T.NeedUpdate = 1
WHEN NOT MATCHED BY SOURCE
    THEN UPDATE SET T.NeedDelete = 1
	;

select * from #tmpDB 

drop table #tmpCurrent,#tmpDB,#tmpCurrentFinal

";

            result = MyUtility.Tool.ProcessWithDatatable(this._detailDatas, string.Empty, sqlcmd, out this.dtLoad);
            if (result == false)
            {
                this.ShowErr(sqlcmd, result);
            }

            return this.dtLoad;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (this.btnClose.Text == "Close")
            {
                this.Close();
            }
            else
            {
                this.EditMode = !this.EditMode;
                this.btnEdit.Text = "Edit";
                this.btnClose.Text = "Close";
                if (this.EditMode)
                {
                    this.txt_SubReason.IsEditable = true;
                }
                else
                {
                    this.txt_SubReason.IsEditable = false;
                }

                // 回到檢視模式，並且重新取得資料
                this.Query();
            }
        }
    }
}
