using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Sci.DB;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Xml.Linq;
using System.Windows.Forms;
using Sci.Win.Tools;
namespace Sci.Production.Class.Command
{
    public class SQL
    {
        ///
        /// SqlBuliCopy
        /// https://msdn.microsoft.com/zh-tw/library/ex21zs8x(v=vs.110).aspx
        /// 
        /// <summary>
        /// SQL Bulk merge refer to 
        /// http://www.zzzprojects.com/guides/bulk-operations/index.html
        /// </summary>
        /// 

        public static string QueryConnectionName = string.Empty;

        private static SqlConnection _queryConn = null;

        /// <summary>
        /// 提供常駐的Connection, 並會判斷連線狀態是否關閉做重開
        /// </summary>
        public static SqlConnection queryConn
        {
            get
            {
                if (null == SQL._queryConn
                    || ConnectionState.Open != SQL._queryConn.State)
                {
                    SQL.GetConnection(out SQL._queryConn, QueryConnectionName);
                }

                //SQL._queryConn.ConnectionTimeout = 9999;
                return SQL._queryConn;
            }

            set { SQL._queryConn = value; }
        }

        #region TableUpdate by Adapter 相關的method

        #region CursorTableUpdate - TSQL - select_TableColumnsDef_ByDataBase_Table
        private const string select_TableColumnsDef_ByDataBase_Table =
@"
Use @myDataBase@

select tab.name as tab_Name,c.name as Col_Name,c.is_identity,c.is_computed
	,t.name as type_Name
	,c.max_length,c.precision,c.scale
from sys.tables as tab
left join sys.columns as c on tab.object_id = c.object_id
inner join sys.types as t on c.user_type_id = t.user_type_id
where tab.type = 'U' and tab.Name = '@myTable@'
order by tab.name,c.name

select tab.name as tab_Name,c.name as Col_Name
from sys.tables as tab
left join sys.indexes as i on i.object_id = tab.object_id
left join sys.index_columns as ic on ic.object_id = tab.object_id and ic.index_id = i.index_id
inner join sys.columns as c on ic.column_id = c.column_id and c.object_id = tab.object_id
where tab.type = 'U' and i.is_primary_key = 1 and tab.Name = '@myTable@'
order by tab.name -- tab.object_id,i.index_id,ic.index_column_id
";
        #endregion
        #region CursorTableUpdate - TSQL - select_AllTableColumnsDef
        private const string select_AllTableColumns =
@"select tab.name as tab_Name,c.name as Col_Name,c.is_identity,c.is_computed
	,t.name as type_Name
	,c.max_length,c.precision,c.scale
	--,sep.value as [Description]
from sys.tables as tab
left join sys.columns as c on tab.object_id = c.object_id
inner join sys.types as t on c.user_type_id = t.user_type_id
/*
left join sys.extended_properties sep on tab.object_id = sep.major_id
                                         and c.column_id = sep.minor_id
                                         and sep.name = 'MS_Description'
*/
where tab.type = 'U' 
order by tab.name,c.name

select tab.name as tab_Name,c.name as Col_Name
	-- ,i.name as Idx_Name,i.is_primary_key,i.is_unique	
	--,tab.object_id,i.index_id
from sys.tables as tab
left join sys.indexes as i on i.object_id = tab.object_id
left join sys.index_columns as ic on ic.object_id = tab.object_id and ic.index_id = i.index_id
inner join sys.columns as c on ic.column_id = c.column_id and c.object_id = tab.object_id
where tab.type = 'U' and i.is_primary_key = 1
order by tab.name -- tab.object_id,i.index_id,ic.index_column_id
";
        #endregion

        private static Dictionary<string, List<DBColumn>> tableCols = new Dictionary<string, List<DBColumn>>(StringComparer.OrdinalIgnoreCase);
        private static Dictionary<string, List<String>> tablePKs = new Dictionary<string, List<String>>(StringComparer.OrdinalIgnoreCase);
        private static Dictionary<string, Dictionary<string, DBTable>> tableSchemas
            = new Dictionary<string, Dictionary<string, DBTable>>(StringComparer.OrdinalIgnoreCase);

        static Dictionary<string, Dictionary<string, ITableSchema>> ictTableSchemas
            = new Dictionary<string, Dictionary<string, ITableSchema>>(StringComparer.OrdinalIgnoreCase);


        public static DBTable GetDBTable_ByDataBase(string tableName, SqlConnection conn)
        {
            string[] str = tableName.Split(new char[] { '.' });

            if (str.Length == 1)
            {
                return GetDBTable(tableName, conn);
            }

            string dataBase = str.Length == 1
                ? string.Empty
                : str[0];

            string finalTableName = str.Length == 3
                ? tableName                   // tableName = Trade.dbo.MyTable
                : dataBase + "." + tableName; // tableName 只給 dbo.MyTable
            string myTable = str[str.Length - 1];
            string sqlCmd = select_TableColumnsDef_ByDataBase_Table
                    .Replace("@myDataBase@", dataBase)
                    .Replace("@myTable@", myTable);
            return GetDBTable(tableName, conn, sqlCmd);
        }

        public static DBTable GetDBTable_ByDataBase(string tableName, SqlConnection conn, string dataBase)
        {
            dataBase = string.IsNullOrWhiteSpace(dataBase)
                ? conn.Database
                : dataBase;
            string sqlCmd = select_TableColumnsDef_ByDataBase_Table
                    .Replace("@myDataBase@", dataBase)
                    .Replace("@myTable@", tableName);
            return GetDBTable(tableName, conn, sqlCmd);
        }

        public static DBTable GetDBTable(string tableName, string connectionName, string newSelectTableCmd = null)
        {
            SqlConnection conn;
            if (!SQL.GetConnection(out conn, connectionName))
            {
                return null;
            }

            return GetDBTable(tableName, conn, newSelectTableCmd);
        }

        public static DBTable GetDBTable(string tableName, SqlConnection conn, string newSelectTableCmd = null)
        {
            Dictionary<string, DBTable> dictionaryTables;
            if (tableSchemas.ContainsKey(conn.Database))
            {
                dictionaryTables = tableSchemas[conn.Database];
                if (dictionaryTables.ContainsKey(tableName))
                {
                    return dictionaryTables[tableName];
                }
            }
            else
            {
                dictionaryTables = new Dictionary<string, DBTable>(StringComparer.OrdinalIgnoreCase);
                tableSchemas.Add(conn.Database, dictionaryTables);
            }

            // 如果沒有現成的就重新制做 DBTable
            List<DBColumn> dbColumns = DBProxy.GetTableColumns(tableName, conn, newSelectTableCmd);
            DBTable dbTable = new DBTable();
            dbTable.name = tableName;
            dbTable.columns = dbColumns.ToDictionary(k => k.Name, v => v, StringComparer.OrdinalIgnoreCase);
            dbTable.identity = dbColumns.FirstOrDefault(column => column.isIdentity);
            dbTable.primaryKey = new DBIndex("PK_" + tableName.Replace('.', '_'), true, true, dbColumns.Where(col => col.isPrimaryKey));

            // 擺到 tableSchemas 裡面去, 方便 reuse
            dictionaryTables.Add(tableName, dbTable);
            return dbTable;
        }

        public static ITableSchema GetTableSchema(string tableName, string connectionName = null, object connection = null)
        {
            Dictionary<string, ITableSchema> dictionaryTables;
            SqlConnection conn;

            // 避免重複建立connection
            if (connection != null)
            {
                conn = (SqlConnection)connection;
            }
            else
            {
                if (!SQL.GetConnection(out conn, connectionName))
                {
                    return null;
                }
            }

            if (ictTableSchemas.ContainsKey(conn.Database))
            {
                dictionaryTables = ictTableSchemas[conn.Database];
                if (dictionaryTables.ContainsKey(tableName))
                {
                    return dictionaryTables[tableName];
                }
            }
            else
            {
                dictionaryTables = new Dictionary<string, ITableSchema>(StringComparer.OrdinalIgnoreCase);
                ictTableSchemas.Add(conn.Database, dictionaryTables);
            }

            // 如果沒有現成的就重新制做 DBTable
            ITableSchema schema;
            DualResult result;
            if (!(result = DBProxy.Current.GetTableSchemaByConn(conn, tableName, out schema)))
            {
                new BugProc(result.GetException()).Show();
                return null;
            }

            // 擺到 tableSchemas 裡面去, 方便 reuse
            dictionaryTables.Add(tableName, schema);
            return schema;
        }

        public static bool DBProxyDelete(string tableName, DataRow row, object connection = null)
        {
            DualResult result;
            ITableSchema schema = GetTableSchema(tableName, connection: connection);
            if (schema == null) { return false; }

            // 判斷要不要開connection
            if (connection == null || connection is string)
            {
                if (!(result = DBProxy.Current.Delete((string)connection, schema, row)))
                {
                    new BugProc(result.GetException()).Show();
                    return false;
                }
            }
            else if (!(result = DBProxy.Current.DeleteByConn((SqlConnection)connection, schema, row)))
            {
                new BugProc(result.GetException()).Show();
                return false;
            }

            return true;
        }

        public static bool DBProxyDelete(string tableName, IEnumerable<DataRow> rows, object connection = null)
        {
            // 透過 Linq 判斷只要一個不符合就結束迴圈
            return rows.All(row => DBProxyDelete(tableName, row, connection));
        }

        public static bool DBProxyInsert(string tableName, DataRow row, Object connection = null)
        {
            DualResult result;
            ITableSchema schema = GetTableSchema(tableName, connection: connection);
            if (null == schema) { return false; }

            // 判斷要不要開connection
            if (connection == null || connection is string)
            {
                if (!(result = DBProxy.Current.Insert((string)connection, schema, row)))
                {
                    new BugProc(result.GetException()).Show();
                    return false;
                }
            }
            else if (!(result = DBProxy.Current.InsertByConn((SqlConnection)connection, schema, row)))
            {
                new BugProc(result.GetException()).Show();
                return false;
            }

            return true;
        }

        public static bool DBProxyInsert(string tableName, IEnumerable<DataRow> rows, object connection = null)
        {
            // 透過 Linq 判斷只要一個不符合就結束迴圈
            return rows.All(row => DBProxyInsert(tableName, row, connection));
        }

        public static bool DBProxyUpdateByChanged(string tableName, DataRow row, object connection = null)
        {
            DualResult result;
            ITableSchema schema = GetTableSchema(tableName, connection: connection);
            if (schema == null) { return false; }

            // 判斷要不要開connection
            bool ischanged;
            if (connection == null || connection is string)
            {
                if (!(result = DBProxy.Current.UpdateByChanged((string)connection, schema, row, out ischanged)))
                {
                    new BugProc(result.GetException()).Show();
                    return false;
                }
            }
            else if (!(result = DBProxy.Current.UpdateByChangedByConn((SqlConnection)connection, schema, row, out ischanged)))
            {
                new BugProc(result.GetException()).Show();
                return false;
            }

            return true;
        }

        public static bool DBProxyUpdateByChanged(string tableName, IEnumerable<DataRow> rows, object connection = null)
        {
            // 透過 Linq 判斷只要一個不符合就結束迴圈
            return rows.All(row => DBProxyUpdateByChanged(tableName, row, connection));
        }

        public static bool DBProxyBatchCommit(string tableName, IEnumerable<DataRow> rows, object connection = null)
        {
            return rows.All(row =>
                (row.RowState == DataRowState.Deleted && DBProxyDelete(tableName, row, connection))
                || (row.RowState == DataRowState.Modified && DBProxyUpdateByChanged(tableName, row, connection))
                || (row.RowState == DataRowState.Added && DBProxyInsert(tableName, row, connection))
                || (row.RowState == DataRowState.Detached && DBProxyUpdateByChanged(tableName, row, connection)));
        }

        /// <summary>
        /// 利用已經暫存好的Table Columns設定拼出 Insert Command
        /// <para>insert 欄位只排除 identity和 computed 欄位</para>
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="source"></param>
        /// <param name="dbColumns"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static SqlCommand createInsertCommand(string tableName, DataTable source, List<DBColumn> dbColumns, SqlConnection conn)
        {
            // 列出哪些 dataTable 欄位是DB上面同名的Column
            // 排除 identity,compute 欄位
            var intersect_DBCols = from dbCol in dbColumns
                                   where source.Columns.Contains(dbCol.Name)
                                        && !dbCol.isIdentity && !dbCol.isComputed
                                   select dbCol;

            var intersect_Col_Names = from dbCol in intersect_DBCols
                                      select dbCol.Name;

            StringBuilder strValues = new StringBuilder();
            for (int i = 0; i < intersect_Col_Names.Count(); i++)
            {
                if (i > 0)
                {
                    strValues.Append(", ");
                }

                var col = intersect_Col_Names.ElementAt(i);
                var defaultValue = dbColumns.Where(r => r.Name == col).FirstOrDefault().defaultValue;
                if (defaultValue != null)
                {
                    if (defaultValue == string.Empty)
                    {
                        defaultValue = "''";
                    }

                    strValues.Append($"isnull(@{col}, {defaultValue})");
                }
                else
                {
                    strValues.Append($"@{col}");
                }
            }

            // 組TSQL
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into ").Append(tableName)
                .Append("(").Append(string.Join(",", intersect_Col_Names)).Append(")")
                .Append($" values({strValues})");
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            foreach (DBColumn dbCol in intersect_DBCols)
            {
                cmd.Parameters.Add("@" + dbCol.Name, dbCol.Type, dbCol.Size, dbCol.Name);
            }

            return cmd;
        }

