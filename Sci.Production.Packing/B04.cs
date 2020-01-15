using Ict;
using Sci.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class B04 : Sci.Win.Tems.Input1
    {
        /// <inheritdoc/>
        public B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@Size", this.CurrentMaintain["Size"].ToString()));

            bool isSizeExists = MyUtility.Check.Seek($"SELECT 1 FROM StickerSize WHERE Size=@Size AND ID <> {this.CurrentMaintain["ID"]} ", paras);

            if (isSizeExists)
            {
                MyUtility.Msg.InfoBox("The Sticker Size exists already. Please check again.");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
