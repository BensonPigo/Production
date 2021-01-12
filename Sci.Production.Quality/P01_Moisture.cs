using Ict;
using Ict.Win;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P01_Moisture : Win.Subs.Input4
    {
        private readonly string loginID = Env.User.UserID;
        private readonly string id;
        private DataRow maindr;
        private DataTable MoistureStandardListDt;
        private DataRow MoistureStandardListdr;

        /// <inheritdoc/>
        public P01_Moisture(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, DataRow mainDr)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.maindr = mainDr;
            this.id = keyvalue1;
        }

        /// <inheritdoc/>
        protected override DualResult OnRequery()
        {
            this.txtsupplier.TextBox1.IsSupportEditMode = false;
            this.txtsupplier.TextBox1.ReadOnly = true;

            string order_cmd = $"Select * from orders WITH (NOLOCK) where id='{this.maindr["POID"]}'";
            if (MyUtility.Check.Seek(order_cmd, out DataRow order_dr))
            {
                this.displayBrand.Text = order_dr["Brandid"].ToString();
                this.displayStyle.Text = order_dr["Styleid"].ToString();
            }
            else
            {
                this.displayBrand.Text = string.Empty;
                this.displayStyle.Text = string.Empty;
            }

            string po_cmd = $"Select suppid from po_supp WITH (NOLOCK) where id='{this.maindr["POID"]}' and seq1 = '{this.maindr["seq1"]}'";
            this.txtsupplier.TextBox1.Text = MyUtility.GetValue.Lookup(po_cmd);

            string receiving_cmd = $"select a.exportid,a.WhseArrival ,b.Refno from Receiving a WITH (NOLOCK) inner join FIR b WITH (NOLOCK) on a.Id=b.Receivingid where b.id='{this.maindr["id"]}'";
            if (MyUtility.Check.Seek(receiving_cmd, out DataRow rec_dr))
            {
                this.displayWKNo.Text = rec_dr["exportid"].ToString();
                this.displayRefno.Text = rec_dr["Refno"].ToString();
            }
            else
            {
                this.displayWKNo.Text = string.Empty;
                this.displayRefno.Text = string.Empty;
            }

            string po_supp_detail_cmd = $"select SCIRefno,colorid from PO_Supp_Detail WITH (NOLOCK) where id='{this.maindr["POID"]}' and seq1='{this.maindr["seq1"]}' and seq2='{this.maindr["seq2"]}'";
            if (MyUtility.Check.Seek(po_supp_detail_cmd, out DataRow po_supp_detail_dr))
            {
                this.displayColor.Text = po_supp_detail_dr["colorid"].ToString();
            }
            else
            {
                this.displayColor.Text = string.Empty;
            }

            this.displaySCIRefno.Text = this.maindr["SCIRefno"].ToString();
            this.displayArriveQty.Text = this.maindr["arriveQty"].ToString();
            this.dateLastInspectionDate.Value = MyUtility.Convert.GetDate(this.maindr["MoistureDate"]);
            this.dateArriveWHDate.Value = MyUtility.Convert.GetDate(this.maindr["WhseArrival"]);
            this.displaySCIRefno1.Text = MyUtility.GetValue.Lookup("Description", this.maindr["SciRefno"].ToString(), "Fabric", "SCIRefno");
            this.displaySEQ.Text = this.maindr["Seq1"].ToString() + "-" + this.maindr["Seq2"].ToString();
            this.displaySP.Text = this.maindr["POID"].ToString();
            this.checkNonMoisture.Value = this.maindr["nonMoisture"].ToString();
            this.displayResult.Text = this.maindr["Moisture"].ToString();
            this.SetMaterialCompositionDatas();
            this.cbmMaterialCompositionGrp.Text = this.maindr["MaterialCompositionGrp"].ToString();
            this.cbmMaterialCompositionItem.Text = this.maindr["MaterialCompositionItem"].ToString();
            this.displayMoistureStandard.Text = this.maindr["MoistureStandardDesc"].ToString();

            return base.OnRequery();
        }

        /// <inheritdoc/>
        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);
            datas.Columns.Add("Name", typeof(string));
            datas.Columns.Add("POID", typeof(string));
            datas.Columns.Add("SEQ1", typeof(string));
            datas.Columns.Add("SEQ2", typeof(string));

            foreach (DataRow dr in datas.Rows)
            {
                dr["Name"] = MyUtility.GetValue.Lookup("Name", dr["Inspector"].ToString(), "Pass1", "ID");
                dr["poid"] = this.maindr["poid"];
                dr["SEQ1"] = this.maindr["SEQ1"];
                dr["SEQ2"] = this.maindr["SEQ2"];
            }
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings rollcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings actMoisture = new DataGridViewGeneratorNumericColumnSettings();

            #region Roll
            rollcell.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode == false)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    string roll_cmd = string.Format("Select roll,dyelot from Receiving_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}'", this.maindr["Receivingid"], this.maindr["Poid"], this.maindr["seq1"], this.maindr["seq2"]);

                    if (!MyUtility.Check.Seek(roll_cmd))
                    {
                        roll_cmd = string.Format("Select roll,dyelot from TransferIn_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}'", this.maindr["Receivingid"], this.maindr["Poid"], this.maindr["seq1"], this.maindr["seq2"]);
                    }

                    sele = new SelectItem(roll_cmd, "15,10,10", dr["roll"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Roll"] = sele.GetSelecteds()[0]["Roll"].ToString().Trim();
                    dr["Dyelot"] = sele.GetSelecteds()[0]["Dyelot"].ToString().Trim();
                }
            };
            rollcell.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Roll"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    // 沒填入資料,清空dyelot
                    dr["Roll"] = string.Empty;
                    dr["Dyelot"] = string.Empty;
                    return;
                }

                // 手動輸入,oldvalue <> newvalue,就不會return並且繼續判斷
                if (oldvalue == newvalue)
                {
                    return;
                }

                string roll_cmd = string.Format("Select roll,dyelot,StockQty from Receiving_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' and roll='{4}'", this.maindr["Receivingid"], this.maindr["Poid"], this.maindr["seq1"], this.maindr["seq2"], e.FormattedValue);

                if (!MyUtility.Check.Seek(roll_cmd))
                {
                    roll_cmd = string.Format("Select roll,dyelot,StockQty=qty from TransferIn_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' and roll='{4}'", this.maindr["Receivingid"], this.maindr["Poid"], this.maindr["seq1"], this.maindr["seq2"], e.FormattedValue);
                }

                DataRow roll_dr;
                if (MyUtility.Check.Seek(roll_cmd, out roll_dr))
                {
                    dr["Roll"] = roll_dr["Roll"];
                    dr["Dyelot"] = roll_dr["Dyelot"];
                    dr.EndEdit();
                }
                else
                {
                    dr["Roll"] = string.Empty;
                    dr["Dyelot"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Roll: {0}> data not found!", e.FormattedValue));
                    return;
                }
            };

            actMoisture.CellValidating += (s, e) =>
            {
                if (e.FormattedValue == DBNull.Value)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                dr["ActMoisture"] = e.FormattedValue;
                dr.EndEdit();
                this.CheckResult();
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.grid)
               .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), settings: rollcell)
               .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .Numeric("ActMoisture", header: "Actual Moisture", maximum: 100, decimal_places: 1, settings: actMoisture)
               .Text("Result", header: "Result", width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Date("InspDate", header: "Insp.Date", width: Widths.AnsiChars(10))
               .CellUser("Inspector", header: "Inspector", width: Widths.AnsiChars(10), userNamePropertyName: "Name")
               .Text("Name", header: "Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
               ;
            this.grid.Columns["Roll"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["ActMoisture"].DefaultCellStyle.BackColor = Color.MistyRose;

            this.grid.Columns["InspDate"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["Inspector"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["Name"].DefaultCellStyle.BackColor = Color.MistyRose;
            return true;
        }

        /// <inheritdoc/>
        protected override void OnInsert()
        {
            base.OnInsert();

            DataRow selectDr = ((DataRowView)this.grid.GetSelecteds(SelectedSort.Index)[0]).Row;

            selectDr["ActMoisture"] = DBNull.Value;
            selectDr["Result"] = string.Empty;
            selectDr["Inspdate"] = DateTime.Now.ToShortDateString();
            selectDr["Inspector"] = this.loginID;
            selectDr["Name"] = MyUtility.GetValue.Lookup("Name", this.loginID, "Pass1", "ID");
            selectDr["poid"] = this.maindr["poid"];
            selectDr["SEQ1"] = this.maindr["SEQ1"];
            selectDr["SEQ2"] = this.maindr["SEQ2"];
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            DataTable gridTb = (DataTable)this.gridbs.DataSource;
            #region 判斷空白不可存檔
            DataRow[] drArray;
            drArray = gridTb.Select("Roll=''");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Roll> can not be empty.");
                return false;
            }

            drArray = gridTb.Select("Inspdate is null");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Insection Date> can not be empty.");
                return false;
            }

            drArray = gridTb.Select("inspector=''");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Inspector> can not be empty.");
                return false;
            }
            #endregion

            return base.OnSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult OnSavePost()
        {
            DataTable gridTb = (DataTable)this.gridbs.DataSource;

            string sqlcmd = $@"
Select distinct dyelot
from Receiving_Detail a WITH (NOLOCK)
where a.id='{this.maindr["receivingid"]}'
and a.poid='{this.maindr["POID"]}'
and a.seq1 ='{this.maindr["seq1"]}'
and a.seq2='{this.maindr["seq2"]}' 
";
            DualResult dual = DBProxy.Current.Select(null, sqlcmd, out DataTable dyeDt);
            if (!dual)
            {
                this.ShowErr(dual);
            }

            string result = "Pass";
            if (gridTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Count() == 0)
            {
                result = string.Empty;
            }
            else
            {
                if (gridTb.Select("Result = ''").Length > 0)
                {
                    result = string.Empty;
                }
                else if (gridTb.Select("Result = 'Fail'").Length > 0)
                {
                    result = "Fail";
                }

                // 沒勾選 nonMoisture 要判斷 必需的 Dyelot
                if (!MyUtility.Convert.GetBool(this.maindr["nonMoisture"]))
                {
                    var nowdyelotList = gridTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted)
                        .Select(s => MyUtility.Convert.GetString(s["dyelot"])).Distinct().ToList();
                    var needdyelotList = dyeDt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["dyelot"])).Distinct().ToList();
                    if (needdyelotList.Except(nowdyelotList).Any() || nowdyelotList.Except(needdyelotList).Any())
                    {
                        result = string.Empty;
                    }
                }
            }

            this.maindr["Moisture"] = result;
            this.maindr["MaterialCompositionGrp"] = this.cbmMaterialCompositionGrp.Text;
            this.maindr["MaterialCompositionItem"] = this.cbmMaterialCompositionItem.Text;
            this.maindr["MoistureStandardDesc"] = this.displayMoistureStandard.Text;
            string[] returnstr = PublicPrg.Prgs.GetOverallResult_Status(this.maindr);

            int c1 = 0, c2 = 0;
            decimal s1 = 0, s2 = 0;
            if (this.MoistureStandardListdr != null)
            {
                s1 = MyUtility.Convert.GetDecimal(this.MoistureStandardListdr["MoistureStandard1"]);
                s2 = MyUtility.Convert.GetDecimal(this.MoistureStandardListdr["MoistureStandard2"]);
                c1 = MyUtility.Convert.GetInt(this.MoistureStandardListdr["MoistureStandard1_Comparison"]);
                c2 = MyUtility.Convert.GetInt(this.MoistureStandardListdr["MoistureStandard2_Comparison"]);
            }

            string cmd = $@"
