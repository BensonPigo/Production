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

        /// <summary>
        /// P05_CreateNewLineMapping
        /// </summary>
        /// <param name="mainRow">mainRow</param>
        /// <param name="dtDetail">dtDetail</param>
        public P05_CreateNewLineMapping(DataRow mainRow, DataTable dtDetail)
        {
            this.InitializeComponent();
            this.mainRow = mainRow;
            this.dtDetail = dtDetail;

            MyUtility.Tool.SetupCombox(this.comboPhase, 2, 1, ",,Initial,Initial,Prelim,Prelim");

            this.txtStyleCreate.TarBrand = this.txtBrandCreate;
            this.txtStyleCreate.BrandObjectName = this.txtBrandCreate;

            this.txtStyleCreate.TarSeason = this.txtSeasonCreate;
            this.txtStyleCreate.SeasonObjectName= this.txtSeasonCreate;

            this.txtStyleCopy.TarBrand = this.txtBrandCopy;
            this.txtStyleCopy.BrandObjectName = this.txtBrandCopy;

            this.txtStyleCopy.TarSeason = this.txtSeasonCopy;
            this.txtStyleCopy.SeasonObjectName = this.txtSeasonCopy;
        }

        private void TxtStyleLocationCreate_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.PopuStyleLocation(this.txtStyleCreate.Text, this.txtBrandCreate.Text, this.txtSeasonCreate.Text, this.txtStyleLocationCreate);
        }

        private void TxtStyleLocationCopy_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.PopuStyleLocation(this.txtStyleCreate.Text, this.txtBrandCreate.Text, this.txtSeasonCreate.Text, this.txtStyleLocationCreate);
        }

        private void PopuStyleLocation(string styleID, string brand, string season, Win.UI.TextBox tarLocation)
        {
            string styleUkey = MyUtility.GetValue.Lookup($"select Ukey from Style with (nolock) where ID = '{styleID}' and BrandID = '{brand}' and SeasonID = '{season}'");
            if (MyUtility.Check.Empty(styleUkey))
            {
                MyUtility.Msg.WarningBox("Style not exists");
                tarLocation.Text = string.Empty;
            }

            SelectItem selectItem = new SelectItem($"select Location from Style_Location WITH (NOLOCK) where StyleUkey = {styleUkey}", null, null);
            DialogResult dialogResult = selectItem.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            tarLocation.Text = selectItem.GetSelectedString();
        }

        private bool IsStyleLocationExists(string styleID, string brand, string season, string location)
        {
            return MyUtility.Check.Seek($@"
select 1
from Style_Location WITH (NOLOCK) sl
where   Location = '{location}' and
        exists( select 1 from Style s with (nolock)
                where   s.ID = '{styleID}' and
                        s.BrandID = '{brand}' and
                        s.SeasonID = '{season}' and
                        s.Ukey = sl.StyleUkey
            )
");
        }

        private void TxtStyleLocationCreate_Validating(object sender, CancelEventArgs e)
        {
            if (!this.IsStyleLocationExists(this.txtStyleCreate.Text, this.txtBrandCreate.Text, this.txtSeasonCreate.Text, this.txtStyleLocationCreate.Text))
            {
                this.txtStyleLocationCreate.Text = string.Empty;
                MyUtility.Msg.WarningBox("[*Create Auto Line Mapping]Combo Type not exists");
                return;
            }
        }

        private void TxtStyleLocationCopy_Validating(object sender, CancelEventArgs e)
        {
            if (!this.IsStyleLocationExists(this.txtStyleCopy.Text, this.txtBrandCopy.Text, this.txtSeasonCopy.Text, this.txtStyleLocationCopy.Text))
            {
                this.txtStyleLocationCopy.Text = string.Empty;
                MyUtility.Msg.WarningBox("[*Copy Other Line Mapping]Combo Type not exists");
                return;
            }
        }

        private void BtnCreateAutoLineMapping_Click(object sender, EventArgs e)
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
        ts.ComboType = '{this.txtStyleLocationCreate.Text}'
order by ts.Version desc
";
            DataRow drInfoGSD;

            if (!MyUtility.Check.Seek(checkGSD, out drInfoGSD))
            {
                MyUtility.Msg.WarningBox("Factory GSD data not found.");
                return;
            }

            if (drInfoGSD["Status"].ToString() != "Confirmed")
            {
                MyUtility.Msg.WarningBox("Factory GSD need to confirm first before create auto line mapping.");
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
AND ALM.OriSewerManpower = '{this.numSewer.Text}'
");
            if (isAlreadyCreate)
            {
                MyUtility.Msg.WarningBox($"This [*Create Auto Line Mapping][No. of Sewer] is {this.numSewer.Text} already exists, cannot create a new line mapping.");
                return;
            }
        }
    }
}
