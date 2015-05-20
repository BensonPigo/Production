using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class B07_BatchAdd : Sci.Win.Subs.Base
    {
        protected DataRow motherData;
        public B07_BatchAdd(DataRow data)
        {
            InitializeComponent();
            this.motherData = data;
        }
    }
}
