using System.Collections.Generic;
using System.Data;
using Ict.Win;
using Sci.Data;
using Ict;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B04_AccountNo
    /// </summary>
    public partial class B04_AccountNo : Win.Subs.Input4
    {
        private DataGridViewGeneratorMaskedTextColumnSettings accountNo = new DataGridViewGeneratorMaskedTextColumnSettings();

        /// <summary>
        /// B04_AccountNo
        /// </summary>
        /// <param name="canedit">canedit</param>
        /// <param name="keyvalue1">keyvalue1</param>
        /// <param name="keyvalue2">keyvalue2</param>
        /// <param name="keyvalue3">keyvalue3</param>
        public B04_AccountNo(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.displayCode.Text = this.KeyValue1;
            this.displayAbbreviation.Text = MyUtility.GetValue.Lookup("Abb", this.KeyValue1, "LocalSupp", "ID");
        }

        /// <summary>
        /// OnRequery
        /// </summary>
        /// <returns>DualResult</returns>
        protected override DualResult OnRequery()
        {
            // sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@id";
            sp1.Value = this.KeyValue1;
            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);

            string selectCommand = @"select @id as ID,a.ID as ArtworkTypeID, a.Seq, b.AccountID, b.AddName,b.AddDate, b.EditName, b.EditDate 
from (select ID,Seq from ArtworkType WITH (NOLOCK) where IsSubprocess = 1 or Classify = 'P' or SystemType = 'P') a 
left join (select ArtworkTypeID,AccountID,AddName,AddDate,EditName,EditDate from LocalSupp_AccountNo WITH (NOLOCK) where ID = @id) b on a.ID = b.ArtworkTypeID 
order by a.Seq";
            DualResult returnResult;
            DataTable artworkTable = new DataTable();
            returnResult = DBProxy.Current.Select(null, selectCommand, cmds, out artworkTable);
            if (!returnResult)
            {
                return returnResult;
            }

            this.SetGrid(artworkTable);
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            this.accountNo.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;

            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("ArtworkTypeID", header: "Type", width: Widths.AnsiChars(20), iseditable: false)
                .MaskedText("AccountID", "0000-0000", header: "Account No", width: Widths.AnsiChars(8), settings: this.accountNo);

            return true;
        }

        /// <summary>
        /// OnUIConvertToMaintain
        /// </summary>
        protected override void OnUIConvertToMaintain()
        {
            base.OnUIConvertToMaintain();
            this.append.Visible = false;
            this.revise.Visible = false;
            this.delete.Visible = false;
        }

        /// <inheritdoc/>
        protected override DualResult OnSave()
        {
            ITableSchema tableSchema;
            DualResult returnResult = DBProxy.Current.GetTableSchema(null, "LocalSupp_AccountNo", out tableSchema);
            IList<DataRow> gridData = this.Datas;

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
                        string selectCommand = string.Format("select ID from LocalSupp_AccountNo WITH (NOLOCK) where ID = '{0}' and ArtworkTypeID = '{1}'", currentRecord["ID"].ToString(), currentRecord["ArtworkTypeID"].ToString());
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

            return Ict.Result.True;
        }
    }
}
