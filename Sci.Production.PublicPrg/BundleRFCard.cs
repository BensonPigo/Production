using Ict;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;
using static Sci.Production.PublicPrg.Prgs;

namespace Sci.Production.Prg
{
    /// <inheritdoc/>
    public class BundleRFCard
    {
        /// <summary>
        /// Bundle Test Type
        /// </summary>
        public enum BundleType : int
        {
            /// <summary>
            /// 0. Open Port
            /// </summary>
            Open = 0,

            /// <summary>
            /// 1. card from Stacker
            /// </summary>
            C31 = 1,

            /// <summary>
            /// 2. Get UID
            /// </summary>
            F31 = 2,

            /// <summary>
            /// 3. Set Print Data
            /// </summary>
            P48 = 3,

            /// <summary>
            /// 4. Print
            /// </summary>
            P41 = 4,

            /// <summary>
            /// 5. Set And Print  3+4
            /// </summary>
            P49 = 5,

            /// <summary>
            /// card to Bin Box
            /// </summary>
            C34 = 6,

            /// <summary>
            /// Write DB
            /// </summary>
            WDB = 7,

            /// <summary>
            /// Create IMG
            /// </summary>
            Create = 8,

            /// <summary>
            /// port close
            /// </summary>
            Close = 9,

            /// <summary>
            /// Card Erase
            /// </summary>
            P21 = 10,

            /// <summary>
            /// Setting the Text data to the Sram.(position free)
            /// </summary>
            P35 = 11,

            /// <summary>
            /// Sram Reset
            /// </summary>
            P42 = 12,
        }

