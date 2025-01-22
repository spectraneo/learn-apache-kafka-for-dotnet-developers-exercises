using System.Diagnostics.Metrics;
using System.Net;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using static Confluent.Kafka.ConfigPropertyNames;

namespace ClientGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientGatewayController : ControllerBase
{
    private string BiometricsImportedTopicName = "RawBiometricsImported";
    private readonly IProducer<String, String> _producer;
    private readonly ILogger<ClientGatewayController> _logger;

    public ClientGatewayController(IProducer<String, String> producer, ILogger<ClientGatewayController> logger)
    {
        _logger = logger;
        _producer = producer;
        logger.LogInformation("ClientGatewayController is Active.");
    }

    [HttpGet("Hello")]
    [ProducesResponseType(typeof(String), (int)HttpStatusCode.OK)]
    public String Hello()
    {
        _logger.LogInformation("Hello World");
        return "Hello World";
    }

    [HttpPost("Biometrics")]
    [ProducesResponseType(typeof(String), (int)HttpStatusCode.OK)]
    public async Task<AcceptedResult> RecordMeasurements(string metrics)
    {
        _logger.LogInformation("Accepted biometrics");
        var message = new Message<String, String>
        {
            Value = metrics
        };
        var result = await _producer.ProduceAsync(BiometricsImportedTopicName, message);
        _producer.Flush();
        return Accepted("", metrics);
    }
}



