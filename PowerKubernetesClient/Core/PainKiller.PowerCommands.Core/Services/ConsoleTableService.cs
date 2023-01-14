using PainKiller.PowerCommands.Shared.Utils.DisplayTable;


namespace PainKiller.PowerCommands.Core.Services
{
    public static class ConsoleTableService
    {
        private static readonly Dictionary<string, IEnumerable<IColumnRender>> TableColumnRenderDefinitions = new();

        public static void RenderTable<T>(IEnumerable<T> items, IConsoleWriter consoleWriter) where T : new()
        {
            var tableITems = items.ToArray();
            if (!tableITems.Any()) return;
            var rows = ConsoleTable
                .From<T>(tableITems)
                .Configure(o => o.NumberAlignment = Alignment.Right)
                .Read(WriteFormat.Alternative).Split("\r\n");

            for (var rowIndex = 0; rowIndex < rows.Length; rowIndex++)
            {
                var row = rows[rowIndex];
                if (rowIndex < 3)
                {
                    consoleWriter.WriteHeadLine(row);
                    continue;
                }
                consoleWriter.WriteLine(row);
            }
        }

        public static void RenderConsoleCommandTable<T>(IEnumerable<T> items, IConsoleWriter consoleWriter) where T : class, IConsoleCommandTable, new()
        {
            var tableITems = items.ToArray();
            if (!tableITems.Any()) return;
            if (typeof(T).GetInterface(nameof(IConsoleCommandTable)) != null)
            {
                var consoleCommandTable = tableITems.First();
                RenderTable(tableITems, consoleCommandTable.GetColumnRenderOptionsAttribute().ToArray(), consoleWriter);
                return;
            }
            var rows = ConsoleTable
                .From<T>(tableITems)
                .Configure(o => o.NumberAlignment = Alignment.Right)
                .Read(WriteFormat.Alternative).Split("\r\n");

            for (var rowIndex = 0; rowIndex < rows.Length; rowIndex++)
            {
                var row = rows[rowIndex];
                if (rowIndex < 3)
                {
                    consoleWriter.WriteHeadLine(row);
                    continue;
                }
                consoleWriter.WriteLine(row);
            }
        }
        public static void AddTableColumnRenderDefinitions(string name, IEnumerable<IColumnRender> columnRenderDefinitions)
        {
            if (TableColumnRenderDefinitions.ContainsKey(name)) return;
            TableColumnRenderDefinitions.Add(name, columnRenderDefinitions);
        }
        private static void RenderTable<T>(IEnumerable<T> tableData, ColumnRenderOptionsAttribute[] columnRenderDefinitions, IConsoleWriter consoleWriter)
        {
            var rows = ConsoleTable
                .From<T>(tableData)
                .Configure(o => o.NumberAlignment = Alignment.Right)
                .Read(WriteFormat.Alternative).Split("\r\n");

            consoleWriter.WriteHeadLine(rows[0]);
            consoleWriter.WriteHeadLine(rows[1]);
            consoleWriter.WriteHeadLine(rows[2]);

            var renderCols = GetColumnRenders<T>(columnRenderDefinitions, consoleWriter).ToList();

            for (var index = 0; index < rows.Length; index++)
            {
                if (index < 3) continue;
                var row = rows[index];
                if (row.StartsWith("+-"))
                {
                    consoleWriter.WriteLine(row);
                    continue;
                }
                var cols = row.Split('|');
                for (var colIndex = 0; colIndex < cols.Length; colIndex++)
                {
                    if (colIndex == cols.Length - 1)
                    {
                        consoleWriter.WriteLine("");
                        break;
                    }
                    var colRender = renderCols[colIndex];
                    if (colIndex > 0) colRender = renderCols[colIndex];
                    colRender.Write(cols[colIndex]);
                }
            }
        }
        private static void WriteHeaderRow(string row, ColumnRenderOptionsAttribute[] columnRenderDefinitions, IConsoleWriter consoleWriter)
        {
            var cols = row.Split('|');
            var render = new ColumnRenderHeader(consoleWriter);
            for (var colIndex = 0; colIndex < cols.Length; colIndex++)
            {
                if (colIndex == cols.Length - 1)
                {
                    consoleWriter.WriteLine("");
                    break;
                }
                render.Write(cols[colIndex]);
            }
        }
        private static IEnumerable<IColumnRender> GetColumnRenders<T>(IEnumerable<ColumnRenderOptionsAttribute> columnRenderDefinitions, IConsoleWriter consoleWriter)
        {
            var renderCol = new List<IColumnRender>();
            foreach (var optionsAttribute in columnRenderDefinitions.OrderBy(c => c.Order))
            {
                IColumnRender render;
                switch (optionsAttribute.RenderFormat)
                {
                    case ColumnRenderFormat.None:
                        render = new ColumnRenderBase(consoleWriter);
                        break;
                    case ColumnRenderFormat.Standard:
                        render = new ColumnRenderStandard(consoleWriter);
                        break;
                    case ColumnRenderFormat.SucessOrFailure:
                        render = new ColumnRenderSuccsessOrFailure(consoleWriter, optionsAttribute.Trigger1, optionsAttribute.Trigger2, optionsAttribute.Mark);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                renderCol.Add(render);
            }
            renderCol.Insert(0, new ColumnRenderStandard(consoleWriter));
            renderCol.Add(new ColumnRenderBase(consoleWriter));
            AddTableColumnRenderDefinitions(typeof(T).Name, renderCol);
            return renderCol;
        }
    }
}