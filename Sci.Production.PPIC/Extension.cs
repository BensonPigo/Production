using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Ict;

using MsExcel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Class.Commons
{
    /// <summary>
    /// extension
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// GetRange
        /// </summary>
        /// <param name="sheet">Worksheet</param>
        /// <param name="rangeName">string</param>
        /// <returns>Range</returns>
        public static MsExcel.Range GetRange(this MsExcel.Worksheet sheet, string rangeName)
        {
            return sheet.get_Range(rangeName) as MsExcel.Range;
        }

        /// <summary>
        /// <![CDATA[GetValue<T>]]>
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="range">Range</param>
        /// <returns><![CDATA[T]]></returns>
        public static T GetValue<T>(this MsExcel.Range range)
        {
            return (T)range.get_Value();
        }

        /// <summary>
        /// SetValue
        /// </summary>
        /// <param name="range">Range</param>
        /// <param name="value">object</param>
        public static void SetValue(this MsExcel.Range range, object value)
        {
            range.Value = value;
        }

        /// <summary>
        /// GetRange
        /// </summary>
        /// <param name="sheet">Worksheet</param>
        /// <param name="cellA_X">int cellA_X</param>
        /// <param name="cellA_Y">int cellA_Y</param>
        /// <param name="cellB_X">int cellB_X</param>
        /// <param name="cellB_Y">int cellB_Y</param>
        /// <returns>Range</returns>
        public static MsExcel.Range GetRange(this MsExcel.Worksheet sheet, int cellA_X, int cellA_Y, int cellB_X, int cellB_Y)
        {
            return sheet.Range[sheet.Cells[cellA_Y, cellA_X], sheet.Cells[cellB_Y, cellB_X]];
        }

        /// <summary>
        /// 把二層的物件陣列，轉為單層二維物件陣列
        /// </summary>
        /// <param name="rowData"><![CDATA[IEnumerable<IEnumerable<object>>]]></param>
        /// <returns>object[,]</returns>
        public static object[,] DoubleArrayConvert2DArray(this IEnumerable<IEnumerable<object>> rowData)
        {
            var d1Length = rowData.Count();
            if (d1Length == 0)
            {
                return new object[0, 0];
            }

            var d2Length = rowData.Max(item => item.Count());
            if (d2Length == 0)
            {
                return new object[d1Length, 0];
            }

            var value2 = new object[d1Length, d2Length];

            rowData.Select((item1, index1) =>
            {
                var valueLevel2 = Enumerable.Range(0, d2Length)
                    .Select(index2 =>
                    {
                        if (item1.Count() > index2)
                        {
                            value2[index1, index2] = item1.Skip(index2).Take(1).First();
                        }

                        return true;
                    })
                    .ToList();
                return true;
            })
            .ToList();
            return value2;
        }

        /// <summary>
        /// InsertMutipleRows
        /// </summary>
        /// <param name="sheet">Worksheet</param>
        /// <param name="firstRow">Range</param>
        /// <param name="rowNumberToInsert">int</param>
        public static void InsertMutipleRows(this MsExcel.Worksheet sheet, MsExcel.Range firstRow, int rowNumberToInsert)
        {
            var insertPos = sheet.Cells[1, firstRow.Row + 1].EntireRow;
            int i = 1;
            int step = 1;
            while (i < rowNumberToInsert)
            {
                // copy the existing row(s)
                sheet.get_Range(
                    "$" + firstRow.Row + ":$" + (firstRow.Row + step - 1),
                    Type.Missing).Copy(Type.Missing);

                // insert copied rows
                insertPos.Insert(MsExcel.XlInsertShiftDirection.xlShiftDown, Type.Missing);

                i += step;

                int diff = rowNumberToInsert - i;
                if (diff > (step * 2))
                {
                    step *= 2;
                }
                else
                {
                    step = diff;
                }
            }
        }
    }
}