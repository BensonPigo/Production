using System;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using System.Data.SqlClient;
using Sci.Win.Tems;
using Sci.Data;
using System.Net.Mail;
using System.Linq;

namespace Sci
{
    /// <summary>
    /// Production Grid Exts
    /// </summary>
    public static class ProductionGridExts
    {
        /// <summary>
        /// Clog Location
        /// </summary>
        /// <param name="gen">DataGridView Generator</param>
        /// <param name="propertyname">Property name</param>
        /// <param name="header">Header</param>
        /// <param name="width">Width</param>
        /// <param name="settings">DataGridView Generator TextColumn Settings</param>
        /// <param name="iseditable">is editable</param>
        /// <param name="iseditingreadonly">is editing readonly</param>
        /// <param name="alignment">DataGridView Content Alignment</param>
        /// <returns>gen</returns>
        public static IDataGridViewGenerator CellClogLocation(this IDataGridViewGenerator gen, string propertyname, string header, IWidth width = null, DataGridViewGeneratorTextColumnSettings settings = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null)
        {
            if (settings == null)
            {
                settings = new DataGridViewGeneratorTextColumnSettings();
            }

            settings.CharacterCasing = CharacterCasing.Upper;
            settings.EditingMouseDown += (s, e) =>
            {
                Win.UI.Grid g = (Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Win.Forms.Base frm = (Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,Description from ClogLocation group by ID,Description order by ID", "10,40", dr["ClogLocationId"].ToString().Trim());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };
            settings.CellValidating += (s, e) =>
            {
                Win.UI.Grid g = (Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Win.Forms.Base frm = (Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                    {
                        if (MyUtility.Check.Seek(e.FormattedValue.ToString(), "ClogLocation", "id") == false)
                        {
                            dr["ClogLocationId"] = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< ClogLocation : {0} > not found!!!", e.FormattedValue.ToString()));
                            return;
                        }
                    }
                }
            };
            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }

        /// <summary>
        /// CFALocation
        /// </summary>
        /// <param name="gen">DataGridView Generator</param>
        /// <param name="propertyname">Property name</param>
        /// <param name="header">Header</param>
        /// <param name="m">MDivision</param>
        /// <param name="width">Width</param>
        /// <param name="settings">DataGridView Generator TextColumn Settings</param>
        /// <param name="iseditable">is editable</param>
        /// <param name="iseditingreadonly">is editing readonly</param>
        /// <param name="alignment">DataGridView Content Alignment</param>
        /// <returns>gen</returns>
        public static IDataGridViewGenerator CellCFALocation(this IDataGridViewGenerator gen, string propertyname, string header, string m = "", IWidth width = null, DataGridViewGeneratorTextColumnSettings settings = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null)
        {
            if (settings == null)
            {
                settings = new DataGridViewGeneratorTextColumnSettings();
            }

            settings.CharacterCasing = CharacterCasing.Upper;
            settings.EditingMouseDown += (s, e) =>
            {
                Win.UI.Grid g = (Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Win.Forms.Base frm = (Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                            string sqlcmd = string.Empty;
                            if (MyUtility.Check.Empty(m))
                            {
                                sqlcmd = "select ID,Description from CFALocation group by ID,Description order by ID";
                            }
                            else
                            {
                                sqlcmd = $"select ID,Description from CFALocation where MDivisionID='{m}'  group by ID,Description order by ID";
                            }

                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "10,40", dr["CFALocationId"].ToString().Trim());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };
            settings.CellValidating += (s, e) =>
            {
                Win.UI.Grid g = (Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Win.Forms.Base frm = (Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                    {
                        string sqlcmd = string.Empty;
                        if (MyUtility.Check.Empty(m))
                        {
                            sqlcmd = "select ID,Description from CFALocation where id = '{e.FormattedValue}' group by ID,Description order by ID";
                        }
                        else
                        {
                            sqlcmd = $"select ID,Description from CFALocation where MDivisionID='{m}'and id = '{e.FormattedValue}'  group by ID,Description order by ID";
                        }

                        if (!MyUtility.Check.Seek(sqlcmd))
                        {
                            dr["CFALocationId"] = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< CFALocation : {0} > not found!!!", e.FormattedValue.ToString()));
                            return;
                        }
                    }
                }
            };
            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }

        /// <summary>
        /// Order id
        /// </summary>
        /// <param name="gen">DataGridView Generator</param>
        /// <param name="propertyname">Property name</param>
        /// <param name="header">Header</param>
        /// <param name="width">Width</param>
        /// <param name="settings">DataGridView Generator TextColumn Settings</param>
        /// <param name="iseditable">is editable</param>
        /// <param name="iseditingreadonly">is editing readonly</param>
        /// <param name="alignment">DataGridView Content Alignment</param>
        /// <returns>gen</returns>
        public static IDataGridViewGenerator CellOrderId(
            this IDataGridViewGenerator gen,
            string propertyname,
            string header,
            IWidth width = null,
            DataGridViewGeneratorTextColumnSettings settings = null,
            bool? iseditable = null,
            bool? iseditingreadonly = null,
            DataGridViewContentAlignment? alignment = null)
        {
            if (settings == null)
            {
                settings = new DataGridViewGeneratorTextColumnSettings();
            }

            settings.CharacterCasing = CharacterCasing.Upper;

            settings.CellValidating += (s, e) =>
            {
                Win.UI.Grid g = (Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Win.Forms.Base frm = (Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                    {
                        if (MyUtility.Check.Seek(e.FormattedValue.ToString(), "Orders", "id") == false)
                        {
                            dr["orderid"] = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Order Id : {0} > is not found!!!", e.FormattedValue.ToString()));
                            return;
                        }
                    }
                }
            };
            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }

        /// <summary>
        /// POID with seq , roll ,dyelot
        /// </summary>
        /// <param name="gen">DataGridView Generator</param>
        /// <param name="propertyname">Propertyname</param>
        /// <param name="header">Header</param>
        /// <param name="width">Width</param>
        /// <param name="settings">DataGridView Generator TextColumn Settings</param>
        /// <param name="iseditable">is editable</param>
        /// <param name="iseditingreadonly">is editing readonly</param>
        /// <param name="alignment">DataGridView Content Alignment</param>
        /// <param name="checkMDivisionID">check MDivisionID</param>
        /// <returns>gen</returns>
        public static IDataGridViewGenerator CellPOIDWithSeqRollDyelot(
            this IDataGridViewGenerator gen,
            string propertyname,
            string header,
            IWidth width = null,
            DataGridViewGeneratorTextColumnSettings settings = null,
            bool? iseditable = null,
            bool? iseditingreadonly = null,
            DataGridViewContentAlignment? alignment = null,
            bool checkMDivisionID = false)
        {
            if (settings == null)
            {
                settings = new DataGridViewGeneratorTextColumnSettings();
            }

            settings.CharacterCasing = CharacterCasing.Upper;

            settings.CellValidating += (s, e) =>
            {
                Win.UI.Grid g = (Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Win.Forms.Base frm = (Win.Forms.Base)g.FindForm();
                DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                if (frm.EditMode && string.Compare(dr["poid"].ToString(), e.FormattedValue.ToString()) != 0)
                {
                    if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                    {
                        if (checkMDivisionID)
                        {
                            if (!MyUtility.Check.Seek(string.Format("select * from dbo.orders inner join dbo.factory on orders.FtyGroup=factory.id where orders.ID='{0}' and factory.MDivisionID='{1}' AND orders.Category!='A'", e.FormattedValue.ToString(), Env.User.Keyword), null))
                            {
                                dr["poid"] = string.Empty;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox(string.Format("< Order Id : {0} > is not found!!!", e.FormattedValue.ToString()));
                                return;
                            }
                            else
                            {
                                dr["poid"] = e.FormattedValue;
                                dr["seq"] = string.Empty;
                                dr["seq1"] = string.Empty;
                                dr["seq2"] = string.Empty;
                                dr["roll"] = string.Empty;
                                dr["dyelot"] = string.Empty;
                            }
                        }
                        else
                        {
                            if (MyUtility.Check.Seek(e.FormattedValue.ToString(), "Orders", "id") == false)
                            {
                                dr["poid"] = string.Empty;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox(string.Format("< Order Id : {0} > is not found!!!", e.FormattedValue.ToString()));
                                return;
                            }
                            else
                            {
                                dr["poid"] = e.FormattedValue;
                                dr["seq"] = string.Empty;
                                dr["seq1"] = string.Empty;
                                dr["seq2"] = string.Empty;
                                dr["roll"] = string.Empty;
                                dr["dyelot"] = string.Empty;
                            }
                        }
                    }
                    else
                    {
                        dr["poid"] = string.Empty;
                        dr["seq"] = string.Empty;
                        dr["seq1"] = string.Empty;
                        dr["seq2"] = string.Empty;
                        dr["roll"] = string.Empty;
                        dr["dyelot"] = string.Empty;
                    }
                }
            };
            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }

        /// <summary>
        /// LocalItem: Carton
        /// </summary>
        /// <param name="gen">DataGridView Generator</param>
        /// <param name="propertyname">Property name</param>
        /// <param name="header">Header</param>
        /// <param name="width">Width</param>
        /// <param name="settings">DataGridView Generator TextColumn Settings</param>
        /// <param name="iseditable">is editable</param>
        /// <param name="iseditingreadonly">is editing readonly</param>
        /// <param name="alignment">DataGridView Content Alignment</param>
        /// <returns>gen</returns>
        public static IDataGridViewGenerator CellCartonItem(this IDataGridViewGenerator gen, string propertyname, string header, IWidth width = null, DataGridViewGeneratorTextColumnSettings settings = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null)
        {
            if (settings == null)
            {
                settings = new DataGridViewGeneratorTextColumnSettings();
            }

            settings.CharacterCasing = CharacterCasing.Upper;
            settings.EditingMouseDown = new EventHandler<Ict.Win.UI.DataGridViewEditingControlMouseEventArgs>(CartonRefnoCommon.EditingMouseDown);

            settings.CellValidating += (s, e) =>
            {
                Win.UI.Grid g = (Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Win.Forms.Base frm = (Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                    {
                        string seekSql = string.Format("select RefNo from LocalItem where Category = 'CARTON' and Junk = 0 and RefNo = '{0}'", e.FormattedValue.ToString());
                        if (MyUtility.Check.Seek(seekSql) == false)
                        {
                            dr[propertyname] = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Ref No. : {0} > not found!!!", e.FormattedValue.ToString()));
                            return;
                        }
                    }
                }
            };
            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }

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
        /// <returns>gen</returns>
        public static IDataGridViewGenerator CellMachineType(this IDataGridViewGenerator gen, string propertyname, string header, InputMasterDetail mainForm, IWidth width = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null)
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
                            DataRow dr = mainForm.detailgrid.GetDataRow<DataRow>(e.RowIndex);
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

                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            settings.CellValidating += (s, e) =>
            {
                if (mainForm.EditMode)
                {
                    DataRow dr = mainForm.detailgrid.GetDataRow<DataRow>(e.RowIndex);
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
                    }
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
                if (mainForm.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = @"
select ID,DescEN 
from Mold WITH (NOLOCK) 
where Junk = 0
and IsAttachment = 1
union all
select ID, Description
from SewingMachineAttachment WITH (NOLOCK) 
where Junk = 0";

                    Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sqlcmd, "ID,DescEN", "13,60,10", mainForm.CurrentDetailData["Attachment"].ToString(), null, null, null)
                    {
                        Width = 666,
                    };
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    mainForm.CurrentDetailData["Attachment"] = item.GetSelectedString();
                }
            };

            settings.CellValidating += (s, e) =>
            {
                if (mainForm.EditMode)
                {
                    List<SqlParameter> cmds = new List<SqlParameter>() { new SqlParameter { ParameterName = "@OperationID", Value = MyUtility.Convert.GetString(mainForm.CurrentDetailData["OperationID"]) } };
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
                        mainForm.CurrentDetailData["Attachment"] = string.Empty;
                        MyUtility.Msg.WarningBox("SQL Connection failt!!\r\n" + result.ToString());
                    }

                    if (dtOperation.Rows.Count > 0)
                    {
                        operationList = dtOperation.Rows[0]["Attachment"].ToString().Replace(";", ",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                        if (e.FormattedValue.Empty())
                        {
                            mainForm.CurrentDetailData["Attachment"] = dtOperation.Rows[0]["Attachment"].ToString();

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
                        mainForm.CurrentDetailData["Attachment"] = string.Empty;
                        MyUtility.Msg.WarningBox("SQL Connection failt!!\r\n" + result.ToString());
                    }

                    // 前端轉進來的資料是用[;]區隔，統一用[,]區隔
                    List<string> getMold = e.FormattedValue.ToString().Replace(";", ",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();

                    // 不存在 Mold
                    var existsMold = getMold.Except(dtMold.AsEnumerable().Select(x => x.Field<string>("ID")).ToList());
                    if (existsMold.Any())
                    {
                        e.Cancel = true;
                        mainForm.CurrentDetailData["Attachment"] = string.Join(",", getMold.Where(x => existsMold.Where(y => !y.EqualString(x)).Any()).ToList());
                        MyUtility.Msg.WarningBox("Attachment : " + string.Join(",", existsMold.ToList()) + "  need include in Mold setting !!", "Data need include in setting");
                        return;
                    }

                    mainForm.CurrentDetailData["Attachment"] = string.Join(",", getMold.ToList());
                }
            };

            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }

        /// <summary>
        /// ThreadColor
        /// </summary>
        /// <param name="gen">DataGridView Generator</param>
        /// <param name="propertyname">Property name</param>
        /// <param name="header">Header</param>
        /// <param name="width">Width</param>
        /// <param name="settings">DataGridView Generator TextColumn Settings</param>
        /// <param name="iseditable">is editable</param>
        /// <param name="iseditingreadonly">is editing readonly</param>
        /// <param name="alignment">DataGridView Content Alignment</param>
        /// <returns>gen</returns>
        public static IDataGridViewGenerator CellThreadColor(this IDataGridViewGenerator gen, string propertyname, string header, IWidth width = null, DataGridViewGeneratorTextColumnSettings settings = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null)
        {
            if (settings == null)
            {
                settings = new DataGridViewGeneratorTextColumnSettings();
            }

            settings.CharacterCasing = CharacterCasing.Upper;
            settings.EditingMouseDown += (s, e) =>
            {
                Win.UI.Grid g = (Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Win.Forms.Base frm = (Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,Description from ThreadColor where junk = 0 order by ID", "10,40", dr["ThreadColorid"].ToString().Trim());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            var sellist = item.GetSelecteds();
                            if (dr.Table.Columns.Contains("Colordesc"))
                            {
                                dr["Colordesc"] = sellist[0][1];
                            }

                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            settings.CellValidating += (s, e) =>
            {
                Win.UI.Grid g = (Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Win.Forms.Base frm = (Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                    {
                        string seekSql = string.Format("select id from ThreadColor where Junk = 0 and id = '{0}'", e.FormattedValue.ToString());
                        if (!MyUtility.Check.Seek(seekSql))
                        {
                            dr["Threadcolorid"] = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Thread Color : {0} > not found!!!", e.FormattedValue.ToString()));
                            return;
                        }
                    }
                }
            };
            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }

        /// <summary>
        /// ThreadLocation
        /// </summary>
        /// <param name="gen">DataGridView Generator</param>
        /// <param name="propertyname">Property name</param>
        /// <param name="header">Header</param>
        /// <param name="width">Width</param>
        /// <param name="settings">DataGridView Generator TextColumn Settings</param>
        /// <param name="iseditable">is editable</param>
        /// <param name="iseditingreadonly">is editing readonly</param>
        /// <param name="alignment">DataGridView Content Alignment</param>
        /// <param name="isChangeSelItem">is Change SelItem</param>
        /// <returns>gen</returns>
        public static IDataGridViewGenerator CellThreadLocation(this IDataGridViewGenerator gen, string propertyname, string header, IWidth width = null, DataGridViewGeneratorTextColumnSettings settings = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null, bool? isChangeSelItem = null)
        {
            if (settings == null)
            {
                settings = new DataGridViewGeneratorTextColumnSettings();
            }

            string keyword = Env.User.Keyword;
            settings.CharacterCasing = CharacterCasing.Upper;
            settings.EditingMouseDown += (s, e) =>
            {
                Win.UI.Grid g = (Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Win.Forms.Base frm = (Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                            string sql = string.Empty;
                            string strwidth = "10,40";
                            if (isChangeSelItem == true)
                            {
                                DataRow dd = g.GetDataRow<DataRow>(e.RowIndex);
                                string ddsql = string.Format("select 1 from ThreadStock where Refno='{0}' and ThreadColorID='{1}' ", dd["Refno"].ToString(), dd["ThreadColorid"].ToString());
                                if (MyUtility.Check.Seek(ddsql))
                                {
                                    sql = string.Format("select ID,Description,NewCone,UsedCone from ThreadLocation a left join ThreadStock b on a.id=b.ThreadLocationID  where  junk = 0 and Refno='{0}' and ThreadColorID='{1}' order by ID", dd["Refno"].ToString(), dd["ThreadColorid"].ToString());
                                    strwidth = "8,18,10,10";
                                }
                                else
                                {
                                    sql = "select ID,Description from ThreadLocation where junk = 0 order by ID";
                                }
                            }
                            else
                            {
                                sql = "select ID,Description from ThreadLocation where junk = 0 order by ID";
                            }

                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, strwidth, dr["ThreadLocationid"].ToString().Trim());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            settings.CellValidating += (s, e) =>
            {
                Win.UI.Grid g = (Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Win.Forms.Base frm = (Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                    {
                        string seekSql = string.Format("select id from ThreadLocation where Junk = 0 and id = '{0}'", e.FormattedValue.ToString());
                        if (!MyUtility.Check.Seek(seekSql))
                        {
                            dr["ThreadLocationid"] = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Thread Location : {0} > not found!!!", e.FormattedValue.ToString()));
                            return;
                        }
                    }
                }
            };
            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }

        /// <summary>
        /// User LogID Name
        /// </summary>
        /// <param name="gen">DataGridView Generator</param>
        /// <param name="propertyname">Property name</param>
        /// <param name="header">Header</param>
        /// <param name="width">Width</param>
        /// <param name="settings">DataGridView Generator TextColumn Settings</param>
        /// <param name="iseditable">is editable</param>
        /// <param name="iseditingreadonly">is editing readonly</param>
        /// <param name="alignment">DataGridView Content Alignment</param>
        /// <param name="userNamePropertyName">userName PropertyName</param>
        /// <returns>gen</returns>
        public static IDataGridViewGenerator CellUser(this IDataGridViewGenerator gen, string propertyname, string header, IWidth width = null, DataGridViewGeneratorTextColumnSettings settings = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null, string userNamePropertyName = null)
        {
            if (settings == null)
            {
                settings = new DataGridViewGeneratorTextColumnSettings();
            }

            string keyword = Env.User.Keyword;
            settings.CharacterCasing = CharacterCasing.Upper;
            settings.EditingMouseDown += (s, e) =>
            {
                Win.UI.Grid g = (Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Win.Forms.Base frm = (Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(string.Format("select ID,Name from Pass1"), "10,40", dr[propertyname].ToString().Trim());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            settings.CellValidating += (s, e) =>
            {
                Win.UI.Grid g = (Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Win.Forms.Base frm = (Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                    DataRow pass_dr;
                    if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                    {
                        List<SqlParameter> listSqlParameter = new List<SqlParameter>();
                        listSqlParameter.Add(new SqlParameter("@UserID", e.FormattedValue.ToString()));
                        string seekSql = "select id,Name from Pass1 where id = @UserID";
                        if (!MyUtility.Check.Seek(seekSql, listSqlParameter, out pass_dr))
                        {
                            dr[propertyname] = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< User ID : {0} > not found!!!", e.FormattedValue.ToString()));
                            return;
                        }
                        else
                        {
                            if (userNamePropertyName != null)
                            {
                                dr[userNamePropertyName] = pass_dr["Name"];
                                dr[propertyname] = e.FormattedValue.ToString();
                                dr.EndEdit();
                            }
                        }
                    }
                }
            };
            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }

        /// <summary>
        /// Scale
        /// </summary>
        /// <param name="gen">DataGridView Generator</param>
        /// <param name="propertyname">Property name</param>
        /// <param name="header">Header</param>
        /// <param name="width">Width</param>
        /// <param name="settings">DataGridView Generator TextColumn Settings</param>
        /// <param name="iseditable">is editable</param>
        /// <param name="iseditingreadonly">is editing readonly</param>
        /// <param name="alignment">DataGridView Content Alignment</param>
        /// <returns>gen</returns>
        public static IDataGridViewGenerator CellScale(this IDataGridViewGenerator gen, string propertyname, string header, IWidth width = null, DataGridViewGeneratorTextColumnSettings settings = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null)
        {
            if (settings == null)
            {
                settings = new DataGridViewGeneratorTextColumnSettings();
            }

            string keyword = Env.User.Keyword;
            settings.CharacterCasing = CharacterCasing.Upper;
            settings.EditingMouseDown += (s, e) =>
            {
                Win.UI.Grid g = (Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Win.Forms.Base frm = (Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID from Scale where junk = 0 order by ID", "10,40", dr["Scale"].ToString().Trim());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            settings.CellValidating += (s, e) =>
            {
                Win.UI.Grid g = (Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Win.Forms.Base frm = (Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                    {
                        string seekSql = string.Format("select id from Scale where Junk = 0 and id = '{0}'", e.FormattedValue.ToString());
                        if (!MyUtility.Check.Seek(seekSql))
                        {
                            dr["Scale"] = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Scale : {0} > not found!!!", e.FormattedValue.ToString()));
                            return;
                        }
                    }
                }
            };
            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }
    }
}
