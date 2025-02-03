using Ict;
using Sci.Data;
using Sci.Production.CallPmsAPI;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// P05_CreateNewLineMapping
    /// </summary>
    public partial class P05_CreateNewLineMapping : Sci.Win.Tems.QueryForm
    {
        private DataRow mainRow;
        private DataTable dtDetail;
        private DataTable dtAutomatedLineMapping_DetailTemp;
        private DataTable dtAutomatedLineMapping_DetailAuto;
        private P05 p05;

        /// <summary>
        /// P05_CreateNewLineMapping
        /// </summary>
        /// <param name="p05">p05</param>
        /// <param name="mainRow">mainRow</param>
        /// <param name="dtDetail">dtDetail</param>
        /// <param name="dtAutomatedLineMapping_DetailTemp">dtAutomatedLineMapping_DetailTemp</param>
        /// <param name="dtAutomatedLineMapping_DetailAuto">dtAutomatedLineMapping_DetailAuto</param>
        public P05_CreateNewLineMapping(P05 p05,  DataRow mainRow, DataTable dtDetail, DataTable dtAutomatedLineMapping_DetailTemp, DataTable dtAutomatedLineMapping_DetailAuto)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.p05 = p05;
            this.mainRow = mainRow;
            this.dtDetail = dtDetail;
            this.dtAutomatedLineMapping_DetailTemp = dtAutomatedLineMapping_DetailTemp;
            this.dtAutomatedLineMapping_DetailAuto = dtAutomatedLineMapping_DetailAuto;

            MyUtility.Tool.SetupCombox(this.comboPhase, 2, 1, ",,Initial,Initial,Prelim,Prelim");

            this.txtStyleCreate.TarBrand = this.txtBrandCreate;
            this.txtStyleCreate.BrandObjectName = this.txtBrandCreate;

            this.txtStyleCreate.TarSeason = this.txtSeasonCreate;
            this.txtStyleCreate.SeasonObjectName = this.txtSeasonCreate;

            this.txtStyleCopy.TarBrand = this.txtBrandCopy;
            this.txtStyleCopy.BrandObjectName = this.txtBrandCopy;

            this.txtStyleCopy.TarSeason = this.txtSeasonCopy;
            this.txtStyleCopy.SeasonObjectName = this.txtSeasonCopy;

//#if DEBUG
//            this.txtFactoryCreate.Text = "GMM";
//            this.txtBrandCreate.Text = "U.ARMOUR";
//            this.txtSeasonCreate.Text = "23SS";
//            this.txtStyleLocationCreate.Text = "B";
//            this.txtStyleCreate.Text = "1375845";
//            this.numSewer.Text = "26";
//            this.numHours.Text = "8";
//#endif
        }

        private void TxtStyleLocationCreate_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.PopuStyleLocation(this.txtStyleCreate.Text, this.txtBrandCreate.Text, this.txtSeasonCreate.Text, this.txtStyleLocationCreate);
        }

        private void TxtStyleLocationCopy_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.PopuStyleLocation(this.txtStyleCopy.Text, this.txtBrandCopy.Text, this.txtSeasonCopy.Text, this.txtStyleLocationCopy);
        }

        private void PopuStyleLocation(string styleID, string brand, string season, Win.UI.TextBox tarLocation)
        {
            string sqlGetStyleUkey = $"select Ukey from Style with (nolock) where ID = '{styleID}'";

            if (!MyUtility.Check.Empty(brand))
            {
                sqlGetStyleUkey += $" and BrandID = '{brand}' ";
            }

            if (!MyUtility.Check.Empty(season))
            {
                sqlGetStyleUkey += $" and SeasonID = '{season}' ";
            }

            string styleUkey = MyUtility.GetValue.Lookup(sqlGetStyleUkey);

            if (MyUtility.Check.Empty(styleUkey))
            {
                styleUkey = "null";
            }

            SelectItem selectItem = new SelectItem($"select Location from Style_Location WITH (NOLOCK) where StyleUkey = {styleUkey}", null, null);
            DialogResult dialogResult = selectItem.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            tarLocation.Text = selectItem.GetSelectedString();
        }

        private void BtnCreateAutoLineMapping_Click(object sender, EventArgs e)
        {
            if (MyUtility.Convert.GetDecimal(this.numHours.Text) == 0 ||
                MyUtility.Convert.GetDecimal(this.numSewer.Text) == 0)
            {
                MyUtility.Msg.WarningBox("[No. of Sewer] and [No. of Hours] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.txtFactoryCreate.Text))
            {
                MyUtility.Msg.WarningBox("[*Create Auto Line Mapping][Factory] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.txtStyleCreate.Text))
            {
                MyUtility.Msg.WarningBox("[*Create Auto Line Mapping][Style] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.txtStyleLocationCreate.Text))
            {
                MyUtility.Msg.WarningBox("[*Create Auto Line Mapping][Combo Type] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.txtSeasonCreate.Text))
            {
                MyUtility.Msg.WarningBox("[*Create Auto Line Mapping][Season] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.txtBrandCreate.Text))
            {
                MyUtility.Msg.WarningBox("[*Create Auto Line Mapping][Brand] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.comboPhase.Text))
            {
                MyUtility.Msg.WarningBox("[*Create Auto Line Mapping][Phase] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.numSewer.Text))
            {
                MyUtility.Msg.WarningBox("[*Create Auto Line Mapping][No. of Sewer] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.numHours.Text))
            {
                MyUtility.Msg.WarningBox("[*Create Auto Line Mapping][No. of Hours] cannot be empty.");
                return;
            }

            string checkGSD = $@"
select  Status
from TimeStudy ts with (nolock)
where   ts.StyleID = '{this.txtStyleCreate.Text}' and
        ts.BrandID = '{this.txtBrandCreate.Text}' and
        ts.SeasonID = '{this.txtSeasonCreate.Text}' and
        ts.ComboType = '{this.txtStyleLocationCreate.Text}' and
        ts.Phase = '{this.comboPhase.Text}'
";
            DataRow drInfoGSD;

            if (!MyUtility.Check.Seek(checkGSD, out drInfoGSD))
            {
                MyUtility.Msg.WarningBox("[*Create Auto Line Mapping]Factory GSD data not found.");
                return;
            }

            if (drInfoGSD["Status"].ToString() != "Confirmed")
            {
                MyUtility.Msg.WarningBox("Factory GSD need to confirm first before create auto line mapping.");
                return;
            }

            DataTable[] dtResults;
            string sqlGetAutomatedLineMapping = $@"
exec dbo.GetAutomatedLineMapping    '{this.txtFactoryCreate.Text}',
                                    '{this.txtStyleCreate.Text}',
                                    '{this.txtSeasonCreate.Text}',
                                    '{this.txtBrandCreate.Text}',
                                    '{this.txtStyleLocationCreate.Text}',
                                    {this.numSewer.Text},
                                    {this.numHours.Text},
                                    '{Env.User.UserID}',
                                    '{this.comboPhase.Text}',
                                    2";
            DualResult result = DBProxy.Current.Select(null, sqlGetAutomatedLineMapping, out dtResults);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            bool isAlreadyCreate = MyUtility.Check.Seek($@"
             SELECT 1
             FROM AutomatedLineMapping ALM
             WHERE   ALM.FactoryID = '{this.txtFactoryCreate.Text}'
             AND ALM.StyleID = '{this.txtStyleCreate.Text}'
             AND ALM.SeasonID = '{this.txtSeasonCreate.Text}'
             AND ALM.BrandID = '{this.txtBrandCreate.Text}'
             AND ALM.ComboType = '{this.txtStyleLocationCreate.Text}'
             AND ALM.Phase = '{this.comboPhase.Text}'
             AND ALM.SewerManpower = '{this.numSewer.Value}'
             AND ALM.OriSewerManpower = '{this.numSewer.Text}'
             AND (ALM.Phase + ' ' +ALM.TimeStudyVersion) = '{MyUtility.Convert.GetString(dtResults[0].Rows[0]["Phase"]) + " " + MyUtility.Convert.GetString(dtResults[0].Rows[0]["TimeStudyVersion"])}'
             ");
            if (isAlreadyCreate)
            {
                MyUtility.Msg.WarningBox($"This [*Create Auto Line Mapping][No. of Sewer] is {this.numSewer.Text} already exists, cannot create a new line mapping.");
                return;
            }

            result = this.MergeMainDataTable(dtResults[0], dtResults[1], dtResults[2], dtResults[3]);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Create Auto Line Mapping success");
            this.Close();
        }

        private void BtnCopyOtherLineMapping_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtFactoryCreate.Text))
            {
                MyUtility.Msg.WarningBox("[*Create Auto Line Mapping][Factory] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.txtStyleCreate.Text))
            {
                MyUtility.Msg.WarningBox("[*Create Auto Line Mapping][Style] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.txtStyleLocationCreate.Text))
            {
                MyUtility.Msg.WarningBox("[*Create Auto Line Mapping][Combo Type] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.txtSeasonCreate.Text))
            {
                MyUtility.Msg.WarningBox("[*Create Auto Line Mapping][Season] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.txtBrandCreate.Text))
            {
                MyUtility.Msg.WarningBox("[*Create Auto Line Mapping][Brand] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.comboPhase.Text))
            {
                MyUtility.Msg.WarningBox("[*Create Auto Line Mapping][Phase] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.numHours.Text))
            {
                MyUtility.Msg.WarningBox("[*Create Auto Line Mapping][No. of Hours] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.txtFactoryCopy.Text))
            {
                MyUtility.Msg.WarningBox("[*Copy Other Line Mapping][Factory] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.txtStyleCopy.Text))
            {
                MyUtility.Msg.WarningBox("[*Copy Other Line Mapping][Style] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.txtStyleLocationCopy.Text))
            {
                MyUtility.Msg.WarningBox("[*Copy Other Line Mapping][Combo Type] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.txtSeasonCopy.Text))
            {
                MyUtility.Msg.WarningBox("[*Copy Other Line Mapping][Season] cannot be empty.");
                return;
            }

            if (MyUtility.Check.Empty(this.numVersion.Text))
            {
                MyUtility.Msg.WarningBox("[*Copy Other Line Mapping][Version] cannot be empty.");
                return;
            }

            string checkAutomatedLineMapping = $@"
select ID, SewerManpower
from AutomatedLineMapping with (nolock)
where   FactoryID = '{this.txtFactoryCopy.Text}' and
        StyleID = '{this.txtStyleCopy.Text}' and
        SeasonID = '{this.txtSeasonCopy.Text}' and
        BrandID = '{this.txtBrandCopy.Text}' and
        ComboType = '{this.txtStyleLocationCopy.Text}' and
        Version = '{this.numVersion.Text}' and
        Status = 'Confirmed'
";
            DataRow drAutomatedLineMappingCopySource;

            if (!MyUtility.Check.Seek(checkAutomatedLineMapping, out drAutomatedLineMappingCopySource))
            {
                MyUtility.Msg.WarningBox("Unable to find Line Mapping in IE_P05 according to [*Copy Other Line Mapping] information.");
                return;
            }

            if (MyUtility.Convert.GetInt(drAutomatedLineMappingCopySource["SewerManpower"]) != MyUtility.Convert.GetInt(this.numSewer.Value))
            {
                MyUtility.Msg.WarningBox($"[*Create Auto Line Mapping][No. of Sewer] is {this.numSewer.Value}, but version [No. of Sewer] is {drAutomatedLineMappingCopySource["SewerManpower"]}, Cannot be created because Sewer is different.");
                return;
            }

            string automatedLineMappingID = drAutomatedLineMappingCopySource["ID"].ToString();

            #region 比對copy來源與copy目標的timestudy_detail operation資料是否一致

            string checkGSD = $@"
select  ts.Status,
        ts.ID,
        [StyleUkey] = s.Ukey,
        [StyleCPU] = s.CPU,
        [TimeStudyID] = ts.ID,
        [TimeStudyVersion] = ts.Version
from TimeStudy ts with (nolock)
inner join Style s with (nolock) on s.ID = ts.StyleID and s.BrandID = ts.BrandID and s.SeasonID = ts.SeasonID
where   ts.StyleID = '{this.txtStyleCreate.Text}' and
        ts.BrandID = '{this.txtBrandCreate.Text}' and
        ts.SeasonID = '{this.txtSeasonCreate.Text}' and
        ts.ComboType = '{this.txtStyleLocationCreate.Text}' and
        ts.Phase = '{this.comboPhase.Text}'
";
            DataRow drInfoGSD;

            if (!MyUtility.Check.Seek(checkGSD, out drInfoGSD))
            {
                MyUtility.Msg.WarningBox("[*Create Auto Line Mapping]Factory GSD data not found.");
                return;
            }

            if (drInfoGSD["Status"].ToString() != "Confirmed")
            {
                MyUtility.Msg.WarningBox("Factory GSD need to confirm first before create auto line mapping.");
                return;
            }

            string sqlCheckTimestudy_DetailDiff = $@"
--抓出要Create的TimeStudy_Detail
select  [RowNum] = ROW_NUMBER() OVER (ORDER BY Seq),
        Seq,
        OperationID
into #tmpCreateTimeStudy_Detail
from TimeStudy_Detail td with (nolock)
where   td.ID = '{drInfoGSD["ID"]}' and
        td.PPA != 'C' and
        td.IsNonSewingline = 0 and
        td.OperationID not like '-%'
order by Seq

--抓出Copy來源的TimeStudy_Detail
select  [RowNum] = ROW_NUMBER() OVER (ORDER BY td.Seq),
        td.Seq,
        td.OperationID
into    #tmpCopyTimeStudy_Detail
from TimeStudy t with (nolock)
inner join TimeStudy_Detail td with (nolock) on t.ID = td.ID
where   exists( select 1 from AutomatedLineMapping am   with (nolock)
                where   am.ID = '{automatedLineMappingID}'  and
                        am.TimeStudyID = t.ID and
                        am.TimeStudyVersion = t.Version
                ) and
        td.PPA != 'C' and
        td.IsNonSewingline = 0 and
        td.OperationID not like '-%'
order by Seq
--沒資料去history找
if not exists (select 1 from #tmpCopyTimeStudy_Detail)
begin
    insert into #tmpCopyTimeStudy_Detail(RowNum, Seq, OperationID)
    select  [RowNum] = ROW_NUMBER() OVER (ORDER BY td.Seq),
            td.Seq,
            td.OperationID
    from TimeStudyHistory t with (nolock)
    inner join TimeStudyHistory_Detail td with (nolock) on t.ID = td.ID
    where   exists( select 1 from AutomatedLineMapping am   with (nolock)
                    where   am.ID = '{automatedLineMappingID}'  and
                            am.StyleID = t.StyleID and
                            am.SeasonID = t.SeasonID and
                            am.ComboType = t.ComboType and
                            am.BrandID = t.BrandID and
                            am.TimeStudyVersion = t.Version and
                            am.Phase = t.Phase
                ) and
            td.PPA != 'C' and
            td.IsNonSewingline = 0 and
            td.OperationID not like '-%'
end


--比對兩邊是否一致
Declare @IsSame bit = 1

if (select count(*) from #tmpCreateTimeStudy_Detail) <> (select count(*) from #tmpCopyTimeStudy_Detail)
    set @IsSame = 0

if exists(  select  1
            from #tmpCreateTimeStudy_Detail a
            inner join #tmpCopyTimeStudy_Detail b on a.RowNum = b.RowNum and  a.OperationID <> b.OperationID)
    set @IsSame = 0


select [IsSame] = @IsSame

drop table #tmpCreateTimeStudy_Detail, #tmpCopyTimeStudy_Detail
";
            DataRow drCheckTimestudy_DetailDiff;
            MyUtility.Check.Seek(sqlCheckTimestudy_DetailDiff, out drCheckTimestudy_DetailDiff);

            if (!MyUtility.Convert.GetBool(drCheckTimestudy_DetailDiff["IsSame"]))
            {
                MyUtility.Msg.WarningBox($"{this.txtStyleCreate.Text} cannot be copied, because the order of Fty GSD operation and seq is inconsistent.");
                return;
            }
            #endregion

            string sqlGetAutomatedLineMapping = $@"
select * from AutomatedLineMapping with (nolock) where ID = '{automatedLineMappingID}'
{string.Format(this.p05.sqlGetAutomatedLineMapping_Detail, $" ad.ID = '{automatedLineMappingID}'")}
{string.Format(this.p05.sqlGetAutomatedLineMapping_DetailTemp, $" ad.ID = '{automatedLineMappingID}'")}
{string.Format(this.p05.sqlGetAutomatedLineMapping_DetailAuto, $" ad.ID = '{automatedLineMappingID}'")}
";
            DataTable[] dtResults;
            DualResult result = DBProxy.Current.Select(null, sqlGetAutomatedLineMapping, out dtResults);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            dtResults[0].Rows[0]["Phase"] = this.comboPhase.Text;
            dtResults[0].Rows[0]["FactoryID"] = this.txtFactoryCreate.Text;
            dtResults[0].Rows[0]["StyleID"] = this.txtStyleCreate.Text;
            dtResults[0].Rows[0]["SeasonID"] = this.txtSeasonCreate.Text;
            dtResults[0].Rows[0]["BrandID"] = this.txtBrandCreate.Text;
            dtResults[0].Rows[0]["ComboType"] = this.txtStyleLocationCreate.Text;
            dtResults[0].Rows[0]["StyleUKey"] = drInfoGSD["StyleUKey"];
            dtResults[0].Rows[0]["StyleCPU"] = drInfoGSD["StyleCPU"];
            dtResults[0].Rows[0]["TimeStudyID"] = drInfoGSD["TimeStudyID"];
            dtResults[0].Rows[0]["TimeStudyVersion"] = drInfoGSD["TimeStudyVersion"];

            result = this.MergeMainDataTable(dtResults[0], dtResults[1], dtResults[2], dtResults[3]);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Copy Other Line Mapping success");
            this.Close();
        }

        private DualResult MergeMainDataTable(
            DataTable dtAutomatedLineMapping,
            DataTable dtAutomatedLineMapping_Detail,
            DataTable dtAutomatedLineMapping_DetailTempNew,
            DataTable dtAutomatedLineMapping_DetailAutoNew)
        {
            try
            {
                foreach (DataColumn col in this.mainRow.Table.Columns)
                {
                    if (dtAutomatedLineMapping.Columns.IndexOf(col.ColumnName) < 0)
                    {
                        continue;
                    }

                    this.mainRow[col.ColumnName] = dtAutomatedLineMapping.Rows[0][col.ColumnName];
                }

                this.mainRow["Status"] = "New";
                this.mainRow["ID"] = DBNull.Value;
                this.mainRow["Version"] = DBNull.Value;

                this.dtDetail.Clear();
                this.dtDetail.MergeBySyncColType(dtAutomatedLineMapping_Detail);

                foreach (DataRow dr in this.dtDetail.Rows)
                {
                    dr["Selected"] = false;
                }

                this.dtAutomatedLineMapping_DetailTemp.Clear();
                this.dtAutomatedLineMapping_DetailTemp.MergeBySyncColType(dtAutomatedLineMapping_DetailTempNew);

                this.dtAutomatedLineMapping_DetailAuto.Clear();
                this.dtAutomatedLineMapping_DetailAuto.MergeBySyncColType(dtAutomatedLineMapping_DetailAutoNew);
            }
            catch (Exception ex)
            {
                return new DualResult(false, ex);
            }

            return new DualResult(true);
        }
    }
}
