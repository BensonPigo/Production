using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.PublicForm
{
    /// <inheritdoc/>
    public partial class EachConsumption_SwitchWorkOrder : Win.Subs.Base
    {
        private readonly string loginID = Env.User.UserID;
        private readonly string keyWord = Env.User.Keyword;
        private readonly string cuttingid;

        /// <inheritdoc/>
        public EachConsumption_SwitchWorkOrder(string cutid)
        {
            this.InitializeComponent();
            this.cuttingid = cutid;

            // 589:CUTTING_P01_EachConsumption_SwitchWorkOrder，(1) 若Orders.IsMixmarker=true則只能選第1個選項，第2個選項要disable。
            string sql = string.Format("SELECT IsMixmarker FROM Orders WITH (NOLOCK) WHERE ID='{0}'", this.cuttingid);
            bool isMixmarker = MyUtility.GetValue.Lookup(sql) == "1";
            if (isMixmarker)
            {
                this.radioBySP.Enabled = false;
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.Query();
        }

        private void GridSetup()
        {
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("FabricCombo", header: "Fabric Combo", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .CheckBox("ExWip", header: "Exclude in WIP", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("FabricCode", header: "Fabric#", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(55), iseditingreadonly: true)
                ;
        }

        private void Query()
        {
            string sqlcmd = $@"
select distinct
	oe.FabricCombo,
	ExWip=CAST(isnull((select 1 from Cutting_WIPExcludePatternPanel cw with(nolock) where cw.ID = oe.Id and cw.PatternPanel = oe.FabricCombo), 0) as bit),
	oe.FabricCode,f.Refno,f.Description
from Order_EachCons oe with(nolock)
inner join Order_BOF bof with(nolock) on bof.Id = oe.Id and bof.FabricCode = oe.FabricCode
LEFT JOIN Fabric f with(nolock) ON bof.SCIRefno=f.SCIRefno
where oe.Id = '{this.cuttingid}'
and oe.CuttingPiece <> 1
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable gridtb);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = gridtb;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            string cmd = string.Empty;
            string worktype;
            if (this.radioCombination.Checked)
            {
                worktype = "1";
            }
            else
            {
                worktype = "2";
            }

            #region 檢核

            #region Article + Sizecode 有缺, 停止
            string checkArticleSize = $@"
select distinct a.SizeCode,a.Article
from Order_Qty a with(nolock)
inner join  Orders o  with(nolock) on o.id = a.id
left join Order_EachCons_Color_Article b with(nolock)on a.SizeCode = b.SizeCode and a.Article = b.Article and a.id = b.id
where a.id = '{this.cuttingid}' and b.Article is null and a.Qty > 0
and (o.Junk=0 or o.Junk=1 and o.NeedProduction=1)
";
            DualResult result = DBProxy.Current.Select(null, checkArticleSize, out DataTable dTcheckAS);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dTcheckAS.Rows.Count > 0)
            {
                var m = new Win.UI.MsgGridForm(dTcheckAS, "Switching is stopped for these arctile size are not found in [ Each Consumpotion ], but exists in [ Quantity Breakdown ]", " Do you still want to Switch to WorkOrder ?", null, MessageBoxButtons.YesNo)
                {
                    Width = 500,
                };
                m.grid1.Columns[1].Width = 140;
                m.text_Find.Width = 140;
                m.btn_Find.Location = new Point(150, 6);
                m.btn_Find.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                m.ShowDialog();

                if (m.result == DialogResult.No)
                {
                    return;
                }
            }
            #endregion

            #region 若只要有一筆不存在BOF就不可轉
            cmd = string.Format(@"Select * from Order_EachCons a WITH (NOLOCK) Left join Order_Bof b WITH (NOLOCK) on a.id = b.id and a.FabricCode = b.FabricCode Where a.id = '{0}' and b.id is null", this.cuttingid);
            DualResult worRes = DBProxy.Current.Select(null, cmd, out DataTable bofnullTb);
            if (!worRes)
            {
                this.ShowErr(cmd, worRes);
                return;
            }

            if (bofnullTb.Rows.Count != 0)
            {
                string errMsg = "Can't find BOF data, please inform MR team !!";
                foreach (DataRow dr in bofnullTb.Rows)
                {
                    errMsg = errMsg + Environment.NewLine + string.Format(@"Seq: {0} MarkName: {1} FabricCombo：{2} can't mapping BOF data!", dr["seq"].ToString(), dr["MarkerName"].ToString(), dr["FabricCombo"].ToString());
                }

                MyUtility.Msg.WarningBox(errMsg, "Warning");
                return;
            }
            #endregion

            #region 若Cutplanid有值就不可刪除重轉
            worRes = DBProxy.Current.Select(null, string.Format("Select id from workorder WITH (NOLOCK) where id = '{0}' and cutplanid  != '' ", this.cuttingid), out DataTable workorder);
            if (!worRes)
            {
                this.ShowErr(worRes);
                return;
            }

            if (workorder.Rows.Count != 0)
            {
                MyUtility.Msg.WarningBox("The Work Order already created cutplan, you cann't re-switch to Work Order !!", "Warning");
                return;
            }
            else
            {
                DialogResult buttonResult = MyUtility.Msg.WarningBox("Data exists, do you want to over-write work order data?", "Warning", MessageBoxButtons.YesNo);
                if (buttonResult == DialogResult.No)
                {
                    return;
                }
            }
            #endregion

            #region 依每個FabricPanelCode, article判斷，若Order_Eachcons_Color的顏色不存在於order_colorcombo中，則跳出警告視窗。
            string checkByFabricPanelCode_article_color = $@"
SELECT  FabricPanelCode, article, ColorID
into #tmp1
FROM Order_Eachcons OE
left join Order_Eachcons_Color OEC on OEC.Order_EachConsUkey=OE.Ukey
where OE.id='{this.cuttingid}'
group by FabricPanelCode, article, ColorID

SELECT  FabricPanelCode, article, ColorID
into #tmp2
FROM order_colorcombo where id='{this.cuttingid}'
group by FabricPanelCode, article, colorid

select a.*
from #tmp1 a
left join #tmp2 b on a.fabricpanelcode = b.fabricpanelcode and a.ColorID = b.ColorID 
and (b.article in (select data from [dbo].[SplitString](a.Article,',')where data !='') or a.Article ='')
where  b.fabricpanelcode is null

drop table #tmp1,#tmp2
";
            worRes = DBProxy.Current.Select(null, checkByFabricPanelCode_article_color, out DataTable dTcheckFabricPanelCode_article_color);
            if (!worRes)
            {
                this.ShowErr(worRes);
                return;
            }

            StringBuilder msgFabricPanelCode_article_color = new StringBuilder();
            if (dTcheckFabricPanelCode_article_color.Rows.Count > 0)
            {
                foreach (DataRow item in dTcheckFabricPanelCode_article_color.Rows)
                {
                    msgFabricPanelCode_article_color.Append($"FabricPanelCode={item["FabricPanelCode"]}, color={item["ColorID"]}" + Environment.NewLine);
                }

                msgFabricPanelCode_article_color.Append("do not exist in order_colorcombo. Please check with MR first. Do you want to continue?");
                DialogResult result1 = MyUtility.Msg.QuestionBox(msgFabricPanelCode_article_color.ToString(), "Warning");
                if (result1 == DialogResult.No)
                {
                    return;
                }
            }
            #endregion

            #endregion

            #region transaction

            // Create the TransactionOptions object
            TransactionOptions oTranOpt = default(TransactionOptions);

            // Set the Isolation Level
            oTranOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;

            // Uses the (hours, minutes, seconds) constructor
            TimeSpan oTime = new TimeSpan(0, 5, 0);
            oTranOpt.Timeout = oTime;

            DualResult dResult;
            TransactionScope transactionscope = new TransactionScope(TransactionScopeOption.RequiresNew, oTranOpt);
            using (transactionscope)
            {
                try
                {
                    cmd = string.Format(
                        @"
select Ukey from Workorder with (nolock) where id= '{0}' and CutRef <> '' and CutRef is not null;
select WorkOrderUkey, OrderID, Article, SizeCode 
from WorkOrder_Distribute  with (nolock)
where WorkOrderUkey in (select Ukey from Workorder with (nolock) where id='{0}' and CutRef <> '' and CutRef is not null);

Delete Workorder where id='{0}';
Delete WorkOrder_Distribute where id='{0}';
Delete WorkOrder_SizeRatio where id='{0}';
Delete WorkOrder_Estcutdate where id='{0}';
Delete WorkOrder_PatternPanel where id='{0}'

Delete Cutting_WIPExcludePatternPanel where id = '{0}'
", this.cuttingid);
                    if (!(dResult = DBProxy.Current.Select(null, cmd, out DataTable[] tablesWorkorder)))
                    {
                        throw new Exception(dResult.Messages.ToString());
                    }

                    DataTable sdt = (DataTable)this.listControlBindingSource1.DataSource;
                    string insertcmd = $@"insert into Cutting_WIPExcludePatternPanel(ID,PatternPanel,AddName,AddDate)
select '{this.cuttingid}',FabricCombo,'{this.loginID}',getdate() from #tmp where ExWip = 1";
                    DualResult result1 = MyUtility.Tool.ProcessWithDatatable(sdt, string.Empty, insertcmd, out DataTable odt);
                    if (!result1)
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result1);
                        return;
                    }

                    string exswitch;
                    if (worktype == "1")
                    {
                        exswitch = string.Format("exec dbo.usp_switchWorkorder '{0}','{1}','{2}','{3}'", worktype, this.cuttingid, this.keyWord, this.loginID);
                    }
                    else
                    {
                        // By SP worktype = 2
                        exswitch = string.Format("exec dbo.usp_switchWorkorder_BySP '{0}','{1}','{2}','{3}'", worktype, this.cuttingid, this.keyWord, this.loginID);
                    }

                    DBProxy sp_excute = new DBProxy
                    {
                        DefaultTimeout = 1200, // 因為資料量多會執行較久所以設定timeout20分鐘
                    };

                    // DualResult dResult = DBProxy.Current.Execute(null, exswitch);
                    dResult = sp_excute.Execute(null, exswitch);
                    if (!dResult)
                    {
                        throw new Exception(dResult.Messages.ToString());
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();

                    if (tablesWorkorder[0].Rows.Count > 0)
                    {
                        List<Guozi_AGV.WorkOrder_Distribute> deleteWorkOrder_Distribute = new List<Guozi_AGV.WorkOrder_Distribute>();
                        List<long> deleteWorkOrder = new List<long>();

                        foreach (DataRow dr in tablesWorkorder[0].AsEnumerable())
                        {
                            deleteWorkOrder.Add((long)dr["Ukey"]);
                        }

                        foreach (DataRow dr in tablesWorkorder[1].AsEnumerable())
                        {
                            deleteWorkOrder_Distribute.Add(new Guozi_AGV.WorkOrder_Distribute()
                            {
                                WorkOrderUkey = (long)dr["WorkOrderUkey"],
                                SizeCode = (string)dr["SizeCode"],
                                Article = (string)dr["Article"],
                                OrderID = (string)dr["OrderID"],
                            });
                        }

                        Task.Run(() => new Guozi_AGV().SentDeleteWorkOrder(deleteWorkOrder));
                        Task.Run(() => new Guozi_AGV().SentDeleteWorkOrder_Distribute(deleteWorkOrder_Distribute));
                    }
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    dResult = new DualResult(false, "Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;

            if (!dResult)
            {
                this.ShowErr(dResult);
                return;
            }
            #endregion

            MyUtility.Msg.InfoBox("Switch successful");
            this.Close();
        }
    }
}
