// ReSharper disable CheckNamespace

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal interface IAnalyticsRequest
    {
        string EventType { get; }
        string ToString();
    }
}
