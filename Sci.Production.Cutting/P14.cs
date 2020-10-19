using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P14 : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        private DataTable dt = new DataTable();

        private void P14_FormLoaded(object sender, EventArgs e)
        {
            this.grid1.AutoGenerateColumns = true;
            this.dt.Columns.Add("msg", typeof(string));
            this.listControlBindingSource1.DataSource = this.dt;
        }

        private void TxtCardNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!Regex.IsMatch(e.KeyChar.ToString(), "[0-9]")) && ((int)e.KeyChar) != 8)
            {
                e.Handled = true;
                return;
            }
        }

        private void TxtBundleNo_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtBundleNo.Text))
            {
                return;
            }

            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@BundleNo", this.txtBundleNo.Text),
            };
            string sqlchk = $@"select 1 from Bundle_Detail with(nolock) where BundleNo = @BundleNo";
            if (!MyUtility.Check.Seek(sqlchk, sqlParameters))
            {
                MyUtility.Msg.WarningBox("Data not found.");
                this.txtBundleNo.Text = string.Empty;
                return;
            }

            #region Datas
            string sqlcmd = $@"
select  Orderid=dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = bd.BundleNo order by OrderID for XML RAW))
        , bd.Patterncode
        , b.Cutno
        , b.Colorid
        , bd.SizeCode
        , bd.Qty
        , b.Article
from Bundle_Detail bd with(nolock)
inner join Bundle b with(nolock) on b.id = bd.id
inner join orders o with(nolock) on o.id = b.Orderid and o.MDivisionID  = b.MDivisionID 
where BundleNo = @BundleNo";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, sqlParameters, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found.");
                this.txtBundleNo.Text = string.Empty;
                return;
            }

            DataRow row = dt.Rows[0];
            this.disSP.Text = MyUtility.Convert.GetString(row["Orderid"]);
            this.disCutpart.Text = MyUtility.Convert.GetString(row["Patterncode"]);
            this.disCutNo.Text = MyUtility.Convert.GetString(row["Cutno"]);
            this.disArticle.Text = MyUtility.Convert.GetString(row["Article"]);
            this.disColor.Text = MyUtility.Convert.GetString(row["Colorid"]);
            this.disSize.Text = MyUtility.Convert.GetString(row["SizeCode"]);
            this.disBundleQty.Text = MyUtility.Convert.GetString(row["Qty"]);
            #endregion

            #region cmdComboType
            string sqlCombotype = $@"
select sl.Location
into #tmp
from Bundle_Detail bd with(nolock)
inner join Bundle b with(nolock) on b.id = bd.id
inner join Orders o with(nolock) on o.id = b.Orderid and o.MDivisionID  = b.MDivisionID 
inner join Style_Location sl with(nolock) on o.StyleUkey = sl.StyleUkey
where BundleNo = @BundleNo
If (select count(1) from #tmp)>1
begin
    select Location = '' union all select Location from #tmp
end
else
begin
    select Location from #tmp
end
drop table #tmp
";
            result = DBProxy.Current.Select(null, sqlCombotype, sqlParameters, out DataTable combotypeDt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Tool.SetupCombox(this.cmdComboType, 1, combotypeDt);
            if (combotypeDt.Rows.Count == 1 && MyUtility.Check.Empty(this.txtBundleNo.Text))
            {
                this.cmdComboType.DataSource = null;
            }
            #endregion
        }

        private void TxtCardNoBundleNoComboType_Validated(object sender, EventArgs e)
        {
            if (!this.Checkempty())
            {
                return;
            }

            this.InsertDatas();
            this.Clearall();
        }

        private void CmdComboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.Checkempty())
            {
                return;
            }

            this.InsertDatas();
            this.Clearall();
        }

        private bool IsSuccessful = true;

        private void InsertDatas()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@SP", this.disSP.Text),
                new SqlParameter("@ComboType", this.cmdComboType.Text),
                new SqlParameter("@CutNo", this.disCutNo.Text),
                new SqlParameter("@CardNo", this.txtCardNo.Text),
                new SqlParameter("@Article", this.disArticle.Text),
                new SqlParameter("@SizeName", this.disSize.Text),
                new SqlParameter("@Qty", this.disBundleQty.Text),
            };

            string sqlupdatacmd = $@"
