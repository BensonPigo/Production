using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class P01_ArtworkTestDox : Sci.Win.Tems.QueryForm
    {
        int StyleUkey;

        public P01_ArtworkTestDox(int styleUkey)
        {
            this.InitializeComponent();
            this.StyleUkey = styleUkey;
        }

        /// <inheritdoc/>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string sqlcmd = $@"
select t.ArtworkTypeID,t.ArtworkID ,t.Article,t.F_FabricPanelCode
,[FabColor] = color.Color
,[FabColorName] = color.ColorName
,t.F_Refno
,[Fab/AccBrandRefno] = BrandRefno.value
,[Finish] = Finish.value
,t.IsA_FabricPanelCodeCanEmpty
,t.A_FabricPanelCode
,[AccColor] = AccColor.Color
,[AccColorName] = AccColor.ColorName
,t.A_Refno
,[AccessorySupplierRefno] = AccSuppRef.value
,[FabricFaceSide] = case t.FabricFaceSide 
					when 'F' then 'Face Side'
					when 'B' then 'Back Side' else '' end
,t.PrintType
,t.TestNo
,[TestResult] = case t.TestResult 
				when 'P' then 'Pass'
				when 'F' then 'Failed' else '' end
,t.OrderID
,[SCIDel] = o.SciDelivery
,[FTY] = t.FactoryID
,t.SubstrateFormSendDate
,t.Remark
,[CreateBy] = (Select dbo.getTPEPass1(t.AddName)+ ' ' + convert(varchar,t.AddDate))
,[EditBy] = (Select dbo.getTPEPass1(t.EditName)+ ' ' +  convert(varchar,t.EditDate))
from Style_ArtworkTestDox t
left join　orders o on o.ID = t.OrderID
outer apply(
	select [Color] = c.ID,[ColorName] = c.Name
	from Style_ColorCombo scc
	left join Style s on s.Ukey = scc.StyleUkey
	left join Color c on c.ID = scc.ColorID and s.BrandID = c.BrandId
	where scc.StyleUkey = t.StyleUkey
	and scc.Article = t.Article
	and scc.FabricPanelCode = t.F_FabricPanelCode
)Color
outer apply(
	select [value] = BrandRefno.BrandRefno
	from (
		select bof.Refno, f.BrandRefno
		from Style_BOF bof
		left join Fabric f on f.SCIRefno = bof.SCIRefno
		where bof.StyleUkey = t.StyleUkey
		union
		select boa.Refno, f.BrandRefno
		from Style_BOA boa
		left join Fabric f on f.SCIRefno = boa.SCIRefno
		where boa.StyleUkey = t.StyleUkey
	) BrandRefno
	where BrandRefno.Refno = t.F_Refno
)BrandRefno
outer apply(
	select [value] = f.Finish
	from style_BOF bof
	left join Fabric f on f.SCIRefNo = bof.SCIRefno
	where bof.StyleUkey = t.StyleUkey 
	and (t.F_Refno = ''	or (t.F_Refno <> '' and bof.Refno = ''))
)Finish
outer apply(
	select [Color] = c.ID,[ColorName] = c.Name
	from Style_ColorCombo scc
	left join Style s on s.Ukey = scc.StyleUkey
	left join Color c on c.ID = scc.ColorID and s.BrandID = c.BrandId
	where scc.StyleUkey = t.StyleUkey
	and scc.Article = t.Article
	and scc.FabricPanelCode = t.A_FabricPanelCode
)AccColor
outer apply(
	select [value] = fs.SuppRefno
	from Style_BOA boa
	left join Fabric_Supp fs on fs.SCIRefno = boa.SCIRefno and ISNULL(boa.SuppIDBulk, boa.SuppIDSample) = fs.SuppID
	left join Fabric f on f.SCIRefno = boa.SCIRefno
	where boa.StyleUkey = t.StyleUkey and f.Mtltypeid like 'Heat Trans%' and boa.Refno = t.A_Refno
)AccSuppRef
where t.StyleUkey = '{this.StyleUkey}'
";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;

            this.Helper.Controls.Grid.Generator(this.grid1)
           .Text("ArtworkTypeID", header: "Artwork Type", width: Widths.AnsiChars(15), iseditingreadonly: true)
           .Text("ArtworkID", header: "Artwork ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
           .Text("Article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true)
           .Text("F_FabricPanelCode", header: "Fab/Acc\r\nFabric Panel Code", width: Widths.AnsiChars(8), iseditingreadonly: true)
           .Text("FabColor", header: "Fab/Acc\r\nColor", width: Widths.AnsiChars(8), iseditingreadonly: true)
           .Text("FabColorName", header: "Fab/Acc\r\nColor Name", width: Widths.AnsiChars(15), iseditingreadonly: true)
           .Text("F_Refno", header: "Fab/Acc\r\nRefno", width: Widths.AnsiChars(20), iseditingreadonly: true)
           .Text("Fab/AccBrandRefno", header: "Fab/Acc\r\nBrand Refno", width: Widths.AnsiChars(20), iseditingreadonly: true)
           .Text("Finish", header: "Finish", width: Widths.AnsiChars(7), iseditingreadonly: true)
           .CheckBox("IsA_FabricPanelCodeCanEmpty", header: "Accessory\r\nFabric Panel Code can empty", width: Widths.AnsiChars(13), iseditable: true, trueValue: 1, falseValue: 0)
           .Text("A_FabricPanelCode", header: "Accessory\r\nFabric Panel Code", width: Widths.AnsiChars(7), iseditingreadonly: true)
           .Text("AccColor", header: "Accessory\r\nColor", width: Widths.AnsiChars(8), iseditingreadonly: true)
           .Text("AccColorName", header: "Accessory\r\nColor Name", width: Widths.AnsiChars(15), iseditingreadonly: true)
           .Text("A_Refno", header: "Accessory \r\nRefno", width: Widths.AnsiChars(14), iseditingreadonly: true)
           .Text("AccessorySupplierRefno", header: "Accessory\r\nSupplier Refno", width: Widths.AnsiChars(20), iseditingreadonly: true)
           .Text("FabricFaceSide", header: "Fabric Face Side", width: Widths.AnsiChars(10), iseditingreadonly: true)
           .Text("PrintType", header: "Print Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
           .Text("TestNo", header: "TestNo", width: Widths.AnsiChars(19), iseditingreadonly: true)
           .Text("TestResult", header: "Test Result", width: Widths.AnsiChars(8), iseditingreadonly: true)
           .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
           .Date("SCIDel", header: "SCI Del.", width: Widths.AnsiChars(13), iseditingreadonly: true)
           .Text("FTY", header: "FTY", width: Widths.AnsiChars(5), iseditingreadonly: true)
           .Date("SubstrateFormSendDate", header: "Substrate From Send Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
           .Text("Remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true)
           .Text("CreateBy", header: "Create by", width: Widths.AnsiChars(35), iseditingreadonly: true)
           .Text("EditBy", header: "Edit by", width: Widths.AnsiChars(35), iseditingreadonly: true)
           ;
        }
    }
}
