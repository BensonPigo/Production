using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Transactions;

namespace Sci.Production.SubProcess
{
    /// <summary>
    /// SubProcess.B02
    /// </summary>
    public partial class B02 : Win.Tems.Input6
    {
        private string user = Env.User.UserID;
        private DataTable dtSMV = new DataTable();
        private int newUkey = 0;

        /// <summary>
        /// B02
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.dtSMV.Columns.Add("StyleFeatureUkey", typeof(int));
            this.dtSMV.Columns.Add("Seq", typeof(string));
            this.dtSMV.Columns.Add("OperationID", typeof(string));
            this.dtSMV.Columns.Add("Annotation", typeof(string));
            this.dtSMV.Columns.Add("MachineTypeID", typeof(string));
            this.dtSMV.Columns.Add("Mold", typeof(string));
            this.dtSMV.Columns.Add("IETMSSMV", typeof(string));
            this.dtSMV.Columns.Add("NewUkey", typeof(int));
        }

        /// <summary>
        /// OnDetailSelectCommandPrepare
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ukey"]);
            this.DetailSelectCommand = string.Format(
                @"
select *,[NewUkey]=0 from style_feature
where styleUkey = '{0}'",
                masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <summary>
        /// OnDetailGridSetup
        /// </summary>
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings gridType = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings gridFeature = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings gridRemark = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings gridSMV = new DataGridViewGeneratorNumericColumnSettings();

            #region gridType
            gridType.EditingMouseDown += (s, e) =>
             {
                 if (this.EditMode && e.Button == MouseButtons.Right)
                 {
                     Win.Tools.SelectItem item = new Win.Tools.SelectItem(@"select  Id,ArtworkTypeId  from  subprocess  where isselection=1 and Junk=0  order by Id", "10,20", null);
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
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(string.Format(@"Select  Feature,Remark  from  SubProcessFeature where Type='{0}' and Junk=0  order by Feature", this.CurrentDetailData["Type"].ToString()), "30,20", null);
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

            gridSMV.EditingMouseDown += (s, e) =>
            {
                int featueUkey = MyUtility.Check.Empty(MyUtility.Convert.GetInt(this.CurrentDetailData["ukey"])) ? MyUtility.Convert.GetInt(this.CurrentDetailData["NewUkey"]) : MyUtility.Convert.GetInt(this.CurrentDetailData["ukey"]);
                string strupdeSmv = string.Empty;
                if (!this.EditMode)
                {
                    if (!MyUtility.Check.Empty(featueUkey))
                    {
                        string sqlcmd = $@"
Select  Seq, OperationID, O.DescEN, SFS.Annotation, SFS.MachineTypeID, Mold, IETMSSMV
From  Style_Feature_SMV SFS
Left join Operation o on o.id=SFS.OperationID
Where StyleFeatureUkey={featueUkey}
Order by Seq
";
                        Win.Tools.SelectItem item1 = new Win.Tools.SelectItem(sqlcmd, "6,15,30,10,10,10,10", null, "Seq,Operation code,Operation Description,Annotation,M/C,Attachment,Std. SMV", columndecimals: "0,0,0,0,0,0,4");
                        item1.Width = 1000;
                        DialogResult returnResult = item1.ShowDialog();
                        if (returnResult == DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                }

                if (e.Button == MouseButtons.Right)
                {
                    string brand = MyUtility.Convert.GetString(this.CurrentMaintain["brandid"]);
                    string style = MyUtility.Convert.GetString(this.CurrentMaintain["id"]);
                    string season = MyUtility.Convert.GetString(this.CurrentMaintain["seasonid"]);
                    string sqlcomb = $@"select COMBOTYPE,id from timestudy where brandid='{brand}'and styleid= '{style}' and seasonid = '{season}'";

                    DataTable comb;
                    DualResult result = DBProxy.Current.Select(null, sqlcomb, out comb);
                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }

                    if (comb.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("Datas not found!");
                    }
                    else if (comb.Rows.Count == 1)
                    {
                        string id = MyUtility.Convert.GetString(comb.Rows[0]["id"]);
                        string sqlcmd = $@"
select 
      td.[SEQ]
	  ,td.[OperationID]
      ,OperationDescEN = o.DescEN
      ,td.[Annotation]
      ,td.[MachineTypeID]
      ,td.[Mold]
      ,td.[IETMSSMV]
from TimeStudy_Detail td WITH (NOLOCK) 
left join Operation o WITH (NOLOCK) on td.OperationID = o.ID
where td.ID = '{id}'
order by td.Seq ";

                        Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sqlcmd, "Seq,Operation code,Operation Description,Annotation,M/C,Attachment,Std. SMV", string.Empty, string.Empty, columndecimals: "0,0,0,0,0,0,4", defaultValueColumn: "IETMSSMV");
                        DialogResult dresult = item.ShowDialog();
                        if (dresult == DialogResult.Cancel)
                        {
                            return;
                        }

                        var smvs = item.GetSelectedList();
                        decimal smv = 0;
                        foreach (string ss in smvs)
                        {
                            smv += MyUtility.Convert.GetDecimal(ss);
                        }

                        if (MyUtility.Check.Seek($@"select 1 from Style_Feature_SMV where StyleFeatureUkey={featueUkey}"))
                        {
                            strupdeSmv = strupdeSmv + $@"delete from Style_Feature_SMV where StyleFeatureUkey={featueUkey} 
                        ";
                        }

                        DataRow[] drCheck = this.dtSMV.Select($@"StyleFeatureUkey={featueUkey}");
                        if (drCheck.Length > 0)
                        {
                            foreach (DataRow dr1 in drCheck)
                            {
                                dr1.Delete();
                            }
                        }

                        for (int i = 0; i < item.GetSelectedList().Count; i++)
                        {
                            DataRow dr = this.dtSMV.NewRow();
                            dr["StyleFeatureUkey"] = featueUkey;
                            dr["seq"] = item.GetSelecteds()[i]["SEQ"].ToString();
                            dr["OperationID"] = item.GetSelecteds()[i]["OperationID"].ToString();
                            dr["Annotation"] = item.GetSelecteds()[i]["Annotation"].ToString();
                            dr["MachineTypeID"] = item.GetSelecteds()[i]["MachineTypeID"].ToString();
                            dr["Mold"] = item.GetSelecteds()[i]["Mold"].ToString();
                            dr["IETMSSMV"] = item.GetSelecteds()[i]["IETMSSMV"].ToString();
                            dr["NewUkey"] = this.CurrentDetailData["NewUkey"];
                            this.dtSMV.Rows.Add(dr);
                        }

                        this.CurrentDetailData["smv"] = smv;
                        this.CurrentDetailData.EndEdit();
                    }
                    else
                    {
                        Win.Tools.SelectItem item1 = new Win.Tools.SelectItem($@"select COMBOTYPE from timestudy where brandid='{brand}'and styleid= '{style}' and seasonid = '{season}'", null, null);
                        DialogResult returnResult = item1.ShowDialog();
                        if (returnResult == DialogResult.Cancel)
                        {
                            return;
                        }

                        string combo = item1.GetSelectedString();
                        string sqlcmd = $@"
select 
      td.[SEQ]
	  ,td.[OperationID]
      ,OperationDescEN = o.DescEN
      ,td.[Annotation]
      ,td.[MachineTypeID]
      ,td.[Mold]
      ,td.[IETMSSMV]
from TimeStudy_Detail td WITH (NOLOCK) 
inner join TimeStudy t WITH (NOLOCK) on t.id = td.id
left join Operation o WITH (NOLOCK) on td.OperationID = o.ID
where COMBOTYPE = '{combo}' and t.brandid='{brand}'and t.styleid= '{style}' and t.seasonid = '{season}'
order by td.Seq";

                        Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sqlcmd, "Seq,Operation code,Operation Description,Annotation,M/C,Attachment,Std. SMV", string.Empty, string.Empty, columndecimals: "0,0,0,0,0,0,4", defaultValueColumn: "IETMSSMV");
                        DialogResult dresult = item.ShowDialog();
                        if (dresult == DialogResult.Cancel)
                        {
                            return;
                        }

                        var smvs = item.GetSelectedList();
                        decimal smv = 0;
                        foreach (string ss in smvs)
                        {
                            smv += MyUtility.Convert.GetDecimal(ss);
                        }

                        if (MyUtility.Check.Seek($@"select 1 from Style_Feature_SMV where StyleFeatureUkey={featueUkey} "))
                        {
                            strupdeSmv = strupdeSmv + $@"delete from Style_Feature_SMV where StyleFeatureUkey={featueUkey} ";
                        }

                        DataRow[] drCheck = this.dtSMV.Select($@"StyleFeatureUkey={featueUkey}");
                        if (drCheck.Length > 0)
                        {
                            foreach (DataRow dr1 in drCheck)
                            {
                                dr1.Delete();
                            }
                        }

                        for (int i = 0; i < item.GetSelectedList().Count; i++)
                        {
                            DataRow dr = this.dtSMV.NewRow();
                            dr["StyleFeatureUkey"] = featueUkey;
                            dr["seq"] = item.GetSelecteds()[i]["SEQ"].ToString();
                            dr["OperationID"] = item.GetSelecteds()[i]["OperationID"].ToString();
                            dr["Annotation"] = item.GetSelecteds()[i]["Annotation"].ToString();
                            dr["MachineTypeID"] = item.GetSelecteds()[i]["MachineTypeID"].ToString();
                            dr["Mold"] = item.GetSelecteds()[i]["Mold"].ToString();
                            dr["IETMSSMV"] = item.GetSelecteds()[i]["IETMSSMV"].ToString();
                            dr["NewUkey"] = this.CurrentDetailData["NewUkey"];
                            this.dtSMV.Rows.Add(dr);
                        }

                        this.CurrentDetailData["smv"] = smv;
                        this.CurrentDetailData.EndEdit();
                    }

                    if (strupdeSmv.Length > 0)
                    {
                        TransactionScope transactionscope = new TransactionScope();
                        using (transactionscope)
                        {
                            DualResult dualResult = DBProxy.Current.Execute(null, strupdeSmv);

                            if (dualResult == false)
                            {
                                transactionscope.Dispose();
                                MyUtility.Msg.WarningBox(dualResult.ToString());
                                return;
                            }

                            transactionscope.Complete();
                            transactionscope.Dispose();
                        }
                    }
                }
            };
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
            .Numeric("SMV", header: "SMV", width: Widths.AnsiChars(8), integer_places: 7,  decimal_places: 4, maximum: 999, settings: gridSMV)
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
            int styleUkey = MyUtility.Convert.GetInt(this.CurrentMaintain["ukey"]);
            this.CurrentDetailData["AddName"] = this.user;
            this.CurrentDetailData["AddDate"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
            this.CurrentDetailData["NewUkey"] = this.newUkey + 1;
        }

        /// <summary>
        /// ClickSaveBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
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
        /// 跳過底層,手寫存表身功能
        /// </summary>
        /// <param name="details">details</param>
        /// <param name="detailtableschema">detailtableschema</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnSaveDetail(IList<DataRow> details, ITableSchema detailtableschema)
        {
            DataTable dtDB;
            DataTable dtDetail = (DataTable)this.detailgridbs.DataSource;
            dtDetail.AcceptChanges();
            string sqlcmd = "declare @ukey bigint ;";
            DualResult result;
            #region 刪除

            // 表身沒有,但DB有資料就刪除
            if (result = DBProxy.Current.Select(null, $@"select * from Style_Feature where styleUkey={this.CurrentMaintain["ukey"]}", out dtDB))
            {
                foreach (DataRow drd in dtDB.Rows)
                {
                    for (int d = 0; d < details.Count; d++)
                    {
                        DataRow[] drdelt = dtDetail.Select($"ukey={drd["ukey"]}");
                        if (drdelt.Length < 1)
                        {
                            sqlcmd = sqlcmd + $@"
delete from Style_Feature where ukey={drd["ukey"]}
delete from Style_Feature_SMV where StyleFeatureUkey={drd["ukey"]}
";
                        }
                    }
                }
            }
            #endregion

            #region 新增,修改
            for (int i = 0; i < dtDetail.Rows.Count; i++)
            {
                if (dtDetail.Rows.Count > 0)
                {
                    // NewUkey!=0,代表該資料不存在SQLDB,Insert
                    if ((int)dtDetail.Rows[i]["NewUkey"] != 0)
                    {
                        sqlcmd = sqlcmd + $@"
insert into Style_Feature (StyleUkey,Type,Feature,SMV,Remark,Addname,AddDate)
values ({dtDetail.Rows[i]["StyleUkey"]},'{dtDetail.Rows[i]["Type"]}','{dtDetail.Rows[i]["Feature"]}','{dtDetail.Rows[i]["SMV"]}','{dtDetail.Rows[i]["Remark"]}','{dtDetail.Rows[i]["AddName"]}',convert(varchar, getdate(), 121))
set @ukey  = (select @@identity);
";
                        for (int ii = 0; ii < this.dtSMV.Rows.Count; ii++)
                        {
                            if (MyUtility.Convert.GetInt(this.dtSMV.Rows[ii]["NewUkey"]) == MyUtility.Convert.GetInt(dtDetail.Rows[i]["NewUkey"]))
                            {
                                sqlcmd = sqlcmd + $@"
insert into Style_Feature_SMV(StyleFeatureUkey,Seq,OperationID,Annotation,MachineTypeID,Mold,IETMSSMV)
values(@ukey,'{this.dtSMV.Rows[ii]["Seq"]}','{this.dtSMV.Rows[ii]["OperationID"]}','{this.dtSMV.Rows[ii]["Annotation"]}','{this.dtSMV.Rows[ii]["MachineTypeID"]}','{this.dtSMV.Rows[ii]["Mold"]}','{this.dtSMV.Rows[ii]["IETMSSMV"]}')
";
                            }
                        }
                    }
                    else
                    {
                        // NewUkey=0,代表該資料已存在SQLDB,Update
                        sqlcmd = sqlcmd + $@"
update Style_Feature
set Type= '{dtDetail.Rows[i]["Type"]}',
Feature= '{dtDetail.Rows[i]["Feature"]}',
SMV= '{dtDetail.Rows[i]["SMV"]}',
Remark = '{dtDetail.Rows[i]["Remark"]}',
EditName = '{dtDetail.Rows[i]["EditName"]}',
EditDate = convert(varchar, getdate(), 121)
where ukey='{dtDetail.Rows[i]["Ukey"]}'
";

                        for (int ii = 0; ii < this.dtSMV.Rows.Count; ii++)
                        {
                            if (MyUtility.Convert.GetInt(this.dtSMV.Rows[ii]["StyleFeatureUkey"]) == MyUtility.Convert.GetInt(dtDetail.Rows[i]["ukey"]))
                            {
                                sqlcmd = sqlcmd + $@"
insert into Style_Feature_SMV(StyleFeatureUkey,Seq,OperationID,Annotation,MachineTypeID,Mold,IETMSSMV)
values({MyUtility.Convert.GetInt(dtDetail.Rows[i]["ukey"])},'{this.dtSMV.Rows[ii]["Seq"]}','{this.dtSMV.Rows[ii]["OperationID"]}','{this.dtSMV.Rows[ii]["Annotation"]}','{this.dtSMV.Rows[ii]["MachineTypeID"]}','{this.dtSMV.Rows[ii]["Mold"]}','{this.dtSMV.Rows[ii]["IETMSSMV"]}')
";
                            }
                        }
                    }
                }
            }
            #endregion
            if (sqlcmd.Length > 0)
            {
                TransactionScope transactionscope = new TransactionScope();
                using (transactionscope)
                {
                    DualResult dualResult = DBProxy.Current.Execute(null, sqlcmd);

                    if (dualResult == false)
                    {
                        transactionscope.Dispose();
                        MyUtility.Msg.WarningBox(dualResult.ToString());
                        return Ict.Result.True;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                }
            }

            return Ict.Result.True;
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

            return Ict.Result.True;
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
