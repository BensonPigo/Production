using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Win.Tems;
using Sci.Data;
using System.Transactions;

namespace Sci.Production.SubProcess
{
    /// <summary>
    /// SubProcess.B02
    /// </summary>
    public partial class B02 : Sci.Win.Tems.Input6
    {
        private string user = Sci.Env.User.UserID;

        /// <summary>
        /// B02
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// OnDetailGridSetup
        /// </summary>
        protected override void OnDetailGridSetup()
        {
            Ict.Win.DataGridViewGeneratorTextColumnSettings gridType = new DataGridViewGeneratorTextColumnSettings();
            Ict.Win.DataGridViewGeneratorTextColumnSettings gridFeature = new DataGridViewGeneratorTextColumnSettings();
            Ict.Win.DataGridViewGeneratorTextColumnSettings gridRemark = new DataGridViewGeneratorTextColumnSettings();
            Ict.Win.DataGridViewGeneratorNumericColumnSettings gridSMV = new DataGridViewGeneratorNumericColumnSettings();

            #region gridType
            gridType.EditingMouseDown += (s, e) =>
             {
                 if (this.EditMode && e.Button == MouseButtons.Right)
                 {
                    Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(@"select  Id,ArtworkTypeId  from  subprocess  where isselection=1 and Junk=0  order by Id", "10,20", null);
                     DialogResult result = item.ShowDialog();
                     if (result == DialogResult.Cancel)
                     {
                         return;
                     }

                     var x = item.GetSelecteds();
                     this.CurrentDetailData["Type"] = x[0]["ID"].ToString();
                 }
             };

            gridType.CellValidating += (s, e) =>
             {
                 DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);

                 // Value變更過,要填入EditName and EditDate
                 this.Edited(dr, "Type");
                 if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                 {
                     DataRow find;
                     if (!MyUtility.Check.Seek(
                         string.Format(
                             @"Select  Id  from  subprocess  where isselection=1 and Junk=0 and id='{0}'",
                         e.FormattedValue),
                         out find))
                     {
                         MyUtility.Msg.WarningBox(string.Format(@"<Type: {0}> not found!", e.FormattedValue));
                         this.CurrentDetailData["Type"] = string.Empty;
                         return;
                     }

                     this.CurrentDetailData["Type"] = e.FormattedValue;
                 }
             };
            #endregion