        /// <summary>
        /// Bundel RF 統一撈取語法
        /// </summary>
        /// <param name="chkExtendAllParts">是否勾選Extend All Parts</param>
        /// <param name="sqlWhere">Sql Where</param>
        /// <returns>回傳語法</returns>
        public static string BundelRFSQLCmd(bool chkExtendAllParts, string sqlWhere)
        {
            string sqlCmd = string.Empty;
            if (chkExtendAllParts)
            {
                #region 有勾[Extend All Parts]
                sqlCmd = string.Format(
                    $@"
select *
from 
(
	select bd.BundleGroup
		, [BundleGroupSerial] = ROW_NUMBER() over(partition by o.ID, b.PatternPanel, b.FabricPanelCode, bd.SizeCode order by o.ID, b.PatternPanel, b.FabricPanelCode, bd.SizeCode, bd.BundleGroup)
		, [BundleGroupCount] = bd_count.cnt
		, b.Sewinglineid
		, o.FactoryID
		, o.StyleID
		, o.ID
		, [Color] = b.Article + '\' + b.Colorid
		, bd.SizeCode
		, bd.Qty
		, b.Item
		, [Sea] = concat(o.SeasonID,'-', o.dest)
		, br.BuyerID
		, [MK] = iif(isnull(b.CutRef,'') <> '',(select top 1 WorkOrder.MarkerNo from WorkOrder WITH (NOLOCK) where WorkOrder.CutRef= b.CutRef and WorkOrder.id = b.poid),'')
		, [BodyCut] = concat(isnull(b.PatternPanel,''),'-', b.FabricPanelCode ,'-',convert(varchar, b.Cutno))
		, [Artwork]= iif(len(Artwork.Artwork) > 43, substring(Artwork.Artwork ,0,43), Artwork.Artwork)
		, bd.PatternDesc
        , [BundleID] = b.ID
        , bd.BundleNo
        , bd.RFPrintDate
	from Bundle b WITH (NOLOCK)
	inner join Orders o WITH (NOLOCK) on b.Orderid = o.ID
	inner join Bundle_Detail bd WITH (NOLOCK) on b.ID = bd.Id
	left join Brand br WITH (NOLOCK) on o.BrandID = br.ID
	outer apply (
		select [cnt] = count(*)
		from Bundle b2 
		inner join Bundle_Detail bd2 WITH (NOLOCK) on b2.ID = bd2.Id
		where b.ID = b2.ID 
		and bd2.Patterncode <> 'ALLPARTS'
		group by b2.Orderid, b2.PatternPanel,b2.FabricPanelCode,bd2.SizeCode
	) bd_count
	outer apply
	(
		select Artwork= stuff((
			 Select distinct concat('+', bda.SubprocessId)
			 from Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
			 where bda.id = bd.Id 
			 and bda.PatternCode = bd.PatternCode 
			 and bda.Bundleno=bd.BundleNo
		 for xml path('')
		),1,1,'')
	)as Artwork
	where b.ID = @ID
    {sqlWhere}
	and bd.Patterncode <> 'ALLPARTS'

	union all

	select bd.BundleGroup
		, [BundleGroupSerial] = ROW_NUMBER() over(partition by o.ID, b.PatternPanel, b.FabricPanelCode, bd.SizeCode order by o.ID, b.PatternPanel, b.FabricPanelCode, bd.SizeCode, bd.BundleGroup)
		, [BundleGroupCount] = bd_count.cnt
		, b.Sewinglineid
		, o.FactoryID
		, o.StyleID
		, o.ID
		, [Color] = b.Article + '\' + b.Colorid
		, bd.SizeCode
		, bd.Qty
		, b.Item
		, [Sea] = concat(o.SeasonID,'-', o.dest)
		, br.BuyerID
		, [MK] = iif(isnull(b.CutRef,'') <> '',(select top 1 WorkOrder.MarkerNo from WorkOrder WITH (NOLOCK) where WorkOrder.CutRef= b.CutRef and WorkOrder.id = b.poid),'')
		, [BodyCut] = concat(isnull(b.PatternPanel,''),'-', b.FabricPanelCode ,'-',convert(varchar, b.Cutno))
		, [Artwork]= ''
		, bda.PatternDesc
        , [BundleID] = b.ID
        , bd.BundleNo
        , bd.RFPrintDate
	from Bundle b WITH (NOLOCK)
	inner join Orders o WITH (NOLOCK) on b.Orderid = o.ID
	inner join Bundle_Detail bd WITH (NOLOCK) on b.ID = bd.Id
	left join Brand br WITH (NOLOCK) on o.BrandID = br.ID
	outer apply (
		select [cnt] = count(*)
		from Bundle b2 
		inner join Bundle_Detail bd2 WITH (NOLOCK) on b2.ID = bd2.Id
		where b.ID = b2.ID 
		and bd2.Patterncode = 'ALLPARTS'
		group by b2.Orderid, b2.PatternPanel,b2.FabricPanelCode,bd2.SizeCode
	) bd_count
	outer apply
	(
		select distinct bda.PatternCode, bda.PatternDesc, bda.Parts
		from Bundle_Detail_Allpart bda with(nolock)
		where bda.id = b.id

	) bda
	where b.ID = @ID
    {sqlWhere}
	and bd.Patterncode = 'ALLPARTS'
)b
");
                #endregion
            }
            else
            {
                #region 沒勾[Extend All Parts]
                sqlCmd = string.Format(
                    $@"
select *
from 
(
	select bd.BundleGroup
		, [BundleGroupSerial] = ROW_NUMBER() over(partition by o.ID, b.PatternPanel, b.FabricPanelCode, bd.SizeCode order by o.ID, b.PatternPanel, b.FabricPanelCode, bd.SizeCode, bd.BundleGroup)
		, [BundleGroupCount] = bd_count.cnt
		, b.Sewinglineid
		, o.FactoryID
		, o.StyleID
		, o.ID
		, [Color] = b.Article + '\' + b.Colorid
		, bd.SizeCode
		, bd.Qty
		, b.Item
		, [Sea] = concat(o.SeasonID,'-', o.dest)
		, br.BuyerID
		, [MK] = iif(isnull(b.CutRef,'') <> '',(select top 1 WorkOrder.MarkerNo from WorkOrder WITH (NOLOCK) where WorkOrder.CutRef= b.CutRef and WorkOrder.id = b.poid),'')
		, [BodyCut] = concat(isnull(b.PatternPanel,''),'-', b.FabricPanelCode ,'-',convert(varchar, b.Cutno))
		, [Artwork]= iif(len(Artwork.Artwork) > 43, substring(Artwork.Artwork ,0,43), Artwork.Artwork)
		, bd.PatternDesc
        , [BundleID] = b.ID
        , bd.BundleNo
        , bd.RFPrintDate
	from Bundle b WITH (NOLOCK)
	inner join Orders o WITH (NOLOCK) on b.Orderid = o.ID
	inner join Bundle_Detail bd WITH (NOLOCK) on b.ID = bd.Id
	left join Brand br WITH (NOLOCK) on o.BrandID = br.ID
	outer apply (
		select [cnt] = count(*)
		from Bundle b2 
		inner join Bundle_Detail bd2 WITH (NOLOCK) on b2.ID = bd2.Id
		where b.ID = b2.ID 
		and bd2.Patterncode <> 'ALLPARTS'
		group by b2.Orderid, b2.PatternPanel,b2.FabricPanelCode,bd2.SizeCode
	) bd_count
	outer apply
	(
		select Artwork= stuff((
			 Select distinct concat('+', bda.SubprocessId)
			 from Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
			 where bda.id = bd.Id 
			 and bda.PatternCode = bd.PatternCode 
			 and bda.Bundleno=bd.BundleNo
		 for xml path('')
		),1,1,'')
	)as Artwork
	where b.ID = @ID 
    {sqlWhere}
	and bd.Patterncode <> 'ALLPARTS'

	union all

	select bd.BundleGroup
		, [BundleGroupSerial] = ROW_NUMBER() over(partition by o.ID, b.PatternPanel, b.FabricPanelCode, bd.SizeCode order by o.ID, b.PatternPanel, b.FabricPanelCode, bd.SizeCode, bd.BundleGroup)
		, [BundleGroupCount] = bd_count.cnt
		, b.Sewinglineid
		, o.FactoryID
		, o.StyleID
		, o.ID
		, [Color] = b.Article + '\' + b.Colorid
		, bd.SizeCode
		, bd.Qty
		, b.Item
		, [Sea] = concat(o.SeasonID,'-', o.dest)
		, br.BuyerID
		, [MK] = iif(isnull(b.CutRef,'') <> '',(select top 1 WorkOrder.MarkerNo from WorkOrder WITH (NOLOCK) where WorkOrder.CutRef= b.CutRef and WorkOrder.id = b.poid),'')
		, [BodyCut] = concat(isnull(b.PatternPanel,''),'-', b.FabricPanelCode ,'-',convert(varchar, b.Cutno))
		, [Artwork]= iif(len(Artwork.Artwork) > 43, substring(Artwork.Artwork ,0,43), Artwork.Artwork)
		, bd.PatternDesc
        , [BundleID] = b.ID
        , bd.BundleNo
        , bd.RFPrintDate
	from Bundle b WITH (NOLOCK)
	inner join Orders o WITH (NOLOCK) on b.Orderid = o.ID
	inner join Bundle_Detail bd WITH (NOLOCK) on b.ID = bd.Id
	left join Brand br WITH (NOLOCK) on o.BrandID = br.ID
	outer apply (
		select [cnt] = count(*)
		from Bundle b2 
		inner join Bundle_Detail bd2 WITH (NOLOCK) on b2.ID = bd2.Id
		where b.ID = b2.ID 
		and bd2.Patterncode = 'ALLPARTS'
		group by b2.Orderid, b2.PatternPanel,b2.FabricPanelCode,bd2.SizeCode
	) bd_count
	outer apply
	(
		select Artwork= stuff((
			 Select distinct concat('+', bda.SubprocessId)
			 from Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
			 where bda.id = bd.Id 
			 and bda.PatternCode = bd.PatternCode 
			 and bda.Bundleno=bd.BundleNo
		 for xml path('')
		),1,1,'')
	)as Artwork
	where b.ID = @ID
    {sqlWhere}
	and bd.Patterncode = 'ALLPARTS'
)b
");
                #endregion
            }

            return sqlCmd;
        }

        /// <summary>
        /// Auto Bundle RFCard Print
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="isEraser">is Eraser</param>
        /// <returns>DualResult</returns>
        public static DualResult BundleRFCardPrint(DataTable dt, bool isEraser = false)
        {
            DualResult result = new DualResult(false);
            BundleRFCardUSB bundleRFCard = new BundleRFCardUSB();
            bool initEraseSet = false;
            try
            {
                if (bundleRFCard.UsbPortOpen())
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        // C31
                        result = CardFromStacker(bundleRFCard);
                        if (!result)
                        {
                            if (result.Messages.EqualString("0x2105"))
                            {
                                result = new DualResult(false, "No card in the stacker, this machine cannot get the card.");
                            }
                            else if (result.Messages.EqualString("0x2006"))
                            {
                                result = new DualResult(false, "Cannot make 2nd commit, Because there is a card in the machine.");
                            }

                            throw new Exception(string.Join("Card Get From Stacker Error :", Environment.NewLine, result.ToString()));
                        }

                        // F31
                        string cardUID = string.Empty;
                        result = CardRFUiD(bundleRFCard);
                        if (result)
                        {
                            cardUID = result.Description;
                        }
                        else
                        {
                            throw new Exception(string.Join("Card Get RF UID Error :", Environment.NewLine, result.ToString()));
                        }

                        if (isEraser)
                        {
                            if (!initEraseSet)
                            {
                                // P22 sets the partial Erase Area.
                                result = CardEraseSettingArea(bundleRFCard);
                                if (!result)
                                {
                                    throw new Exception(string.Join("Card set Erase Error :", Environment.NewLine, result.ToString()));
                                }

                                initEraseSet = true;
                            }

                            // P24 Card Erase the partial Area.
                            result = CardErasePartialArea(bundleRFCard);
                            if (!result)
                            {
                                throw new Exception(string.Join("Card Erase Error :", Environment.NewLine, result.ToString()));
                            }
                        }

                        // P42 Sram Reset
                        result = CardSramReset(bundleRFCard);
                        if (!result)
                        {
                            throw new Exception(string.Join("Card Sram Reset Error :", Environment.NewLine, result.ToString()));
                        }

                        // P35
                        List<string> settings = new List<string>();
                        result = GetSettingText(dr, out settings);
                        if (!result)
                        {
                            throw new Exception(string.Join("Get SettingText Error :", Environment.NewLine, result.ToString()));
                        }

                        result = CardSettingTextTOSram(bundleRFCard, settings);
                        if (!result)
                        {
                            throw new Exception(string.Join("Card SettingText TO Sram Error :", Environment.NewLine, result.ToString()));
                        }

                        // P41
                        result = CardPrint(bundleRFCard);
                        if (!result)
                        {
                            throw new Exception(string.Join("Card Print Error :", Environment.NewLine, result.ToString()));
                        }

                        // write DB
                        result = UpdateBundleDetailRFUID(dr["BundleID"].ToString(), dr["BundleNo"].ToString(), cardUID);
                        if (!result)
                        {
                            throw new Exception(string.Join("Write To DB Error :", Environment.NewLine, result.ToString()));
                        }

                        // C36
                        result = CardDrop(bundleRFCard);
                        if (!result)
                        {
                            throw new Exception(string.Join("Card Capture Error :", Environment.NewLine, result.ToString()));
                        }
                    }
                }
                else
                {
                    throw new Exception("Printer(CHP_1800) usb port not open");
                }

                result = new DualResult(true);
            }
            catch (Exception ex)
            {
                result = new DualResult(false, ex);
            }
            finally
            {
                bundleRFCard.UsbPortClose();
            }

            return result;
        }

