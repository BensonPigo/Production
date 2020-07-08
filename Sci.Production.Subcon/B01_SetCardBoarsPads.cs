using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class B01_SetCardBoarsPads : Win.Forms.Base
    {
        private DataRow masterrow;

        public B01_SetCardBoarsPads(DataRow mdr)
        {
            this.InitializeComponent();
            this.EditMode = false;
            this.masterrow = mdr;
        }

        private DataTable dt;

        private void Query()
        {
            string sqlcmd = $@"select * from LocalItem_CartonCardboardPad where refno = '{this.masterrow["refno"]}'";
            DBProxy.Current.Select(null, sqlcmd, out this.dt);
            this.listControlBindingSource1.DataSource = this.dt;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Query();
            #region
            DataGridViewGeneratorTextColumnSettings buyer = new DataGridViewGeneratorTextColumnSettings();
            buyer.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                    string item_cmd = $@"select id from buyer where junk = 0 ";
                    SelectItem item = new SelectItem(item_cmd, "12", dr["buyer"].ToString());
                    item.Text = "Buyer";
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["buyer"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };
            buyer.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetString(e.FormattedValue).EqualString(string.Empty))
                {
                    dr["buyer"] = string.Empty;
                    dr.EndEdit();
                    return;
                }

                if (!MyUtility.Check.Seek($@"select id from buyer where junk = 0 and id = '{e.FormattedValue}'"))
                {
                    MyUtility.Msg.WarningBox($"Buyer: {e.FormattedValue} not found!");
                    dr["buyer"] = string.Empty;
                    dr.EndEdit();
                }
            };

            #endregion
            #region  PadRefno
            DataGridViewGeneratorTextColumnSettings PadRefno = new DataGridViewGeneratorTextColumnSettings();
            PadRefno.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                    string item_cmd = $@"select Refno,Description from LocalItem where  Category = 'CARTON' and junk = 0 and Refno<> '{this.masterrow["Refno"]}'";
                    SelectItem item = new SelectItem(item_cmd, "12", dr["Refno"].ToString());
                    item.Text = "Pad Refno";
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["PadRefno"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };
            PadRefno.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Seek($@"select Refno from LocalItem where  Category = 'CARTON' and junk = 0 and Refno = '{e.FormattedValue}' and Refno<> '{this.masterrow["Refno"]}'"))
                {
                    MyUtility.Msg.WarningBox($"PadRefno: {e.FormattedValue} not found!");
                    dr["PadRefno"] = DBNull.Value;
                    dr.EndEdit();
                }
            };
            #endregion

            #region -- Grid 設定 --
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("Buyer", header: "Buyer", width: Widths.AnsiChars(15), settings: buyer)
            .Text("PadRefno", header: "Pad Refno", width: Widths.AnsiChars(15), settings: PadRefno)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(10))
            .DateTime("AddDate", header: "Create Date", iseditingreadonly: true, iseditable: false)
            .Text("AddName", header: "Create Name", iseditingreadonly: true, iseditable: false)
            .DateTime("EditDate", header: "Edit Date", iseditingreadonly: true, iseditable: false)
            .Text("EditName", header: "Edit Name", iseditingreadonly: true, iseditable: false)
            ;
            #endregion
        }

        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (this.EditMode)
            {
                this.btnAppend.Visible = true;
                this.btnDelete.Visible = true;
                this.btnClose.Text = "UnDo";
                this.BtnEdit.Text = "Save";
                this.grid1.IsEditingReadOnly = false;
            }
            else
            {
                this.btnAppend.Visible = false;
                this.btnDelete.Visible = false;
                this.btnClose.Text = "Close";
                this.BtnEdit.Text = "Edit";
                this.grid1.IsEditingReadOnly = true;
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                foreach (DataRow item in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
                {
                    if (MyUtility.Check.Empty(item["PadRefno"]))
                    {
                        MyUtility.Msg.WarningBox("PadRefno can not empty!");
                        return;
                    }

                    if (MyUtility.Check.Empty(item["Qty"]))
                    {
                        MyUtility.Msg.WarningBox("Qty can not 0!");
                        return;
                    }

                    if (MyUtility.Check.Empty(item["Buyer"]))
                    {
                        item["Buyer"] = string.Empty;
                    }
                }

                var query = from t in this.dt.AsEnumerable()
                group t by new { t1 = t.Field<string>("Refno"), t2 = t.Field<string>("Buyer"), t3 = t.Field<string>("PadRefno") } into m
                select new
                {
                    Refno = m.Key.t1,
                    Buyer = m.Key.t2,
                    PadRefno = m.Key.t3,
                    ct = m.Count(),
                };

                string pkd = string.Empty;
                foreach (var item in query)
                {
                    if (item.ct > 1)
                    {
                        pkd += $@"Buyer:{item.Buyer} , PadRefno:{item.PadRefno}
";
                    }
                }

                if (!MyUtility.Check.Empty(pkd))
                {
                    MyUtility.Msg.WarningBox(@"Pkey duplicate!
" + pkd);
                    return;
                }

                string mergecmd = $@"
merge LocalItem_CartonCardboardPad t
using #tmp s
on t.Refno = s.Refno and t.Buyer = s.Buyer and t.PadRefno = s.PadRefno
when matched and t.qty <> s.qty then update set
	t.Qty = s.Qty,
	EditDate = getdate(),
	EditName = '{Sci.Env.User.UserID}'
when not matched by target then
    insert(Refno,Buyer,PadRefno,Qty,AddDate,AddName)
    values(s.Refno,s.Buyer,s.PadRefno,s.Qty,getdate(),'{Sci.Env.User.UserID}');

delete lc
from LocalItem_CartonCardboardPad lc
where refno = '{this.masterrow["refno"]}'
and not exists(select 1 from #tmp t where t.Refno = '{this.masterrow["refno"]}' and t.Buyer = lc.Buyer and t.PadRefno = lc.PadRefno)
";
                DataTable dt;
                DualResult result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.listControlBindingSource1.DataSource, string.Empty, mergecmd, out dt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                this.Query();
                this.EditMode = false;
            }
            else
            {
                this.EditMode = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.EditMode = false;
                this.Query();
            }
            else
            {
                this.Close();
            }
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            DataRow dr = ((DataTable)this.listControlBindingSource1.DataSource).NewRow();
            dr["Refno"] = this.masterrow["Refno"];
            dr["Qty"] = 0;
            ((DataTable)this.listControlBindingSource1.DataSource).Rows.Add(dr);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1.Position != -1)
            {
                ((DataTable)this.listControlBindingSource1.DataSource).Rows.RemoveAt(this.listControlBindingSource1.Position);
            }
        }
    }
}
