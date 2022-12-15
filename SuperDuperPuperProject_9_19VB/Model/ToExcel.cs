using ClosedXML.Excel;
using System.Data;

namespace SuperDuperPuperProject_9_19VB.Model {
    internal class ToExcel {
        public ToExcel(DataTable table, string name) {
            XLWorkbook wb = new XLWorkbook();
            wb.Worksheets.Add(table, "KolhoZ");
            wb.SaveAs(name);

        }
    }
}