        /// <summary>
        /// Auto Bundle RFCard Print With Output Error Card
        /// </summary>
        /// <param name="data">List.P10_PrintData</param>
        /// <param name="nowIndex">now index</param>
        /// <param name="returnIndex">return Index</param>
        /// <param name="isEraser">is Eraser</param>
        /// <returns>DualResult</returns>
        public static DualResult BundleRFCardPrint(List<P10_PrintData> data, int nowIndex, out int returnIndex, bool isEraser = false)
        {
            DualResult result = new DualResult(false);
            BundleRFCardUSB bundleRFCard = new BundleRFCardUSB();
            bool initEraseSet = true;
            returnIndex = nowIndex;
            try
            {
                if (bundleRFCard.UsbPortOpen())
                {
                    while (nowIndex <= data.Count - 1)
                    {
                        returnIndex = nowIndex;

                        // C31
                        result = CardFromStacker(bundleRFCard);
                        if (!result)
                        {
                            if (result.Messages.EqualString("0x2005") || result.Messages.EqualString("0x2105") || result.Messages.EqualString("0x2305"))
                            {
                                throw new Exception("No card in the stacker, printer cannot get the card.Please top up the card to continue printing.");
                            }
                            else if (result.Messages.EqualString("0x2006"))
                            {
                                // C36
                                result = CardDrop(bundleRFCard);
                                if (!result)
                                {
                                    throw new Exception("Card Capture Error " + BFPrintErrorMSG(result.Messages.ToString()));
                                }

                                continue;
                            }
                            else
                            {
                                throw new Exception("Card Capture Error " + BFPrintErrorMSG(result.Messages.ToString()));
                            }
                        }

                        // F31
                        string cardUID = string.Empty;
                        result = CardRFUiD(bundleRFCard);
                        if (result)
                        {
                            cardUID = result.Description;
                        }
                        else
                        {
                            result = BundleRFCardPrintErrorMsg(bundleRFCard, "Card Get RF UID Error " + BFPrintErrorMSG(result.Messages.ToString()));
                            throw new Exception(result.Messages.ToString());
                        }

                        if (isEraser)
                        {
                            if (initEraseSet)
                            {
                                // P22 sets the partial Erase Area.
                                result = CardEraseSettingArea(bundleRFCard);
                                if (!result)
                                {
                                    result = BundleRFCardPrintErrorMsg(bundleRFCard, "Card set Erase Error " + BFPrintErrorMSG(result.Messages.ToString()));
                                    throw new Exception(result.Messages.ToString());
                                }

                                initEraseSet = false;
                            }

                            // P24 Card Erase the partial Area.
                            result = CardErasePartialArea(bundleRFCard);
                            if (!result)
                            {
                                result = BundleRFCardPrintErrorMsg(bundleRFCard, "Card Erase Error " + BFPrintErrorMSG(result.Messages.ToString()));
                                throw new Exception(result.Messages.ToString());
                            }
                        }

                        // P42 Sram Reset
                        result = CardSramReset(bundleRFCard);
                        if (!result)
                        {
                            result = BundleRFCardPrintErrorMsg(bundleRFCard, "Card Sram Reset Error " + BFPrintErrorMSG(result.Messages.ToString()));
                            throw new Exception(result.Messages.ToString());
                        }

                        // P35
                        List<string> settings = new List<string>();
                        result = GetSettingText(data[nowIndex], out settings);
                        if (!result)
                        {
                            result = BundleRFCardPrintErrorMsg(bundleRFCard, "Get SettingText Error " + BFPrintErrorMSG(result.Messages.ToString()));
                            throw new Exception(result.Messages.ToString());
                        }

                        result = CardSettingTextTOSram(bundleRFCard, settings);
                        if (!result)
                        {
                            result = BundleRFCardPrintErrorMsg(bundleRFCard, "Card SettingText TO Sram Error " + BFPrintErrorMSG(result.Messages.ToString()));
                            throw new Exception(result.Messages.ToString());
                        }

                        // P37 Barcode
                        result = CardSettingBarcodeSram(bundleRFCard, data[nowIndex].Barcode);
                        if (!result)
                        {
                            result = BundleRFCardPrintErrorMsg(bundleRFCard, "Card Setting Barcode TO Sram Error " + BFPrintErrorMSG(result.Messages.ToString()));
                            throw new Exception(result.Messages.ToString());
                        }

                        // P41
                        result = CardPrint(bundleRFCard);
                        if (!result)
                        {
                            result = BundleRFCardPrintErrorMsg(bundleRFCard, "Card Print Error " + BFPrintErrorMSG(result.Messages.ToString()));
                            throw new Exception(result.Messages.ToString());
                        }

                        // C36
                        result = CardDrop(bundleRFCard);
                        if (!result)
                        {
                            result = BundleRFCardPrintErrorMsg(bundleRFCard, "Card Capture Error " + BFPrintErrorMSG(result.Messages.ToString()));
                            throw new Exception(result.Messages.ToString());
                        }

                        // write DB
                        result = UpdateBundleDetailRFUID(data[nowIndex].BundleID.ToString(), data[nowIndex].BundleNo.ToString(), cardUID);
                        if (!result)
                        {
                            result = BundleRFCardPrintErrorMsg(bundleRFCard, "Write To DB Error " + BFPrintErrorMSG(result.Messages.ToString()));
                            throw new Exception(result.Messages.ToString());
                        }

                        nowIndex++;
                    }
                }
                else
                {
                    throw new Exception("Printer(CHP_1800) usb port not open");
                }

                result = new DualResult(true);
            }
            catch (Exception ex)
            {
                result = new DualResult(false, new BaseResult.MessageInfo(ex.Message.ToString()));
            }
            finally
            {
                bundleRFCard.UsbPortClose();
            }

            return result;
        }

        /// <summary>
        /// Clean Thermal Printer Head
        /// </summary>
        /// <returns>DualResult</returns>
        public static DualResult BundleRFCleanThermalPrinterHead()
        {
            DualResult result = new DualResult(false);
            BundleRFCardUSB bundleRFCard = new BundleRFCardUSB();
            try
            {
                if (bundleRFCard.UsbPortOpen())
                {
                    result = CardCleanThermalPrinterHead(bundleRFCard);
                    if (!result)
                    {
                        throw new Exception(string.Join("Card Clean Thermal Printer Head : ", BFPrintErrorMSG(result.Messages.ToString())));
                    }
                }
            }
            catch (Exception ex)
            {
                result = new DualResult(false, ex);
            }
            finally
            {
                bundleRFCard.UsbPortClose();
            }

            return result;
        }

