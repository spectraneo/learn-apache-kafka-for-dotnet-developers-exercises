using HeartRateZoneService.Domain;
using Confluent.Kafka;
using static Confluent.Kafka.ConfigPropertyNames;

namespace HeartRateZoneService.Workers;

public class HeartRateZoneWorker : BackgroundService
{
    private readonly ILogger<HeartRateZoneWorker> _logger;
    private readonly IConsumer<String, Biometrics> _consumer;
    private readonly string BiometricsImportedTopicName = "BiometricsImported";

    public HeartRateZoneWorker(IConsumer<string, Biometrics> consumer, ILogger<HeartRateZoneWorker> logger)
    {
        _consumer = consumer;
        _logger = logger;
        logger.LogInformation("HeartRateZoneWorker is Active.");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(BiometricsImportedTopicName);
        while(!stoppingToken.IsCancellationRequested)
        {
            var result = _consumer.Consume(stoppingToken);
            await HandleMessage(result.Message.Value, stoppingToken);
        }
        _consumer.Close();
    }   

    protected virtual async Task HandleMessage(Biometrics biometrics, CancellationToken stoppingToken)
    {
        _logger.LogInformation("Message Received: " + biometrics.DeviceId);
        await Task.CompletedTask;
    }
}
