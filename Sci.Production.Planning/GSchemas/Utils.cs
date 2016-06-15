using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using Ict;
using Sci.Data;
namespace Sci.Trade.Report.GSchemas
{
    public class DropDownListHelper
    {
        public DropDownListHelper(GLO.DropDownListDataTable datas)
        {
            _datas = datas;
        }

        GLO.DropDownListDataTable _datas;

        bool _init;
        IDictionary<string, GLO.DropDownListRow> _id_to_data;

        public GLO.DropDownListDataTable Datas { get { return _datas; } set { _datas = value; } }

        private void EnsureInit()
        {
            if (_init) return;

            if (null == _id_to_data)
            {
                var id_to_data = new Dictionary<string, GLO.DropDownListRow>();
                foreach (var it in _datas) id_to_data.Set(it.ID, it);
                _id_to_data = id_to_data;
            }

            _init = true;
        }
        public GLO.DropDownListRow Get(string id)
        {
            if (null == id) return null;
            EnsureInit();

            return _id_to_data.GetOrDefault(id);
        }
        public string GetDisplayText_ID_NAME(string id)
        {
            var data = Get(id); if (null == data) return null;

            if (data.IsNameNull()) return data.ID;
            else return data.ID + "-" + data.Name;
        }
        public TextValuePairs<string> GetPairs()
        {
            var pairs = new TextValuePairs<string>();
            foreach (var it in Datas)
            {
                pairs.Add(it.Name, it.ID);
            }
            return pairs;
        }
    }
    public static class Utils
    {
        public static string ConnectionName { get; set; }

        public static DualResult GetDropDownList(string type, out DropDownListHelper helper)
        {
            helper = null;
            DualResult result;

            GLO.DropDownListDataTable datas;
            if (!(result = GetDropDownList(type, out datas))) return result;

            helper = new DropDownListHelper(datas);
            return Result.True;
        }
        public static DualResult GetDropDownList(string type, out GLO.DropDownListDataTable datas)
        {
            datas = null;
            DualResult result;

            try
            {
                SqlConnection conn = null;
                if (!(result = DBProxy.Current.OpenConnection(ConnectionName, out conn))) return result;
                using (conn)
                {
                    using (var adapter = new GLOTableAdapters.DropDownListTableAdapter())
                    {
                        adapter.Connection = conn;

                        datas = adapter.GetsByType(type);
                    }
                }
            }
            catch (Exception ex)
            {
                return new DualResult(false, "Get Ap Qty value error.", ex);
            }

            return Result.True;
        }
    }
}
