using Ict;
using Ict.Win;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.PublicPrg
{
    /// <summary>
    /// Prgs
    /// </summary>
    public static class DataGridViewGeneratorExtensions
    {
        public static IDataGridViewGenerator MarkerLength(
            this IDataGridViewGenerator generator,
            string propertyName,
            string header,
            string name,
            IWidth width,
            Func<DataRow, bool> canEditData,
            string propertyName2 = "MarkerLength")
        {
            DataGridViewGeneratorMaskedTextColumnSettings settings = new DataGridViewGeneratorMaskedTextColumnSettings();

            settings.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string newValue = Prgs.SetMarkerLengthMaskString(e.FormattedValue.ToString());
                string oldValue = MyUtility.Convert.GetString(dr[propertyName]);
                if (!canEditData(dr) || newValue == oldValue)
                {
                    return;
                }

                dr[propertyName] = Prgs.SetMarkerLengthMaskString(e.FormattedValue.ToString());
                dr[propertyName2] = Prgs.SetMarkerLengthMaskString(e.FormattedValue.ToString());
                dr.EndEdit();
            };

            return generator.MaskedText(propertyName, "00Y00-0/0+0\"", header, name, width, settings);
        }
    }
}