        /// <summary>
        /// 利用已經暫存好的Table Columns設定拼出 Update Command
        /// <para>update ... set 的部分排除 primary key, identity 和 computed 欄位</para>
        /// <para>where 條件式的部分才會使用到primary key</para>
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="source"></param>
        /// <param name="dbColumns"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static SqlCommand createUpdateCommand(string tableName, DataTable source, List<DBColumn> dbColumns, SqlConnection conn)
        {
            // 列出哪些 dataTable 欄位是DB上面同名的Column
            // 排除 identity,compute,primary key 欄位
            var intersect_DBCols = from dbCol in dbColumns
                                   where source.Columns.Contains(dbCol.Name)
                                        && !dbCol.isIdentity && !dbCol.isComputed && !dbCol.isPrimaryKey
                                   select dbCol;

            // 將columnName 轉成update指令用的語法:
            // 例如 id = @id 這樣
            var intersect_Col_Names = from dbCol in intersect_DBCols
                                      select dbCol.Name + " = " + (dbCol.defaultValue != null ? $"isnull(@{dbCol.Name}, {(dbCol.defaultValue == string.Empty ? "''" : dbCol.defaultValue)})" : "@" + dbCol.Name);
            // 列出 pk column 擺在where的部分
            var pk_Cols = from dbCol in dbColumns
                          where dbCol.isPrimaryKey
                          select dbCol;
            var pk_Names = from dbCol in pk_Cols
                           select dbCol.Name + " = @" + dbCol.Name;

            // 組TSQL
            StringBuilder sb = new StringBuilder();
            sb.Append("update ").Append(tableName)
                .Append(" set ").Append(string.Join(", ", intersect_Col_Names))
                .Append(" where ").Append(string.Join(" and ", pk_Names));

            // 產生 sql command物件
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            foreach (DBColumn dbCol in intersect_DBCols)
            {
                cmd.Parameters.Add("@" + dbCol.Name, dbCol.Type, dbCol.Size, dbCol.Name);
            }

            foreach (DBColumn dbCol in pk_Cols)
            {
                cmd.Parameters.Add("@" + dbCol.Name, dbCol.Type, dbCol.Size, dbCol.Name);
            }

            return cmd;
        }

        /// <summary>
        /// 利用已經暫存好的Table Columns設定拼出 Delete Command
        /// <para> 使用到的column 只有 primary key </para>
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="source"></param>
        /// <param name="dbColumns"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static SqlCommand createDeleteCommand(string tableName, DataTable source, List<DBColumn> dbColumns, SqlConnection conn)
        {
            // 列出 pk column 擺在where的部分
            var pk_Cols = from dbCol in dbColumns
                          where dbCol.isPrimaryKey
                          select dbCol;
            var pk_Names = from dbCol in pk_Cols
                           select dbCol.Name + " = @" + dbCol.Name;

            // 組TSQL
            StringBuilder sb = new StringBuilder();
            sb.Append("delete ").Append(tableName)
                .Append(" where ").Append(string.Join(" and ", pk_Names));

            // 產生 sql command物件
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            foreach (DBColumn dbCol in pk_Cols)
            {
                // 刪除掉的 DataRow只能抓到 original 的欄位
                cmd.Parameters
                    .Add("@" + dbCol.Name, dbCol.Type, dbCol.Size, dbCol.Name)
                    .SourceVersion = DataRowVersion.Original;
            }

            return cmd;
        }

        /// <summary>
        /// update DataTable to DB by DataRow.RowState
        /// <para>RowState.Added    --> Insert </para>
        /// <para>RowState.Modified --> Update </para>
        /// <para>RowState.Deleted  --> Delete </para>
        /// <para>RowState.Detached --> 還沒寫 </para>
        /// <para>RowState.Unchanged--> 還沒寫 </para>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static bool TableUpdate(DataTable data, string tableName, object connection = null)
        {
            SqlConnection sqlConnection = null;               // SQL Connection
            bool isUpdateOK;                               // 記錄是否成功
            bool innerConnected = false;
            bool connectOK = false;
            #region -- Create Connection
            if (connection is string)
            {
                connectOK = SQL.GetConnection(out sqlConnection, (string)connection);
                innerConnected = true;
            }
            else if (connection == null)
            {
                // 如果傳入的是 SqlConnection 物件, 但卻是 null
                connectOK = SQL.GetConnection(out sqlConnection);
                innerConnected = true;
            }
            else if (connection is SqlConnection && ((SqlConnection)connection).State == ConnectionState.Closed)
            {
                sqlConnection = (SqlConnection)connection;
                sqlConnection.Open();
                connectOK = true;
            }

            if (!connectOK)
            {
                return connectOK;
            }
            #endregion

            List<Sci.DB.DBColumn> dbColumns = DBProxy.GetTableColumns(tableName, sqlConnection);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = SQL.createInsertCommand(tableName, data, dbColumns, sqlConnection);
            adapter.UpdateCommand = SQL.createUpdateCommand(tableName, data, dbColumns, sqlConnection);
            adapter.DeleteCommand = SQL.createDeleteCommand(tableName, data, dbColumns, sqlConnection);

            try
            {
                // for Delete
                DataTable deletes = data.GetChanges(DataRowState.Deleted);
                if (null != deletes && 0 != deletes.Rows.Count)
                {
                    adapter.Update(deletes);
                }

                // for insert & update
                DataTable upserts = data.GetChanges(DataRowState.Modified | DataRowState.Added);
                if (null != upserts && 0 != upserts.Rows.Count)
                {
                    adapter.Update(upserts);
                }

                isUpdateOK = true;
            }
            catch (Exception ex)
            {
                new BugProc(ex).Show();
                isUpdateOK = false;
            }

            if (innerConnected)
            {
                sqlConnection.Close();
            }

            return isUpdateOK;
        }

        #endregion

        public static int tmpTableCount = 0;
        public static readonly string sql_MergeInsertUpdateDelete = @"
{0} ;
MERGE {1} AS t
USING {2} AS s
ON ( {3} )
WHEN MATCHED {4} THEN UPDATE SET 
	{5} 
WHEN NOT MATCHED BY TARGET {6} THEN
	{7}
WHEN NOT MATCHED BY SOURCE {8} THEN
	DELETE
	{9}
;
{10}
";

        public static readonly string sql_MergeInsertUpdate = @"
{0} ;
MERGE {1} AS t
USING {2} AS s
ON ( {3} )
WHEN MATCHED {4} THEN UPDATE SET 
	{5} 
WHEN NOT MATCHED BY TARGET {6} THEN
	{7}
	{8}
;
{9}
";

        public static readonly string sql_MergeInsert = @"
{0} ;
MERGE {1} AS t
USING {2} AS s
ON ( {3} )
WHEN NOT MATCHED BY TARGET {4} THEN 
	{5}
	{6}
;
{7}
";
        #region --- Class Fields ----

        public enum MergeOption
        {
            /// <summary> 只作Insert into (not Merge) </summary>
            Insert = 1,

            /// <summary> 使用Merge 作Insert / update </summary>
            InsertUpdate = 2,

            /// <summary> 先 Delete (not merge) , 再使用Merge 作Insert / update </summary>
            InsertUpdate_SafeDelete = 4
        }

        bool writeDebugLog = false;
        string connectionName = null;
        public SqlConnection conn = null;
        SqlTransaction trans = null;
        public SqlBulkCopy bulkCopy = null;
        Sci.DB.DBTable tableStructure = null;
        string tmpTableNamePrefix = "#tmpBlk_";
        string tmpTableName = null;
        bool isTmpTableSelfCreatedAndDrop = false; // 辨識 tmpTable 是否內部產生, 並要自己drop清掉
        string destinationTableName = null;
        DataTable sourceTable = null;
        string Cmd_CreateTmpTable = null;
        string columns_CreateTmpTable = null;
        string columns_MatchUpdate = null;
        string excludes_TmpTable = null;
        string includes_TmpTable = null;
        string excludes_Insert = null;
        string includes_Insert = null;
        string excludes_Update = null;
        string includes_Update = null;
        string identity = null;
        Dictionary<String, String> tableMapping = new Dictionary<String, String>(StringComparer.OrdinalIgnoreCase);
        string Cmd_Merge = null;
        string Cmd_beforeMerge = null;
        string mergeOn = null;
        string mergeOnColumns = null;
        DBIndex mergeOnIndex = null;
        string matchedAndFilter = null;
        string matchedThenCmdType = null; // insert / update / delete 
        string matchedThen = null;
        string notMatchedByTargetAndFilter = null;
        string notMatchedThenCmdType = null; // insert / update / delete 
        string notMatchedByTargetThen = null;
        string Cmd_output = null;
        string Cmd_afterMerge = null;
        string Cmd_InsertTargetTableByTmp = null;
        IList<SqlParameter> parameters = null;
        bool hasAddDate = false;
        bool hasAddName = false;
        bool hasEditDate = false;
        bool hasEditName = false;
        bool showBugProc = true;
        const string parameter_userID = "@param_BulkUpdatingUserID";
        #endregion
        /////////////////////////////////////////
        #region --- Class Methods ----

        public SQL() { }
        public SQL SetWriteDebugLog(bool need) { this.writeDebugLog = need; return this; }
        public SQL SetConnection(string connectionName) { this.connectionName = connectionName; return this; }
        public SQL SetConnection(SqlConnection conn) { this.conn = conn; return this; }
        public SQL SetTransaction(SqlTransaction trans) { this.trans = trans; return this; }
        public SQL SetSqlBulkCopy(SqlBulkCopy bulkCopy) { this.bulkCopy = bulkCopy; return this; }

        public SQL SetTableStructure(string structureKeyword)
        {
            this.SetTableStructure(DBTable.GetByKeyword(structureKeyword));
            return this;
        }

        public SQL SetTableStructure(string TableName, string connectionName = null, string newSelectTableCmd = null)
        {
            return this.SetTableStructure(SQL.GetDBTable(TableName, connectionName, newSelectTableCmd));
        }

        public SQL SetTableStructure(DBTable structure)
        {
            this.tableStructure = structure;
            if (this.destinationTableName == null) { this.destinationTableName = structure.name; }

            // 判斷若沒有 MergeOn條件或是index , 就先用 table primary key取代
            if (null == this.mergeOn && null == this.mergeOnIndex) { this.SetMergeOnIndex(structure.primaryKey); }
            return this;
        }

        public SQL SetDestinationTableName(string destinationTableName) { this.destinationTableName = destinationTableName; return this; }
        public SQL SetSourceTable(DataTable sourceTable) { this.sourceTable = sourceTable; return this; }
        public SQL SetTmpTableName(string tmpTableName) { this.tmpTableName = tmpTableName; return this; }
        public SQL SetCreateTmpTableCmd(string createTmpTableCmd) { this.Cmd_CreateTmpTable = createTmpTableCmd; return this; }
        public SQL SetColumnExclude_TmpTable(string excludes_TmpTable) { this.excludes_TmpTable = excludes_TmpTable; return this; }
        public SQL SetColumnInclude_TmpTable(string includes_TmpTable) { this.includes_TmpTable = includes_TmpTable; return this; }

        public SQL SetColumnExclude_Insert(string excludes_Insert) { this.excludes_Insert = excludes_Insert; return this; }
        public SQL SetColumnInclude_Insert(string includes_Insert) { this.includes_Insert = includes_Insert; return this; }
        public SQL SetColumnExclude_Update(string excludes_Update) { this.excludes_Update = excludes_Update; return this; }
        public SQL SetColumnInclude_Update(string includes_Update) { this.includes_Update = includes_Update; return this; }
        public SQL SetColumnsIdentity(string identityColumnName) { this.identity = identityColumnName; return this; }
        public SQL SetCmd_Merge(string mergeCmd) { this.Cmd_Merge = mergeCmd; return this; }
        public SQL SetMergeOn(string mergeOn) { this.mergeOn = mergeOn; return this; }
        public SQL SetMergeOnColumns(string mergeOnColumns)
        {
            this.mergeOnColumns = mergeOnColumns;
            string[] arr = mergeOnColumns.Split(this.commaSplit, StringSplitOptions.RemoveEmptyEntries);
            bool isFirst = true;
            StringBuilder sb = new StringBuilder();
            foreach (string col in arr)
            {
                if (isFirst) { sb.Append("t.").Append(col.Trim()).Append(" = s.").Append(col.Trim()); }
                else { sb.Append(" and t.").Append(col.Trim()).Append(" = s.").Append(col.Trim()); }
                isFirst = false;
            }

            this.mergeOn = sb.ToString();
            return this;
        }

