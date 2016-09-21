using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci;
using Sci.Data;
using Ict;
using Ict.Win;
using Sci.Win;
using Sci.Trade.Class;
using Sci.Trade.Class.Commons;
using System.IO;
using Word = Microsoft.Office.Interop.Word;
using System.Linq;
using System.Runtime.InteropServices;


namespace Sci.Production.Warehouse
{
    public partial class P01_TrimCardPrint : Sci.Win.Tems.PrintForm
    {
        DataTable dtPrint, dtPrint2, dtColor, dtColor2;
        DataRow rowColor;
        string sql, temp, orderID, StyleID, SeasonID, FactoryID, BrandID, POID, colorName, ColorID;
        List<string> ListColor = new List<string>();


        public P01_TrimCardPrint(string _orderID, string _StyleID, string _SeasonID, string _FactoryID, string _BrandID, string _POID)
        {
            InitializeComponent();
            orderID = _orderID;
            StyleID = _StyleID;
            SeasonID = _SeasonID;
            FactoryID = _FactoryID;
            BrandID = _BrandID;
            POID = _POID;
        }

        //欄位檢核
        protected override bool ValidateInput()  
        {
            if (!radioFabric.Checked && !radioAccessory.Checked && !radioOther.Checked && !radioThread.Checked)
            {
                ShowErr("Please select an item !!");
                return false;
            }

            return true;
        }

        protected override Ict.DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result = Result.True;
            if (dtPrint != null) dtPrint.Rows.Clear();

