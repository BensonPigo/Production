using Ict.Win;
using Sci.Data;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Sci.Production.Class;
using System.Linq;

namespace Sci.Production.Thread
{
    /// <summary>
    /// P01_Generate
    /// </summary>
    public partial class P01_Generate : Win.Subs.Base
    {
        private DataTable gridTable;
        private string styleUkey;
        private string strstyleid;
        private string strseason;
        private string strbrandid;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <summary>
        /// P01_Generate
        /// </summary>
        /// <param name="str_styleukey">str_styleukey</param>
        /// <param name="str_styleid">str_styleid</param>
        /// <param name="str_season">str_season</param>
        /// <param name="str_brandid">str_brandid</param>
        public P01_Generate(string str_styleukey, string str_styleid, string str_season, string str_brandid)
        {
            this.InitializeComponent();

            this.cmbDescription.SelectedIndex = 0;
            this.styleUkey = str_styleukey;
            this.strstyleid = str_styleid;
            this.strseason = str_season;
            this.strbrandid = str_brandid;
            this.displayBoxEdit.Text = MyUtility.GetValue.Lookup(string.Format(
                @"
select ThreadEditname = concat(ThreadEditname,'-',p.name ,' ',format(ThreadEditdate,'yyyy/MM/dd HH:mm:ss' ))
from style s ,pass1 p
where s.ThreadEditname = p.id and
s.id = '{0}' and BrandID ='{1}' and SeasonID = '{2}'",
                this.strstyleid,
                this.strbrandid,
                this.strseason));

            DataGridViewGeneratorTextColumnSettings threadcombcell = cellthreadcomb.GetGridCell(true);
            this.gridDetail.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.Helper.Controls.Grid.Generator(this.gridDetail)
            .CheckBox("Sel", header: string.Empty, width: Widths.Auto(true), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
            .Text("ComboType", header: "ComboType", width: Widths.Auto(true), iseditingreadonly: true)
            .Text("Seq", header: "SEQ", width: Widths.Auto(true), iseditingreadonly: true)
            .Text("Operationid", header: "Operation Code", width: Widths.Auto(true), iseditingreadonly: true)
            .Text("Description", header: "Operation Description", width: Widths.Auto(true), iseditingreadonly: true)
            .Text("Annotation", header: "Annotation", width: Widths.Auto(true), iseditingreadonly: true)
            .Numeric("Seamlength", header: "Seam Length", width: Widths.Auto(true), integer_places: 9, decimal_places: 2, iseditingreadonly: true)
           .Numeric("Frequency", header: "Frequency", width: Widths.AnsiChars(6), integer_places: 4, decimal_places: 2, iseditingreadonly: true)
            .Text("MachineTypeid", header: "Machine Type", width: Widths.Auto(true), iseditingreadonly: true)
            .Text("Threadcombid", header: "Thread Combination", width: Widths.Auto(true), settings: threadcombcell);
            this.gridDetail.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridDetail.Columns["Threadcombid"].DefaultCellStyle.BackColor = Color.Pink;
            this.Loadgrid();
        }

        private void Loadgrid()
        {
            string sql = string.Format(
            @"
with a as (
    Select b.combotype,b.seq,threadcombid,operationid ,isnull(a.id,'') as id,f.id as styleid,f.seasonid,f.brandid
    from threadcolorcomb a WITH (NOLOCK) , threadcolorcomb_Operation b WITH (NOLOCK) ,style f WITH (NOLOCK) 
    where a.id = b.id and a.styleukey = f.ukey and 
    f.id = '{0}' and f.seasonid = '{1}' and f.brandid = '{2}'
),b as(
    Select c.ComboType,seq,operationid,d.annotation,e.SeamLength,d.MachineTypeID,
        Description = case when '{3}' = 'English' then e.descEN
                                    when '{3}' = 'Chinese' then g.DescCHS
                                    when '{3}' = 'Vietnamese' then g.DescVI
                                    when '{3}' = 'Cambodian' then g.DescKH end,
        styleid,seasonid,brandid,d.Frequency
    from timestudy c WITH (NOLOCK) ,timestudy_Detail d WITH (NOLOCK)  
    join operation e on e.id = d.operationid left join MachineType f on d.MachineTypeID=f.id
    left join OperationDesc g WITH (NOLOCK) on g.id = e.id
    where c.id = d.id and c.styleid = '{0}' and c.seasonid = '{1}' and c.brandid = '{2}' and e.SeamLength>0 and f.isThread=1
)

select 0 as sel,a.id,b.*,a.threadcombid
from b WITH (NOLOCK) 
left join a WITH (NOLOCK) on a.operationid = b.operationid and b.styleid = a.styleid 
and a.seasonid  = b.seasonid and a.brandid = b.brandid and a.combotype= b.ComboType and a.seq=b.Seq
order by seq",
            this.strstyleid,
            this.strseason,
            this.strbrandid,
            this.cmbDescription.Text);

            DualResult dResult = DBProxy.Current.Select(null, sql, out this.gridTable);
            if (dResult)
            {
                this.listControlBindingSource1.DataSource = this.gridTable;
            }
            else
            {
                this.ShowErr(sql);
                return;
            }
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            this.gridDetail.ValidateControl();
            if (MyUtility.Check.Empty(this.txtMachineType.Text))
            {
                this.gridTable.DefaultView.RowFilter = this.checkOnlyShowNotYetAssignCombination.Value == "True" ? "Threadcombid is null" : string.Empty;
            }
            else
            {
                this.gridTable.DefaultView.RowFilter = this.checkOnlyShowNotYetAssignCombination.Value == "True" ? string.Format("MachineTypeid = '{0}' and Threadcombid is null", this.txtMachineType.Text) : string.Format("MachineTypeid = '{0}'", this.txtMachineType.Text);
            }
        }

        // close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // save到DB
        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            this.gridDetail.ValidateControl();
            DataTable groupTable, operTable, gridTable3;

            gridTable3 = this.gridTable.Clone(); // 複製結構
            #region 準備有輸入資料的row到gridTable3,並判斷Threadcombid是否符合規則

            // Threadcombid任一筆有值則true
            if (this.gridTable.AsEnumerable().Any(row => !MyUtility.Check.Empty(row["Threadcombid"])))
            {
                // Threadcombid欄位有資料複製到gridTable3
                gridTable3 = this.gridTable.Select("threadcombid is not null and Threadcombid <> ''").CopyToDataTable();
            }
            #endregion
            DualResult dResult;

            // 準備未改變前ThreadColorComb_Detail的ID (Table)
            DataTable old_Detail;
            string old_Detailsql = string.Format("select distinct tcd.id,tcd.Machinetypeid,tcd.ThreadCombid from ThreadColorComb_Detail tcd inner join ThreadColorComb t on t.id = tcd.Id where styleukey = {0}", this.styleUkey);
            dResult = DBProxy.Current.Select(null, old_Detailsql, out old_Detail);
            if (!dResult)
            {
                this.ShowErr(dResult);
                return;
            }

            // 先Delete threadcolorcomb_operation,在更新ThreadColorComb之前,要先刪除threadcolorcomb_operation
            // 因為更新ThreadColorComb後ID會更新,threadcolorcomb_operation需要用未變更前的ID當條件,去清乾淨
            string deleteo_peration = string.Format("Delete from threadcolorcomb_operation where id in (select id from ThreadColorComb where styleukey = {0})", this.styleUkey);
            dResult = DBProxy.Current.Execute(null, deleteo_peration);
            if (!dResult)
            {
                this.ShowErr(dResult);
                return;
            }

            // 更新ThreadColorComb
            // 先準備#source,  id一樣update欄位,沒有的新增,多餘的刪去,再撈出更新後的表身
            dResult = MyUtility.Tool.ProcessWithDatatable(
                gridTable3,
                "id,threadcombid,Machinetypeid,seamlength,Frequency",
                string.Format(
                    @"
select id = iif(id = LAG(id,1) over(order by id),'',id),threadcombid,Machinetypeid,styleUkey,Length
into #source
from
(
    Select id = max(id),threadcombid,Machinetypeid,styleUkey = {0} ,Length = isnull(sum(seamlength * Frequency),0)
    from #tmp 
    group by threadcombid,machinetypeid
)a

merge ThreadColorComb as t
using(select * from #source) as s
on t.id = s.id
when matched then
	update set
	t.ThreadCombID	 = s.ThreadCombID
	,t.Machinetypeid = s.Machinetypeid
	,t.Length		 = s.Length
when not matched by target then
	insert(ThreadCombID,Machinetypeid,StyleUkey,Length)
	values(s.ThreadCombID,s.Machinetypeid,s.StyleUkey,s.Length)
when not matched by source and t.StyleUkey = {0} then
	delete;

select * from ThreadColorComb where StyleUkey = {0}",
                    this.styleUkey),
                out groupTable);

            if (!dResult)
            {
                this.ShowErr(dResult);
                return;
            }

            // threadcolorcomb_Operation 其他欄位先用grid上的table組好, 但id要從已經存到DB的ThreadColorComb
            MyUtility.Tool.ProcessWithDatatable(
                gridTable3,
                "operationid,ComboType,SEQ,threadcombid,Machinetypeid,Frequency",
                string.Format(
                    @"
Select distinct operationid,ComboType,SEQ,threadcombid,Machinetypeid,Frequency
into #new
from #tmp
--
insert into threadcolorcomb_Operation(Id,Operationid,ComboType,Seq,Frequency)
select t.id,n.operationid,n.ComboType,n.SEQ,n.Frequency
from #new n inner join ThreadColorComb t on t.threadcombid = n.threadcombid and t.Machinetypeid = n.Machinetypeid
where t.styleukey = {0}

select tco.* from threadcolorcomb_Operation tco inner join ThreadColorComb t on t.id = tco.Id where  styleukey = {0}",
                    this.styleUkey),
                out operTable);

            // 更新ThreadColorComb.ConsPC, 要在更新threadcolorcomb_Operation之後做
            string computeConsPC = string.Format(
                @"
update Tcc
	Set ConsPC = ROUND(ConsPC.ConsPC, 2)
from ThreadColorComb Tcc
Outer Apply(
    Select ConsPC = sum(isnull(O.SeamLength, 0) * isnull(TccO.Frequency, 0) * isnull(MtTr.UseRatio, 0) + isnull(MtTr.Allowance, 0))
    From ThreadColorComb_operation TccO
    left join Operation O on TccO.Operationid = O.ID
    left join MachineType_ThreadRatio MtTr on Tcc.Machinetypeid = MtTr.ID
    where   Tcc.ID = TccO.Id
) ConsPC
where Tcc.StyleUkey = '{0}'",
                this.styleUkey);

            dResult = DBProxy.Current.Execute(null, computeConsPC);
            if (!dResult)
            {
                this.ShowErr(dResult);
                return;
            }

            // 更新ThreadColorComb_Detail,只會有兩種況狀,減少就Delete, ThreadCombid變動就更新
            DataTable tcc_Detail;
            if (old_Detail.Rows.Count > 0)
            {
                dResult = MyUtility.Tool.ProcessWithDatatable(
                    old_Detail,
                    "id,Machinetypeid,ThreadCombid",
                    string.Format(
                        @"
delete ThreadColorComb_Detail where id in
(
    select a.id
    from #tmp a left join (select distinct id,Machinetypeid,ThreadCombid from ThreadColorComb where styleukey = {0})b
    on a.Machinetypeid = b.Machinetypeid and a.ThreadCombid = b.ThreadCombid
    where isnull(b.id,'') =''
)

update tcd
    set tcd.id = t.id
from ThreadColorComb_Detail tcd inner join ThreadColorComb t on tcd.Machinetypeid = t.Machinetypeid and tcd.ThreadCombid = t.ThreadCombid
where styleukey = {0} and tcd.id in (select distinct id from #tmp)

select tcd.ThreadCombid from ThreadColorComb_Detail tcd inner join ThreadColorComb t on t.id = tcd.Id where styleukey = {0}
",
                        this.styleUkey),
                    out tcc_Detail);

                if (!dResult)
                {
                    this.ShowErr(dResult);
                    return;
                }
            }

            string styleEdit = string.Format(
                @"
update s set 
    ThreadEditname ='{3}',ThreadEditdate='{4}' 
from style s 
where id = '{0}' and BrandID ='{1}' and SeasonID = '{2}'",
                this.strstyleid,
                this.strbrandid,
                this.strseason,
                Sci.Env.User.UserID,
                DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            DualResult result = DBProxy.Current.Execute(null, styleEdit);
            if (!result)
            {
                this.ShowErr(styleEdit, result);
                return;
            }

            this.Close();
        }

        private void BtnBatchUpdate_Click(object sender, EventArgs e)
        {
            foreach (DataRowView dr in this.gridTable.DefaultView)
            {
                if (dr["Sel"].ToString() == "1")
                {
                    dr["ThreadCombid"] = this.txtthreadcomb.Text;
                    dr["Sel"] = false;
                    dr.EndEdit();
                }
            }

            this.gridDetail.ValidateControl();
        }

        private void TxtMachineType_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                string.Format(
                    @"Select distinct d.MachineTypeID
            from timestudy c WITH (NOLOCK) ,timestudy_Detail d WITH (NOLOCK) 
            join operation e WITH (NOLOCK) on e.id = d.operationid
            where c.id = d.id and c.styleid = '{0}' and c.seasonid = '{1}' and c.brandid = '{2}' and e.SeamLength>0 ",
                    this.strstyleid,
                    this.strseason,
                    this.strbrandid),
                "23",
                this.txtMachineType.Text,
                false,
                ",");

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtMachineType.Text = item.GetSelectedString();
        }

        private void TxtMachineType_Validating(object sender, CancelEventArgs e)
        {
            this.OnValidating(e);
            string str = this.txtMachineType.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.txtMachineType.OldValue)
            {
                string tmp = MyUtility.GetValue.Lookup("id", str, "Machinetype", "id");
                if (string.IsNullOrWhiteSpace(tmp))
                {
                    this.txtMachineType.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Machine Type : {0}> not found!!!", str));
                    return;
                }

                // string cjunk = MyUtility.GetValue.Lookup("Junk", str, "Machinetype", "id");
                // if (cjunk == "True")
                // {
                //    MyUtility.Msg.WarningBox(string.Format("Machine Type already junk, you can't choose!!"));
                //    this.textBox1.Text = "";
                // }
            }
        }

        private void cmbDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Loadgrid();
        }
    }
}
