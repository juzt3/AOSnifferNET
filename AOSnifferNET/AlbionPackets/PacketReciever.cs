using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace AOSnifferNET
{
    class PacketReciever
    {
        readonly PacketHandler photonParser;
        readonly Thread photonThread;

        public PacketReciever()
        {
            this.photonParser = new PacketHandler();
            try
            {
                this.photonThread = new Thread(() => this.CreateListener()) { };
                this.photonThread.Start();
            }
            catch (Exception ea)
            {
                Console.WriteLine(ea.ToString());
            }
        }

        private void CreateListener()
        {
            List<ILiveDevice> devicesOpened = new List<ILiveDevice>();
            Console.WriteLine("Start Listening for Devices...");

            bool running = true;
            while (running)
            {
                try
                {
                    CaptureDeviceList.Instance.Refresh();
                    var allDevices = CaptureDeviceList.Instance;
                    if (allDevices.Count < 1)
                    {
                        throw new Exception("No interfaces found! Make sure NPcap is installed.");
                    }

                    // Verificar todos los dispositivos en cada iteración del bucle
                    foreach (ILiveDevice deviceSelected in allDevices)
                    {
                        // Verifica si el dispositivo ya está en la lista
                        if (!devicesOpened.Contains(deviceSelected))
                        {
                            Console.WriteLine($"Open... {deviceSelected.Description}");
                            deviceSelected.OnPacketArrival += this.PacketHandler;
                            deviceSelected.Open(DeviceModes.Promiscuous, 1);
                            deviceSelected.StartCapture();
                            devicesOpened.Add(deviceSelected);
                        }
                    }

                    // Espera unos segundos antes de volver a buscar nuevos dispositivos
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error while listening for devices: {e.Message}");
                }
            }

            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("Ctrl+C or Ctrl+Break has been pressed. Performing closing tasks...");
                StopDevices(devicesOpened);
                running = false;
                e.Cancel = true;
            };

            AppDomain.CurrentDomain.DomainUnload += (sender, e) =>
            {
                Console.WriteLine("Managing SIGTERM. Performing closing tasks...");
                StopDevices(devicesOpened);
                running = false;
                Console.WriteLine("Closure completed.");
            };
        }

        private void StopDevices(List<ILiveDevice> devicesOpened)
        {
            foreach (ILiveDevice device in devicesOpened)
            {
                if (device != null && device.Started)
                {
                    device.StopCapture();
                    device.OnPacketArrival -= this.PacketHandler;
                    device.Close();
                }
            }
        }

        private void PacketHandler(object sender, PacketCapture e)
        {
            try
            {
                var packet = Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data);
                UdpPacket udp_packet = packet.Extract<UdpPacket>();
                if (udp_packet != null && (udp_packet.SourcePort == 5056 || udp_packet.DestinationPort == 5056))
                {
                    this.photonParser.ReceivePacket(udp_packet.PayloadData);
                }
                else
                {
                    if (udp_packet != null && (udp_packet.SourcePort == 5055 || udp_packet.DestinationPort == 5055))
                    {
                        if (packet.PayloadPacket is IPv4Packet ip_packet && (ip_packet.SourceAddress.ToString() == "5.188.125.60" || ip_packet.SourceAddress.ToString() == "5.45.187.118"))
                        {
                            var output = new StreamWriter(Console.OpenStandardOutput());
                            output.WriteLine("[onLogin][{status:\"New Packet\"}]");
                            output.Flush();
                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }

        public string getLastPacket()
        {
            return this.photonParser.getLastPacket();
        }
    }
}
