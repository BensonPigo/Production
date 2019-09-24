using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tems;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class P40 : Sci.Win.Tems.Input6
    {
        protected DataRow lastADIDASComplain;
        protected DataTable lastADIDASComplain_Detail;
        protected bool isShowHistory = false;
        protected Color yellow = Color.FromArgb(255, 198, 10);
        protected Color displayDefaultBack = Color.FromArgb(183, 227, 255);

        private string[] compareColumns = { "SalesID", "SalesName", "Article", "ArticleName",
                                            "ProductionDate", "DefectMainID", "DefectSubID","OrderID","SuppID","Refno" };

        public P40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.btnHistory.Visible = true;
        }

        public P40()
        {
            InitializeComponent();
            this.isShowHistory = true;
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            #region 取得過去版本資料
            string whereVersion = this.isShowHistory ? $" and Version < {this.CurrentMaintain["Version"]}" : string.Empty;
            string sqlGetLastADIDASComplain = $@"
select AGCCode, FactoryName 
    from ADIDASComplain_History 
    where   ID = '{this.CurrentMaintain["ID"].ToString()}' and 
            version = (select max(version) from ADIDASComplain_History 
                                    where ID = '{this.CurrentMaintain["ID"].ToString()}' {whereVersion})";

            string sqlGetLastADIDASComplain_Detail = $@"
select SalesID, SalesName, Article, ArticleName, ProductionDate, DefectMainID, DefectSubID, OrderID, SuppID, Refno, UKey
    from ADIDASComplain_Detail_History 
    where   ID = '{this.CurrentMaintain["ID"].ToString()}' and 
            version = (select max(version) from ADIDASComplain_History 
                                    where ID = '{this.CurrentMaintain["ID"].ToString()}' {whereVersion})";

            
            bool hasOldData = MyUtility.Check.Seek(sqlGetLastADIDASComplain, out this.lastADIDASComplain);
            DualResult result = DBProxy.Current.Select(null, sqlGetLastADIDASComplain_Detail, out this.lastADIDASComplain_Detail);
            if (!result)
            {
                this.ShowErr(result);
            }
            this.panelHistoryHint.Visible = hasOldData;
            this.displayAGCCode.BackColor = displayDefaultBack;
            this.displayFactoryName.BackColor = displayDefaultBack;
            if (hasOldData)
            {
                this.btnHistory.Enabled = true;
                this.btnHistory.ForeColor = Color.Blue;

                this.ChangeColorByHistory();
            }
            else
            {
                this.btnHistory.Enabled = false;
                this.btnHistory.ForeColor = Color.Black;
            }
            #endregion
        }

        protected override void SetDetailEditByAndCreateBy(DataRow data)
        {
            if (null == data) return;

            string tpeApvDate = MyUtility.Check.Empty(this.CurrentMaintain["TPEApvDate"]) ? string.Empty : MyUtility.Convert.GetDate(this.CurrentMaintain["TPEApvDate"]).Value.ToString("yyyy/MM/dd HH:mm:ss");
            string ftyApvDate = MyUtility.Check.Empty(this.CurrentMaintain["FtyApvDate"]) ? string.Empty : MyUtility.Convert.GetDate(this.CurrentMaintain["FtyApvDate"]).Value.ToString("yyyy/MM/dd HH:mm:ss");
            string addDate = MyUtility.Check.Empty(this.CurrentMaintain["AddDate"]) ? string.Empty : MyUtility.Convert.GetDate(this.CurrentMaintain["AddDate"]).Value.ToString("yyyy/MM/dd HH:mm:ss");
            string editDate = MyUtility.Check.Empty(this.CurrentMaintain["EditDate"]) ? string.Empty : MyUtility.Convert.GetDate(this.CurrentMaintain["EditDate"]).Value.ToString("yyyy/MM/dd HH:mm:ss");

            string tpeApv = MyUtility.GetValue.Lookup($"select '{this.CurrentMaintain["TPEApvName"]}' + '-' + Name + ' ' + '{tpeApvDate}' from TPEPass1 where ID = '{this.CurrentMaintain["TPEApvName"]}'");
            string addBy = MyUtility.GetValue.Lookup($"select '{this.CurrentMaintain["AddName"]}' + '-' + Name + ' ' + '{tpeApvDate}' from TPEPass1 where ID = '{this.CurrentMaintain["AddName"]}'");
            string editBy = MyUtility.GetValue.Lookup($"select '{this.CurrentMaintain["EditName"]}' + '-' + Name + ' ' + '{tpeApvDate}' from TPEPass1 where ID = '{this.CurrentMaintain["EditName"]}'");
            string ftyApv = MyUtility.GetValue.Lookup($"select '{this.CurrentMaintain["FtyApvName"]}' + '-' + Name + ' ' + '{ftyApvDate}' from Pass1 where ID = '{this.CurrentMaintain["FtyApvName"]}'");
            if (MyUtility.Check.Empty(tpeApv))
            {
                tpeApv = $"{this.CurrentMaintain["TPEApvName"]} {tpeApvDate}";
            }

            if (MyUtility.Check.Empty(addBy))
            {
                tpeApv = $"{this.CurrentMaintain["AddName"]} {addDate}";
            }

            if (MyUtility.Check.Empty(editBy))
            {
                tpeApv = $"{this.CurrentMaintain["EditName"]} {editDate}";
            }

            if (MyUtility.Check.Empty(ftyApv))
            {
                ftyApv = $"{this.CurrentMaintain["FtyApvName"]} {ftyApvDate}";
            }

            this.displayTPEApv.Text = tpeApv;
            this.displayFtyApv.Text = ftyApv;
            this.createby.Text = addBy;
            this.editby.Text = editBy;
        }

        protected override void ClickConfirm()
        {
            bool detailCellHasEmpty = this.DetailDatas.Where(s => MyUtility.Check.Empty(s["SuppID"]) || MyUtility.Check.Empty(s["Refno"])).Any();
            if (detailCellHasEmpty)
            {
                MyUtility.Msg.WarningBox("<Supplier>,<Ref#> can not be empty");
                return;
            }

            string sqlConfirm = $"update ADIDASComplain set FtyApvName = '{Env.User.UserID}',FtyApvDate = GETDATE() where ID = '{this.CurrentMaintain["ID"].ToString()}'";
            DualResult result = DBProxy.Current.Execute(null, sqlConfirm);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            MyUtility.Msg.InfoBox("Confirmed success!");
        }

        protected override void ClickUnconfirm()
        {
            string sqlUnConfirm = $"update ADIDASComplain set FtyApvName = '',FtyApvDate = null where ID = '{this.CurrentMaintain["ID"].ToString()}'";
            DualResult result = DBProxy.Current.Execute(null, sqlUnConfirm);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            MyUtility.Msg.InfoBox("UnConfirmed success!");
            
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string cmd = string.Format(
@"
select 
ad.*,
[MainDefect] = asdMain.ID + '-' + asdMain.Name,
[SubDefect] = asdSub.SubID + '-' + asdSub.SubName,
asdSub.MtlTypeID,
asdSub.FabricType,
o.StyleID
from ADIDASComplain_Detail ad with (nolock)
left join ADIDASComplainDefect asdMain with (nolock) on ad.DefectMainID = asdMain.ID
left join ADIDASComplainDefect_Detail asdSub with (nolock) on  asdMain.ID = asdSub.ID and ad.DefectSubID = asdSub.SubID
left join orders o on o.ID=ad.OrderID
where ad.ID = '{0}'
order by ad.SalesID,ad.Article,asdMain.ID + '-' + asdMain.Name,asdSub.SubID + '-' + asdSub.SubName,ad.OrderID
", masterID);
            this.DetailSelectCommand = cmd;
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override bool ClickSaveBefore()
        {
            return base.ClickSaveBefore();
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            DataGridViewGeneratorTextColumnSettings textSuppID = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings textRefno = new DataGridViewGeneratorTextColumnSettings();

            #region Event

            textSuppID.CellValidating = (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }
                var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                string suppID = e.FormattedValue.ToString();
                if (MyUtility.Check.Empty(suppID)) return;
                if (suppID == dr["SuppID"].ToString()) return;

                string sqlGetSupplier = GetSupplierSql(suppID);
                bool isExistsSupp = MyUtility.Check.Seek(sqlGetSupplier);
                if (!isExistsSupp)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox($"<Supplier>{suppID} not exists");
                    return;
                }
            };

            textSuppID.EditingMouseUp = (s, e) =>
            {
                if (e.Button != MouseButtons.Right || !this.EditMode)
                {
                    return;
                }
                string sqlGetSupplier = GetSupplierSql(string.Empty);
                SelectItem selectItem = new SelectItem(sqlGetSupplier, string.Empty, string.Empty);

                DialogResult dialogResult = selectItem.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                this.CurrentDetailData["SuppID"] = selectItem.GetSelectedString();
            };

            textRefno.CellValidating = (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }
                var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                string refno = e.FormattedValue.ToString();
                if (MyUtility.Check.Empty(refno)) return;
                if (refno == dr["Refno"].ToString()) return;

                string sqlGetRefno = GetRefnoSql(refno);
                bool isExistsRefno = MyUtility.Check.Seek(sqlGetRefno);
                if (!isExistsRefno)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox($"<Refno>{refno} not exists");
                    return;
                }
            };

            textRefno.EditingMouseUp = (s, e) =>
            {
                if (e.Button != MouseButtons.Right || !this.EditMode)
                {
                    return;
                }
                string sqlGetRefno = GetRefnoSql(string.Empty);
                SelectItem selectItem = new SelectItem(sqlGetRefno, string.Empty, string.Empty);

                DialogResult dialogResult = selectItem.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                this.CurrentDetailData["Refno"] = selectItem.GetSelectedString();
            };
            #endregion

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("SalesID", header: "Sales Org. ID", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("SalesName", header: "Sales Org. Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Article", header: "Article ID", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("ArticleName", header: "Article Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("ProductionDate", header: "Production Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("MainDefect", header: "Main Defect", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("SubDefect", header: "Sub Defect", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SuppID", header: "Supplier", width: Widths.AnsiChars(6), settings: textSuppID, iseditingreadonly: this.isShowHistory)
                .Text("Refno", header: "Ref#", width: Widths.AnsiChars(20), settings: textRefno, iseditingreadonly: this.isShowHistory)
                .CheckBox("IsEM", header: "IsEM");

            if (!this.isShowHistory)
            {
                this.detailgrid.Columns["SuppID"].DefaultCellStyle.BackColor = Color.Pink;
                this.detailgrid.Columns["Refno"].DefaultCellStyle.BackColor = Color.Pink;
                this.detailgrid.Columns["IsEM"].DefaultCellStyle.BackColor = Color.Pink;
            }
        }

        private string GetRefnoSql(string refno)
        {
            string whereRefno = MyUtility.Check.Empty(refno) ? string.Empty : $" and PSD.Refno = '{refno}'";
            string sqlGetRefno = $@"
SELECT DISTINCT PSD.Refno
  FROM [PO_Supp_Detail] PSD
  inner JOIN Fabric F ON F.SCIRefno = PSD.SCIRefno
  inner JOIN PO_Supp PS ON PS.ID = PSD.ID AND PS.SEQ1= PSD.SEQ1
  inner join(
	select distinct o.POID,o.ID,oq.OrderIDFrom 
	from Orders o
	left join Order_Qty_Garment oq on oq.ID=o.POID and o.Category='G'
	where o.ID='{this.CurrentDetailData["OrderID"]}'
) o on isnull(o.OrderIDFrom,o.poid) = ps.ID
outer apply(
	select value = RTRIM(LTRIM(data)) from dbo.SplitString('{this.CurrentDetailData["MtlTypeID"].ToString()}',',')
) MtlType
  WHERE o.id = '{this.CurrentDetailData["OrderID"].ToString()}' AND 
        F.MtlTypeID like MtlType.value AND 
        PSD.FabricType = '{this.CurrentDetailData["FabricType"].ToString()}' AND 
        PS.SuppID = '{this.CurrentDetailData["SuppID"].ToString()}' {whereRefno}
ORDER BY PSD.Refno

";

            return sqlGetRefno;
        }

        private string GetSupplierSql(string suppID)
        {
            string whereSuppID = MyUtility.Check.Empty(suppID) ? string.Empty : $" and ps.SuppID = '{suppID}'";
            string sqlGetSupplier = $@"
select distinct [Supplier] = ps.SuppID 
from PO_Supp ps
left join(
	select distinct o.POID,o.ID,oq.OrderIDFrom 
	from Orders o
	left join Order_Qty_Garment oq on oq.ID=o.POID and o.Category='G'
	where o.ID='{this.CurrentDetailData["OrderID"]}'
) o on isnull(o.OrderIDFrom,o.poid) = ps.ID
where o.id = '{this.CurrentDetailData["OrderID"]}' 
{whereSuppID}
order by ps.SuppID
";

            return sqlGetSupplier;
        }

        private void BtnHistory_Click(object sender, EventArgs e)
        {
            new P40_History(this.CurrentMaintain["ID"].ToString()).ShowDialog();
        }

        protected void ChangeColorByHistory()
        {
            if (this.lastADIDASComplain["AGCCode"].ToString() != this.CurrentMaintain["AGCCode"].ToString())
            {
                this.displayAGCCode.BackColor = yellow;
            }

            if (this.lastADIDASComplain["FactoryName"].ToString() != this.CurrentMaintain["FactoryName"].ToString())
            {
                this.displayFactoryName.BackColor = yellow;
            }

            var lastADIDASComplain_Detail_qry = this.lastADIDASComplain_Detail.AsEnumerable();
            foreach (DataGridViewRow curGridRow in this.detailgrid.Rows)
            {
                DataRow curDataRow = this.detailgrid.GetDataRow(curGridRow.Index);
                var getDataRow = lastADIDASComplain_Detail_qry.Where(s => s["UKey"].ToString() == curDataRow["UKey"].ToString());
                if (!getDataRow.Any())
                {
                    continue;
                }

                DataRow lastDataRow = getDataRow.First();

                foreach (string colName in this.compareColumns)
                {
                    if (curDataRow[colName].ToString() == lastDataRow[colName].ToString())
                    {
                        continue;
                    }

                    switch (colName)
                    {
                        case "DefectMainID":
                            curGridRow.Cells["MainDefect"].Style.BackColor = yellow;
                            break;
                        case "DefectSubID":
                            curGridRow.Cells["SubDefect"].Style.BackColor = yellow;
                            break;
                        default:
                            curGridRow.Cells[colName].Style.BackColor = yellow;
                            break;
                    }
                }
            }
        }

        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            if (this.CurrentMaintain != null && !this.EditMode)
            {
                bool isCanConfirm = MyUtility.Check.Empty(this.CurrentMaintain["FtyApvDate"]);
                this.toolbar.cmdConfirm.Enabled = isCanConfirm;
                this.toolbar.cmdUnconfirm.Enabled = !isCanConfirm;
            }
        }
    }
}
