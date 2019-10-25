using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class P08_Download : Sci.Win.Tems.QueryForm
    {
        DataRow Master;
        string ClipPath;
        int i = 1;
        public P08_Download(DataRow master)
        {
            this.InitializeComponent();
            this.Master = master;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("FileName", header: "File Name", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("AddName", header: "Add Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .DateTime("AddDate", header: "Add Date", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Button("Download", propertyname: "Download", header: string.Empty, width: Widths.AnsiChars(12), onclick: this.DownLoad)
            ;

            this.Query();
        }

        private void Query()
        {
            string sqlcmd = $@"
select Clip.*,FileName  = concat(TableName,PKey,'.'+x.value) ,Download='Download'
from Clip 
outer apply(
	select top 1 s.Data
	from dbo.SplitString(SourceFile,'.') s
	order by no desc
)s
outer apply(
	select value=iif(SourceFile like '%.%',s.Data,null)
)x
where TableName = 'ReplacementReport' AND UniqueKey = '{this.Master["ID"]}'";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;

            sqlcmd = "select ReplacementReport from System";
            this.ClipPath = MyUtility.GetValue.Lookup(sqlcmd);
        }

        private void DownLoad(object sender, EventArgs s)
        {
            DataRow dr = this.grid1.GetDataRow(this.listControlBindingSource1.Position);

            if (!Directory.Exists(this.ClipPath))
            {
                MyUtility.Msg.WarningBox("Please check the path setting.");
                return;
            }

            DirectoryInfo diInfo = new DirectoryInfo(this.ClipPath);
            if (!diInfo.Exists)
            {
                MyUtility.Msg.WarningBox("Please check the path setting.");
                return;
            }

            string fullpath = Path.Combine(this.ClipPath, dr["FileName"].ToString());
            if (!File.Exists(fullpath))
            {
                MyUtility.Msg.WarningBox("File not Exists!");
                return;
            }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "*|*";
            saveFileDialog1.Title = "Save File";
            saveFileDialog1.FileName = dr["FileName"].ToString();

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    System.IO.File.Copy(fullpath, saveFileDialog1.FileName, true);
                }
                catch (System.IO.IOException exception)
                {
                    MyUtility.Msg.ErrorBox("Error: Download file fail. Original error: " + exception.Message);
                }
            }
        }
    }
}