            #region gridFeature
            gridFeature.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(string.Format(@"Select  Feature,Remark  from  SubProcessFeature where Type='{0}' and Junk=0  order by Feature", this.CurrentDetailData["Type"].ToString()), "30,20", null);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    var x = item.GetSelecteds();
                    this.CurrentDetailData["Feature"] = x[0]["Feature"].ToString();
                }
            };

            gridFeature.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);

                // Value變更過,要填入EditName and EditDate
                this.Edited(dr, "Feature");
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    DataRow find;
                    if (!MyUtility.Check.Seek(
                        string.Format(
                            @"Select  Feature  from  SubProcessFeature where Type='{0}' and Junk=0 and Feature='{1}'",
                        this.CurrentDetailData["Type"],
                        e.FormattedValue),
                        out find))
                    {
                        MyUtility.Msg.WarningBox(string.Format(@"<Feature: {0}> not found!", e.FormattedValue));
                        this.CurrentDetailData["Feature"] = string.Empty;
                        return;
                    }

                    this.CurrentDetailData["Feature"] = e.FormattedValue;
                }
            };
            #endregion

            gridSMV.CellValidating += (s, e) =>
             {
                 DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);

                 // Value變更過,要填入EditName and EditDate
                 this.Edited(dr, "SMV");
             };

            gridRemark.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);

                // Value變更過,要填入EditName and EditDate
                this.Edited(dr, "Remark");
            };

            #region 表身欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Type", header: "Type", width: Widths.AnsiChars(10),  settings: gridType)
            .Text("Feature", header: "Feature", width: Widths.AnsiChars(30),  settings: gridFeature)
            .Numeric("SMV", header: "SMV", width: Widths.AnsiChars(8), integer_places: 7,  decimal_places: 4,maximum: 999, settings: gridSMV)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(20), settings: gridRemark)
            .Text("AddName", header: "AddName", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .DateTime("AddDate", header: "AddDate", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("EditName", header: "EditName", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .DateTime("EditDate", header: "EditDate", width: Widths.AnsiChars(20), iseditingreadonly: true)
            ;
            #endregion
        }

        /// <summary>
        /// OnDetailGridInsert 新增表身時,設定預設值
        /// </summary>
        /// <param name="index">index</param>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            this.CurrentDetailData["AddName"] = this.user;
            this.CurrentDetailData["AddDate"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
        }

        /// <summary>
        /// ClickSaveBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            if (this.DetailDatas.Count < 1)
            {
                MyUtility.Msg.WarningBox("detail cannot be empty!");
                return false;
            }

            foreach (DataRow row in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(row["Type"]) || MyUtility.Check.Empty(row["Feature"]) || MyUtility.Check.Empty(row["SMV"]))
                {
                    MyUtility.Msg.WarningBox("Type,Feature,SMV cannot be empty!");
                    return false;
                }

                if (MyUtility.Convert.GetDecimal(row["SMV"]) >= 1000)
                {
                    MyUtility.Msg.WarningBox(string.Format(@"<SMV: {0}> cannot more than 999.9999!", row["SMV"]));
                    return false;
                }
            }

            return base.ClickSaveBefore();
        }

        /// <summary>
        /// ClickSave
        /// </summary>
        /// <returns>DualResult</returns>
        protected override DualResult ClickSave()
        {
            // 修改表身資料,不寫入表頭EditName and EditDate
            ITableSchema pass1Schema;
            var ok = DBProxy.Current.GetTableSchema(null, "Style", out pass1Schema);
            pass1Schema.IsSupportEditDate = false;
            pass1Schema.IsSupportEditName = false;

            return Result.True;
        }

        /// <summary>
        /// 表身資料修改,填入EditName,EditDate
        /// </summary>
        /// <param name="dr">currentDetailData</param>
        /// <param name="key">判斷是修改哪個欄位</param>
        private void Edited(DataRow dr, string key)
        {
            switch (key)
            {
                case "Type":
                    string oldValue_type = this.CurrentDetailData["Type"].ToString();
                    string newValue_type = dr["Type"].ToString();
                    if (!MyUtility.Check.Empty(newValue_type) && oldValue_type != newValue_type)
                    {
                        this.CurrentDetailData["EditName"] = this.user;
                        this.CurrentDetailData["EditDate"] = DateTime.Now;
                    }

                    break;

                case "Feature":
                    string oldValue_F = this.CurrentDetailData["Feature"].ToString();
                    string newValue_F = dr["Feature"].ToString();
                    if (!MyUtility.Check.Empty(newValue_F) && oldValue_F != newValue_F)
                    {
                        this.CurrentDetailData["EditName"] = this.user;
                        this.CurrentDetailData["EditDate"] = DateTime.Now;
                    }

                    break;

                case "SMV":
                    string oldValue_S = this.CurrentDetailData["SMV"].ToString();
                    string newValue_S = dr["SMV"].ToString();
                    if (!MyUtility.Check.Empty(newValue_S) && oldValue_S != newValue_S)
                    {
                        this.CurrentDetailData["EditName"] = this.user;
                        this.CurrentDetailData["EditDate"] = DateTime.Now;
                    }

                    break;

                case "Remark":
                    string oldValue_R = this.CurrentDetailData["Remark"].ToString();
                    string newValue_R = dr["Remark"].ToString();
                    if (!MyUtility.Check.Empty(newValue_R) && oldValue_R != newValue_R)
                    {
                        this.CurrentDetailData["EditName"] = this.user;
                        this.CurrentDetailData["EditDate"] = DateTime.Now;
                    }

                    break;
            }
        }
    }
}
