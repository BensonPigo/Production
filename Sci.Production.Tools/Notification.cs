using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using Ict;
using Ict.Win;

using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Class.Commons;

namespace Sci.Production.Tools
{
    /// <inheritdoc/>
    public partial class Notification : Sci.Win.Subs.Input4
    {
        /// <summary>
        /// 共用方式
        /// </summary>
        public enum ModuleEnum
        {
            /// <summary>
            /// Tools.P02
            /// </summary>
            Position,

            /// <summary>
            /// Tools.P01
            /// </summary>
            User,
        }

        private ModuleEnum Usage;
        private DataRow MasterData;

        /// <inheritdoc/>
        public Notification(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, string workalais, string keyfield, DataRow masterData, ModuleEnum usage)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.Usage = usage;
            this.KeyField1 = keyfield;
            this.WorkAlias = workalais;
            this.MasterData = masterData;

            if (usage == ModuleEnum.Position)
            {
                this.Text = this.Text + " - Position";
            }
            else
            {
                this.Text = this.Text + " - User";
            }

            this.grid.RowsAdded += this.DetailGrid_RowsAdded;
        }

        /// <inheritdoc/>
        protected override DualResult OnRequery(out DataTable datas)
        {
            string sqlCmd = string.Empty;
            if (this.Usage == ModuleEnum.Position)
            {
                sqlCmd = $@"Select sel = iif(isnull(pn.Pass0_Ukey, '') = '', 0, 1)
, n.MenuName
, Name
, SystemNotify = cast(isnull(pn.SystemNotify, 1) as bit)
, pn.AddName
, pn.AddDate
, pn.EditName
, pn.EditDate
, pn.Pass0_Ukey
, NotificationList_Ukey = n.Ukey
, isPosition = cast(0 as bit)
From NotificationList n with(nolock)
Left join Menu with(nolock) on n.MenuName = Menu.MenuName
outer apply(Select * From Pass0_Notify pn with(nolock) Where pn.NotificationList_Ukey = n.ukey and pn.Pass0_Ukey = '{this.KeyValue1}') pn
Where n.junk = 0
Order by Menu.MenuName
";
            }

            if (this.Usage == ModuleEnum.User)
            {
                sqlCmd = $@"Select sel = iif(isnull(pn.Pass0_Ukey, '') = '', 0, 1)
, n.MenuName
, Name
, SystemNotify = cast(isnull(pn.SystemNotify, 1) as bit)
, AddName = ''
, AddDate = null
, EditName = ''
, EditDate = null
, pn.Pass0_Ukey
, NotificationList_Ukey = n.Ukey
, Pass1_ID = '{this.KeyValue1}'
, isPosition = cast(0 as bit)
From NotificationList n with(nolock)
Left join Menu with(nolock) on n.MenuName = Menu.MenuName
outer apply(Select * From Pass0_Notify pn with(nolock) Where pn.NotificationList_Ukey = n.ukey and pn.Pass0_Ukey = '{this.MasterData["pkey"]}') pn
Where n.junk = 0
Order by Menu.MenuName";
            }

            DualResult result;
            if (!(result = DBProxy.Current.Select(null, sqlCmd, out datas)))
            {
                return result;
            }

            return Result.True;
        }

        /// <inheritdoc/>
        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);
        }

        private void DetailGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            Enumerable.Range(e.RowIndex, e.RowCount)
                .ToList()
                .ForEach(idx =>
                {
                    this.SetSelectColumnReadOnly(idx);
                });
        }

        /// <inheritdoc />
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            var sqlCmd = $@"
select MenuName From
(
	Select MenuName = '', MenuNo = 0 from Menu with(nolock)
	union
	Select MenuName, MenuNo From Menu with(nolock) Where IsSubMenu = 0
) as a";

            DataTable dtddl;
            if (Class.Command.SQL.Select(Class.Command.SQL.queryConn, sqlCmd, out dtddl))
            {
                if (dtddl != null && dtddl.Columns.Contains("MenuName"))
                {
                    this.cmbMenu.DataSource = dtddl;
                    this.cmbMenu.ValueMember = "MenuName";
                    this.cmbMenu.DisplayMember = "MenuName";
                }

                this.cmbMenu.SelectedIndex = -1;
            }
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            // 取消除check box排序,避免底層報錯
            DataGridViewGeneratorCheckBoxColumnSettings setCheckCol = new DataGridViewGeneratorCheckBoxColumnSettings();
            setCheckCol.HeaderAction = DataGridViewGeneratorCheckBoxHeaderAction.None;

            this.Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(3), trueValue: true, falseValue: false)
                .Text("MenuName", header: "MenuName", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("Name", header: "Name", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .CheckBox("SystemNotify", header: "System Notify", width: Widths.AnsiChars(5), settings: setCheckCol);

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnSave()
        {
            DataTable dt = (DataTable)this.gridbs.DataSource;

            string sql = $@"
-- 1 => 1 修改
Update target set target.SystemNotify = source.SystemNotify, target.EditName = '{Env.User.UserID}', target.EditDate = GetDate()
from {this.WorkAlias} target with(nolock)
left join #tmp Source on Source.{this.KeyField1} = target.{this.KeyField1} and Source.NotificationList_Ukey = target.NotificationList_Ukey
Where Source.sel = 1 
and target.SystemNotify <> Source.SystemNotify

--1 => 0 刪除
delete target from {this.WorkAlias} target with(nolock)
full join #tmp Source on Source.{this.KeyField1} = target.{this.KeyField1} and Source.NotificationList_Ukey = target.NotificationList_Ukey
Where Source.sel = 0

--0 => 1 新增
Insert into {this.WorkAlias}({this.KeyField1},NotificationList_Ukey,SystemNotify,AddName,AddDate)
select '{this.KeyValue1}',Source.NotificationList_Ukey,Source.SystemNotify,'{Env.User.UserID}',GetDate()
from {this.WorkAlias} target with(nolock)
full join #tmp Source on Source.{this.KeyField1} = target.{this.KeyField1} and Source.NotificationList_Ukey = target.NotificationList_Ukey
Where isnull(target.{this.KeyField1}, '') = '' and Source.sel = 1
";

            DataTable newDatas;
            var result = MyUtility.Tool.ProcessWithDatatable(dt, null, sql, out newDatas);
            if (!result)
            {
                this.ShowErr(result);
                return result;
            }

            // 將設定檔Table重新撈資料
            NotificationPrg.SetData sd = new NotificationPrg.SetData();
            sd.SettingData = NotificationPrg.GetNotifyDataTable();

            MyUtility.Msg.InfoBox("Please re-login for notification setting update.");

            return result;
        }

        private void CmbModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.gridbs.DataSource == null)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.cmbMenu.SelectedValue2))
            {
                this.gridbs.Filter = string.Empty;
            }
            else
            {
                this.gridbs.Filter = $"MenuName = '{this.cmbMenu.SelectedValue2}'";
            }
        }

        private void SetSelectColumnReadOnly(int index)
        {
            var row = ((DataTable)this.gridbs.DataSource).Rows[index];
            if (Convert.ToBoolean(row["isPosition"]))
            {
                this.grid.Rows[index].Cells["Sel"].ReadOnly = true;
            }
            else
            {
                this.grid.Rows[index].Cells["Sel"].ReadOnly = false;
            }
        }
    }
}
