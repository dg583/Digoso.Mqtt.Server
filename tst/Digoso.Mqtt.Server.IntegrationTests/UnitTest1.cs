using System.Threading;
using System;
using NUnit.Framework;

using static Digoso.Mqtt.Server.IntegrationTests.Tests;
using System.Threading.Tasks;
using MQTTnet.Server;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Client;

namespace Digoso.Mqtt.Server.IntegrationTests {
    public class Tests {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public async Task Test1() {
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionValidator((e) => {
                })
                .WithConnectionBacklog(100)
                ;

            string ip = "192.168.0.61";

            var factory = new MqttFactory();
            var client = factory.CreateMqttClient();


            var clientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(ip)
                .WithCleanSession()
                .WithClientId("client1")

                .Build();

            client.UseApplicationMessageReceivedHandler(e => { });
            var connectResult1 = await client.ConnectAsync(clientOptions, CancellationToken.None);

            var subscribeResult = await client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("my/topic").Build());



            var client2 = factory.CreateMqttClient();
            var clientOptions2 = new MqttClientOptionsBuilder()
                .WithTcpServer(ip)
                .WithCleanSession()
                .WithClientId("client2")

                .Build();
            var connectResult2 = await client2.ConnectAsync(clientOptions2, CancellationToken.None);

            client2.UseApplicationMessageReceivedHandler(e => { });

            var subscribeResult2 = await client2.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("my/topic").Build());

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("my/topic")
                .WithPayload("Hello World")
                //.WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            var publishResult = await client.PublishAsync(message, CancellationToken.None);



        }
    }
}