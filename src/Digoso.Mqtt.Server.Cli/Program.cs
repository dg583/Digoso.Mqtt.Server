using System;
using System.Device.Gpio;
using System.Threading;
using Digoso.Mqtt.Server;
using Iot.Device.Button;
using MQTTnet.Client;
using MQTTnet.Server;




//GPIO.setup(10, GPIO.IN, pull_up_down = GPIO.PUD_DOWN)

int pin = 10;
using var controller = new GpioController(PinNumberingScheme.Board);
controller.OpenPin(pin, PinMode.InputPullDown);

Console.WriteLine("Hello Mqtt-World!");

IMqttServerOptions mqttServerOptions = new MqttServerOptions();


var optionsBuilder = new MqttServerOptionsBuilder()
    .WithConnectionValidator((e) => {
    })
    .WithConnectionBacklog(100)
    ;
mqttServerOptions = optionsBuilder.Build();

MqttServerManager manager = new MqttServerManager(mqttServerOptions);

await manager.StartServerAsync(server => {

    server.UseApplicationMessageReceivedHandler((e) => {

        Console.WriteLine($"Message received from '{e.ClientId}'; Topic: {e.ApplicationMessage.Topic}");
    });
});


while (true)
{
    if (controller.Read(pin).Equals(PinValue.Low))
    {
        Console.WriteLine($"{DateTime.Now:s} - LOW");
    }
    else
    {
        Console.WriteLine($"{DateTime.Now:s} - HIGH");
    }
    Thread.Sleep(500);
}

Console.WriteLine("MQTT-Server is running - Press any key to stop");

var key = Console.ReadKey();
