namespace DiagnosticsSharp.Core
{

    public interface IDiagnosticRenderer
    {
        string Name { get; set; }
        string SectionTitle { get; set; }

        void Render();
    }
}
