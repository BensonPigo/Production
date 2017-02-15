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
using System.IO;
//using Word = Microsoft.Office.Interop.Word;
using System.Linq;
using System.Runtime.InteropServices;


namespace Sci.Production.Warehouse
{
    public partial class P01_TrimCardPrint : Sci.Win.Tems.PrintForm
    {
        DataTable dtPrint, dtPrint2, dtColor, dtColor2;
        DataRow rowColor;
        string sql, temp, orderID, StyleID, SeasonID, FactoryID, BrandID, POID;
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
                                where Id='{0}' 
                                and LectraCode in (select LectraCode from Order_FabricCode where ID='{0}')", orderID);
                result = DBProxy.Current.Select(null, sql, out dtPrint2);
                if (!result) return result;

                sql = string.Format(@"select ColorID , B.Name , LectraCode , Article
                                from Order_ColorCombo A
                                left join Color B on B.BrandId='{0}' and B.ID=A.ColorID
                                where A.Id='{1}'
                                and LectraCode in (
	                                select A.Lectracode 
	                                from Order_FabricCode A
	                                left join Order_BOF B on B.Id=A.Id and B.FabricCode=A.FabricCode
	                                left join Fabric C on C.SCIRefno=B.SCIRefno
	                                where A.ID='{1}'
                                )"
                                , BrandID, orderID);
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
                                    and b.MtlTypeID in (select id from MtlType where IsTrimcardOther=0)
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

