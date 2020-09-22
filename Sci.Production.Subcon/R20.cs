using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using Sci.Utility.Excel;

namespace Sci.Production.Subcon
{
    public partial class R20 : Win.Tems.PrintForm
    {
        private string type;
        private string supplier;
        private string SelectedType;
        private DataTable dtPrint;

        public R20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            // 下拉選單設定方式：Thread,Thread→一個代表Text、一個代表Value，全部串在一個字串即可
            MyUtility.Tool.SetupCombox(this.comboBoxType, 2, 1, "Thread,Thread,Carton,Carton");
            this.comboBoxType.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            // 預設一個Type "Thread", 之後可能會有Label,Carton....
            switch (this.comboBoxType.Text)
            {
                case "Thread":
                    this.type = @"'SP_THREAD','EMB_THREAD'";
                    break;
                case "Carton":
                    this.type = @"'Carton'";
                    break;
            }

            this.SelectedType = this.comboBoxType.Text;
            this.supplier = this.txtlocalSupp.TextBox1.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            #region Filter
            List<string> listSQLFilter = new List<string>();
            if (!MyUtility.Check.Empty(this.type))
            {
                listSQLFilter.Add($"and l.Category in ({this.type})");
            }

            if (!MyUtility.Check.Empty(this.supplier))
            {
                listSQLFilter.Add($"and l.LocalSuppid='{this.supplier}'");
            }
            #endregion

            #region SqlCmd
            string strcmd = string.Empty;

            // 根據選擇的Type撈資料
            switch (this.SelectedType)
            {
                case "Thread":
                    strcmd = $@"
                                select l.Refno
                                    ,junk
                                    ,Description
                                    ,Category
                                    ,UnitID
                                    ,LocalSuppid
                                    ,l.Price
                                    ,CurrencyID
                                    ,ThreadTypeID
                                    ,ThreadTex
                                    ,Weight
                                    ,AxleWeight
                                    ,MeterToCone
                                    ,QuotDate
                                    ,[AddName]= Ladd.IdAndName
                                    ,[AddDate]=convert(varchar, l.AddDate, 111) + ' ' +LEFT(convert(time, l.AddDate, 108),5)
                                    ,[EditName]=Ledit.IdAndName
                                    ,[EditDate]=convert(varchar, l.EditDate, 111) + ' ' +LEFT(convert(time, l.EditDate, 108),5) 
                                    ,NLCode
                                    ,HSCode
                                    ,CustomsUnit
                                    ,ArtTkt
                                    ,lt.BuyerID
                                    ,lt.ThreadColorGroupID
                                    ,lt.Price
                                    ,[LtAddName] = Ltadd.IdAndName
                                    ,[LtAddDate]= convert(varchar, lt.AddDate, 111) + ' ' +LEFT(convert(time,lt.AddDate, 108),5) 
                                    ,[LtEditName]=  Ltedit.IdAndName 
                                    ,[LtEditDate]= convert(varchar, lt.EditDate, 111) + ' ' +LEFT(convert(time,lt.EditDate, 108),5) 

                                from LocalItem l
                                left join LocalItem_ThreadBuyerColorGroupPrice lt on l.RefNo=lt.Refno
                                LEFT JOIN GetName Ladd ON Ladd.ID=l.AddName
                                LEFT JOIN GetName Ledit ON Ledit.ID=l.EditName
                                LEFT JOIN GetName Ltadd ON Ltadd.ID=lt.AddName
                                LEFT JOIN GetName Ltedit ON Ltedit.ID=lt.EditName
                                where 1=1
                                {listSQLFilter.JoinToString($"{Environment.NewLine} ")}
                                ";
                    break;
                case "Carton":
                    strcmd = $@"
                                select [Refno]=l.Refno
                                    ,[Junk]=l.junk
	                                ,[Description]=l.Description
	                                ,[Category]=l.Category
	                                ,[UnitID]=l.UnitID
	                                ,[LocalSuppid]=l.LocalSuppid
                                    ,[Price]=l.Price
	                                ,[Currency]=l.CurrencyID
	                                ,[QuotDate]=l.QuotDate
	                                ,[L]=l.CtnLength
	                                ,[W]=l.CtnWidth
	                                ,[H]=l.CtnHeight
	                                ,[Unit]=l.CtnUnit
	                                ,[CBM]=l.CBM
	                                ,[CTN. Weigth]=l.CtnWeight
	                                ,[AddName]= Ladd.IdAndName
	                                ,[AddDate]=convert(varchar, l.AddDate, 111) + ' ' +LEFT(convert(time,l.AddDate, 108),5) 
	                                ,[EditName]=Ledit.IdAndName
	                                ,[EditDate]=convert(varchar, l.EditDate, 111) + ' ' +LEFT(convert(time, l.EditDate, 108),5) 
	                                ,[Buyer]=lt.Buyer
	                                ,[Pad Refno]=lt.PadRefno
	                                ,[Qty]=lt.Qty
	                                ,[Create Name]=   Ltadd.IdAndName
	                                ,[Create  Date]=convert(varchar, lt.AddDate, 111) + ' ' +LEFT(convert(time, lt.AddDate, 108),5) 
	                                ,[Edit Name]=  Ltedit.IdAndName 
	                                ,[Edit Date]=convert(varchar, lt.EditDate, 111) + ' ' +LEFT(convert(time, lt.EditDate, 108),5) 
                                from LocalItem l
                                left join LocalItem_CartonCardboardPad lt on l.RefNo=lt.Refno
                                LEFT JOIN GetName Ladd ON Ladd.ID=l.AddName
                                LEFT JOIN GetName Ledit ON Ledit.ID=l.EditName
                                LEFT JOIN GetName Ltadd ON Ltadd.ID=lt.AddName
                                LEFT JOIN GetName Ltedit ON Ltedit.ID=lt.EditName
                                where 1=1
                                {listSQLFilter.JoinToString($"{Environment.NewLine} ")}
                                ";
                    break;
            }
            #endregion

            DualResult res = DBProxy.Current.Select(string.Empty, strcmd, out this.dtPrint);
            if (!res)
            {
                this.ShowErr(res);
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.dtPrint.Rows.Count < 1)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");

            this.SetCount(this.dtPrint.Rows.Count);

            // 建立範本物件
            SaveXltReportCls xl = new SaveXltReportCls("SubCon_R20.xltx")
            {
                BoOpenFile = true,
            };

            // 建立範本物件
            SaveXltReportCls.XltRptTable xdt_All = new SaveXltReportCls.XltRptTable(this.dtPrint)
            {
                ShowHeader = false,   // 表頭範本有了所以False
                BoAutoFitColumn = true,  // 自動調整欄寬
            };

            Microsoft.Office.Interop.Excel.Application excel = xl.ExcelApp;

            // 刪掉沒被選的Type的Sheet
            switch (this.SelectedType)
            {
                case "Thread":

                    xl.DicDatas.Add("##R20Tread", xdt_All);
                    excel.Worksheets[2].Delete();

                    break;

                case "Carton":

                    xl.DicDatas.Add("##R20Carton", xdt_All);
                    excel.Worksheets[1].Delete();
                    break;
            }

            xl.Save(Class.MicrosoftFile.GetName("SubCon_R20"));
            this.HideWaitMessage();
            return true;
        }
    }
}
