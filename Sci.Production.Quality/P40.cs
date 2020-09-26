using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class P40 : Win.Tems.Input6
    {
        protected DataRow lastADIDASComplain;
        protected DataTable lastADIDASComplain_Detail;
        protected bool isShowHistory = false;
        protected Color yellow = Color.FromArgb(255, 198, 10);
        protected Color displayDefaultBack = Color.FromArgb(183, 227, 255);

        private readonly string[] compareColumns =
        {
            "SalesID", "SalesName", "Article", "ArticleName",
            "ProductionDate", "DefectMainID", "DefectSubID", "OrderID", "SuppID", "Refno",
        };

        public P40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.btnHistory.Visible = true;
        }

        public P40()
        {
            this.InitializeComponent();
            this.isShowHistory = true;
        }

        /// <inheritdoc/>
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
            this.displayAGCCode.BackColor = this.displayDefaultBack;
            this.displayFactoryName.BackColor = this.displayDefaultBack;
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

        /// <inheritdoc/>
        protected override void SetDetailEditByAndCreateBy(DataRow data)
        {
            if (data == null)
            {
                return;
            }

            string tpeApvDate = MyUtility.Check.Empty(this.CurrentMaintain["TPEApvDate"]) ? string.Empty : MyUtility.Convert.GetDate(this.CurrentMaintain["TPEApvDate"]).Value.ToString("yyyy/MM/dd HH:mm:ss");
            string ftyApvDate = MyUtility.Check.Empty(this.CurrentMaintain["FtyApvDate"]) ? string.Empty : MyUtility.Convert.GetDate(this.CurrentMaintain["FtyApvDate"]).Value.ToString("yyyy/MM/dd HH:mm:ss");
            string addDate = MyUtility.Check.Empty(this.CurrentMaintain["AddDate"]) ? string.Empty : MyUtility.Convert.GetDate(this.CurrentMaintain["AddDate"]).Value.ToString("yyyy/MM/dd HH:mm:ss");
            string editDate = MyUtility.Check.Empty(this.CurrentMaintain["EditDate"]) ? string.Empty : MyUtility.Convert.GetDate(this.CurrentMaintain["EditDate"]).Value.ToString("yyyy/MM/dd HH:mm:ss");

            string tpeApv = MyUtility.GetValue.Lookup($"select '{this.CurrentMaintain["TPEApvName"]}' + '-' + Name + ' ' + '{tpeApvDate}' from TPEPass1 where ID = '{this.CurrentMaintain["TPEApvName"]}'");
            string addBy = MyUtility.GetValue.Lookup($"select '{this.CurrentMaintain["AddName"]}' + '-' + Name + ' ' + '{addDate}' from TPEPass1 where ID = '{this.CurrentMaintain["AddName"]}'");
            string editBy = MyUtility.GetValue.Lookup($"select '{this.CurrentMaintain["EditName"]}' + '-' + Name + ' ' + '{editDate}' from TPEPass1 where ID = '{this.CurrentMaintain["EditName"]}'");
            string ftyApv = MyUtility.GetValue.Lookup($"select '{this.CurrentMaintain["FtyApvName"]}' + '-' + Name + ' ' + '{ftyApvDate}' from Pass1 where ID = '{this.CurrentMaintain["FtyApvName"]}'");
            if (MyUtility.Check.Empty(tpeApv))
            {
                tpeApv = $"{this.CurrentMaintain["TPEApvName"]} {tpeApvDate}";
            }

            if (MyUtility.Check.Empty(addBy))
            {
                addBy = $"{this.CurrentMaintain["AddName"]} {addDate}";
            }

            if (MyUtility.Check.Empty(editBy))
            {
                editBy = $"{this.CurrentMaintain["EditName"]} {editDate}";
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["id"].ToString();
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

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            DataGridViewGeneratorTextColumnSettings textSuppID = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings textRefno = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings responsibility = new DataGridViewGeneratorTextColumnSettings();

            #region Event
            textSuppID.CellValidating = (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                string suppID = e.FormattedValue.ToString();
                if (MyUtility.Check.Empty(suppID))
                {
                    dr["SuppID"] = string.Empty;
                    dr["IsLocalSupp"] = false;
                    return;
                }

                if (suppID == dr["SuppID"].ToString())
                {
                    return;
                }

                if (suppID == "N/A")
                {
                    dr["SuppID"] = "N/A";
                    dr["IsLocalSupp"] = false;
                }
                else
                {
                    string sqlGetSupplier = this.GetSupplierSql(suppID);
                    DataRow findRow;
                    bool isExistsSupp = MyUtility.Check.Seek(sqlGetSupplier, out findRow);
                    if (!isExistsSupp)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox($"<Supplier>{suppID} not exists");
                        return;
                    }

                    dr["SuppID"] = suppID;

                    dr["IsLocalSupp"] = MyUtility.Convert.GetString(findRow["Is Local Supp"]) == "Y" ? true : false;
                }

                dr.EndEdit();
            };

            textSuppID.EditingMouseUp = (s, e) =>
            {
                if (e.Button != MouseButtons.Right || !this.EditMode)
                {
                    return;
                }

                string sqlGetSupplier = this.GetSupplierSql(string.Empty, true);
                SelectItem selectItem = new SelectItem(sqlGetSupplier, "10,50,5", string.Empty, string.Empty);
                selectItem.Width = 900;
                DialogResult dialogResult = selectItem.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                this.CurrentDetailData["SuppID"] = selectItem.GetSelectedString();

                var selectRows = selectItem.GetSelecteds();
                bool isLocal = false;
                isLocal = MyUtility.Convert.GetString(selectRows[0]["Is Local Supp"]) == "Y" ? true : false;
                this.CurrentDetailData["IsLocalSupp"] = isLocal;
            };

            textRefno.CellValidating = (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                string refno = e.FormattedValue.ToString();
                if (MyUtility.Check.Empty(refno))
                {
                    dr["Refno"] = string.Empty;
                    return;
                }

                if (refno == dr["Refno"].ToString())
                {
                    return;
                }

                if (refno == "N/A")
                {
                    dr["Refno"] = refno;
                }
                else
                {
                    string sqlGetRefno = this.GetRefnoSql(refno);
                    bool isExistsRefno = MyUtility.Check.Seek(sqlGetRefno);
                    if (!isExistsRefno)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox($"<Refno>{refno} not exists");
                        return;
                    }

                    dr["Refno"] = refno;
                }

                dr.EndEdit();
            };

            textRefno.EditingMouseUp = (s, e) =>
            {
                if (e.Button != MouseButtons.Right || !this.EditMode)
                {
                    return;
                }

                string sqlGetRefno = this.GetRefnoSql(string.Empty, true);
                SelectItem selectItem = new SelectItem(sqlGetRefno, string.Empty, string.Empty);

                DialogResult dialogResult = selectItem.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                this.CurrentDetailData["Refno"] = selectItem.GetSelectedString();
            };

            textSuppID.CellMouseDoubleClick = (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                if (e.Button == MouseButtons.Left)
                {
                    this.CurrentDetailData["SuppID"] = @"N/A";
                    this.CurrentDetailData["IsLocalSupp"] = false;
                    this.CurrentDetailData.EndEdit();
                }
            };
            textRefno.CellMouseDoubleClick = (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                if (e.Button == MouseButtons.Left)
                {
                    this.CurrentDetailData["Refno"] = @"N/A";
                    this.CurrentDetailData.EndEdit();
                }
            };

            responsibility.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    string sqlcmd = @"select distinct responsibility from ADIDASComplainDefect_Detail";
                    SelectItem item1 = new SelectItem(sqlcmd, string.Empty, this.CurrentDetailData["responsibility"].ToString());
                    DialogResult result = item1.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["responsibility"] = item1.GetSelectedString();
                    this.CurrentDetailData.EndEdit();
                }
            };
            responsibility.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                string sqlcmd = $@"select 1 from ADIDASComplainDefect_Detail where responsibility = '{e.FormattedValue}'";
                if (!MyUtility.Check.Seek(sqlcmd))
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    this.CurrentDetailData["responsibility"] = string.Empty;
                }
                else
                {
                    this.CurrentDetailData["responsibility"] = e.FormattedValue;
                }

                this.CurrentDetailData.EndEdit();
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
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
                .Text("Responsibility", header: "Responsibility", width: Widths.AnsiChars(20), settings: responsibility, iseditingreadonly: this.isShowHistory)

                .CheckBox("IsEM", header: "IsEM");

            if (!this.isShowHistory)
            {
                this.detailgrid.Columns["SuppID"].DefaultCellStyle.BackColor = Color.Pink;
                this.detailgrid.Columns["Refno"].DefaultCellStyle.BackColor = Color.Pink;
                this.detailgrid.Columns["IsEM"].DefaultCellStyle.BackColor = Color.Pink;
                this.detailgrid.Columns["Responsibility"].DefaultCellStyle.BackColor = Color.Pink;
            }
        }

        private string GetRefnoSql(string refno, bool isRightClick = false)
        {
            string whereRefno = isRightClick ? string.Empty : $" AND p.Refno='{refno}' ";
            string sqlGetRefno = $@"
SELECT DISTINCT p.Refno
,[FabricType]=  CASE WHEN p.FabricType = 'F' THEN 'Fabric'
					 WHEN p.FabricType = 'A' THEN 'Accessory' ELSE '' 
				END 
,f.Description
FROM PO_Supp_Detail p
LEFT JOIN Orders o ON o.POID=p.ID
LEFT JOIN Fabric f ON p.SCIRefno = f.SCIRefno
WHERE o.ID='{this.CurrentDetailData["OrderID"]}' {whereRefno}
UNION
SELECT DISTINCT p.Refno 
,[FabricType]=  CASE WHEN p.FabricType = 'F' THEN 'Fabric'
					 WHEN p.FabricType = 'A' THEN 'Accessory' ELSE '' 
				END 
,f.Description
FROM PO_Supp_Detail p
LEFT JOIN Order_Qty_Garment o ON o.OrderIDFrom=p.ID
LEFT JOIN Fabric f ON p.SCIRefno = f.SCIRefno
WHERE o.ID = '{this.CurrentDetailData["OrderID"]}' {whereRefno}

";

            return sqlGetRefno;
        }

        private string GetSupplierSql(string suppID, bool isRightClick = false)
        {
            string whereSuppID = isRightClick ? string.Empty : $"  AND p.SuppID='{suppID}' ";
            string whereSuppID_subcon = isRightClick ? string.Empty : $"  AND a.LocalSuppID='{suppID}' ";
            string sqlGetSupplier = $@"
SELECT DISTINCT p.SuppID ,[Supp Name]=Supp.NameEN,[Is Local Supp]='N'
FROM PO_Supp p
LEFT JOIN Orders o ON o.POID=p.ID
LEFT JOIN Supp ON p.SuppID = Supp.ID
WHERE o.ID='{this.CurrentDetailData["OrderID"]}' {whereSuppID}
UNION 
SELECT DISTINCT [SuppID]= a.LocalSuppID ,[Supp Name]=LocalSupp.Name,[Is Local Supp ]='Y'
FROM ArtworkPO a
LEFT JOIN LocalSupp ON a.LocalSuppID = LocalSupp.ID
WHERE a.Status IN ('Closed','Approved') {whereSuppID_subcon}
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
                this.displayAGCCode.BackColor = this.yellow;
            }

            if (this.lastADIDASComplain["FactoryName"].ToString() != this.CurrentMaintain["FactoryName"].ToString())
            {
                this.displayFactoryName.BackColor = this.yellow;
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
                            curGridRow.Cells["MainDefect"].Style.BackColor = this.yellow;
                            break;
                        case "DefectSubID":
                            curGridRow.Cells["SubDefect"].Style.BackColor = this.yellow;
                            break;
                        default:
                            curGridRow.Cells[colName].Style.BackColor = this.yellow;
                            break;
                    }
                }
            }
        }

        /// <inheritdoc/>
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
