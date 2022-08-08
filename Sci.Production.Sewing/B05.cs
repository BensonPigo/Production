using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// B05
    /// </summary>
    public partial class B05 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B05
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            bool canNew = Prgs.GetAuthority(Sci.Env.User.UserID, "B05. CMP Efficiency Setting By Factory", "CanNew");
            this.btnBatchImport.Enabled = canNew;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            if (!MyUtility.Check.Empty(this.CurrentMaintain["Ukey"]) &&
                MyUtility.Check.Seek($"select 1 from SewingOutputEfficiency_History with (nolock) where SewingOutputEfficiencyUkey = '{this.CurrentMaintain["Ukey"]}'"))
            {
                this.btnHistory.ForeColor = Color.Blue;
            }
            else
            {
                this.btnHistory.ForeColor = Color.Black;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {

            if (MyUtility.Check.Empty(this.CurrentMaintain["StyleID"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["SeasonID"]))
            {
                MyUtility.Msg.WarningBox("<Brand>,<Style>,<Season> can not be empty");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["StyleUkey"]))
            {
                DataRow drStyleUkey;
                string sqlGetStyleUkey = $@"
select Ukey from Style with (nolock) where ID = '{this.CurrentMaintain["StyleID"]}' and BrandID = '{this.CurrentMaintain["BrandID"]}' and SeasonID = '{this.CurrentMaintain["SeasonID"]}'
";
                bool styleExists = MyUtility.Check.Seek(sqlGetStyleUkey, out drStyleUkey);

                if (!styleExists)
                {
                    MyUtility.Msg.WarningBox("Style not exists");
                    return false;
                }

                this.CurrentMaintain["StyleUkey"] = drStyleUkey["Ukey"];
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            DataRow oriCurrentMaintain;

            bool isExists = MyUtility.Check.Seek($"select Junk, Efficiency from SewingOutputEfficiency where Ukey = '{this.CurrentMaintain["Ukey"]}'", out oriCurrentMaintain);

            string insertSewingOutputEfficiency_History = string.Empty;
            if (isExists)
            {
                if (MyUtility.Convert.GetBool(oriCurrentMaintain["Junk"]) != MyUtility.Convert.GetBool(this.CurrentMaintain["Junk"]))
                {
                    string oldValueJunk = MyUtility.Convert.GetBool(this.CurrentMaintain["Junk"]) ? string.Empty : "Junk";
                    string newValueJunk = MyUtility.Convert.GetBool(this.CurrentMaintain["Junk"]) ? "Junk" : string.Empty;

                    insertSewingOutputEfficiency_History += $@"
insert into SewingOutputEfficiency_History(SewingOutputEfficiencyUkey, Action, OldValue, NewValue, AddName, AddDate)
        values('{this.CurrentMaintain["Ukey"]}', 'Junk', '{oldValueJunk}', '{newValueJunk}', '{Env.User.UserID}', getdate())
";
                }

                if (MyUtility.Convert.GetDecimal(oriCurrentMaintain["Efficiency"]) != MyUtility.Convert.GetDecimal(this.CurrentMaintain["Efficiency"]))
                {
                    insertSewingOutputEfficiency_History += $@"
insert into SewingOutputEfficiency_History(SewingOutputEfficiencyUkey, Action, OldValue, NewValue, AddName, AddDate)
        values('{this.CurrentMaintain["Ukey"]}', 'Set Eff(%)', '{oriCurrentMaintain["Efficiency"]}', '{this.CurrentMaintain["Efficiency"]}', '{Env.User.UserID}', getdate())
";
                }
            }

            if (!MyUtility.Check.Empty(insertSewingOutputEfficiency_History))
            {
                DualResult result = DBProxy.Current.Execute(null, insertSewingOutputEfficiency_History);
                if (!result)
                {
                    return result;
                }
            }

            return base.ClickSave();
        }

        private void BtnHistory_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain["Ukey"]))
            {
                new B05_History((long)this.CurrentMaintain["Ukey"]).ShowDialog();
            }
        }

        private void BtnBatchImport_Click(object sender, EventArgs e)
        {
            new B05_BatchImport().ShowDialog();
            this.ReloadDatas();
        }
    }
}
