using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class P01_IrregularPriceReason : Sci.Win.Subs.Base
    {
        DataTable _IrregularPriceReasonDT;
        DataTable OriginDT_FromDB;
        DataTable ModifyDT_FromP01;
        int _Type = 0;
        string _ArtWorkPO_ID = string.Empty;

        //「未曾有過」價格異常紀錄，現在價格異常 Type = 1
        //「曾經」有過價格異常紀錄，現在還是異常 Type = 2
        //「曾經」有過價格異常紀錄，現在價格正常 Type = 3
        public P01_IrregularPriceReason(DataTable IrregularPriceReasonDT, string ArtWorkPO_ID, int Type = 0)
        {
            InitializeComponent();

            this.EditMode = false;
            _IrregularPriceReasonDT = IrregularPriceReasonDT;
            _ArtWorkPO_ID = ArtWorkPO_ID;
            _Type = Type;
        }

        protected override void OnFormLoaded()
        {
            if (!MyUtility.Check.Empty(_IrregularPriceReasonDT) && _Type == 1)
            {  //Type=1和 Type 2
                //_IrregularPriceReasonDT 不為空，表示現在有「新的」價格異常，新的意思即是DB沒有這筆紀錄
                ModifyDT_FromP01 = _IrregularPriceReasonDT.Copy();
                listControlBindingSource1.DataSource = _IrregularPriceReasonDT;
                btnEdit.Enabled = true;
            }
            else
            {
                //DB有資料就呈現出來
                switch (_Type)
                {
                    case 3://Type=3
                        IrregularPriceReasonDT_Initial(true);
                        break;
                    case 2://Type=2
                        IrregularPriceReasonDT_Initial(true);
                        break;
                    default:
                        btnEdit.Enabled = false;
                        break;
                }
            }

            this.gridgridIrregularPrice.DataSource = listControlBindingSource1;

            DataGridViewGeneratorTextColumnSettings col_SubconReasonID = new DataGridViewGeneratorTextColumnSettings();

            col_SubconReasonID.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode) return;//非編輯模式 
                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.gridgridIrregularPrice.GetDataRow<DataRow>(e.RowIndex);
                    string sqlCmd = $@"SELECT ID,[ResponsibleID]=Responsible,(select Name from DropDownList d where d.type = 'IrregularPriceResp' and d.ID = SubconReason.Responsible) as ResponsibleName,Reason 
                                        FROM SubconReason WHERE Type = 'IP' AND Junk = 0";
                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "10,20,20", string.Empty, "ID,Responsible,Reason");
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }
                    else
                    {
                        dr["SubconReasonID"] = item.GetSelectedString();

                        DataTable dt;
                        DBProxy.Current.Select(null, $"SELECT ID,[ResponsibleID]=Responsible,(select Name from DropDownList d where d.type = 'IrregularPriceResp' and d.ID = SubconReason.Responsible) as ResponsibleName,Reason FROM SubconReason WHERE ID='{item.GetSelectedString()}' AND Type='IP' AND Junk=0", out dt);
                        dr["ResponsibleID"] = dt.Rows[0]["ResponsibleID"];
                        dr["ResponsibleName"] = dt.Rows[0]["ResponsibleName"];
                        dr["Reason"] = dt.Rows[0]["Reason"];
                    }
                }
            };

            col_SubconReasonID.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;//非編輯模式 
                if (e.RowIndex == -1) return; //沒東西 return
                DataRow dr = gridgridIrregularPrice.GetDataRow(e.RowIndex);
                string ori_SubconReasonID = dr["SubconReasonID"].ToString();
                DataTable dt;
                DBProxy.Current.Select(null, $"SELECT ID,[ResponsibleID]=Responsible,(select Name from DropDownList d where d.type = 'IrregularPriceResp' and d.ID = SubconReason.Responsible) as ResponsibleName,Reason FROM SubconReason WHERE ID='{e.FormattedValue}' AND Type='IP' AND Junk=0", out dt);

                if (dt.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("No Data!!");
                    dr["SubconReasonID"] = ori_SubconReasonID;
                    dr["ResponsibleID"] = dr["ResponsibleID"];
                    dr["ResponsibleName"] = dr["ResponsibleName"];
                    dr["Reason"] = dr["Reason"];
                }
                else
                {
                    dr["SubconReasonID"] = dt.Rows[0]["ID"];
                    dr["ResponsibleID"] = dt.Rows[0]["ResponsibleID"];
                    dr["ResponsibleName"] = dt.Rows[0]["ResponsibleName"];
                    dr["Reason"] = dt.Rows[0]["Reason"];
                }

            };

            #region Grid欄位設定
            
            Helper.Controls.Grid.Generator(this.gridgridIrregularPrice)
                .Text("Factory", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("Type", header: "Type", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("POID", header: "POID", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("StyleID", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("BrandID", header: "Brand", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Numeric("PoPrice", header: "PO" + Environment.NewLine + "Price", decimal_places: 4, iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Numeric("StdPrice", header: "Standard" + Environment.NewLine + "Price", decimal_places: 4, iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("SubconReasonID", header: "Reason" + Environment.NewLine + "ID", width: Widths.AnsiChars(7), settings: col_SubconReasonID)
                .Text("ResponsibleID", header: "ResponsibleID", iseditingreadonly: true, width: null)
                .Text("ResponsibleName", header: "Responsible", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("Reason", header: "Reason", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("AddDate", header: "Create" + Environment.NewLine + "Date", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("AddName", header: "Create" + Environment.NewLine + "Name", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("EditDate", header: "Edit" + Environment.NewLine + "Date", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("EditName", header: "Edit" + Environment.NewLine + "Name", iseditingreadonly: true, width: Widths.AnsiChars(10));
            #endregion

            this.gridgridIrregularPrice.Columns["SubconReasonID"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridgridIrregularPrice.Columns["ResponsibleID"].Visible = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            if (this.EditMode)
            {
                gridgridIrregularPrice.IsEditingReadOnly = true;

                StringBuilder sql = new StringBuilder();
                DataTable ModifyTable = (DataTable)listControlBindingSource1.DataSource;
                //ModifyTable 去掉 OriginDT_FromDB，剩下的不是新增就是修改
                var Insert_Or_Update = ModifyTable.AsEnumerable().Except(OriginDT_FromDB.AsEnumerable(), DataRowComparer.Default);

                //抓出ReasonID為空的出來刪除
                var Delete = ModifyTable.AsEnumerable().Except(OriginDT_FromDB.AsEnumerable(), DataRowComparer.Default).Where(o => o.Field<string>("SubconReasonID").Trim() == "");

                #region SQL組合
                foreach (var item in Delete)
                {
                    string POID = item.Field<string>("POID");
                    string ArtworkType = item.Field<string>("Type");
                    sql.Append($"DELETE FROM [ArtworkPO_IrregularPrice] WHERE POID='{POID}' AND ArtworkTypeID='{ArtworkType}'" + Environment.NewLine);
                    sql.Append(" " + Environment.NewLine);
                }

                foreach (var item in Insert_Or_Update)
                {
                    string POID = item.Field<string>("POID");
                    string ArtworkType = item.Field<string>("Type");
                    string SubconReasonID = item.Field<string>("SubconReasonID");
                    decimal POPrice = item.Field<decimal>("POPrice");
                    decimal StandardPrice = item.Field<decimal>("StdPrice");
                    DataTable dt;
                    DualResult result = DBProxy.Current.Select(null, $"SELECT * FROM ArtworkPO_IrregularPrice WHERE POID='{POID}' AND ArtworkTypeID='{ArtworkType}'", out dt);
                    if (result)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["SubconReasonID"].ToString() != SubconReasonID && !string.IsNullOrEmpty(SubconReasonID))
                            {
                                sql.Append($"UPDATE [ArtworkPO_IrregularPrice] SET [SubconReasonID]='{SubconReasonID}',EditDate=GETDATE(),EditName='{Sci.Env.User.UserID}'" + Environment.NewLine);
                                sql.Append($"                                  WHERE [POID]='{POID}' AND [ArtworkTypeID]='{ArtworkType}'" + Environment.NewLine);
                            }
                        }
                        else
                        {
                            sql.Append("INSERT INTO [ArtworkPO_IrregularPrice]([POID],[ArtworkTypeID],[POPrice],[StandardPrice],[SubconReasonID],[AddDate],[AddName])" + Environment.NewLine);
                            sql.Append($"                              VALUES ('{POID}','{ArtworkType}',{POPrice},{StandardPrice},'{SubconReasonID}',GETDATE(),'{Sci.Env.User.UserID}')" + Environment.NewLine);
                        }
                    }
                    sql.Append(" " + Environment.NewLine);
                }
                #endregion
                if (!MyUtility.Check.Empty(sql.ToString()))
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        DualResult upResult;
                        try
                        {
                            upResult = DBProxy.Current.Execute(null, sql.ToString());
                            if (!upResult)
                            {
                                ShowErr(sql.ToString(), upResult);
                                return;
                            }

                            scope.Complete();
                            scope.Dispose();
                            IrregularPriceReasonDT_Initial(true);
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            ShowErr("Commit transaction error.", ex);
                        }
                    }
                }
            }
            else
            {
                gridgridIrregularPrice.IsEditingReadOnly = false;
                IrregularPriceReasonDT_Initial();
            }

            this.EditMode = !this.EditMode;
            btnEdit.Text = this.EditMode ? "Save" : "Edit";
            btnClose.Text = this.EditMode ? "Undo" : "Close";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "Close")
                Close();
            else
            {
                this.EditMode = !this.EditMode;
                btnEdit.Text = "Edit";
                btnClose.Text = "Close";
                gridgridIrregularPrice.IsEditingReadOnly = false;
                IrregularPriceReasonDT_Initial();
            }
        }

        private void IrregularPriceReasonDT_Initial(bool isSaveAct = false)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            StringBuilder sql = new StringBuilder();
            DualResult result;


            sql.Append(" " + Environment.NewLine);
            sql.Append(" SELECT DISTINCT [Factory]=a.FactoryId" + Environment.NewLine);
            sql.Append(" ,[POID]=al.POID" + Environment.NewLine);
            sql.Append(" ,[Type]=a.ArtworkTypeID" + Environment.NewLine);
            sql.Append(" ,o.StyleID" + Environment.NewLine);
            sql.Append(" ,o.BrandID" + Environment.NewLine);
            sql.Append(" ,[PoPrice]=al.POPrice" + Environment.NewLine);
            sql.Append(" ,[StdPrice]=al.StandardPrice" + Environment.NewLine);
            sql.Append(" ,al.SubconReasonID" + Environment.NewLine);
            sql.Append(" ,[ResponsibleID]=sr.Responsible" + Environment.NewLine);
            sql.Append(" ,[ResponsibleName]=(select Name from DropDownList d where d.type = 'IrregularPriceResp' and d.ID = sr.Responsible)" + Environment.NewLine);
            sql.Append(" ,sr.Reason" + Environment.NewLine);
            sql.Append(" ,al.AddDate" + Environment.NewLine);
            sql.Append(" ,al.AddName" + Environment.NewLine);
            sql.Append(" ,al.EditDate" + Environment.NewLine);
            sql.Append(" ,al.EditName" + Environment.NewLine);
            sql.Append(" FROM ArtWorkPO a" + Environment.NewLine);
            sql.Append(" INNER JOIN ArtworkPO_Detail ad ON a.ID = ad.ID" + Environment.NewLine);
            sql.Append(" INNER JOIN Orders o ON ad.OrderID = o.ID" + Environment.NewLine);
            sql.Append(" INNER JOIN ArtworkPO_IrregularPrice al ON al.POID = o.POID AND al.ArtworkTypeID = ad.ArtworkTypeID" + Environment.NewLine);
            sql.Append(" LEFT JOIN SubconReason sr ON sr.Type = 'IP' AND sr.ID = al.SubconReasonID" + Environment.NewLine);
            sql.Append(" WHERE a.ID = @artWorkPO_ID" + Environment.NewLine);

            parameters.Add(new SqlParameter("@artWorkPO_ID", _ArtWorkPO_ID));

            result = DBProxy.Current.Select(null, sql.ToString(), parameters, out OriginDT_FromDB);
            if (!result)
            {
                ShowErr(sql.ToString(), result);
                return;
            }

            if (isSaveAct)
            {
                ModifyDT_FromP01 = OriginDT_FromDB.Copy();
                P01._IrregularPriceReasonDT = OriginDT_FromDB.Copy();
            }

            //原始資料：OriginDT_FromDB
            //即將異動，寫回DB的資料：ModifyDT_FromP01
            listControlBindingSource1.DataSource = ModifyDT_FromP01.Copy();

        }


    }
}
