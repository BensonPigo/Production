using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// Basic_B14
    /// </summary>
    public partial class Basic_B14 : Win.Tems.Input1
    {
        /// <summary>
        /// Basic_B14
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public Basic_B14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            DualResult result;
            DataTable dt = new DataTable();
            string sqlCmd;

            #region Machine Button Color
            sqlCmd = string.Format(
    @"
IF (SELECT COUNT( m.ArtworkTypeID)FROm MachineType m 
	INNER JOIN ArtworkType a On m.ArtworkTypeID=a.ID
	WHERE A.Seq LIKE '1%' AND m.ArtworkTypeID='{0}' ) 
	> 0
BEGIN
    select (
	    select concat( ',',cast(rtrim(m.ID) as nvarchar))
	    FROM MachineType m
	    INNER JOIN ArtworkType a ON m.ArtworkTypeID=a.ID
	    where  A.Seq LIKE '1%'AND m.ArtworkTypeID = '{0}' 
	    for XML Path('')
    ) as MatchTypeID
    INTO #tmp

    SELECt [MatchTypeID]=STUFF( MatchTypeID,1,1,'')
    FROM #tmp

    DROP TABLE #tmp
END
ELSE
BEGIN
    select (
	    select cast(rtrim(ID) as nvarchar) +',' 
	    from MachineType MT WITH (NOLOCK) LEFT JOIN Artworktype_Detail ATD WITH (NOLOCK) ON MT.ID=ATD.MachineTypeID
	    where ATD.ArtworkTypeID = '{0}' for XML Path('')
    ) as MatchTypeID
END

",
    this.CurrentMaintain["ID"]);

            result = DBProxy.Current.Select(this.ConnectionName, sqlCmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.editMachineID.Text = dt.Rows[0]["MatchTypeID"].ToString();
            #endregion

            #region Machine ID

            // 根據Code 第一碼決定顯示內容
            sqlCmd = string.Format(
                                    @"
IF (SELECT COUNT( m.ArtworkTypeID)FROm MachineType m 
	INNER JOIN ArtworkType a On m.ArtworkTypeID=a.ID
	WHERE A.Seq LIKE '1%' AND m.ArtworkTypeID='{0}' ) 
	> 0
BEGIN
        select m.ID,Description
	    FROM MachineType m
	    INNER JOIN ArtworkType a ON m.ArtworkTypeID=a.ID
	    where  A.Seq LIKE '1%'AND m.ArtworkTypeID = '{0}' 
END
ELSE
BEGIN
    select ID,Description 
    from MachineType MT WITH (NOLOCK) LEFT JOIN Artworktype_Detail ATD WITH (NOLOCK) ON MT.ID=ATD.MachineTypeID
    where ATD.ArtworkTypeID = '{0}'
END

",
                                    this.CurrentMaintain["ID"].ToString());

            result = DBProxy.Current.Select(this.ConnectionName, sqlCmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            if (dt.Rows.Count > 0)
            {
                this.btnMachine.ForeColor = Color.Blue;
            }
            else
            {
                this.btnMachine.ForeColor = Color.Black;
            }
            #endregion

            #region checkbox color
            sqlCmd = string.Format("select * from [ProductionTPE].[dbo].[ArtworkType_FTY] where ArtworkTypeID = '{0}'", this.CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Select(this.ConnectionName, sqlCmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.chkIsShowinIEP01.Checked = dt.AsEnumerable().Where(x => x.Field<bool>("IsShowinIEP01").Equals(true)).Any();
            var showinIEP03 = dt.AsEnumerable().Where(x => x.Field<bool>("IsShowinIEP03").Equals(true));
            this.chkIsShowinIEP03.Checked = showinIEP03.Any();
            var sewingline = dt.AsEnumerable().Where(x => x.Field<bool>("IsSewingline").Equals(true));
            this.chkIsSewingline.Checked = sewingline.Any();

            this.txtCentralizedmulitFactoryIEP03.Text = string.Empty;
            if (showinIEP03.Any())
            {
                this.txtCentralizedmulitFactoryIEP03.Text = string.Join(",", showinIEP03.Select(x => x.Field<string>("FactoryID")).ToArray());
            }

            this.txtCentralizedmulitFactorySewingline.Text = string.Empty;
            if (sewingline.Any())
            {
                this.txtCentralizedmulitFactorySewingline.Text = string.Join(",", sewingline.Select(x => x.Field<string>("FactoryID")).ToArray());
            }
            #endregion
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            DualResult result = new DualResult(false);
            string sqlCmd = string.Format(
                @"
create table #tmp_Factory
(
	id_num int IDENTITY(1,1), 
	factoryID varchar(5),
)

declare @_name as varchar(20)
declare @sql as nvarchar(max)
DECLARE CURSOR_ CURSOR FOR
select name
from sys.servers
where name like 'PMSDB\%'
and name not in ('PMSDB\POWERBI', 'PMSDB\TRAINING')

OPEN CURSOR_
FETCH NEXT FROM CURSOR_ INTO @_name
While @@FETCH_STATUS = 0
Begin	
	set @sql = 'select distinct FTYGroup from ['+@_name+'].[Production].[dbo].[Factory] f where IsProduceFty = 1 and junk = 0' 

	insert into #tmp_Factory
	EXECUTE sp_executesql @sql
FETCH NEXT FROM CURSOR_ INTO @_name
End
CLOSE CURSOR_
DEALLOCATE CURSOR_ 

update a
	set a.IsShowinIEP01 = '{1}'
from [ProductionTPE].[dbo].ArtworkType_FTY a
inner join #tmp_Factory b on a.FactoryID = b.factoryID
where a.ArtworkTypeID = '{0}'

insert into [ProductionTPE].[dbo].ArtworkType_FTY ([ArtworkTypeID], [FactoryID], [IsShowinIEP01])
select [ArtworkTypeID] = '{0}' 
	, factoryID
	, [IsShowinIEP01] = '{1}'
from #tmp_Factory t
where not exists (select 1 from [ProductionTPE].[dbo].ArtworkType_FTY where ArtworkTypeID = '{0}' and FactoryID = t.factoryID)


drop table #tmp_Factory;
",
                this.CurrentMaintain["ID"],
                this.chkIsShowinIEP01.Checked);

            if (this.chkIsShowinIEP03.Checked && this.txtCentralizedmulitFactoryIEP03.Text.Length > 0)
            {
                sqlCmd += string.Format(
                    @"
update a
	set IsShowinIEP03 = 1
from [ProductionTPE].[dbo].ArtworkType_FTY a
where a.ArtworkTypeID = '{0}'
and a.FactoryID in ('{1}');
",
                    this.CurrentMaintain["ID"],
                    this.txtCentralizedmulitFactoryIEP03.Text.Replace(",", "','"));

                foreach (string factroy in this.txtCentralizedmulitFactoryIEP03.Text.ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    sqlCmd += string.Format(
                        @"
if not exists (select 1 from [ProductionTPE].[dbo].ArtworkType_FTY a where a.ArtworkTypeID = '{0}' and a.FactoryID = '{1}')
begin
	insert into [ProductionTPE].[dbo].ArtworkType_FTY([ArtworkTypeID], [FactoryID], [IsShowinIEP03])
	values ('{0}', '{1}', 1)
end; 
",
                        this.CurrentMaintain["ID"],
                        factroy);
                }
            }
            else
            {
                // 沒勾選: 要把對應的[Factory]清空
                sqlCmd += string.Format(
                    @"
update a
	set IsShowinIEP03 = 0
from [ProductionTPE].[dbo].ArtworkType_FTY a
where a.ArtworkTypeID = '{0}';
",
                    this.CurrentMaintain["ID"]);
            }

            if (this.chkIsSewingline.Checked && this.txtCentralizedmulitFactorySewingline.Text.Length > 0)
            {
                sqlCmd += string.Format(
                    @"
update a
	set IsSewingline = 1
from [ProductionTPE].[dbo].ArtworkType_FTY a
where a.ArtworkTypeID = '{0}'
and a.FactoryID in ('{1}');
",
                    this.CurrentMaintain["ID"],
                    this.txtCentralizedmulitFactorySewingline.Text.Replace(",", "','"));

                foreach (string factroy in this.txtCentralizedmulitFactorySewingline.Text.ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    sqlCmd += string.Format(
                        @"
if not exists (select 1 from [ProductionTPE].[dbo].ArtworkType_FTY a where a.ArtworkTypeID = '{0}' and a.FactoryID = '{1}')
begin
	insert into [ProductionTPE].[dbo].ArtworkType_FTY([ArtworkTypeID], [FactoryID], [IsSewingline])
	values ('{0}', '{1}', 1)
end; 
",
                        this.CurrentMaintain["ID"],
                        factroy);
                }
            }
            else
            {
                // 沒勾選: 要把對應的[Factory]清空
                sqlCmd += string.Format(
                    @"
update a
	set IsSewingline = 0
from [ProductionTPE].[dbo].ArtworkType_FTY a
where a.ArtworkTypeID = '{0}';
",
                    this.CurrentMaintain["ID"]);
            }

            result = DBProxy.Current.Execute(this.ConnectionName, sqlCmd);
            return result;
        }

        private void BtnMachine_Click(object sender, EventArgs e)
        {
            Basic_B14_Machine callNextForm = new Basic_B14_Machine(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }
    }
}
