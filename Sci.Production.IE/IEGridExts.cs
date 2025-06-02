using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tems;
using Sci.Win.Tools;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// IEGridExts
    /// </summary>
    public static class IEGridExts
    {
        /// <summary>
        /// CellMachineType
        /// </summary>
        /// <param name="gen">DataGridView Generator</param>
        /// <param name="propertyname">Property name</param>
        /// <param name="header">Header</param>
        /// <param name="mainForm">call from</param>
        /// <param name="width">Width</param>
        /// <param name="iseditable">is editable</param>
        /// <param name="iseditingreadonly">is editing readonly</param>
        /// <param name="alignment">DataGridView Content Alignment</param>
        /// <param name="p03">IE P03</param>
        /// <returns>gen</returns>
        public static IDataGridViewGenerator CellMachineType(this IDataGridViewGenerator gen, string propertyname, string header, InputMasterDetail mainForm, IWidth width = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null, P03 p03 = null)
        {
            DataGridViewGeneratorTextColumnSettings settings = new DataGridViewGeneratorTextColumnSettings();

            settings.EditingMouseDown += (s, e) =>
            {
                if (mainForm.EditMode)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            Grid parentGrid = (Grid)((DataGridViewColumn)s).DataGridView;
                            DataRow dr = parentGrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = "select ID,Description from MachineType WITH (NOLOCK) where Junk = 0";
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "8,43", dr["MachineTypeID"].ToString())
                            {
                                Width = 590,
                            };
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            dr["MachineTypeID"] = item.GetSelectedString();
                            if (dr.Table.Columns.Contains("MasterPlusGroup"))
                            {
                                dr["MasterPlusGroup"] = string.Empty;
                            }

                            if (p03 != null)
                            {
                                p03.CalculateSewerDiffPercentage();
                            }
                        }
                    }
                }
            };

            settings.CellValidating += (s, e) =>
            {
                if (mainForm.EditMode)
                {
                    Grid parentGrid = (Grid)((DataGridViewColumn)s).DataGridView;
                    DataRow dr = parentGrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["MachineTypeID"].ToString())
                    {
                        // sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter
                        {
                            ParameterName = "@id",
                            Value = e.FormattedValue.ToString(),
                        };

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>
                        {
                            sp1,
                        };
                        string sqlCmd = "select ID from MachineType WITH (NOLOCK) where Junk = 0 and ID = @id";
                        DataTable machineData;
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out machineData);
                        if (!result)
                        {
                            dr["MachineTypeID"] = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            return;
                        }
                        else
                        {
                            if (machineData.Rows.Count <= 0)
                            {
                                dr["MachineTypeID"] = string.Empty;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox(string.Format("< ST/MC type: {0} > not found!!!", e.FormattedValue.ToString()));
                                return;
                            }
                        }

                        dr["MachineTypeID"] = e.FormattedValue.ToString();
                        if (dr.Table.Columns.Contains("MasterPlusGroup"))
                        {
                            dr["MasterPlusGroup"] = string.Empty;
                        }
                    }

                    if (dr.Table.Columns.Contains("MasterPlusGroup") && MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["MachineTypeID"] = e.FormattedValue.ToString();
                        dr["MasterPlusGroup"] = string.Empty;
                    }

                    if (p03 != null)
                    {
                        p03.CalculateSewerDiffPercentage();
                    }
                }
            };

            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }

        /// <summary>
        /// CellIEReason
        /// </summary>
        /// <param name="gen">DataGridView Generator</param>
        /// <param name="propertyname">Property name</param>
        /// <param name="header">Header</param>
        /// <param name="mainForm">call from</param>
        /// <param name="type">type</param>
        /// <param name="width">Width</param>
        /// <param name="iseditable">is editable</param>
        /// <param name="iseditingreadonly">is editing readonly</param>
        /// <param name="alignment">DataGridView Content Alignment</param>
        /// <returns>gen</returns>
        public static IDataGridViewGenerator CellIEReason(this IDataGridViewGenerator gen, string propertyname, string header, Sci.Win.Tems.Base mainForm, string type, IWidth width = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null)
        {
            DataGridViewGeneratorTextColumnSettings settings = new DataGridViewGeneratorTextColumnSettings();

            settings.EditingMouseDown += (s, e) =>
            {
                if (!mainForm.EditMode)
                {
                    return;
                }

                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = gen.Control.GetDataRow<DataRow>(e.RowIndex);
                string sqlCmd = $"select ID, Description from IEReason WITH (NOLOCK) where Junk = 0 and type = '{type}'";
                Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "8,43", dr["IEReasonID"].ToString())
                {
                    Width = 590,
                };
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                dr["IEReasonID"] = item.GetSelecteds()[0]["ID"];
                dr["Description"] = item.GetSelecteds()[0]["Description"];
            };

            settings.CellValidating += (s, e) =>
            {
                if (!mainForm.EditMode)
                {
                    return;
                }

                DataRow dr = gen.Control.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["IEReasonID"].ToString())
                {
                    // sql參數
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter
                    {
                        ParameterName = "@id",
                        Value = e.FormattedValue.ToString(),
                    };

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>
                        {
                            sp1,
                        };
                    string sqlCmd = $"select ID, Description from IEReason WITH (NOLOCK) where Junk = 0 and ID = @id and type = '{type}'";
                    DataTable reasonData;
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out reasonData);
                    if (!result)
                    {
                        dr["IEReasonID"] = string.Empty;
                        dr["Description"] = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                        return;
                    }

                    if (reasonData.Rows.Count <= 0)
                    {
                        dr["IEReasonID"] = string.Empty;
                        dr["Description"] = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Reason ID: {0} > not found!!!", e.FormattedValue.ToString()));
                        return;
                    }

                    dr["IEReasonID"] = e.FormattedValue.ToString();
                    dr["Description"] = reasonData.Rows[0]["Description"].ToString();
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["IEReasonID"] = string.Empty;
                    dr["Description"] = string.Empty;
                }
            };

            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }

        /// <summary>
        /// CellAttachment
        /// </summary>
        /// <param name="gen">DataGridView Generator</param>
        /// <param name="propertyname">Property name</param>
        /// <param name="header">Header</param>
        /// <param name="mainForm">call from</param>
        /// <param name="width">Width</param>
        /// <param name="iseditable">is editable</param>
        /// <param name="iseditingreadonly">is editing readonly</param>
        /// <param name="alignment">DataGridView Content Alignment</param>
        /// <returns>gen</returns>
        public static IDataGridViewGenerator CellAttachment(this IDataGridViewGenerator gen, string propertyname, string header, InputMasterDetail mainForm, IWidth width = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null)
        {
            DataGridViewGeneratorTextColumnSettings settings = new DataGridViewGeneratorTextColumnSettings();

            settings.EditingMouseDown += (s, e) =>
            {
                Grid parentGrid = (Grid)((DataGridViewColumn)s).DataGridView;
                DataRow dr = parentGrid.GetDataRow<DataRow>(e.RowIndex);
                if (mainForm.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = @"
select ID,DescEN 
from Mold WITH (NOLOCK) 
where Junk = 0
and IsAttachment = 1";

                    Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sqlcmd, "ID,DescEN", "13,60,10", dr["Attachment"].ToString(), null, null, null)
                    {
                        Width = 666,
                    };
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    if (MyUtility.Convert.GetString(dr["Attachment"]) != item.GetSelectedString())
                    {
                        dr["SewingMachineAttachmentID"] = string.Empty;
                    }

                    dr["Attachment"] = item.GetSelectedString();

                    if (dr.Table.Columns.Contains("MoldID"))
                    {
                        dr["MoldID"] = item.GetSelectedString();
                    }
                }
            };

            settings.CellValidating += (s, e) =>
            {
                if (mainForm.EditMode)
                {
                    Grid parentGrid = (Grid)((DataGridViewColumn)s).DataGridView;
                    DataRow dr = parentGrid.GetDataRow<DataRow>(e.RowIndex);

                    List<SqlParameter> cmds = new List<SqlParameter>() { new SqlParameter { ParameterName = "@OperationID", Value = MyUtility.Convert.GetString(dr["OperationID"]) } };
                    string sqlcmd = @"
select [Attachment] = STUFF((
		        select concat(',' ,s.Data)
		        from SplitString(o.MoldID, ';') s
		        inner join Mold m WITH (NOLOCK) on s.Data = m.ID
		        where m.IsAttachment = 1
                and m.Junk = 0
		        for xml path ('')) 
	        ,1,1,'')
	,[Template] = STUFF((
		            select concat(',' ,s.Data)
		            from SplitString(o.MoldID, ';') s
		            inner join Mold m WITH (NOLOCK) on s.Data = m.ID
		            where m.IsTemplate = 1
                    and m.Junk = 0
		            for xml path ('')) 
	            ,1,1,'')
from Operation o WITH (NOLOCK) 
where o.ID = @OperationID";
                    DataTable dtOperation;
                    DualResult result = DBProxy.Current.Select(null, sqlcmd, cmds, out dtOperation);
                    List<string> operationList = new List<string>();
                    if (!result)
                    {
                        dr["Attachment"] = string.Empty;
                        dr["MoldID"] = string.Empty;
                        MyUtility.Msg.WarningBox("SQL Connection failt!!\r\n" + result.ToString());
                    }

                    if (dtOperation.Rows.Count > 0)
                    {
                        operationList = dtOperation.Rows[0]["Attachment"].ToString().Replace(";", ",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                        if (e.FormattedValue.Empty())
                        {
                            dr["Attachment"] = dtOperation.Rows[0]["Attachment"].ToString();
                            if (dr.Table.Columns.Contains("MoldID"))
                            {
                                dr["MoldID"] = dtOperation.Rows[0]["Attachment"].ToString();
                            }

                            dr["SewingMachineAttachmentID"] = string.Empty;

                            return;
                        }
                    }

                    sqlcmd = @"
select ID 
from Mold WITH (NOLOCK) 
where Junk = 0
and IsAttachment = 1
union all
select ID
from SewingMachineAttachment WITH (NOLOCK) 
where Junk = 0";
                    DataTable dtMold;
                    result = DBProxy.Current.Select(null, sqlcmd, out dtMold);
                    if (!result)
                    {
                        dr["Attachment"] = string.Empty;
                        if (dr.Table.Columns.Contains("MoldID"))
                        {
                            dr["MoldID"] = string.Empty;
                        }
                        dr["SewingMachineAttachmentID"] = string.Empty;
                        MyUtility.Msg.WarningBox("SQL Connection failt!!\r\n" + result.ToString());
                    }

                    // 前端轉進來的資料是用[;]區隔，統一用[,]區隔
                    List<string> getMold = e.FormattedValue.ToString().Replace(";", ",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();

                    // 不存在 Mold
                    var existsMold = getMold.Except(dtMold.AsEnumerable().Select(x => x.Field<string>("ID")).ToList());
                    getMold.AddRange(operationList);
                    if (existsMold.Any())
                    {
                        e.Cancel = true;
                        dr["Attachment"] = string.Join(",", getMold.Where(x => existsMold.Where(y => !y.EqualString(x)).Any()).Distinct().ToList());
                        if (dr.Table.Columns.Contains("MoldID"))
                        {
                            dr["MoldID"] = string.Join(",", getMold.Where(x => existsMold.Where(y => !y.EqualString(x)).Any()).Distinct().ToList());
                        }

                        MyUtility.Msg.WarningBox("Attachment : " + string.Join(",", existsMold.ToList()) + "  need include in Mold setting !!", "Data need include in setting");
                        return;
                    }

                    if (MyUtility.Convert.GetString(dr["Attachment"]) != string.Join(",", getMold.Distinct().ToList()))
                    {
                        dr["SewingMachineAttachmentID"] = string.Empty;
                    }

                    dr["Attachment"] = string.Join(",", getMold.Distinct().ToList());

                    if (dr.Table.Columns.Contains("MoldID"))
                    {
                        dr["MoldID"] = string.Join(",", getMold.Distinct().ToList());
                    }
                }
            };

            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }

        /// <summary>
        /// CellPartID
        /// </summary>
        /// <param name="gen">DataGridView Generator</param>
        /// <param name="propertyname">Property name</param>
        /// <param name="header">Header</param>
        /// <param name="mainForm">call from</param>
        /// <param name="width">Width</param>
        /// <param name="iseditable">is editable</param>
        /// <param name="iseditingreadonly">is editing readonly</param>
        /// <param name="alignment">DataGridView Content Alignment</param>
        /// <returns>gen</returns>
        public static IDataGridViewGenerator CellPartID(this IDataGridViewGenerator gen, string propertyname, string header, InputMasterDetail mainForm, IWidth width = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null)
        {
            DataGridViewGeneratorTextColumnSettings settings = new DataGridViewGeneratorTextColumnSettings();

            settings.EditingMouseDown += (s, e) =>
            {
                if (mainForm.EditMode && e.Button == MouseButtons.Right)
                {
                    if (e.RowIndex != -1)
                    {
                        Grid parentGrid = (Grid)((DataGridViewColumn)s).DataGridView;
                        DataRow dr = parentGrid.GetDataRow<DataRow>(e.RowIndex);

                        if (MyUtility.Check.Empty(dr["Attachment"]))
                        {
                            return;
                        }

                        P01_PartID callNextForm = new P01_PartID(MyUtility.Convert.GetString(dr["Attachment"]), MyUtility.Convert.GetString(dr["SewingMachineAttachmentID"]));
                        DialogResult result = callNextForm.ShowDialog(mainForm);

                        if (result == DialogResult.Cancel)
                        {
                            if (callNextForm.P01SelectPartID != null)
                            {
                                dr["SewingMachineAttachmentID"] = callNextForm.P01SelectPartID["ID"].ToString();
                                dr.EndEdit();
                            }
                        }

                        if (result == DialogResult.OK)
                        {
                            if (callNextForm.P01SelectPartID != null)
                            {
                                dr["SewingMachineAttachmentID"] = callNextForm.P01SelectPartID["ID"].ToString();
                                dr.EndEdit();
                            }
                        }
                    }
                }
            };

            settings.CellValidating += (s, e) =>
            {
                if (mainForm.EditMode)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        return;
                    }
                    Grid parentGrid = (Grid)((DataGridViewColumn)s).DataGridView;
                    DataRow dr = parentGrid.GetDataRow<DataRow>(e.RowIndex);

                    string newSewingMachineAttachmentID = MyUtility.Convert.GetString(e.FormattedValue);
                    string moldID = dr["Attachment"].ToString();

                    string sqlcmd = $@"
select a.ID
    ,a.Description
    ,a.MachineMasterGroupID
    ,AttachmentTypeID
    ,MeasurementID
    ,FoldTypeID
from SewingMachineAttachment a
left join AttachmentType b on a.AttachmentTypeID = b.Type 
left join AttachmentMeasurement c on a.MeasurementID = c.Measurement
left join AttachmentFoldType d on a.FoldTypeID = d.FoldType 
where a.MoldID IN ('{string.Join("','", moldID.Split(','))}') ";

                    List<SqlParameter> paras = new List<SqlParameter>();

                    // SewingMachineAttachment.ID可以多選
                    if (newSewingMachineAttachmentID.Split(',').Length > 1)
                    {
                        sqlcmd += $@" AND a.ID IN ('{string.Join("','", newSewingMachineAttachmentID.Split(','))}')";
                    }
                    else
                    {
                        sqlcmd += " AND a.ID = @ID";
                        paras.Add(new SqlParameter("@ID", MyUtility.Convert.GetString(e.FormattedValue)));
                    }

                    DataTable dt;

                    DualResult r = DBProxy.Current.Select(null, sqlcmd, paras, out dt);

                    if (!r)
                    {
                        e.Cancel = true;
                        MsgHelper.Current.ShowErr(mainForm, r);
                        return;
                    }

                    if (dt.Rows == null || dt.Rows.Count == 0)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Data not found");
                    }
                }
            };

            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }

        /// <summary>
        /// CellThreadComboID
        /// </summary>
        /// <param name="gen">DataGridView Generator</param>
        /// <param name="propertyname">Property name</param>
        /// <param name="header">Header</param>
        /// <param name="mainForm">call from</param>
        /// <param name="width">Width</param>
        /// <param name="iseditable">is editable</param>
        /// <param name="iseditingreadonly">is editing readonly</param>
        /// <param name="alignment">DataGridView Content Alignment</param>
        /// <returns>gen</returns>
        public static IDataGridViewGenerator CellThreadComboID(this IDataGridViewGenerator gen, string propertyname, string header, InputMasterDetail mainForm, IWidth width = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null)
        {
            DataGridViewGeneratorTextColumnSettings settings = new DataGridViewGeneratorTextColumnSettings();

            settings.EditingMouseDown += (s, e) =>
            {
                if (!mainForm.EditMode)
                {
                    return;
                }

                if (((DataGridViewGenerator.TextColumn)s).IsEditingReadOnly)
                {
                    return;
                }

                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                string styleUkey;
                if (mainForm.CurrentMaintain.Table.Columns.Contains("StyleUkey"))
                {
                    styleUkey = mainForm.CurrentMaintain["StyleUkey"].ToString();
                }
                else
                {
                    styleUkey = MyUtility.GetValue.Lookup($"select ukey from Style with (nolock) where ID = '{mainForm.CurrentMaintain["StyleID"]}' and SeasonID = '{mainForm.CurrentMaintain["SeasonID"]}' and BrandID = '{mainForm.CurrentMaintain["BrandID"]}'");
                }

                Grid parentGrid = (Grid)((DataGridViewColumn)s).DataGridView;
                DataRow dr = parentGrid.GetDataRow<DataRow>(e.RowIndex);
                string sqlCmd = $@"
select　Thread_ComboID
from Style_ThreadColorCombo st with (nolock)
where	st.StyleUkey = '{styleUkey}'
GROUP by Thread_ComboID
order by Thread_ComboID asc
";
                SelectItem item = new Win.Tools.SelectItem(sqlCmd, "12", dr[propertyname].ToString());
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                e.EditingControl.Text = item.GetSelectedString();
            };

            settings.CellValidating += (s, e) =>
            {
                if (!mainForm.EditMode)
                {
                    return;
                }

                Grid parentGrid = (Grid)((DataGridViewColumn)s).DataGridView;
                DataRow dr = parentGrid.GetDataRow<DataRow>(e.RowIndex);
                if (MyUtility.Check.Empty(e.FormattedValue) || e.FormattedValue.ToString() == dr[propertyname].ToString())
                {
                    return;
                }

                string styleUkey;
                if (mainForm.CurrentMaintain.Table.Columns.Contains("StyleUkey"))
                {
                    styleUkey = mainForm.CurrentMaintain["StyleUkey"].ToString();
                }
                else
                {
                    styleUkey = MyUtility.GetValue.Lookup($"select ukey from Style with (nolock) where ID = '{mainForm.CurrentMaintain["StyleID"]}' and SeasonID = '{mainForm.CurrentMaintain["SeasonID"]}' and BrandID = '{mainForm.CurrentMaintain["BrandID"]}'");
                }

                string sqlCmd = $@"
select　1
from Style_ThreadColorCombo st with (nolock)
where	st.StyleUkey = '{styleUkey}' and
        st.Thread_ComboID = '{e.FormattedValue}'
";
                DataTable machineData;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out machineData);
                if (!result)
                {
                    dr[propertyname] = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                    return;
                }

                if (machineData.Rows.Count <= 0)
                {
                    dr[propertyname] = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< ST/MC type: {0} > not found!!!", e.FormattedValue.ToString()));
                    return;
                }
            };

            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }

        /// <summary>
        /// CellTemplate
        /// </summary>
        /// <param name="gen">DataGridView Generator</param>
        /// <param name="propertyname">Property name</param>
        /// <param name="header">Header</param>
        /// <param name="mainForm">call from</param>
        /// <param name="width">Width</param>
        /// <param name="iseditable">is editable</param>
        /// <param name="iseditingreadonly">is editing readonly</param>
        /// <param name="alignment">DataGridView Content Alignment</param>
        /// <returns>gen</returns>
        public static IDataGridViewGenerator CellTemplate(this IDataGridViewGenerator gen, string propertyname, string header, InputMasterDetail mainForm, IWidth width = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null)
        {
            DataGridViewGeneratorTextColumnSettings settings = new DataGridViewGeneratorTextColumnSettings();

            settings.EditingMouseDown += (s, e) =>
            {
                if (mainForm.EditMode && e.Button == MouseButtons.Right)
                {
                    Grid parentGrid = (Grid)((DataGridViewColumn)s).DataGridView;
                    DataRow dr = parentGrid.GetDataRow<DataRow>(e.RowIndex);

                    string sqlcmd = @"
select PartID = smt.ID , m.DescEN ,MoldID = m.ID
from Mold m WITH (NOLOCK)
right join SewingMachineTemplate smt on m.ID = smt.MoldID
where m.Junk = 0 and m.IsTemplate = 1 and smt.Junk = 0
UNION
SELECT PartID=ID , DescEN ,MoldID = ID
from Mold
where Junk=0 and IsTemplate=1
";

                    SelectItem2 item = new SelectItem2(sqlcmd, "MoldID,DescEN,PartID", "13,60,20", dr["Template"].ToString(), null, null, null)
                    {
                        Width = 1000,
                    };
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    IList<DataRow> selectedRows = item.GetSelecteds();

                    if (selectedRows.Any())
                    {
                        var t = selectedRows.ToList();
                        dr["Template"] = string.Join(",", t.Select(o => o["PartID"]).ToList());
                    }
                    else
                    {
                        dr["Template"] = string.Empty;
                    }
                }
            };

            settings.CellValidating += (s, e) =>
            {
                if (mainForm.EditMode)
                {
                    Grid parentGrid = (Grid)((DataGridViewColumn)s).DataGridView;
                    DataRow dr = parentGrid.GetDataRow<DataRow>(e.RowIndex);

                    List<SqlParameter> cmds = new List<SqlParameter>() { new SqlParameter { ParameterName = "@OperationID", Value = MyUtility.Convert.GetString(dr["OperationID"]) } };
                    string sqlcmd = @"
select [Mold] = STUFF((
		        select concat(',' ,s.Data)
		        from SplitString(o.MoldID, ';') s
		        inner join Mold m WITH (NOLOCK) on s.Data = m.ID
		        where m.IsAttachment = 1
                and m.Junk = 0
		        for xml path ('')) 
	        ,1,1,'')
	,[Template] = STUFF((
		            select concat(',' ,s.Data)
		            from SplitString(o.MoldID, ';') s
		            inner join Mold m WITH (NOLOCK) on s.Data = m.ID
		            where m.IsTemplate = 1
                    and m.Junk = 0
		            for xml path ('')) 
	            ,1,1,'')
from Operation o WITH (NOLOCK) 
where o.ID = @OperationID";
                    DataTable dtOperation;
                    DualResult result = DBProxy.Current.Select(null, sqlcmd, cmds, out dtOperation);
                    List<string> operationList = new List<string>();
                    if (!result)
                    {
                        dr["Mold"] = string.Empty;
                        MyUtility.Msg.WarningBox("SQL Connection failt!!\r\n" + result.ToString());
                    }

                    if (dtOperation.Rows.Count > 0)
                    {
                        operationList = dtOperation.Rows[0]["Mold"].ToString().Replace(";", ",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();

                        if (MyUtility.Check.Empty(e.FormattedValue))
                        {
                            dr["Template"] = dtOperation.Rows[0]["Template"].ToString();
                            return;
                        }
                    }

                    dr["Template"] = e.FormattedValue;
                    sqlcmd = @"
select PartID = smt.ID , m.DescEN ,MoldID = m.ID
from Mold m WITH (NOLOCK)
right join SewingMachineTemplate smt on m.ID = smt.MoldID
where m.Junk = 0 and m.IsTemplate = 1 and smt.Junk = 0
UNION
SELECT PartID=ID , DescEN ,MoldID = ID
from Mold
where Junk=0 and IsTemplate=1
";
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = dr["Template"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errTemplate = new List<string>();
                    List<string> trueTemplate = new List<string>();
                    foreach (string item in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["PartID"].EqualString(item)) && !item.EqualString(string.Empty))
                        {
                            selectId &= false;
                            errTemplate.Add(item);
                        }
                        else if (!item.EqualString(string.Empty))
                        {
                            trueTemplate.Add(item);
                        }
                    }

                    if (!selectId)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Template : " + string.Join(",", errTemplate.ToArray()) + "  Data not found !!", "Data not found");
                    }

                    trueTemplate.Sort();
                    dr["Template"] = string.Join(",", trueTemplate.ToArray());
                }
            };

            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }
    }
}