        public SQL SetMergeOnIndex(DBIndex idx)
        {
            this.mergeOnIndex = idx;

            // Merge的 on 條件可根據 table index 設定 , 預設是 table primaryKey
            this.mergeOnColumns = string.Join(",", this.mergeOnIndex.Columns); // 轉成string of " A,B,C"
            List<string> criteria = new List<string>();
            this.mergeOnIndex.Columns.ForEach(colName => criteria.Add("t." + colName + " = s." + colName));
            this.mergeOn = string.Join(" and ", criteria);  // 轉成string of " t.A=s.A and t.B=s.B and t.C=s.C "
            return this;
        }

        public SQL SetMatchedAndFilter(string matchedAndFilter) { this.matchedAndFilter = matchedAndFilter; return this; }
        public SQL SetMatchedThen(string matchedThen) { this.matchedThen = matchedThen; return this; }
        public SQL SetNotMatchedByTargetAndFilter(string notMatchedAndFilter) { this.notMatchedByTargetAndFilter = notMatchedAndFilter; return this; }
        public SQL SetNotMatchedByTargetThen(string notMatchedThen) { this.notMatchedByTargetThen = notMatchedThen; return this; }
        public SQL SetCmd_Output(string outputCmd) { this.Cmd_output = outputCmd; return this; }
        public SQL SetCmd_beforeMerge(string cmd_beforeMerge) { this.Cmd_beforeMerge = cmd_beforeMerge; return this; }
        public SQL SetCmd_afterMerge(string cmd_afterMerge) { this.Cmd_afterMerge = cmd_afterMerge; return this; }
        public SQL SetParameters(IList<SqlParameter> parameters) { this.parameters = parameters; return this; }

        public SQL Reset()
        {
            tmpTableName = null;
            isTmpTableSelfCreatedAndDrop = false;
            destinationTableName = null;
            tableStructure = null;
            sourceTable = null;
            Cmd_CreateTmpTable = null;
            columns_CreateTmpTable = null;
            columns_MatchUpdate = null;
            excludes_TmpTable = null;
            includes_TmpTable = null;
            excludes_Insert = null;
            includes_Insert = null;
            excludes_Update = null;
            includes_Update = null;
            identity = null;
            tableMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Cmd_Merge = null;
            Cmd_beforeMerge = null;
            mergeOn = null;
            mergeOnIndex = null;
            matchedAndFilter = null;
            matchedThenCmdType = null; // insert / update / delete 
            matchedThen = null;
            notMatchedByTargetAndFilter = null;
            notMatchedThenCmdType = null; // insert / update / delete 
            notMatchedByTargetThen = null;
            Cmd_output = null;
            Cmd_afterMerge = null;
            Cmd_InsertTargetTableByTmp = null;
            parameters = null;

            return this;
        }

        public SQL Reset_Del()
        {
            tmpTableName_Del = null;
            isTmpTableSelfCreatedAndDrop_Del = false;
            columns_CreateTmpTable_Del = null;
            Cmd_CreateTmpTable_Del = null;
            Cmd_DelTargetTableByTmp = null;
            parameters = null;

            return this;
        }


