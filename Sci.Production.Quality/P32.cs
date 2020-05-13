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
using System.Linq;

namespace Sci.Production.Quality
{
    public partial class P32 : Sci.Win.Tems.Input6
    {
        public string _Type = string.Empty;
        private List<string> _Articles = new List<string>();
        private List<string> _Articles_c = new List<string>();

        // 每一Article底下的Size數量
        private Dictionary<string, int> Size_per_Article = new Dictionary<string, int>();

        public P32(ToolStripMenuItem menuitem,string type)
            : base(menuitem)
        {
            InitializeComponent();
            this.Text = type == "1" ? "P32. CFA Master List" : "P321. CFA Master List(History)";
            this._Type = type;

            //this.DefaultWhere = this._Type == "1" ? $"(SELECT MDivisionID FROM Orders WHERE ID = Order_QtyShip.ID) = '{Sci.Env.User.Keyword}' AND (SELECT Finished FROM Orders WHERE ID = Order_QtyShip.ID) = 0 " : $"(SELECT MDivisionID FROM Orders WHERE ID = Order_QtyShip.ID) = '{Sci.Env.User.Keyword}' AND (SELECT Finished FROM Orders WHERE ID = Order_QtyShip.ID) = 1";

            if (type != "1")
            {
                this.IsSupportEdit = false;
            }

            this.comboTeam.SelectedIndex = 0;
        }


        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.disPO.Value = MyUtility.GetValue.Lookup($@"
SELECT  CustPoNo
FROM Orders 
WHERE ID = '{this.CurrentMaintain["OrderID"].ToString()}'
");

            this.dateBuyerDev.Value =MyUtility.Convert.GetDate( MyUtility.GetValue.Lookup($@"
SELECT BuyerDelivery
FROM Order_QtyShip 
WHERE ID = '{this.CurrentMaintain["OrderID"].ToString()}' AND Seq ='{this.CurrentMaintain["Seq"].ToString()}'
"));

            this.disDest.Value = MyUtility.GetValue.Lookup($@"
SELECT Dest
FROM Orders 
WHERE ID = '{this.CurrentMaintain["OrderID"].ToString()}'
");

        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();

            CurrentMaintain["Status"] ="New";
            CurrentMaintain["AuditDate"] = DateTime.Now;
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["CFA"] = Sci.Env.User.UserID;
        }


    }
}