        /// <summary>
        /// RF Print 列印且出錯顯示confirmbox，選擇Continue 則重複執行，直到Stop或完成
        /// </summary>
        /// <param name="data">List.P10_PrintData</param>
        /// <param name="nowIndex">DataTable now index</param>
        /// <param name="isEraser">Is Eraser</param>
        /// <returns>DualResult</returns>
        public static DualResult BundleRFCardPrintAndRetry(List<P10_PrintData> data, int nowIndex, bool isEraser = false)
        {
            DualResult result = new DualResult(false);

            if (data == null || data.Count == 0)
            {
                return result;
            }

            string confirmTitle = "RF Print Error";
            int returnIndex = 0;
            while (!(result = BundleRFCardPrint(data, nowIndex, out returnIndex, isEraser)))
            {
                nowIndex = returnIndex;
                DialogResult confirmResult;
                if (result.Messages.ToString().IndexOf(BFPrintErrorMSG("0x2620")) > -1)
                {
                    confirmResult = Prg.MessageBoxEX.Show(
                            result.Messages.ToString(),
                            confirmTitle,
                            MessageBoxButtons.YesNoCancel,
                            new string[] { "Retry", "Head Clean", "Cancel" });
                    if (confirmResult.EqualString("Yes"))
                    {
                        continue;
                    }
                    else if (confirmResult.EqualString("No"))
                    {
                        DualResult resultClean = BundleRFCleanThermalPrinterHead();
                        if (!resultClean)
                        {
                            result = new DualResult(false, resultClean.Messages.ToString());
                            continue;
                        }

                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    confirmResult = MessageBox.Show(
                                               result.Messages.ToString(),
                                               confirmTitle,
                                               MessageBoxButtons.RetryCancel);
                    if (confirmResult.EqualString("Retry"))
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Print Error RFCard
        /// </summary>
        private static DualResult BundleRFCardPrintErrorMsg(BundleRFCardUSB bundleRFCard, string errMsg)
        {
            DualResult result;

            // P42 Sram Reset
            result = CardSramReset(bundleRFCard);
            if (!result)
            {
                return new DualResult(false, new BaseResult.MessageInfo("Card Sram Reset Error " + BFPrintErrorMSG(result.Messages.ToString())));
            }

            // P35
            List<string> settings = new List<string>() { errMsg };
            result = CardSettingTextTOSram(bundleRFCard, settings);
            if (!result)
            {
                return new DualResult(false, new BaseResult.MessageInfo("Card SettingText TO Sram Error " + BFPrintErrorMSG(result.Messages.ToString())));
            }

            // P41
            result = CardPrint(bundleRFCard);
            if (!result)
            {
                return new DualResult(false, new BaseResult.MessageInfo("Card Print Error " + BFPrintErrorMSG(result.Messages.ToString())));
            }

            // C36
            result = CardDrop(bundleRFCard);
            if (!result)
            {
                return new DualResult(false, new BaseResult.MessageInfo("Card Capture Error " + BFPrintErrorMSG(result.Messages.ToString())));
            }

            result = new DualResult(false, new BaseResult.MessageInfo(errMsg));

            return result;
        }

        /// <summary>
        /// Only RF Card Erase (Stacker all Card)
        /// </summary>
        /// <returns>DualResult</returns>
        public static DualResult BundleRFErase()
        {
            DualResult result = new DualResult(false);
            BundleRFCardUSB bundleRFCard = new BundleRFCardUSB();
            bool initEraseSet = false;
            try
            {
                if (bundleRFCard.UsbPortOpen())
                {
                    while (true)
                    {
                        // C31
                        result = CardFromStacker(bundleRFCard);
                        if (!result)
                        {
                            if (result.Messages.EqualString("0x2105"))
                            {
                                result = new DualResult(true);
                                break;
                            }
                            else if (result.Messages.EqualString("0x2006"))
                            {
                                // C36
                                result = CardDrop(bundleRFCard);
                                if (!result)
                                {
                                    throw new Exception("Card Capture Error");
                                }

                                continue;
                            }
                            else
                            {
                                throw new Exception("Card Capture Error" + result.Messages.ToString());
                            }
                        }

                        // P22 sets the partial Erase Area.
                        if (!initEraseSet)
                        {
                            result = CardEraseSettingArea(bundleRFCard);
                            initEraseSet = true;
                            if (!result)
                            {
                                result = BundleRFCardPrintErrorMsg(bundleRFCard, "Card set Erase Error");
                                throw new Exception(result.Messages.ToString());
                            }
                        }

                        // P24 Card Erase the partial Area.
                        result = CardErasePartialArea(bundleRFCard);
                        if (!result)
                        {
                            result = BundleRFCardPrintErrorMsg(bundleRFCard, "Card Erase Error");
                            throw new Exception(result.Messages.ToString());
                        }

                        // C36
                        result = CardDrop(bundleRFCard);
                        if (!result)
                        {
                            result = BundleRFCardPrintErrorMsg(bundleRFCard, "Card Capture Error");
                            throw new Exception(result.Messages.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = new DualResult(false, ex);
            }
            finally
            {
                bundleRFCard.UsbPortClose();
            }

            return result;
        }

        /// <summary>
        /// Bundel Test Flow
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="type">Bundle Type</param>
        /// <returns>DualResult</returns>
        public static DualResult BundelTest(DataTable dt, BundleType type)
        {
            DualResult result = new DualResult(false);
            try
            {
                DataRow dr;
                string path;
                string fileName;

                if (dt.Rows.Count == 0)
                {
                    throw new Exception("No Data");
                }

                BundleRFCardUSB bundleRFCard = new BundleRFCardUSB();
                dr = dt.Rows[0];
                switch (type)
                {
                    case BundleType.Open:
                        bool isopen = bundleRFCard.UsbPortOpen();
                        result = new DualResult(isopen, isopen ? string.Empty : "Printer(CHP_1800) usb port not open");
                        break;
                    case BundleType.Close:
                        bool isClose = bundleRFCard.UsbPortClose();
                        result = new DualResult(isClose, isClose ? string.Empty : "Printer(CHP_1800) usb port not Close");
                        break;
                    case BundleType.C31:
                        result = CardFromStacker(bundleRFCard);
                        break;
                    case BundleType.F31:
                        result = CardRFUiD(bundleRFCard);

                        MyUtility.Msg.InfoBox("BundleID" + dr["BundleID"].ToString() + ",BundleNo" + dr["BundleNo"].ToString() + ",RFUiD" + result.Description);
                        break;
                    case BundleType.Create:
                        path = Directory.GetCurrentDirectory() + @"\data";
                        fileName = dr["BundleNo"].ToString() + ".png";
                        result = CardConvertHtmlToImage(dr, path, fileName);
                        break;
                    case BundleType.P48:
                        fileName = dr["BundleNo"].ToString() + ".png";
                        result = CardSetPrintData(bundleRFCard, fileName);
                        break;
                    case BundleType.P41:
                        result = CardPrint(bundleRFCard);
                        break;
                    case BundleType.P49:
                        path = Directory.GetCurrentDirectory() + @"\data";
                        fileName = dr["BundleNo"].ToString() + ".png";
                        result = CardSetAndPrint(bundleRFCard, path, fileName);
                        break;
                    case BundleType.C34:
                        result = CardCapture(bundleRFCard);
                        break;
                    case BundleType.WDB:
                        result = CardRFUiD(bundleRFCard);
                        if (!result)
                        {
                            throw new Exception(string.Join("Card Get RF UID Error :", Environment.NewLine, result.ToString()));
                        }

                        MyUtility.Msg.InfoBox("BundleID" + dr["BundleID"].ToString() + ",BundleNo" + dr["BundleNo"].ToString() + ",RFUiD" + result.Description);

                        result = UpdateBundleDetailRFUID(dr["BundleID"].ToString(), dr["BundleNo"].ToString(), result.Description);
                        break;
                    case BundleType.P21:
                        result = CardErase(bundleRFCard);
                        break;
                    case BundleType.P35:
                        List<string> settings = new List<string>();
                        result = GetSettingText(dr, out settings);
                        if (!result)
                        {
                            throw new Exception(string.Join("Get SettingText Error :", Environment.NewLine, result.ToString()));
                        }

                        result = CardSettingTextTOSram(bundleRFCard, settings);
                        if (!result)
                        {
                            throw new Exception(string.Join("Card SettingText TO Sram Error :", Environment.NewLine, result.ToString()));
                        }

                        break;
                    case BundleType.P42:
                        result = CardSramReset(bundleRFCard);
                        if (!result)
                        {
                            throw new Exception(string.Join("Card Sram Reset Error :", Environment.NewLine, result.ToString()));
                        }

                        break;
                    default:
                        result = new DualResult(false, "Not Fund Type");
                        break;
                }
            }
            catch (Exception ex)
            {
                result = new DualResult(false, ex);
            }

            return result;
        }

        /// <summary>
        /// [C16] The card is check by existent location.
        /// </summary>
        /// <returns>DualResult</returns>
        private static DualResult CardPositionCheck(BundleRFCardUSB bundleRFCard)
        {
            DualResult result = new DualResult(false);
            byte[] gbacmd = new byte[3];
            byte[] tDat = new byte[1000], rDat = new byte[1000];
            ushort tLen = 0, res = 0;
            ushort[] rLen = new ushort[2];

            // C16
            gbacmd[0] = 0x43;
            gbacmd[1] = 0x31;
            gbacmd[2] = 0x36;

            res = bundleRFCard.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);

            string errcode = string.Format("0x{0:x4}", res);

            if (rDat[0] == 0x00)
            {
                // There is No card in the unit.
                result = new DualResult(true);
            }
            else
            {
                result = new DualResult(false, errcode);
            }

            return result;
        }

        /// <summary>
        /// [C31] It is to take a card from Stacker and to move it to Card Reader / Writer Module.
        /// </summary>
        /// <returns>DualResult</returns>
        private static DualResult CardFromStacker(BundleRFCardUSB bundleRFCard)
        {
            DualResult result = new DualResult(false);
            byte[] gbacmd = new byte[3];
            byte[] tDat = new byte[1000], rDat = new byte[1000];
            ushort tLen = 0, res = 0;
            ushort[] rLen = new ushort[2];
            string errcode;

            // C31
            gbacmd[0] = 0x43;
            gbacmd[1] = 0x33;
            gbacmd[2] = 0x31;

            tDat[0] = 0x01;     // Select  Stacker 1.
            tDat[1] = 0x03;     // Card moves to Feeder.

            tLen = 2;

            res = bundleRFCard.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
            errcode = string.Format("0x{0:x4}", res);
            if (res != 0x0000)
            {
                result = new DualResult(false, new BaseResult.MessageInfo(errcode));
            }
            else
            {
                result = new DualResult(true);
            }

            return result;
        }

        /// <summary>
        /// [F31] RF card detect in antenna area.
        /// 讀取出 16進位的byte 轉 ASCII = 10進位
        /// * [F30] 為16進位的byte 轉 ASCII = 16進位
        /// </summary>
        /// <returns>DualResult</returns>
        private static DualResult CardRFUiD(BundleRFCardUSB bundleRFCard)
        {
            DualResult result = new DualResult(false);
            byte[] gbacmd = new byte[3];
            byte[] tDat = new byte[1000], rDat = new byte[1000];
            ushort tLen = 0, res = 0;
            ushort[] rLen = new ushort[2];
            string errcode;
            string result_data = string.Empty;
            uint recLen = 0;

            // F31
            gbacmd[0] = 0x46;
            gbacmd[1] = 0x33;
            gbacmd[2] = 0x31;

            res = bundleRFCard.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
            errcode = string.Format("0x{0:x4}", res);

            if (rDat[0] == 0x00)
            {
                var rrDat = new byte[1000];
                recLen = (uint)(rLen[1] << 8);
                recLen |= rLen[0];
                Array.Copy(rDat, 1, rrDat, 0, recLen);

                result_data = Encoding.Default.GetString(rrDat);

                if (int.TryParse(result_data, out int tryPraseResult))
                {
                    result_data = tryPraseResult.ToString();
                }

                // Card UID
                result = new DualResult(true, result_data);
            }
            else
            {
                result = new DualResult(false, new BaseResult.MessageInfo(errcode));
            }

            return result;
        }

        /// <summary>
        /// Convert Html To Image
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <param name="path">路徑</param>
        /// <param name="fileName">檔名</param>
        private static DualResult CardConvertHtmlToImage(DataRow dr, string path, string fileName)
        {
            DualResult result = new DualResult(false);
            Bitmap m_Bitmap = new Bitmap(500, 556);
            PointF point = new PointF(0, 0);
            SizeF maxSize = new SizeF(500, 556);

            string styleIDLast = dr["StyleID"].ToString().Right(5);
            string styleIDFirst = styleIDLast.Empty() ? string.Empty : dr["StyleID"].ToString().Replace(styleIDLast, string.Empty);
            string spLast = dr["ID"].ToString().Right(3);
            string spMiddle = spLast.Empty() ? string.Empty : dr["ID"].ToString().Replace(spLast, string.Empty).Right(6);
            string spFirst = spLast.Empty() || spMiddle.Empty() ? string.Empty : dr["ID"].ToString().Replace(spLast, string.Empty).Replace(spMiddle, string.Empty);

            string subFirst = dr["Artwork"].ToString().Left(19);
            string subLast = subFirst.Empty() ? string.Empty : dr["Artwork"].ToString().Replace(subFirst, string.Empty).Left(23);

            string descFirst = dr["PatternDesc"].ToString().Left(18);
            string descLast = descFirst.Empty() ? string.Empty : dr["PatternDesc"].ToString().Replace(descFirst, string.Empty).Left(23);

            string htmlBody =
                $@"<!DOCTYPE html>
<html style='width:500px; height:556px;'>
<body style='background-color:#ffffff; font-family: Arial; font-size:37px; margin:0px;'>
<div>
    <li style='display: inline;'><span style='background-color: #000000; color: #ffffff; font-weight:bolder;'>Gp:{dr["BundleGroup"].ToString()}</span>(<span style='font-weight: bolder;'>{dr["BundleGroupSerial"].ToString()}</span>/{dr["BundleGroupCount"].ToString()})</li>
    <li style='display: inline; margin-left: 70px; font-weight: bolder;'>Ln#{dr["Sewinglineid"]} SNP</li>
</div>
<div><span style='font-weight: bolder;'>STL#:{dr["StyleID"].ToString()}</span></div>
<div><span style='font-weight: bolder;'>SP#</span>:<span style='font-weight: bolder;'>{spFirst}</span><span style='font-weight: bolder;'>{spMiddle}<u>{spLast}</u></span></div>
<div><span style='font-weight: bolder;'>Color:{dr["Color"].ToString()}<br />Size:{dr["SizeCode"].ToString()}</span></div>
<div style='width: 500px;height: 42px;'>
    <li style='display: inline;'><span style='font-weight: bolder;'>Qty:{dr["Qty"].ToString()}</span></li>
    <li style='display: inline; margin-left: 70px;'><span style='font-weight: bolder;'>Item:{dr["Item"].ToString()}</span></li>
</div>
<div><span style='font-weight: bolder;'>Sea.{dr["Sea"].ToString()}</span></div>
<div><span style='font-weight: bolder;'>Buyer:{dr["BuyerID"].ToString()}</span></div>
<div><span style='font-weight: bolder;'>MK#:{dr["MK"].ToString()}</span></div>
<div><span style='font-weight: bolder;'>Body/Cut#:{dr["BodyCut"].ToString()}</span></div>
<div><span style='font-weight: bolder;'>Sub:{subFirst}<br />{subLast}</span></div>
<div><span style='font-weight: bolder;'>Desc:{descFirst}<br />{descLast}</span></div>
</body></html>
";

            HtmlRenderer.HtmlRender.Render(
               Graphics.FromImage(m_Bitmap),
               htmlBody,
               point,
               maxSize);

            try
            {
                FileExists(path, fileName);
                m_Bitmap.Save(path + @"\" + fileName, System.Drawing.Imaging.ImageFormat.Png);
                result = new DualResult(true);
            }
            catch (Exception ex)
            {
                result = new DualResult(false, ex.ToString());
            }

            return result;
        }

        /// <summary>
        /// P48 Set Print Data
        /// </summary>
        /// <param name="fileName">檔名</param>
        /// <returns>DualResult</returns>
        private static DualResult CardSetPrintData(BundleRFCardUSB bundleRFCard, string fileName)
        {
            DualResult result = new DualResult(false);
            byte[] gbacmd = new byte[3];
            byte[] tDat = new byte[1000], rDat = new byte[1000];
            byte[] dataSendBufY = new byte[150000];
            int tLen = 0, res = 0;
            ushort[] rLen = new ushort[2];
            string errcode;
            string result_data = string.Empty;
            bool ret;

            ret = MakeDll.Func.ResetBitmap();

            MakeDll.Func.ImageDataStruct ii = new MakeDll.Func.ImageDataStruct
            {
                xaxis = "20",
                yaxis = "20",
                width = "500",
                height = "556",
                threshold = "250",
                datapath = "data\\" + fileName,
                filename = fileName,
                rotation = "90",
                property = "IMAGE",
            };

            ret = MakeDll.Func.S_ImageToDithering(ii);

            tLen = MakeDll.Func.S_MakeDataForComSendYLen("P48");
            dataSendBufY = MakeDll.Func.S_MakeDataForComSendY("P48");

            res = bundleRFCard.ImageExeCmd(dataSendBufY, tLen, rDat, rLen, 15000);
            errcode = string.Format("0x{0:x4}", res);
            if (res != 0x0000)
            {
                result = new DualResult(false, errcode);
            }
            else
            {
                result = new DualResult(true);
            }

            return result;
        }

        /// <summary>
        /// P41 Print
        /// </summary>
        /// <returns>DualResult</returns>
        private static DualResult CardPrint(BundleRFCardUSB bundleRFCard)
        {
            DualResult result = new DualResult(false);
            byte[] gbacmd = new byte[3];
            byte[] tDat = new byte[1000], rDat = new byte[1000];
            ushort tLen = 0, res = 0;
            ushort[] rLen = new ushort[2];
            string errcode;

            // P41
            gbacmd[0] = 0x50;
            gbacmd[1] = 0x34;
            gbacmd[2] = 0x31;

            res = bundleRFCard.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
            errcode = string.Format("0x{0:x4}", res);
            if (res != 0x0000)
            {
                result = new DualResult(false, new BaseResult.MessageInfo(errcode));
            }
            else
            {
                result = new DualResult(true);
            }

            return result;
        }

        /// <summary>
        /// [P49] Set Data and Print  =  [P48] + [P41]
        /// </summary>
        /// <param name="path">路徑</param>
        /// <param name="fileName">檔名</param>
        /// <returns>DualResult</returns>
        private static DualResult CardSetAndPrint(BundleRFCardUSB bundleRFCard, string path, string fileName)
        {
            DualResult result = new DualResult(false);
            byte[] gbacmd = new byte[3];
            byte[] tDat = new byte[1000], rDat = new byte[1000];
            byte[] dataSendBufY = new byte[150000];
            int tLen = 0, res = 0;
            ushort[] rLen = new ushort[2];
            string errcode;
            string result_data = string.Empty;
            bool ret;

            MakeDll.Func.ImageDataStruct ii = new MakeDll.Func.ImageDataStruct
            {
                xaxis = "270",
                yaxis = "80",
                width = "500",
                height = "556",
                threshold = "255",
                datapath = "data\\" + fileName,
                filename = fileName,
                rotation = "270",
            };

            ret = MakeDll.Func.ResetBitmap();
            ret = MakeDll.Func.S_ImageToDithering(ii);

            tLen = MakeDll.Func.S_MakeDataForComSendYLen("P49");
            dataSendBufY = MakeDll.Func.S_MakeDataForComSendY("P49");

            res = bundleRFCard.ImageExeCmd(dataSendBufY, tLen, rDat, rLen, 15000);
            errcode = string.Format("0x{0:x4}", res);
            if (res != 0x0000)
            {
                result = new DualResult(false, errcode);
            }
            else
            {
                result = new DualResult(true);
            }

            return result;
        }

        /// <summary>
        /// [C34] It takes card to Bin Box (Capture)
        /// </summary>
        /// <returns>DualResult</returns>
        private static DualResult CardCapture(BundleRFCardUSB bundleRFCard)
        {
            DualResult result = new DualResult(false);
            byte[] gbacmd = new byte[3];
            byte[] tDat = new byte[1000], rDat = new byte[1000];
            ushort tLen = 0, res = 0;
            ushort[] rLen = new ushort[2];
            string errcode;

            // C34
            gbacmd[0] = 0x43;
            gbacmd[1] = 0x33;
            gbacmd[2] = 0x34;

            res = bundleRFCard.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
            errcode = string.Format("0x{0:x4}", res);
            if (res != 0x0000)
            {
                result = new DualResult(false, errcode);
            }
            else
            {
                result = new DualResult(true);
            }

            return result;
        }

        /// <summary>
        /// [C36] Dispense the card to front and drop it out of the unit.
        /// </summary>
        /// <returns>DualResult</returns>
        private static DualResult CardDrop(BundleRFCardUSB bundleRFCard)
        {
            DualResult result = new DualResult(false);
            byte[] gbacmd = new byte[3];
            byte[] tDat = new byte[1000], rDat = new byte[1000];
            ushort tLen = 0, res = 0;
            ushort[] rLen = new ushort[2];
            string errcode;

            // C36
            gbacmd[0] = 0x43;
            gbacmd[1] = 0x33;
            gbacmd[2] = 0x36;

            res = bundleRFCard.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
            errcode = string.Format("0x{0:x4}", res);
            if (res != 0x0000)
            {
                result = new DualResult(false, new BaseResult.MessageInfo(errcode));
            }
            else
            {
                result = new DualResult(true);
            }

            return result;
        }

        /// <summary>
        /// [P21] All of the Card Erase. (For SNP company)
        /// </summary>
        /// <returns>DualResult</returns>
        private static DualResult CardErase(BundleRFCardUSB bundleRFCard)
        {
            DualResult result = new DualResult(false);
            byte[] gbacmd = new byte[3];
            byte[] tDat = new byte[1000], rDat = new byte[1000];
            ushort tLen = 0, res = 0;
            ushort[] rLen = new ushort[2];
            string errcode;
            string result_data = string.Empty;

            // F30
            gbacmd[0] = 0x50;
            gbacmd[1] = 0x32;
            gbacmd[2] = 0x31;

            res = bundleRFCard.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
            errcode = string.Format("0x{0:x4}", res);
            if (rDat[0] == 0x00)
            {
                result = new DualResult(true);
            }
            else
            {
                result = new DualResult(false, errcode);
            }

            return result;
        }

        /// <summary>
        /// [P22] It sets the partial Erase Area.
        /// </summary>
        /// <returns>DualResult</returns>
        private static DualResult CardEraseSettingArea(BundleRFCardUSB bundleRFCard)
        {
            DualResult result = new DualResult(true);
            byte[] gbacmd = new byte[3];
            byte[] tDat = new byte[1000], rDat = new byte[1000];
            ushort tLen = 0, res = 0;
            ushort[] rLen = new ushort[2];
            string errcode;
            byte[] chr = new byte[4];
            int len;
            byte[] chrTdat = new byte[100];

            // P22
            gbacmd[0] = 0x50;
            gbacmd[1] = 0x32;
            gbacmd[2] = 0x32;

            int xStart = 20, xEnd = 620, yStart = 50, yEnd = 735;

            len = CaclLen(xStart);
            tDat[0] = (byte)(len >> 8);
            tDat[1] = (byte)len; // x Start Point
            len = CaclLen(xEnd);
            tDat[2] = (byte)(len >> 8);
            tDat[3] = (byte)len; // x End Point
            len = CaclLen(yStart);
            tDat[4] = (byte)(len >> 8);
            tDat[5] = (byte)len; // y Start Point
            len = CaclLen(yEnd);
            tDat[6] = (byte)(len >> 8);
            tDat[7] = (byte)len; // y End Point

            tLen = Convert.ToUInt16(tDat.Length);
            res = bundleRFCard.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
            errcode = string.Format("0x{0:x4}", res);
            if (rDat[0] == 0x00)
            {
                result = new DualResult(true);
            }
            else
            {
                result = new DualResult(false, new BaseResult.MessageInfo(errcode));
            }

            return result;
        }

        /// <summary>
        /// [P24] Card Erase the partial Area.
        /// </summary>
        /// <returns>DualResult</returns>
        private static DualResult CardErasePartialArea(BundleRFCardUSB bundleRFCard)
        {
            DualResult result = new DualResult(false);
            byte[] gbacmd = new byte[3];
            byte[] tDat = new byte[1000], rDat = new byte[1000];
            ushort tLen = 0, res = 0;
            ushort[] rLen = new ushort[2];
            string errcode;
            string result_data = string.Empty;

            // P24
            gbacmd[0] = 0x50;
            gbacmd[1] = 0x32;
            gbacmd[2] = 0x34;

            res = bundleRFCard.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
            errcode = string.Format("0x{0:x4}", res);
            if (rDat[0] == 0x00)
            {
                result = new DualResult(true);
            }
            else
            {
                result = new DualResult(false, new BaseResult.MessageInfo(errcode));
            }

            return result;
        }

        /// <summary>
        /// [P32] It is to clean Thermal Printer Head.
        /// </summary>
        /// <returns>DualResult</returns>
        private static DualResult CardCleanThermalPrinterHead(BundleRFCardUSB bundleRFCard)
        {
            DualResult result = new DualResult(false);
            byte[] gbacmd = new byte[3];
            byte[] tDat = new byte[1000], rDat = new byte[1000];
            ushort tLen = 0, res = 0;
            ushort[] rLen = new ushort[2];
            string errcode;

            // P32
            gbacmd[0] = 0x50;
            gbacmd[1] = 0x33;
            gbacmd[2] = 0x32;

            res = bundleRFCard.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
            errcode = string.Format("0x{0:x4}", res);
            if (rDat[0] == 0x00)
            {
                result = new DualResult(true);
            }
            else
            {
                result = new DualResult(false, new BaseResult.MessageInfo(errcode));
            }

            return result;
        }

        /// <summary>
        /// [P35] Setting the Text data to the Sram.(position free)
        /// </summary>
        /// <returns>DualResult</returns>
        private static DualResult CardSettingTextTOSram(BundleRFCardUSB bundleRFCard, List<string> settings)
        {
            DualResult result = new DualResult(true);
            byte[] gbacmd = new byte[3];
            byte[] tDat = new byte[1000], rDat = new byte[1000];
            ushort tLen = 0, res = 0;
            ushort[] rLen = new ushort[2];
            string errcode;
            byte[] chr = new byte[4];
            int len;
            byte[] chrTdat = new byte[100];

            if (settings.Count == 0)
            {
                return new DualResult(false, "No Data");
            }

            int x = 10;
            int y = 245;
            foreach (string str in settings)
            {
                if (str.Empty())
                {
                    continue;
                }

                // P35
                gbacmd[0] = 0x50;
                gbacmd[1] = 0x33;
                gbacmd[2] = 0x35;

                #region x setting
                len = x.ToString().Length;
                chr = Encoding.ASCII.GetBytes(x.ToString());
                if (len >= 3)
                {
                    len = ((chr[0] & 0xF) * 100) + ((chr[1] & 0xF) * 10) + (chr[2] & 0xF);
                }
                else if (len == 2)
                {
                    len = ((chr[0] & 0xF) * 10) + (chr[1] & 0xF);
                }
                else if (len == 1)
                {
                    len = chr[0] & 0xF;
                }

                tDat[0] = (byte)(len >> 8);
                tDat[1] = (byte)len; // y Point
                #endregion
                #region y setting
                len = y.ToString().Length;
                chr = Encoding.ASCII.GetBytes(y.ToString());
                if (len >= 3)
                {
                    len = ((chr[0] & 0xF) * 100) + ((chr[1] & 0xF) * 10) + (chr[2] & 0xF);
                }
                else if (len == 2)
                {
                    len = ((chr[0] & 0xF) * 10) + (chr[1] & 0xF);
                }
                else if (len == 1)
                {
                    len = chr[0] & 0xF;
                }

                tDat[2] = (byte)(len >> 8);
                tDat[3] = (byte)len; // y Point
                #endregion

                tDat[4] = (byte)1; // Font Select
                tDat[5] = (byte)3; // Rotate

                // Text Data
                chrTdat = Encoding.ASCII.GetBytes(str);
                for (int i = 0; i < str.Length; i++)
                {
                    tDat[i + 6] = chrTdat[i];
                }

                tLen = Convert.ToUInt16(6 + str.Length);

                res = bundleRFCard.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
                errcode = string.Format("0x{0:x4}", res);
                if (res != 0x0000)
                {
                    result = new DualResult(false, new BaseResult.MessageInfo(errcode));
                }

                if (!result)
                {
                    return result;
                }

                y = y + 40;
            }

            return result;
        }

        /// <summary>
        /// [P37] It sets Bar Code options into the Sram buffer.
        /// </summary>
        /// <returns>DualResult</returns>
        private static DualResult CardSettingBarcodeSram(BundleRFCardUSB bundleRFCard, string barcode)
        {
            DualResult result = new DualResult(true);
            byte[] gbacmd = new byte[3];
            byte[] tDat = new byte[1000], rDat = new byte[1000];
            ushort tLen = 0, res = 0;
            ushort[] rLen = new ushort[2];
            string errcode;
            byte[] chr = new byte[4];
            int len;
            byte[] chrTdat = new byte[100];

            if (barcode.Empty())
            {
                return new DualResult(true);
            }

            int x = 10;
            int y = 725;
            int h = 80;

            // P37
            gbacmd[0] = 0x50;
            gbacmd[1] = 0x33;
            gbacmd[2] = 0x37;

            #region x setting
            len = CaclLen(x);
            chr = Encoding.ASCII.GetBytes(x.ToString());

            tDat[0] = (byte)(len >> 8);
            tDat[1] = (byte)len; // x Point
            #endregion
            #region y setting
            len = CaclLen(y);
            chr = Encoding.ASCII.GetBytes(y.ToString());

            tDat[2] = (byte)(len >> 8);
            tDat[3] = (byte)len; // y Point
            #endregion

            tDat[4] = (byte)1; // Font Select
            tDat[5] = (byte)3; // Text Direction
            tDat[6] = (byte)1; // Bar-Code Scale

            len = CaclLen(h);
            chr = Encoding.ASCII.GetBytes(h.ToString());
            tDat[7] = (byte)(len >> 8);
            tDat[8] = (byte)len; // Height

            tDat[9] = (byte)1; // Bar Code Digit On/Off

            // Text Data
            char[] charArray = barcode.ToCharArray();
            chrTdat = Encoding.ASCII.GetBytes(charArray);
            for (int i = 0; i < charArray.Length; i++)
            {
                tDat[i + 10] = chrTdat[i];
            }

            tLen = Convert.ToUInt16(10 + charArray.Length);

            res = bundleRFCard.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
            errcode = string.Format("0x{0:x4}", res);
            if (res != 0x0000)
            {
                result = new DualResult(false, new BaseResult.MessageInfo(errcode));
            }

            if (!result)
            {
                return result;
            }

            return result;
        }

        /// <summary>
        /// [P42] Sram Reset
        /// </summary>
        /// <returns>DualResult</returns>
        private static DualResult CardSramReset(BundleRFCardUSB bundleRFCard)
        {
            DualResult result = new DualResult(false);
            byte[] gbacmd = new byte[3];
            byte[] tDat = new byte[1000], rDat = new byte[1000];
            ushort tLen = 0, res = 0;
            ushort[] rLen = new ushort[2];
            string errcode;
            string result_data = string.Empty;

            // P42
            gbacmd[0] = 0x50;
            gbacmd[1] = 0x34;
            gbacmd[2] = 0x32;

            res = bundleRFCard.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
            errcode = string.Format("0x{0:x4}", res);
            if (rDat[0] == 0x00)
            {
                result = new DualResult(true);
            }
            else
            {
                result = new DualResult(false, new BaseResult.MessageInfo(errcode));
            }

            return result;
        }

        /// <summary>
        /// 判斷資料夾與檔案是否存在
        /// 資料夾不存在則建立
        /// 檔案存在則刪除
        /// </summary>
        /// <param name="path">路徑</param>
        /// <param name="fileName">檔名</param>
        private static void FileExists(string path, string fileName)
        {
            // 資料夾是否存在
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo file in dir.GetFiles("*.png").Where(x => x.EqualString(fileName)))
            {
                file.Delete();
            }
        }

        /// <summary>
        /// 回寫Bundle_Detail RFUID
        /// </summary>
        /// <param name="id">Bundle ID</param>
        /// <param name="bundleNO">BundleNo</param>
        /// <param name="rfUID">RFUID</param>
        /// <returns>DualResult</returns>
        private static DualResult UpdateBundleDetailRFUID(string id, string bundleNO, string rfUID)
        {
            string sqlCmd = $"update Bundle_Detail set RFUID = '{rfUID}', RFPrintDate = Getdate() where ID = '{id}' and BundleNo = '{bundleNO}'";
            DualResult result = Data.DBProxy.Current.Execute(string.Empty, sqlCmd);
            return result;
        }

        /// <summary>
        /// 檔案刪除
        /// </summary>
        /// <param name="path">路徑</param>
        /// <param name="fileName">檔名</param>
        private static void FileDelete(string path, string fileName)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo file in dir.GetFiles("*.png").Where(x => x.EqualString(fileName)))
            {
                file.Delete();
            }
        }

        /// <summary>
        /// Convert Html To Image
        /// </summary>
        private static DualResult GetSettingText(DataRow dr, out List<string> settings)
        {
            DualResult result = new DualResult(false);

            string styleIDLast = dr["StyleID"].ToString().Right(5);
            string styleIDFirst = styleIDLast.Empty() ? string.Empty : dr["StyleID"].ToString().Replace(styleIDLast, string.Empty);
            string spLast = dr["ID"].ToString().Right(3);
            string spMiddle = spLast.Empty() ? string.Empty : dr["ID"].ToString().Replace(spLast, string.Empty).Right(6);
            string spFirst = spLast.Empty() || spMiddle.Empty() ? string.Empty : dr["ID"].ToString().Replace(spLast, string.Empty).Replace(spMiddle, string.Empty);

            string subFirst = dr["Artwork"].ToString().Left(19);
            string subLast = subFirst.Empty() ? string.Empty : dr["Artwork"].ToString().Replace(subFirst, string.Empty).Left(23);

            string descFirst = dr["PatternDesc"].ToString().Left(18);
            string descLast = descFirst.Empty() ? string.Empty : dr["PatternDesc"].ToString().Replace(descFirst, string.Empty).Left(23);

            settings = new List<string>()
            {
                $"Gp:{dr["BundleGroup"].ToString()}({dr["BundleGroupSerial"].ToString()}/{dr["BundleGroupCount"].ToString()})                Ln#{dr["Sewinglineid"]} SNP",
                $"STL#:{dr["StyleID"].ToString()}",
                $"SP# {dr["ID"].ToString()}",
                $"Color:{dr["Color"].ToString()}   Size:{dr["SizeCode"].ToString()}",
                $"Qty:{dr["Qty"].ToString()}                    Item:{dr["Item"].ToString()}",
                $"Sea.{dr["Sea"].ToString()}",
                $"Buyer:{dr["BuyerID"].ToString()}",
                $"MK#:{dr["MK"].ToString()}",
                $"Body/Cut#:{dr["BodyCut"].ToString()}",
                $"Sub:{subFirst}",
                $"{subLast}",
                $"Desc:{descFirst}",
                $"{descLast}",
            };

            try
            {
                result = new DualResult(true);
            }
            catch (Exception ex)
            {
                result = new DualResult(false, ex.ToString());
            }

            return result;
        }

        /// <summary>
        /// Convert Html To Image
        /// </summary>
        /// <param name="dr">DataRow</param>
        private static DualResult GetSettingText(P10_PrintData printData, out List<string> settings)
        {
            DualResult result = new DualResult(false);
            settings = new List<string>()
            {
                $"Grp:{printData.Group_right}  Tone:{printData.Tone}  Line#:{printData.Line} {printData.Group_left}",
            };

            string nSP = "SP#:" + printData.SP;
            int len = nSP.Length % 32 == 0 ? nSP.Length / 32 : (nSP.Length / 32) + 1;
            for (int i = 0; i <= len - 1; i++)
            {
                settings.Add($"{nSP.Left(32)}");
                nSP = nSP.Replace(nSP.Left(32), string.Empty);
            }

            settings.AddRange(new List<string>()
            {
                $"Style#:{printData.Style}",
                $"Cut#:{printData.Body_Cut}",
                $"Color:{printData.Color}",
                $"Size:{printData.Size}     Part:{printData.Parts}",
                $"Sea:{printData.Season}     Brand:{printData.ShipCode}",
                $"MK#:{printData.MarkerNo}     Cut/L:",
                $"Sub Process:{printData.Artwork}",
                $"Desc:{printData.Desc}",
                $"Qty:{printData.Quantity}(#{printData.No})  Item:{printData.Item}",
            });

            try
            {
                result = new DualResult(true);
            }
            catch (Exception ex)
            {
                result = new DualResult(false, new BaseResult.MessageInfo(ex.ToString()));
            }

            return result;
        }

        private static int CaclLen(int x)
        {
            int len = x.ToString().Length;
            byte[] chr = Encoding.ASCII.GetBytes(x.ToString());
            if (len >= 3)
            {
                len = ((chr[0] & 0xF) * 100) + ((chr[1] & 0xF) * 10) + (chr[2] & 0xF);
            }
            else if (len == 2)
            {
                len = ((chr[0] & 0xF) * 10) + (chr[1] & 0xF);
            }
            else if (len == 1)
            {
                len = chr[0] & 0xF;
            }

            return len;
        }

        /// <summary>
        /// 錯誤訊息代碼對應的描述
        /// </summary>
        /// <param name="code">錯誤代碼</param>
        /// <returns>錯誤代碼描述</returns>
        private static string BFPrintErrorMSG(string code)
        {
            string rtnStr = string.Empty;
            switch (code)
            {
                case "0x0010":
                    rtnStr = "The printer is not connected to PC. Please check the connection.";
                    break;
                case "0x2001":
                    rtnStr = "Using the command that does not defined in this model.";
                    break;
                case "0x2002":
                    rtnStr = "Not available command in this model. ";
                    break;
                case "0x2003":
                    rtnStr = "Sending the command that has the invalid communication frame. ";
                    break;
                case "0x2004":
                    rtnStr = "When the card is jammed. ";
                    break;
                case "0x2005":
                    rtnStr = "No cards. ";
                    break;
                case "0x2006":
                    rtnStr = "When the card exists already in the terminal. ";
                    break;
                case "0x2007":
                    rtnStr = "When the terminal is running or busy.";
                    break;
                case "0x2008":
                    rtnStr = "When the RTC time is incorrect by internal terminal or incorrect input data. ";
                    break;
                case "0x2009":
                    rtnStr = "When more than two cards exit in the terminal simultaneously.";
                    break;
                case "0x200B":
                    rtnStr = "When the using card error, commonly occur in MSRW.";
                    break;
                case "0x2100":
                    rtnStr = "Not Applicable Dispenser. ";
                    break;
                case "0x2101":
                    rtnStr = "Dispenser communication error";
                    break;
                case "0x2104":
                    rtnStr = "No cards at stacker. ";
                    break;
                case "0x2300":
                    rtnStr = "Unavailable RF module. ";
                    break;
                case "0x2301":
                    rtnStr = "Communication error at the RF Module. ";
                    break;
                case "0x2302":
                    rtnStr = "Authentication Error at the RF Module. ";
                    break;
                case "0x2303":
                    rtnStr = "Error while the terminal writes at the RF Card.";
                    break;
                case "0x2304":
                    rtnStr = "Error while the terminal reads at the RF Card. ";
                    break;
                case "0x2305":
                    rtnStr = "No RF Card. ";
                    break;
                case "0x2306":
                    rtnStr = "Error while the value increases(or decreases) at the RF Card.";
                    break;
                case "0x2400":
                    rtnStr = "Unavailable FLASH memory ic. ";
                    break;
                case "0x2600":
                    rtnStr = "Unavailable PRINTER module.";
                    break;
                case "0x2601":
                    rtnStr = "Unavailable PRINTER module.";
                    break;
                case "0x2602":
                    rtnStr = "THERMAL SHUTTER OPEN ERROR. ";
                    break;
                case "0x2603":
                    rtnStr = "THERMAL SHUTTER CLOSE ERROR.";
                    break;
                case "0x2604":
                    rtnStr = " Too big the chosen value. ";
                    break;
                case "0x2608":
                    rtnStr = "Can’t detect the Black mark.";
                    break;
                case "0x2609":
                    rtnStr = "Too High the Thermal head temperature.";
                    break;
                case "0x2620":
                    rtnStr = "Exceeded the print count. ";
                    break;
                default:
                    break;
            }

            return rtnStr.Empty() ? code : code + " : " + rtnStr;
        }
    }
}