            #region SQL
            if (radioFabric.Checked)
            {
                #region FABRIC
                sql = string.Format(@"select  A.PatternPanel , A.FabricCode , B.Refno , C.Description , A.Lectracode
                                from Order_FabricCode A
                                left join Order_BOF B on B.Id=A.Id and B.FabricCode=A.FabricCode
                                left join Fabric C on C.SCIRefno=B.SCIRefno
                                where A.ID='{0}'
                                order by FabricCode", orderID);
                result = DBProxy.Current.Select(null, sql, out dtPrint);
                if (!result) return result;

                sql = string.Format(@"select distinct Article 
                                from Order_ColorCombo 
                                where Id='{0}' and LectraCode in (select LectraCode from Order_FabricCode where ID='{0}')", orderID);
                result = DBProxy.Current.Select(null, sql, out dtPrint2);
                if (!result) return result;

                sql = string.Format(@"select ColorID , B.Name , LectraCode , Article
                                from Order_ColorCombo A
                                left join Color B on B.BrandId='{0}' and B.ID=A.ColorID
                                where A.Id='{1}'", BrandID, orderID);
                result = DBProxy.Current.Select(null, sql, out dtColor);
                if (!result) return result;

                #endregion
            }
            else if (radioAccessory.Checked)
            {
                #region ACCESSORY
                sql = string.Format(@"select A.Refno, B.Description 
                                    from Order_BOA_Expend A
                                    left join Fabric B on B.SCIRefno=A.SCIRefno
                                    where Id='{0}' and Article<>'' and ColorId<>''
                                    group by A.Refno, B.Description ", orderID);
                result = DBProxy.Current.Select(null, sql, out dtPrint);
                if (!result) return result;

                sql = string.Format(@"select distinct article 
                                    from Order_BOA_Expend A
                                    left join Fabric B on B.SCIRefno=A.SCIRefno
                                    where Id='{0}' and Article<>'' and ColorId<>''
                                    group by A.Refno, B.Description , article, ColorId
                                    order by Article", orderID);
                result = DBProxy.Current.Select(null, sql, out dtPrint2);
                if (!result) return result;

                sql = string.Format(@"select A.Refno, B.Description , article , ColorId
                                    from Order_BOA_Expend A
                                    left join Fabric B on B.SCIRefno=A.SCIRefno
                                    where Id='{0}' and Article<>'' and ColorId<>''
                                    group by A.Refno, B.Description , article, ColorId", orderID);
                result = DBProxy.Current.Select(null, sql, out dtColor);
                if (!result) return result;

                sql = string.Format(@"select ID , Name from Color where BrandId='{0}' and JUNK=0", BrandID);
                result = DBProxy.Current.Select(null, sql, out dtColor2);
                if (!result) return result;
                #endregion
            }
            else if (radioOther.Checked)
            {
                #region OTHER
                //架構要調，先HOLD住
                sql = string.Format(@"select A.Refno,B.DescDetail
                                    from Order_BOA_Expend A
                                    left join Fabric B on B.SCIRefno=A.SCIRefno
                                    where Id='{0}' and Article<>'' and ColorId<>''
                                    group by A.Refno, B.DescDetail ", orderID);
                result = DBProxy.Current.Select(null, sql, out dtPrint);
                if (!result) return result;

                sql = string.Format(@"select distinct orders.ID 
                                    from orders
                                    where orders.POID='{0}'", POID);
                result = DBProxy.Current.Select(null, sql, out dtPrint2);
                if (!result) return result;
                #endregion
            }
            else if (radioThread.Checked)
            {
                #region Thread
                sql = string.Format(@"select B.Article , A.ThreadColorID
                                    from ThreadRequisition_Detail A
                                    left join ThreadRequisition_Detail_Cons B on B.Ukey=A.Ukey
                                    where A.orderid= '{0}'
                                    group by article, threadcolorid", POID);
                result = DBProxy.Current.Select(null, sql, out dtPrint);
                if (!result) return result;

                sql = string.Format(@"select distinct B.Article 
                                    from ThreadRequisition_Detail A
                                    left join ThreadRequisition_Detail_Cons B on B.Ukey=A.Ukey
                                    where A.orderid= '201109391N'", POID);
                result = DBProxy.Current.Select(null, sql, out dtPrint2);
                if (!result) return result;
                #endregion
            }
            #endregion

            try
            {
                e.Report.ReportDataSource = dtPrint;
                SetCount(dtPrint.Rows.Count);
                if (dtPrint != null && dtPrint.Rows.Count > 0)
                {
                    //顯示筆數
                    SetCount(dtPrint.Rows.Count);
                    return transferToExcel();
                }
                else
                {
                    return new DualResult(false, "Data not found.");
                }
            }
            catch (Exception ex)
            {
                return new DualResult(false, "data loading error.", ex);
            }     
        }

        private DualResult transferToExcel()
        {
            DualResult result = Result.True;
            decimal intRowsCount = dtPrint.Rows.Count;
            int page = Convert.ToInt16(Math.Ceiling(intRowsCount / 6));
            Object temfile; ;

            if (radioOther.Checked)
                temfile = Sci.Env.Cfg.XltPathDir + "\\Warehouse-P01.TrimCardPrint_B.dot";
            else
                temfile = Sci.Env.Cfg.XltPathDir + "\\Warehouse-P01.TrimCardPrint_A.dot";

            Microsoft.Office.Interop.Word._Application winword = new Microsoft.Office.Interop.Word.Application();

            //Set status for word application is to be visible or not.
            winword.Visible = false;

            //Create a missing variable for missing value
            object missing = System.Reflection.Missing.Value;

            //Create a new document
            Microsoft.Office.Interop.Word._Document document = winword.Documents.Add(ref temfile, ref missing, ref missing, ref missing);

            Word.Table tables = null;

            try
            {
                document.Activate();
                Word.Tables table = document.Tables;

                //retreive the first table of the document
                tables = table[1];

                #region ROW1
                if (radioOther.Checked)
                    tables.Cell(1, 1).Range.Text = string.Format("LABELLING & PACKAGING (SP# {0})", orderID);
                else
                    tables.Cell(1, 1).Range.Text = string.Format("SP#{0}     ST:{1}     SEASON:{2}     FTY:{3}", orderID, StyleID, SeasonID, FactoryID);
                #endregion

                #region ROW2
                if (radioFabric.Checked)
                {
                    for (int i = 2; i <= tables.Columns.Count; i++) tables.Cell(2, i).Range.Text = "FABRIC";
                }
                else if (radioAccessory.Checked)
                {
                    for (int i = 2; i <= tables.Columns.Count; i++) tables.Cell(2, i).Range.Text = "ACCESSORY";
                }
                else if (radioOther.Checked)
                {
                    for (int i = 2; i <= tables.Columns.Count; i++) tables.Cell(2, i).Range.Text = "OTHER";
                }
                else if (radioThread.Checked)
                {
                    for (int i = 2; i <= tables.Columns.Count; i++) tables.Cell(2, i).Range.Text = "Thread";
                }
                #endregion

                #region ROW4 抬頭
                if (radioFabric.Checked || radioAccessory.Checked || radioThread.Checked)
                {
                    for (int i = 0; i < dtPrint2.Rows.Count; i++)
                        tables.Cell(i + 4, 1).Range.Text = dtPrint2.Rows[i]["Article"].ToString().Trim();
                }
                else if (radioOther.Checked)
                {
                    for (int i = 0; i < dtPrint2.Rows.Count; i++)
                        tables.Cell(i + 4+(3*(i/7)), 1).Range.Text = dtPrint2.Rows[i]["ID"].ToString().Trim();
                }
                #endregion

                #region 複製第1頁的格式到後面幾頁
                winword.Selection.Tables[1].Select();
                winword.Selection.Copy();
                for (int i = 1; i < page; i++)
                {
                    winword.Selection.MoveDown(Microsoft.Office.Interop.Word.WdUnits.wdLine, 7);
                    winword.Selection.Paste();
                }
                #endregion

                //Word.Tables AllTable = document.Tables;

                #region [ROW3][ROW4]
                int count = 1;
                int NowPage = 1;
                int columnIndex;
                if (radioFabric.Checked)
                {
                    #region Fabric
                    foreach (DataRow row in dtPrint.Rows)
                    {
                        NowPage = ((count - 1) / 6);  //從0開始(例:0,1,2...)
                        columnIndex = ((count - 1) % 6) + 2;

                        temp = "Pattern Panel:" + row["Lectracode"].ToString().Trim() + Environment.NewLine
                             + "Fabric Code:" + row["FabricCode"].ToString().Trim() + Environment.NewLine
                             + row["Refno"].ToString().Trim() + Environment.NewLine
                             + row["Description"].ToString().Trim();
                        tables.Cell(3 + 7 * NowPage, columnIndex).Range.Text = temp;

                        for (int i = 0; i < dtPrint2.Rows.Count; i++)
                        {
                            sql = string.Format(@"LectraCode='{0}' and Article='{1}'", row["Lectracode"].ToString().Trim(), dtPrint2.Rows[i]["Article"].ToString().Trim());
                            
                            if (dtColor.Select(sql).Length > 0)
                            {
                                rowColor = dtColor.Select(sql)[0];
                                temp = rowColor["Name"].ToString().Trim() + Environment.NewLine + rowColor["ColorID"].ToString().Trim();
                            }
                            else
                            {
                                temp = "";
                            }
                                
                            tables.Cell((4 + i) + (7 * NowPage), columnIndex).Range.Text = temp;

                        }

                        count++;
                    }
                    #endregion
                }
                else if (radioAccessory.Checked)
                {
                    #region Accessory
                    foreach (DataRow row in dtPrint.Rows)
                    {
                        NowPage = ((count - 1) / 6);  //從0開始(例:0,1,2...)
                        columnIndex = ((count - 1) % 6) + 2;

                        temp = row["Refno"].ToString().Trim() + Environment.NewLine 
                             + row["Description"].ToString().Trim();
                        tables.Cell(3 + 7 * NowPage, columnIndex).Range.Text = temp;

                        
                        for (int i = 0; i < dtPrint2.Rows.Count; i++)
                        {
                            sql = string.Format(@"Refno='{0}' and Article='{1}'", row["Refno"].ToString().Trim(), dtPrint2.Rows[i]["Article"].ToString().Trim());

                            ListColor.Clear();
                            colorName = string.Empty;
                            ColorID = string.Empty;
                            if (dtColor.Select(sql).Length > 0)
                            {
                                foreach (DataRow rowC in dtColor.Select(sql))
                                {
                                    temp = string.Join(",", rowC["colorID"].ToString().Trim().Split(',').Select(s => s.Trim()).ToArray());
                                    ListColor.AddRange(temp.Split(',').ToList());
                                }
                                ListColor = ListColor.Distinct().ToList();  //去除重複的值

                                foreach (string color in ListColor)
                                {
                                    colorName += dtColor2.Select(string.Format("ID='{0}'", color))[0]["Name"].ToString() + "/" ;
                                    ColorID += color + "/";
                                }
                                colorName = colorName.TrimEnd('/');
                                ColorID = ColorID.TrimEnd('/');

                                temp = colorName + Environment.NewLine + ColorID;
                            }
                            else
                            {
                                temp = "";
                            }

                            tables.Cell((4 + i) + (7 * NowPage), columnIndex).Range.Text = temp;

                        }

                        count++;
                    }
                    #endregion
                }
                else if (radioOther.Checked)
                {
                    foreach (DataRow row in dtPrint.Rows)
                    {
                        NowPage = ((count - 1) / 6);  //從0開始(例:0,1,2...)
                        columnIndex = ((count - 1) % 6) + 2;

                        temp = row["Refno"].ToString().Trim() + Environment.NewLine
                             + row["DescDetail"].ToString().Trim();
                        tables.Cell(3 + 10 * NowPage, columnIndex).Range.Text = temp;
                        
                        count++;
                    }
                }
                else if (radioThread.Checked)
                {
                    #region Thread
                    foreach (DataRow row in dtPrint.Rows)
                    {
                        NowPage = ((count - 1) / 6);  //從0開始(例:0,1,2...)
                        columnIndex = ((count - 1) % 6) + 2;

                        for (int i = 0; i < dtPrint2.Rows.Count; i++)
                        {
                            sql = string.Format(@"Article='{1}'", dtPrint2.Rows[i]["Article"].ToString().Trim());

                            if (dtPrint.Select(sql).Length > 0)
                            {
                                temp = dtPrint.Select(sql)[0].ToString().Trim();
                            }
                            else
                            {
                                temp = "";
                            }

                            tables.Cell((4 + i) + (7 * NowPage), columnIndex).Range.Text = temp;

                        }

                        count++;
                    }
                    #endregion  
                }
                #endregion          

                winword.Quit(ref missing, ref missing, ref missing);     //close word application
                winword = null;

                return Result.True;
            }
            catch (Exception ex)
            {
                if (null != winword)
                    winword.Quit(ref missing, ref missing, ref missing);
                return new DualResult(false, "Export word error.", ex);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                //Marshal.FinalReleaseComObject(winword);
            }

        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            return true;
        }


    }
}
