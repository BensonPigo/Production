using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using Ict;
using Sci.Data;

namespace Sci.Production.Report.GSchemas
{
    /// <summary>
    /// DropDownListHelper
    /// </summary>
    public class DropDownListHelper
    {
        /// <summary>
        /// DropDownListHelper
        /// </summary>
        /// <param name="datas">datas</param>
        public DropDownListHelper(GLO.DropDownListDataTable datas)
        {
            this._datas = datas;
        }

        private GLO.DropDownListDataTable _datas;

        private bool _init;
        private IDictionary<string, GLO.DropDownListRow> _id_to_data;

        public GLO.DropDownListDataTable Datas { get { return this._datas; } set { this._datas = value; } }

        private void EnsureInit()
        {
            if (this._init)
            {
                return;
            }

            if (this._id_to_data == null)
            {
                var id_to_data = new Dictionary<string, GLO.DropDownListRow>();
                foreach (var it in this._datas)
                {
                    id_to_data.Set(it.ID, it);
                }

                this._id_to_data = id_to_data;
            }

            this._init = true;
        }

        public GLO.DropDownListRow Get(string id)
        {
            if (id == null)
            {
                return null;
            }

            this.EnsureInit();

            return this._id_to_data.GetOrDefault(id);
        }

        public string GetDisplayText_ID_NAME(string id)
        {
            var data = this.Get(id);
            if (data == null) return null;

            if (data.IsNameNull())
            {
                return data.ID;
            }
            else
            {
                return data.ID + "-" + data.Name;
            }
        }

        public TextValuePairs<string> GetPairs()
        {
            var pairs = new TextValuePairs<string>();
            foreach (var it in this.Datas)
            {
                pairs.Add(it.Name, it.ID);
            }

            return pairs;
        }
    }
}
