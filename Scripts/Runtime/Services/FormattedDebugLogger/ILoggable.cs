namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger
{
    public interface ILoggable
    {
        FormattedLogger GetFormattedLogger();
        string GetName();
    }
}