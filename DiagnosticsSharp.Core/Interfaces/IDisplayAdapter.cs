namespace DiagnosticsSharp.Core.Interfaces
{
    public interface IDisplayAdapter
    {
        string DriverVersion { get; set; }
        string Frequency { get; set; }
        string Name { get; set; }
        string FullName { get; set; }
    }
}