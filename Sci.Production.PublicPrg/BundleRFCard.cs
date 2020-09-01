using Ict;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;

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
            F30 = 2,

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
			 left join Bundle_Detail_Allpart bdall WITH (NOLOCK) on bdall.ID = bda.ID 
			 where bda.id = bd.Id 
			 and bda.PatternCode = bdall.PatternCode
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
        /// <returns>DualResult</returns>
        public static DualResult BundleRFCardPrint(DataTable dt)
        {
            DualResult result = new DualResult(false);

            try
            {
                if (BundleRFCardUSB.UsbPortOpen())
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        // C31
                        result = CardFromStacker();
                        if (!result)
                        {
                            throw new Exception(string.Join("Card Get From Stacker Error :", Environment.NewLine, result.ToString()));
                        }

                        // P21 All of the Card Erase. (For SNP company)
                        result = CardErase();
                        if (!result)
                        {
                            throw new Exception(string.Join("Card Erase Error :", Environment.NewLine, result.ToString()));
                        }

                        // P42 Sram Reset
                        result = CardSramReset();
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

                        result = CardSettingTextTOSram(settings);
                        if (!result)
                        {
                            throw new Exception(string.Join("Card SettingText TO Sram Error :", Environment.NewLine, result.ToString()));
                        }

                        // P41
                        result = CardPrint();
                        if (!result)
                        {
                            throw new Exception(string.Join("Card Print Error :", Environment.NewLine, result.ToString()));
                        }

                        // F30
                        string cardUID = string.Empty;
                        result = CardRFUiD();
                        if (result)
                        {
                            cardUID = result.Description;
                        }
                        else
                        {
                            throw new Exception(string.Join("Card Get RF UID Error :", Environment.NewLine, result.ToString()));
                        }

                        // write DB
                        result = UpdateBundleDetailRFUID(dr["BundleID"].ToString(), dr["BundleNo"].ToString(), cardUID);
                        if (!result)
                        {
                            throw new Exception(string.Join("Write To DB Error :", Environment.NewLine, result.ToString()));
                        }

                        // C34
                        result = CardCapture();
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
                BundleRFCardUSB.UsbPortClose();
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

                dr = dt.Rows[0];
                switch (type)
                {
                    case BundleType.Open:
                        bool isopen = BundleRFCardUSB.UsbPortOpen();
                        result = new DualResult(isopen, isopen ? string.Empty : "Printer(CHP_1800) usb port not open");
                        break;
                    case BundleType.Close:
                        bool isClose = BundleRFCardUSB.UsbPortClose();
                        result = new DualResult(isClose, isClose ? string.Empty : "Printer(CHP_1800) usb port not Close");
                        break;
                    case BundleType.C31:
                        result = CardFromStacker();
                        break;
                    case BundleType.F30:
                        result = CardRFUiD();

                        MyUtility.Msg.InfoBox("BundleID" + dr["BundleID"].ToString() + ",BundleNo" + dr["BundleNo"].ToString() + ",RFUiD" + result.Description);
                        break;
                    case BundleType.Create:
                        path = Directory.GetCurrentDirectory() + @"\data";
                        fileName = dr["BundleNo"].ToString() + ".png";
                        result = CardConvertHtmlToImage(dr, path, fileName);
                        break;
                    case BundleType.P48:
                        fileName = dr["BundleNo"].ToString() + ".png";
                        result = CardSetPrintData(fileName);
                        break;
                    case BundleType.P41:
                        result = CardPrint();
                        break;
                    case BundleType.P49:
                        path = Directory.GetCurrentDirectory() + @"\data";
                        fileName = dr["BundleNo"].ToString() + ".png";
                        result = CardSetAndPrint(path, fileName);
                        break;
                    case BundleType.C34:
                        result = CardCapture();
                        break;
                    case BundleType.WDB:
                        result = CardRFUiD();
                        if (!result)
                        {
                            throw new Exception(string.Join("Card Get RF UID Error :", Environment.NewLine, result.ToString()));
                        }

                        MyUtility.Msg.InfoBox("BundleID" + dr["BundleID"].ToString() + ",BundleNo" + dr["BundleNo"].ToString() + ",RFUiD" + result.Description);

                        result = UpdateBundleDetailRFUID(dr["BundleID"].ToString(), dr["BundleNo"].ToString(), result.Description);
                        break;
                    case BundleType.P21:
                        result = CardErase();
                        break;
                    case BundleType.P35:
                        List<string> settings = new List<string>();
                        result = GetSettingText(dr, out settings);
                        if (!result)
                        {
                            throw new Exception(string.Join("Get SettingText Error :", Environment.NewLine, result.ToString()));
                        }

                        result = CardSettingTextTOSram(settings);
                        if (!result)
                        {
                            throw new Exception(string.Join("Card SettingText TO Sram Error :", Environment.NewLine, result.ToString()));
                        }

                        break;
                    case BundleType.P42:
                        result = CardSramReset();
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
        private static DualResult CardPositionCheck()
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

            res = BundleRFCardUSB.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);

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
        private static DualResult CardFromStacker()
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
            tDat[1] = 0x05;     // Card moves to Feeder.

            tLen = 2;

            res = BundleRFCardUSB.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
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
        /// [F30] RF card detect in antenna area.
        /// </summary>
        /// <returns>DualResult</returns>
        private static DualResult CardRFUiD()
        {
            DualResult result = new DualResult(false);
            byte[] gbacmd = new byte[3];
            byte[] tDat = new byte[1000], rDat = new byte[1000];
            ushort tLen = 0, res = 0;
            ushort[] rLen = new ushort[2];
            string errcode;
            string result_data = string.Empty;
            uint recLen = 0;

            // F30
            gbacmd[0] = 0x46;
            gbacmd[1] = 0x33;
            gbacmd[2] = 0x30;

            res = BundleRFCardUSB.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
            errcode = string.Format("0x{0:x4}", res);

            if (rDat[0] == 0x00)
            {
                recLen = (uint)(rLen[1] << 8);
                recLen |= rLen[0];
                for (int i = 0; i <= recLen - 1; i++)
                {
                    result_data += Convert.ToInt32(rDat[i]);
                }

                // Card UID
                result = new DualResult(true, result_data.Right(16));
            }
            else
            {
                result = new DualResult(false, errcode);
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
        private static DualResult CardSetPrintData(string fileName)
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

            res = BundleRFCardUSB.ImageExeCmd(dataSendBufY, tLen, rDat, rLen, 15000);
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
        private static DualResult CardPrint()
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

            res = BundleRFCardUSB.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
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
        /// [P49] Set Data and Print  =  [P48] + [P41]
        /// </summary>
        /// <param name="path">路徑</param>
        /// <param name="fileName">檔名</param>
        /// <returns>DualResult</returns>
        private static DualResult CardSetAndPrint(string path, string fileName)
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

            res = BundleRFCardUSB.ImageExeCmd(dataSendBufY, tLen, rDat, rLen, 15000);
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
        private static DualResult CardCapture()
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

            res = BundleRFCardUSB.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
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
        /// [P21] All of the Card Erase. (For SNP company)
        /// </summary>
        /// <returns>DualResult</returns>
        private static DualResult CardErase()
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

            res = BundleRFCardUSB.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
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
        /// [P35] Setting the Text data to the Sram.(position free)
        /// </summary>
        /// <returns>DualResult</returns>
        private static DualResult CardSettingTextTOSram(List<string> settings)
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
            int y = 225;
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

                res = BundleRFCardUSB.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
                errcode = string.Format("0x{0:x4}", res);
                if (res != 0x0000)
                {
                    result = new DualResult(false, errcode);
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
        /// [P42] Sram Reset
        /// </summary>
        /// <returns>DualResult</returns>
        private static DualResult CardSramReset()
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

            res = BundleRFCardUSB.ExeCmd(gbacmd, tDat, tLen, rDat, rLen, 15000);
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
            string sqlCmd = $"update Bundle_Detail set RFUID = '{rfUID}' where ID = '{id}' and BundleNo = '{bundleNO}'";
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
        /// <param name="dr">DataRow</param>
        private static DualResult GetSettingText(DataRow dr, out List<string> settings)
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
    }
}
