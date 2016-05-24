using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class B42_BatchCreate_Consumption : Sci.Win.Subs.Base
    {
        DataTable middetaildata, detaildata;
        string styleUKey, sizeCode, article, contract;
        Ict.Win.DataGridViewGeneratorTextColumnSettings nlcode = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorNumericColumnSettings qty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        public B42_BatchCreate_Consumption(DataTable MidDetailData, DataTable DetailData, string StyleUKey, string SizeCode, string Article, string ContractID)
        {
            InitializeComponent();
            middetaildata = MidDetailData;
            detaildata = DetailData;
            styleUKey = StyleUKey;
            sizeCode = SizeCode;
            article = Article;
            contract = ContractID;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            //組Grid資料
            DataRow[] selectedData = middetaildata.Select(string.Format("StyleUKey = {0} and SizeCode = '{1}' and Article = '{2}' and Deleted = 0", styleUKey, sizeCode, article));
            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, "select NLCode,'' as HSCode,'' as Unit,0.0 as Qty, 0.0 as Waste, 0 as UserCreate from VNConsumption_Detail_Detail where 1 = 0", out gridData);

            for (int i = 0; i < selectedData.Length; i++)
            {
                DataRow newrow = gridData.NewRow();
                newrow["NLCode"] = selectedData[i]["NLCode"];
                newrow["HSCode"] = selectedData[i]["HSCode"];
                newrow["Unit"] = selectedData[i]["UnitID"];
                newrow["Qty"] = selectedData[i]["Qty"];
                newrow["Waste"] = selectedData[i]["Waste"];
                newrow["UserCreate"] = selectedData[i]["UserCreate"];
                gridData.Rows.Add(newrow);
            }

            #region NL Code 按右鍵與validating與EditingControlShowing
            nlcode.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex != -1)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetString(dr["UserCreate"]) == "0")
                    {
                        e.Control.Enabled = false;
                    }
                    else
                    {
                        e.Control.Enabled = true;
                    }
                }
            };


            nlcode.EditingMouseDown += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if (e.RowIndex != -1)
                    {
                        DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                        Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(string.Format("select distinct NLCode,HSCode,UnitID,Waste from VNContract_Detail where ID = '{0}'", contract), "5,8,8,5", MyUtility.Convert.GetString(dr["NLCode"]), headercaptions: "NL Code,HS Code,Unit, Waste", columndecimals: "0,0,0,3");
                        DialogResult returnResult = item.ShowDialog();
                        if (returnResult == DialogResult.Cancel) { return; }

                        dr["NLCode"] = (item.GetSelecteds())[0]["NLCode"];
                        dr["HSCode"] = (item.GetSelecteds())[0]["HSCode"];
                        dr["Qty"] = 0;
                        dr["Unit"] = (item.GetSelecteds())[0]["UnitID"];
                        dr["Waste"] = (item.GetSelecteds())[0]["Waste"];
                    }
                }
            };

            nlcode.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);

                if (e.FormattedValue.ToString() != dr["NLCode"].ToString())
                {
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        DataRow seekrow;
                        if (!MyUtility.Check.Seek(string.Format("select UnitID,Waste,HSCode from VNContract_Detail where ID = '{0}' and NLCode = '{1}'", contract, e.FormattedValue.ToString()), out seekrow))
                        {
                            MyUtility.Msg.WarningBox("NL Code not found!!");
                            dr["NLCode"] = "";
                            dr["HSCode"] = "";
                            dr["Qty"] = 0;
                            dr["Unit"] = "";
                            dr["Waste"] = 0;
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            dr["NLCode"] = e.FormattedValue.ToString();
                            dr["HSCode"] = seekrow["HSCode"];
                            dr["Qty"] = 0;
                            dr["Unit"] = seekrow["UnitID"];
                            dr["Waste"] = seekrow["Waste"];
                        }
                    }
                    else
                    {
                        dr["NLCode"] = "";
                        dr["HSCode"] = "";
                        dr["Qty"] = 0;
                        dr["Unit"] = "";
                        dr["Waste"] = 0;
                    }
                }
            };
            #endregion

            #region Qty 的DBClick
            qty.EditingMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetString(dr["UserCreate"]) == "1")
                    {
                        MyUtility.Msg.InfoBox("This NL Code is not create by the system, so no more detail can be show.");
                    }
                    else
                    {
                        DataTable detail2s;
                        DataRow[] selectData = detaildata.Select(string.Format("StyleUKey = {0} and SizeCode = '{1}' and Article = '{2}' and NLCode = '{3}' and RefNo <> '' ", styleUKey, sizeCode, article, MyUtility.Convert.GetString(dr["NLCode"])));
                        DualResult result1 = DBProxy.Current.Select(null, "select RefNo,'' as Description, '' as SuppID, '' as Type, '' as UnitID, Qty from VNConsumption_Detail_Detail where 1 = 0", out detail2s);
                        for (int i = 0; i < selectData.Length; i++)
                        {
                            DataRow newrow = detail2s.NewRow();
                            newrow["RefNo"] = selectData[i]["RefNo"];
                            newrow["Description"] = selectData[i]["Description"];
                            newrow["SuppID"] = selectData[i]["RefNo"];
                            newrow["Type"] = selectData[i]["Type"];
                            newrow["UnitID"] = selectData[i]["CustomsUnit"];
                            newrow["Qty"] = selectData[i]["Qty"];
                            detail2s.Rows.Add(newrow);
                        }
                        Sci.Production.Shipping.B42_Detail callNextForm = new Sci.Production.Shipping.B42_Detail(detail2s);
                        DialogResult result2 = callNextForm.ShowDialog(this);
                        callNextForm.Dispose();
                    }
                }
            };
            #endregion

            this.grid1.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("NLCode", header: "NL Code", width: Widths.AnsiChars(8), settings: nlcode)
                .Text("Unit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", decimal_places: 3, width: Widths.AnsiChars(7), settings: qty)
                .Numeric("Waste", header: "Waste", decimal_places: 3, width: Widths.AnsiChars(5), iseditingreadonly: true);

            listControlBindingSource1.DataSource = gridData;
        }

        //Append
        private void button1_Click(object sender, EventArgs e)
        {
            DataRow newrow = ((DataTable)listControlBindingSource1.DataSource).NewRow();
            newrow["NLCode"] = "";
            newrow["Unit"] = "";
            newrow["Qty"] = 0;
            newrow["Waste"] = 0;
            newrow["UserCreate"] = 1;
            ((DataTable)listControlBindingSource1.DataSource).Rows.Add(newrow);
        }

        //Delete
        private void button2_Click(object sender, EventArgs e)
        {
            if (listControlBindingSource1.Position != -1)
            {
                listControlBindingSource1.RemoveCurrent();
            }
        }

        //Save
        private void button3_Click(object sender, EventArgs e)
        {
            grid1.EndEdit();
            DataTable gridData = (DataTable)listControlBindingSource1.DataSource;
            #region 檢查是否有Qty為0的
            foreach (DataRow dr in gridData.Rows)
            {
                if (MyUtility.Check.Empty(dr["Qty"]))
                {
                    MyUtility.Msg.WarningBox("Qty can't empty!!");
                    return;
                }
            }
            #endregion

            #region 檢查是否有重複的NL Code
            DataTable DuplicateData;
            try
            {
                MyUtility.Tool.ProcessWithDatatable(gridData, "NLCode",
                    "select NLCode, count(NLCode) as Ctn from #tmp group by NLCode having count(NLCode) > 1", out DuplicateData);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Check duplicate data fail!!\r\n" + ex.ToString());
                return;
            }
            if (DuplicateData != null && DuplicateData.Rows.Count > 0)
            {
                StringBuilder duplicateNLCode = new StringBuilder();
                duplicateNLCode.Append("Below NL Code was duplicate, Pls check!!\r\n");
                foreach (DataRow dr in DuplicateData.Rows)
                {
                    duplicateNLCode.Append(string.Format("{0}\r\n", MyUtility.Convert.GetString(dr["NLCode"])));
                }
                MyUtility.Msg.WarningBox(duplicateNLCode.ToString());
                return;
            }
            #endregion

            #region 刪除
            foreach (DataRow dr in middetaildata.Rows)
            {
                if (MyUtility.Convert.GetString(dr["StyleUKey"]) == styleUKey && MyUtility.Convert.GetString(dr["SizeCode"]) == sizeCode && MyUtility.Convert.GetString(dr["Article"]) == article)
                {
                    DataRow[] selectedData = gridData.Select(string.Format("NLCode = '{0}'", MyUtility.Convert.GetString(dr["NLCode"])));
                    if (selectedData.Length <= 0)
                    {
                        dr["Deleted"] = 1;
                    }
                }
            }
            #endregion

            #region 新增& 修改
            foreach (DataRow dr in gridData.Rows)
            {
                DataRow[] selectedData = middetaildata.Select(string.Format("StyleUKey = {0} and SizeCode = '{1}' and Article = '{2}' and NLCode = '{3}' and Deleted = 0", styleUKey, sizeCode, article, MyUtility.Convert.GetString(dr["NLCode"])));
                if (selectedData.Length > 0)
                {
                    selectedData[0]["Qty"] = dr["Qty"];
                }
                else
                {
                    DataRow newrow = middetaildata.NewRow();
                    newrow["NLCode"] = dr["NLCode"];
                    newrow["HSCode"] = dr["HSCode"];
                    newrow["UnitID"] = dr["Unit"];
                    newrow["Qty"] = dr["Qty"];
                    newrow["Waste"] = dr["Waste"];
                    newrow["UserCreate"] = dr["UserCreate"];
                    newrow["StyleUKey"] = styleUKey;
                    newrow["SizeCode"] = sizeCode;
                    newrow["Article"] = article;
                    newrow["Deleted"] = 0;
                    middetaildata.Rows.Add(newrow);
                }
            }
            #endregion

            DialogResult = System.Windows.Forms.DialogResult.OK;

        }
    }
}
