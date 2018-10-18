using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class P14 : Sci.Win.Tems.QueryForm
    {
        public P14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.EditMode = true;
        }

        private void txtCardNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            int ikc = e.KeyChar;
            if ((!Regex.IsMatch(e.KeyChar.ToString(), "[0-9]")) && ((int)e.KeyChar) != 8)
            {
                e.Handled = true;
                return;
            }
        }

        private bool CaneditSql = true;
        private void txtBundleNo_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtBundleNo.Text)) return;
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@BundleNo", this.txtBundleNo.Text));
            string sqlchk = $@"select 1 from Bundle_Detail with(nolock) where BundleNo = @BundleNo";
            if (!MyUtility.Check.Seek(sqlchk, sqlParameters))
            {
                MyUtility.Msg.WarningBox("Data not found.");
                this.txtBundleNo.Text = string.Empty;
                return;
            }

            #region Datas
            string sqlcmd = $@"
select b.Orderid,bd.Patterncode,b.Cutno,c.Name,b.Colorid,bd.SizeCode,bd.Qty
from Bundle_Detail bd with(nolock)
inner join Bundle b with(nolock) on b.id = bd.id
inner join orders o with(nolock) on o.id = b.Orderid
left join color c with(nolock) on c.id = b.Colorid and c.BrandId = o.BrandID
where BundleNo = @BundleNo";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, sqlParameters, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            DataRow row = dt.Rows[0];
            this.disSP.Text = MyUtility.Convert.GetString(row["Orderid"]);
            this.disCutpart.Text = MyUtility.Convert.GetString(row["Patterncode"]);
            this.disCutNo.Text = MyUtility.Convert.GetString(row["Cutno"]);
            this.disColorName.Text = MyUtility.Convert.GetString(row["Name"]);
            this.disColor.Text = MyUtility.Convert.GetString(row["Colorid"]);
            this.disSize.Text = MyUtility.Convert.GetString(row["SizeCode"]);
            this.disBundleQty.Text = MyUtility.Convert.GetString(row["Qty"]);
            #endregion

            #region cmdComboType
            string sqlCombotype = $@"
select Location
into #tmp
from Bundle_Detail bd with(nolock)
inner join Bundle b with(nolock) on b.id = bd.id
inner join Orders o with(nolock) on o.id = b.Orderid
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
            DataTable combotypeDt;
            result = DBProxy.Current.Select(null, sqlCombotype, sqlParameters, out combotypeDt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.cmdComboType.DataSource = null;
            MyUtility.Tool.SetupCombox(this.cmdComboType, 1, combotypeDt);
            #endregion
        }

        private void txtCardNoBundleNoComboType_Validated(object sender, EventArgs e)
        {
            if (!checkempty() || !CaneditSql) return;
            InsertDatas();
            clearall();
        }

        private void cmdComboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!checkempty() || !CaneditSql) return;
            InsertDatas();
            clearall();
        }

        private void InsertDatas()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@SP", this.disSP.Text));            
            sqlParameters.Add(new SqlParameter("@ComboType", this.cmdComboType.Text));
            sqlParameters.Add(new SqlParameter("@CutNo", this.disCutNo.Text));
            sqlParameters.Add(new SqlParameter("@CardNo", this.txtCardNo.Text));
            sqlParameters.Add(new SqlParameter("@ColorName", this.disColorName.Text));
            sqlParameters.Add(new SqlParameter("@CordColor", this.disColor.Text));
            sqlParameters.Add(new SqlParameter("@SizeName", this.disSize.Text));
            sqlParameters.Add(new SqlParameter("@Qty", this.disBundleQty.Text));

            string sqlupdatacmd = $@"
update t set
	t.[BillDate]    = getdate(),
	t.[MONO]		= @SP+'-'+@ComboType ,
	t.[PONO]		= @SP ,
	t.[CutLotNo]	= @CutNo ,
	t.[BundNo]		= @CardNo,
	t.[CardNo]		= @CardNo,
	t.[ColorName]= @ColorName ,
	t.[CordColor]	= @CordColor ,
	t.[SizeName]	= @SizeName ,
	t.[Qty]			= @Qty ,
	t.[BarCode]	= @CardNo,
	t.[CmdTime]	= getdate(),
	t.[InterfaceTime]=null
from SciSUNRISEEXCH t
where t.CardNo =@CardNo
and(
	t.[MONO]		<>@SP+'-'+@ComboType or
	t.[PONO]		<>@SP or
	t.[CutLotNo]	<>@CutNo or
	t.[ColorName]<>@ColorName or
	t.[CordColor]	<>@CordColor or
	t.[SizeName]	<>@SizeName or
	t.[Qty]			<>@Qty
)

If not exists (select 1 from SciSUNRISEEXCH where CardNo = @CardNo)
Begin
	insert into SciSUNRISEEXCH
	(BillDate ,MONO				 ,PONO,CutLotNo,BundNo, CardNo, CardType,ColorName, CordColor, SizeName, QTY,Vatno,
	BarCode,CmdType,CmdTime, InterfaceTime)
	values
	(getdate(),@SP+'-'+@ComboType,@SP, @CutNo,  @CardNo,@CardNo,2,       @ColorName,@CordColor,@SizeName,@Qty,null,
	@CardNo,@CardNo,getdate(),null)
End
";
            using (TransactionScope scope = new TransactionScope())
            {
                DualResult upResult;
                if (!MyUtility.Check.Empty(sqlupdatacmd))
                {
                    if (!(upResult = DBProxy.Current.Execute(null, sqlupdatacmd, sqlParameters)))
                    {
                        this.ShowErr(upResult);
                        return;
                    }
                }
                scope.Complete();
            }
            CaneditSql = false;
        }

        private bool checkempty()
        {
            if (MyUtility.Check.Empty(this.txtCardNo.Text) || MyUtility.Check.Empty(this.txtBundleNo.Text) || MyUtility.Check.Empty(this.cmdComboType.Text))
            {
                return false;
            }
            return true;
        }

        private void clearall()
        {
            this.txtCardNo.Text = string.Empty;
            this.txtBundleNo.Text = string.Empty;
            this.cmdComboType.DataSource = null;
            this.disSP.Text = string.Empty;
            this.disCutpart.Text = string.Empty;
            this.disCutNo.Text = string.Empty;
            this.disColorName.Text = string.Empty;
            this.disColor.Text = string.Empty;
            this.disSize.Text = string.Empty;
            this.disBundleQty.Text = string.Empty;
            CaneditSql = true;
            this.txtCardNo.Focus();
        }
    }
}
