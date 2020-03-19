using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class R01 : Sci.Win.Tems.PrintForm
    {
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }


        string MDivisionID, FactoryID;
        DateTime? SewingDate_s, SewingDate_e;

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.txtMdivision.Text = Sci.Env.User.Keyword;
            this.txtfactory.Text = Sci.Env.User.Factory;
        }
        protected override bool ValidateInput()
        {
            if (!(SewingDate.Value1.HasValue || SewingDate.Value2.HasValue))
            {
                MyUtility.Msg.InfoBox("<Sewing Date> can’t be empty!!");
                return false;
            }


            this.MDivisionID = this.txtMdivision.Text;
            this.FactoryID = this.txtfactory.Text;
            this.SewingDate_s = SewingDate.Value1.Value;
            this.SewingDate_e = SewingDate.Value2.Value;

            if (!Check_Subprocess_LeadTime())
            {
                return false;
            }

            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return base.OnAsyncDataLoad(e);
        }


        public bool Check_Subprocess_LeadTime()
        {
            DataTable PoID_dt;
            DataTable GarmentTb;
            DataTable Msg_dt;
            DualResult result;

            string cmd = $@"

SELECT  DISTINCT OrderID
INTO #OrderList
FROM SewingSchedule
WHERE Inline >= '{SewingDate_s.Value.ToString("yyyy/MM/dd")}'
AND Inline <= '{SewingDate_e.Value.ToString("yyyy/MM/dd")}' 
";
            if (!MyUtility.Check.Empty(this.MDivisionID))
            {
                cmd += Environment.NewLine + $@"AND MDivisionID='{this.MDivisionID}'";
            }
            if (!MyUtility.Check.Empty(this.FactoryID))
            {
                cmd += Environment.NewLine + $@"AND FactoryID='{this.FactoryID}'";
            }

            cmd += $@"
SELECT DIStINCT  b.POID
FROM #OrderList a
INNER JOIN Orders b ON a.OrderID= b.ID 
";
            result = DBProxy.Current.Select(null, cmd, out PoID_dt);

            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            List<string> PoID_List = PoID_dt.AsEnumerable().Select(o => o["POID"].ToString()).ToList();
            List<string> Msg = new List<string>();
            foreach (var POID in PoID_List)
            {

                PublicPrg.Prgs.GetGarmentListTable(string.Empty, POID, "", out GarmentTb);

                List<string> AnnotationList = GarmentTb.AsEnumerable().Where(o => !MyUtility.Check.Empty(o["Annotation"].ToString())).Select(o => o["Annotation"].ToString()).Distinct().ToList();


                List<string> AnnotationList_Final = new List<string>();

                foreach (var Annotation in AnnotationList)
                {
                    foreach (var item in Annotation.Split('+'))
                    {
                        string input = "";
                        for (int i = 0; i <= item.Length-1; i++)
                        {
                            // 排除掉數字
                            if ( !int.TryParse(item[i].ToString(),out int x) )
                            {
                                input += item[i].ToString();
                            }
                        }
                        if (!AnnotationList_Final.Contains(input) && MyUtility.Check.Seek($"SELECT 1 FROM Subprocess WHERE ID='{input}' "))
                        {
                            AnnotationList_Final.Add(input);
                        }
                    }  
                }

                string AnnotationStr = AnnotationList_Final.OrderBy(o => o.ToString()).JoinToString("+");
                
                string chk_LeadTime = $@"
SELECT DISTINCT SD.ID,Subprocess.IDs
FROM SubprocessLeadTime_Detail SD
OUTER APPLY(
	SELECT IDs=STUFF(
	 (
		SELECT '+'+SubprocessID
		FROM SubprocessLeadTime_Detail
		WHERE ID = SD.ID
		FOR XML PATH('')
	)
	,1,1,'')
)Subprocess
WHERE Subprocess.IDs = '{AnnotationStr}'
";
                result = DBProxy.Current.Select(null, chk_LeadTime, out Msg_dt);
                if (!result)
                {
                    this.ShowErr(result);
                    return false;
                }

                // 收集需要顯示訊息的Subprocess ID
                if (Msg_dt.Rows.Count == 0)
                {
                    //var SubprocessList = Msg_dt.AsEnumerable().Select(o => o["IDs"].ToString()).ToList();
                    Msg.Add(AnnotationStr);
                }
            }

            if (Msg.Count > 0)
            {
                string Message = "<" + Msg.Distinct().JoinToString(">" + Environment.NewLine + "<") + ">";
                Message += Environment.NewLine + @"Please set cutting lead time in [Cutting_B09. Subprocess Lead Time].
When the settings are complete, can be export excel!
";

                MyUtility.Msg.InfoBox(Message);
                return false;
            }
            return true;
        }
    }
}