update FIR set
    Result='{returnstr[0]}',
    Status='{returnstr[1]}',
	Moisture = '{result}',
	MoistureDate = GETDATE(),
	MaterialCompositionGrp='{this.cbmMaterialCompositionGrp.Text}',
	MaterialCompositionItem='{this.cbmMaterialCompositionItem.Text}',
	MoistureStandardDesc=N'{this.displayMoistureStandard.Text}',
	MoistureStandard1={s1},
	MoistureStandard2={s2},
	MoistureStandard1_Comparison={c1},
	MoistureStandard2_Comparison={c2},
    EditName='{this.loginID}',
    EditDate = GetDate()
where ID = '{this.id}'
";
            dual = DBProxy.Current.Execute(null, cmd);
            if (!dual)
            {
                return dual;
            }

            // 更新PO.FIRInspPercent和AIRInspPercent
            dual = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'FIR','{this.maindr["POID"].ToString()}'; ");
            if (!dual)
            {
                return dual;
            }

            return base.OnSavePost();
        }

        private void SetMaterialCompositionDatas()
        {
            string cmd = $@"select * from Brand_QAMoistureStandardList where BrandID = '{this.displayBrand.Text}'";
            DualResult result = DBProxy.Current.Select(null, cmd, out this.MoistureStandardListDt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return;
            }

            var m_grp = this.MoistureStandardListDt.AsEnumerable().Select(s => s["MaterialCompositionGrp"].ToString()).Distinct().ToArray();
            Dictionary<string, string> comboBoxRowSource = new Dictionary<string, string>();
            comboBoxRowSource.Add(string.Empty, string.Empty);
            foreach (var item in m_grp)
            {
                comboBoxRowSource.Add(item, item);
            }

            this.cbmMaterialCompositionGrp.DataSource = new System.Windows.Forms.BindingSource(comboBoxRowSource, null);
            this.cbmMaterialCompositionGrp.ValueMember = "Key";
            this.cbmMaterialCompositionGrp.DisplayMember = "Value";
            this.cbmMaterialCompositionGrp.SelectedIndex = 0;
        }

        private void CbmMaterialCompositionGrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cbmMaterialCompositionItem.DataSource = null;
            DataRow[] drs = this.MoistureStandardListDt.Select($"MaterialCompositionGrp = '{this.cbmMaterialCompositionGrp.Text}'");
            if (drs.Length > 0)
            {
                Dictionary<string, string> comboBoxRowSource = new Dictionary<string, string>();
                comboBoxRowSource.Add(string.Empty, string.Empty);
                foreach (var item in drs)
                {
                    comboBoxRowSource.Add(item["MaterialCompositionItem"].ToString(), item["MaterialCompositionItem"].ToString());
                }

                this.cbmMaterialCompositionItem.DataSource = new System.Windows.Forms.BindingSource(comboBoxRowSource, null);
                this.cbmMaterialCompositionItem.ValueMember = "Key";
                this.cbmMaterialCompositionItem.DisplayMember = "Value";
                this.cbmMaterialCompositionItem.SelectedIndex = 0;
            }
        }

        private void CbmMaterialCompositionItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRow[] drs = this.MoistureStandardListDt.Select($"MaterialCompositionGrp = '{this.cbmMaterialCompositionGrp.Text}' and MaterialCompositionItem = '{this.cbmMaterialCompositionItem.Text}'");
            if (drs.Length > 0)
            {
                this.displayMoistureStandard.Text = drs[0]["MoistureStandardDesc"].ToString();
                this.MoistureStandardListdr = drs[0];
            }
            else
            {
                this.displayMoistureStandard.Text = string.Empty;
                this.MoistureStandardListdr = null;
            }

            this.CheckResult();
        }

        private void CheckResult()
        {
            if (this.gridbs.DataSource == null)
            {
                return;
            }

            if (this.MoistureStandardListdr == null)
            {
                foreach (DataRow dr in ((DataTable)this.gridbs.DataSource).AsEnumerable().Where(w => w.RowState != DataRowState.Deleted))
                {
                    dr["Result"] = string.Empty;
                }
            }
            else
            {
                int c1 = MyUtility.Convert.GetInt(this.MoistureStandardListdr["MoistureStandard1_Comparison"]);
                int c2 = MyUtility.Convert.GetInt(this.MoistureStandardListdr["MoistureStandard2_Comparison"]);
                decimal s1 = MyUtility.Convert.GetDecimal(this.MoistureStandardListdr["MoistureStandard1"]);
                decimal s2 = MyUtility.Convert.GetDecimal(this.MoistureStandardListdr["MoistureStandard2"]);

                foreach (DataRow dr in ((DataTable)this.gridbs.DataSource).AsEnumerable().Where(w => w.RowState != DataRowState.Deleted))
                {
                    if (dr["ActMoisture"] == DBNull.Value)
                    {
                        dr["Result"] = string.Empty;
                    }
                    else if (this.CheckType(c1, s1, MyUtility.Convert.GetDecimal(dr["ActMoisture"])) &&
                        this.CheckType(c2, s2, MyUtility.Convert.GetDecimal(dr["ActMoisture"])))
                    {
                        dr["Result"] = "Pass";
                    }
                    else
                    {
                        dr["Result"] = "Fail";
                    }
                }
            }
        }

        private bool CheckType(int c, decimal s, decimal a)
        {
            switch (c)
            {
                case 0:
                    return s == a;
                case 1:
                    return s < a;
                case 2:
                    return s <= a;
                case 3:
                    return s > a;
                case 4:
                    return s >= a;
                default:
                    return false;
            }
        }
    }
}
