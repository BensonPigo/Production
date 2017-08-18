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
            DualResult result = DBProxy.Current.Select(null, "select NLCode,'' as HSCode,'' as Unit,0.0 as Qty, 0.0 as Waste, 0 as UserCreate from VNConsumption_Detail_Detail WITH (NOLOCK) where 1 = 0", out gridData);

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
                    DataRow dr = this.gridConsumption.GetDataRow<DataRow>(e.RowIndex);
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
                        DataRow dr = this.gridConsumption.GetDataRow<DataRow>(e.RowIndex);
                        Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(string.Format("select distinct NLCode,HSCode,UnitID,Waste from VNContract_Detail WITH (NOLOCK) where ID = '{0}'", contract), "5,8,8,5", MyUtility.Convert.GetString(dr["NLCode"]), headercaptions: "Customs Code,HS Code,Unit, Waste", columndecimals: "0,0,0,3");
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
                DataRow dr = this.gridConsumption.GetDataRow<DataRow>(e.RowIndex);

                if (e.FormattedValue.ToString() != dr["NLCode"].ToString())
                {
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        DataRow seekrow;
                        if (!MyUtility.Check.Seek(string.Format("select UnitID,Waste,HSCode from VNContract_Detail WITH (NOLOCK) where ID = '{0}' and NLCode = '{1}'", contract, e.FormattedValue.ToString()), out seekrow))
                        {
                            dr["NLCode"] = "";
                            dr["HSCode"] = "";
                            dr["Qty"] = 0;
                            dr["Unit"] = "";
                            dr["Waste"] = 0;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Customs Code not found!!");
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
                    DataRow dr = this.gridConsumption.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetString(dr["UserCreate"]) == "1")
                    {
                        MyUtility.Msg.InfoBox("This Customs Code is not create by the system, so no more detail can be show.");
                    }
                    else
                    {
                        DualResult result1;
                        DataTable detail2s = null, selectDataDt;
                        DataRow[] selectData = detaildata.Select(string.Format("StyleUKey = {0} and SizeCode = '{1}' and Article = '{2}' and NLCode = '{3}' and RefNo <> '' ", styleUKey, sizeCode, article, MyUtility.Convert.GetString(dr["NLCode"])));
                        if (selectData != null && selectData.Length > 0)
                        {
                            string getInfoSQL = string.Format(@"
select 	a.RefNo
		, Description = IIF(a.LocalItem = 1,a.Description,a.DescDetail)
		, SuppID = IIF(a.LocalItem = 1,a.LocalSuppid,'') 
		, Type = IIF(a.LocalItem = 1,a.Category,IIF(a.Type = 'F','Fabric','Accessory'))
		, UnitID = IIF(a.LocalItem = 1,a.LUnit,a.FUnit) 
		, a.Qty
from (
	select 	t.RefNo
			, t.Qty
			, t.LocalItem
			, f.DescDetail
			, f.Type
			, f.CustomsUnit as FUnit
			, l.Description
			, l.Category
		 	, l.LocalSuppid
		 	, l.CustomsUnit as LUnit
	from #tmp t
	left join Fabric f WITH (NOLOCK) on t.SCIRefno = f.SCIRefno
	left join LocalItem l WITH (NOLOCK) on t.RefNo = l.RefNo
	where t.NLCode = '{0}'
) a
order by RefNo", MyUtility.Convert.GetString(dr["NLCode"]));
                            result1 = MyUtility.Tool.ProcessWithDatatable(selectData.CopyToDataTable(), "", getInfoSQL, out selectDataDt, "#tmp");
                            if (!result1)
                            {
                                MyUtility.Msg.WarningBox(result1.Description);
                                return;
                            }
                            result1 = DBProxy.Current.Select(null, "select RefNo,'' as Description, '' as SuppID, '' as Type, '' as UnitID, Qty from VNConsumption_Detail_Detail WITH (NOLOCK) where 1 = 0", out detail2s);
                            if (!result1)
                            {
                                MyUtility.Msg.WarningBox(result1.Description);
                                return;
                            }
                            if (selectData != null)
                            {
                                foreach (DataRow selectDr in selectDataDt.Rows)
                                {
                                    DataRow newrow = detail2s.NewRow();
                                    newrow["RefNo"] = selectDr["RefNo"];
                                    newrow["Description"] = selectDr["Description"];
                                    newrow["SuppID"] = selectDr["SuppID"];
                                    newrow["Type"] = selectDr["Type"];
                                    newrow["UnitID"] = selectDr["UnitID"];
                                    newrow["Qty"] = selectDr["Qty"];
                                    detail2s.Rows.Add(newrow);
                                }
                            }
                        }
                        Sci.Production.Shipping.B42_Detail callNextForm = new Sci.Production.Shipping.B42_Detail(detail2s);
                        DialogResult result2 = callNextForm.ShowDialog(this);
                        callNextForm.Dispose();
                    }
                }
            };
            #endregion

            this.gridConsumption.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.gridConsumption)
                .Text("NLCode", header: "Customs Code", width: Widths.AnsiChars(8), settings: nlcode)
                .Text("Unit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", decimal_places: 3, width: Widths.AnsiChars(7), settings: qty)
                .Numeric("Waste", header: "Waste", decimal_places: 3, width: Widths.AnsiChars(5), iseditingreadonly: true);

            listControlBindingSource1.DataSource = gridData;
        }

        //Append
        private void btnAppend_Click(object sender, EventArgs e)
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
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listControlBindingSource1.Position != -1)
            {
                listControlBindingSource1.RemoveCurrent();
            }
        }

        //Save
        private void btnSave_Click(object sender, EventArgs e)
        {
            gridConsumption.EndEdit();
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
                duplicateNLCode.Append("Below Customs Code was duplicate, Pls check!!\r\n");
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
