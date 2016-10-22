using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using Ict;

namespace Sci.Production.Basic
{
    public partial class B04_AccountNo : Sci.Win.Subs.Input4
    {
        DataGridViewGeneratorMaskedTextColumnSettings accountNo = new DataGridViewGeneratorMaskedTextColumnSettings();
        public B04_AccountNo(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            this.displayBox1.Text = this.KeyValue1;
            this.displayBox2.Text = MyUtility.GetValue.Lookup("Abb", this.KeyValue1, "LocalSupp", "ID");
        }

        protected override DualResult OnRequery()
        {
            //sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@id";
            sp1.Value = this.KeyValue1;
            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);

            string selectCommand = @"select @id as ID,a.ID as ArtworkTypeID, a.Seq, b.AccountID, b.AddName,b.AddDate, b.EditName, b.EditDate 
from (select ID,Seq from ArtworkType where IsSubprocess = 1 or Classify = 'P' or SystemType = 'P') a 
left join (select ArtworkTypeID,AccountID,AddName,AddDate,EditName,EditDate from LocalSupp_AccountNo where ID = @id) b on a.ID = b.ArtworkTypeID 
order by a.Seq";
            Ict.DualResult returnResult;
            DataTable ArtworkTable = new DataTable();
            returnResult = DBProxy.Current.Select(null, selectCommand, cmds, out ArtworkTable);
            if (!returnResult)
            {
                return returnResult;
            }
            SetGrid(ArtworkTable);
            return Result.True;
        }

        protected override bool OnGridSetup()
        {
            accountNo.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;

            Helper.Controls.Grid.Generator(this.grid)
                .Text("ArtworkTypeID", header: "Type", width: Widths.AnsiChars(20),iseditable:false)
                .MaskedText("AccountID", "0000-0000", header: "Account No", width: Widths.AnsiChars(8), settings: accountNo);
            
            return true;
        }

        protected override void OnUIConvertToMaintain()
        {
            base.OnUIConvertToMaintain();
            append.Visible = false;
            revise.Visible = false;
            delete.Visible = false;
        }

        protected override DualResult OnSave()
        {
            ITableSchema tableSchema;
            DualResult returnResult = DBProxy.Current.GetTableSchema(null, "LocalSupp_AccountNo", out tableSchema);
            IList<DataRow> gridData = Datas;

            foreach (DataRow currentRecord in gridData)
            {
                if (currentRecord.RowState == DataRowState.Modified)
                {
                    if (MyUtility.Check.Empty(currentRecord["AccountID"]))
                    {
                        returnResult = DBProxy.Current.Delete(null, tableSchema, currentRecord);
                        if (!returnResult)
                        {
                            return returnResult;
                        }
                    }
                    else
                    {
                        string selectCommand = string.Format("select ID from LocalSupp_AccountNo where ID = '{0}' and ArtworkTypeID = '{1}'", currentRecord["ID"].ToString(), currentRecord["ArtworkTypeID"].ToString());
                        if (!MyUtility.Check.Seek(selectCommand, null))
                        {
                            returnResult = DBProxy.Current.Insert(null, tableSchema, currentRecord);
                            if (!returnResult)
                            {
                                return returnResult;
                            }
                        }
                        else
                        {
                            bool different;
                            returnResult = DBProxy.Current.UpdateByChanged(null, tableSchema, currentRecord, out different);
                            if (!returnResult)
                            {
                                return returnResult;
                            }
                        }
                    }
                }
            }
            return Result.True;
        }
    }
}
