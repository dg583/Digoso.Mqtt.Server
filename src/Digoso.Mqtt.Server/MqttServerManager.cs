using System;

using MQTTnet.Server;
using MQTTnet;
using System.Threading.Tasks;

namespace Digoso.Mqtt.Server {
    public class MqttServerManager {
        private readonly IMqttServerOptions _options;
        private IMqttServer _mqttServer;

        public MqttServerManager(IMqttServerOptions options) {
            this._options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task StartServerAsync(Action<IMqttServer> configure) {
            this._mqttServer = new MqttFactory().CreateMqttServer();

            configure(this._mqttServer);

            await this._mqttServer.StartAsync(this._options);


        }

        public async Task StopServerAsync() {
            if(this._mqttServer == null) 
                return;

            await this._mqttServer.StopAsync();
        }

    }
}
