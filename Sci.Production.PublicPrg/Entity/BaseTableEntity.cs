using System.Data;

namespace Sci.Production.Prg.Entity
{
    public abstract class BaseTableEntity
    {
        /// <summary>
        /// 以完全預設值初始化執行個體
        /// </summary>
        public BaseTableEntity()
        {
        }

        /// <summary>
        /// 以指定的資料列內容初始化執行個體
        /// </summary>
        /// <param name="row">指定的資料列</param>
        public BaseTableEntity(DataRow row)
        {
            this.SetRowAsInitializeValue(row);
        }

        /// <summary>
        /// 以指定的資料列內容初始化執行個體
        /// </summary>
        /// <param name="row">指定的資料列</param>
        protected virtual void SetRowAsInitializeValue(DataRow row)
        {
            ProjExts.DatarowFillObject(row, this);
        }
    }
}
