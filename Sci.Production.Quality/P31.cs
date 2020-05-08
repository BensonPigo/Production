using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using System.Data.SqlClient;
using Ict.Win.UI;

namespace Sci.Production.Quality
{
    public partial class P31 : Sci.Win.Tems.Input6
    {
        public string _Type = string.Empty;

        public P31(ToolStripMenuItem menuitem,string type)
            : base(menuitem)
        {
            InitializeComponent();
            this.Text = type == "1" ? "P31. CFA Master List" : "P311. CFA Master List(History)";
            this._Type = type;

            this.DefaultWhere = this._Type == "1" ? $"(SELECT MDivisionID FROM Orders WHERE ID = Order_QtyShip.ID) = '{Sci.Env.User.Keyword}' AND (SELECT Finished FROM Orders WHERE ID = Order_QtyShip.ID) = 0 " : $"(SELECT MDivisionID FROM Orders WHERE ID = Order_QtyShip.ID) = '{Sci.Env.User.Keyword}' AND (SELECT Finished FROM Orders WHERE ID = Order_QtyShip.ID) = 1";
        }


        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
        }

    }
}
