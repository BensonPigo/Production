using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class B03_CanvassRecord : Sci.Win.Tems.Input1
    {
        protected DataRow motherData;
        public B03_CanvassRecord(bool canedit, DataRow data)
        {
            InitializeComponent();
            motherData = data;
            this.DefaultFilter = "ID = '" + motherData["ID"].ToString().Trim() + "'";
        }
    }
}