        public void Check_AddEdit_Columns()
        {
            this.hasAddDate = this.tableStructure.columns.Keys.Any(name => name.Equals("AddDate", StringComparison.OrdinalIgnoreCase));
            this.hasAddName = this.tableStructure.columns.Keys.Any(name => name.Equals("AddName", StringComparison.OrdinalIgnoreCase));
            this.hasEditDate = this.tableStructure.columns.Keys.Any(name => name.Equals("EditDate", StringComparison.OrdinalIgnoreCase));
            this.hasEditName = this.tableStructure.columns.Keys.Any(name => name.Equals("EditName", StringComparison.OrdinalIgnoreCase));

            if (this.hasAddName || this.hasEditName)
            {
                if (null == this.parameters) { this.parameters = new List<SqlParameter>(); }
                if (!this.parameters.Any(par => par.ParameterName == parameter_userID))
                {
                    this.parameters.Add(new SqlParameter(parameter_userID, Env.User.UserID));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="original"></param>
        /// <param name="filter"></param>
        /// <param name="includeOrNot">true: include ; false: exclude</param>
        /// <param name="setBulkMapping"></param>
        /// <returns></returns>
        public string getFilteredColumns(string original, string filter, bool includeOrNot, bool setBulkMapping = false)
        {
            string[] ori_Array = original.ToUpper().Split(',');
            if (null == filter)
            {
                filter = string.Empty;
            }

            string[] filter_Array = filter.ToUpper().Split(',');
            IEnumerable<string> result;
            if (includeOrNot)
            {
                result = ori_Array.Intersect(filter_Array);
            }
            else
            {
                result = ori_Array.Except(filter_Array);
            }

            if (setBulkMapping)
            {
                bulkCopy.ColumnMappings.Clear();
                foreach (string field in result)
                {
                    bulkCopy.ColumnMappings.Add(field, field);
                }
            }

            return string.Join(",", result);
        }

        private string Trim_IdentiryColumn(string original)
        {
            // 判斷使用 tableStructure 或是 user 指定的 identity 
            string identityCol = null != this.tableStructure && null != this.tableStructure.identity
                ? this.tableStructure.identity.Name
                : null == this.identity ? null : this.identity;

            return null == identityCol
                ? original
                : this.getFilteredColumns(original, identityCol, false, setBulkMapping: false);
        }

        private string[] commaSplit = new string[] { "," };

        private string Trim_notTableStructureColumns(string original)
        {
            string[] columns = original.Split(commaSplit, StringSplitOptions.RemoveEmptyEntries);
            List<string> result = new List<string>();
            bulkCopy.ColumnMappings.Clear();
            if (0 != this.tableMapping.Count)
            {
                // 先用user 指定的 tableColumnMapping 判斷是否要替換 column name
                // 再用tableStructure 檢查是否為正確的 column Name
                foreach (string tmpColumn in columns)
                {
                    if (this.tableMapping.ContainsKey(tmpColumn))
                    {
                        string replaceToName = this.tableMapping[tmpColumn];
                        if (this.tableStructure.containsColumn(replaceToName))
                        {
                            bulkCopy.ColumnMappings.Add(tmpColumn, replaceToName);
                            ////columns[idx] = replaceToName;
                            result.Add(replaceToName);
                        }
                    }
                    else
                    {
                        if (this.tableStructure.containsColumn(tmpColumn))
                        {
                            bulkCopy.ColumnMappings.Add(tmpColumn, tmpColumn);
                            result.Add(tmpColumn);
                        }
                    }
                }
            }
            else
            {
                // 單用tableStructure 檢查是否為正確的 column Name
                foreach (string tmpColumn in columns)
                {
                    if (this.tableStructure.containsColumn(tmpColumn))
                    {
                        bulkCopy.ColumnMappings.Add(tmpColumn, tmpColumn);
                        result.Add(tmpColumn);
                    }
                }
            }

            return string.Join(",", result);
        }

        private string GetInsertCmd()
        {
            this.Check_AddEdit_Columns();
            string col_InsertInto = (null == this.identity ? this.columns_CreateTmpTable : this.getFilteredColumns(this.columns_CreateTmpTable, this.identity, false, setBulkMapping: true));
            if (null != this.excludes_Insert || null != this.includes_Insert)
            {
                if (null != this.excludes_Insert)
                {
                    col_InsertInto = this.getFilteredColumns(col_InsertInto, this.excludes_Insert, true);
                }
                else
                {
                    col_InsertInto = this.getFilteredColumns(col_InsertInto, this.includes_Insert, true);
                }
            }

            if (this.hasAddDate || this.hasAddName)
            {
                col_InsertInto = this.getFilteredColumns(col_InsertInto, "AddDate,AddName", false);
            }

            StringBuilder selectCmd = new StringBuilder();
            string[] columns = col_InsertInto.Split(this.commaSplit, StringSplitOptions.RemoveEmptyEntries);
            var cols = this.tableStructure.columns;
            for (int idx = 0; idx < columns.Length; idx++)
            {
                var col = columns[idx].Trim();
                var defaultValue = cols[col].defaultValue;

                if (idx > 0)
                {
                    selectCmd.Append(", ");
                }

                if (defaultValue != null)
                {
                    if (defaultValue == string.Empty)
                    {
                        defaultValue = "''";
                    }

                    selectCmd.Append($"isnull({col}, {defaultValue})");
                }
                else
                {
                    selectCmd.Append($"{col}");
                }
            }

            string cols_Selects = selectCmd.ToString();

            if (this.hasAddDate)
            {
                col_InsertInto += string.IsNullOrWhiteSpace(col_InsertInto)
                    ? "AddDate"
                    : ",AddDate";
                cols_Selects += string.IsNullOrWhiteSpace(cols_Selects)
                    ? "GetDate() as AddDate"
                    : ",GetDate() as AddDate";
            }

            if (this.hasAddName)
            {
                col_InsertInto += string.IsNullOrWhiteSpace(col_InsertInto)
                    ? "AddName"
                    : ",AddName";
                cols_Selects += string.IsNullOrWhiteSpace(col_InsertInto) ? string.Empty : $", isnull({parameter_userID}, '') as AddName";
            }

            return new StringBuilder()
                .AppendFormat(@"insert into {0} ( {1} ) select {2} from {3} ;"
                        , this.destinationTableName
                        , col_InsertInto
                        , cols_Selects
                        , this.tmpTableName)
                .Append(this.isTmpTableSelfCreatedAndDrop
                        ? "drop table " + this.tmpTableName
                        : string.Empty
                ).ToString();
            /*
            return "insert into " + this.destinationTableName + "(" + result + ") select " + result + " from " + this.tmpTableName;*/
        }

        private string GetMergeInsertNotMatchCmd()
        {
            string result = this.Trim_IdentiryColumn(this.columns_CreateTmpTable);
            if (null != this.excludes_Insert || null != this.includes_Insert)
            {
                if (null != this.excludes_Insert)
                {
                    result = this.getFilteredColumns(result, this.excludes_Insert, false);
                }
                else
                {
                    result = this.getFilteredColumns(result, this.includes_Insert, false);
                }
            }

            // 先判斷移除AddDate,AddName 這些欄位
            if (this.hasAddDate || this.hasAddName)
            {
                result = this.getFilteredColumns(result, "AddDate,AddName", false);
            }

            /*
            組insert 指令,
            像 insert (a,b,AddDate,AddName) values( s.a ,s.b,GetDate,@param_BulkUpdatingUserID) 這樣
            */
            string[] columns = result.Split(this.commaSplit, StringSplitOptions.RemoveEmptyEntries);
            var cols = this.tableStructure.columns;
            StringBuilder insertCmd = new StringBuilder();
            insertCmd.Append("Insert (");

            for (int idx = 0; idx < columns.Length; idx++)
            {
                if (idx > 0)
                {
                    insertCmd.Append(", ");
                }

                insertCmd.Append(columns[idx].Trim());
            }

            if (this.hasAddDate)
            {
                if (columns.Length > 0)
                {
                    insertCmd.Append(",");
                }

                insertCmd.Append("AddDate");
            }

            if (this.hasAddName)
            {
                if (columns.Length > 0 || this.hasAddDate)
                {
                    insertCmd.Append(",");
                }

                insertCmd.Append("AddName");
            }

            insertCmd.Append(") values(");

            // StringBuilder insertValues = new StringBuilder();
            for (int idx = 0; idx < columns.Length; idx++)
            {
                var col = columns[idx].Trim();
                var defaultValue = cols[col].defaultValue;

                if (idx > 0)
                {
                    insertCmd.Append(", ");
                }

                if (defaultValue != null)
                {
                    if (defaultValue == string.Empty)
                    {
                        defaultValue = "''";
                    }

                    insertCmd.Append($"isnull(s.{col}, {defaultValue})");
                }
                else
                {
                    insertCmd.Append($"s.{col}");
                }
            }

            if (this.hasAddDate)
            {
                if (columns.Length > 0)
                {
                    insertCmd.Append(",");
                }

                insertCmd.Append("GetDate()");
            }

            if (this.hasAddName)
            {
                if (columns.Length > 0 || this.hasAddDate)
                {
                    insertCmd.Append(",");
                }

                insertCmd.Append($"isnull({parameter_userID}, '')");
            }

            insertCmd.Append(") ");

            return insertCmd.ToString();
            ////return "insert (" + result + ") values(s." + result.Replace(",", " ,s.") +")";
        }

        private string GetMergeUpdateMatchCmd()
        {
            string result = this.columns_CreateTmpTable;

            // update 指令應 Trim掉 identity 還有 merge on 的條件欄位
            if (null != this.identity)
            {
                result = this.getFilteredColumns(result, this.identity, false);
            }

            if (null != this.mergeOnColumns)
            {
                result = this.getFilteredColumns(result, this.mergeOnColumns, false);
            }

            // 再 trim 掉 user指定的欄位
            if (null != this.excludes_Update || null != this.includes_Update)
            {
                if (null != this.excludes_Update)
                {
                    result = this.getFilteredColumns(result, this.excludes_Update, false);
                }
                else
                {
                    result = this.getFilteredColumns(result, this.includes_Update, true);
                }
            }

            // 先判斷移除EditDate,EditName 這些欄位
            if (this.hasEditDate || this.hasEditName)
            {
                result = this.getFilteredColumns(result, "EditDate,EditName", false);
            }

            /*
            組 update指令部分 ,
            像 set t.Qty = s.Qty ,t.Price = s.Price, t.EditDate = GetDate(),t.EditName = @param_BulkUpdatingUserID 這樣
            */
            string[] columns = result.Split(this.commaSplit, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();

            var cols = this.tableStructure.columns;

            for (int idx = 0; idx < columns.Length; idx++)
            {
                var col = columns[idx].Trim();
                var defaultValue = cols[col].defaultValue;

                if (idx > 0)
                {
                    sb.Append(", ");
                }

                if (defaultValue != null)
                {
                    if (defaultValue == string.Empty)
                    {
                        defaultValue = "''";
                    }

                    sb.Append($"t.{col} = isnull(s.{col}, {defaultValue})");
                }
                else
                {
                    sb.Append($"t.{col} = s.{col}");
                }
            }

            if (this.hasEditDate)
            {
                if (columns.Length > 0)
                {
                    sb.Append(",");
                }

                sb.Append(" t.EditDate = GetDate() ");
            }

            if (this.hasEditName)
            {
                if (columns.Length > 0 || this.hasAddDate)
                {
                    sb.Append(",");
                }

                sb.Append($" t.EditName = isnull({parameter_userID}, '')");
            }

            return sb.ToString();
        }

        /// <summary>
        ///  依據各種 option 重新整理正確的參數 & TSQL 指令去做 Merge
        /// </summary>
        /// <param name="mOption"></param>
        public string FinalizeForMerge(MergeOption mOption)
        {
            if (this.tableStructure == null)
            {
                return "SQL.tableStructure is null (Sci.DB.DBTable)";
            }

            if (null == this.mergeOn)
            {
                return "SQL.mergOn are null, please at least setup one of them.";
            }

            if (this.sourceTable == null)
            {
                return "SQL.sourceTable is null.";
            }

            if (this.destinationTableName == null)
            {
                return "SQL.destinationTableName is null.";
            }

            if (this.conn == null)
            {
                DBProxy.Current.OpenConnection(this.connectionName, out this.conn);
            }

            if (this.bulkCopy == null)
            {
                this.bulkCopy = new SqlBulkCopy(this.conn);
            }

            /////////////////////////////////////////////////////
            // bulk copy relateds , insert data to #Temp Table
            if (this.tmpTableName == null)
            {
                this.tmpTableName = this.tmpTableNamePrefix + (SQL.tmpTableCount++);
                this.isTmpTableSelfCreatedAndDrop = true;
            }

            this.bulkCopy.DestinationTableName = this.tmpTableName;
            this.bulkCopy.ColumnMappings.Clear();
            if (this.columns_CreateTmpTable == null)
            {
                this.columns_CreateTmpTable = GetAllFields(this.sourceTable);
                this.columns_CreateTmpTable = this.Trim_notTableStructureColumns(this.columns_CreateTmpTable);
                if (this.excludes_TmpTable != null || this.includes_TmpTable != null)
                {
                    if (this.excludes_TmpTable != null)
                    {
                        this.columns_CreateTmpTable = this.getFilteredColumns(this.columns_CreateTmpTable, this.excludes_TmpTable, false, setBulkMapping: true);
                    }
                    else
                    {
                        this.columns_CreateTmpTable = this.getFilteredColumns(this.columns_CreateTmpTable, this.includes_TmpTable, true, setBulkMapping: true);
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(this.columns_CreateTmpTable))
            {
                return "SQL.columns_CreateTmpTable is empty.";
            }

            if (this.Cmd_CreateTmpTable == null)
            {
                // select id, Cast(0 as BigInt) as ukey,poid into #Tmp_1 from dbo.Export_Detail where 1=2
                string[] cols = this.columns_CreateTmpTable.Split(commaSplit, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder cmdSB = new StringBuilder("select ");
                for (int i = 0; i < cols.Length; i++)
                {
                    cmdSB.Append(i == 0 ? string.Empty : ",")
                        .Append(this.tableStructure.isIdentity(cols[i])
                            ? "Cast(0 as bigInt) as " + cols[i]
                            : cols[i]);
                }

                cmdSB.Append(" into ").Append(this.tmpTableName).Append(" from ").Append(this.destinationTableName).Append(" where 1=2 ");

                // 使TmpTable的欄位允許null
                cmdSB.Append("union select ");
                for (int i = 0; i < cols.Length; i++)
                {
                    cmdSB.Append(i == 0 ? string.Empty : ",").Append("null");
                }

                cmdSB.Append(" delete ").Append(this.tmpTableName);

                this.Cmd_CreateTmpTable = cmdSB.ToString();
            }

            /////////////////////////////////////////////////////
            // Merge relateds , Merge #Temp table to DB Table
            this.Check_AddEdit_Columns();
            if (this.Cmd_Merge == null)
            {
                this.Cmd_beforeMerge = (null == this.Cmd_beforeMerge) ? string.Empty : this.Cmd_beforeMerge;
                this.Cmd_output = (null == this.Cmd_output) ? string.Empty : this.Cmd_output;
                this.Cmd_afterMerge = (null == this.Cmd_afterMerge) ? string.Empty : this.Cmd_afterMerge;

                ///////////////////////////////////////////////////
                //
                switch (mOption)
                {
                    case MergeOption.InsertUpdate:
                        // 組 Merge Cmd
                        this.matchedAndFilter = (this.matchedAndFilter == null) ? string.Empty : " and " + this.matchedAndFilter;

                        this.notMatchedByTargetAndFilter = (this.notMatchedByTargetAndFilter == null) ? string.Empty : this.notMatchedByTargetAndFilter;
                        if (string.IsNullOrWhiteSpace(this.matchedThen))
                        {
                            this.matchedThen = this.GetMergeUpdateMatchCmd();
                        }

                        if (string.IsNullOrWhiteSpace(this.notMatchedByTargetThen))
                        {
                            this.notMatchedByTargetThen = this.GetMergeInsertNotMatchCmd();
                        }

                        // GetInsertNotMatchCmd()
                        this.Cmd_Merge = string.Format(
                            SQL.sql_MergeInsertUpdate
                            , this.Cmd_beforeMerge, this.destinationTableName, this.tmpTableName, this.mergeOn
                            , this.matchedAndFilter, this.matchedThen
                            , this.notMatchedByTargetAndFilter, this.notMatchedByTargetThen
                            , this.Cmd_output, this.Cmd_afterMerge)
                            + (this.isTmpTableSelfCreatedAndDrop ? "drop table " + this.tmpTableName : string.Empty);
                        break;
                    case MergeOption.Insert:
                        // 組 Merge Cmd
                        this.notMatchedByTargetAndFilter = (this.notMatchedByTargetAndFilter == null) ? string.Empty : " and " + this.notMatchedByTargetAndFilter;
                        if (string.IsNullOrWhiteSpace(this.notMatchedByTargetThen))
                        {
                            this.notMatchedByTargetThen = this.GetMergeInsertNotMatchCmd();
                        }

                        this.Cmd_Merge = string.Format(SQL.sql_MergeInsert
                            , this.Cmd_beforeMerge, this.destinationTableName, this.tmpTableName, this.mergeOn
                            , this.notMatchedByTargetAndFilter, this.notMatchedByTargetThen
                            , this.Cmd_output, this.Cmd_afterMerge)
                            + (this.isTmpTableSelfCreatedAndDrop ? "drop table " + this.tmpTableName : string.Empty);
                        break;
                    default: break;
                }
            }

            /////////////////////////////////////////////////////
            ////
            if (this.writeDebugLog)
            {
                this.LogBulkMappingString();
            }

            return null;
        }

        private string FinalizeForInsert()
        {
            if (this.sourceTable == null)
            {
                return "SQL.sourceTable is null.";
            }

            if (this.destinationTableName == null)
            {
                return "SQL.destinationTableName is null.";
            }

            if (this.conn == null)
            {
                DBProxy.Current.OpenConnection(this.connectionName, out this.conn);
            }

            if (this.bulkCopy == null)
            {
                this.bulkCopy = new SqlBulkCopy(this.conn);
            }

            /////////////////////////////////////////////////////
            // bulk copy relateds.
            if (this.tmpTableName == null)
            {
                this.tmpTableName = this.tmpTableNamePrefix + (SQL.tmpTableCount++);
                this.isTmpTableSelfCreatedAndDrop = true;
            }

            this.bulkCopy.DestinationTableName = this.tmpTableName;
            if (this.columns_CreateTmpTable == null)
            {
                this.columns_CreateTmpTable = GetAllFields(this.sourceTable);
                this.columns_CreateTmpTable = this.Trim_IdentiryColumn(this.columns_CreateTmpTable);
                this.columns_CreateTmpTable = this.Trim_notTableStructureColumns(this.columns_CreateTmpTable);

                if (null != this.excludes_TmpTable || null != this.includes_TmpTable)
                {
                    if (null != this.excludes_TmpTable)
                    {
                        this.columns_CreateTmpTable = this.getFilteredColumns(this.columns_CreateTmpTable, this.excludes_TmpTable, false, setBulkMapping: true);
                    }
                    else
                    {
                        this.columns_CreateTmpTable = this.getFilteredColumns(this.columns_CreateTmpTable, this.includes_TmpTable, true, setBulkMapping: true);
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(this.columns_CreateTmpTable)) { return "SQL.columns_CreateTmpTable is empty."; }
            if (null == this.Cmd_CreateTmpTable)
            {
                this.Cmd_CreateTmpTable = "select " + this.columns_CreateTmpTable + " into " + this.tmpTableName + " from " + this.destinationTableName + " where 1=2";

                List<string> listNull = new List<string>();
                for (int i = 0; i < this.columns_CreateTmpTable.Split(',').Count(); i++)
                {
                    listNull.Add("Null");
                }

                this.Cmd_CreateTmpTable += " union select " + string.Join(",", listNull) + " delete " + this.tmpTableName;
            }

            /////////////////////////////////////////////////////
            // insert cmd related.
            if (string.IsNullOrWhiteSpace(this.Cmd_InsertTargetTableByTmp))
            {
                this.Cmd_InsertTargetTableByTmp =
                    (null == this.Cmd_beforeMerge ? string.Empty : this.Cmd_beforeMerge + Environment.NewLine)
                    + this.GetInsertCmd()
                    + (null == this.Cmd_afterMerge ? string.Empty : Environment.NewLine + this.Cmd_afterMerge)
                    ;
            }

            /////////////////////////////////////////////////////
            //
            // String[] fields = this.columns_CreateTmpTable.Split(',');
            // foreach(var f in fields){this.bulkCopy.ColumnMappings.Add(f,f);}

            //this.sourceTable.Columns.Remove
            if (this.writeDebugLog)
            {
                this.LogBulkMappingString();
            }

            return null;
        }

        string tmpTableName_Del = null;
        bool isTmpTableSelfCreatedAndDrop_Del = false;
        string columns_CreateTmpTable_Del = null;
        string Cmd_CreateTmpTable_Del = null;
        string Cmd_DelTargetTableByTmp = null;

        public string FinalizeForDelete()
        {
            if (this.sourceTable == null) { return "SQL.sourceTable is null."; }
            if (this.tableStructure == null) { return "SQL.tableStructure is null (Sci.DB.DBTable)"; }
            if (this.destinationTableName == null) { return "SQL.destinationTableName is null."; }
            if (this.conn == null) { DBProxy.Current.OpenConnection(this.connectionName, out this.conn); }
            if (this.bulkCopy == null) { this.bulkCopy = new SqlBulkCopy(this.conn); }

            /////////////////////////////////////////////////////
            // bulk copy relateds.
            if (this.tmpTableName_Del == null)
            {
                this.tmpTableName_Del = this.tmpTableNamePrefix + (SQL.tmpTableCount++);
                this.isTmpTableSelfCreatedAndDrop_Del = true;
            }

            this.bulkCopy.DestinationTableName = this.tmpTableName_Del;
            if (this.columns_CreateTmpTable_Del == null)
            {
                this.columns_CreateTmpTable_Del = string.Join(",", this.mergeOnIndex.Columns);
            }

            if (string.IsNullOrWhiteSpace(this.columns_CreateTmpTable_Del)) { return "SQL.columns_CreateTmpTable_Del is empty."; }

            // 重新設定 column mapping
            this.bulkCopy.ColumnMappings.Clear();
            string[] cols = this.columns_CreateTmpTable_Del.Split(commaSplit, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < cols.Length; i++)
            {
                this.bulkCopy.ColumnMappings.Add(cols[i], cols[i]);
            }

            if (this.Cmd_CreateTmpTable_Del == null)
            {
                // select id, Cast(0 as BigInt) as ukey,poid into #Tmp_1 from dbo.Export_Detail where 1=2
                StringBuilder cmdSB = new StringBuilder("select ");
                for (int i = 0; i < cols.Length; i++)
                {
                    cmdSB.Append(i == 0 ? string.Empty : ",")
                        .Append(this.tableStructure.isIdentity(cols[i])
                            ? "Cast(0 as bigInt) as " + cols[i]
                            : cols[i]);
                }

                this.Cmd_CreateTmpTable_Del = cmdSB.Append(" into ").Append(this.tmpTableName_Del).Append(" from ").Append(this.destinationTableName).Append(" where 1=2 ; ").ToString();
            }

            /////////////////////////////////////////////////////
            // insert cmd related.
            if (this.Cmd_DelTargetTableByTmp == null)
            {
                this.Cmd_DelTargetTableByTmp = string.Format(
@"DELETE t
FROM {0} t
INNER JOIN {1} s ON {2}",
                    this.destinationTableName,
                    this.tmpTableName_Del,
                    this.mergeOn);
            }

            if (this.writeDebugLog)
            {
                this.LogBulkMappingString();
            }

            return null;
        }

        private void LogBulkMappingString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (SqlBulkCopyColumnMapping map in this.bulkCopy.ColumnMappings)
            {
                sb.AppendFormat("[{0},{1}]", map.SourceColumn, map.DestinationColumn);
            }

            Ict.Logs.UI.LogInfo("SqlBulkCopyColumnMapping : ", sb.ToString());
        }

        #endregion

        /////////////////////////////////////////

        #region Bulk & Merge Methods
        private static string GetExceptionMsg(Exception ex, SqlBulkCopy bulkCopy)
        {
            if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
            {
                string pattern = @"\d+";
                Match match = Regex.Match(ex.Message.ToString(), pattern);
                var index = Convert.ToInt32(match.Value) - 1;

                FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
                var sortedColumns = fi.GetValue(bulkCopy);
                var items = (object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

                FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                var metadata = itemdata.GetValue(items[index]);

                var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                return Environment.NewLine + $"  Column: {column} contains data with a length greater than: {length}";
            }

            return string.Empty;
        }

        private static DualResult Mute_GoMerge(SQL sql)
        {
            // 先產生一個作Bulk Copy用的 #tmp Table
            if (sql.writeDebugLog)
            {
                Ict.Logs.UI.LogInfo("Merge - Create #tmp Table ...", sql.Cmd_CreateTmpTable);
            }

            SqlCommand cmd = new SqlCommand(sql.Cmd_CreateTmpTable, sql.conn);
            if (sql.trans != null)
            {
                cmd.Transaction = sql.trans;
            }

            try
            {
                cmd.ExecuteNonQuery();

                // 把 sourceTable 匯入到 tempTable 裡
                sql.bulkCopy.WriteToServer(sql.sourceTable);
            }
            catch (Exception e)
            {
                string bulkMsg = GetExceptionMsg(e, sql.bulkCopy);
                DualResult fails = Result.F($"Fail to Merge to [{sql.destinationTableName}] {bulkMsg}", e);
                return fails;
            }

            // 用 tempTable merge回 DB
            if (sql.writeDebugLog)
            {
                Ict.Logs.UI.LogInfo("Merge - Execute  ...", sql.Cmd_Merge);
            }

            return DBProxy.Current.ExecuteByConn(sql.conn, sql.Cmd_Merge, sql.parameters);
        }

        private static bool GoMerge(SQL sql)
        {
            // 先產生一個作Bulk Copy用的 #tmp Table
            if (sql.writeDebugLog)
            {
                Ict.Logs.UI.LogInfo("Merge - Create #tmp Table ...", sql.Cmd_CreateTmpTable);
            }

            SqlCommand cmd = new SqlCommand(sql.Cmd_CreateTmpTable, sql.conn);
            if (sql.trans != null)
            {
                cmd.Transaction = sql.trans;
            }

            try
            {
                cmd.ExecuteNonQuery();

                // 把 sourceTable 匯入到 tempTable 裡
                sql.bulkCopy.WriteToServer(sql.sourceTable);
            }
            catch (Exception e)
            {
                string bulkMsg = GetExceptionMsg(e, sql.bulkCopy);
                new BugProc(e, $"Fail to Merge to [{sql.destinationTableName}] {bulkMsg}")
                    .Show();
                return false;
            }

            // 用 tempTable merge回 DB
            if (sql.writeDebugLog)
            {
                Ict.Logs.UI.LogInfo("Merge - Execute  ...", sql.Cmd_Merge);
            }

            return SQL.Execute(sql.conn, sql.Cmd_Merge, sql.parameters);
        }

        private static bool GoMerge(SQL sql, out DataTable table)
        {
            table = null;

            // 先產生一個作Bulk Copy用的 #tmp Table
            if (sql.writeDebugLog)
            {
                Ict.Logs.UI.LogInfo("Merge - Create #tmp Table ...", sql.Cmd_CreateTmpTable);
            }

            SqlCommand cmd = new SqlCommand(sql.Cmd_CreateTmpTable, sql.conn);
            if (sql.trans != null)
            {
                cmd.Transaction = sql.trans;
            }

            try
            {
                cmd.ExecuteNonQuery();

                // 把 sourceTable 匯入到 tempTable 裡
                sql.bulkCopy.WriteToServer(sql.sourceTable);
            }
            catch (Exception e)
            {
                string bulkMsg = GetExceptionMsg(e, sql.bulkCopy);
                new BugProc(e, $"Fail to Merge to [{sql.destinationTableName}] {bulkMsg}")
                    .Show();
                return false;
            }

            // 用 tempTable merge回 DB
            if (sql.writeDebugLog)
            {
                Ict.Logs.UI.LogInfo("Merge - Execute  ...", sql.Cmd_Merge);
            }

            return SQL.Select(sql.conn, sql.Cmd_Merge, out table, sql.parameters);
        }

        private static DualResult Mute_GoInsert(SQL sql)
        {
            // 先產生一個作Bulk Copy用的 #tmp Table
            if (sql.writeDebugLog)
            {
                Ict.Logs.UI.LogInfo("Bulk Insert - Create #tmp Table ...", sql.Cmd_CreateTmpTable);
            }

            SqlCommand cmd = new SqlCommand(sql.Cmd_CreateTmpTable, sql.conn);
            if (sql.trans != null) { cmd.Transaction = sql.trans; }

            try
            {
                cmd.ExecuteNonQuery();

                // 把 sourceTable 匯入到 tempTable 裡
                sql.bulkCopy.WriteToServer(sql.sourceTable);
            }
            catch (Exception e)
            {
                string bulkMsg = GetExceptionMsg(e, sql.bulkCopy);
                DualResult fails = Result.F($"Fail to Merge to [{sql.destinationTableName}] {bulkMsg}", e);
                return fails;
            }

            // 用 tempTable merge回 DB
            if (sql.writeDebugLog)
            {
                Ict.Logs.UI.LogInfo("Bulk Insert -  ...", sql.Cmd_InsertTargetTableByTmp);
            }

            return SQL.Mute_Execute(sql.conn, sql.Cmd_InsertTargetTableByTmp, sql.parameters);
        }

        private static bool GoInsert(SQL sql)
        {
            // 先產生一個作Bulk Copy用的 #tmp Table
            if (sql.writeDebugLog)
            {
                Ict.Logs.UI.LogInfo("Bulk Insert - Create #tmp Table ...", sql.Cmd_CreateTmpTable);
            }

            SqlCommand cmd = new SqlCommand(sql.Cmd_CreateTmpTable, sql.conn);
            if (sql.trans != null)
            {
                cmd.Transaction = sql.trans;
            }

            try
            {
                cmd.ExecuteNonQuery();

                // 把 sourceTable 匯入到 tempTable 裡
                sql.bulkCopy.WriteToServer(sql.sourceTable);
            }
            catch (Exception e)
            {
                string bulkMsg = GetExceptionMsg(e, sql.bulkCopy);
                new BugProc(e, $"Fail to Merge to [{sql.destinationTableName}] {bulkMsg}")
                    .Show();
                return false;
            }

            // 用 tempTable merge回 DB
            if (sql.writeDebugLog)
            {
                Ict.Logs.UI.LogInfo("Bulk Insert -  ...", sql.Cmd_InsertTargetTableByTmp);
            }

            return SQL.Execute(sql.conn, sql.Cmd_InsertTargetTableByTmp, sql.parameters);
        }

        private static bool GoInsert(SQL sql, out DataTable table)
        {
            table = null;

            // 先產生一個作Bulk Copy用的 #tmp Table
            if (sql.writeDebugLog)
            {
                Ict.Logs.UI.LogInfo("Bulk Insert - Create #tmp Table ...", sql.Cmd_CreateTmpTable);
            }

            try
            {
                SqlCommand cmd = new SqlCommand(sql.Cmd_CreateTmpTable, sql.conn);
                cmd.ExecuteNonQuery();

                // 把 sourceTable 匯入到 tempTable 裡
                sql.bulkCopy.WriteToServer(sql.sourceTable);
            }
            catch (Exception e)
            {
                string bulkMsg = GetExceptionMsg(e, sql.bulkCopy);
                new BugProc(e, $"Fail to Merge to [{sql.destinationTableName}] {bulkMsg}")
                    .Show();
                return false;
            }

            // 用 tempTable merge回 DB
            if (sql.writeDebugLog)
            {
                Ict.Logs.UI.LogInfo("Bulk Insert -  ...", sql.Cmd_InsertTargetTableByTmp);
            }

            return SQL.Select(sql.conn, sql.Cmd_InsertTargetTableByTmp, out table, sql.parameters);
        }

        private static DualResult Mute_GoDelete(SQL sql, bool useDeletedStatus = true)
        {
            DataTable delRows;
            if (useDeletedStatus)
            {
                delRows = sql.sourceTable.GetChanges(DataRowState.Deleted);
                if (delRows == null || delRows.Rows.Count == 0)
                {
                    return Result.True;
                }

                delRows.RejectChanges(); // 將state 還原之後不會影響原Table的 State
            }
            else
            {
                delRows = sql.sourceTable;
                if (delRows == null || delRows.Rows.Count == 0)
                {
                    return Result.True;
                }
            }

            // 先產生一個作Bulk Copy用的 #tmp Table
            if (sql.writeDebugLog)
            {
                Ict.Logs.UI.LogInfo("Bulk Delete - Create #tmp Table ...", sql.Cmd_CreateTmpTable_Del);
            }

            SqlCommand cmd = new SqlCommand(sql.Cmd_CreateTmpTable_Del, sql.conn);
            if (sql.trans != null)
            {
                cmd.Transaction = sql.trans;
            }

            try
            {
                cmd.ExecuteNonQuery();

                // 把 sourceTable 匯入到 tempTable 裡
                sql.bulkCopy.WriteToServer(delRows);
            }
            catch (Exception e)
            {
                string bulkMsg = GetExceptionMsg(e, sql.bulkCopy);
                DualResult fails = Result.F($"Fail to Merge to [{sql.destinationTableName}] {bulkMsg}", e);
                return fails;
            }

            // 用 tempTable join & delet DB table
            if (sql.writeDebugLog)
            {
                Ict.Logs.UI.LogInfo("Bulk Delete -  Execute command...", sql.Cmd_DelTargetTableByTmp);
            }

            return SQL.Mute_Execute(sql.conn, sql.Cmd_DelTargetTableByTmp, sql.parameters);
        }

        private static bool GoDelete(SQL sql, bool useDeletedStatus = true)
        {
            DataTable delRows;
            if (useDeletedStatus)
            {
                delRows = sql.sourceTable.GetChanges(DataRowState.Deleted);
                if (delRows == null || delRows.Rows.Count == 0)
                {
                    return true;
                }

                delRows.RejectChanges(); // 將state 還原之後不會影響原Table的 State
            }
            else
            {
                delRows = sql.sourceTable;
                if (delRows == null || delRows.Rows.Count == 0)
                {
                    return true;
                }
            }

            // 先產生一個作Bulk Copy用的 #tmp Table
            if (sql.writeDebugLog)
            {
                Ict.Logs.UI.LogInfo("Bulk Delete - Create #tmp Table ...", sql.Cmd_CreateTmpTable_Del);
            }

            SqlCommand cmd = new SqlCommand(sql.Cmd_CreateTmpTable_Del, sql.conn);
            if (sql.trans != null)
            {
                cmd.Transaction = sql.trans;
            }

            try
            {
                cmd.ExecuteNonQuery();

                // 把 sourceTable 匯入到 tempTable 裡
                sql.bulkCopy.WriteToServer(delRows);
            }
            catch (Exception e)
            {
                string bulkMsg = GetExceptionMsg(e, sql.bulkCopy);
                new BugProc(e, $"Fail to Delete [{sql.destinationTableName}] {bulkMsg}").Show();
                return false;
            }

            // 用 tempTable join & delet DB table
            if (sql.writeDebugLog)
            {
                Ict.Logs.UI.LogInfo("Bulk Delete -  Execute command...", sql.Cmd_DelTargetTableByTmp);
            }

            return SQL.Execute(sql.conn, sql.Cmd_DelTargetTableByTmp, sql.parameters);
        }

        public static bool BulkMergeInsertUpdate_SafeDelete(SQL sql)
        {
            string errMsg = sql.FinalizeForDelete();
            if (errMsg != null)
            {
                new BugProc(new Exception(errMsg), $"Fail to Merge to [{sql.destinationTableName}]").Show();
                ////Msg.ShowException(null, , caption: "Fail to Merge to [" + sql.destinationTableName + "]");
                sql.Reset_Del();
                return false;
            }

            bool result = SQL.GoDelete(sql);
            sql.Reset_Del();
            if (!result)
            {
                return false;
            }

            errMsg = sql.FinalizeForMerge(MergeOption.InsertUpdate);
            if (errMsg != null)
            {
                new BugProc(new Exception(errMsg), $"Fail to Merge to [{sql.destinationTableName}]")
                    .Show();
                sql.Reset();
                return false;
            }

            result = SQL.GoMerge(sql);
            sql.Reset();
            return result;
        }

        public static bool BulkMergeInsertUpdate_SafeDelete(SQL sql, out DataTable table)
        {
            table = null;
            string errMsg = sql.FinalizeForDelete();
            if (errMsg != null)
            {
                new BugProc(new Exception(errMsg), $"Fail to Merge to [{sql.destinationTableName}]")
                    .Show();
                sql.Reset_Del();
                return false;
            }

            bool result = SQL.GoDelete(sql);
            sql.Reset_Del();
            if (!result)
            {
                return false;
            }

            errMsg = sql.FinalizeForMerge(MergeOption.InsertUpdate);
            if (errMsg != null)
            {
                new BugProc(new Exception(errMsg), $"Fail to Merge to [{sql.destinationTableName}]")
                    .Show();
                sql.Reset();
                return false;
            }

            result = SQL.GoMerge(sql, out table);

            sql.Reset();
            return result;
        }

        public static bool BulkMergeInsertUpdate(SQL sql)
        {
            string errMsg = sql.FinalizeForMerge(MergeOption.InsertUpdate);
            if (errMsg != null)
            {
                new BugProc(new Exception(errMsg), $"Fail to Merge to [{sql.destinationTableName}]")
                    .Show();
                sql.Reset();
                return false;
            }

            bool result = SQL.GoMerge(sql);

            sql.Reset();
            return result;
        }

        public static bool BulkMergeInsertUpdate(SQL sql, out DataTable table)
        {
            string errMsg = sql.FinalizeForMerge(MergeOption.InsertUpdate);
            if (errMsg != null)
            {
                new BugProc(new Exception(errMsg), $"Fail to Merge to [{sql.destinationTableName}]")
                    .Show();
                sql.Reset();
                table = null;
                return false;
            }

            bool result = SQL.GoMerge(sql, out table);
            sql.Reset();
            return result;
        }

        /// <summary>
        /// Merge 有問題, 當條件mapping之後, 已存在的話不會被insert
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool BulkMergeInsert(SQL sql)
        {
            string errMsg = sql.FinalizeForMerge(MergeOption.Insert);
            if (errMsg != null)
            {
                new BugProc(new Exception(errMsg), $"Fail to Merge to [{sql.destinationTableName}]")
                    .Show();
                sql.Reset();
                return false;
            }

            bool result = SQL.GoMerge(sql);
            sql.Reset();
            return result;
        }

        /// <summary>
        /// Merge 有問題, 當條件mapping之後, 已存在的話不會被insert
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static bool BulkMergeInsert(SQL sql, out DataTable table)
        {
            string errMsg = sql.FinalizeForMerge(MergeOption.Insert);
            if (errMsg != null)
            {
                new BugProc(new Exception(errMsg), $"Fail to Merge to [{sql.destinationTableName}]")
                    .Show();
                sql.Reset();
                table = null;
                return false;
            }

            bool result = GoMerge(sql, out table);
            sql.Reset();
            return result;
        }

        public static bool BulkInsertForColumns(SQL sql)
        {
            string errMsg = sql.FinalizeForInsert();
            if (errMsg != null)
            {
                new BugProc(new Exception(errMsg), $"Fail to Insert to [{sql.destinationTableName}]")
                    .Show();
                sql.Reset();
                return false;
            }

            bool result = SQL.GoInsert(sql);
            sql.Reset();
            return result;
        }

        public static DualResult Mute_BulkMergeInsertUpdate_SafeDelete(SQL sql)
        {
            string errMsg = sql.FinalizeForDelete();
            if (errMsg != null)
            {
                DualResult fails = Result.F($"Fail to delete [{sql.destinationTableName}] , {errMsg}");
                sql.Reset_Del();
                return fails;
            }

            DualResult result = SQL.Mute_GoDelete(sql);
            sql.Reset_Del();
            if (!result)
            {
                return result;
            }

            errMsg = sql.FinalizeForMerge(MergeOption.InsertUpdate);
            if (errMsg != null)
            {
                DualResult fails = Result.F($"Fail to Merge [{sql.destinationTableName}] , {errMsg}");
                sql.Reset();
                return fails;
            }

            result = SQL.Mute_GoMerge(sql);
            sql.Reset();
            return result;
        }

        public static DualResult Mute_BulkDelete(SQL sql)
        {
            string errMsg = sql.FinalizeForDelete();
            if (errMsg != null)
            {
                DualResult fails = Result.F($"Fail to delete [{sql.destinationTableName}] , {errMsg}");
                sql.Reset_Del();
                return fails;
            }

            DualResult result = SQL.Mute_GoDelete(sql, false);
            sql.Reset_Del();
            return result;
        }

        public static DualResult Mute_BulkInsertForColumns(SQL sql)
        {
            string errMsg = sql.FinalizeForInsert();
            if (errMsg != null)
            {
                DualResult fails = Result.F($"Fail to Insert to [{sql.destinationTableName}] , {errMsg}");
                sql.Reset();
                return fails;
            }

            DualResult result = SQL.Mute_GoInsert(sql);
            sql.Reset();
            return result;
        }

        public static bool BulkInsertForColumns(SQL sql, out DataTable table)
        {
            string errMsg = sql.FinalizeForInsert();
            if (errMsg != null)
            {
                new BugProc(new Exception(errMsg), $"Fail to Insert to [{sql.destinationTableName}]")
                    .Show();
                sql.Reset();
                table = null;
                return false;
            }

            bool result = SQL.GoInsert(sql, out table);
            sql.Reset();
            return result;
        }

        /// <summary>
        /// to Insert All data into DB table
        /// <para> - copyTable 的欄位順序要和DB Table一樣 </para>
        /// <para> - connection 使用default connection, 用完自動關閉 </para>
        /// <para> - BulkCopy物件透過 connection產生, 用完自動關閉  </para>
        /// </summary>
        /// <param name="destinationTableName">DB Table name , ex: Brand </param>
        /// <param name="copyTable">copyTable 的欄位順序要和DB Table一樣</param>
        /// <returns></returns>
        public static bool BulkInsert(string destinationTableName, DataTable copyTable)
        {
            bool updateOk = false;
            SqlConnection conn;
            DBProxy.Current.OpenConnection(null, out conn);
            using (conn)
            {
                updateOk = SQL.BulkInsert(destinationTableName, copyTable, conn);
            }

            return updateOk;
        }

        /// <summary>
        /// to Insert All data in table to DB
        /// <para> - copyTable 的欄位順序要和DB Table一樣 </para>
        /// <para> - BulkCopy物件透過指定的 connection產生, 用完自動關閉  </para>
        /// </summary>
        /// <param name="destinationTableName">DB Table name , ex: Brand </param>
        /// <param name="copyTable">copyTable 的欄位順序要和DB Table一樣</param>
        /// <returns></returns>
        public static bool BulkInsert(string destinationTableName, DataTable copyTable, SqlConnection conn)
        {
            bool updateOk = false;
            SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);
            using (bulkCopy)
            {
                updateOk = SQL.BulkInsert(destinationTableName, copyTable, bulkCopy);
            }

            return updateOk;
        }

        /// <summary>
        /// to Insert All data in table to DB
        /// <para> - copyTable 的欄位順序要和DB Table一樣 </para>
        /// <para> - connection和 BulkCopy物件不會自動關閉  </para>
        /// </summary>
        /// <param name="destinationTableName">DB Table name , ex: Brand </param>
        /// <param name="copyTable">copyTable 的欄位順序要和DB Table一樣</param>
        /// <returns></returns>
        public static bool BulkInsert(string destinationTableName, DataTable copyTable, SqlBulkCopy bulkCopy)
        {
            bool updateOk = true;
            try
            {
                bulkCopy.DestinationTableName = destinationTableName;
                bulkCopy.WriteToServer(copyTable);
            }
            catch (Exception e)
            {
                updateOk = false;
                string bulkMsg = GetExceptionMsg(e, bulkCopy);
                new BugProc(e, $"Fail to Insert to [{destinationTableName}] {bulkMsg}")
                    .Show();
            }

            return updateOk;
        }

        #endregion


        public static bool ExecuteNonQuery(string connName, string sqlCmd, out int result, IList<SqlParameter> parameters = null, int? timeOut = null)
        {
            SqlConnection conn;
            if (!SQL.GetConnection(out conn, connName))
            {
                result = 0;
                return false;
            }

            return ExecuteNonQuery(conn, sqlCmd, out result, parameters, timeOut);
        }

        public static bool ExecuteNonQuery(SqlConnection conn, string sqlCmd, out int result, IList<SqlParameter> parameters = null, int? timeOut = null)
        {
            SqlCommand cmd = new SqlCommand(sqlCmd, conn);
            bool ok;
            if (parameters != null)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(parameters.ToArray());
            }

            if (timeOut != null)
            {
                cmd.CommandTimeout = (int)timeOut;
            }

            Exception ex = null;
            try
            {
                result = cmd.ExecuteNonQuery();
                ok = true;
            }
            catch (Exception e)
            {
                ex = e;
                result = 0;
                ok = false;
                new BugProc(e)
                    .Show();
            }

            return ok;
        }

        public static DualResult Mute_ExecuteNonQuery(string connName, string sqlCmd, out int result, IList<SqlParameter> parameters = null, int? timeOut = null)
        {
            SqlConnection conn;
            DualResult ok = SQL.Mute_GetConnection(out conn, connName);
            if (!ok)
            {
                result = 0;
                return ok;
            }

            return Mute_ExecuteNonQuery(conn, sqlCmd, out result, parameters, timeOut);
        }

        public static DualResult Mute_ExecuteNonQuery(SqlConnection conn, string sqlCmd, out int result, IList<SqlParameter> parameters = null, int? timeOut = null)
        {
            SqlCommand cmd = new SqlCommand(sqlCmd, conn);
            bool ok;
            if (parameters != null)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(parameters.ToArray());
            }

            if (timeOut != null)
            {
                cmd.CommandTimeout = (int)timeOut;
            }

            Exception ex = null;
            try
            {
                result = cmd.ExecuteNonQuery();
                ok = true;
            }
            catch (Exception e)
            {
                ex = e;
                result = 0;
                ok = false;
            }

            return ok ? Result.True : Result.F(ex);
        }

        public static bool ExecuteScalar(string connName, string sqlCmd, out object result, IList<SqlParameter> parameters = null, int? timeOut = null)
        {
            SqlConnection conn;
            if (!SQL.GetConnection(out conn, connName))
            {
                result = null;
                return false;
            }

            return ExecuteScalar(conn, sqlCmd, out result, parameters, timeOut);
        }

        public static bool ExecuteScalar(SqlConnection conn, string sqlCmd, out object result, IList<SqlParameter> parameters = null, int? timeOut = null)
        {
            SqlCommand cmd = new SqlCommand(sqlCmd, conn);
            bool ok;
            if (null != parameters)
            {
                cmd.Parameters.AddRange(parameters.ToArray());
            }

            if (null != timeOut)
            {
                cmd.CommandTimeout = (int)timeOut;
            }

            try
            {
                result = cmd.ExecuteScalar();
                ok = true;
            }
            catch (Exception e)
            {
                result = null;
                ok = false;
                new BugProc(e)
                    .Show();
            }

            return ok;
        }

        public static bool Execute(string connName, string sqlCmd, IList<SqlParameter> parameters = null, int? timeOut = null)
        {
            SqlConnection conn;
            if (!SQL.GetConnection(out conn, connName))
            {
                return false;
            }

            return SQL.Execute(conn, sqlCmd, parameters, timeOut);
        }

        public static bool Execute(SqlConnection conn, string sqlCmd, IList<SqlParameter> parameters = null, int? timeOut = null)
        {
            int hiddenCounts;
            return SQL.ExecuteNonQuery(conn, sqlCmd, out hiddenCounts, parameters, timeOut);
        }

        public static DualResult Mute_Execute(string connName, string sqlCmd, IList<SqlParameter> parameters = null, int? timeOut = null)
        {
            SqlConnection conn;
            DualResult ok = SQL.Mute_GetConnection(out conn, connName);
            if (!ok)
            {
                return ok;
            }

            return SQL.Mute_Execute(conn, sqlCmd, parameters, timeOut);
        }

        public static DualResult Mute_Execute(SqlConnection conn, string sqlCmd, IList<SqlParameter> parameters = null, int? timeOut = null)
        {
            int hiddenCounts;
            return SQL.Mute_ExecuteNonQuery(conn, sqlCmd, out hiddenCounts, parameters, timeOut);
        }

        public static bool Select(string conn, string sqlCmd, out DataTable table, IList<SqlParameter> parameters = null)
        {
            DualResult result;
            if (!(result = DBProxy.Current.Select(conn, sqlCmd, parameters, out table)))
            {
                new BugProc(result.GetException())
                    .Show();
                return false;
            }

            return true;
        }

        public static bool Select(SqlConnection conn, string sqlCmd, out DataTable table, IList<SqlParameter> parameters = null)
        {
            DualResult result;
            if (!(result = DBProxy.Current.SelectByConn(conn, sqlCmd, parameters, out table)))
            {
                new BugProc(result.GetException())
                    .Show();
                return false;
            }

            return true;
        }

        public static bool Selects(string conn, string sqlCmd, out DataSet set, IList<SqlParameter> parameters = null)
        {
            bool closeConnection = true;
            SqlConnection connection;
            if (!SQL.GetConnection(out connection))
            {
                set = null;
                return false;
            }

            return SQL.Selects(connection, sqlCmd, out set, parameters, closeConnection);
        }

        public static bool Selects(SqlConnection conn, string sqlCmd, out DataSet set, IList<SqlParameter> parameters = null, bool closeConnection = false)
        {
            set = null;
            if (null == conn)
            {
                if (!SQL.GetConnection(out conn))
                {
                    return false;
                }

                closeConnection = true;
            }

            var result = DBProxy.Current.SelectSetByConn(conn, sqlCmd, parameters, out set);
            if (closeConnection) { conn.Close(); }
            if (!result)
            {
                new BugProc(result.GetException())
                    .Show();
            }

            return result;
        }

        public static bool OpenConnection(out SqlConnection connection, Object connectionObj)
        {
            bool connectOK = true;

            // SqlConnection conn = null;
            if (null == connectionObj)
            {
                // 如果傳入的 SqlConnection 是 null
                connectOK = SQL.GetConnection(out connection);
            }
            else if (connectionObj is string)
            {
                connectOK = SQL.GetConnection(out connection, (string)connectionObj);
            }
            else
            {
                connection = (SqlConnection)connectionObj;
                if (((SqlConnection)connectionObj).State == ConnectionState.Closed)
                {
                    connection.Open();
                }
            }

            return connectOK;
        }

        public static bool GetConnection(out SqlConnection connection, string conn = null)
        {
            DualResult result;
            if (!(result = DBProxy.Current.OpenConnection(conn, out connection)))
            {
                new BugProc(result.GetException())
                    .Show();
                return false;
            }

            return true;
        }

        internal static DualResult Mute_GetConnection(out SqlConnection connection, string conn = null)
        {
            DualResult result;
            if (!(result = DBProxy.Current.OpenConnection(conn, out connection)))
            {
                return result;
            }

            return Result.True;
        }

        public static bool Exists(SqlConnection conn, string sql, IList<SqlParameter> parameters = null)
        {
            bool isExists = false;
            try
            {
                if (null == conn)
                {
                    if (!SQL.GetConnection(out conn)) { return false; }
                }

                var cmd = new SqlCommand("select iif( exists(" + sql + ") ,1,0)", conn);
                if (null != parameters)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                isExists = (int)cmd.ExecuteScalar() == 1;
            }
            catch (Exception e)
            {
                new BugProc(e)
                    .Show();
            }

            return isExists;
        }

        #region Sample codes

        /// <summary>
        /// 將資料取出 & insert , update 回DB
        /// 此範例是排除ukey update
        /// </summary>
        private static void example_BulkMergeInsertUpdate1()
        {
            string connectionString = "Data Source=127.0.0.1;Initial Catalog=TRADE;Persist Security Info=True;User ID=sa;Password=123456";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            // SqlBulkCopy bulk = new SqlBulkCopy(conn);
            SqlDataAdapter adapter = new SqlDataAdapter(
                "select ukey,id,poid,seq1,seq2,editDate from dbo.Export_Detail where id='WK20150400605' "
                , conn);
            DataTable export1 = new DataTable();
            adapter.Fill(export1);

            // 改資料
            export1.Rows[0]["editDate"] = DateTime.Now;

            // 新增
            DataRow newOne = export1.NewRow();
            newOne["ID"] = "WK20150400XXX";
            export1.Rows.Add(newOne);

            // 取出定義檔, 從定義檔才能知道DataTable哪些欄位和DB是一樣的名稱
            DBTable wk1 = DBTable.GetByKeyword("trade,export_Detail");
            // 同 DBTable wk1 = new Sci.Trade.Class.DB.CodeGen.Export_Detail()
            // 透過SQL 做 Merge , 
            // 過程會排除ukey 這個identity欄位 
            // (不會被 update .. set ukey = @ukey 
            //  也不會在 insert中塞入ukey
            SQL sql = new SQL();
            sql.SetConnection(conn)
                .SetTableStructure(wk1)
                .SetSourceTable(export1);
            if (SQL.BulkMergeInsertUpdate(sql))
            {
                // 成功
            }
            else
            {
                // 失敗
            }
        }

        /// <summary>
        /// 將資料取出 , 修改ID之後 , 
        /// 使用Ukey 做mapping & insert , update 回DB    
        /// 並在Merge 時, 限定只insert update 特定欄位
        /// </summary>
        private static void example_BulkMergeInsertUpdate2()
        {
            string connectionString = "Data Source=127.0.0.1;Initial Catalog=TRADE;Persist Security Info=True;User ID=sa;Password=123456";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            // SqlBulkCopy bulk = new SqlBulkCopy(conn);
            SqlDataAdapter adapter = new SqlDataAdapter(
                "select ukey,id,poid,seq1,seq2,editDate from dbo.Export_Detail where id='WK20150400605' "
                , conn);
            DataTable export1 = new DataTable();
            adapter.Fill(export1);

            // 改資料
            export1.Rows[0]["ID"] = "TestID";

            // 取出定義檔, 從定義檔才能知道DataTable哪些欄位和DB是一樣的名稱
            DBTable wk1 = DBTable.GetByKeyword("trade,export_detail");

            // 同 DBTable wk1 = new Sci.Trade.Class.DB.CodeGen.Export_Detail()
            // 透過SQL 做 Merge , 
            // 過程會排除ukey 這個identity欄位 
            // (不會被 update .. set ukey = @ukey 
            // 也不會在 insert中塞入ukey
            SQL sql = new SQL();
            sql.SetConnection(conn)
                .SetTableStructure(wk1)
                .SetSourceTable(export1)
                .SetMergeOnIndex(wk1.primaryKey) // export_Detail的primary key 是 Ukey
                .SetColumnInclude_TmpTable("id,ukey,editDate"); // 產生tmpTable時, 直接指定只放入id,ukey,editDate

            if (SQL.BulkMergeInsertUpdate(sql))
            {
                // 成功
            }
            else
            {
                // 失敗
            }

        }

        private static void example_BulkInsert_1()
        {
            return;
            SqlConnection conn;
            DBProxy.Current.OpenConnection(null, out conn);
            DataTable export;
            if (!SQL.Select(conn, "select * from dbo.export where id = '555555555' ", out export))
            {
                return;
            }

            string newID = "AA" + DateTime.Now.ToString("HH:mm:ss.ff");
            export.Rows[0]["id"] = newID;
            export.Rows[0]["EditDate"] = DateTime.Now;

            DataTable export1;
            if (!SQL.Select(conn, "select * from dbo.export_detail where id = '555555555' ", out export1))
            {
                return;
            }

            foreach (DataRow row in export1.Rows)
            {
                row["id"] = newID;
                row["EditDate"] = DateTime.Now;
            }

            SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);

            using (conn)
            {
                using (bulkCopy)
                {
                    SQL.BulkInsert("dbo.Export", export, bulkCopy);
                    SQL.BulkInsert("dbo.Export_Detail", export1, bulkCopy);
                }
            }
        }

        private static void example_BulkInsert_2()
        {
            return;
            SqlConnection conn;
            DBProxy.Current.OpenConnection(null, out conn);
            DataTable export;
            if (!SQL.Select(conn, "select * from dbo.export where id = '555555555' ", out export))
            {
                return;
            }

            string newID = "AA" + DateTime.Now.ToString("HH:mm:ss.ff");
            export.Rows[0]["id"] = newID;
            export.Rows[0]["EditDate"] = DateTime.Now;

            DataTable export1;
            if (!SQL.Select(conn, "select * from dbo.export_detail where id = '555555555' ", out export1))
            {
                return;
            }

            foreach (DataRow row in export1.Rows)
            {
                row["id"] = newID;
                row["EditDate"] = DateTime.Now;
            }

            using (conn)
            {
                // 傳入Connection 物件, 就會自動產生SqlBulkCopy A物件, 並自動kill it after writing to server 
                SQL.BulkInsert("dbo.Export", export, conn);

                // 傳入Connection 物件, 就會自動產生SqlBulkCopy B物件, 並自動kill it after writing to server 
                SQL.BulkInsert("dbo.Export_Detail", export1, conn);
            }
        }

        private static void example_BulkInsert_3()
        {
            return;
            DataTable export;
            if (!SQL.Select(string.Empty, "select * from dbo.export where id = '555555555' ", out export))
            {
                return;
            }

            string newID = "AA" + DateTime.Now.ToString("HH:mm:ss.ff");
            export.Rows[0]["id"] = newID;
            export.Rows[0]["EditDate"] = DateTime.Now;
            DataTable export1;
            if (!SQL.Select(string.Empty, "select * from dbo.export_detail where id = '555555555' ", out export1))
            {
                return;
            }

            foreach (DataRow row in export1.Rows)
            {
                row["id"] = newID;
                row["EditDate"] = DateTime.Now;
            }

            // 自動產生Connection 物件 A ,SqlBulkCopy 物件 B, 並自動kill them after writing to server
            SQL.BulkInsert("Export", export);

            // 自動產生Connection 物件 C ,SqlBulkCopy 物件 D, 自動kill them after writing to server
            SQL.BulkInsert("Export_Detail", export1);

        }

        /// <summary>
        /// ForColumns 適用狀況1: 
        ///  - 程式的DataTable只有幾個跟DB相同的欄位  
        ///  - 每個DataTable 欄位都是DB有的
        /// </summary>
        private static void example_BulkInsertForColumns_1()
        {
            return;
            SqlConnection conn;
            DBProxy.Current.OpenConnection(null, out conn);
            SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);
            DataTable po3;
            SQL.Select(conn, "select  id as poid,seq1,seq2,qty,foc,suppid from dbo.PO_Supp_Detail where id = '16040008UC   '", out po3);
            DataColumn ID = new DataColumn("ID", typeof(String));
            ID.DefaultValue = "555555555";
            po3.Columns.Add(ID);

            SQL sql = new SQL();
            sql.SetConnection(conn);
            sql.SetSqlBulkCopy(bulkCopy);
            sql.SetDestinationTableName("dbo.Export_Detail").SetSourceTable(po3);
            Exception error = null;
            TransactionScope tranScope = new TransactionScope();
            try
            {
                using (conn)
                {
                    using (bulkCopy)
                    {
                        SQL.BulkInsertForColumns(sql);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex;
            }

            if (null == error)
            {
                tranScope.Complete();
            }

            tranScope.Dispose();
            tranScope = null;
            if (null != error)
            {
                new BugProc(error, "fail to create WK.")
                    .Show();
            }
            else
            {
                Ict.Win.MsgHelper.Current.ShowInfo(null, " " + po3.Rows.Count + " WK details created .");
            }
        }

        /// <summary>
        /// ForColumns 適用狀況2: 
        ///  - 程式的DataTable只有幾個跟DB相同的欄位  
        ///  - 有幾個欄位是DB沒有的
        /// </summary>
        private static void example_BulkInsertForColumns_2()
        {
            return;

            SqlConnection conn;
            DBProxy.Current.OpenConnection(null, out conn);
            SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);
            DataTable po3;
            SQL.Select(conn, "select  id as poid,seq1,seq2,qty as POQty,foc as POFoc,ShipQty,ShipFoc,Shortage,suppid from dbo.PO_Supp_Detail where id = '16040008UC   '", out po3);
            DataColumn ID = new DataColumn("ID", typeof(string));
            ID.DefaultValue = "555555555";
            po3.Columns.Add(ID);
            DataColumn Qty = new DataColumn("Qty", typeof(decimal));
            ID.DefaultValue = 0;
            po3.Columns.Add(Qty);
            DataColumn Foc = new DataColumn("Foc", typeof(decimal));
            ID.DefaultValue = 0;
            po3.Columns.Add(Foc);
            po3.Columns.Add("BalanceQty", typeof(decimal), "POQty-ShipQty-Shortage");
            po3.Columns.Add("BalanceFoc", typeof(decimal), "POFoc-ShipFoc");
            po3.Columns.Add("LeftQty", typeof(decimal), "BalanceQty-Qty");
            po3.Columns.Add("POQtyOld", typeof(decimal), "POQty");
            po3.Columns.Add("POFocOld", typeof(decimal), "POFoc");

            SQL sql = new SQL();
            sql.SetConnection(conn);
            sql.SetSqlBulkCopy(bulkCopy);
            sql.SetDestinationTableName("dbo.Export_Detail").SetSourceTable(po3);
            sql.SetColumnInclude_TmpTable("id,poid,seq1,seq2,PoType,Qty,Foc,POQtyOld,POFocOld,BalanceQty,BalanceFOC");
            /*  SetColumnInclude_TmpTable 和 SetColumnExclude_TmpTable 二擇一使用
            sql.SetColumnExclude_TmpTable("LeftQty,POQty,POFoc");
            */
            sql.SetColumnExclude_Update("Qty,Foc");
            sql.SetColumnExclude_Insert("ShipQty,ShipFoc,ShipETA");

            Exception error = null;
            TransactionScope tranScope = new TransactionScope();
            try
            {
                using (conn)
                {
                    using (bulkCopy)
                    {

                        SQL.BulkInsertForColumns(sql);

                    }
                }
            }
            catch (Exception ex)
            {
                error = ex;
            }

            if (null == error) { tranScope.Complete(); }
            tranScope.Dispose();
            tranScope = null;
            if (null != error)
            {

                new BugProc(error, "fail to create WK.")
                    .Show();
            }
            else
            {
                Ict.Win.MsgHelper.Current.ShowInfo(null, " " + po3.Rows.Count + " WK details created .");
            }
        }

        private static void testBulkCopy()
        {
            return;
            DataTable mTable = new DataTable();
            mTable.Columns.Add(new DataColumn("id", typeof(string)));
            mTable.Columns.Add(new DataColumn("xSeq11", typeof(string)));
            mTable.Columns.Add(new DataColumn("Seq2", typeof(string)));
            mTable.Rows.Add(new string[] { "1xxx", "01", "01" });
            mTable.Rows.Add(new string[] { "1xxx", "01", "02" });
            mTable.Rows.Add(new string[] { "1xxx", "01", "03" });

            SqlConnection conn;
            DBProxy.Current.OpenConnection(null, out conn);
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
            {
                // Write to temp table
                string createTmpTable =
@" create table #tmpTable (
     id CHAR(16) NULL,  
    dddd bigint NOT NULL IDENTITY (77, 1) ,
     seq2 VARCHAR(3),
     seq1 VARCHAR(3) NULL,
     seq3 VARCHAR(3) NULL
  );
    insert into #tmpTable(id,seq1,seq2) values('S','1','2');

";
                SqlCommand cmd = new SqlCommand(createTmpTable, conn);
                cmd.ExecuteNonQuery();

                bulkCopy.DestinationTableName = "#tmpTable";
                //SqlParameter SqlParam = cmd.Parameters.Add("#tmpTable", System.Data.SqlDbType.Structured);
                //SqlParam.Value = mTable;
                //cmd.ExecuteNonQuery();
                bulkCopy.WriteToServer(mTable);

                //Merge changes in temp table with ZipCodeTerritory
                /*
                string mergeSql = "merge ZipCodeTerritory as Target " +
                                    "using ZipCodeTerritoryTemp as Source " +
                                    "on " +
                                    "Target.Id = Source.Id " +
                                    "when matched then " +
                                    updateCommand + ";";
                */
                string mergeSql = "SELECT  * FROM #tmpTable ";
                cmd.CommandText = mergeSql;
                string re2 = string.Empty;
                using (SqlDataReader reader2 = cmd.ExecuteReader())
                {
                    while (reader2.Read())
                    {
                        for (int i = 0; i < reader2.FieldCount; i++)
                        {
                            re2 += reader2.GetValue(i);
                        }

                        re2 += Environment.NewLine;
                    }
                }

                Ict.Win.MsgHelper.Current.ShowInfo(null, re2);

                // Drop temp table
            }
        }

        private static void TestMerge()
        {
            SQL xx = new SQL();
            return;
            string mergeCmd =
@"  
drop table #t_Source,#t_Target,#t_Log 

create table #t_Source(
     id CHAR(10)     
	 ,name varchar(10)     );
create table #t_Target( 
     id CHAR(10)     
	 ,name varchar(10)     );
create table #t_Log(    
     id_del CHAR(10)     ,
	 name_del varchar(10)     ,
	ActionTaken nvarchar(10)  ,
	id_Insert CHAR(10)  ,
	 name_Insert varchar(10)         );
truncate table #t_Source
truncate table #t_Target
truncate table #t_Log
insert into #t_Source (id,name) values(1,'a'),(2,'b'),(3,'c')
insert into #t_Target (id,name) values(3,'cc'),(4,'dd'),(5,'ee')
select * from #t_Source
select * from #t_Target
  

MERGE #t_Target AS t
USING #t_Source AS s
ON (Target.id = Source.id)
WHEN MATCHED and Source.id>='5'THEN
	UPDATE SET t.name = s.name 
WHEN NOT MATCHED BY TARGET and s.id>'1' THEN
	INSERT (id, name )
	VALUES (s.id, s.name)	
WHEN NOT MATCHED BY Source and t.id>='5' THEN
	delete 
	OUTPUT deleted.*, $action, inserted.* INTO #t_Log;
	
  select * from #t_Target
  select * from #t_Log
";

            SqlConnection conn;
            DBProxy.Current.OpenConnection(null, out conn);
            SqlCommand cmd = new SqlCommand("select * from dbo.pass0 WHERE id = 'admin' ", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            // SqlCommandBuilder cb = new SqlCommandBuilder(da);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dt.AcceptChanges();
            dt.Rows[0]["EditDate"] = DateTime.Now;
            ////dt.Rows.Add("value1", 0); //I presume three varchar columns
            da.Update(dt);
        }

        #endregion 

        /// <summary>
        /// 取得Table全部的欄位名稱(逗號隔開)
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="targetTable"></param>
        /// <returns></returns>
        private static string GetAllFields(System.Data.DataTable sourceTable)
        {
            string fields = string.Empty;
            foreach (DataColumn d in sourceTable.Columns)
            {
                fields += "," + d.ColumnName;
            }

            if (fields != string.Empty)
            {
                // 去掉第一個逗號;
                fields = fields.Substring(1);
            }

            return fields;
        }

        /// <summary>
        /// 批次透過 SqlBulkCopy 將DataTable 匯到 connection 的 #tmpTable
        /// <para>匯入會排除 DataRowState = Deleted 的資料</para>
        /// <para>再將#tmpTable 批次 insert into {TargetTable} 中</para>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        /// <param name="connection">可傳 null, String , SqlConnection物件</param>
        /// <returns></returns>
        public static bool TableBulkInsert(DataTable data, String tableName, Object connection = null)
        {
            #region -- Get Connection
            bool connectOK = false;
            SqlConnection conn = null;
            if (null == connection)
            {
                // 如果傳入的 SqlConnection 是 null
                connectOK = GetConnection(out conn);
            }
            else if (connection is String)
            {
                connectOK = GetConnection(out conn, (String)connection);
            }
            else if (connection is SqlConnection && ((SqlConnection)connection).State == ConnectionState.Closed)
            {
                conn = (SqlConnection)connection;
                conn.Open();
                connectOK = true;
            }
            else { connectOK = true; }
            if (!connectOK) { return connectOK; }
            #endregion

            return TableBulkInsert(data, GetDBTable(tableName, conn), conn);
        }

        /// <summary>
        /// 批次透過 SqlBulkCopy 將DataTable 匯到 connection 的 #tmpTable
        /// <para>匯入會排除 DataRowState = Deleted 的資料</para>
        /// <para>再將#tmpTable 批次 insert into {TargetTable} 中</para>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tableSchema"></param>
        /// <param name="connection">可傳 null, String , SqlConnection物件</param>
        /// <returns></returns>
        public static bool TableBulkInsert(DataTable data, Sci.DB.DBTable tableSchema, Object connection = null)
        {
            #region -- Get Connection
            bool connectOK = false;
            SqlConnection conn = null;
            if (null == connection)
            {
                // 如果傳入的 SqlConnection 是 null
                connectOK = SQL.GetConnection(out conn);
            }
            else if (connection is String)
            {
                connectOK = SQL.GetConnection(out conn, (String)connection);
            }
            else if (connection is SqlConnection && ((SqlConnection)connection).State == ConnectionState.Closed)
            {
                conn = (SqlConnection)connection;
                conn.Open();
                connectOK = true;
            }
            else { connectOK = true; }
            if (!connectOK) { return connectOK; }
            #endregion

            SQL sql = new SQL();
            sql.SetConnection(conn)
                .SetTableStructure(tableSchema)
                .SetSourceTable(data);

            return BulkInsertForColumns(sql);
        }

        /// <summary>
        /// 批次透過 SqlBulkCopy 將DataTable 匯到 connection 的 #tmpTable
        /// <para>只有使用MergeOption.InsertUpdate_SafeDelete選項的時候會使用 DataRowState = Deleted 的資料做刪除動作</para>
        /// <para>再將#tmpTable 批次 Merge 到 {TargetTable} 中</para>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        /// <param name="connection">可傳 null, String , SqlConnection物件</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static bool TableBulkMerge(DataTable data, String tableName
            , Object connection = null
            , SQL.MergeOption option = SQL.MergeOption.InsertUpdate_SafeDelete)
        {

            #region -- Get Connection
            bool connectOK = false;
            SqlConnection conn = null;
            if (null == connection)
            {
                // 如果傳入的 SqlConnection 是 null
                connectOK = SQL.GetConnection(out conn);
            }
            else if (connection is String)
            {
                connectOK = SQL.GetConnection(out conn, (String)connection);
            }
            else if (connection is SqlConnection && ((SqlConnection)connection).State == ConnectionState.Closed)
            {
                conn = (SqlConnection)connection;
                conn.Open();
                connectOK = true;
            }
            else { connectOK = true; }
            if (!connectOK) { return connectOK; }
            #endregion

            return TableBulkMerge(data, GetDBTable(tableName, conn), conn, option);
        }

        /// <summary>
        /// 批次透過 SqlBulkCopy 將DataTable 匯到 connection 的 #tmpTable
        /// <para>只有使用MergeOption.InsertUpdate_SafeDelete選項的時候會使用 DataRowState = Deleted 的資料做刪除動作</para>
        /// <para>再將#tmpTable 批次 Merge 到 {TargetTable} 中</para>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tableSchema"></param>
        /// <param name="connection">可傳 null, String , SqlConnection物件</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static bool TableBulkMerge(DataTable data, Sci.DB.DBTable tableSchema
            , Object connection = null
            , MergeOption option = MergeOption.InsertUpdate_SafeDelete)
        {
            #region -- Get Connection
            bool connectOK = false;
            SqlConnection conn = null;
            if (null == connection)
            {
                // 如果傳入的 SqlConnection 是 null
                connectOK = SQL.GetConnection(out conn);
            }
            else if (connection is String)
            {
                connectOK = SQL.GetConnection(out conn, (String)connection);
            }
            else if (connection is SqlConnection && ((SqlConnection)connection).State == ConnectionState.Closed)
            {
                conn = (SqlConnection)connection;
                conn.Open();
                connectOK = true;
            }
            else { connectOK = true; }
            if (!connectOK) { return connectOK; }
            #endregion

            // 透過Merge 作 batch insert/ update/ delete
            SQL sql = new SQL();
            sql.SetConnection(conn)
                .SetTableStructure(tableSchema)
                .SetSourceTable(data);
            switch (option)
            {
                case SQL.MergeOption.Insert:
                    // 只作Insert into (not Merge)
                    return SQL.BulkInsertForColumns(sql);
                case SQL.MergeOption.InsertUpdate:
                    // 使用Merge 作Insert / update
                    return SQL.BulkMergeInsertUpdate(sql);
                case SQL.MergeOption.InsertUpdate_SafeDelete:
                    // 先 Delete (not merge) , 再使用Merge 作Insert / update
                    return SQL.BulkMergeInsertUpdate_SafeDelete(sql);
            }

            // false for no option matched.
            return false;
        }
    }
}
