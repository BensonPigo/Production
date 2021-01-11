using Sci.Data;
using System.ComponentModel;
using System.Data;
using Ict;

namespace Sci.Production.Class
{
    /// <summary>
    /// ComboDropDownList
    /// </summary>
    public partial class ComboDropDownList : Win.UI.ComboBox
    {
        private string type;
        private bool addAllItem = false;

        /// <summary>
        /// 是否有All的選項
        /// </summary>
        public bool AddAllItem
        {
            get
            {
                return this.addAllItem;
            }

            set
            {
                this.addAllItem = value;
                this.SetSource();
            }
        }

        /// <summary>
        /// Type
        /// </summary>
        [Category("Custom Properties")]
        public string Type
        {
            get
            {
                return this.type;
            }

            set
            {
                this.type = value;
                this.SetSource();
            }
        }

        private void SetSource()
        {
            if (!Env.DesignTime)
            {
                string unionAllItem = this.addAllItem ? "select [ID] = 'ALL', [Name] = 'ALL' , [Seq] = 0 union all" : string.Empty;
                string selectCommand = $@"
select *
from (
    {unionAllItem}
    select  ID
            , Name = rtrim(Name)
            , Seq
    from DropDownList WITH (NOLOCK) 
    where Type = '{this.Type}' 
    ) a
order by Seq

";

                DualResult returnResult;
                DataTable dropDownListTable = new DataTable();
                if (returnResult = DBProxy.Current.Select(null, selectCommand, out dropDownListTable))
                {
                    this.DataSource = dropDownListTable;
                    this.DisplayMember = "Name";
                    this.ValueMember = "ID";
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboDropDownList"/> class.
        /// </summary>
        public ComboDropDownList()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboDropDownList"/> class.
        /// </summary>
        /// <param name="container">container</param>
        public ComboDropDownList(System.ComponentModel.IContainer container)
        {
            container.Add(this);

            this.InitializeComponent();
        }
    }
}
