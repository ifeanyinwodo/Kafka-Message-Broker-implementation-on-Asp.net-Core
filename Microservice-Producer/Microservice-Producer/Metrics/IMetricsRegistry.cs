namespace Microservice_Producer.Metrics
{
    public interface IMetricsRegistry
    {
        Serilog.Core.Logger _receivedMessage();
    }
}