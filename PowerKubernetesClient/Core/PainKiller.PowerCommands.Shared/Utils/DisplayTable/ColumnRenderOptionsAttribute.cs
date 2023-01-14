namespace PainKiller.PowerCommands.Shared.Utils.DisplayTable
{
    public class ColumnRenderOptionsAttribute : Attribute
    {
        public ColumnRenderOptionsAttribute(string caption, int order = 100, bool ingnore = false, ColumnRenderFormat renderFormat = ColumnRenderFormat.Standard, string trigger1 = "", string trigger2 = "", string mark = "")
        {
            Order = order;
            Ingnore = ingnore;
            RenderFormat = renderFormat;
            Caption = caption;
            Trigger1 = trigger1;
            Trigger2 = trigger2;
            Mark = mark;
        }
        public int Order { get; }
        public bool Ingnore { get; }
        public ColumnRenderFormat RenderFormat { get; }
        public string Caption { get; }
        public string Trigger1 { get; }
        public string Trigger2 { get; }
        public string Mark { get; }
    }
}