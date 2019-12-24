﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Win.UI;
using Sci.Data;
using Ict.Win;
using Ict;
using Sci.Win.Tools;

namespace Sci.Production.Class
{
    public partial class txtCFALocation : Sci.Win.UI.TextBox
    {
        public txtCFALocation()
        {
            this.Size = new System.Drawing.Size(80, 23);
            this.IsSupportSytsemContextMenu = false;
        }

        private Control mDivisionObject;
        [Category("Custom Properties")]
        public Control MDivisionObjectName
        {
            set { this.mDivisionObject = value; }
            get { return this.mDivisionObject; }
        }
        private string m;
        [Category("Custom Properties")]
        public string M
        {
            set { this.m = value; }
            get { return this.m; }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {            
            base.OnPopUp(e);
            if (e.IsHandled) return;

            Sci.Win.Tems.Base myform = (Sci.Win.Tems.Base)this.FindForm();
            if (myform.EditMode)
            {
                string sql = "select ID,Description from CFALocation WITH (NOLOCK) order by ID";
                if (this.mDivisionObject != null && !string.IsNullOrWhiteSpace((string)this.mDivisionObject.Text))
                {
                    sql = string.Format("select ID,Description from CFALocation WITH (NOLOCK) where MDivisionID = '{0}' and junk=0 order by ID", this.mDivisionObject.Text);
                }

                if (!MyUtility.Check.Empty(this.M))
                {
                    sql = string.Format("select ID,Description from CFALocation WITH (NOLOCK) where MDivisionID = '{0}' and junk=0 order by ID", this.M);
                }
                DataTable tbCFALocation;
                DBProxy.Current.Select("Production", sql, out tbCFALocation);
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(tbCFALocation, "ID,Description", "10,40,10", this.Text, "ID,Description");

                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                this.Text = item.GetSelectedString();
                e.IsHandled = true;
            }
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string str = this.Text;
            if (!MyUtility.Check.Empty(str))
            {
                if (str != this.OldValue)
                {
                    if (MyUtility.Check.Seek(str, "CFALocation", "id") == false)
                    {
                        this.Text = "";
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< CFALocation : {0} > not found!!!", str));
                        return;
                    }
                }
                else
                {
                    if (this.mDivisionObject != null)
                    {
                        if (!string.IsNullOrWhiteSpace((string)this.mDivisionObject.Text))
                        {
                            string selectCommand = string.Format("select ID from CFALocation WITH (NOLOCK) where MDivisionID = '{0}' and ID = '{1}'", (string)this.mDivisionObject.Text, str);
                            if (!MyUtility.Check.Seek(selectCommand, null))
                            {
                                this.Text = "";
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox(string.Format("< CFALocation : {0} > not found!!!", str));
                                return;
                            }
                        }
                    }
                    if (!MyUtility.Check.Empty(this.M))
                    {
                        if (!string.IsNullOrWhiteSpace((string)this.mDivisionObject.Text))
                        {
                            string selectCommand = string.Format("select ID from CFALocation WITH (NOLOCK) where MDivisionID = '{0}' and ID = '{1}'", this.M, str);
                            if (!MyUtility.Check.Seek(selectCommand, null))
                            {
                                this.Text = "";
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox(string.Format("< CFALocation : {0} > not found!!!", str));
                                return;
                            }
                        }
                    }
                }
            }
        }
        
        public class CellCFALocation : DataGridViewGeneratorTextColumnSettings
        {
            public static DataGridViewGeneratorTextColumnSettings GetGridCell(string mdivisionID)
            {
                //pur 為ture 表示需判斷PurchaseFrom
                CellCFALocation ts = new CellCFALocation();
                ts.EditingMouseDown += (s, e) =>
                {
                    // 右鍵彈出功能
                    if (e.Button == MouseButtons.Right)
                    {
                        DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                        // Parent form 若是非編輯狀態就 return 
                        if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode) { return; }
                        DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                        DataTable tbCFALocation;
                        string sql = $@"select ID,Description from CFALocation WITH (NOLOCK) where MDivisionID = '{mdivisionID.ToString().Trim()}' and junk=0 order by ID ";
                        DBProxy.Current.Select("Production", sql, out tbCFALocation);
                        Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(tbCFALocation, "ID,Description", "10,40", row["CFALocationID"].ToString(), "ID,Description,M");
                        DialogResult result = item.ShowDialog();
                        if (result == DialogResult.Cancel) { return; }
                        var sellist = item.GetSelecteds();
                        e.EditingControl.Text = item.GetSelectedString();
                    }
                };
                // 正確性檢查
                ts.CellValidating += (s, e) =>
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                    // Parent form 若是非編輯狀態就 return 
                    if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode) { return; }
                    // 右鍵彈出功能
                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    String oldValue = row["CFALocationID"].ToString();
                    String newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow
                    string sql;

                    sql = $@"select ID from CFALocation WITH (NOLOCK) where MDivisionID = '{mdivisionID.ToString().Trim()}' and ID = '{newValue}' and junk=0 ";
                    if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                    {
                        if (!MyUtility.Check.Seek(sql))
                        {
                            row["CFALocationID"] = "";
                            row.EndEdit();
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox($"< CFALocation : {newValue}> not found.");
                            return;
                        }
                    }
                };
                return ts;
            }

        }
    }
}
