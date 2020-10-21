using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using static Sci.Production.Packing.P26;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class P26_AssignPackingList : Win.Tems.Base
    {
        private List<NewFormModel> _NewFormModels;
        private DataTable _P25Dt;
        private DataTable GridDt = new DataTable();
        private bool isClickProcessing = false;

        /// <inheritdoc/>
        public bool canConvert = false;
        private string _UploadType = string.Empty;
        private List<PDF_Model> PO_File_List = new List<PDF_Model>();
        private List<List<UpdateModel>> UpdateModel_List = new List<List<UpdateModel>>();
        private List<string> _wattingForConvert;
        private Dictionary<string, string> _File_Name_PDF;
        private List<string> _wattingForConvert_contentsOfZPL;

        /// <inheritdoc/>
        public P26_AssignPackingList(List<NewFormModel> newFormModels, DataTable p25Dt, string uploadType, List<string> wattingForConvert, Dictionary<string, string> file_Name_PDF, List<string> wattingForConvert_contentsOfZPL)
        {
            this.InitializeComponent();
            this._NewFormModels = newFormModels;
            this._P25Dt = p25Dt;
            this._UploadType = uploadType;
            this.GridDt.Columns.Add(new DataColumn() { ColumnName = "Sel", DataType = typeof(bool) });
            this.GridDt.Columns.Add(new DataColumn() { ColumnName = "FileName", DataType = typeof(string) });
            this.GridDt.Columns.Add(new DataColumn() { ColumnName = "PONO", DataType = typeof(string) });

            // this.GridDt.Columns.Add(new DataColumn() { ColumnName = "SKU", DataType = typeof(string) });
            // this.GridDt.Columns.Add(new DataColumn() { ColumnName = "Qty", DataType = typeof(string) });
            this.GridDt.Columns.Add(new DataColumn() { ColumnName = "PackingListID", DataType = typeof(string) });

            this._wattingForConvert = wattingForConvert;
            this._File_Name_PDF = file_Name_PDF;
            this._wattingForConvert_contentsOfZPL = wattingForConvert_contentsOfZPL;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            var fileNameList = this._NewFormModels.Select(o => o.FileName).Distinct().ToList();

            if (this._UploadType == "ZPL")
            {
                foreach (var fileName in fileNameList)
                {
                    foreach (var newFormModel in this._NewFormModels.Where(o => o.FileName == fileName))
                    {
                        string pono = newFormModel.ZPL_Content.Rows[0]["CustPONO"].ToString();
                        List<string> packinListIDs = newFormModel.PackingListIDs;

                        foreach (var packinListID in packinListIDs)
                        {
                            if (!this.GridDt.AsEnumerable().Where(o =>
                                o["FileName"].ToString() == fileName
                                && o["PONO"].ToString() == pono
                                && o["PackingListID"].ToString() == packinListID).Any())
                            {
                                DataRow dr = this.GridDt.NewRow();
                                dr["FileName"] = fileName;
                                dr["PONO"] = pono;

                                // dr["SKU"] = key.SKU;
                                // dr["Qty"] = key.Qty;
                                dr["PackingListID"] = packinListID;
                                this.GridDt.Rows.Add(dr);
                            }
                        }
                    }
                }

                this.listControlBindingSource1.DataSource = this.GridDt;

                this.grid2.IsEditingReadOnly = false;
                this.Helper.Controls.Grid.Generator(this.grid2)
                .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("FileName", header: "File Name ", width: Widths.AnsiChars(70), iseditingreadonly: true)
                .Text("PONO", header: "PO#", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Text("PackingListID", header: "PackingList#", width: Widths.AnsiChars(30), iseditingreadonly: false)
                ;
            }

            if (this._UploadType == "PDF")
            {
                List<PDF_Model> list = new List<PDF_Model>();
                foreach (var fileName in fileNameList)
                {
                    foreach (var newFormModel in this._NewFormModels.Where(o => o.FileName == fileName))
                    {
                        string pono = newFormModel.ZPL_Content.Rows[0]["CustPONO"].ToString();
                        List<string> packinListIDs = newFormModel.PackingListIDs;

                        foreach (var packinListID in packinListIDs)
                        {
                            if (!list.Where(o => o.PONO == pono && o.PackingListID == packinListID).Any())
                            {
                                PDF_Model m = new PDF_Model()
                                {
                                    PONO = pono,
                                    FileName = fileName,
                                    PackingListID = packinListID,
                                };
                                list.Add(m);
                            }

                            this.PO_File_List.Add(new PDF_Model()
                            {
                                PONO = pono,
                                FileName = fileName,
                            });
                        }
                    }
                }

                foreach (var model in list)
                {
                    DataRow dr = this.GridDt.NewRow();
                    dr["FileName"] = model.FileName;
                    dr["PONO"] = model.PONO;
                    dr["PackingListID"] = model.PackingListID;
                    this.GridDt.Rows.Add(dr);
                }

                this.listControlBindingSource1.DataSource = this.GridDt;

                this.grid2.IsEditingReadOnly = false;
                this.Helper.Controls.Grid.Generator(this.grid2)
                .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)

                // .Text("FileName", header: "File Name ", width: Widths.AnsiChars(70), iseditingreadonly: true)
                .Text("PONO", header: "PO#", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Text("PackingListID", header: "PackingList#", width: Widths.AnsiChars(30), iseditingreadonly: false)
                ;
            }
        }

        private void BtnProcessing_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;

            this.UpdateModel_List.Clear();

            List<DataRow> selectedRow = dt.AsEnumerable().Where(o => o["Sel"].ToString().ToUpper() == "TRUE").ToList();

            List<string> keys = dt.AsEnumerable().Select(o => o["PONO"].ToString()).Distinct().ToList();

            foreach (var pono in keys)
            {
                int selectCont = dt.AsEnumerable().Where(o => o["Sel"].ToString().ToUpper() == "TRUE" && o["PONO"].ToString().ToUpper() == pono.ToUpper()).Count();

                if (selectCont == 0)
                {
                    MyUtility.Msg.InfoBox("Please select row every PO#.");
                    return;
                }
            }

            Dictionary<string, string> file_PackID = new Dictionary<string, string>();

            if (this._UploadType == "ZPL")
            {
                foreach (DataRow dr in selectedRow)
                {
                    if (file_PackID.Keys.Contains(dr["FileName"].ToString()))
                    {
                        MyUtility.Msg.InfoBox("One File can only mapping one PackingList#");
                        return;
                    }

                    file_PackID.Add(dr["FileName"].ToString(), dr["PackingListID"].ToString());
                }
            }

            if (this._UploadType == "PDF")
            {
                foreach (DataRow dr in selectedRow)
                {
                    if (file_PackID.Keys.Contains(dr["PONO"].ToString()))
                    {
                        MyUtility.Msg.InfoBox("One File can only PO$# one PackingList#");
                        return;
                    }

                    file_PackID.Add(dr["PONO"].ToString(), dr["PackingListID"].ToString());
                }
            }

            P26 p26 = new P26(null);

            foreach (var item in file_PackID)
            {
                string fileName = item.Key;
                string pono = item.Key;
                string packingListID = item.Value;

                NewFormModel model = new NewFormModel();
                if (this._UploadType == "ZPL")
                {
                    model = this._NewFormModels.Where(o => o.FileName == fileName).FirstOrDefault();
                }

                if (this._UploadType == "PDF")
                {
                    foreach (var aModel in this._NewFormModels)
                    {
                        model.ZPL_List = aModel.ZPL_List.Where(o => o.CustPONo == pono).ToList();

                        // foreach (var bModel in aModel.ZPL_List)
                        // {
                        //    //bModel.CustPONo
                        // }
                    }
                }

                List<UpdateModel> updateModels = new List<UpdateModel>();

                if (this._UploadType == "ZPL")
                {
                    updateModels = p26.ZPL_Mapping(model.ZPL_List, fileName, selected_PackingListID: packingListID);
                }

                if (this._UploadType == "PDF")
                {
                    updateModels = p26.PDF_Mapping(model.ZPL_List, selected_PackingListID: packingListID);
                }

                this.UpdateModel_List.Add(updateModels);
            }

            // 先完成P24資料寫入、轉圖片、將圖片存入P24表身
            bool result = p26.P24_Database_Update(this.UpdateModel_List, this._UploadType, true, this._wattingForConvert, this._File_Name_PDF, this._wattingForConvert_contentsOfZPL);

            if (result)
            {
                // 修改PackingList_Detail
                result = p26.P03_Database_Update(this.UpdateModel_List, this._UploadType, true);
            }

            if (result)
            {
                this.canConvert = true;

                if (this._UploadType == "ZPL")
                {
                    foreach (var item in file_PackID)
                    {
                        string fileName = item.Key;

                        List<DataRow> dl = this._P25Dt.AsEnumerable().Where(o => o["FileName"].ToString() == fileName).ToList(); // .FirstOrDefault()["Result"] = "Pass";

                        foreach (DataRow dr in dl)
                        {
                            dr["Result"] = "Pass";
                        }
                    }
                }

                if (this._UploadType == "PDF")
                {
                    foreach (var item in this.PO_File_List)
                    {
                        string pono = item.PONO;
                        string fileName = item.FileName;

                        List<DataRow> dl = this._P25Dt.AsEnumerable().Where(o => o["FileName"].ToString() == fileName).ToList();

                        foreach (DataRow dr in dl)
                        {
                            dr["Result"] = "Pass";
                        }
                    }
                }
            }
            else
            {
                this.canConvert = false;

                foreach (var item in file_PackID)
                {
                    string fileName = item.Key;

                    List<DataRow> dl = this._P25Dt.AsEnumerable().Where(o => o["FileName"].ToString() == fileName).ToList(); // .FirstOrDefault()["Result"] = "Pass";

                    foreach (DataRow dr in dl)
                    {
                        dr["Result"] = "Fail";
                    }
                }
            }

            this.isClickProcessing = true;
            this.Close();
        }

        /// <inheritdoc/>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!this.isClickProcessing)
            {
                var list = this._NewFormModels.Select(o => o.FileName).Distinct().ToList();

                foreach (var fileName in list)
                {
                    List<DataRow> dl = this._P25Dt.AsEnumerable().Where(o => o["FileName"].ToString() == fileName).ToList(); // .FirstOrDefault()["Result"] = "Pass";

                    foreach (DataRow dr in dl)
                    {
                        dr["Result"] = "Fail";
                    }
                }
            }
        }

        private class PDF_Model
        {
            public string PONO { get; set; }

            public string FileName { get; set; }

            public string PackingListID { get; set; }
        }
    }
}
