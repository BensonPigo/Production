using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Transactions;
using Sci.Production.PublicPrg;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B42_BatchCreate
    /// </summary>
    public partial class B42_BatchCreate : Win.Subs.Base
    {
        private DataTable AllDetailData;
        private DataTable MidDetailData;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_CustomSP;
        private DataGridViewGeneratorTextColumnSettings vncontract = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings currentcustom = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings consumption = new DataGridViewGeneratorTextColumnSettings();

        /// <summary>
        /// B42_BatchCreate
        /// </summary>
        public B42_BatchCreate()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(this.comboCategory, 1, 1, "Bulk,Sample");
            this.comboCategory.SelectedIndex = -1;
            DataTable gridData;
            DBProxy.Current.Select(null, "select 0 as Selected,'' as CurrentCustomSP,'' as Article,'' as SizeCode,'' as Consumption,* from VNConsumption WITH (NOLOCK) where 1 = 0", out gridData);

            #region Current Custom的DbClick
            this.currentcustom.EditingMouseDoubleClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (e.RowIndex != -1)
                    {
                        DataRow dr = this.gridBatchCreate.GetDataRow<DataRow>(e.RowIndex);
                        string selected = MyUtility.Convert.GetString(dr["Selected"]) == "1" ? "0" : "1";
                        foreach (DataRow all in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
                        {
                            if (MyUtility.Convert.GetString(all["CurrentCustomSP"]) == MyUtility.Convert.GetString(dr["CurrentCustomSP"]))
                            {
                                all["Selected"] = selected;
                            }
                        }
                    }
                }
            };
            #endregion

            #region Contract no 按右鍵與validating
            this.vncontract.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (e.RowIndex != -1)
                    {
                        DataRow dr = this.gridBatchCreate.GetDataRow<DataRow>(e.RowIndex);
                        Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,StartDate,EndDate from VNContract WITH (NOLOCK) where GETDATE() between StartDate and EndDate and IsSubconIn = 0 order by StartDate", "15,10,10", MyUtility.Convert.GetString(dr["VNContractID"]), headercaptions: "Contract No.,Start Date, End Date");
                        DialogResult returnResult = item.ShowDialog();
                        if (returnResult == DialogResult.Cancel)
                        {
                            return;
                        }

                        e.EditingControl.Text = item.GetSelectedString();
                    }
                }
            };

            this.vncontract.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridBatchCreate.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["VNContractID"].ToString())
                {
                    if (!MyUtility.Check.Seek(string.Format("select ID from VNContract WITH (NOLOCK) where ID = '{0}' and IsSubconIn = 0", e.FormattedValue.ToString())))
                    {
                        dr["VNContractID"] = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Contract no. not found!!");
                        return;
                    }
                }
            };
            #endregion

            #region Consumption的DbClick
            this.consumption.EditingMouseDoubleClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (e.RowIndex != -1)
                    {
                        DataRow dr = this.gridBatchCreate.GetDataRow<DataRow>(e.RowIndex);
                        B42_BatchCreate_Consumption callNextForm = new B42_BatchCreate_Consumption(this.MidDetailData, this.AllDetailData, MyUtility.Convert.GetString(dr["StyleUKey"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Article"]).Substring(0, MyUtility.Convert.GetString(dr["Article"]).IndexOf(',')), MyUtility.Convert.GetString(dr["VNContractID"]));
                        DialogResult result = callNextForm.ShowDialog(this);
                        callNextForm.Dispose();
                    }
                }
            };
            #endregion

            this.gridBatchCreate.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridBatchCreate)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("CustomSP", header: "Custom SP#", width: Widths.AnsiChars(8)).Get(out this.col_CustomSP)
                .Text("CurrentCustomSP", header: "Current Custom", width: Widths.AnsiChars(8), settings: this.currentcustom, iseditingreadonly: true)
                .Text("VNContractID", header: "Contract no", width: Widths.AnsiChars(15), settings: this.vncontract)
                .Date("CDate", header: "Date", width: Widths.AnsiChars(10))
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Category", header: "Category", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Text("Article", header: "Color way", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6))
                .Text("Consumption", header: "Consumption", width: Widths.AnsiChars(40), settings: this.consumption, iseditingreadonly: true);

            this.col_CustomSP.MaxLength = 8;
            this.listControlBindingSource1.DataSource = gridData;
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                this.dateBuyerDelivery.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Buyer Delivery can't empty!");
                return;
            }

            DataTable groupData, gridData;
            DataTable[] gandM;
            StringBuilder sqlCmd = new StringBuilder();
            string contractID = MyUtility.GetValue.Lookup(@"
select ID 
from VNContract WITH (NOLOCK) 
where StartDate = 
    (select MAX(StartDate) from VNContract WITH (NOLOCK) where GETDATE() between StartDate and EndDate and Status = 'Confirmed') 
and IsSubconIn = 0
");
            #region 取得VNConsumption_Detail_Detail資料
            Prgs.ParGetVNConsumption_Detail_Detail parData = new Prgs.ParGetVNConsumption_Detail_Detail();
            parData.DateBuyerDeliveryFrom = this.dateBuyerDelivery.Value1;
            parData.DateBuyerDeliveryTo = this.dateBuyerDelivery.Value2;
            parData.Style = this.txtstyle.Text;
            parData.Category = this.comboCategory.Text;
            parData.BrandID = this.txtbrand.Text;
            parData.ContractID = string.Empty;
            DualResult result = Prgs.GetVNConsumption_Detail_Detail(parData, out this.AllDetailData);
            if (!result)
            {
                MyUtility.Msg.WarningBox(string.Format("Query detail data fail.\r\n{0}", result.ToString()));
                return;
            }
            #endregion
            #region 整理出依Style,Season Group的資料
            string strSQL = @"
alter table #tmp alter column StyleID		varchar(100)
alter table #tmp alter column SeasonID		varchar(100)
alter table #tmp alter column OrderBrandID	varchar(100)
alter table #tmp alter column Category		varchar(100)
alter table #tmp alter column SizeCode		varchar(100)
alter table #tmp alter column Article		varchar(100)
alter table #tmp alter column GMTQty		varchar(100)
alter table #tmp alter column SCIRefno		varchar(100)
alter table #tmp alter column Refno			varchar(100)
alter table #tmp alter column BrandID		varchar(100)
alter table #tmp alter column NLCode		varchar(100)
alter table #tmp alter column HSCode		varchar(100)
alter table #tmp alter column CustomsUnit	varchar(100)
alter table #tmp alter column LocalItem		varchar(100)
alter table #tmp alter column StyleCPU		varchar(100)
alter table #tmp alter column StyleUKey		varchar(100)

select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , Article
        , SizeCode
        , NLCode
        , SUM(Qty) as Qty
        , CustomsUnit 
into #tmpSumConsData
from #tmp
group by StyleID, SeasonID, OrderBrandID, Category, Article, SizeCode, NLCode, CustomsUnit;

select  distinct StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , Article
        , SizeCode
        , GMTQty
        , StyleCPU
        , StyleUkey 
into #tmpGroupStyle
from #tmp

DECLARE cursor_tmpBasic CURSOR FOR
SELECT  Distinct StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , StyleCPU
        , StyleUkey 
FROM #tmpGroupStyle

DECLARE @tempCombColor TABLE (
   StyleID VARCHAR(15),
   SeasonID VARCHAR(10),
   BrandID VARCHAR(8),
   Category VARCHAR(1),
   Article VARCHAR(max),
   SizeCode VARCHAR(8),
   GMTQty INT,
   StyleCPU NUMERIC(5,3),
   CustomSP VARCHAR(8),
   VNContractID VARCHAR(15),
   StyleUkey BIGINT
)

DECLARE @style varchar(15),
		@season varchar(10),
		@brand varchar(8),
		@category varchar(1),
		@size varchar(8),
		@cpu numeric(5,3),
		@article varchar(15),
		@gmtqty int,
		@firstrecord bit,
		@recordno int,
		@newdata bit,
		@consist bit,
		@allmatch bit,
		@comboarticle varchar(15),
		@nlcode varchar(9),
		@usedqty numeric(14,3),
		@customsunit varchar(8),
		@contract varchar(15),
		@styleukey bigint

select @contract = ID 
from VNContract WITH (NOLOCK) 
where StartDate = (select MAX(StartDate) 
                   from VNContract WITH (NOLOCK) 
                   where GETDATE() between StartDate and EndDate 
                         and Status = 'Confirmed')
and IsSubconIn = 0
order by AddDate desc 

OPEN cursor_tmpBasic
FETCH NEXT FROM cursor_tmpBasic INTO @style, @season, @brand, @category,@size,@cpu,@styleukey
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @firstrecord = 1
	SET @recordno = 0


	DECLARE cursor_tmpArticle CURSOR FOR
	select Article
           , GMTQty 
    from #tmpGroupStyle 
    where StyleID = @style 
          and SeasonID = @season 
          and OrderBrandID = @brand 
          and Category = @category 
          and SizeCode = @size;

	OPEN cursor_tmpArticle
	FETCH NEXT FROM cursor_tmpArticle INTO @article,@gmtqty
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @newdata = 0
		IF @firstrecord = 1
			BEGIN
				SET @newdata = 1
			END
		ELSE
			BEGIN
				--檢查NL Code與用量
				SET @consist = 0
				DECLARE cursor_tmpCombColor CURSOR FOR
				select Article 
                from @tempCombColor 
                where StyleID = @style 
                      and SeasonID = @season 
                      and BrandID = @brand 
                      and Category = @category 
                      and SizeCode = @size

				OPEN cursor_tmpCombColor
				FETCH NEXT FROM cursor_tmpCombColor INTO @comboarticle
				WHILE @@FETCH_STATUS = 0
				BEGIN
					SET @allmatch = 0
					SET @comboarticle = SUBSTRING(@comboarticle,1,PATINDEX('%,%',@comboarticle))

					DECLARE cursor_tmpSumConsData CURSOR FOR
					select NLCode
                           , Qty
                           , CustomsUnit 
                    from #tmpSumConsData 
                    where StyleID = @style 
                          and SeasonID = @season 
                          and OrderBrandID = @brand 
                          and Category = @category 
                          and SizeCode = @size 
                          and Article = @comboarticle

					OPEN cursor_tmpSumConsData
					FETCH NEXT FROM cursor_tmpSumConsData INTO @nlcode,@usedqty,@customsunit
					WHILE @@FETCH_STATUS = 0
					BEGIN
						
						select @allmatch = IIF(@usedqty = isnull(Qty,0),1,0) 
                        from #tmpSumConsData 
                        where StyleID = @style 
                              and SeasonID = @season 
                              and OrderBrandID = @brand 
                              and Category = @category 
                              and SizeCode = @size 
                              and Article = @comboarticle 
                              and NLCode = @nlcode 
                              and CustomsUnit = @customsunit
						
						IF @allmatch = 0
							Break;

						FETCH NEXT FROM cursor_tmpSumConsData INTO @nlcode,@usedqty,@customsunit
					END
					CLOSE cursor_tmpSumConsData
					DEALLOCATE cursor_tmpSumConsData

					IF @allmatch = 0
						BEGIN
							DECLARE cursor_tmpSumConsData CURSOR FOR
							select NLCode
                                   , Qty
                                   , CustomsUnit 
                            from #tmpSumConsData 
                            where StyleID = @style 
                                  and SeasonID = @season 
                                  and OrderBrandID = @brand 
                                  and Category = @category 
                                  and SizeCode = @size 
                                  and Article = @article

							OPEN cursor_tmpSumConsData
							FETCH NEXT FROM cursor_tmpSumConsData INTO @nlcode,@usedqty,@customsunit
							WHILE @@FETCH_STATUS = 0
							BEGIN
						
								select @allmatch = IIF(@usedqty = isnull(Qty,0),1,0) 
                                from #tmpSumConsData 
                                where StyleID = @style 
                                      and SeasonID = @season 
                                      and OrderBrandID = @brand 
                                      and Category = @category 
                                      and SizeCode = @size 
                                      and Article = @article 
                                      and NLCode = @nlcode 
                                      and CustomsUnit = @customsunit
						
								IF @allmatch = 0
									Break;

								FETCH NEXT FROM cursor_tmpSumConsData INTO @nlcode,@usedqty,@customsunit
							END
							CLOSE cursor_tmpSumConsData
							DEALLOCATE cursor_tmpSumConsData

							IF @allmatch = 0
								SET @consist = 1
								break;
						END

					IF @consist = 0
						SET @newdata = 1

					FETCH NEXT FROM cursor_tmpCombColor INTO @comboarticle
				END
				CLOSE cursor_tmpCombColor
				DEALLOCATE cursor_tmpCombColor

			END
		IF @newdata = 1
			BEGIN
				insert into @tempCombColor (
                    StyleID             , SeasonID  , BrandID       , Category      , Article
                    , SizeCode          ,GMTQty     , StyleCPU      , VNContractID  , CustomSP  
                    , StyleUkey
                ) values (
                    @style              , @season   , @brand        , @category , @article + ','
                    , @size             , @gmtqty   , @cpu          , @contract , isnull((select top 1 v.CustomSP 
                                                                                          from VNConsumption v, VNConsumption_Article va, VNConsumption_SizeCode vs 
                                                                                          where v.VNContractID = @contract 
                                                                                                and v.StyleID = @style 
                                                                                                and v.SeasonID = @season 
                                                                                                and v.BrandID = @brand 
                                                                                                and v.ID = va.ID 
                                                                                                and v.ID = vs.ID 
                                                                                                and va.Article = @article 
                                                                                                and vs.SizeCode = @size),'')
                    , @styleukey)
			END
		ELSE
			BEGIN
				update @tempCombColor 
                set GMTQty = GMTQty + @gmtqty
                    , Article = Article + @article + ',' 
				where StyleID = @style 
                      and SeasonID = @season 
                      and BrandID = @brand 
                      and Category = @category 
                      and SizeCode = @size
			END
		SET @firstrecord = 0
		FETCH NEXT FROM cursor_tmpArticle INTO @article,@gmtqty
	END
	CLOSE cursor_tmpArticle
	DEALLOCATE cursor_tmpArticle


	FETCH NEXT FROM cursor_tmpBasic INTO @style, @season, @brand, @category,@size,@cpu,@styleukey
END
CLOSE cursor_tmpBasic
DEALLOCATE cursor_tmpBasic

select * 
from @tempCombColor 
order by StyleID, SeasonID, Category, Article, SizeCode

select distinct id = ''
       , t.NLCode
       ,t.HSCode
       , UnitID = t.CustomsUnit
       , Qty = sum(t.Qty) over(partition by t.StyleUkey,t.SizeCode,t.Article,t.NLCode,t.Category)
       , UserCreate = 0
       , x.StyleUkey
       , x.SizeCode
       , t.Article       
       , t.Category
       , Deleted = 0
       , x.StyleID,x.BrandID,x.SeasonID,x.VNContractID
into #tmpdis 
from #tmp t
inner join @tempCombColor x on t.StyleUKey = x.StyleUkey and t.SizeCode = x.SizeCode and t.Category = x.Category and t.Article = SUBSTRING(x.Article,0, CHARINDEX(',',x.Article)) 

-- 取得計算waste的keyword
select distinct StyleID,Brandid,SeasonID,VNContractID
into #tmpWasteKey
from @tempCombColor 

-- 取得NLCode Waste最終計算唯一結果
select b.NLCode,Waste = (select [dbo].[getWaste]( StyleID,BrandID,SeasonID,VNContractID, b.NLCode))
into #tmpWaste
from #tmpWasteKey a ,VNNLCodeDesc b
inner join (select distinct NLCode from #tmp) c on b.NLCode=c.NLCode


-- 如遇相同NLCODE,SizeCode,StyleUkey,Article,Category  只取第一筆 
;with a as
(
	select id ,a.NLCode,HSCode, UnitID,Qty ,UserCreate,StyleUkey, SizeCode,Article,Category,Deleted
	, b.waste , rnd = ROW_NUMBER() over (partition by a.NLCODE,SizeCode,StyleUkey,Article,Category order by a.NLCODE,SizeCode,StyleUkey,Article,Category)
	from #tmpdis a
	inner join #tmpWaste b on a.NLCode=b.NLCode
)
select id ,NLCode,HSCode, UnitID,Qty ,UserCreate,StyleUkey, SizeCode,Article,Category,Deleted
	, waste
from a where rnd=1

";
            result = MyUtility.Tool.ProcessWithDatatable(
                this.AllDetailData,
                string.Empty,
                strSQL,
                out gandM);

            if (!result)
            {
                MyUtility.Msg.WarningBox(string.Format("Query detail data fail.\r\n{0}", result.ToString()));
                return;
            }
            #endregion
            groupData = gandM[0];
            this.MidDetailData = gandM[1];

            // 撈出每個Consumption一定都要有的NLCode
            DataTable necessaryItem;
            result = DBProxy.Current.Select(null,$@"
select NLCode 
from VNContract_Detail vd WITH (NOLOCK)
inner join VNContract v WITH (NOLOCK) on vd.ID=v.ID
where vd.NecessaryItem = 1 and vd.ID = '{contractID}'
and v.IsSubconIn = 0", out necessaryItem);
            if (!result)
            {
                MyUtility.Msg.WarningBox(string.Format("Query NecessaryItem data fail.\r\n{0}", result.ToString()));
                return;
            }

            gridData = (DataTable)this.listControlBindingSource1.DataSource;
            gridData.Clear();
            string colorway, lackNLCode;
            foreach (DataRow dr in groupData.Rows)
            {
                colorway = MyUtility.Convert.GetString(dr["Article"]).Substring(0, MyUtility.Convert.GetString(dr["Article"]).IndexOf(','));

                // 找出是否有空白的NL Code
                DataRow[] findData = this.AllDetailData.Select(string.Format(
                    "StyleUKey = '{0}' and Article = '{1}' and SizeCode = '{2}' and NLCode = '' and Category = '{3}'",
                    MyUtility.Convert.GetString(dr["StyleUKey"]),
                    colorway,
                    MyUtility.Convert.GetString(dr["SizeCode"]),
                    MyUtility.Convert.GetString(dr["Category"])));

                // 找出是否有缺料
                lackNLCode = string.Empty;
                foreach (DataRow lack in necessaryItem.Rows)
                {
                    DataRow[] findLackData = this.AllDetailData.Select(string.Format(
                        "StyleUKey = '{0}' and Article = '{1}' and SizeCode = '{2}' and NLCode = '{3}' and Category = '{4}'",
                        MyUtility.Convert.GetString(dr["StyleUKey"]),
                        colorway,
                        MyUtility.Convert.GetString(dr["SizeCode"]),
                        MyUtility.Convert.GetString(lack["NLCode"]),
                        MyUtility.Convert.GetString(dr["Category"])));

                    if (findLackData.Length <= 0)
                    {
                        lackNLCode = lackNLCode + MyUtility.Convert.GetString(lack["NLCode"]) + ",";
                    }
                }

                DataRow newrow = gridData.NewRow();
                newrow["Selected"] = 0;
                newrow["CustomSP"] = string.Empty;
                newrow["CurrentCustomSP"] = dr["CustomSP"];
                newrow["VNContractID"] = dr["VNContractID"];
                newrow["CDate"] = DateTime.Today;
                newrow["StyleID"] = dr["StyleID"];
                newrow["SeasonID"] = dr["SeasonID"];
                newrow["BrandID"] = dr["BrandID"];
                newrow["Category"] = dr["Category"];
                newrow["Article"] = dr["Article"];
                newrow["SizeCode"] = dr["SizeCode"];
                newrow["Qty"] = dr["GMTQty"];
                newrow["StyleUKey"] = dr["StyleUKey"];
                newrow["CPU"] = dr["StyleCPU"];
                newrow["Consumption"] = (MyUtility.Check.Empty(lackNLCode) ? string.Empty : ("Lacking Customs Code:" + lackNLCode.Substring(0, lackNLCode.Length - 1) + ". ")) + (findData.Length > 0 ? "Appear empty Customs Code." : string.Empty);

                gridData.Rows.Add(newrow);
            }

            this.listControlBindingSource1.DataSource = gridData;
        }

        // Auto Custom SP#
        private void BtnAutoCustomSPNo_Click(object sender, EventArgs e)
        {
            this.gridBatchCreate.ValidateControl();
            DataTable customSP;
            DualResult result = DBProxy.Current.Select(null, "select VNContractID,MAX(CustomSP) as CustomSP from VNConsumption WITH (NOLOCK) group by VNContractID", out customSP);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail, please try again.\r\n" + result.ToString());
                return;
            }

            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
            {
                if (MyUtility.Convert.GetString(dr["Selected"]) == "1" && !MyUtility.Check.Empty(dr["VNContractID"]))
                {
                    DataRow[] findCustom = customSP.Select(string.Format("VNContractID = '{0}'", MyUtility.Convert.GetString(dr["VNContractID"])));
                    if (findCustom.Length > 0)
                    {
                        string lastCustomsp = "SP" + (MyUtility.Convert.GetInt(MyUtility.Convert.GetString(findCustom[0]["CustomSP"]).Substring(2)) + 1).ToString("000000");
                        if (lastCustomsp.Length > 8)
                        {
                            MyUtility.Msg.InfoBox(string.Format("<CustomSP : {0}>  length can't be more than 8 Characters", lastCustomsp));
                            return;
                        }

                        findCustom[0]["CustomSP"] = lastCustomsp;
                        dr["CustomSP"] = lastCustomsp;
                    }
                    else
                    {
                        DataRow newrow = customSP.NewRow();
                        newrow["VNContractID"] = MyUtility.Convert.GetString(dr["VNContractID"]);
                        newrow["CustomSP"] = "SP000001";
                        customSP.Rows.Add(newrow);
                        dr["CustomSP"] = "SP000001";
                    }
                }
            }

            MyUtility.Msg.InfoBox("Complete!!");
        }

        // update Contract
        private void TxtVNContractID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(@"
select ID,StartDate,EndDate 
from VNContract WITH (NOLOCK) 
where GETDATE() between StartDate and EndDate 
and issubconin = 0
order by StartDate", "15,10,10", this.txtVNContractID.Text, headercaptions: "Contract No.,Start Date, End Date");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtVNContractID.Text = item.GetSelectedString();
        }

        // update Contract
        private void TxtVNContractID_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtVNContractID.OldValue != this.txtVNContractID.Text)
            {
                if (!MyUtility.Check.Seek(string.Format("select ID from VNContract WITH (NOLOCK) where ID = '{0}'", this.txtVNContractID.Text)))
                {
                    this.txtVNContractID.Text = string.Empty;
                    MyUtility.Msg.WarningBox("Contract no. not found!!");
                    return;
                }
            }
        }

        // update Contract
        private void PictureBox1_Click(object sender, EventArgs e)
        {
            this.gridBatchCreate.ValidateControl();
            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
            {
                if (MyUtility.Convert.GetString(dr["Selected"]) == "1")
                {
                    dr["VNContractID"] = this.txtVNContractID.Text;
                }
            }
        }

        // update Date
        private void PictureBox2_Click(object sender, EventArgs e)
        {
            this.gridBatchCreate.ValidateControl();
            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
            {
                if (MyUtility.Convert.GetString(dr["Selected"]) == "1")
                {
                    if (MyUtility.Check.Empty(this.dateCdate.Value))
                    {
                        dr["CDate"] = DBNull.Value;
                    }
                    else
                    {
                        dr["CDate"] = this.dateCdate.Value;
                    }
                }
            }
        }

        // Create
        private void BtnCreate_Click(object sender, EventArgs e)
        {
            this.gridBatchCreate.ValidateControl();
            #region 檢查必輸欄位
            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
            {
                if (MyUtility.Convert.GetString(dr["Selected"]) == "1" && (MyUtility.Check.Empty(dr["CustomSP"]) || MyUtility.Check.Empty(dr["VNContractID"]) || MyUtility.Check.Empty(dr["CDate"])))
                {
                    MyUtility.Msg.WarningBox("Custom SP# or Contract no. or Date can't empty!!");
                    return;
                }
            }
            #endregion

            #region 檢查合約是否合法，Custom SP#是否有重複
            DataTable errorData;
            try
            {
                string strSQL = @"
with tmpInvalidContract as (
    select t.VNContractID
           , t.CustomSP
           , Contract = isnull(v.ID,'')
           , CustomSPNo = isnull(c.CustomSP,'')
    from #tmp t
    left join VNContract v WITH (NOLOCK) on t.VNContractID = v.ID 
                                            and t.CDate between	v.StartDate and v.EndDate
    left join VNConsumption c WITH (NOLOCK) on t.CustomSP = c.CustomSP 
                                               and t.VNContractID = c.VNContractID
    where t.Selected = 1
)

select *
from tmpInvalidContract
where Contract = '' or CustomSPNo <> ''";

                MyUtility.Tool.ProcessWithDatatable(
                    (DataTable)this.listControlBindingSource1.DataSource,
                    "Selected,CustomSP,VNContractID,CDate",
                    strSQL,
                    out errorData);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Query invalid date fail!!\r\n" + ex.ToString());
                return;
            }

            if (errorData.Rows.Count > 0)
            {
                if (MyUtility.Check.Empty(errorData.Rows[0]["Contract"]))
                {
                    MyUtility.Msg.WarningBox(string.Format("Custom SP# {0}'s contract can't use.", MyUtility.Convert.GetString(errorData.Rows[0]["CustomSP"])));
                    return;
                }
                else
                {
                    MyUtility.Msg.WarningBox(string.Format("Custom SP# {0} already exist!!", MyUtility.Convert.GetString(errorData.Rows[0]["CustomSP"])));
                    return;
                }
            }
            #endregion

            #region 存檔
            IList<string> insertCmds = new List<string>();
            string newID, maxVersion, vnMultiple;
            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
            {
                TransactionScope transcation = new TransactionScope();
                using (transcation)
                {
                    if (MyUtility.Convert.GetString(dr["Selected"]) == "1" && MyUtility.Check.Empty(dr["ID"]))
                    {
                        insertCmds.Clear();
                        newID = MyUtility.GetValue.GetID(Env.User.Keyword + "SP", "VNConsumption", Convert.ToDateTime(dr["CDate"]), 2, "ID", null);
                        maxVersion = MyUtility.GetValue.Lookup(string.Format("select isnull(MAX(Version),0) as MaxVersion from VNConsumption WITH (NOLOCK) where StyleUKey = {0}", MyUtility.Convert.GetString(dr["StyleUKey"])));
                        vnMultiple = MyUtility.GetValue.Lookup("select VNMultiple from System WITH (NOLOCK) ");
                        insertCmds.Add(string.Format(
                            @"
Insert into VNConsumption (
	ID 			, CustomSP 	, VNContractID 	, CDate 		, StyleID
	, StyleUKey , SeasonID 	, BrandID 		, Category 		, SizeCode
	, Qty 		, Version 	, CPU 			, VNMultiple 	, Status
	, AddName 	, AddDate
) Values (
	'{0}' 		, '{1}' 	, '{2}' 		, '{3}' 		, '{4}'
	, {5} 		, '{6}' 	, '{7}' 		, '{8}' 		, '{9}'
	, {10} 		, '{11}' 	, {12} 			, {13} 			, 'Confirmed'
	, '{14}' 	,GETDATE()
);",
                            newID,
                            MyUtility.Convert.GetString(dr["CustomSP"]),
                            MyUtility.Convert.GetString(dr["VNContractID"]),
                            Convert.ToDateTime(dr["CDate"]).ToString("d"),
                            MyUtility.Convert.GetString(dr["StyleID"]),
                            MyUtility.Convert.GetString(dr["StyleUKey"]),
                            MyUtility.Convert.GetString(dr["SeasonID"]),
                            MyUtility.Convert.GetString(dr["BrandID"]),
                            MyUtility.Convert.GetString(dr["Category"]),
                            MyUtility.Convert.GetString(dr["SizeCode"]),
                            MyUtility.Convert.GetString(dr["Qty"]),
                            MyUtility.Convert.GetString(MyUtility.Convert.GetInt(maxVersion) + 1).PadLeft(3, '0'),
                            MyUtility.Convert.GetString(dr["CPU"]),
                            vnMultiple,
                            Env.User.UserID));

                        insertCmds.Add(string.Format(
                            @"
Insert into VNConsumption_SizeCode (
	ID 		, SizeCode
) Values (
	'{0}' 	,'{1}'
);",
                            newID,
                            MyUtility.Convert.GetString(dr["SizeCode"])));

                        string[] article = MyUtility.Convert.GetString(dr["Article"]).Split(',');
                        foreach (string str in article)
                        {
                            if (!MyUtility.Check.Empty(str))
                            {
                                insertCmds.Add(string.Format(
                                    @"
Insert into VNConsumption_Article (
	ID 		, Article
) Values (
	'{0}' 	, '{1}'
);",
                                    newID,
                                    str.ToString()));
                            }
                        }

                        DataRow[] selectedData = this.MidDetailData.Select(string.Format("StyleUKey = {0} and SizeCode = '{1}' and Article = '{2}' and Category = '{3}' and Deleted = 0", MyUtility.Convert.GetString(dr["StyleUKey"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Article"]).Substring(0, MyUtility.Convert.GetString(dr["Article"]).IndexOf(',')), dr["Category"]));

                        for (int i = 0; i < selectedData.Length; i++)
                        {
                            DataRow[] selectedDetailData = this.AllDetailData.Select(string.Format(
                                @"
StyleUKey = {0} 
and SizeCode = '{1}' 
and Article = '{2}' 
and NLCode = '{3}'  
and Category = '{4}' 
and (VNContractID = '{5}' or VNContractID ='NA')",
                                MyUtility.Convert.GetString(dr["StyleUKey"]),
                                MyUtility.Convert.GetString(dr["SizeCode"]),
                                MyUtility.Convert.GetString(dr["Article"]).Substring(0, MyUtility.Convert.GetString(dr["Article"]).IndexOf(',')),
                                MyUtility.Convert.GetString(selectedData[i]["NLCode"]),
                                MyUtility.Convert.GetString(dr["Category"]),
                                MyUtility.Convert.GetString(dr["VNContractID"])));
                            #region 檢查ID,NLCode,HSCode,UnitID Group後是否有ID,NLCode重複的資料
                            bool isVNConsumption_Detail_DetailHasDupData = !Prgs.CheckVNConsumption_Detail_Dup(selectedDetailData, false);
                            if (isVNConsumption_Detail_DetailHasDupData)
                            {
                                return;
                            }
                            #endregion
                            for (int j = 0; j < selectedDetailData.Length; j++)
                            {
                                if (!MyUtility.Check.Empty(selectedDetailData[j]["RefNo"]))
                                {
                                    insertCmds.Add(string.Format(
                                        @"
Insert into VNConsumption_Detail_Detail (
	ID 			, NLCode 	, SCIRefno 	, RefNo 	, Qty, SystemQty
	, LocalItem , UserCreate, StockQty  , StockUnit ,HSCode, UnitID, FabricBrandID, FabricType, UsageQty, UsageUnit  
) Values (
	'{0}' 		, '{1}' 	, '{2}' 	, '{3}' 	, {4}, {4}
	,{5}        , 0, {6}  , '{7}', '{8}', '{9}', '{10}', '{11}',{12},'{13}'
);",
                                        newID,
                                        MyUtility.Convert.GetString(selectedData[i]["NLCode"]),
                                        MyUtility.Convert.GetString(selectedDetailData[j]["SCIRefno"].ToString().Replace("'", "''")),
                                        MyUtility.Convert.GetString(selectedDetailData[j]["RefNo"].ToString().Replace("'", "''")),
                                        MyUtility.Convert.GetString(selectedDetailData[j]["Qty"]),
                                        MyUtility.Convert.GetString(selectedDetailData[j]["LocalItem"]),
                                        MyUtility.Convert.GetString(selectedDetailData[j]["StockQty"]),
                                        MyUtility.Convert.GetString(selectedDetailData[j]["StockUnit"]),
                                        MyUtility.Convert.GetString(selectedDetailData[j]["HSCode"]),
                                        MyUtility.Convert.GetString(selectedDetailData[j]["CustomsUnit"]),
                                        MyUtility.Convert.GetString(selectedDetailData[j]["BrandID"]),
                                        MyUtility.Convert.GetString(selectedDetailData[j]["FabricType"]),
                                        MyUtility.Convert.GetString(selectedDetailData[j]["UsageQty"]),
                                        MyUtility.Convert.GetString(selectedDetailData[j]["UsageUnit"])));
                                }
                            }
                        }

                        // 產生VNConsumption_Detail的資料 呼叫CreateVNConsumption_Detail
                        insertCmds.Add($"exec CreateVNConsumption_Detail '{newID}'");

                        DualResult result = DBProxy.Current.Executes(null, insertCmds);
                        if (!result)
                        {
                            transcation.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                        else
                        {
                            dr["ID"] = newID;
                        }

                        transcation.Complete();
                        transcation.Dispose();
                    }
                }
            }
            #endregion

            MyUtility.Msg.InfoBox("Complete!!");
        }

        // Empty NL Code (to Excel)
        private void BtnEmptyNLCodetoExcel_Click(object sender, EventArgs e)
        {
            DataTable toExcelData;
            try
            {
                MyUtility.Tool.ProcessWithDatatable(
                    this.AllDetailData,
                    "NLCode,RefNo,BrandID",
                    @"select distinct RefNo,BrandID from #tmp where NLCode = ''",
                    out toExcelData);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Query empty Customs Code data fail!!\r\n" + ex.ToString());
                return;
            }

            if (toExcelData.Rows.Count <= 0)
            {
                MyUtility.Msg.InfoBox("No data!!");
                return;
            }

            bool result = MyUtility.Excel.CopyToXls(toExcelData, string.Empty);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString(), "Warning");
            }
        }
    }
}
