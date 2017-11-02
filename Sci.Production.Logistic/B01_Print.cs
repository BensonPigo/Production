using System;
using System.Data;
using System.Text;
using Ict;
using Sci.Data;
using Word = Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;

namespace Sci.Production.Logistic
{
    public partial class B01_Print : Sci.Win.Tems.PrintForm
    {
        private DataRow masterData;
        private string code1;
        private string code2;
        private DataTable printData;

        public string Code2
        {
            get
            {
                return this.code2;
            }

            set
            {
                this.code2 = value;
            }
        }

        public B01_Print(DataRow master)
        {
            this.InitializeComponent();
            this.masterData = master;
            this.txtCodeStart.Text = MyUtility.Convert.GetString(this.masterData["ID"]);
            this.txtCodeEnd.Text = MyUtility.Convert.GetString(this.masterData["ID"]);
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            this.code1 = this.txtCodeStart.Text;
            this.Code2 = this.txtCodeEnd.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format("select ID from ClogLocation WITH (NOLOCK) where MDivisionID = '{0}' and Junk = 0", Sci.Env.User.Keyword));
            if (!MyUtility.Check.Empty(this.code1))
            {
                sqlCmd.Append(string.Format(" and ID >= '{0}'", this.code1));
            }

            if (!MyUtility.Check.Empty(this.Code2))
            {
                sqlCmd.Append(string.Format(" and ID <= '{0}'", this.Code2));
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.Description);
                return result;
            }

            //e.Report.ReportDataSource = printData;
            return Result.True;
        }

        protected override bool ToPrint()
        {
            this.ValidateInput();
            this.OnAsyncDataLoad(null);
            if (printData == null || printData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }

            this.SetCount(printData.Rows.Count);
            this.ShowWaitMessage("Data Loading ...");
            Microsoft.Office.Interop.Word._Application winword = new Microsoft.Office.Interop.Word.Application();
            winword.FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip;
            winword.Visible = false;
            Microsoft.Office.Interop.Word._Document document;
            Word.Table tables = null;

            Object printFile = Sci.Env.Cfg.XltPathDir + "\\Logistic_B01_Barcode.dotx";
            document = winword.Documents.Add(ref printFile);
            try
            {
                document.Activate();
                Word.Tables table = document.Tables;

                #region 計算頁數
                winword.Selection.Tables[1].Select();
                winword.Selection.Copy();
                int page = (printData.Rows.Count / 6) + ((printData.Rows.Count % 6 > 0) ? 1 : 0);
                for (int i = 1; i < page; i++)
                {
                    winword.Selection.MoveDown();
                    if (page > 1)
                    {
                        winword.Selection.InsertNewPage();
                    }

                    winword.Selection.Paste();
                }
                #endregion
                #region 填入資料
                for (int i = 0; i < page; i++)
                {
                    tables = table[i + 1];
                    for (int p = i * 6; p < (i * 6) + 6; p++)
                    {
                        if (p >= this.printData.Rows.Count)
                        {
                            break;
                        }

                        tables.Cell((p % 6) + 1, 1).Range.Text = "*" + this.printData.Rows[p]["ID"].ToString().Trim() + "*";
                    }
                }
                #endregion
                winword.ActiveDocument.Protect(Word.WdProtectionType.wdAllowOnlyComments, Password: "ScImIs");

                #region Show Word
                winword.Visible = true;
                Marshal.ReleaseComObject(winword);
                Marshal.ReleaseComObject(document);
                Marshal.ReleaseComObject(table);
                #endregion
            }
            catch (Exception ex)
            {
                return new DualResult(false, "Export word error.", ex);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            this.HideWaitMessage();
            return true;
        }
    }
}
