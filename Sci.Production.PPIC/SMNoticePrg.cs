using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using Ict;
using Sci.Data;
using Sci.Utility;
using Sci.Win.UI;

using MsExcel = Microsoft.Office.Interop.Excel;
using System.Drawing;

namespace Sci.Production.Class.Commons
{
    /// <summary>
    /// 用來處理Pattern/Marker/IE/SMNotice等等的QueryFor，因為他們有共同特性，會用登入者的CountryList欄位來做篩選
    /// </summary>
    public static class SMNoticePrg
    {
        private static object MissingType = Type.Missing;

        #region QueryFors setup

        /// <summary>
        /// 要把下拉選單做成哪一種
        /// </summary>
        public enum SMNoticeCategoryEnum
        {
            /// <summary>
            /// GarmentMarker
            /// </summary>
            GarmentMarker,

            /// <summary>
            /// IE
            /// </summary>
            IE,

            /// <summary>
            /// Main
            /// </summary>
            Main,
        }

        /// <summary>
        /// (Pattern/Marker/IE/SMNotice)建立畫面上的UseFor下拉選單，並且處理他的SelectIndexChanged
        /// </summary>
        /// <param name="sMNoticeCategory">SMNoticeCategoryEnum</param>
        /// <param name="form">Input1</param>
        /// <param name="queryfors">ComboBox</param>
        /// <param name="showErr"><![CDATA[Func<Ict.DualResult, bool>]]></param>
        /// <param name="reloadDatas"><![CDATA[Func<Ict.DualResult>]]></param>
        public static void SetupQueryFors_SMNoticeStatus(SMNoticeCategoryEnum sMNoticeCategory, Win.Tems.Input1 form, ComboBox queryfors, Func<DualResult, bool> showErr, Func<DualResult> reloadDatas)
        {
            var resetDefaultWhere = new Action(() =>
            {
                var creteria = new List<string>();
                var statusFieldName = string.Empty;
                switch (sMNoticeCategory)
                {
                    case SMNoticeCategoryEnum.GarmentMarker:
                        statusFieldName = "StatusPattern";

                        // 如果不是Admin，就要拿國別來篩選目前使用者能不能看到(申請單的細項的指派工廠SMNotice_Detail.Factory)
                        if (Env.User.IsAdmin == false)
                        {
                            // creteria.Add("CHARINDEX((Select CountryID From Factory Where ID in (Select d.Factory From SMNotice_Detail d Where ID = SMNotice.ID and d.Type in ('P', 'M'))), (Select CountryList from Pass1 where ID = '" + Env.User.UserID + "'), 1) > 0");
                            creteria.Add(@"
Exists (
	Select 1 
	From dbo.SplitString((Select CountryList from Pass1 WITH (NOLOCK) where ID = '" + Env.User.UserID + @"'), ',') x
	inner Join Factory f WITH (NOLOCK)  on f.CountryID = x.Data
	Inner Join SMNotice_Detail smd WITH (NOLOCK)  on smd.ID = SMNotice.ID and smd.Type in ('P', 'M') and smd.Factory = f.ID
)
");
                        }

                        break;
                    case SMNoticeCategoryEnum.IE:
                        statusFieldName = "StatusIE";

                        // 如果不是Admin，就要拿國別來篩選目前使用者能不能看到
                        if (Env.User.IsAdmin == false)
                        {
                            creteria.Add(@"
Exists (
	Select 1 
	From dbo.SplitString((Select CountryList from Pass1 WITH (NOLOCK) where ID = '" + Env.User.UserID + @"'), ',') x
	inner Join Factory f WITH (NOLOCK)  on f.CountryID = x.Data
	Inner Join SMNotice_Detail smd WITH (NOLOCK)  on smd.ID = SMNotice.ID and smd.Type in ('I') and smd.Factory = f.ID
)
");
                        }

                        break;
                    case SMNoticeCategoryEnum.Main:
                        statusFieldName = "Status";

                        // 如果不是Admin，就要拿BrandID來篩選目前使用者能不能看到
                        // if (Env.User.IsAdmin == false)
                        // {
                        //    //有特殊權限(AllCustomer)不用檢查BrandID
                        //    bool IsAllCustomer = AuthPrg.hasSpecialAuth("CUST");
                        //    if (IsAllCustomer == false)
                        //        creteria.Add("SMNotice.BrandID In (Select BrandID From PASS_AuthBrand Where ID = '" + Env.User.UserID + "')");
                        // }
                        break;
                    default:
                        throw new NotImplementedException();
                }

                // 如果下拉選單選ALL，就不做特別篩選，狀態不為空就好，如果不是選ALL，就依照下拉選單所選來篩選
                if (queryfors.SelectedIndex == 0)
                {
                    creteria.Add("SMNotice." + statusFieldName + " <> ''");
                }
                else
                {
                    creteria.Add(("SMNotice." + statusFieldName + " = '" + queryfors.SelectedValue as string) + "'");
                }

                form.DefaultWhere = string.Join(" and ", creteria.ToArray());

                // 每次下拉選單有變動，都會清除當前已經設定的Express
                form.QueryExpress = string.Empty;
            });
            SharedQueryForsSetup("SMNoticeStatus", queryfors, showErr, reloadDatas, resetDefaultWhere);
        }

        /// <summary>
        /// (Pattern.P02 - Pattern List)建立畫面上的UseFor下拉選單，並且處理他的SelectIndexChanged
        /// </summary>
        /// <param name="form">Input1</param>
        /// <param name="queryfors">ComboBox</param>
        /// <param name="showErr"><![CDATA[Func<Ict.DualResult, bool>]]></param>
        /// <param name="reloadDatas"><![CDATA[Func<Ict.DualResult>]]></param>
        public static void SetupQueryFors_Pattern(Win.Tems.Input1 form, ComboBox queryfors, Func<DualResult, bool> showErr, Func<DualResult> reloadDatas)
        {
            var resetDefaultWhere = new Action(() =>
            {
                var statusFieldName = (string)queryfors.SelectedValue;
                var creteria = new List<string>();

                // 如果不是Admin，就要拿國別來篩選目前使用者能不能看到
                if (Env.User.IsAdmin == false)
                {
                    creteria.Add("CHARINDEX((Select CountryID From Factory Where ID = Pattern.ActFtyPattern), (Select CountryList from Pass1 where ID = '" + Env.User.UserID + "'), 1) > 0");
                }

                if (statusFieldName == string.Empty)
                {
                    creteria.Add("pattern.Status <> ''");
                }
                else if (statusFieldName == "Completed/History")
                {
                    creteria.Add("pattern.Status in ('Completed', 'History')");
                }
                else
                {
                    creteria.Add("pattern.Status = '" + statusFieldName + "'");
                }

                if (creteria.Any())
                {
                    form.DefaultWhere = string.Join(" and ", creteria.ToArray());
                }
                else
                {
                    form.DefaultWhere = string.Empty;
                }

                // 每次下拉選單有變動，都會清除當前已經設定的Express
                form.QueryExpress = string.Empty;
            });
            SharedQueryForsSetup("PatternStatus", queryfors, showErr, reloadDatas, resetDefaultWhere);
        }

        /// <summary>
        /// (Pattern.P03 - Pattern List)建立畫面上的UseFor下拉選單，並且處理他的SelectIndexChanged
        /// </summary>
        /// <param name="form">Input1</param>
        /// <param name="queryfors">ComboBox</param>
        /// <param name="showErr"><![CDATA[Func<Ict.DualResult, bool>]]></param>
        /// <param name="reloadDatas"><![CDATA[Func<Ict.DualResult>]]></param>
        public static void SetupQueryFors_Marker(Win.Tems.Input1 form, ComboBox queryfors, Func<DualResult, bool> showErr, Func<DualResult> reloadDatas)
        {
            var resetDefaultWhere = new Action(() =>
            {
                var statusFieldName = (string)queryfors.SelectedValue;
                var creteria = new List<string>();

                // 如果不是Admin，就要拿國別來篩選目前使用者能不能看到
                if (Env.User.IsAdmin == false)
                {
                    creteria.Add("CHARINDEX((Select CountryID From Factory Where ID = Marker.ActFtyMarker), (Select CountryList from Pass1 where ID = '" + Env.User.UserID + "'), 1) > 0");
                }

                if (statusFieldName == string.Empty)
                {
                    creteria.Add("Marker.Status <> ''");
                }
                else if (statusFieldName == "Completed/History")
                {
                    creteria.Add("Marker.Status in ('Completed', 'History')");
                }
                else
                {
                    creteria.Add("Marker.Status = '" + statusFieldName + "'");
                }

                if (creteria.Any())
                {
                    form.DefaultWhere = string.Join(" and ", creteria.ToArray());
                }
                else
                {
                    form.DefaultWhere = string.Empty;
                }

                // 每次下拉選單有變動，都會清除當前已經設定的Express
                form.QueryExpress = string.Empty;
            });
            SharedQueryForsSetup("PatternStatus", queryfors, showErr, reloadDatas, resetDefaultWhere);
        }

        /// <summary>
        /// 共用部分，主要是抓DropDownList裡面的東西來給QueryFors下拉選單使用
        /// </summary>
        /// <param name="dropdownType">DropDownList.Type</param>
        /// <param name="queryfors">ComboBox</param>
        /// <param name="showErr"><![CDATA[Func<Ict.DualResult, bool>]]></param>
        /// <param name="reloadDatas"><![CDATA[Func<Ict.DualResult>]]></param>
        /// <param name="resetDefaultWhere">Action</param>
        private static void SharedQueryForsSetup(string dropdownType, ComboBox queryfors, Func<DualResult, bool> showErr, Func<DualResult> reloadDatas, Action resetDefaultWhere)
        {
            using (var drQueryFor = DBProxy.Current.SelectEx("SELECT ID, Name FROM DropDownList Where Type = '" + dropdownType + "' ORDER BY ID"))
            {
                if (drQueryFor == false)
                {
                    showErr(drQueryFor.InnerResult);
                    return;
                }

                drQueryFor.DisposeExtendedData = false;
                var dtStatus = drQueryFor.ExtendedData;
                var dr = dtStatus.NewRow();
                dr["Name"] = "All";
                dr["ID"] = string.Empty;
                dtStatus.Rows.InsertAt(dr, 0); // 固定放0位置，方便resetDefaultWhere裡面的步驟三判斷

                queryfors.DataSource = dtStatus;
                queryfors.DisplayMember = "Name";
                queryfors.ValueMember = "ID";
                queryfors.SelectedValue = "New";
                queryfors.Name = "Status";
                resetDefaultWhere();
            }

            queryfors.SelectedIndexChanged += (s, e) =>
            {
                if (((ComboBox)s).SelectedIndex == -1)
                {
                    return;
                }

                resetDefaultWhere();
                reloadDatas();
            };
        }

        #endregion

        #region ChangeFactory

        /// <summary>
        /// 呼叫來源
        /// </summary>
        public enum ChangeFactoryEnum
        {
            /// <summary>
            /// SampleP01
            /// </summary>
            SampleP01,

            /// <summary>
            /// IeP01
            /// </summary>
            IeP01,

            /// <summary>
            /// IeP02
            /// </summary>
            IeP02,

            /// <summary>
            /// IeP03
            /// </summary>
            IeP03,
        }

        /// <summary>
        /// 變更工廠的結果
        /// </summary>
        public enum ChangeFactoryResultEnum
        {
            /// <summary>
            /// 更新了Pattern or Marker or IETMS
            /// </summary>
            UpdateActFactory,

            /// <summary>
            /// 更新了SMNotice_Detial
            /// </summary>
            UpdateAssignFactory,

            /// <summary>
            /// 更新失敗
            /// </summary>
            Fail,
        }

        /// <summary>
        /// 負責樣品生產任務的工廠變更(Sample.P01, Ie.P01, Ie.P02, Ie.P03 共用)
        /// </summary>
        /// <param name="callerType">ChangeFactoryEnum</param>
        /// <param name="callerRow">DataRow</param>
        /// <param name="newFactoryId">string</param>
        /// <returns>ChangeFactoryResultEnum</returns>
        public static ChangeFactoryResultEnum ChangeFactory(ChangeFactoryEnum callerType, DataRow callerRow, string newFactoryId)
        {
            var id = callerRow.Field<string>("ID");
            var originalFactoryId = string.Empty;
            ChangeFactoryResultEnum updateType;
            using (var scope = new System.Transactions.TransactionScope())
            using (var helper = new AutoUpdator())
            {
                var dtSMNotice = helper.LoadReferenceTable("Select ID, StatusPattern, StatusIE From SMNotice Where ID = @ID", "SMNotice", "ID", id);

                var rowSMNotice = dtSMNotice.AsEnumerable().FirstOrDefault();
                if (rowSMNotice == null)
                {
                    scope.Dispose();
                    MyUtility.Msg.ErrorBox("can't find SMNotice data.(" + id + ")");
                    return ChangeFactoryResultEnum.Fail;
                }

                switch (callerType)
                {
                    case ChangeFactoryEnum.SampleP01: // 從Sample.P01呼叫，有可能已經Confirm=>(Update Pattern/Marker)，有可能還沒有Confirm=>(Update SMNotice_Detail Type in P, M)
                        {
                            var isApproved = rowSMNotice.Field<string>("StatusPattern") == "Approved";
                            if (isApproved)
                            {
                                var dtPattern = helper.LoadTable("Select ID, Version, ActFtyPattern, EditName, EditDate From Pattern Where ID = @ID And Version = (Select max(Version) From Pattern Where ID = @ID)", "ID", id);
                                var dtMarker = helper.LoadTable("Select ID, Version, ActFtyMarker, EditName, EditDate From Marker Where ID = @ID And Version = (Select max(Version) From Marker Where ID = @ID)", "ID", id);

                                if (dtPattern.Rows.Count != 0)
                                {
                                    originalFactoryId = dtPattern.Rows[0].Field<string>("ActFtyPattern");
                                }
                                else if (dtMarker.Rows.Count != 0)
                                {
                                    originalFactoryId = dtMarker.Rows[0].Field<string>("ActFtyMarker");
                                }
                                else
                                {
                                    scope.Dispose();
                                    MyUtility.Msg.ErrorBox("can't find Pattern/Marker data.(" + id + ")");
                                    return ChangeFactoryResultEnum.Fail;
                                }

                                dtMarker.AsEnumerable().ToList().ForEach(row =>
                                {
                                    row["ActFtyMarker"] = newFactoryId;
                                    UIClassPrg.ModifyRecords(row);
                                });
                                dtPattern.AsEnumerable().ToList().ForEach(row =>
                                {
                                    row["ActFtyPattern"] = newFactoryId;
                                    UIClassPrg.ModifyRecords(row);
                                });
                                updateType = ChangeFactoryResultEnum.UpdateActFactory;
                            }
                            else
                            {
                                var dtSMNotice_Detail = helper.LoadTable("Select ID, Type, Factory, EditName, EditDate From SMNotice_Detail Where ID = @ID and Type in ('P', 'M')", "ID", id);
                                if (dtSMNotice_Detail.Rows.Count == 0)
                                {
                                    scope.Dispose();
                                    MyUtility.Msg.ErrorBox("can't find SMNotice_Detail data.(" + id + ")");
                                    return ChangeFactoryResultEnum.Fail;
                                }

                                originalFactoryId = dtSMNotice_Detail.Rows[0].Field<string>("Factory");
                                dtSMNotice_Detail.AsEnumerable().ToList().ForEach(row =>
                                {
                                    row["Factory"] = newFactoryId;
                                    UIClassPrg.ModifyRecords(row);
                                });
                                updateType = ChangeFactoryResultEnum.UpdateAssignFactory;
                            }
                        }

                        break;
                    case ChangeFactoryEnum.IeP01: // 從Ie.P01呼叫，有可能已經Confirm=>(Update IETMS)，有可能還沒有Confirm=>(Update SMNotice_Detail Type=I)
                        {
                            var isApproved = rowSMNotice.Field<string>("StatusIE") == "Approved";
                            if (isApproved)
                            {
                                var dt = helper.LoadTable("Select ID, Version, ActFtyIe, EditName, EditDate From IETMS Where ID = @ID And Version = (Select max(Version) From IETMS Where ID = @ID)", "ID", id);

                                if (dt.Rows.Count != 0)
                                {
                                    originalFactoryId = dt.Rows[0].Field<string>("ActFtyIe");
                                }
                                else
                                {
                                    scope.Dispose();
                                    MyUtility.Msg.ErrorBox("can't find IETMS data.(" + id + ")");
                                    return ChangeFactoryResultEnum.Fail;
                                }

                                dt.AsEnumerable().ToList().ForEach(row =>
                                {
                                    row["ActFtyIe"] = newFactoryId;
                                    UIClassPrg.ModifyRecords(row);
                                });
                                updateType = ChangeFactoryResultEnum.UpdateActFactory;
                            }
                            else
                            {
                                var dtSMNotice_Detail = helper.LoadTable("Select ID, Type, Factory, EditName, EditDate From SMNotice_Detail Where ID = @ID and Type = 'I'", "ID", id);
                                if (dtSMNotice_Detail.Rows.Count == 0)
                                {
                                    scope.Dispose();
                                    MyUtility.Msg.ErrorBox("can't find SMNotice_Detail data.(" + id + ")");
                                    return ChangeFactoryResultEnum.Fail;
                                }

                                originalFactoryId = dtSMNotice_Detail.Rows[0].Field<string>("Factory");
                                dtSMNotice_Detail.AsEnumerable().ToList().ForEach(row =>
                                {
                                    row["Factory"] = newFactoryId;
                                    UIClassPrg.ModifyRecords(row);
                                });
                                updateType = ChangeFactoryResultEnum.UpdateAssignFactory;
                            }
                        }

                        break;
                    case ChangeFactoryEnum.IeP02:
                    case ChangeFactoryEnum.IeP03: // Update IETMS
                        {
                            var version = callerRow.Field<string>("Version");
                            var dt = helper.LoadTable("Select ID, Version, ActFtyIE, EditName, EditDate From IETMS Where ID = @ID And Version = @Version", "ID", id, "Version", version);
                            if (dt.Rows.Count == 0)
                            {
                                scope.Dispose();
                                MyUtility.Msg.ErrorBox("can't find IETMS data.(" + id + ")");
                                return ChangeFactoryResultEnum.Fail;
                            }

                            var row = dt.Rows[0];
                            originalFactoryId = row.Field<string>("ActFtyIE");
                            row["ActFtyIE"] = newFactoryId;
                            UIClassPrg.ModifyRecords(row);
                            updateType = ChangeFactoryResultEnum.UpdateActFactory;
                        }

                        break;
                    default:
                        throw new NotImplementedException();
                }

                try
                {
                    helper.UpdateAllTable();
                    scope.Complete();
                    return updateType;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    MyUtility.Msg.ErrorBox("update factory failed: " + ex.Message);
                    return ChangeFactoryResultEnum.Fail;
                }
            }
        }

        #endregion

        #region Print SMNotice

        /// <summary>
        /// EnuPrintSMType
        /// </summary>
        public enum EnuPrintSMType
        {
            /// <summary>
            /// SMNotice
            /// </summary>
            SMNotice,

            /// <summary>
            /// Order
            /// </summary>
            Order,
        }

        /// <summary>
        /// 列印SMNotice報表
        /// </summary>
        /// <param name="iD">string</param>
        /// <param name="enuType">EnuPrintSMType</param>
        public static void PrintSMNotice(string iD, EnuPrintSMType enuType = EnuPrintSMType.SMNotice)
        {
            // var xltFolder = Sci.Production.Class.Commons.TradeSystem.Env.XltPathDir;
            var xltFolder = Env.Cfg.XltPathDir;
            var xltPath = System.IO.Path.Combine(xltFolder, "PPIC_Pattern-P01_PrintSMnotice.xlt");

            if (System.IO.File.Exists(xltPath) == false)
            {
                MyUtility.Msg.WarningBox("can't find template file (" + xltPath + ")");
                return;
            }

            var tmpFileName = System.IO.Path.GetTempFileName();
            tmpFileName = System.IO.Path.ChangeExtension(tmpFileName, ".xlt");
            System.IO.File.Copy(xltPath, tmpFileName, true);
            var app = new MsExcel.Application();
            MsExcel.Workbook book = null;
            try
            {
                app.DisplayAlerts = false;
                book = app.Workbooks.Open(tmpFileName);
#if DEBUG
                app.Visible = true;
#endif
                var mainSheet = book.Worksheets[1] as MsExcel.Worksheet;

                var pageHBreakList = new List<int>(); // 換頁符號的水平插入點，基本上P2的時候換一次，之後的TrimCard每頁換一次

                // 依照Type移除Title共用的部分
                if (enuType == EnuPrintSMType.SMNotice)
                {
                    mainSheet.get_Range("A9:AC13").Delete();
                }
                else
                {
                    mainSheet.get_Range("A3:AC9").Delete();
                }

                int rowPosition = 1; // 各責任區域輪流使用
                PrintSMNoticeBlock1(mainSheet, iD, ref rowPosition, enuType); // Block1: SMNotice/Style
                PrintSMNoticeBlock2(mainSheet, iD, ref rowPosition, enuType); // Block2: BOF-ColorCombo
                PrintSMNoticeBlock3(mainSheet, iD, ref rowPosition, enuType); // Block3: BOF/Fabric
                PrintSMNoticeBlock4(mainSheet, iD, ref rowPosition, enuType); // Block4: BOa-ColorCombo
                PrintSMNoticeBlock5(mainSheet, iD, ref rowPosition, enuType); // Block5: MixColor table
                PrintSMNoticeBlock6(mainSheet, iD, ref rowPosition, enuType); // Block6: Tape
                PrintSMNoticeBlock7(mainSheet, iD, ref rowPosition, enuType); // Block7: BOA/Accessory
                PrintSMNoticeBlock8(mainSheet, iD, ref rowPosition, enuType); // Block8: Order Qty BreakDown
                PrintSMNoticeBlock9(mainSheet, iD, ref rowPosition, enuType); // Block9: MR/SMR info
                pageHBreakList.Add(rowPosition);
                PrintSMNoticeBlock10(mainSheet, iD, ref rowPosition, enuType); // Block10: formal table for sign
                PrintSMNoticeBlock11(mainSheet, iD, ref rowPosition, pageHBreakList, enuType); // Block11: FabricColor TrimCard

                // 把分業符號整理好，自動產生的移除，加入指定位置的分頁設定
                // mainSheet.PageSetup.PrintArea = "$A$1:$AC$" + rowPosition;
                // while (mainSheet.VPageBreaks.Count > 0)
                // {
                //    mainSheet.VPageBreaks[1].Delete();
                // }
                // while (mainSheet.HPageBreaks.Count > 1)
                // {
                //    mainSheet.HPageBreaks[1].Delete();
                // }
                // mainSheet.VPageBreaks.Add(mainSheet.Range["AA1"]);
                pageHBreakList.ForEach(hBreakIndex => mainSheet.HPageBreaks.Add(mainSheet.Range["A" + hBreakIndex]));

                mainSheet.Cells[1, 1].Select();
                app.DisplayAlerts = true;
                mainSheet.Protect("SCIMIS919", Type.Missing, Type.Missing, Type.Missing, Type.Missing, true, true, true);

                #region Save & Show Excel
                string strExcelName = MicrosoftFile.GetName("PPIC_Pattern-P01_PrintSMnotice");
                MsExcel.Workbook workbook = app.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                app.Quit();
                Marshal.ReleaseComObject(mainSheet);

                strExcelName.OpenFile();
                #endregion
            }
            finally
            {
                Marshal.ReleaseComObject(book);
                Marshal.ReleaseComObject(app);
                book = null;
                app = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            // MyUtility.Msg.InfoBox("print complete");
        }

        /// <summary>
        /// Block1: SMNotice/Style
        /// </summary>
        /// <param name="sheet">Worksheet</param>
        /// <param name="iD">string</param>
        /// <param name="rowPosition">int</param>
        /// <param name="enuType">EnuPrintSMType</param>
        /// <returns>bool</returns>
        private static bool PrintSMNoticeBlock1(MsExcel.Worksheet sheet, string iD, ref int rowPosition, EnuPrintSMType enuType = EnuPrintSMType.SMNotice)
        {
            if (enuType == EnuPrintSMType.SMNotice)
            {
                var sql = @"
select sm.ID, sm.BrandID, sm.PatternNo, sm.SizeCode, Convert(varchar(10), sm.AddDate, 111) AddDate
, sm.StyleID, sm.SeasonID, phase.PhaseID, sm.OrderID, oa.RequireDate
, ob.BasicPattern, s.CdCodeID, s.ProgramID
, s.Description, nc.PatternMaker
, ne.Remark1, nd.MarkerMaker
, nf.Remark2, sm.ProductionFactory
From SMNotice sm
Left Join Style s on s.Ukey = sm.StyleUkey
outer apply (Select Stuff((select distinct ',' + PhaseID from SMNotice_Detail smd where smd.ID = sm.ID for xml path('')), 1, 1, '') as PhaseID) phase
outer apply (Select Replace(Convert(varchar(10), Max(RequireDate)), '-', '/') RequireDate from SMNotice_Detail smd where smd.ID = sm.ID) oa
outer apply (Select Stuff((select ',' + BasicPattern from SMNotice_Detail smd where smd.ID = sm.ID and IsNull(BasicPattern, '') <> '' for xml path('')), 1, 1, '') as BasicPattern) ob
outer apply (select m.ActFtyPattern as PatternMaker from Pattern m where m.ID = sm.ID and m.Version = (select max(version) From Pattern Where ID = sm.ID)) nc
outer apply (select m.ActFtyMarker as MarkerMaker from Marker m where m.ID = sm.ID and m.Version = (select max(version) From Marker Where ID = sm.ID)) nd
outer apply (Select Stuff((select ',' + Remark1 from SMNotice_Detail smd where smd.ID = sm.ID and IsNull(Remark1, '') <> '' for xml path('')), 1, 1, '') as Remark1) ne
outer apply (Select Stuff((select ',' + Remark2 from SMNotice_Detail smd where smd.ID = sm.ID and IsNull(Remark2, '') <> '' for xml path('')), 1, 1, '') as Remark2) nf
where sm.ID = @ID
";
                using (var dr = DBProxy.Current.SelectEx(sql, "ID", iD))
                {
                    if (dr == false)
                    {
                        MyUtility.Msg.WarningBox(dr.ToSimpleString());
                        return false;
                    }

                    var row = dr.ExtendedData.Rows[0];

                    // line1
                    sheet.GetRange("Block1ApplyNo").SetValue(row.Field<string>("ID"));
                    sheet.GetRange("Block1BrandID").SetValue(row.Field<string>("BrandID"));
                    sheet.GetRange("Block1PatternNo").SetValue(row.Field<string>("PatternNo"));
                    sheet.GetRange("Block1SampleSize").SetValue(row.Field<string>("SizeCode"));
                    sheet.GetRange("Block1ApplyDate").SetValue(row.Field<string>("AddDate"));

                    // line2
                    sheet.GetRange("Block1StyleID").SetValue(row.Field<string>("StyleID"));
                    sheet.GetRange("Block1SeasonID").SetValue(row.Field<string>("SeasonID"));
                    sheet.GetRange("Block1PhaseID").SetValue(row.Field<string>("PhaseID"));
                    sheet.GetRange("Block1OrderID").SetValue(row.Field<string>("OrderID"));
                    sheet.GetRange("Block1RequireFinish").SetValue(row.Field<string>("RequireDate"));

                    // line3
                    sheet.GetRange("Block1BasicPattern").SetValue(row.Field<string>("BasicPattern"));
                    sheet.GetRange("Block1CDCodeID").SetValue(row.Field<string>("CdCodeID"));
                    sheet.GetRange("Block1ProgramID").SetValue(row.Field<string>("ProgramID"));

                    // line4
                    sheet.GetRange("Block1Description").SetValue(row.Field<string>("Description"));
                    sheet.GetRange("Block1PatternMaker").SetValue(row.Field<string>("PatternMaker"));

                    // line5
                    sheet.GetRange("Block1Remark1").SetValue(row.Field<string>("Remark1"));
                    sheet.GetRange("Block1MarkerMaker").SetValue(row.Field<string>("MarkerMaker"));

                    // line6
                    sheet.GetRange("Block1Remark2").SetValue(row.Field<string>("Remark2"));
                    sheet.GetRange("Block1ProductionFactory").SetValue(row.Field<string>("ProductionFactory"));
                }

                rowPosition += 9;
            }
            else
            {
                var sql = @"
select o.BrandID, o.OrderTypeID, o.ProgramID
, o.StyleID, o.SeasonID, o.FactoryID, Convert(varchar(10), o.BuyerDelivery, 111) as BuyerDelivery
, s.Description, o.CdCodeID, Convert(varchar(10), o.SciDelivery, 111) as SciDelivery
, op.POComboList as spno
, Convert(varchar(10), o.ChangeMemoDate ,111) as ChangeMemoDate
From Orders o 
inner join Style s on o.StyleUkey = s.Ukey
left join Order_POComboList op on o.POID = op.ID
where o.POID = @ID
";
                using (var dr = DBProxy.Current.SelectEx(sql, "ID", iD))
                {
                    if (dr == false)
                    {
                        MyUtility.Msg.WarningBox(dr.ToSimpleString());
                        return false;
                    }

                    var row = dr.ExtendedData.Rows[0];

                    // line1
                    sheet.GetRange("Block1BrandID_O").SetValue(row.Field<string>("BrandID"));
                    sheet.GetRange("Block1OrderType_O").SetValue(row.Field<string>("OrderTypeID"));
                    sheet.GetRange("Block1ProgramID_O").SetValue(row.Field<string>("ProgramID"));

                    // line2
                    sheet.GetRange("Block1StyleID_O").SetValue(row.Field<string>("StyleID"));
                    sheet.GetRange("Block1SeasonID_O").SetValue(row.Field<string>("SeasonID"));
                    sheet.GetRange("Block1Factory_O").SetValue(row.Field<string>("FactoryID"));
                    sheet.GetRange("Block1ChangeMemoDate_O").SetValue(row.Field<string>("ChangeMemoDate"));
                    sheet.GetRange("Block1Delivery_O").SetValue(row.Field<string>("BuyerDelivery"));

                    // line3
                    sheet.GetRange("Block1Description_O").SetValue(row.Field<string>("Description"));
                    sheet.GetRange("Block1CDCodeID_O").SetValue(row.Field<string>("CdCodeID"));
                    sheet.GetRange("Block1SCIDelivery_O").SetValue(row.Field<string>("SciDelivery"));

                    // line4
                    sheet.GetRange("Block1SPNo_O").SetValue(row.Field<string>("spno"));
                }

                rowPosition += 7;
            }

            return true;
        }

        /// <summary>
        /// Block2: BOF-ColorCombo
        /// </summary>
        /// <param name="sheet">Worksheet</param>
        /// <param name="iD">string</param>
        /// <param name="rowPosition">int</param>
        /// <param name="enuType">EnuPrintSMType</param>
        /// <returns>bool</returns>
        private static bool PrintSMNoticeBlock2(MsExcel.Worksheet sheet, string iD, ref int rowPosition, EnuPrintSMType enuType = EnuPrintSMType.SMNotice)
        {
            var sqlX = string.Empty;
            var sqlY = string.Empty;
            var sqlZ = string.Empty;
            if (enuType == EnuPrintSMType.SMNotice)
            {
                sqlX = @"
Select 
    fabricCode.FabricPanelCode, fabricCode.PatternPanel, bof.FabricCode, q.QTWith 
From SMNotice sm
Left Join Style s on s.Ukey = sm.StyleUkey
Left Join Style_BOF bof on bof.StyleUkey = s.Ukey
Inner Join Style_FabricCode fabricCode on fabricCode.Style_BOFUkey = bof.Ukey
Outer Apply (
    Select Top 1 Concat(fabricCode.FabricCode, fabricCode.PatternPanel) as QTWith
    From Style_FabricCode_QT qt
    Where qt.StyleUkey = sm.StyleUkey
    and qt.FabricPanelCode = fabricCode.FabricPanelCode
	and qt.SeqNO > (Select distinct SeqNO From Style_FabricCode_QT QR where QR.StyleUkey = qt.StyleUkey and QR.QTFabricPanelCode = fabricCode.FabricPanelCode)
	Order by qt.SeqNO
) as q
Where sm.ID = @ID
order by case when fabricCode.FabricPanelCode like 'A%' then 1 else 2 end, fabricCode.FabricPanelCode
";

                sqlY = @"
Select article.Article
	From SMNotice sm
	Left Join Style s on s.Ukey = sm.StyleUkey
	Left Join Style_Article article on article.StyleUkey = s.Ukey
	where sm.ID = @ID
";

                sqlZ = @"
Select color.Article, color.FabricCode, color.FabricPanelCode, color.ColorID
, Cast(iif(xc.VividCnt is null, 0, iif(xc.VividCnt > 0, 1, 0)) as bit) as VIVID
	From SMNotice sm
	Left Join Style s on s.Ukey = sm.StyleUkey
	Left Join Style_ColorCombo color on color.StyleUkey = s.Ukey and FabricCode <> ''
    Left Join Color c on c.ID = color.ColorID and c.BrandId = s.BrandID
    outer apply (
	    Select Count(*) as VividCnt 
	    From Color_multiple cm
	    Inner Join Color c2 on c2.ID = cm.ColorID and c2.BrandId = cm.BrandID and c2.VIVID =1
	    where cm.ColorUkey = c.Ukey) xc
	where sm.ID = @ID
";
            }
            else
            {
                sqlX = @"
Select fabricCode.FabricPanelCode, fabricCode.PatternPanel, bof.FabricCode, q.QTWith 
	From Orders o
	inner Join Order_BOF bof on bof.ID = o.ID
	inner Join Order_FabricCode fabricCode on fabricCode.Order_BOFUkey = bof.Ukey
Outer Apply (
    Select Top 1 Concat(fabricCode.FabricCode, fabricCode.PatternPanel) as QTWith
    From Order_FabricCode_QT qt
    Where qt.Id = o.ID
    and qt.FabricPanelCode = fabricCode.FabricPanelCode
	and qt.SeqNO > (Select distinct SeqNO From Order_FabricCode_QT QR where QR.Id = qt.Id and QR.QTFabricPanelCode = fabricCode.FabricPanelCode)
	Order by qt.SeqNO
) as q
Where o.ID = @ID
order by case when fabricCode.FabricPanelCode like 'A%' then 1 else 2 end, fabricCode.FabricPanelCode
";

                sqlY = @"
Select distinct article.Article
	From Orders o
	Left Join Order_Article article on article.id = o.ID
	where o.POID = @ID
";

                sqlZ = @"
Select distinct color.Article, color.FabricCode, color.FabricPanelCode, color.ColorID
, Cast(iif(xc.VividCnt is null, 0, iif(xc.VividCnt > 0, 1, 0)) as bit) as VIVID
	From Orders o
	Left Join Order_ColorCombo color on color.Id = o.ID and FabricCode <> ''
    Left Join Color c on c.ID = color.ColorID and c.BrandId = o.BrandID
    outer apply (
	    Select Count(*) as VividCnt 
	    From Color_multiple cm
	    Inner Join Color c2 on c2.ID = cm.ColorID and c2.BrandId = cm.BrandID and c2.VIVID =1
	    where cm.ColorUkey = c.Ukey) xc
	where o.POID = @ID
";
            }

            using (var drX = DBProxy.Current.SelectEx(sqlX, "ID", iD)) // 抓X軸
            using (var drY = DBProxy.Current.SelectEx(sqlY, "ID", iD)) // 抓Y軸
            using (var drZ = DBProxy.Current.SelectEx(sqlZ, "ID", iD)) // 抓Z軸
            {
                if (drX == false)
                {
                    MyUtility.Msg.WarningBox(drX.ToSimpleString());
                    return false;
                }

                if (drY == false)
                {
                    MyUtility.Msg.WarningBox(drY.ToSimpleString());
                    return false;
                }

                if (drZ == false)
                {
                    MyUtility.Msg.WarningBox(drZ.ToSimpleString());
                    return false;
                }

                if (drX.ExtendedData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Pattern panel not key in");
                    return true;
                }

                var dataX = drX.ExtendedData
                    .AsEnumerable()
                    .Select(row => new
                    {
                        FabricPanelCode = row.Field<string>("FabricPanelCode"),
                        PatternPanel = row.Field<string>("PatternPanel"),
                        FabricCode = row.Field<string>("FabricCode"),
                        QTWith = row.Field<string>("QTWith"),
                    })
                    .ToDictionary(item => item.FabricPanelCode, item => item);
                var dataY = drY.ExtendedData
                    .AsEnumerable()
                    .Select(row => row.Field<string>("Article"))
                    .ToList();
                var dataZ = drZ.ExtendedData
                    .AsEnumerable()
                    .Select(row => new
                    {
                        Article = row.Field<string>("Article"),
                        FabricPanelCode = row.Field<string>("FabricPanelCode"),
                        ColorID = row.Field<string>("ColorID"),
                    })
                    .ToDictionary(
                        item => new Tuple<string, string>(item.Article, item.FabricPanelCode),
                        item => item.ColorID);

                // 找出需要改為灰底的欄位
                var vividColors = drZ.ExtendedData.AsEnumerable().Where(row => row.Field<bool>("VIVID") == true).Select(row => row.Field<string>("ColorID")).ToList();

                #region ColorCombo-Fabric
                {
                    var linesOfExcel = new List<IEnumerable<object>>();

                    // 第一行要印Art# 和有使用到的FabricPanelCode
                    linesOfExcel.Add(
                        new[] { (object)"Art#" }.Concat(dataX.Select(pair => pair.Key)));

                    // 第二行要印PatternPanel
                    linesOfExcel.Add(
                        new[] { (object)"Pattern Panel" }.Concat(dataX.Select(pair => pair.Value.PatternPanel)));

                    // 接著把Article，串上屬於他的Color
                    dataY.ToList().ForEach(article =>
                    {
                        var colorsOfThisArticle = dataX.Select(pair =>
                        {
                            var key = new Tuple<string, string>(article, pair.Key);
                            if (dataZ.ContainsKey(key))
                            {
                                return dataZ[key];
                            }
                            else
                            {
                                return string.Empty;
                            }
                        }).ToList();
                        linesOfExcel.Add(new[] { (object)article }.Concat(colorsOfThisArticle));
                    });

                    // 倒數第二行要印FabricCode
                    linesOfExcel.Add(
                        new[] { (object)"FabricCode" }.Concat(dataX.Select(pair => pair.Value.FabricCode)));

                    // 倒數第一行要印QTWith
                    linesOfExcel.Add(
                        new[] { (object)"QT with" }.Concat(dataX.Select(pair => pair.Value.QTWith)));

                    // 最後變成string[][]，再轉為object[,]準備放給ExlceRange.Value2
                    var rangeValue2 = linesOfExcel.DoubleArrayConvert2DArray();

                    // 尋找要改變顏色的欄位的座標
                    var cellLocationsForVividColor = new List<Point>();
                    for (int y = 0; y < rangeValue2.GetLength(0); y++)
                    {
                        for (int x = 0; x < rangeValue2.GetLength(1); x++)
                        {
                            var value = (string)rangeValue2[y, x];
                            if (string.IsNullOrWhiteSpace(value) == false &&
                                vividColors.Contains(value))
                            {
                                cellLocationsForVividColor.Add(new Point(x, y));
                            }
                        }
                    }

                    // 關於第二區塊的WorkSheet
                    var thisSheet = (sheet.Parent as MsExcel.Workbook).Worksheets.get_Item("P1B2") as MsExcel.Worksheet;

                    try
                    {
                        // ======Excel動態生成區塊開始=======
                        // column的動態新增:
                        var columnNeeded = rangeValue2.GetLength(1);
                        if (columnNeeded > 26)
                        {
                            thisSheet.GetRange(2, 2, 2, 4).Copy();
                            thisSheet.GetRange(28, 2, 28 + columnNeeded - 28, 4).PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                        }

                        // row的動態新增:
                        if (dataY.Count > 1)
                        {
                            // 將動態的行空間製造出來(第四行是TemplateRow，所以從第5行開始加，並且少加入1行，因為到時候會連TemplateRow一起放值用掉)
                            thisSheet.Range[thisSheet.Rows[5], thisSheet.Rows[5 + dataY.Count - 2]].Insert();

                            // 把Template行複製給剛剛製作出來的空間
                            thisSheet.Rows[4].Copy();
                            thisSheet.Range[thisSheet.Rows[5], thisSheet.Rows[5 + dataY.Count - 2]].PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                        }

                        // 把值放給剛剛動態製作出來的空間 (從Row2開始放值)
                        var leftTopCell = thisSheet.Cells[2, 1];
                        var rightBottomCell = thisSheet.Cells[2 + rangeValue2.GetLength(0) - 1, 1 + rangeValue2.GetLength(1) - 1];
                        thisSheet.Range[leftTopCell, rightBottomCell].Value2 = rangeValue2;

                        // 開始把VIVID的格子變色
                        cellLocationsForVividColor.ToList().ForEach(point =>
                        {
                            thisSheet.Cells[point.Y + 2, point.X + 1].Interior.Color = Color.FromArgb(239, 169, 64);
                        });

                        // VIVID顏色設定灰底
                        // ======Excel動態生成區塊結束=======

                        // 搬回去給主WorkSheet
                        MoveSubBlockIntoMainSheet(sheet, ref rowPosition, thisSheet);
                    }
                    finally
                    {
                        thisSheet.Delete();
                        Marshal.ReleaseComObject(thisSheet);
                        thisSheet = null;
                    }
                }
                #endregion

                #region Washing Fabric Swatch
                {
                    var linesOfExcel = new List<IEnumerable<object>>();

                    // 第一行要印Art# 和有使用到的SizeCode (Washing Fabric Swatch只需要印第一個SizeCode)
                    linesOfExcel.Add(
                        new[] { (object)"Art#" }.Concat(dataX.Take(1).Select(pair => pair.Key)));

                    // 接著把Article，串上屬於他的Color (Washing Fabric Swatch只需要印第一個SizeCode)
                    dataY.ToList().ForEach(article =>
                    {
                        var colorsOfThisArticle = dataX.Take(1).Select(pair =>
                        {
                            var key = new Tuple<string, string>(article, pair.Key);
                            if (dataZ.ContainsKey(key))
                            {
                                return dataZ[key];
                            }
                            else
                            {
                                return string.Empty;
                            }
                        }).ToList();
                        linesOfExcel.Add(new[] { (object)article }.Concat(colorsOfThisArticle));
                    });

                    // 最後變成string[][]，再轉為object[,]準備放給ExlceRange.Value2
                    var rangeValue2 = linesOfExcel.DoubleArrayConvert2DArray();

                    // 尋找要改變顏色的欄位的座標
                    var cellLocationsForVividColor = new List<Point>();
                    for (int y = 0; y < rangeValue2.GetLength(0); y++)
                    {
                        for (int x = 0; x < rangeValue2.GetLength(1); x++)
                        {
                            var value = (string)rangeValue2[y, x];
                            if (string.IsNullOrWhiteSpace(value) == false &&
                                vividColors.Contains(value))
                            {
                                cellLocationsForVividColor.Add(new Point(x, y));
                            }
                        }
                    }

                    // 關於第Washing Fabric Swatch的WorkSheet(這個區塊不用複製去MainSheet)
                    var thisSheet = (sheet.Parent as MsExcel.Workbook).Worksheets.get_Item("washing fabric swatch") as MsExcel.Worksheet;

                    try
                    {
                        // ======Excel動態生成區塊開始=======
                        // row的動態新增:
                        if (dataY.Count > 1)
                        {
                            // 將動態的行空間製造出來(第3行是TemplateRow，所以從第4行開始加，並且少加入1行，因為到時候會連TemplateRow一起放值用掉)
                            thisSheet.Range[thisSheet.Rows[3], thisSheet.Rows[3 + dataY.Count - 2]].Insert();

                            // 把Template行複製給剛剛製作出來的空間
                            thisSheet.Rows[3].Copy();
                            thisSheet.Range[thisSheet.Rows[4], thisSheet.Rows[4 + dataY.Count - 2]].PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                        }

                        // 把值放給剛剛動態製作出來的空間 (從Row2開始放值)
                        var leftTopCell = thisSheet.Cells[2, 1];
                        var rightBottomCell = thisSheet.Cells[2 + rangeValue2.GetLength(0) - 1, 1 + rangeValue2.GetLength(1) - 1];
                        thisSheet.Range[leftTopCell, rightBottomCell].Value2 = rangeValue2;

                        // 開始把VIVID的格子變色
                        cellLocationsForVividColor.ToList().ForEach(point =>
                        {
                            thisSheet.Cells[point.Y + 2, point.X + 1].Interior.Color = Color.FromArgb(239, 169, 64);
                        });

                        // VIVID顏色設定灰底
                        // ======Excel動態生成區塊結束=======
                    }
                    finally
                    {
                        Marshal.ReleaseComObject(thisSheet);
                        thisSheet = null;
                    }
                }
                #endregion

                return true;
            }
        }

        /// <summary>
        /// Block3: BOF/Fabric
        /// </summary>
        /// <param name="sheet">Worksheet</param>
        /// <param name="iD">string</param>
        /// <param name="rowPosition">int</param>
        /// <param name="enuType">EnuPrintSMType</param>
        /// <returns>bool</returns>
        private static bool PrintSMNoticeBlock3(MsExcel.Worksheet sheet, string iD, ref int rowPosition, EnuPrintSMType enuType = EnuPrintSMType.SMNotice)
        {
            var sql = string.Empty;
            if (enuType == EnuPrintSMType.SMNotice)
            {
                sql = @"
Select fc.FabricPanelCode
	, Concat(
		IsNull(bof.Refno + '; ', ''), 
		IsNull(f.Description, ''), 
		'(', IsNull(Cast(f.width as varchar) + '; ', ''), IsNull(RTrim(f.WeaveTypeID)+ '; ', ''), IsNull(Cast(Convert(numeric(10,4), f.WeightM2) as varchar), ''), 'g)',
		iif(bof.MatchFabric <> 0, 'MatchFabric:' + ddl.Name + '; ', ''), 
		iif(bof.HRepeat <> 0, 'H-Repeat:' + format(bof.HRepeat, '##0.###') + '; ', ''), 
		iif(bof.VRepeat <> 0, 'V-Repeat:' + format(bof.VRepeat, '##0.###') + '; ', ''),
		iif(bof.HorizontalCutting = 1, 'Straight fabric use Horizontal cutting; ', ''),
		iif(bof.OneTwoWay = 2, ' two way cutting;', '')) as MixInfo
From SMNotice sm
inner join Style_BOF bof on bof.StyleUkey = sm.StyleUkey
Left Join Fabric f on f.SCIRefno = bof.SCIRefno
Left Join DropdownList ddl on ddl.Type = 'MatchFabric' and ddl.ID = bof.MatchFabric
Outer Apply(Select Stuff((Select '/' + fc.FabricPanelCode from Style_FabricCode fc where fc.Style_BOFUkey = bof.Ukey for xml path('')), 1, 1, '') as FabricPanelCode) fc
Where sm.ID = @ID
Order by fc.FabricPanelCode
";
            }
            else
            {
                sql = @"
Select fc.FabricPanelCode
	, Concat(
		IsNull(bof.Refno + '; ', ''), 
		IsNull(f.Description, ''), 
		'(', IsNull(Cast(f.width as varchar) + '; ', ''), IsNull(RTrim(f.WeaveTypeID)+ '; ', ''), IsNull(Cast(Convert(numeric(10,4), f.WeightM2) as varchar), ''), 'g)',
		iif(sbof.MatchFabric <> 0, 'MatchFabric:' + ddl.Name + '; ', ''), 
		iif(sbof.HRepeat <> 0, 'H-Repeat:' + format(sbof.HRepeat, '##0.###') + '; ', ''), 
		iif(sbof.VRepeat <> 0, 'V-Repeat:' + format(sbof.VRepeat, '##0.###') + '; ', ''),
		iif(sbof.HorizontalCutting = 1, 'Straight fabric use Horizontal cutting; ', ''),
		iif(sbof.OneTwoWay = 2, ' two way cutting;', '')) as MixInfo
From Orders o
inner join Order_BOF bof on bof.Id = o.ID
Left Join Fabric f on f.SCIRefno = bof.SCIRefno
LEFT JOIN Style_BOF sbof on sbof.StyleUkey = o.StyleUkey and sbof.FabricCode = bof.FabricCode
Left Join DropdownList ddl on ddl.Type = 'MatchFabric' and ddl.ID = sbof.MatchFabric
Outer Apply(Select Stuff((Select '/' + fc.FabricPanelCode from Order_FabricCode fc where fc.Order_BOFUkey = bof.Ukey for xml path('')), 1, 1, '') as FabricPanelCode) fc
Where o.ID = @ID
Order by fc.FabricPanelCode
";
            }

            using (var dr = DBProxy.Current.SelectEx(sql, "ID", iD))
            {
                if (dr == false)
                {
                    MyUtility.Msg.ErrorBox(dr.ToSimpleString());
                    return false;
                }
                else
                {
                    // 關於第三區塊的WorkSheet
                    var thisSheet = (sheet.Parent as MsExcel.Workbook).Worksheets.get_Item("P1B3") as MsExcel.Worksheet;
                    try
                    {
                        var rangeValue = dr.ExtendedData.AsEnumerable().Select(row => new[] { row.Field<string>("FabricPanelCode"), string.Empty, row.Field<string>("MixInfo") }).DoubleArrayConvert2DArray();

                        // 一共有多少筆資料要被放入Block3
                        var recordCount = dr.ExtendedData.Rows.Count;

                        // ======Excel動態生成區塊開始=======
                        if (recordCount > 1)
                        {
                            // 將動態的行空間製造出來(第2行是TemplateRow，所以從第3行開始加，並且少加入1行，因為到時候會連TemplateRow一起放值用掉)
                            thisSheet.Range[thisSheet.Rows[3], thisSheet.Rows[3 + recordCount - 2]].Insert();

                            // 把Template行複製給剛剛製作出來的空間
                            thisSheet.Rows[2].Copy();
                            thisSheet.Range[thisSheet.Rows[3], thisSheet.Rows[3 + recordCount - 2]].PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                        }

                        // 把值放給剛剛動態製作出來的空間 (從Row2開始放值)
                        var leftTopCell = thisSheet.Cells[2, 1];
                        var rightBottomCell = thisSheet.Cells[2 + rangeValue.GetLength(0) - 1, 1 + rangeValue.GetLength(1) - 1];
                        thisSheet.Range[leftTopCell, rightBottomCell].Value2 = rangeValue;

                        // ======Excel動態生成區塊結束=======

                        // 搬回去給主WorkSheet
                        MoveSubBlockIntoMainSheet(sheet, ref rowPosition, thisSheet);

                        return true;
                    }
                    finally
                    {
                        thisSheet.Delete();
                        Marshal.ReleaseComObject(thisSheet);
                        thisSheet = null;
                    }
                }
            }
        }

        /// <summary>
        /// Block4: BOa-ColorCombo
        /// </summary>
        /// <param name="sheet">Worksheet</param>
        /// <param name="iD">string</param>
        /// <param name="rowPosition">int</param>
        /// <param name="enuType">EnuPrintSMType</param>
        /// <returns>bool</returns>
        private static bool PrintSMNoticeBlock4(MsExcel.Worksheet sheet, string iD, ref int rowPosition, EnuPrintSMType enuType = EnuPrintSMType.SMNotice)
        {
            var sqlX = string.Empty;
            var sqlY = string.Empty;
            var sqlZ = string.Empty;
            if (enuType == EnuPrintSMType.SMNotice)
            {
                sqlX = @"
Select * from (
select distinct boa.PatternPanel as FabricPanelCode
From SMNotice sm
Left Join Style_BOA boa on boa.StyleUkey = sm.StyleUkey
Where sm.ID = @ID and IsNull(boa.PatternPanel, '') <> ''
union 
Select distinct color.FabricPanelCode
From SMNotice sm
	inner Join Style_ColorCombo color on color.StyleUkey = sm.StyleUkey and FabricCode = ''
Where sm.ID = @ID)x
order by case when x.FabricPanelCode like 'A%' then 1 else 2 end, x.FabricPanelCode
";
                sqlY = @"
Select article.Article
	From SMNotice sm
	Left Join Style s on s.Ukey = sm.StyleUkey
	Left Join Style_Article article on article.StyleUkey = s.Ukey
	where sm.ID = @ID
";
                sqlZ = @"
Select color.Article, color.FabricCode, color.FabricPanelCode, color.ColorID, color.PatternPanel
, Cast(iif(xc.VividCnt is null, 0, iif(xc.VividCnt > 0, 1, 0)) as bit) as VIVID
	From SMNotice sm
	Left Join Style s on s.Ukey = sm.StyleUkey
	Left Join Style_ColorCombo color on color.StyleUkey = s.Ukey and FabricCode = ''
    Left Join Color c on c.ID = color.ColorID and c.BrandId = s.BrandID
    outer apply (
	    Select Count(*) as VividCnt 
	    From Color_multiple cm
	    Inner Join Color c2 on c2.ID = cm.ColorID and c2.BrandId = cm.BrandID and c2.VIVID =1
	    where cm.ColorUkey = c.Ukey) xc
	where sm.ID = @ID
";
            }
            else
            {
                sqlX = @"
Select * from (
select distinct boa.FabricPanelCode as FabricPanelCode
From Orders o
Left Join Order_BOA boa on boa.Id = o.ID
Where o.ID = @ID and IsNull(boa.FabricPanelCode, '') <> ''
union 
Select distinct color.FabricPanelCode
From Orders o
	inner Join Order_ColorCombo color on color.Id = o.ID and FabricType = 'A'
Where o.POID = @ID)x
order by case when x.FabricPanelCode like 'A%' then 1 else 2 end, x.FabricPanelCode
";

                sqlY = @"
Select distinct article.Article
	From Orders o
	Left Join Order_Article article on article.id = o.ID
	where o.POID = @ID
";
                sqlZ = @"
Select distinct color.Article, color.FabricCode, color.FabricPanelCode, color.ColorID, color.PatternPanel
, Cast(iif(xc.VividCnt is null, 0, iif(xc.VividCnt > 0, 1, 0)) as bit) as VIVID
	From Orders o
	Left Join Order_ColorCombo color on color.Id = o.ID and FabricType = 'A'
    Left Join Color c on c.ID = color.ColorID and c.BrandId = o.BrandID
    outer apply (
	    Select Count(*) as VividCnt 
	    From Color_multiple cm
	    Inner Join Color c2 on c2.ID = cm.ColorID and c2.BrandId = cm.BrandID and c2.VIVID =1
	    where cm.ColorUkey = c.Ukey) xc
	where o.ID = @ID
";
            }

            using (var drX = DBProxy.Current.SelectEx(sqlX, "ID", iD)) // 抓X軸
            using (var drY = DBProxy.Current.SelectEx(sqlY, "ID", iD)) // 抓Y軸
            using (var drZ = DBProxy.Current.SelectEx(sqlZ, "ID", iD)) // 抓Z軸
            {
                // 關於本區塊的WorkSheet
                var thisSheet = (sheet.Parent as MsExcel.Workbook).Worksheets.get_Item("P1B4") as MsExcel.Worksheet;

                try
                {
                    if (drX == false)
                    {
                        MyUtility.Msg.WarningBox(drX.ToSimpleString());
                        return false;
                    }

                    if (drY == false)
                    {
                        MyUtility.Msg.WarningBox(drY.ToSimpleString());
                        return false;
                    }

                    if (drZ == false)
                    {
                        MyUtility.Msg.WarningBox(drZ.ToSimpleString());
                        return false;
                    }

                    // 換成用FabricPanelCode 和 Article當X座標與Y座標的字典備用
                    var dataZ = drZ.ExtendedData
                        .AsEnumerable()
                        .Select(row => new
                        {
                            Article = row.Field<string>("Article"),
                            FabricPanelCode = row.Field<string>("FabricPanelCode"),
                            ColorID = row.Field<string>("ColorID"),
                        })
                        .ToDictionary(
                            item => new Tuple<string, string>(item.Article, item.FabricPanelCode),
                            item => item.ColorID);

                    var dataX = drX.ExtendedData
                        .AsEnumerable()
                        .Select(row => row.Field<string>("FabricPanelCode"))
                        .Distinct()
                        .Where(fabricPanelCode => dataZ.Any(pair => pair.Key.Item2 == fabricPanelCode))
                        .OrderBy(fabricPanelCode => fabricPanelCode.Length == 1 ? "A" + fabricPanelCode : fabricPanelCode)
                        .ToList();

                    var dataY = drY.ExtendedData
                        .AsEnumerable()
                        .Select(row => row.Field<string>("Article"))
                        .ToList();

                    var linesOfExcel = new List<IEnumerable<object>>();

                    // 第一行要印Art# 和有使用到的FabricPanelCode
                    linesOfExcel.Add(
                        new[] { (object)"Art#" }.Concat(dataX.ToArray()));

                    // 找出需要改為灰底的欄位
                    var vividColors = drZ.ExtendedData.AsEnumerable().Where(row => row.Field<bool>("VIVID") == true).Select(row => row.Field<string>("ColorID")).ToList();

                    // 接著把Article，串上屬於他的Color
                    dataY.ToList().ForEach(article =>
                    {
                        var colorsOfThisArticle = dataX.Select(fabricPanelCode =>
                        {
                            var key = new Tuple<string, string>(article, fabricPanelCode);
                            if (dataZ.ContainsKey(key))
                            {
                                return dataZ[key];
                            }
                            else
                            {
                                return string.Empty;
                            }
                        }).ToList();
                        linesOfExcel.Add(new[] { (object)article }.Concat(colorsOfThisArticle));
                    });

                    // 最後變成string[][]，再轉為object[,]準備放給ExlceRange.Value2
                    var rangeValue2 = linesOfExcel.DoubleArrayConvert2DArray();

                    // 尋找要改變顏色的欄位的座標
                    var cellLocationsForVividColor = new List<Point>();
                    for (int y = 0; y < rangeValue2.GetLength(0); y++)
                    {
                        for (int x = 0; x < rangeValue2.GetLength(1); x++)
                        {
                            var value = (string)rangeValue2[y, x];
                            if (string.IsNullOrWhiteSpace(value) == false &&
                                vividColors.Contains(value))
                            {
                                cellLocationsForVividColor.Add(new Point(x, y));
                            }
                        }
                    }

                    // ======Excel動態生成區塊開始=======
                    var columnNeeded = rangeValue2.GetLength(1);
                    if (columnNeeded > 26)
                    {
                        thisSheet.GetRange(2, 2, 2, 4).Copy();
                        thisSheet.GetRange(28, 2, 28 + columnNeeded - 28, 4).PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                    }

                    if (dataY.Count > 1)
                    {
                        // 將動態的行空間製造出來(第3行是TemplateRow，所以從第4行開始加，並且少加入1行，因為到時候會連TemplateRow一起放值用掉)
                        thisSheet.Range[thisSheet.Rows[4], thisSheet.Rows[4 + dataY.Count - 2]].Insert();

                        // 把Template行複製給剛剛製作出來的空間
                        thisSheet.Rows[3].Copy();
                        thisSheet.Range[thisSheet.Rows[4], thisSheet.Rows[4 + dataY.Count - 2]].PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                    }

                    // 把值放給剛剛動態製作出來的空間 (從Row2開始放值)
                    var leftTopCell = thisSheet.Cells[2, 1];
                    var rightBottomCell = thisSheet.Cells[2 + rangeValue2.GetLength(0) - 1, 1 + rangeValue2.GetLength(1) - 1];
                    thisSheet.Range[leftTopCell, rightBottomCell].Value2 = rangeValue2;

                    // 開始把VIVID的格子變色
                    cellLocationsForVividColor.ToList().ForEach(point =>
                    {
                        thisSheet.Cells[point.Y + 2, point.X + 1].Interior.Color = Color.FromArgb(239, 169, 64);
                    });

                    // ======Excel動態生成區塊結束=======

                    // 搬回去給主WorkSheet
                    MoveSubBlockIntoMainSheet(sheet, ref rowPosition, thisSheet);

                    return true;
                }
                finally
                {
                    thisSheet.Delete();
                    Marshal.ReleaseComObject(thisSheet);
                    thisSheet = null;
                }
            }
        }

        /// <summary>
        /// Block5: MixColor table
        /// </summary>
        /// <param name="sheet">Worksheet</param>
        /// <param name="iD">string</param>
        /// <param name="rowPosition">int</param>
        /// <param name="enuType">EnuPrintSMType</param>
        /// <returns>bool</returns>
        private static bool PrintSMNoticeBlock5(MsExcel.Worksheet sheet, string iD, ref int rowPosition, EnuPrintSMType enuType = EnuPrintSMType.SMNotice)
        {
            string sql = string.Empty, sql2 = string.Empty;
            if (enuType == EnuPrintSMType.SMNotice)
            {
                sql = @"
Select Caption, ColorID, ColorList from (
Select 1 as tp, Concat(color.Article, '/', color.FabricPanelCode) as Caption, r.u as ColorList, color.ColorID
	From SMNotice sm
	inner Join Style s on s.Ukey = sm.StyleUkey
	inner Join Style_ColorCombo color on color.StyleUkey = s.Ukey and FabricCode <> ''
	inner Join Color c on c.BrandID = s.BrandID and c.ID = color.ColorID
	outer apply (Select STUFF((Select '|' + ColorID from Color_multiple cm where cm.ColorUkey = c.Ukey order by cm.Seqno for xml path('')), 1, 1, '') u) r
	where sm.ID = @ID
	and Substring(color.ColorID, 1, 1) = 'X'
union all
Select 2 as tp, Concat(color.Article, '/', color.FabricPanelCode) as Caption, r.u, color.ColorID
	From SMNotice sm
	inner Join Style s on s.Ukey = sm.StyleUkey
	inner Join Style_ColorCombo color on color.StyleUkey = sm.StyleUkey and FabricCode = ''
	inner Join Color c on c.BrandID = s.BrandID and c.ID = color.ColorID
	outer apply (Select STUFF((Select '|' + ColorID from Color_multiple cm where cm.ColorUkey = c.Ukey order by cm.Seqno for xml path('')), 1, 1, '') u) r
	where sm.ID = @ID
	and Substring(color.ColorID, 1, 1) = 'X') x
order by x.tp, x.Caption
";
                sql2 = @"
Select c.ID
From SMNotice sm
Inner Join Style s on s.Ukey = sm.StyleUkey
Inner Join Color c on c.BrandID = s.BrandID and c.VIVID = 1
where sm.ID = @ID
";
            }
            else
            {
                sql = @"
Select Caption, ColorID, ColorList from (
Select 1 as tp, Concat(color.Article, '/', color.FabricPanelCode) as Caption, r.u as ColorList, color.ColorID
	From Orders o
	inner Join Order_ColorCombo color on color.Id = o.ID and FabricCode <> ''
	inner Join Color c on c.BrandID = o.BrandID and c.ID = color.ColorID
	outer apply (Select STUFF((Select '|' + ColorID from Color_multiple cm where cm.ColorUkey = c.Ukey order by cm.Seqno for xml path('')), 1, 1, '') u) r
	where o.ID = @ID
	and Substring(color.ColorID, 1, 1) = 'X'
union all
Select 2 as tp, Concat(color.Article, '/', color.FabricPanelCode) as Caption, r.u, color.ColorID
	From Orders o
	inner Join Order_ColorCombo color on color.Id = o.ID and FabricCode = ''
	inner Join Color c on c.BrandID = o.BrandID and c.ID = color.ColorID
	outer apply (Select STUFF((Select '|' + ColorID from Color_multiple cm where cm.ColorUkey = c.Ukey order by cm.Seqno for xml path('')), 1, 1, '') u) r
	where o.ID = @ID
	and Substring(color.ColorID, 1, 1) = 'X') x
order by x.tp, x.Caption
";
                sql2 = @"
Select c.ID
From SMNotice sm
Inner Join Style s on s.Ukey = sm.StyleUkey
Inner Join Color c on c.BrandID = s.BrandID and c.VIVID = 1
where sm.ID = @ID
";
            }

            using (var dr = DBProxy.Current.SelectEx(sql, "ID", iD))
            using (var dr2 = DBProxy.Current.SelectEx(sql2, "ID", iD))
            {
                var allVividColors = dr2.ExtendedData.AsEnumerable().Select(row => row.Field<string>("ID")).ToList();
                var thisSheet = (sheet.Parent as MsExcel.Workbook).Worksheets.get_Item("P1B5") as MsExcel.Worksheet;
                try
                {
                    if (dr == false)
                    {
                        MyUtility.Msg.ErrorBox(dr.ToSimpleString());
                        return false;
                    }
                    else if (dr.ExtendedData.Rows.Count == 0)
                    {
                        return true;
                    }

                    var rangeValue = dr.ExtendedData.AsEnumerable().Select(row => new[] { row.Field<string>("Caption"), string.Empty, row.Field<string>("ColorID") }.Concat(row.Field<string>("ColorList").Split('|'))).DoubleArrayConvert2DArray();

                    // 尋找要改變顏色的欄位的座標
                    var cellLocationsForVividColor = new List<Point>();
                    for (int y = 0; y < rangeValue.GetLength(0); y++)
                    {
                        for (int x = 0; x < rangeValue.GetLength(1); x++)
                        {
                            var value = (string)rangeValue[y, x];
                            if (string.IsNullOrWhiteSpace(value) == false &&
                                allVividColors.Contains(value))
                            {
                                cellLocationsForVividColor.Add(new Point(x, y));
                            }
                        }
                    }

                    // 一共有多少筆資料要被放入
                    var recordCount = dr.ExtendedData.Rows.Count;

                    // ======Excel動態生成區塊開始=======
                    var columnNeeded = rangeValue.GetLength(1);
                    if (columnNeeded > 3)
                    {
                        thisSheet.Range["D1"].Copy();
                        thisSheet.GetRange(5, 1, 5 + columnNeeded - 5, 1).PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                    }

                    var rowNeeded = rangeValue.GetLength(0);
                    if (recordCount > 1)
                    {
                        // 將動態的行空間製造出來(第1行是TemplateRow，所以從第2行開始加，並且少加入1行，因為到時候會連TemplateRow一起放值用掉)
                        thisSheet.Range[thisSheet.Rows[2], thisSheet.Rows[2 + recordCount - 2]].Insert();

                        // 把Template行複製給剛剛製作出來的空間
                        thisSheet.Rows[1].Copy();
                        thisSheet.Range[thisSheet.Rows[2], thisSheet.Rows[2 + recordCount - 2]].PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                    }

                    // 把值放給剛剛動態製作出來的空間 (從Row1開始放值)
                    var areaLeft = 1;
                    var areaTop = 1;
                    thisSheet.GetRange(areaLeft, areaTop, areaLeft + columnNeeded - 1, areaTop + rowNeeded - 1).Value2 = rangeValue;

                    // 開始把VIVID的格子變色
                    cellLocationsForVividColor.ToList().ForEach(point =>
                    {
                        thisSheet.Cells[point.Y + 1, point.X + 1].Interior.Color = Color.FromArgb(239, 169, 64);
                    });

                    // ======Excel動態生成區塊結束=======

                    // 搬回去給主WorkSheet
                    MoveSubBlockIntoMainSheet(sheet, ref rowPosition, thisSheet, 1);
                    return true;
                }
                finally
                {
                    thisSheet.Delete();
                    Marshal.ReleaseComObject(thisSheet);
                    thisSheet = null;
                }
            }
        }

        /// <summary>
        /// Block6: BOA/Accessory
        /// </summary>
        /// <param name="sheet">Worksheet</param>
        /// <param name="iD">string</param>
        /// <param name="rowPosition">int</param>
        /// <param name="enuType">EnuPrintSMType</param>
        /// <returns>bool</returns>
        private static bool PrintSMNoticeBlock6(MsExcel.Worksheet sheet, string iD, ref int rowPosition, EnuPrintSMType enuType = EnuPrintSMType.SMNotice)
        {
            var sql = string.Empty;
            if (enuType == EnuPrintSMType.SMNotice)
            {
                sql = @"
Select Concat(boa.PatternPanel, '[', IsNull(boa.SizeItem, ''), ']:') as Mix1, Concat(IsNull(boa.Refno, ''), ' ', IsNull(f.Description, '')) as Mix2
From SMNotice sm
inner join Style_BOA boa on boa.StyleUkey = sm.StyleUkey
Inner Join Fabric f on f.SCIRefno = boa.SCIRefno
--Inner Join MtlType mt on mt.ID = f.MtltypeId and (mt.ID like '%TAPE' or mt.ID like '%ELASTIC%')
Where 1 = 1
And sm.ID = @ID 
And boa.FabricPanelCode <> '' 
And IsNull(boa.ProvidedPatternRoom, 0) = 1
And IsNull(f.BomTypeCalculate, 0) = 1
Order by boa.FabricPanelCode
";
            }
            else
            {
                sql = @"
Select Concat(boa.FabricPanelCode, '[', IsNull(boa.SizeItem, ''), ']:') as Mix1, Concat(IsNull(boa.Refno, ''), ' ', IsNull(f.Description, '')) as Mix2
From Orders o
Inner join Order_BOA boa on boa.Id = o.ID
Inner Join Fabric f on f.SCIRefno = boa.SCIRefno
--Inner Join MtlType mt on mt.ID = f.MtltypeId and (mt.ID like '%TAPE' or mt.ID like '%ELASTIC%')
Where 1 = 1
And o.ID = @ID 
And boa.FabricPanelCode <> '' 
And IsNull(boa.ProvidedPatternRoom, 0) = 1
And IsNull(f.BomTypeCalculate, 0) = 1
Order by boa.FabricPanelCode
";
            }

            using (var dr = DBProxy.Current.SelectEx(sql, "ID", iD))
            {
                var thisSheet = (sheet.Parent as MsExcel.Workbook).Worksheets.get_Item("P1B6") as MsExcel.Worksheet;
                try
                {
                    if (dr == false)
                    {
                        MyUtility.Msg.ErrorBox(dr.ToSimpleString());
                        return false;
                    }
                    else if (dr.ExtendedData.Rows.Count == 0)
                    {
                        return true;
                    }

                    var rangeValue = dr.ExtendedData.AsEnumerable().Select(row => new[] { row.Field<string>("Mix1"), string.Empty, row.Field<string>("Mix2") }).DoubleArrayConvert2DArray();

                    // 一共有多少筆資料要被放入
                    var recordCount = dr.ExtendedData.Rows.Count;

                    // ======Excel動態生成區塊開始=======
                    if (recordCount > 1)
                    {
                        // 將動態的行空間製造出來(第2行是TemplateRow，所以從第3行開始加，並且少加入1行，因為到時候會連TemplateRow一起放值用掉)
                        thisSheet.Range[thisSheet.Rows[3], thisSheet.Rows[3 + recordCount - 2]].Insert();

                        // 把Template行複製給剛剛製作出來的空間
                        thisSheet.Rows[2].Copy();
                        thisSheet.Range[thisSheet.Rows[3], thisSheet.Rows[3 + recordCount - 2]].PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                    }

                    // 把值放給剛剛動態製作出來的空間 (從Row2開始放值)
                    var leftTopCell = thisSheet.Cells[2, 1];
                    var rightBottomCell = thisSheet.Cells[2 + rangeValue.GetLength(0) - 1, 1 + rangeValue.GetLength(1) - 1];
                    thisSheet.Range[leftTopCell, rightBottomCell].Value2 = rangeValue;

                    // ======Excel動態生成區塊結束=======

                    // 搬回去給主WorkSheet
                    MoveSubBlockIntoMainSheet(sheet, ref rowPosition, thisSheet);
                    return true;
                }
                finally
                {
                    thisSheet.Delete();
                    Marshal.ReleaseComObject(thisSheet);
                    thisSheet = null;
                }
            }
        }

        /// <summary>
        /// Block7: BOA/Accessory
        /// </summary>
        /// <param name="sheet">Worksheet</param>
        /// <param name="iD">string</param>
        /// <param name="rowPosition">int</param>
        /// <param name="enuType">EnuPrintSMType</param>
        /// <returns>bool</returns>
        private static bool PrintSMNoticeBlock7(MsExcel.Worksheet sheet, string iD, ref int rowPosition, EnuPrintSMType enuType = EnuPrintSMType.SMNotice)
        {
            var sql = string.Empty;
            if (enuType == EnuPrintSMType.SMNotice)
            {
                sql = @"
Select boa.PatternPanel + iif(boa.SizeItem <> '', '[' + IsNull(boa.SizeItem, '') + ']:', '') as PatternPanel, Concat(IsNull(boa.Refno + ' ', ''), IsNull(f.Description, '')) as MixInfo
From SMNotice sm
Inner join Style_BOA boa on boa.StyleUkey = sm.StyleUkey
Inner Join Fabric f on f.SCIRefno = boa.SCIRefno
--Inner Join MtlType mt on mt.ID = f.MtltypeId and not (mt.ID like '%TAPE' or mt.ID like '%ELASTIC%')
Where sm.ID = @ID 
and boa.PatternPanel <> '' 
and IsNull(boa.ProvidedPatternRoom, 0) = 1
And IsNull(f.BomTypeCalculate, 0) = 0
Order by boa.PatternPanel
";
            }
            else
            {
                sql = @"
Select boa.FabricPanelCode + iif(boa.SizeItem <> '', '[' + IsNull(boa.SizeItem, '') + ']:', '') as PatternPanel, Concat(IsNull(boa.Refno + ' ', ''), IsNull(f.Description, '')) as MixInfo
From Orders o
Inner join Order_BOA boa on boa.Id = o.ID
Inner Join Fabric f on f.SCIRefno = boa.SCIRefno
--Inner Join MtlType mt on mt.ID = f.MtltypeId and not (mt.ID like '%TAPE' or mt.ID  like '%ELASTIC%')
Where o.ID = @ID 
and boa.FabricPanelCode <> '' 
and IsNull(boa.ProvidedPatternRoom, 0) = 1
And IsNull(f.BomTypeCalculate, 0) = 0
Order by boa.PatternPanel
";
            }

            using (var dr = DBProxy.Current.SelectEx(sql, "ID", iD))
            {
                // 關於第三區塊的WorkSheet
                var thisSheet = (sheet.Parent as MsExcel.Workbook).Worksheets.get_Item("P1B7") as MsExcel.Worksheet;
                try
                {
                    if (dr == false)
                    {
                        MyUtility.Msg.ErrorBox(dr.ToSimpleString());
                        return false;
                    }
                    else if (dr.ExtendedData.Rows.Count == 0)
                    {
                        return true;
                    }

                    var rangeValue = dr.ExtendedData.AsEnumerable().Select(row => new[] { row.Field<string>("PatternPanel"), string.Empty, row.Field<string>("MixInfo") }).DoubleArrayConvert2DArray();

                    // 一共有多少筆資料要被放入Block6
                    var recordCount = dr.ExtendedData.Rows.Count;

                    // ======Excel動態生成區塊開始=======
                    if (recordCount > 1)
                    {
                        // 將動態的行空間製造出來(第2行是TemplateRow，所以從第3行開始加，並且少加入1行，因為到時候會連TemplateRow一起放值用掉)
                        thisSheet.Range[thisSheet.Rows[3], thisSheet.Rows[3 + recordCount - 2]].Insert();

                        // 把Template行複製給剛剛製作出來的空間
                        thisSheet.Rows[2].Copy();
                        thisSheet.Range[thisSheet.Rows[3], thisSheet.Rows[3 + recordCount - 2]].PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                    }

                    // 把值放給剛剛動態製作出來的空間 (從Row2開始放值)
                    var leftTopCell = thisSheet.Cells[2, 1];
                    var rightBottomCell = thisSheet.Cells[2 + rangeValue.GetLength(0) - 1, 1 + rangeValue.GetLength(1) - 1];
                    thisSheet.Range[leftTopCell, rightBottomCell].Value2 = rangeValue;

                    // ======Excel動態生成區塊結束=======

                    // 搬回去給主WorkSheet
                    MoveSubBlockIntoMainSheet(sheet, ref rowPosition, thisSheet, 1);

                    return true;
                }
                finally
                {
                    thisSheet.Delete();
                    Marshal.ReleaseComObject(thisSheet);
                    thisSheet = null;
                }
            }
        }

        /// <summary>
        /// Block8: Order Qty BreakDown
        /// </summary>
        /// <param name="sheet">Worksheet</param>
        /// <param name="iD">string</param>
        /// <param name="rowPosition">int</param>
        /// <param name="enuType">EnuPrintSMType</param>
        /// <returns>bool</returns>
        private static bool PrintSMNoticeBlock8(MsExcel.Worksheet sheet, string iD, ref int rowPosition, EnuPrintSMType enuType = EnuPrintSMType.SMNotice)
        {
            var sqlX = string.Empty;
            var sqlY = string.Empty;
            var sqlZ = string.Empty;
            if (enuType == EnuPrintSMType.SMNotice)
            {
                sqlX = @"
Select s.SizeCode
From SMNotice sm
inner Join Orders o on o.ID = (Select POID From Orders Where ID = sm.OrderID)
inner Join Order_SizeCode s on s.ID = o.ID
Where sm.ID = @ID AND s.SizeCode is not null
Order by s.Seq
";

                sqlY = @"
Select distinct a.seq, a.Article
From SMNotice sm
inner Join Orders o on o.POID in (Select POID From Orders Where ID = sm.OrderID)
inner Join Order_Article a on a.ID = o.ID
Where sm.ID = @ID and a.Article <> ''
Order by a.Seq
";

                sqlZ = @"
Select q.SizeCode, q.Article, sum(q.Qty) as Qty
From SMNotice sm
inner Join Orders o on o.POID in (Select POID From Orders Where ID = sm.OrderID)
inner Join Order_Qty q on q.ID = o.ID
Where sm.ID = @ID and q.SizeCode <> '' and q.Article <> ''
group by q.Article, q.SizeCode
order by q.Article, q.SizeCode
";
            }
            else
            {
                sqlX = @"
Select s.SizeCode
From Orders o
inner Join Order_SizeCode s on s.ID = o.ID
Where o.ID = @ID AND s.SizeCode is not null
Order by s.Seq
";

                sqlY = @"
Select distinct a.seq, a.Article
From Orders o
inner Join Order_Article a on a.ID = o.ID
Where o.POID = @ID and a.Article <> ''
Order by a.Seq
";

                sqlZ = @"
Select q.SizeCode, q.Article, sum(q.Qty) as Qty
From Orders o
inner Join Order_Qty q on q.ID = o.ID
Where o.POID = @ID and q.SizeCode <> '' and q.Article <> ''
group by q.Article, q.SizeCode
order by q.Article, q.SizeCode
";
            }

            using (var drX = DBProxy.Current.SelectEx(sqlX, "ID", iD)) // 抓X軸
            using (var drY = DBProxy.Current.SelectEx(sqlY, "ID", iD)) // 抓Y軸
            using (var drZ = DBProxy.Current.SelectEx(sqlZ, "ID", iD)) // 抓Z軸
            {
                // 關於本區塊的WorkSheet
                var thisSheet = (sheet.Parent as MsExcel.Workbook).Worksheets.get_Item("P1B8") as MsExcel.Worksheet;

                try
                {
                    if (drX == false)
                    {
                        MyUtility.Msg.WarningBox(drX.ToSimpleString());
                        return false;
                    }

                    if (drY == false)
                    {
                        MyUtility.Msg.WarningBox(drY.ToSimpleString());
                        return false;
                    }

                    if (drZ == false)
                    {
                        MyUtility.Msg.WarningBox(drZ.ToSimpleString());
                        return false;
                    }

                    if (drZ.ExtendedData.Rows.Count == 0)
                    {
                        return true;
                    }

                    var dataX = drX.ExtendedData
                        .AsEnumerable()
                        .Select(row => row.Field<string>("SizeCode"))
                        .ToList();

                    var dataY = drY.ExtendedData
                        .AsEnumerable()
                        .Select(row => row.Field<string>("Article"))
                        .ToList();

                    // 換成用FabricPanelCode 和 Article當X座標與Y座標的字典備用
                    var dataZ = drZ.ExtendedData
                        .AsEnumerable()
                        .Select(row => new
                        {
                            Article = row.Field<string>("Article"),
                            SizeCode = row.Field<string>("SizeCode"),
                            Qty = row.Field<int?>("Qty"),
                        })
                        .ToDictionary(
                            item => new Tuple<string, string>(item.Article, item.SizeCode),
                            item => item.Qty);

                    var linesOfExcel = new List<IEnumerable<object>>();

                    // 第一行要印Article 和有使用到的SizeCode
                    linesOfExcel.Add(
                        new[] { (object)"Article" }.Concat(dataX));

                    // 接著把Article，串上屬於他的Qty
                    dataY.ToList().ForEach(article =>
                    {
                        var qtyOfThisArticle = dataX.Select(sizeCode =>
                        {
                            var key = new Tuple<string, string>(article, sizeCode);
                            if (dataZ.ContainsKey(key))
                            {
                                return (object)dataZ[key];
                            }
                            else
                            {
                                return null;
                            }
                        }).ToList();
                        if (qtyOfThisArticle.Any(item => item != null))
                        {
                            linesOfExcel.Add(new[] { (object)article }.Concat(qtyOfThisArticle));
                        }
                    });

                    // 最後一行Total小計
                    var summaryData = dataX
                        .Select(sizeCode => (object)dataZ.Where(pair => pair.Key.Item2 == sizeCode).Sum(pair => pair.Value).GetValueOrDefault(0));
                    linesOfExcel.Add(
                        new[] { (object)"Total" }.Concat(summaryData));

                    // 最後變成string[][]，再轉為object[,]準備放給ExlceRange.Value2
                    var rangeValue2 = linesOfExcel.DoubleArrayConvert2DArray();

                    // ======Excel動態生成區塊開始=======
                    // column的動態新增:
                    var columnNeeded = rangeValue2.GetLength(1);
                    if (columnNeeded > 26)
                    {
                        thisSheet.GetRange(2, 2, 2, 4).Copy();
                        thisSheet.GetRange(28, 2, 28 + columnNeeded - 28, 4).PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                    }

                    // row的動態新增:
                    var rowNeeded = rangeValue2.GetLength(0);
                    if (rowNeeded > 3)
                    {
                        // 將動態的行空間製造出來(第3行是TemplateRow，所以從第4行開始加，並且少加入1行，因為到時候會連TemplateRow一起放值用掉)
                        thisSheet.Range[thisSheet.Rows[4], thisSheet.Rows[4 + rowNeeded - 4]].Insert();

                        // 把Template行複製給剛剛製作出來的空間
                        thisSheet.Rows[3].Copy();
                        thisSheet.Range[thisSheet.Rows[4], thisSheet.Rows[4 + rowNeeded - 4]].PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                    }

                    // 把值放給剛剛動態製作出來的空間 (從Row2開始放值)
                    var areaLeft = 1;
                    var areaTop = 2;
                    thisSheet.GetRange(areaLeft, areaTop, areaLeft + rangeValue2.GetLength(1) - 1, areaTop + rangeValue2.GetLength(0) - 1).Value2 = rangeValue2;

                    // ======Excel動態生成區塊結束=======

                    // 搬回去給主WorkSheet
                    MoveSubBlockIntoMainSheet(sheet, ref rowPosition, thisSheet);
                    return true;
                }
                finally
                {
                    thisSheet.Delete();
                    Marshal.ReleaseComObject(thisSheet);
                    thisSheet = null;
                }
            }
        }

        /// <summary>
        /// Block9: MR/SMR info
        /// </summary>
        /// <param name="sheet">Worksheet</param>
        /// <param name="iD">string</param>
        /// <param name="rowPosition">int</param>
        /// <param name="enuType">EnuPrintSMType</param>
        /// <returns>bool</returns>
        private static bool PrintSMNoticeBlock9(MsExcel.Worksheet sheet, string iD, ref int rowPosition, EnuPrintSMType enuType = EnuPrintSMType.SMNotice)
        {
            var sqlHandle = string.Empty;
            if (enuType == EnuPrintSMType.SMNotice)
            {
                sqlHandle = @"
Select uMR.IdAndNameAndExt as MRInfo, uSMR.IdAndNameAndExt as SMRInfo
From SMNotice sm
Left Join GetName uMR on uMR.ID = sm.Mr
Left Join GetName uSMR on uSMR.ID = sm.SMR
Where sm.ID = @ID
";
            }
            else
            {
                sqlHandle = @"
Select uMR.IdAndNameAndExt as MRInfo, uSMR.IdAndNameAndExt as SMRInfo
From Orders o
Left Join GetName uMR on uMR.ID = o.MRHandle
Left Join GetName uSMR on uSMR.ID = o.SMR
Where o.ID = @ID
";
            }

            using (var drHandle = DBProxy.Current.SelectEx(sqlHandle, "ID", iD))
            {
                if (drHandle == false)
                {
                    MyUtility.Msg.WarningBox(drHandle.ToSimpleString());
                    return false;
                }

                // 關於本區塊的WorkSheet
                var thisSheet = (sheet.Parent as MsExcel.Workbook).Worksheets.get_Item("P1B9") as MsExcel.Worksheet;

                try
                {
                    var rowHandle = drHandle.ExtendedData.Rows[0];
                    thisSheet.Range["Block6CreateBy"].Value = rowHandle.Field<string>("MRInfo");
                    thisSheet.Range["Block6Auditor"].Value = rowHandle.Field<string>("SMRInfo");

                    // 搬回去給主WorkSheet
                    MoveSubBlockIntoMainSheet(sheet, ref rowPosition, thisSheet, 1);

                    return true;
                }
                finally
                {
                    thisSheet.Delete();
                    Marshal.ReleaseComObject(thisSheet);
                    thisSheet = null;
                }
            }
        }

        /// <summary>
        /// Block10: formal table for sign
        /// </summary>
        /// <param name="sheet">Worksheet</param>
        /// <param name="iD">string</param>
        /// <param name="rowPosition">int</param>
        /// <param name="enuType">EnuPrintSMType</param>
        /// <returns>bool</returns>
        private static bool PrintSMNoticeBlock10(MsExcel.Worksheet sheet, string iD, ref int rowPosition, EnuPrintSMType enuType = EnuPrintSMType.SMNotice)
        {
            // 關於本區塊的WorkSheet
            var thisSheet = (sheet.Parent as MsExcel.Workbook).Worksheets.get_Item("P2") as MsExcel.Worksheet;

            try
            {
                // 搬回去給主WorkSheet
                MoveSubBlockIntoMainSheet(sheet, ref rowPosition, thisSheet);

                return true;
            }
            finally
            {
                thisSheet.Delete();
                Marshal.ReleaseComObject(thisSheet);
                thisSheet = null;
            }
        }

        /// <summary>
        /// Block11: FabricColor TrimCard
        /// </summary>
        /// <param name="sheet">Worksheet</param>
        /// <param name="iD">string</param>
        /// <param name="rowPosition">int</param>
        /// <param name="pageHBreakList"><![CDATA[List<int>]]></param>
        /// <param name="enuType">EnuPrintSMType</param>
        /// <returns>bool</returns>
        private static bool PrintSMNoticeBlock11(MsExcel.Worksheet sheet, string iD, ref int rowPosition, List<int> pageHBreakList, EnuPrintSMType enuType = EnuPrintSMType.SMNotice)
        {
            var sql = string.Empty;
            var sqlStyleInfo = string.Empty;
            if (enuType == EnuPrintSMType.SMNotice)
            {
                sql = @"
Select art.Article, fc.FabricPanelCode, color.ColorID, bof.Refno
From SMNotice sm
inner Join Style_BOF bof on bof.StyleUkey = sm.StyleUkey
inner join Style_FabricCode fc on fc.Style_BOFUkey = bof.Ukey
inner join Style_Article art on art.StyleUkey = sm.StyleUkey
inner Join Style_ColorCombo color on color.StyleUkey = sm.StyleUkey and color.FabricPanelCode = fc.FabricPanelCode and color.Article = art.Article and color.FabricCode <> ''
Where sm.ID = @ID
Order by art.Article, fc.FabricPanelCode
";
                sqlStyleInfo = @"
Select CONCAT(s.ID, '-', s.SeasonID) as StyleInfo
From SMNotice sm
Inner Join Style s on s.Ukey = sm.StyleUkey
Where sm.ID = @ID
";
            }
            else
            {
                sql = @"
Select art.Article, fc.FabricPanelCode, color.ColorID, bof.Refno
From Orders o
inner Join Order_BOF bof on bof.Id = o.ID
inner join Order_FabricCode fc on fc.Order_BOFUkey = bof.Ukey
inner join Order_Article art on art.id = o.ID
inner Join Order_ColorCombo color on color.Id = o.ID and color.FabricPanelCode = fc.FabricPanelCode and color.Article = art.Article and color.FabricCode <> ''
Where o.ID = @ID
Order by art.Article, fc.FabricPanelCode
";
                sqlStyleInfo = @"
Select CONCAT(o.StyleID, '-', o.SeasonID) as StyleInfo
From Orders o
Where o.ID = @ID
";
            }

            using (var dr = DBProxy.Current.SelectEx(sql, "ID", iD))
            using (var drStyleInfo = DBProxy.Current.SelectEx(sqlStyleInfo, "ID", iD))
            {
                if (dr == false)
                {
                    MyUtility.Msg.ErrorBox(dr.ToSimpleString());
                    return false;
                }
                else if (drStyleInfo == false)
                {
                    MyUtility.Msg.ErrorBox(drStyleInfo.ToSimpleString());
                    return false;
                }
                else
                {
                    // 關於本區塊的WorkSheet
                    var thisSheet = (sheet.Parent as MsExcel.Workbook).Worksheets.get_Item("P3") as MsExcel.Worksheet;
                    try
                    {
                        thisSheet.GetRange("P3Style").Value = drStyleInfo.ExtendedData.Rows[0].Field<string>("StyleInfo");
                        thisSheet.GetRange("P3Date").Value = DateTime.Now.ToString("yyyy/MM/dd");

                        var data = dr.ExtendedData.AsEnumerable().Select(row => new
                        {
                            Article = row.Field<string>("Article"),
                            FabricPanelCode = row.Field<string>("FabricPanelCode"),
                            ColorID = row.Field<string>("ColorID"),
                            Refno = row.Field<string>("Refno"),
                        })

                            // 第一層先用Ar.ticle做GroupBy
                        .GroupBy(item => item.Article)

                            // 第二層，同一個Article內，怕會有超過7個FabricPanelCode (雖然很少但可能有)
                            // 所以把GroupItem用7切分，逢7換行
                            // 這邊會把本來Article -> FabricPanelCode[]變成 Article -> LineX -> FabricPanelCode
                            // 保證每一個Line都只會最多有7個FabricPanelCode要印
                        .SelectMany(group =>
                        {
                            var subData = group
                                .AsEnumerable()
                                .Select((groupItem, idx) => new
                                {
                                    Item = groupItem,
                                    Line = Convert.ToInt32(idx / 7),
                                })
                                .GroupBy(groupItem => groupItem.Line)
                                .Select(subGroup => new
                                {
                                    Article = group.Key,
                                    LineItems = subGroup
                                        .AsEnumerable()
                                        .Select(subGroupItem => subGroupItem.Item)
                                        .ToArray(),
                                })
                                .ToArray();

                            return subData;
                        })
                        .ToArray();

                        // 這邊會把7x4的表格一組一組的填值
                        // 每一組填完值就複製去給主Sheet，然後才填下一組的值
                        // 每次填值因為都是用Value2整片蓋過，所以不會有資料殘留的問題
                        // 這樣做可以省略複製的計算，也可以讓整體區域更完整
                        while (data.Any())
                        {
                            pageHBreakList.Add(rowPosition);

                            var count = data.Length;
                            var thisPack = data.Take(count < 4 ? count : 4).ToArray();
                            data = data.Skip(count < 4 ? count : 4).ToArray();

                            var rangeValue = new object[35, 21];
                            thisPack.Select((lineData, idx) =>
                            {
                                var lineBasePosition = idx * 9;
                                var lineFabricPanelCode = lineBasePosition + 0;
                                var lineColor = lineBasePosition + 1;
                                var lineRefno = lineBasePosition + 2;
                                var lineArticle = lineBasePosition + 4;
                                rangeValue[lineFabricPanelCode, 0] = "Combo";
                                rangeValue[lineColor, 0] = "Color";
                                rangeValue[lineRefno, 0] = "Refno";
                                rangeValue[lineRefno + 1, 0] = "ART#";
                                rangeValue[lineArticle, 0] = lineData.Article;
                                lineData.LineItems.Select((lineItem, lineItemIndex) =>
                                {
                                    var columnIndex = 1 + (lineItemIndex * 3);
                                    rangeValue[lineFabricPanelCode, columnIndex] = lineItem.FabricPanelCode;
                                    rangeValue[lineColor, columnIndex] = lineItem.ColorID;
                                    rangeValue[lineRefno, columnIndex] = lineItem.Refno;
                                    return true;
                                })
                                .ToList();
                                return true;
                            })
                            .ToList();

                            // 把二微陣列蓋進去Excel內
                            var areaTop = 2;
                            var areaLeft = 1;
                            thisSheet.GetRange(areaLeft, areaTop, areaLeft + rangeValue.GetLength(1), areaTop + rangeValue.GetLength(0)).Value2 = rangeValue;

                            // 搬回去給主WorkSheet
                            MoveSubBlockIntoMainSheet(sheet, ref rowPosition, thisSheet, 1);
                        }

                        return true;
                    }
                    finally
                    {
                        thisSheet.Delete();
                        Marshal.ReleaseComObject(thisSheet);
                        thisSheet = null;
                    }
                }
            }
        }

        #endregion

        #region Print Dev TrimCard

        /// <summary>
        /// 列印Dev TrimCard
        /// </summary>
        /// <param name="smID">string</param>
        public static void PrintSMNoticeDevTrimCard(string smID)
        {
            var xltFolder = Env.Cfg.XltPathDir;
            var xltPath = System.IO.Path.Combine(xltFolder, "Pattern-P01_PrintDev.xlt");
            if (System.IO.File.Exists(xltPath) == false)
            {
                MyUtility.Msg.WarningBox("can't find template file (" + xltPath + ")");
                return;
            }

            var tmpFileName = System.IO.Path.GetTempFileName();
            tmpFileName = System.IO.Path.ChangeExtension(tmpFileName, ".xlt");
            System.IO.File.Copy(xltPath, tmpFileName, true);
            var app = new MsExcel.Application();
            MsExcel.Workbook book = null;
            try
            {
                app.DisplayAlerts = false;
                book = app.Workbooks.Open(tmpFileName);
#if DEBUG
                app.Visible = true;
#endif
                var mainSheet = book.Worksheets[1] as MsExcel.Worksheet;
                int rowPosition = 1; // 各責任區域輪流使用
                if (PrintSMNoticeDevTrimCardBlock1(mainSheet, smID, ref rowPosition) == false)
                {
                    return; // Block1: SMNotice/Style
                }

                if (PrintSMNoticeDevTrimCardBlock2(mainSheet, smID, ref rowPosition) == false)
                {
                    return; // Block2: FabricPanelCode - Article (repeatly)
                }

                mainSheet.Cells[1, 1].Select();
                app.DisplayAlerts = true;

#if !DEBUG
            app.Visible = true;
#endif
            }
            finally
            {
                Marshal.ReleaseComObject(book);
                Marshal.ReleaseComObject(app);
                book = null;
                app = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            MyUtility.Msg.InfoBox("print complete");
        }

        private static bool PrintSMNoticeDevTrimCardBlock1(MsExcel.Worksheet mainSheet, string smID, ref int rowPosition)
        {
            var sql = @"
Select s.ID as WorkingNumber, s.SeasonID
From SMNotice sm
Inner Join Style s on s.Ukey = sm.StyleUkey
Where sm.ID = @ID
";
            using (var dr = DBProxy.Current.SelectEx(sql, "ID", smID))
            {
                if (dr == false)
                {
                    MyUtility.Msg.WarningBox(dr.ToSimpleString());
                    return false;
                }

                var row = dr.ExtendedData.Rows[0];
                mainSheet.GetRange("fWorkingNumber").SetValue(row.Field<string>("WorkingNumber"));
                mainSheet.GetRange("fSeason").SetValue(row.Field<string>("SeasonID"));
            }

            rowPosition += 6;
            return true;
        }

        private static bool PrintSMNoticeDevTrimCardBlock2(MsExcel.Worksheet mainSheet, string smID, ref int rowPosition)
        {
            var sql = @"
Select x.PNO, x.[REF#], x.Description, x.Article, x.ColorName from (
select 1 as tp, color.FabricPanelCode as PNO, f.Refno as [REF#], f.Description, color.Article, c2.Name as ColorName
From SMNotice sm
Inner Join Style s on s.Ukey = sm.StyleUkey
Inner Join Style_BOF bof on bof.StyleUkey = s.Ukey
Inner Join Style_FabricCode fc on fc.Style_BOFUkey = bof.Ukey
Inner Join Style_ColorCombo color on color.StyleUkey = s.Ukey and color.FabricPanelCode = fc.FabricPanelCode
Inner Join Fabric f on f.SCIRefno = bof.SCIRefno
Inner Join Color_multiple cm on cm.ID = color.ColorID and cm.BrandID = s.BrandID
Left Join Color c2 on c2.ID = cm.ColorID and c2.BrandId = s.BrandID
Where sm.ID = @ID
union all
select 2 as tp, color.FabricPanelCode as PNO, f.Refno as [REF#], f.Description, color.Article, c2.Name as ColorName
From SMNotice sm
Inner Join Style s on s.Ukey = sm.StyleUkey
Inner Join Style_BOA boa on boa.StyleUkey = s.Ukey
Inner Join Style_ColorCombo color on color.StyleUkey = s.Ukey and color.FabricPanelCode = boa.PatternPanel
Inner Join Fabric f on f.SCIRefno = boa.SCIRefno
Left Join Color_multiple cm on cm.ID = color.ColorID and cm.BrandID = s.BrandID
Left Join Color c2 on c2.ID = cm.ColorID and c2.BrandId = s.BrandID
Where sm.ID = @ID)x
order by x.tp, x.PNO
";
            using (var dr = DBProxy.Current.SelectEx(sql, "ID", smID))
            {
                if (dr == false)
                {
                    MyUtility.Msg.ErrorBox(dr.ToSimpleString());
                    return false;
                }
                else if (dr.ExtendedData.Rows.Count == 0)
                {
                    return true;
                }
                else
                {
                    var thisSheet = (mainSheet.Parent as MsExcel.Workbook).Worksheets.get_Item("P2") as MsExcel.Worksheet;
                    try
                    {
                        var data = dr.ExtendedData.AsEnumerable()

                            // 先把DataRow裡面的值取出來
                            .Select(row => new
                            {
                                PNO = row.Field<string>("PNO"),
                                REF = row.Field<string>("REF#"),
                                Description = row.Field<string>("Description"),
                                Article = row.Field<string>("Article"),
                                ColorName = row.Field<string>("ColorName"),
                            })

                            // 用PNO + REF + Description + Article群組
                            .GroupBy(
                                item => new
                                {
                                    PNO = item.PNO,
                                    REF = item.REF,
                                    Description = item.Description,
                                    Article = item.Article,
                                },
                                item => item.ColorName)

                            // 將顏色之間用換行符號串起來
                            .Select(item => new
                            {
                                PNO = item.Key.PNO,
                                REF = item.Key.REF,
                                Description = item.Key.Description,
                                Article = item.Key.Article,
                                Color = item.AsEnumerable().JoinToString("\r\n"),
                            })

                            // 第二階段是用 PNO + REF + Description 群組
                            .GroupBy(
                                item => new
                                {
                                    PNO = item.PNO,
                                    REF = item.REF,
                                    Description = item.Description,
                                },
                                item => new
                                {
                                    Article = item.Article,
                                    Color = item.Color,
                                })

                            // 把Article 和 ColorName組合成一對一對的資料，並且以長度2做切分
                            .SelectMany(item => item.AsEnumerable().Select((item2, idx) => new
                            {
                                LineIndex = Convert.ToInt32(idx / 2),
                                ArticlePair = item2,
                            })
                                .GroupBy(item2 => item2.LineIndex, item2 => item2.ArticlePair)
                                .Select(item2 => new
                                {
                                    PNO = item.Key.PNO,
                                    REF = item.Key.REF,
                                    Description = item.Key.Description,
                                    ArticlePairs = item2.AsEnumerable().ToArray(),
                                })
                                .ToArray())
                            .ToList();

                        // 因為要使用ref rowPosition這個傳址變數，所以不能包在Linq裡面
                        // 上面組裝好的每一個item，都代表一組PNO + REF + DESCRIPTION，以及這個組合的1或2個Article-Color Pair
                        foreach (var item in data)
                        {
                            var rangeValue = new object[4, 6];
                            rangeValue[0, 0] = "ARTICLE NO:";
                            rangeValue[1, 0] = "COLOR CODE/NAME:";
                            rangeValue[2, 0] = "PNO:";
                            rangeValue[2, 1] = "REF#:";
                            rangeValue[2, 2] = "DESCRIPTION:";
                            rangeValue[3, 0] = item.PNO;
                            rangeValue[3, 1] = item.REF;
                            rangeValue[3, 2] = item.Description;
                            rangeValue[0, 4] = item.ArticlePairs[0].Article;
                            rangeValue[1, 4] = item.ArticlePairs[0].Color;
                            if (item.ArticlePairs.Length > 1)
                            {
                                rangeValue[0, 5] = item.ArticlePairs[1].Article;
                                rangeValue[1, 5] = item.ArticlePairs[1].Color;
                            }

                            var areaLeft = 1;
                            var areaTop = 1;
                            thisSheet.GetRange(areaLeft, areaTop, areaLeft + rangeValue.GetLength(1) - 1, areaTop + rangeValue.GetLength(0) - 1).Value2 = rangeValue;

                            // 搬回去給主WorkSheet
                            MoveSubBlockIntoMainSheet(mainSheet, ref rowPosition, thisSheet);
                        }

                        return true;
                    }
                    finally
                    {
                        thisSheet.Delete();
                        Marshal.ReleaseComObject(thisSheet);
                        thisSheet = null;
                    }
                }
            }
        }

        #endregion

        #region Print Garment List

        /// <summary>
        /// 列印SMNotice報表
        /// </summary>
        /// <param name="uKey">long</param>
        /// <param name="savePath">string</param>
        public static void PrintGarmentList(long uKey, string savePath = null)
        {
            var directlyOpenExportReport = savePath == null;
            var xltFolder = Env.Cfg.XltPathDir;
            var xltPath = System.IO.Path.Combine(xltFolder, "Pattern-P02.Garment List-Print.xlt");
            var bmpFolder = System.IO.Path.Combine(xltFolder, "BMP");
            if (System.IO.File.Exists(xltPath) == false)
            {
                MyUtility.Msg.WarningBox("can't find template file (" + xltPath + ")");
                return;
            }

            var tmpFileName = System.IO.Path.GetTempFileName();
            tmpFileName = System.IO.Path.ChangeExtension(tmpFileName, ".xlt");
            System.IO.File.Copy(xltPath, tmpFileName, true);
            var app = new MsExcel.Application();
            MsExcel.Workbook book = null;
            MsExcel.Worksheet mainSheet = null;
            try
            {
                app.DisplayAlerts = false;
                book = app.Workbooks.Add(tmpFileName);

                // book = app.Workbooks.Open(tmpFileName);
#if DEBUG
                app.Visible = true;
#endif
                mainSheet = book.Worksheets[1] as MsExcel.Worksheet;

                int rowPosition = 1; // 各責任區域輪流使用
                PrintGarmentListBlock1(mainSheet, uKey, ref rowPosition); // Block1: Header - Pattern
                var articleCount = 0;
                PrintGarmentListBlock2(mainSheet, uKey, ref rowPosition, out articleCount); // Block2: Garment List
                PrintGarmentListBlock3(mainSheet, uKey, ref rowPosition, articleCount, bmpFolder); // Block3: Cutting Piece
                PrintGarmentListBlock4(mainSheet, uKey, ref rowPosition, articleCount); // Block4: Remark

                // mainSheet.UsedRange.Rows.AutoFit();
                mainSheet.Cells[1, 1].Select();
                mainSheet.Protect(Env.User.UserPassword);
                var saveDir = new System.IO.DirectoryInfo(Env.Cfg.ReportTempDir).FullName;
                using (var dr = DBProxy.Current.SelectEx(@"select StyleID, SeasonID, PatternNO from Pattern where UKey = @UKey", "Ukey", uKey))
                {
                    var row = dr.ExtendedData.Rows[0];
                    if (savePath == null)
                    {
                        savePath = System.IO.Path.Combine(saveDir, string.Format("{0}-{1}-{2}-GL_{3}.xlsx", row["StyleID"], row["SeasonID"], row["PatternNo"], DateTime.Now.ToString("yyyyMMdd_HHmmss")));
                    }

                    if (System.IO.File.Exists(savePath))
                    {
                        while (true)
                        {
                            try
                            {
                                System.IO.File.Delete(savePath);
                                break;
                            }
                            catch (Exception)
                            {
                                if (MyUtility.Msg.QuestionBox("file already exists and was opend, do yo want to retry?") == System.Windows.Forms.DialogResult.No)
                                {
                                    return;
                                }
                            }
                        }
                    }

                    book.SaveAs(Filename: savePath);
                }
            }
            finally
            {
                book.Close();
                app.Quit();
                Marshal.ReleaseComObject(mainSheet);
                Marshal.ReleaseComObject(book);
                Marshal.ReleaseComObject(app);
                book = null;
                app = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            if (directlyOpenExportReport)
            {
                System.Diagnostics.Process.Start(savePath);
                MyUtility.Msg.InfoBox("print complete");
            }
        }

        /// <summary>
        /// Block1: Header - Pattern
        /// </summary>
        /// <param name="sheet">Worksheet</param>
        /// <param name="uKey">long</param>
        /// <param name="rowPosition">int</param>
        /// <returns>bool</returns>
        private static bool PrintGarmentListBlock1(MsExcel.Worksheet sheet, long uKey, ref int rowPosition)
        {
            string strSelect = @"
With theSizeSets as (
	Select sl.ID
	From Pattern p
	Left Join SMNotice sm on sm.ID = p.ID
	Left Join Style s on s.UKey = sm.StyleUKey
	Left Join SizeSetList sl on sl.BrandID = s.BrandID and sl.SizeGroupID = sm.SizeGroup
	Where p.UKey = @UKey
),
SizeListOfSizeSet as (
	Select data From dbo.SplitString((select top 1 ID From theSizeSets), ',')
),
limitedSizeLists as (
	Select S1.Data as SizeCode
	From SizeListOfSizeSet s1
	Where s1.data in (
		Select sc.SizeCode
		From Pattern p
		Left Join Style_SizeCode sc on sc.StyleUkey = p.StyleUkey
		Where p.UKey = @UKey)
),
linesOfSizeList as (
	Select Stuff((Select ','+ SizeCode From limitedSizeLists for xml path('')), 1, 1, '') as SizeCode
)
Select p.ID, sm.BrandID, sm.SeasonID, sm.StyleID, s.CdCodeID, p.PatternNo, s.Description
, iif(IsNull(p.SizeRound, Cast(1 as bit)) = 1, (Select Top 1 SizeCode From linesOfSizeList), p.SizeRange) as SizeRound
, smd.PhaseID, sm.ProductionFactory, p.StyleRemark
, uMR.NameAndExt as MRBy, uPtn.NameAndExt as PtnBy
, p.ActFtyPattern
From Pattern p
Left Join SMNotice sm on sm.ID = p.ID
Left Join SMNotice_Detail smd on smd.ID = sm.iD and smd.Type = 'P'
Left Join Style s on s.UKey = sm.StyleUKey
Left Join GetName uMR on uMR.ID = sm.Mr
Left Join GetName uPtn on uPtn.ID = p.CFMName
Where p.UKey = @UKey";
            using (var dr = DBProxy.Current.SelectEx(strSelect, "UKey", uKey))
            {
                if (dr == false)
                {
                    MyUtility.Msg.WarningBox(dr.ToSimpleString());
                    return false;
                }

                var row = dr.ExtendedData.Rows[0];

                // line2
                sheet.GetRange("A2").SetValue(string.Format("Apply No#: {0}", row.Field<string>("ID")));

                // line3
                sheet.GetRange("A3").SetValue(string.Format("Unify Brand: {0}", row.Field<string>("BrandID")));
                sheet.GetRange("D3").SetValue(string.Format("Season: {0}", row.Field<string>("SeasonID")));

                // line4
                sheet.GetRange("A4").SetValue(string.Format("Style: {0}", row.Field<string>("StyleID")));
                sheet.GetRange("D4").SetValue(string.Format("CD Code: {0}", row.Field<string>("CdCodeID")));
                sheet.GetRange("I4").SetValue(string.Format("Phase: {0}", row.Field<string>("PhaseID")));

                // line5
                sheet.GetRange("A5").SetValue(string.Format("Ptn No: {0}", row.Field<string>("PatternNo")));
                sheet.GetRange("D5").SetValue(string.Format("Description: {0}", row.Field<string>("Description")));
                sheet.GetRange("I5").SetValue(string.Format("Sample Making: {0}", row.Field<string>("ProductionFactory")));

                // line6
                var sizeRoundText = string.Format("Size Round#: {0}", row.Field<string>("SizeRound"));
                using (var img = new Bitmap(1000, 1000))
                using (var gra = Graphics.FromImage(img))
                using (var fnt = new Font("Arial", 10f))
                {
                    sheet.GetRange("A6").WrapText = true;
                    sheet.GetRange("A6").SetValue(sizeRoundText);
                    var width = Convert.ToInt32(Math.Floor((double)sheet.GetRange("A6:I6").Width * 1.33));
                    sheet.GetRange("A6").RowHeight = gra.MeasureString(sizeRoundText, fnt, width).Height / 1.2;
                }

                // line7
                sheet.GetRange("A7").SetValue(string.Format("Style Remark: {0}", row.Field<string>("StyleRemark")));

                // line8
                sheet.GetRange("A8").SetValue(string.Format("Mr by: {0}", row.Field<string>("MRBy")));
                sheet.GetRange("D8").SetValue(string.Format("Pattern by: {0}", row.Field<string>("PtnBy")));
                sheet.GetRange("I8").SetValue(string.Format("Provide By: {0}", row.Field<string>("ActFtyPattern")));

                sheet.Name = string.Format("{0}-{1}", row.Field<string>("StyleID"), row.Field<string>("PatternNo"));
            }

            rowPosition += 10;
            return true;
        }

        /// <summary>
        /// Block2: Garment List
        /// </summary>
        /// <param name="sheet">Worksheet</param>
        /// <param name="uKey">long</param>
        /// <param name="rowPosition">int rowPosition</param>
        /// <param name="articleCount">int articleCount</param>
        /// <returns>bool</returns>
        private static bool PrintGarmentListBlock2(MsExcel.Worksheet sheet, long uKey, ref int rowPosition, out int articleCount)
        {
            List<string> articleGroupList;
            string strSelect = @"
Select ArticleGroup 
From Pattern_GL_Article 
Where PatternUKey = @PatternUKey
And ArticleGroup <> 'F_CODE'
Order by SEQ";
            using (var dr = DBProxy.Current.SelectEx(strSelect, "PatternUKey", uKey))
            {
                if (dr == false)
                {
                    MyUtility.Msg.WarningBox(dr.ToSimpleString());
                    articleCount = -1;
                    return false;
                }

                articleGroupList = dr.ExtendedData.AsEnumerable().Select(row => row.Field<string>("ArticleGroup")).Distinct().ToList();
                articleCount = articleGroupList.Count;
            }

            var sql = @"
Select 
ROW_NUMBER() OVER(ORDER BY pg.SEQ) AS Row
, iif(Substring(pg.PatternCode, 1, len(p.PatternNo)) = p.PatternNo, Substring(pg.PatternCode, len(p.PatternNo) + 1, 999), pg.PatternCode) as PatternCode
, pg.PatternDesc, pg.Annotation, pg.Alone, pg.PAIR, pg.DV, F_Code.FabricPanelCode, pg.Remarks
" + articleGroupList.Select(item => string.Format(", [{0}].FabricPanelCode as [{0}]", item)).JoinToString("\r\n") + @"
From Pattern p
Left Join Pattern_GL pg on pg.PatternUKEY = p.UKey
Outer Apply (Select FabricPanelCode from Pattern_GL_LectraCode lc where lc.PatternUKEY = p.UKey and lc.Seq = pg.Seq and lc.ArticleGroup = 'F_CODE') F_Code
" + articleGroupList.Select(item => string.Format(
"Outer Apply (Select FabricPanelCode from Pattern_GL_LectraCode lc where lc.PatternUKEY = p.UKey and lc.Seq = pg.Seq and lc.ArticleGroup = '{0}') [{0}]", item)).JoinToString("\r\n") + @"
Where pg.PatternUKey = @PatternUKey
Order by pg.SEQ
";
            using (var dr = DBProxy.Current.SelectEx(sql, "PatternUKey", uKey))
            {
                if (dr == false)
                {
                    MyUtility.Msg.WarningBox(dr.ToSimpleString());
                    articleCount = -1;
                    return false;
                }

                var rangeValue = dr.ExtendedData
                    .AsEnumerable()
                    .Select(row => row.ItemArray)
                    .DoubleArrayConvert2DArray();

                var dynamicCodeHeader = Enumerable.Repeat(
                    articleGroupList
                        .Select(code => code.ToUpper().StartsWith("CODE") ?
                            code.Substring(4).ToUpper() :
                            code.ToUpper())
                        .Cast<object>(), 1).DoubleArrayConvert2DArray();

                // 關於第二區塊的WorkSheet
                var thisSheet = (sheet.Parent as MsExcel.Workbook).Worksheets.get_Item("B2") as MsExcel.Worksheet;
                try
                {
                    // ======Excel動態生成區塊開始=======
                    // column的動態新增:
                    var columnNeeded = dynamicCodeHeader.GetLength(1);
                    if (columnNeeded == 0)
                    {
                        thisSheet.GetRange(10, 1, 10, 3).Delete();
                    }
                    else if (columnNeeded > 1)
                    {
                        thisSheet.GetRange(10, 1, 10, 3).Copy();
                        thisSheet.GetRange(11, 1, 11 + columnNeeded - 2, 3).PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                    }

                    thisSheet.GetRange(10, 2, 10 + dynamicCodeHeader.GetLength(1) - 1, 2 + dynamicCodeHeader.GetLength(0) - 1).Value2 = dynamicCodeHeader;

                    // row的動態新增:
                    var rowNeeded = rangeValue.GetLength(0);
                    if (rowNeeded > 1)
                    {
                        // 將動態的行空間製造出來(第3行是TemplateRow，所以從第4行開始加，並且少加入1行，因為到時候會連TemplateRow一起放值用掉)
                        thisSheet.Range[thisSheet.Rows[4], thisSheet.Rows[4 + rowNeeded - 2]].Insert();

                        // 把Template行複製給剛剛製作出來的空間
                        thisSheet.Rows[3].Copy();
                        thisSheet.Range[thisSheet.Rows[4], thisSheet.Rows[4 + rowNeeded - 2]].PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                    }

                    thisSheet.GetRange(1, 3, 1 + rangeValue.GetLength(1) - 1, 3 + rangeValue.GetLength(0) - 1).Value2 = rangeValue;
                    thisSheet.GetRange(1, 3, 1 + rangeValue.GetLength(1) - 1, 3 + rangeValue.GetLength(0) - 1).Rows.AutoFit();

                    // ======Excel動態生成區塊結束=======

                    // 搬回去給主WorkSheet
                    MoveSubBlockIntoMainSheet(sheet, ref rowPosition, thisSheet, 1);

                    return true;
                }
                finally
                {
                    thisSheet.Delete();
                    Marshal.ReleaseComObject(thisSheet);
                    thisSheet = null;
                }
            }
        }

        /// <summary>
        /// Block3: BOF/Fabric
        /// </summary>
        /// <param name="sheet">Worksheet</param>
        /// <param name="uKey">long</param>
        /// <param name="rowPosition">int rowPosition</param>
        /// <param name="articleCount">int articleCount</param>
        /// <param name="bmpFolder">string</param>
        /// <returns>bool</returns>
        private static bool PrintGarmentListBlock3(MsExcel.Worksheet sheet, long uKey, ref int rowPosition, int articleCount, string bmpFolder)
        {
            var sql = @"
Select cp.PieceName
, cp.PieceDesc
, iif(cp.Type = 'C'
	, 'W:' + Cast(cp.CuttingWidth as varchar)
	, '') as CuttingWidth
, iif(cp.Type = 'C'
	, cp.FabricPanelCode
	, Concat(cp.FabricPanelCode, '[', cp.SizeItem, ']')) as FabricPanelCode
, cp.Level
, cp.Article
, '#' + SizeCode as SizeCode
, iif(cp.Unit = 'CM',
	Concat('L:'
			, iif(IsNull(cp.CuttingLossLength, 0) = 0, '','(')
			, cp.Length, ' cm', iif(IsNull(cp.CuttingLossLength, 0) = 0, '', (' + ' + format(cp.CuttingLossLength, '#,##0.####') + ' cm'))
			, iif(IsNull(cp.CuttingLossLength, 0) = 0, '',')')
			, ' x ', cp.UsedQty
            , iif(cp.CuttingLossRate = 0, '', ' x 1.' + Right('0' + Cast(cp.CuttingLossRate as varchar), 2))),
	Concat('L:'
			, iif(IsNull(cp.CuttingLossLength, 0) = 0, '','(')
			, cp.Length, iif(IsNull(cp.CuttingLossLength, 0) = 0, '', (' + ' + format(cp.CuttingLossLength, '#,##0.####') + ' ""'))
			, iif(IsNull(cp.CuttingLossLength, 0) = 0, '',')')
			, ' x ', cp.UsedQty
            , iif(cp.CuttingLossRate = 0, '', ' x 1.' + Right('0' + Cast(cp.CuttingLossRate as varchar), 2)))) as detail
, dir.JPGPath
, cp.Level
, dir.DirectionEN
From Pattern_CuttingPiece cp
Left Join Direction dir WITH (NOLOCK) ON cp.Level = dir.ID
Where cp.PatternUKey = @PatternUKey
and cp.SizeCode <> ''
Order by (
	Case cp.Type 
		When 'C' then 1
		When 'T' then 2
		When 'C' then 3
		Else 4 end
), cp.SortMain, cp.SortSEQ
";
            using (var dr = DBProxy.Current.SelectEx(sql, "PatternUKey", uKey))
            {
                if (dr == false)
                {
                    MyUtility.Msg.ErrorBox(dr.ToSimpleString());
                    return false;
                }
                else
                {
                    var groupData = dr.ExtendedData.AsEnumerable()
                    .GroupBy(
                        row => new
                        {
                            PieceName = row.Field<string>("PieceName"),
                            PieceDesc = row.Field<string>("PieceDesc"),
                            CuttingWidth = row.Field<string>("CuttingWidth"),
                            FabricPanelCode = row.Field<string>("FabricPanelCode"),
                            JPGPath = row.Field<string>("JPGPath"),
                            DirectionEN = row.Field<string>("DirectionEN"),
                            Article = row.Field<string>("Article"),
                        },
                        row => new
                        {
                            SizeCode = row.Field<string>("SizeCode"),
                            Detail = row.Field<string>("Detail"),
                        })
                    .ToList();

                    // 關於第三區塊的WorkSheet
                    var thisSheet = (sheet.Parent as MsExcel.Workbook).Worksheets.get_Item("B3") as MsExcel.Worksheet;
                    var articleCellWidth = Convert.ToInt32(Math.Floor((double)thisSheet.GetRange(3, 2, 9, 2).Width * 1.3333));
                    using (var img = new Bitmap(1000, 1000))
                    using (var gra = Graphics.FromImage(img))
                    using (var fnt = new Font("Arial", 10f))
                    {
                        try
                        {
                            foreach (var groupItem in groupData)
                            {
                                thisSheet.Copy(Type.Missing, (sheet.Parent as MsExcel.Workbook).Sheets[(sheet.Parent as MsExcel.Workbook).Sheets.Count]);
                                var blankSheet = (sheet.Parent as MsExcel.Workbook).Worksheets[(sheet.Parent as MsExcel.Workbook).Sheets.Count] as MsExcel.Worksheet;
                                blankSheet.Range[blankSheet.Cells[1, 9], blankSheet.Cells[1, 9 + articleCount]].Merge();
                                blankSheet.Range[blankSheet.Cells[2, 3], blankSheet.Cells[2, 9 + articleCount]].Merge();
                                blankSheet.Range[blankSheet.Cells[3, 9], blankSheet.Cells[3, 9 + articleCount]].Merge();
                                blankSheet.Range[blankSheet.Cells[4, 1], blankSheet.Cells[4, 9 + articleCount]].Merge();
                                var borders = blankSheet.UsedRange.Borders;
                                borders.LineStyle = MsExcel.XlLineStyle.xlContinuous;

                                var groupKey = groupItem.Key;
                                var rangeValue = new object[3 + groupItem.Count(), 9];

                                // 第一行
                                rangeValue[0, 0] = groupKey.PieceName;
                                rangeValue[0, 2] = groupKey.PieceDesc;
                                rangeValue[0, 4] = groupKey.CuttingWidth;
                                rangeValue[0, 7] = groupKey.FabricPanelCode;
                                rangeValue[0, 8] = groupKey.DirectionEN;

                                // 第二行
                                rangeValue[1, 0] = "Art No";
                                rangeValue[1, 2] = groupKey.Article;

                                groupItem.Select((item, index) =>
                                {
                                    rangeValue[2 + index, 2] = item.SizeCode;
                                    rangeValue[2 + index, 3] = item.Detail;
                                    return true;
                                }).ToList();

                                // ======Excel動態生成區塊開始=======
                                var rowsNeeded = rangeValue.GetLength(0);
                                if (rowsNeeded > 4)
                                {
                                    // 將動態的行空間製造出來(第3行是TemplateRow，所以從第4行開始加，並且少加入1行，因為到時候會連TemplateRow一起放值用掉)
                                    blankSheet.Range[blankSheet.Rows[4], blankSheet.Rows[4 + rowsNeeded - 5]].Insert();

                                    // 把Template行複製給剛剛製作出來的空間
                                    blankSheet.Rows[3].Copy();
                                    blankSheet.Range[blankSheet.Rows[4], blankSheet.Rows[4 + rowsNeeded - 5]].PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                                }

                                // 把值放給剛剛動態製作出來的空間 (從Row2開始放值)
                                blankSheet.GetRange(1, 1, 1 + rangeValue.GetLength(1) - 1, 1 + rangeValue.GetLength(0) - 1).Value2 = rangeValue;
                                if (string.IsNullOrWhiteSpace(groupKey.Article))
                                {
                                    (blankSheet.Rows[2] as MsExcel.Range).Delete();
                                }
                                else
                                {
                                    (blankSheet.Rows[2] as MsExcel.Range).WrapText = true;
                                    (blankSheet.Rows[2] as MsExcel.Range).RowHeight = gra.MeasureString(groupKey.Article, fnt, articleCellWidth).Height / 1.2;
                                }

                                // ======Excel動態生成區塊結束=======

                                // 搬回去給主WorkSheet
                                MoveSubBlockIntoMainSheet(sheet, ref rowPosition, blankSheet);

                                // 圖片
                                if (string.IsNullOrWhiteSpace(groupKey.JPGPath) == false)
                                {
                                    var imgpath = System.IO.Path.Combine(bmpFolder, groupKey.JPGPath);
                                    if (System.IO.File.Exists(imgpath))
                                    {
                                        var cell = sheet.Cells[rowPosition - rangeValue.GetLength(0), 9];
                                        var iPosition = groupKey.DirectionEN.Length * 5d;
                                        sheet.Shapes.AddPicture(
                                            imgpath,
                                            Microsoft.Office.Core.MsoTriState.msoFalse,
                                            Microsoft.Office.Core.MsoTriState.msoTrue,
                                            cell.Left + iPosition,
                                            cell.Top,
                                            50,
                                            17);
                                    }
                                }

                                blankSheet.Delete();
                                Marshal.ReleaseComObject(blankSheet);
                            }

                            return true;
                        }
                        finally
                        {
                            thisSheet.Delete();
                            Marshal.ReleaseComObject(thisSheet);
                            thisSheet = null;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Block4: BOa-ColorCombo
        /// </summary>
        /// <param name="sheet">Worksheet</param>
        /// <param name="uKey">long</param>
        /// <param name="rowPosition">int rowPosition</param>
        /// <param name="articleCount">int articleCount</param>
        /// <returns>bool</returns>
        private static bool PrintGarmentListBlock4(MsExcel.Worksheet sheet, long uKey, ref int rowPosition, int articleCount)
        {
            var sql = @"
Select p.HisRemark, u.NameAndExt as CFMName, p.CheckerDate
From Pattern p
Left Join GetName u on u.ID = p.CFMName
Where p.UKey = @UKey
";
            using (var dr = DBProxy.Current.SelectEx(sql, "UKey", uKey)) // 抓X軸
            {
                if (dr == false)
                {
                    MyUtility.Msg.WarningBox(dr.ToSimpleString());
                    return false;
                }

                // 關於本區塊的WorkSheet
                var thisSheet = (sheet.Parent as MsExcel.Workbook).Worksheets.get_Item("B4") as MsExcel.Worksheet;
                thisSheet.Range[thisSheet.Cells[1, 1], thisSheet.Cells[1, 9 + articleCount]].Merge();
                thisSheet.Range[thisSheet.Cells[2, 1], thisSheet.Cells[2, 9 + articleCount]].Merge();
                var row = dr.ExtendedData.Rows[0];
                try
                {
                    var remarkCellWidth = Convert.ToInt32((double)thisSheet.GetRange(1, 2, 9, 2).Width * 1.2);
                    using (var img = new Bitmap(1000, 1000))
                    using (var gra = Graphics.FromImage(img))
                    using (var fnt = new Font("Arial", 10f))
                    {
                        thisSheet.GetRange("A2").Value = row.Field<string>("HisRemark");
                        var heightOfRemark = gra.MeasureString(row.Field<string>("HisRemark") ?? string.Empty, fnt, remarkCellWidth).Height / 1.2;
                        if (heightOfRemark < 20)
                        {
                            heightOfRemark = 20;
                        }

                        thisSheet.GetRange("A2").RowHeight = heightOfRemark;
                    }

                    thisSheet.GetRange("A3").Value = string.Format("Approval#:{0}", row.Field<string>("CFMName"));
                    thisSheet.GetRange("H3").Value = row.Field<DateTime?>("CheckerDate").ToStringEx("MM/dd");

                    // 搬回去給主WorkSheet
                    MoveSubBlockIntoMainSheet(sheet, ref rowPosition, thisSheet);

                    return true;
                }
                finally
                {
                    thisSheet.Delete();
                    Marshal.ReleaseComObject(thisSheet);
                    thisSheet = null;
                }
            }
        }

        #endregion

        #region Print shared method

        /// <summary>
        /// 把子區塊的資料，搬移到主WorkSheet內，直接使用UsedRange來做搬移範圍
        /// </summary>
        /// <param name="mainSheet">main Worksheet</param>
        /// <param name="rowPosition">int</param>
        /// <param name="subBlockSheet">subBlock Worksheet</param>
        /// <param name="blankRowsAfterThisBlock">int?</param>
        private static void MoveSubBlockIntoMainSheet(MsExcel.Worksheet mainSheet, ref int rowPosition, MsExcel.Worksheet subBlockSheet, int? blankRowsAfterThisBlock = null)
        {
            // 把這個Block3完整複製過去主Sheet(參考rowPosition)
            var thisSheetUsedRange = subBlockSheet.UsedRange;
            (mainSheet.Rows[rowPosition] as MsExcel.Range).EntireRow.InsertIndent(thisSheetUsedRange.Rows.Count);

            var rowStart = thisSheetUsedRange.Rows[1].Row;
            var rowEnd = rowStart + thisSheetUsedRange.Rows.Count;

            // Full Row Copy for row height copy purpose
            subBlockSheet.Range[subBlockSheet.Rows[rowStart], subBlockSheet.Rows[rowEnd]].Copy();
            mainSheet.Range[mainSheet.Rows[rowPosition], mainSheet.Rows[rowPosition + thisSheetUsedRange.Rows.Count]].PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);

            ////Range Copy for content & cell format  <-- because Range Copy ignore Row height copy, so I have to copy full rows before here
            // thisSheetUsedRange.Copy();
            // mainSheet.Cells[rowPosition, 1].PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);

            // rowPosition遞移，給下一個區塊使用
            rowPosition += thisSheetUsedRange.Rows.Count + blankRowsAfterThisBlock.GetValueOrDefault(0); // 與下個Block空一行

            Marshal.ReleaseComObject(thisSheetUsedRange);
            thisSheetUsedRange = null;
        }

        #endregion
    }
}