update t set
	t.[BillDate]    = getdate(),
	t.[MONO]		= @SP+'-'+@ComboType ,
	t.[PONO]		= @SP ,
	t.[CutLotNo]	= @CutNo ,
	t.[BundNo]		= @CardNo,
	t.[CardNo]		= @CardNo,
	t.[ColorName]   = @Article ,
	t.[CordColor]	= @Article ,
	t.[SizeName]	= @SizeName ,
	t.[Qty]			= @Qty ,
	t.[BarCode]	    = @CardNo,
    t.[CmdType]     ='Update',
	t.[CmdTime]	    = getdate(),
	t.[InterfaceTime]   =null
from [dbo].[tCutbundcard] t
where t.CardNo =@CardNo
and(
	t.[MONO]		<>@SP+'-'+@ComboType or
	t.[PONO]		<>@SP or
	t.[CutLotNo]	<>@CutNo or
	t.[ColorName]   <>@Article or
	t.[CordColor]	<>@Article or
	t.[SizeName]	<>@SizeName or
	t.[Qty]			<>@Qty
)

If not exists (select 1 from [dbo].[tCutbundcard] where CardNo = @CardNo)
Begin
	insert into [dbo].[tCutbundcard]
	(BillDate       , MONO				    , PONO       , CutLotNo     , BundNo
     , CardNo       , CardType              , ColorName  , CordColor    , SizeName
     , QTY          , Vatno                 , BarCode    , CmdType      , CmdTime
     , InterfaceTime)
	values
	(getdate()      , @SP+'-'+@ComboType    , @SP        , @CutNo       , @CardNo
     , @CardNo      , 2                     , @Article   , @Article     , @SizeName
     , @Qty         , null                  , @CardNo    , 'Insert'     , getdate()
     ,null)
End
";
            using (TransactionScope scope = new TransactionScope())
            {
                DualResult upResult;
                if (!MyUtility.Check.Empty(sqlupdatacmd))
                {
                    if (!(upResult = DBProxy.Current.Execute("SUNRISEEXCH", sqlupdatacmd, sqlParameters)))
                    {
                        scope.Dispose();
                        this.ShowErr(upResult);
                        this.IsSuccessful = false;
                        return;
                    }
                }

                scope.Complete();
            }

            this.IsSuccessful = true;
        }

        private bool Checkempty()
        {
            if (MyUtility.Check.Empty(this.txtCardNo.Text) || MyUtility.Check.Empty(this.txtBundleNo.Text) || MyUtility.Check.Empty(this.cmdComboType.Text))
            {
                return false;
            }

            return true;
        }

        private void Addmsg(string msg)
        {
            DataRow dr = this.dt.NewRow();
            dr[0] = msg;
            this.dt.Rows.Add(dr);
            this.listControlBindingSource1.Position = this.dt.Rows.Count - 1;
            this.grid1.AutoResizeColumns();
        }

        private void Grid1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            this.grid1.Rows[e.RowIndex].Cells[0].Style.ForeColor = this.IsSuccessful ? Color.Black : Color.Red;
        }

        private void Clearall()
        {
            if (this.IsSuccessful)
            {
                this.Addmsg($"( {DateTime.Now.ToString("HH:mm:ss")} ) Successful Card# : {this.txtCardNo.Text}, Bundle# : {this.txtBundleNo.Text}, Combo Type : {this.cmdComboType.Text}");
            }
            else
            {
                this.Addmsg($"( {DateTime.Now.ToString("HH:mm:ss")} ) Not Successful Card# : {this.txtCardNo.Text}, Bundle# : {this.txtBundleNo.Text}, Combo Type : {this.cmdComboType.Text}");
            }

            this.txtCardNo.Text = string.Empty;
            this.txtBundleNo.Text = string.Empty;
            this.cmdComboType.DataSource = null;
            this.disSP.Text = string.Empty;
            this.disCutpart.Text = string.Empty;
            this.disCutNo.Text = string.Empty;
            this.disArticle.Text = string.Empty;
            this.disColor.Text = string.Empty;
            this.disSize.Text = string.Empty;
            this.disBundleQty.Text = string.Empty;
            this.txtBundleNo.Focus();
        }
    }
}
