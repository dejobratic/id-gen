// See https://aka.ms/new-console-template for more information

// TODO: Provide sample code for id generation
// TODO: Add support for multiple id generators: Snowflake, Nuid, Guid, HiLo, etc.
// TODO: Consider sequence overflow strategy where applicable


using IdGen;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddIdGen()
    .BuildServiceProvider();

var idGenerator = serviceProvider.GetRequiredService<IIdGenerator>();

for (var i = 0; i < 10; i++)
{
    var id = idGenerator.NewId();
    Console.WriteLine($"Id: {id}");
}

Console.WriteLine("Done!");