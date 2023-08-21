using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P69_CreateSeq : Sci.Win.Subs.Base
    {
        private bool boolIsCreate;
        private bool boolImport = false;
        private DataRow drDetail;

        /// <inheritdoc/>
        public P69_CreateSeq(bool isCreate,DataRow dr = null)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.boolIsCreate = isCreate;
            this.drDetail = dr;
            MyUtility.Tool.SetupCombox(this.cbFabricType, 2, 1, "A,Accessory,F,Fabric ");
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Text = this.boolIsCreate ? "P69. Create Seq" : "P69. Seq(Modify)";
            this.btnCreate.Text = this.boolIsCreate ? "Create" : "Save";
            this.txtSP.ReadOnly = this.boolIsCreate ? false : true;
            this.txtSeq1.ReadOnly = this.boolIsCreate ? false : true;
            this.txtSeq2.ReadOnly = this.boolIsCreate ? false : true;
            this.txtRefno.ReadOnly = this.boolIsCreate ? false : true;
            this.cbFabricType.ReadOnly = this.boolIsCreate ? false : true;
            this.txtSP.Text = this.boolIsCreate ? string.Empty : MyUtility.Convert.GetString(this.drDetail["POID"]);
            this.txtSeq1.Text = this.boolIsCreate ? string.Empty : MyUtility.Convert.GetString(this.drDetail["Seq1"]);
            this.txtSeq2.Text = this.boolIsCreate ? string.Empty : MyUtility.Convert.GetString(this.drDetail["Seq2"]);
            this.txtRefno.Text = this.boolIsCreate ? string.Empty : MyUtility.Convert.GetString(this.drDetail["Refno"]);
            this.cbFabricType.SelectedValue = this.boolIsCreate ? "A" : MyUtility.Convert.GetString(this.drDetail["FabricType"]);
            this.txtWeaveType1.Text = this.boolIsCreate ? string.Empty : MyUtility.Convert.GetString(this.drDetail["WeaveType"]);
            this.txtMtlType.Text = this.boolIsCreate ? string.Empty : MyUtility.Convert.GetString(this.drDetail["MtlType"]);
            this.txtColor.Text = this.boolIsCreate ? string.Empty : MyUtility.Convert.GetString(this.drDetail["Color"]);
            this.txtUnit.Text = this.boolIsCreate ? string.Empty : MyUtility.Convert.GetString(this.drDetail["Unit"]);
            this.txtSize.Text = this.boolIsCreate ? string.Empty : MyUtility.Convert.GetString(this.drDetail["Size"]);
            this.EditDesc.Text = this.boolIsCreate ? string.Empty : MyUtility.Convert.GetString(this.drDetail["Description"]);
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            string sqlcmd = string.Empty;
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@POID", this.txtSP.Text));
            sqlParameters.Add(new SqlParameter("@Seq1", this.txtSeq1.Text));
            sqlParameters.Add(new SqlParameter("@Seq2", this.txtSeq2.Text));
            sqlParameters.Add(new SqlParameter("@Refno", this.txtRefno.Text));
            sqlParameters.Add(new SqlParameter("@FabricType", this.cbFabricType.SelectedValue));
            sqlParameters.Add(new SqlParameter("@WeaveType", this.txtWeaveType1.Text));
            sqlParameters.Add(new SqlParameter("@MtlType", this.txtMtlType.Text));
            sqlParameters.Add(new SqlParameter("@Color", this.txtColor.Text));
            sqlParameters.Add(new SqlParameter("@Unit", this.txtUnit.Text));
            sqlParameters.Add(new SqlParameter("@SizeCode", this.txtSize.Text));
            sqlParameters.Add(new SqlParameter("@Des", this.EditDesc.Text));

            if (this.boolIsCreate)
            {
                if (MyUtility.Check.Empty(this.txtSP.Text) ||
                    MyUtility.Check.Empty(this.txtSeq1.Text) ||
                    MyUtility.Check.Empty(this.txtSeq2.Text) ||
                    MyUtility.Check.Empty(this.txtRefno.Text) ||
                    MyUtility.Check.Empty(this.txtColor.Text))
                {
                    MyUtility.Msg.WarningBox("These column cannot be empty <SP#, Seq, Refno, Color>.");
                    return;
                }

                if (!MyUtility.Check.Seek($@"SELECT * FROM Orders o WITH(NOLOCK) WHERE o.LocalOrder = 1 AND o.ID = @POID", sqlParameters))
                {
                    MyUtility.Msg.WarningBox($"SP# :{this.txtSP.Text} cannot be found!");
                    return;
                }

                string sqlExists = $@"";
                if (MyUtility.Check.Seek($@"select 1 from LocalOrderMaterial where PoID=@POID and Seq1 =@Seq1 and Seq2 = @Seq2 ", sqlParameters))
                {
                    MyUtility.Msg.WarningBox($"SP# :{this.txtSP.Text},Seq: {this.txtSeq1.Text} - {this.txtSeq2.Text} already existed.");
                    return;
                }

                sqlcmd = $@"INSERT INTO LocalOrderMaterial(POID,Seq1,Seq2,Refno,FabricType,WeaveType,MtlType,Color,Unit,SizeCode,[Desc],AddName,AddDate) 
                           VALUES (@POID,@Seq1,@Seq2,@Refno,@FabricType,@WeaveType,@MtlType,@Color,@Unit,@SizeCode,@Des,'{Sci.Env.User.UserID}',GetDate())";
            }
            else
            {
                sqlcmd = $@"UPDATE LocalOrderMaterial 
                            SET 
                            WeaveType = @WeaveType ,
                            MtlType = @MtlType,
                            Color = @Color,
                            Unit = @Unit,
                            SizeCode = @SizeCode ,
                            [Desc] = @Des , 
                            EditName = '{Sci.Env.User.UserID}' ,
                            EditDate = GETDATE() 
                            WHERE 
                            POID = @POID AND
                            SEQ1 = @SEQ1 AND
                            SEQ2 = @SEQ2 AND
                            REFNO = @REFNO AND
                            FabricType = @FabricType
                            ";
            }

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd, sqlParameters)))
            {
                this.ShowErr(result);
                return;
            }

            this.boolImport = true;
            this.Close();
        }

        /// <inheritdoc/>
        public bool GetBoolImport()
        {
            return this.boolImport;
        }

        /// <inheritdoc/>
        public string GetPOID()
        {
            return this.txtSP.Text;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