                sql = string.Format(@"select distinct A.Refno, article , ColorId,c.Name
                                    from Order_BOA_Expend A
                                    left join Fabric B on B.SCIRefno=A.SCIRefno
                                    left join Color C on C.BrandId='{1}' and C.ID=A.ColorID
                                    where A.Id='{0}' and Article<>'' and ColorId<>''
                                    and a.Refno in (
	                                    select A.Refno
	                                    from Order_BOA_Expend A
	                                    left join Fabric B on B.SCIRefno=A.SCIRefno
	                                    where Id='{0}' and Article<>'' and ColorId<>''
	                                    group by A.Refno, B.Description 
                                    )"
                                    , orderID, BrandID);
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
                sql = string.Format(@"
select distinct A.Refno,B.DescDetail
from Order_BOA_Expend A
inner join Order_BOA ob on A.Order_BOAUkey = ob.Ukey
left join Fabric B on B.SCIRefno=A.SCIRefno
where a.Id='{0}'
and b.MtlTypeID in (select id from MtlType where IsTrimcardOther=1)
and not ob.SuppID = 'fty' 
and not ob.SuppID = 'fty-c'
                                    ", orderID);
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
            {//jimmy 記得把 註解 & isnull 拿掉
                #region Thread
                sql = string.Format(@"select distinct B.Article
                                    from ThreadRequisition_Detail A
                                    left join ThreadRequisition_Detail_Cons B on B.Ukey=A.Ukey
                                    where A.orderid= '{0}' and article<>''", POID);
                result = DBProxy.Current.Select(null, sql, out dtPrint2);
                if (!result) return result;

                sql = string.Format(@"select B.Article, A.ThreadColorID
                                    from ThreadRequisition_Detail A
                                    left join ThreadRequisition_Detail_Cons B on B.Ukey=A.Ukey
                                    where A.orderid= '{0}' and article<>''
                                    group by article, threadcolorid", POID);
                result = DBProxy.Current.Select(null, sql, out dtPrint);
                if (!result) return result;
                #endregion
            }
            #endregion

            
            if (dtPrint != null && dtPrint.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                SetCount(0);
                MessageBox.Show("Data not found!!");
                return false;
            }
        }

        protected override Ict.DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            e.Report.ReportDataSource = dtPrint;

            try
            {
                //顯示筆數
                SetCount(dtPrint.Rows.Count);
                return transferToExcel();
            }
            catch (Exception ex)
            {
                return new DualResult(false, "data loading error.", ex);
            }     
        }

        private DualResult transferToExcel()
        {
            DualResult result = Result.True;
            //decimal intRowsCount = dtPrint.Rows.Count;
            //int page = Convert.ToInt16(Math.Ceiling(intRowsCount / 4));
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

            Microsoft.Office.Interop.Word.Table tables = null;

            try
            {
                document.Activate();
                Microsoft.Office.Interop.Word.Tables table = document.Tables;

                //retreive the first table of the document
                tables = table[1];

                #region ROW1
                 if (radioOther.Checked)
                    if(dtPrint2.Rows.Count > 1)
                        tables.Cell(1, 1).Range.Text = string.Format("LABELLING & PACKAGING (SP# {0} - {1})", orderID, dtPrint2.Rows[dtPrint2.Rows.Count - 1]["id"].ToString().Substring(8));
                    else
                        tables.Cell(1, 1).Range.Text = string.Format("LABELLING & PACKAGING (SP# {0})", orderID);
                else
                    tables.Cell(1, 1).Range.Text = string.Format("SP#{0}     ST:{1}     SEASON:{2}     FTY:{3}", orderID, StyleID, SeasonID, FactoryID);
                #endregion

                #region ROW2
                 string Row2Type = "";
                 //if (radioFabric.Checked)
                 //{
                 //    for (int i = 2; i <= tables.Columns.Count; i++) tables.Cell(2, i).Range.Text = "FABRIC";
                 //}
                 //else if (radioAccessory.Checked)
                 //{
                 //    for (int i = 2; i <= tables.Columns.Count; i++) tables.Cell(2, i).Range.Text = "ACCESSORY";
                 //}
                 //else if (radioOther.Checked)
                 //{
                 //    for (int i = 2; i <= tables.Columns.Count; i++) tables.Cell(2, i).Range.Text = "OTHER";
                 //}
                 //else if (radioThread.Checked)
                 //{
                 //    for (int i = 2; i <= tables.Columns.Count; i++) tables.Cell(2, i).Range.Text = "Thread";
                 //}
                 if (radioFabric.Checked)
                 {
                     Row2Type = "FABRIC";
                 }
                 else if (radioAccessory.Checked)
                 {
                     Row2Type = "ACCESSORY";
                 }
                 else if (radioOther.Checked)
                 {
                     Row2Type = "OTHER";
                 }
                 else if (radioThread.Checked)
                 {
                     Row2Type = "Thread";
                 }
                #endregion

                #region 計算共要幾頁
                int pagecount;
                int CC, rC;
                if (radioOther.Checked)
                {
                    CC = (dtPrint.Rows.Count - 1) / 6 + 1;
                    rC = (dtPrint2.Rows.Count - 1) / 7 + 1;
                    pagecount = CC * rC;
                }
                else
                {
                    CC = (dtPrint.Rows.Count - 1) / 6 + 1;
                    rC = (dtPrint2.Rows.Count - 1) / 4 + 1;
                    pagecount = CC * rC;
                }
                #endregion

                #region 複製第1頁的格式到後面幾頁
                winword.Selection.Tables[1].Select();
                winword.Selection.Copy();
                for (int i = 1; i < pagecount; i++)
                {
                    winword.Selection.MoveDown();
                    if (pagecount > 1)
                        winword.Selection.InsertBreak();
                    winword.Selection.Paste();
                }
                #endregion

                #region ROW4開始 左側抬頭
                int nextPage = 1;
                tables = table[nextPage];
                if (radioFabric.Checked || radioAccessory.Checked || radioThread.Checked)
                {
                    for (int j = 0; j < CC; j++)
                    {
                        for (int i = 0; i < dtPrint2.Rows.Count; i++)
                        {
                            //根據 DataRow 數量選取 Table, Dot DataRow = 4
                            tables = table[nextPage + (i / 4)];
                            tables.Cell(4 + (i % 4), 1).Range.Text = dtPrint2.Rows[i]["Article"].ToString().Trim();
                            //tables.Cell((i + 4 + 3 * (i / 4)) + rC * j * 7, 1).Range.Text = dtPrint2.Rows[i]["Article"].ToString().Trim();
                        }
                        nextPage += rC;
                        if (!(nextPage > pagecount))
                            tables = table[nextPage];
                    }
                }
                else if (radioOther.Checked)
                {
                    for (int j = 0; j < CC; j++)
                    {
                        for (int i = 0; i < dtPrint2.Rows.Count; i++)
                        {
                            //根據 DataRow 數量選取 Table, Dot DataRow = 7
                            tables = table[nextPage + (i / 7)];
                            tables.Cell(4 + (i % 7), 1).Range.Text = dtPrint2.Rows[i]["ID"].ToString().Trim().Substring(8); ;
                            //tables.Cell(i + 4 + (3 * (i / 7)) + rC * j * 10, 1).Range.Text = dtPrint2.Rows[i]["ID"].ToString().Trim().Substring(8);
                        }
                        nextPage += rC;
                        if (!(nextPage > pagecount))
                            tables = table[nextPage];
                    }
                }
                #endregion

                #region [ROW3]欄位名,[ROW4]~[ROW7]對應資料
                nextPage = 1;
                tables = table[nextPage];

                if (radioFabric.Checked)
                {
                    for (int i = 0; i < dtPrint.Rows.Count; i++)
                    {
                        #region 準備欄位名稱
                        temp = "Pattern Panel:" + dtPrint.Rows[i]["Lectracode"].ToString() + Environment.NewLine
                             + "Fabric Code:" + dtPrint.Rows[i]["FabricCode"].ToString().Trim() + Environment.NewLine
                             + dtPrint.Rows[i]["Refno"].ToString().Trim() + Environment.NewLine
                             + dtPrint.Rows[i]["Description"].ToString().Trim();
                        //填入欄位名稱,從第一欄開始填入需要的頁數
                        for (int j = 0; j < rC; j++)
                        {
                            //根據 DataColumn 選取 Table => 首頁 + (DataColumnIndex / 6 * 每個 LectraCode 會占用的 Table 數) + 目前是編輯第 j 個 Table
                            //其中 6 代表, 每個 Table 可以存的 LectraCode 數量
                            tables = table[nextPage + (i / 6 * rC) + j];

                            //有資料時才顯示Type
                            tables.Cell(2, (2 + (i % 6))).Range.Text = Row2Type;
                            tables.Cell(3, (2 + (i % 6))).Range.Text = temp;
                        }
                        #endregion

                        #region 填入Datas
                        for (int k = 0; k < dtPrint2.Rows.Count; k++)
                        {
                            //準備filter字串
                            sql = string.Format(@"LectraCode='{0}' and Article='{1}'"
                                , dtPrint.Rows[i]["Lectracode"].ToString().Trim(), dtPrint2.Rows[k]["Article"].ToString().Trim());
                            if (dtColor.Select(sql).Length > 0)
                            {//找出對應的Datas組起來
                                rowColor = dtColor.Select(sql)[0];
                                temp = rowColor["Name"].ToString().Trim() + Environment.NewLine + rowColor["ColorID"].ToString().Trim();
                            }
                            else
                            {
                                temp = "";
                            }
                            //根據 DataColumn & DataRow 選取 Table => 首頁 + (DataColumnIndex / 6 * 每個 LectraCode 會占用的 Table 數) + k / Table 可存的 Article 數量
                            //其中 K 代表, 目前編輯到 LectraCode 的第幾個 Article
                            tables = table[nextPage + (i / 6 * rC) + (k / 4)];

                            //填入字串
                            tables.Cell((4 + (k % 4)), (2 + (i % 6))).Range.Text = temp;
                        }
                        #endregion
                        //調整頁數
                    }
                }
                else if (radioAccessory.Checked) 
                {
                    for (int i = 0; i < dtPrint.Rows.Count; i++)
                    {
                        #region 準備欄位名稱
                        temp = dtPrint.Rows[i]["Refno"].ToString() + Environment.NewLine
                             + dtPrint.Rows[i]["Description"].ToString().Trim();
                        //填入欄位名稱,從第一欄開始填入需要的頁數
                        for (int j = 0; j < rC; j++)
                        {
                            //根據 DataColumn 選取 Table => 首頁 + (DataColumnIndex / 6 * 每個 LectraCode 會占用的 Table 數) + 目前是編輯第 j 個 Table
                            //其中 6 代表, 每個 Table 可以存的 LectraCode 數量
                            tables = table[nextPage + (i / 6 * rC) + j];

                            //有資料時才顯示Type
                            tables.Cell(2, (2 + (i % 6))).Range.Text = Row2Type;
                            tables.Cell(3, (2 + (i % 6))).Range.Text = temp;
                        }
                        #endregion
                        #region 填入Datas
                        for (int k = 0; k < dtPrint2.Rows.Count; k++)
                        {
                            //準備filter字串
                            sql = string.Format(@"Refno='{0}' and Article='{1}'"
                                , dtPrint.Rows[i]["Refno"].ToString().Trim(), dtPrint2.Rows[k]["Article"].ToString().Trim());
                            if (dtColor.Select(sql).Length > 0)
                            {
                                rowColor = dtColor.Select(sql)[0];
                                temp = rowColor["Name"].ToString().Trim() + Environment.NewLine + rowColor["ColorID"].ToString().Trim();
                            }
                            else
                            {
                                temp = "";
                            }

                            //根據 DataColumn & DataRow 選取 Table => 首頁 + (DataColumnIndex / 6 * 每個 LectraCode 會占用的 Table 數) + k / Table 可存的 Article 數量
                            //其中 K 代表, 目前編輯到 LectraCode 的第幾個 Article
                            tables = table[nextPage + (i / 6 * rC) + (k / 4)];

                            //填入字串
                            tables.Cell((4 + (k % 4)), (2 + (i % 6))).Range.Text = temp;
                        }
                        #endregion
                    }
                }
                else if (radioOther.Checked)
                {
                    for (int i = 0; i < dtPrint.Rows.Count; i++)
                    {
                        #region 準備欄位名稱
                        temp = dtPrint.Rows[i]["Refno"].ToString() + Environment.NewLine
                             + dtPrint.Rows[i]["DescDetail"].ToString().Trim();
                        //填入欄位名稱,從第一欄開始填入需要的頁數
                        for (int j = 0; j < rC; j++)
                        {
                            //根據 DataColumn 選取 Table => 首頁 + (DataColumnIndex / 6 * 每個 LectraCode 會占用的 Table 數) + 目前是編輯第 j 個 Table
                            //其中 6 代表, 每個 Table 可以存的 LectraCode 數量
                            tables = table[nextPage + (i / 6 * rC) + j];

                            //有資料時才顯示Type
                            tables.Cell(2, (2 + (i % 6))).Range.Text = Row2Type;
                            tables.Cell(3, (2 + (i % 6))).Range.Text = temp;
                        }                       
                        #endregion
                    }
                }               
                else if (radioThread.Checked)
                {
                    for (int i = 0; i < dtPrint.Rows.Count; i++)
                    {
                        for (int j = 0; j < rC; j++)
                        {
                            //根據 DataColumn 選取 Table => 首頁 + (DataColumnIndex / 6 * 每個 LectraCode 會占用的 Table 數) + 目前是編輯第 j 個 Table
                            //其中 6 代表, 每個 Table 可以存的 LectraCode 數量
                            tables = table[nextPage + (i / 6 * rC) + j];

                            //有資料時才顯示Type
                            tables.Cell(2, (2 + (i % 6))).Range.Text = Row2Type;
                        }
                    }
                    #region 填入Datas
                    for (int k = 0; k < dtPrint2.Rows.Count; k++)
                    {
                        //準備filter字串
                        sql = string.Format(@"Article='{0}'"
                            ,dtPrint2.Rows[k]["Article"].ToString().Trim());
                        if (dtPrint.Select(sql).Length > 0)
                        {
                            DataRow[] rowColorA = dtPrint.Select(sql);
                            for (int l = 0; l < rowColorA.Count(); l++)
                            {
                                //填入字串
                                temp = rowColorA[l]["ThreadColorID"].ToString().Trim();

                                //根據 DataColumn & DataRow 選取 Table => 首頁 + (DataColumnIndex / 6 * 每個 LectraCode 會占用的 Table 數) + k / Table 可存的 Article 數量
                                //其中 K 代表, 目前編輯到 LectraCode 的第幾個 Article
                                tables = table[nextPage + (l / 6 * rC) + (k / 4)];


                                tables.Cell((4 + (k % 4)), (2 + (l % 6))).Range.Text = temp;
                            }
                        }
                    }
                    #endregion
                }
                #endregion
                winword.Visible = true;
               // winword.Quit(ref missing, ref missing, ref missing);     //close word application
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
