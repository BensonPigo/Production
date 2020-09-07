using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P27 : Win.Tems.Input6
    {
        public P27(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            #region Stock Type 下拉選單 ‘B’,’Bulk’,’O’,’Scrap’
            Dictionary<string, string> di_Stocktype = new Dictionary<string, string>();
            di_Stocktype.Add("B", "Bulk");
            di_Stocktype.Add("O", "Scrap");
            this.comboStockType.DataSource = new BindingSource(di_Stocktype, null);
            this.comboStockType.ValueMember = "Key";
            this.comboStockType.DisplayMember = "Value";
            #endregion

            #region 隱藏 3個小按鈕-新增/插入
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Visible = false;
            #endregion
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
            this.CurrentMaintain["Status"] = "New";
            this.comboStockType.SelectedIndex = 0;
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            // !EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.");
                return false;
            }

            return base.ClickEditBefore();
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            #region 判斷表頭Stock Type/Issue Date不可為空
            if (MyUtility.Check.Empty(this.CurrentMaintain["stocktype"]))
            {
                MyUtility.Msg.WarningBox("Stock Type can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["issuedate"]))
            {
                MyUtility.Msg.WarningBox("Issue Date can't empty!");
                return false;
            }
            #endregion
            #region 刪除明細ToLocation為空的資料
            for (int i = ((DataTable)this.detailgridbs.DataSource).Rows.Count - 1; i >= 0; i--)
            {
                if (((DataTable)this.detailgridbs.DataSource).Rows[i]["ToLocation"].Empty())
                {
                    ((DataTable)this.detailgridbs.DataSource).Rows[i].Delete();
                }
            }
            #endregion
            #region 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "LL", "LocationTransLocal ", (DateTime)this.CurrentMaintain["Issuedate"], 2, "ID", null);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;
            }
            #endregion
            return base.ClickSaveBefore();
        }

        // Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            StringBuilder sqlup = new StringBuilder();
            string location = string.Empty;
            if (this.CurrentMaintain["StockType"].EqualString("B"))
            {
                location = "ALocation";
            }

            if (this.CurrentMaintain["StockType"].EqualString("O"))
            {
                location = "CLocation";
            }

            sqlup.Append(string.Format(
                @"
update LI set LI.{1} = LD.ToLocation 
from LocationTransLocal_Detail LD 
left join LocalInventory LI on LI.OrderID = LD.PoId and LI.Refno = LD.Refno and LI.ThreadColorID = LD.Color
where LD.Id = '{0}';", this.CurrentMaintain["id"], location));

            sqlup.Append(string.Format("update LocationTransLocal set status='Confirmed' where id = '{0}'", this.CurrentMaintain["id"]));

            DualResult dr = DBProxy.Current.Execute(null, sqlup.ToString());
            if (!dr)
            {
                MyUtility.Msg.ErrorBox("Update sql command error!");
                return;
            }
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        // refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.label25.Text = this.CurrentMaintain["status"].ToString();
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region Location 右鍵開窗

            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem2 item = Prgs.SelectLocation(this.CurrentMaintain["stocktype"].ToString(), this.CurrentDetailData["tolocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["tolocation"] = item.GetSelectedString();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    this.CurrentDetailData["tolocation"] = e.FormattedValue;
                    string sqlcmd = string.Format(
                        @"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{0}'
        and junk != '1'", this.CurrentMaintain["stocktype"].ToString());
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = this.CurrentDetailData["tolocation"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !location.EqualString(string.Empty))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!location.EqualString(string.Empty))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", errLocation.ToArray()) + "  Data not found !!", "Data not found");
                    }

                    trueLocation.Sort();
                    this.CurrentDetailData["tolocation"] = string.Join(",", trueLocation.ToArray());

                    // 去除錯誤的Location將正確的Location填回
                }
            };
            #endregion Location 右鍵開窗

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
            .Text("Refno", header: "Refno", width: Widths.AnsiChars(10), iseditingreadonly: true) // 1
            .Text("Color", header: "Color", width: Widths.AnsiChars(10), iseditingreadonly: true) // 2
            .EditText("Description", header: "Description", width: Widths.AnsiChars(15), iseditingreadonly: true) // 3
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 4
            .Text("FromLocation", header: "FromLocation", iseditingreadonly: true) // 5
            .Text("ToLocation", header: "ToLocation", iseditingreadonly: false, width: Widths.AnsiChars(14), settings: ts2) // 6
            ;
            #endregion 欄位設定
            this.detailgrid.Columns["ToLocation"].DefaultCellStyle.BackColor = Color.Pink;
        }

        // 寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(
                @"select LD.id,LD.PoId,LD.Refno,LD.Color,LD.Qty,LD.FromLocation,LD.ToLocation,L.Description
from LocationTransLocal_detail LD WITH (NOLOCK) 
left join LocalItem L WITH (NOLOCK) on L.RefNo = LD.Refno
Where LD.id = '{0}' ", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            var frm = new P27_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
        }

        // 當表身已經有值時，編輯時若換了stock type則表身要一併清空。
        private void ComboStockType_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.comboStockType.SelectedValue) && this.comboStockType.SelectedValue != this.comboStockType.OldValue)
            {
                if (this.detailgridbs.DataSource == null)
                {
                    return;
                }

                if (((DataTable)this.detailgridbs.DataSource).Rows.Count > 0)
                {
                    for (int i = 0; i < ((DataTable)this.detailgridbs.DataSource).Rows.Count; i++)
                    {
                        ((DataTable)this.detailgridbs.DataSource).Rows[i].Delete();
                    }
                }
            }
        }
    }
}