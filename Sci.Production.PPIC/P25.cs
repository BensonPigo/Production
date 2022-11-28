using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Sci.MyUtility;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.PPIC
{
    public partial class P25 : Sci.Win.Tems.QueryForm
    {
        public P25(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.EditMode = true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // Grid設定
            this.grid.IsEditingReadOnly = false;
            this.grid.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("BrandID", header: "Brand", width: Widths.Auto(),iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.Auto(), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.Auto(), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.Auto(), iseditingreadonly: true)
                .Text("Category", header: "Category", width: Widths.Auto(), iseditingreadonly: true)
                .Date("SCIDelivery", header: "SCI Delivery", width: Widths.Auto(), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.Auto(), iseditingreadonly: true)
                .Text("FactoryID", header: "Factory", width: Widths.Auto(), iseditingreadonly: true)
                .Text("Region", header: "Region", width: Widths.Auto(), iseditingreadonly: true)
                .Text("SizePage", header: "Size Page", width: Widths.Auto(), iseditingreadonly: true)
                .Image("...", header: "   ", image: Sci.Properties.Resources.WZLOCATE, settings: this.ColButtonSettings())
                .Text("BrandGender", header: "Gender", width: Widths.Auto(), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.Auto(), iseditingreadonly: true)
                .Text("SizeCode", header: "Sourcing Size", width: Widths.Auto(), iseditingreadonly: true)
                .Text("CustOrderSize", header: "CustOrderSize", width: Widths.Auto(), iseditingreadonly: true)
                .Text("Location", header: "Location", width: Widths.Auto(), iseditingreadonly: true)
                .Text("MoldRef", header: "Mold#Refno", width: Widths.Auto(), iseditingreadonly: true)
                .Text("LabelFor", header: "Label For", width: Widths.Auto(), iseditingreadonly: true)
                .Text("AgeGroup", header: "Age Group", width: Widths.Auto(), iseditingreadonly: true)
                .Text("SuppID", header: "Supplier", width: Widths.Auto(), iseditingreadonly: true)
                .Text("MainSize", header: "Main Size", width: Widths.Auto(), iseditingreadonly: true)
                .Text("SizeSpec", header: "SizeSpec", width: Widths.Auto(), iseditingreadonly: true)
                .Text("Remark", header: "Remark", width: Widths.Auto(), iseditingreadonly: true)
                ;
        }

        private DataGridViewGeneratorImageColumnSettings ColButtonSettings()
        {
            var settings = new DataGridViewGeneratorImageColumnSettings();
            settings.CellMouseClick += (s, e) =>
            {
                if (!this.EditMode || e.Button != MouseButtons.Left || e.RowIndex < 0)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                var frm = new Sci.Production.PPIC.P25_PadPrintInUse(dr);
                frm.ShowDialog();
            };
            return settings;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFindNow_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateSCIDel.Value1) || MyUtility.Check.Empty(this.dateSCIDel.Value2))
            {
                MyUtility.Msg.WarningBox("SCI Del cannot be empty.");
                return;
            }

            this.Find();
        }

        private void Find()
        {
            this.ShowWaitMessage("Data Loading...");
            #region 檢查必填欄位
            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.txtbrand.Text))
            {
                where += Environment.NewLine + $" and o.BrandID = '{this.txtbrand.Text}'";
            }

            if (!MyUtility.Check.Empty(this.dateSCIDel.Value1) || !MyUtility.Check.Empty(this.dateSCIDel.Value2))
            {
                where += Environment.NewLine + $" and o.SciDelivery between '{((DateTime)this.dateSCIDel.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateSCIDel.Value2).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.txtstyle.Text))
            {
                where += Environment.NewLine + $" and o.StyleID = '{this.txtstyle.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                where += Environment.NewLine + $" and o.FactoryID = '{this.txtfactory.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSMR.TextBox1Binding))
            {
                where += Environment.NewLine + $" and o.SMR = '{this.txtSMR.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtHandle.TextBox1Binding))
            {
                where += Environment.NewLine + $" and o.MRHandle = '{this.txtHandle.Text}'";
            }
            #endregion

            string sqlcmd = $@"
WITH temp_PadPrint_Mold
(SuppID,Refno,BrandID,PadPrint_ukey,MoldID,Season,LabelFor,MainSize,Gender,AgeGroup,SizeSpec,Part,Region,Junk)
as(
    select  p.SuppID,p.Refno,p.BrandID,pm.PadPrint_ukey,pm.MoldID,pm.Season,pm.LabelFor,pm.MainSize,pm.Gender,pm.AgeGroup,pm.SizeSpec
    ,case when pm.LabelFor='O' then (select name from DropDownList where Type = 'PadPrint_Part'  and id = pm.Part) else pm.Part end as Part,pm.Region,p.Junk
    from padprint p 
    inner join PadPrint_Mold pm on p.Ukey = pm.PadPrint_ukey 
    where p.Junk=0
),
temp_PadPrint_Mold_Spec (PadPrint_ukey,MoldID,MoldRef,Side,SizePage,SourceSize,CustomerSize,Version) 
AS (
   select pms.PadPrint_ukey,pms.MoldID,pms.MoldRef,pms.Side,case when pms.SizePage ='J4/J5' then 'J4' else pms.SizePage end as SizePage ,pms.SourceSize,pms.CustomerSize,pms.Version
   from  PadPrint_Mold_Spec pms 
    where pms.Junk=0
   union
   select pms.PadPrint_ukey,pms.MoldID,pms.MoldRef,pms.Side,case when pms.SizePage ='J4/J5' then 'J5' else pms.SizePage end as SizePage ,pms.SourceSize,pms.CustomerSize,pms.Version
   from  PadPrint_Mold_Spec pms 
    where pms.Junk=0
),
temp_PadPrint (SuppID,Refno,BrandID,PadPrint_ukey,MoldID,Season,LabelFor,MainSize,Gender,AgeGroup,SizeSpec,Part,Region,Junk,Side,SizePage,SourceSize,CustomerSize,MoldRef,Version
) AS (
   select a.SuppID,Refno,BrandID,a.PadPrint_ukey,a.MoldID,Season,LabelFor,MainSize,Gender,AgeGroup,SizeSpec,Part,Region,Junk,Side,SizePage,SourceSize,CustomerSize,MoldRef,Version
   from temp_PadPrint_Mold a 
    inner join temp_PadPrint_Mold_Spec b on a.PadPrint_ukey = b.PadPrint_ukey and a.MoldID = b.MoldID
),
temp_Order_SizeSpec (Id,SizeCode,SizeSpec)
AS
(
    select oqd.Id,oqd.SizeCode,isnull(os.SizeSpec,'') 
    from Orders o 
    inner join Order_QtyShip_Detail oqd  on o.ID = oqd.ID
    Left join Style_CustOrderSize os on o.StyleUkey = os.StyleUkey
        and os.SizeCode = oqd.SizeCode and oqd.Qty>0
    where o.SciDelivery between '{((DateTime)this.dateSCIDel.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateSCIDel.Value2).ToString("yyyy/MM/dd")}'
     group by oqd.Id,oqd.SizeCode,os.SizeSpec
),
Order_QtyShip_Detail_temp (Id,SizeCode,qty)
AS
(
    select oqd.Id,oqd.SizeCode,sum(oqd.qty) 
    from Order_QtyShip_Detail oqd 
    inner join orders o on o.ID = oqd.Id  
    where o.SciDelivery between '{((DateTime)this.dateSCIDel.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateSCIDel.Value2).ToString("yyyy/MM/dd")}'
    and o.Qty>0
    group by oqd.Id,oqd.SizeCode
),
style_temp(Ukey,SizePage,BrandGender,type,Location)
as
(
SELECT A.Ukey,A.SizePage,A.BrandGender,'1', B.value
    FROM(
        SELECT s.Ukey,s.SizePage,s.BrandGender,Location= CONVERT(xml,'<root><v>' + REPLACE(s.Location, ',', '</v><v>') + '</v></root>') FROM
		Style s inner join orders o on o.StyleUkey = s.Ukey
		where o.SciDelivery between '{((DateTime)this.dateSCIDel.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateSCIDel.Value2).ToString("yyyy/MM/dd")}'
    )A
    OUTER APPLY(
        SELECT value = N.v.value('.', 'varchar(100)') FROM A.Location.nodes('/root/v') N(v)
    )B
	union
	SELECT s.Ukey,s.SizePage,s.BrandGender,'2',s.Location from style s inner
	join orders o on o.StyleUkey = s.Ukey
	where o.SciDelivery between '{((DateTime)this.dateSCIDel.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateSCIDel.Value2).ToString("yyyy/MM/dd")}'
)
----Grid1
select
o.BrandID
,[OrderID] = o.ID
,o.StyleID
,o.SeasonID
,o.Category
,o.SCIDelivery
,o.BuyerDelivery
,o.FactoryID
,f.PadPrintGroup as Region
,s.SizePage
,s.BrandGender
,ob.refno
,oqd.SizeCode
,[CustOrderSize] = os.SizeSpec
,s.Location
,p.MoldRef
,[LabelFor] = case pl.LabelFor 
				when 'O' then 'One Asia' 
				when 'E' then 'EMEA' 
                else pl.LabelFor end
,[AgeGroup] = AgeGroupName.Name
,p.SuppID
,[MainSize] = MainSizeName.Name
,p.SizeSpec
,[Remark] = case when isnull(p.MoldRef,'') != '' then ''
			else iif(isnull(o.FactoryID,'') = '',' <Orders.FactoryID>','') + 
			iif(isnull(s.SizePage,'') = '' ,' <Style.SizePage>' ,'')+
			iif(isnull(s.BrandGender,'') = '', ' <Style.BrandGender>','')+
			iif(isnull(p.SizeSpec,'') = '', ' <Style_CustOrderSize>','')+
			' is Null' end
from Orders o
Left join Factory f on f.ID = o.FactoryID
inner join (
    select Id,Refno,SCIRefno,SuppID 
    from Order_BOA 
    where Refno like '%PAD PRINT%' 
    group by Id,Refno,SCIRefno,SuppID
) ob on  (ob.Id = o.POID )
left join (
    select distinct p.Refno,pm.LabelFor 
    from PadPrint p 
    inner join PadPrint_Mold pm on p.Ukey = pm.PadPrint_ukey
) pl on pl.Refno =  SUBSTRING(ob.Refno,0,CHARINDEX('-',ob.Refno))
Left join style_temp s on s.type = (case when pl.LabelFor = 'E' then '2' else '1' end) 
    and s.Ukey = o.StyleUkey
inner join Order_QtyShip_Detail_temp oqd on  oqd.Id = o.Id
Left join temp_Order_SizeSpec os on case when  (pl.LabelFor = 'O' or pl.LabelFor is null) then 1 else 0 end =1  and os.Id = o.ID and os.SizeCode = oqd.SizeCode
Left join Brand_SizeCode bs on bs.BrandID = o.BrandID and bs.SizeCode = oqd.SizeCode
Left join temp_PadPrint p on p.BrandID = o.BrandID and p.Region = f.PadPrintGroup
    and p.LabelFor = pl.LabelFor and p.Refno =pl.Refno
    and p.Gender = (case when pl.LabelFor = 'O' then (select id from DropDownList where  name = s.BrandGender and type ='PadPrint_Gender' ) else p.Gender end)
    and p.part = (case when pl.LabelFor = 'O' then (select name from DropDownList where Type = 'Location' and id = (select top 1 Location from Style_Location where StyleUkey = o.StyleUkey)) else p.part end)
    and p.AgeGroup =  (case when pl.LabelFor = 'O' then bs.AgeGroupID else p.AgeGroup end)
    and p.SizePage = s.SizePage
    and p.SourceSize = oqd.SizeCode
    and isnull(p.CustomerSize,'') =  (case when pl.LabelFor = 'O' then os.SizeSpec else isnull(p.CustomerSize,'') end)
    and p.Junk = 0
Left join DropDownList MainSizeName on MainSizeName.Type = 'PadPrint_MainSize' and MainSizeName.ID = p.MainSize
Left join DropDownList AgeGroupName on AgeGroupName.Type = 'PadPrint_AgeGroup' and AgeGroupName.ID = bs.AgeGroupID
where 1=1
{where}
and isnull(p.version,'')= (select isnull(max(version),'') from temp_PadPrint where Refno = p.Refno and SizePage = p.SizePage and SourceSize = p.SourceSize and  
isnull(Gender,'') = isnull(p.Gender,'')  and 
isnull(AgeGroup,'') = isnull(p.AgeGroup,'')  and 
isnull(SizeSpec,'') = isnull(p.SizeSpec,'')  and 
isnull(Part,'') = isnull(p.Part,'')  and 
(LabelFor='E' or isnull(CustomerSize,'') = isnull(p.CustomerSize,'') ) and 
Region = p.Region 
)
order by o.ID,ob.Refno,oqd.SizeCode
";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail.\r\n" + result.ToString());
            }

            if (gridData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("No data found!!");
                this.HideWaitMessage();
                return;
            }

            this.listControlBindingSource1.DataSource = gridData;
            this.grid.DataSource = this.listControlBindingSource1.DataSource;
            this.grid.AutoResizeColumns();
            this.HideWaitMessage();
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            DataTable excelTable = (DataTable)this.listControlBindingSource1.DataSource;

            // 判斷是否有資料
            if (excelTable == null || excelTable.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }

            this.ShowWaitMessage("Excel Processing...");

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\PPIC_P25.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(excelTable, string.Empty, "PPIC_P25.xltx", 1, false, null, objApp); // 將datatable copy to excel
            Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("PPIC_P25");
            Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
        }
    }
}