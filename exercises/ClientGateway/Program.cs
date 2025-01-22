using ClientGateway;
using ClientGateway.Controllers;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TODO: Load a config of type ProducerConfig from the "Kafka" section of the config:
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection("Kafka"));


// TODO: Register an IProducer of type <String, String>:
builder.Services.AddSingleton<IProducer<string, string>>(sp =>
{
    var config = sp.GetRequiredService<IOptions<ProducerConfig>>().Value;
    return new ProducerBuilder<string, string>(config).Build();
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();