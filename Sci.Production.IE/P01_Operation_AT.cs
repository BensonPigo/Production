using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class.Command;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <inheritdoc/>
    public partial class P01_Operation_AT : Sci.Win.Subs.Base
    {
        private string strIetmsUkey;
        private string strCodeFrom;
        private DataTable dtRPM;
        private DataTable dtSpeed;
        private DataTable dtHead;
        private string strTimeStudyID = string.Empty;
        private DataTable dtDetail;

        private List<P01_OperationClass> p01List;

        /// <inheritdoc/>
        public P01_Operation_AT(string[] typeList, ref DataTable dataTable,ref List<P01_OperationClass> list, string ietmsUkey = "", string codeFrom = "", bool isEnabled = true, bool isSetAT = true, string strTimeStudyID = "")
        {
            this.InitializeComponent();
            this.strIetmsUkey = ietmsUkey;
            this.strCodeFrom = codeFrom;
            this.strTimeStudyID = strTimeStudyID;
            this.Text = !isSetAT ? "Std. AT" : "Operation AT";
            this.dtDetail = dataTable;
            this.btnStdAT.Visible = isSetAT;
            this.btnCalculate.Visible = isEnabled;
            this.btnConfirm.Visible = isEnabled;
            this.cbRPM.ReadOnly = !isEnabled;
            this.cbSpeedLaser.ReadOnly = !isEnabled;
            this.txtPieceOfSeamer.ReadOnly = !isEnabled;
            #region 表頭
            string sqlcmd_Head = $@"
            SELECT
             [Type] = 'AT'
            ,[ATGroup] = getATGroup.DisplayATGroup
            ,[Component] = IA.Component
            ,[IsQuilting] = IA.IsQuilting
            ,[PieceOfSeamer] = IIF(IA.PieceOfSeamerEdited <> 0 ,IA.PieceOfSeamerEdited , IA.PieceOfSeamer)
            ,[TradePieceOfSeamer] = IA.PieceOfSeamer
            ,[PieceOfGarment] = IA.PieceOfGarment
            ,[OperationID] = IA.OperationID
            ,[TradeRPM] = IA.RPM
            ,[RPM] =  COALESCE(NULLIF(IA.RPMEdited , ''),IA.RPM)
            ,[LaserSpeed] = COALESCE(NULLIF(IA.LaserSpeedEdited, ''), IA.LaserSpeed)
            ,[TradeLaserSpeed] = isnull(IA.LaserSpeed,'')
            ,[SewingLength] = isnull(IA.SewingLength,'')
            ,[SewingLine] = IA.SewingLine
            ,[LaserLength] =IA.LaserLength
            ,[LaserLine] = IA.LaserLine
            FROM IETMS_AT IA
            Outer apply (
	            Select DisplayATGroup = STUFF((
		            select ',' + gla.Annotation
		            from Pattern_GL_Artwork gla
		            where gla.UKEY in (select data from dbo.SplitString(IA.Pattern_GL_ArtworkUkey, ','))
		            for xml path ('')), 1, 1, '' )
            ) getATGroup
            where 
            IA.IETMSUkey = {ietmsUkey} and
            IA.CodeFrom = '{codeFrom}'";
            DualResult dualResult = DBProxy.Current.Select(null, sqlcmd_Head, out this.dtHead);
            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            this.cbType.Items.AddRange(typeList);
            this.cbType.SelectedValue2 = "AT";

            this.cbType.Text = MyUtility.Convert.GetString(this.dtHead.Rows[0]["Type"]);
            this.txtATGroup.Text = MyUtility.Convert.GetString(this.dtHead.Rows[0]["ATGroup"]);
            this.txtComponent.Text = MyUtility.Convert.GetString(this.dtHead.Rows[0]["Component"]);
            this.chkQuilting.Checked = MyUtility.Convert.GetBool(this.dtHead.Rows[0]["IsQuilting"]);
            this.txtReplacedOperation.Text = MyUtility.Convert.GetString(this.dtHead.Rows[0]["OperationID"]);
            this.txtPieceOfGarment.Text = MyUtility.Convert.GetString(this.dtHead.Rows[0]["PieceOfGarment"]);
            this.txtPieceOfSeamer.Text = isSetAT ? MyUtility.Convert.GetString(this.dtHead.Rows[0]["PieceOfSeamer"]) : MyUtility.Convert.GetString(this.dtHead.Rows[0]["TradePieceOfSeamer"]);
            #endregion 表頭

            #region RPM下拉選單

            string sqlRPM = $@" 
            Select [ID] = '0', [Name] = ' ', [Value] = 0
            UNION
            Select [ID], [Name], [Value] from IESelectCode where Type = '00016'";

            DualResult cbResult = DBProxy.Current.Select(null, sqlRPM, out this.dtRPM);
            if (!cbResult)
            {
                MyUtility.Msg.WarningBox(cbResult.ToString());
                return;
            }
            else
            {
                this.cbRPM.DataSource = this.dtRPM;
                this.cbRPM.ValueMember = "ID";
                this.cbRPM.DisplayMember = "Name";
            }

            #endregion RPM下拉選單

            #region Speed下拉選單

            string sqlSpeed = $@"
            Select [ID] = '0', [Name] = ' ', [Value] = 0
            UNION
            select [ID], [Name], [Value] from IESelectCode where Type = '00017'";

            if (cbResult = DBProxy.Current.Select(null, sqlSpeed, out this.dtSpeed))
            {
                this.cbSpeedLaser.DataSource = this.dtSpeed;
                this.cbSpeedLaser.ValueMember = "ID";
                this.cbSpeedLaser.DisplayMember = "Name";
            }
            else
            {
                this.ShowErr(cbResult);
                return;
            }
            #endregion

            #region 下方表身
            this.cbRPM.SelectedValue2 = isSetAT ? MyUtility.Convert.GetString(this.dtHead.Rows[0]["RPM"]) : MyUtility.Convert.GetString(this.dtHead.Rows[0]["TradeRPM"]);
            this.cbSpeedLaser.SelectedValue2 = isSetAT ? MyUtility.Convert.GetString(this.dtHead.Rows[0]["LaserSpeed"]) : MyUtility.Convert.GetString(this.dtHead.Rows[0]["TradeLaserSpeed"]);
            this.txtLenghtSweing.Text = MyUtility.Convert.GetString(this.dtHead.Rows[0]["SewingLength"]);
            this.txtLineSewing.Text = MyUtility.Convert.GetString(this.dtHead.Rows[0]["SewingLine"]);
            this.txtLenghtLaser.Text = MyUtility.Convert.GetString(this.dtHead.Rows[0]["LaserLength"]);
            this.txtLineLaser.Text = MyUtility.Convert.GetString(this.dtHead.Rows[0]["LaserLine"]);
            #endregion 下方表身

            this.GetBottom();

            #region 存取原始資料
            this.p01List = list;
            if (this.p01List.Where(x => x.IETMSUkey == ietmsUkey && x.CodeFrom == codeFrom).ToList().Count == 0 && isEnabled)
            {
                this.p01List.Add(new P01_OperationClass()
                {
                    IETMSUkey = MyUtility.Convert.GetString(this.strIetmsUkey),
                    CodeFrom = MyUtility.Convert.GetString(this.strCodeFrom),
                    ID = MyUtility.Convert.GetString(this.strTimeStudyID),
                    PieceOfSeamerEdited = MyUtility.Convert.GetString(this.txtPieceOfSeamer.Text),
                    RPMEdited = MyUtility.Convert.GetString(this.cbRPM.SelectedValue2),
                    LaserSpeedEdited = MyUtility.Convert.GetString(this.cbSpeedLaser.SelectedValue2),
                    AT_SMV = MyUtility.Convert.GetString(this.txtAT_Garment.Text),
                    MM2AT_SMV = MyUtility.Convert.GetString(this.txtMM2AT_Gaement.Text),
                });
            }
            #endregion 存取原始資料
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("ITEM", header: "Item", width: Widths.AnsiChars(30))
            .Text("DESC", header: "Desc", width: Widths.AnsiChars(40))
            .Numeric("NUMBER", header: "Number", decimal_places: 0, integer_places: 0, iseditingreadonly: false)
            .Numeric("SEC", header: "Sec.", decimal_places: 3, integer_places: 3, iseditingreadonly: true)
            .ExtEditText("REMARK", header: "Remark", width: Widths.AnsiChars(15), charCasing: CharacterCasing.Normal, iseditingreadonly: false, maxLength: 100)
            ;
            this.GetDataSource();
        }

        private void GetDataSource()
        {
            DualResult dualResult;
            string sqlcmd = $@"
            SELECT
            [ITEM] = R.Name
            ,[DESC] = IC.Name
            ,[NUMBER] = IAD.Number
            ,[SEC] = IAD.Value
            ,[REMARK] = IAD.Remark
            FROM IETMS_AT_Detail IAD WITH(NOLOCK)
            LEFT JOIN IESelectCode IC WITH(NOLOCK) ON IAD.IESelectCodeType = IC.Type AND IAD.IESelectCodeID = IC.ID
            LEFT JOIN Reason R WITH(NOLOCK) ON IAD.IESelectCodeType = R.ID AND R.ReasonTypeID = 'IE_SELECT_CODE'
            WHERE IAD.IETMSUkey = {this.strIetmsUkey} and IAD.CodeFrom = '{this.strCodeFrom}'";

            dualResult = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            this.grid.DataSource = dt;
        }

        private void GetBottom()
        {
            string sqlcmd = $@"Select * From IETMS_AT_Detail Where IETMSUkey = {this.strIetmsUkey} and CodeFrom = '{this.strCodeFrom}'";
            DualResult dualResult = DBProxy.Current.Select(null, sqlcmd, out DataTable dtATDetail);
            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            decimal sum_00012_00013_00014 = 0.0m;
            decimal sum_00011_00015 = 0.0m;
            decimal decMM2AT = 0.0m;
            decimal decAT = 0.0m;
            decimal decTmpMM2AT = 0.0m;
            decimal decTmpAT = 0.0m;
            decimal sum_00015 = 0.0m;
            decimal sum_RPM_Speed = 0.0m;
            int iSuggested_Manpower = 0;
            int iSuggested_Machine = 0;
            decimal speedValue = 0m;
            decimal rpmValue = 0m;

            // 資料來源
            decimal seamer = MyUtility.Convert.GetDecimal(this.txtPieceOfSeamer.Text);
            decimal garment = MyUtility.Convert.GetDecimal(this.txtPieceOfGarment.Text);
            decimal lengthSewing = MyUtility.Convert.GetDecimal(this.txtLenghtSweing.Text);
            decimal lineSewing = MyUtility.Convert.GetDecimal(this.txtLineSewing.Text);
            decimal lengthLaser = MyUtility.Convert.GetDecimal(this.txtLenghtLaser.Text);
            decimal lineLaser = MyUtility.Convert.GetDecimal(this.txtLineLaser.Text);
            if (this.cbRPM.SelectedValue2 != null)
            {
                rpmValue = this.dtRPM.AsEnumerable().Where(r => r["ID"].ToString() == this.cbRPM.SelectedValue2.ToString()).Select(r => r.Field<decimal?>("Value") ?? 0m).FirstOrDefault();
            }

            if (this.cbSpeedLaser.SelectedValue2 != null)
            {
                speedValue = this.dtSpeed.AsEnumerable().Where(r => r["ID"].ToString() == this.cbSpeedLaser.SelectedValue2.ToString()).Select(r => r.Field<decimal?>("Value") ?? 0m).FirstOrDefault();
            }

            foreach (DataRow dr in dtATDetail.Rows)
            {
                if (dr.Field<string>("IESelectCodeType").IsOneOfThe("00012", "00013", "00014"))
                {
                    sum_00012_00013_00014 += dr.Field<int>("Number") * dr.Field<decimal>("Value") * garment;
                }
                else if (dr.Field<string>("IESelectCodeType").IsOneOfThe("00011", "00015"))
                {
                    sum_00011_00015 += dr.Field<int>("Number") * dr.Field<decimal>("Value");
                    if (dr.Field<string>("IESelectCodeType") == "00015")
                    {
                        sum_00015 += dr.Field<int>("Number") * dr.Field<decimal>("Value");
                    }
                }
            }

            // RPM 區域
            var decRPM = lengthSewing / 120m * rpmValue * garment;
            var rpmLinesValue = (lineSewing * garment * 1.5m) + (((lineSewing * garment) - 1m) * (decRPM / (lineSewing * garment) / 2.5m));
            sum_RPM_Speed += decRPM + rpmLinesValue;

            // Speed 區域
            if (!VFP.Empty(this.cbSpeedLaser.SelectedValue2))
            {
                var speedLengthValue = lengthLaser / 120m * speedValue * garment;
                decimal speedLinesValue = 0;
                decimal lineLaserGarmentProduct = lineLaser * garment;
                decimal divisor = this.cbSpeedLaser.SelectedValue2.ToString().Contains("40") ? 2m : 1.39m;

                // 檢查分母是否為零
                if (lineLaserGarmentProduct != 0 && divisor != 0)
                {
                    speedLinesValue = (lineLaserGarmentProduct - 1m) * (speedLengthValue / lineLaserGarmentProduct / divisor);
                }

                sum_RPM_Speed += speedLengthValue + speedLinesValue;
            }

            decMM2AT = seamer == 0 ? 0 : sum_00012_00013_00014 + (sum_00011_00015 / seamer * garment);
            decAT = seamer == 0 ? 0 : sum_RPM_Speed + (sum_00015 / seamer * garment);
            decTmpMM2AT = seamer == 0 ? 0 : (sum_00012_00013_00014 * seamer) + sum_00011_00015;
            decTmpAT = seamer == 0 ? 0 : (sum_RPM_Speed * seamer) + sum_00015;

            if (decMM2AT > decAT)
            {
                decimal ratio = decMM2AT / decAT;
                int roundedValue = (int)Math.Round(ratio);
                if (ratio > 2)
                {
                    iSuggested_Manpower = 2;
                }
                else
                {
                    iSuggested_Manpower = roundedValue;
                }
            }
            else
            {
                iSuggested_Manpower = 1;
            }

            iSuggested_Machine = decTmpAT > decTmpMM2AT ? (int)Math.Round(decTmpAT / decTmpMM2AT) : 1;

            this.txtAT_Garment.Text = Math.Round(decAT, 3).ToString();
            this.txtMM2AT_Gaement.Text = Math.Round(decMM2AT, 3).ToString();
            this.txtAT_Templeate.Text = Math.Round(decTmpAT, 3).ToString();
            this.txtMM2AT_template.Text = Math.Round(decTmpMM2AT, 3).ToString();
            this.txtManpower.Text = iSuggested_Manpower.ToString();
            this.txtMachine.Text = iSuggested_Machine.ToString();
            this.txtTemplate.Text = (iSuggested_Manpower + 1).ToString();
        }

        private void BtnStdAT_Click(object sender, EventArgs e)
        {
            var frm = new P01_Operation_AT(new string[] { "AT" }, ref this.dtDetail,ref this.p01List, this.strIetmsUkey, this.strCodeFrom, false, false);
            frm.StartPosition = FormStartPosition.Manual;
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is P01_Operation_AT)
                {
                    int newX = openForm.Left + 25;
                    int newY = openForm.Top + 25;
                    frm.Location = new Point(newX, newY);
                }
            }

            frm.ShowDialog();
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            this.GetBottom();
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            string sqlcmd = $@"
            UPDATE IETMS_AT SET
            PieceOfSeamerEdited = {this.txtPieceOfSeamer.Text}
            , RPMEdited  = '{this.cbRPM.SelectedValue2}' , LaserSpeedEdited = '{this.cbSpeedLaser.SelectedValue2}' WHERE IETMSUkey =  '{this.strIetmsUkey}' AND CodeFrom  = '{this.strCodeFrom}'

            UPDATE TimeStudy_Detail SET SMV = '{this.txtMM2AT_Gaement.Text}'
            from TimeStudy_Detail td WITH (NOLOCK) 
            INNER JOIN TimeStudy t WITH(NOLOCK) ON td.id = t.id
            INNER JOIN IETMS i ON t.IETMSID = i.ID AND t.IETMSVersion = i.[Version]
            INNER JOIN IETMS_Detail ID ON I.Ukey = ID.IETMSUkey AND ID.SEQ = TD.Seq
            WHERE  
            Id.IETMSUkey = {this.strIetmsUkey} and 
            ID.CodeFrom = '{this.strCodeFrom}' and 
            td.ID = '{this.strTimeStudyID}' and 
            td.[MachineTypeID] like 'MM2AT%'

            UPDATE TimeStudy_Detail SET SMV = '{this.txtAT_Garment.Text}'
            from TimeStudy_Detail td WITH (NOLOCK) 
            INNER JOIN TimeStudy t WITH(NOLOCK) ON td.id = t.id
            INNER JOIN IETMS i ON t.IETMSID = i.ID AND t.IETMSVersion = i.[Version]
            INNER JOIN IETMS_Detail ID ON I.Ukey = ID.IETMSUkey AND ID.SEQ = TD.Seq
            WHERE  
            Id.IETMSUkey = {this.strIetmsUkey} and 
            ID.CodeFrom = '{this.strCodeFrom}' and 
            td.ID = '{this.strTimeStudyID}' and 
            td.[MachineTypeID] like 'AT%'
            ";
            DBProxy.Current.Execute(null, sqlcmd);
            this.GetBottom();

            var drMM2AT = this.dtDetail.AsEnumerable().Where(
                x => x.Field<long>("IETMSUkey") == Convert.ToInt64(this.strIetmsUkey) &&
                x.Field<string>("CodeFrom") == this.strCodeFrom &&
                x.Field<string>("MachineTypeID").StartsWith("MM2AT")
                ).FirstOrDefault();

            drMM2AT["SMV"] = MyUtility.Convert.GetDecimal(this.txtMM2AT_Gaement.Text);

            var drAT = this.dtDetail.AsEnumerable().Where(
                x => x.Field<long>("IETMSUkey") == Convert.ToInt64(this.strIetmsUkey) &&
                x.Field<string>("CodeFrom") == this.strCodeFrom &&
                x.Field<string>("MachineTypeID").StartsWith("AT")
                ).FirstOrDefault();
            drAT["SMV"] = MyUtility.Convert.GetDecimal(this.txtAT_Garment.Text);
        }
    }
}