using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static VRM.Utilities.Excel.Emums;

namespace VRM.Utilities.Excel
{
    public class ColumnValue
    {
        public int ColIndex { get; set; }

        public int RowIndex { get; set; }

        public bool IsValue { get; set; }

        public string TilteValue { get; set; }

        public object FieldValue { get; set; }

        public string FieldName { get; set; }

        public bool IsMerged { get; set; }

        public Aspose.Cells.Range Range { get; set; }

        public Aspose.Cells.Style Style { get; set; }

        public ValueDataType ValueType { get; set; }
    }
}
