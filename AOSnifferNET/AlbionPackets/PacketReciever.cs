using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace AOSnifferNET
{
    class PacketReciever
    {
        readonly PacketHandler photonParser;
        readonly Thread photonThread;
        readonly string localIp;
        bool serverDetected = false;

        public PacketReciever()
        {
            this.photonParser = new PacketHandler();
            this.localIp = GetLocalIPAddress();

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

        private string GetLocalIPAddress()
        {
            var candidates = NetworkInterface.GetAllNetworkInterfaces()
                .Where(ni =>
                    ni.OperationalStatus == OperationalStatus.Up &&
                    ni.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    ni.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                .SelectMany(ni => ni.GetIPProperties().UnicastAddresses)
                .Where(addr =>
                    addr.Address.AddressFamily == AddressFamily.InterNetwork &&
                    !IPAddress.IsLoopback(addr.Address))
                .Select(addr => addr.Address)
                .ToList();

            var preferred = candidates
                .OrderByDescending(ip => GetIPPriority(ip))
                .FirstOrDefault();

            if (preferred == null)
                throw new Exception("No se pudo determinar la IP local.");

            Console.WriteLine($"Local IP detected: {preferred}");
            return preferred.ToString();
        }

        private int GetIPPriority(IPAddress ip)
        {
            var bytes = ip.GetAddressBytes();

            if (bytes[0] == 192 && bytes[1] == 168) return 3; // Red doméstica/corporativa típica
            if (bytes[0] == 10) return 2; // Red corporativa amplia
            if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) return 1; // Privado legítimo (excluye Docker/VMs)

            return 0; // Pública, Docker u otras
        }

        private void CreateListener()
        {
            List<ILiveDevice> devicesOpened = new List<ILiveDevice>();
            Console.WriteLine("Start Listening for Devices...");

            bool isLinux = File.Exists("/proc/sys/kernel/ostype");
            string[] virtualKeywords =
            {
                "loopback",
                "npcap",
                "vmware",
                "hyper-v",
                "vbox",
                "tap",
                "wan miniport",
                "wsl",
                "pseudo",
                "virtual",
                "bluetooth"
            };

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

                    foreach (ILiveDevice deviceSelected in allDevices)
                    {
                        if (string.IsNullOrEmpty(deviceSelected.Description))
                            continue;

                        string desc = deviceSelected.Description.ToLowerInvariant();
                        if (!isLinux)
                        {
                            //Console.WriteLine(deviceSelected.Description.ToLowerInvariant());
                            bool isVirtual = virtualKeywords.Any(k => desc.Contains(k));
                            if (isVirtual)
                                continue;
                        }
                        else
                        {
                            if (!desc.Contains("pseudo-device"))
                                continue;
                        }

                        if (devicesOpened.Contains(deviceSelected))
                            continue;

                        Console.WriteLine($"Open physical device: {deviceSelected.Description}");

                        deviceSelected.OnPacketArrival += PacketHandler;
                        deviceSelected.Open(DeviceModes.Promiscuous, 1);
                        deviceSelected.StartCapture();
                        devicesOpened.Add(deviceSelected);
                    }

                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error while listening for devices: {e.Message}");
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

                if (udp_packet != null)
                {
                    if (packet.PayloadPacket is IPv4Packet ip_packet)
                    {
                        if (ip_packet.SourceAddress.ToString() != this.localIp &&
                            ip_packet.DestinationAddress.ToString() != this.localIp)
                        {
                            return;
                        }

                        if (udp_packet.SourcePort == 5056 || udp_packet.DestinationPort == 5056)
                        {
                            if (!serverDetected)
                            {
                                string gameServerIP = ip_packet.SourceAddress.ToString() == this.localIp
                                    ? ip_packet.DestinationAddress.ToString()
                                    : ip_packet.SourceAddress.ToString();
                                Console.WriteLine(gameServerIP.ToString());
                                if (gameServerIP.StartsWith("5.188.125.") || gameServerIP.StartsWith("85.234.70.")) // Second one if inside avalonian roads
                                {
                                    Console.WriteLine("[ServerRegion][{\"server\":\"America\"}]");
                                    serverDetected = true;
                                }
                                else if (gameServerIP.StartsWith("5.45.187."))
                                {
                                    Console.WriteLine("[ServerRegion][{\"server\":\"Asia\"}]");
                                    serverDetected = true;
                                }
                                else if (gameServerIP.StartsWith("193.169.238."))
                                {
                                    Console.WriteLine("[ServerRegion][{\"server\":\"Europe\"}]");
                                    serverDetected = true;
                                }
                            }

                            this.photonParser.ReceivePacket(udp_packet.PayloadData);
                        }
                        else if (udp_packet.SourcePort == 5055 || udp_packet.DestinationPort == 5055)
                        {
                            if (packet.PayloadPacket is IPv4Packet ip_pkt)
                            {
                                if (ip_pkt.SourceAddress.ToString() == "5.188.125.60" ||
                                    ip_pkt.SourceAddress.ToString() == "5.45.187.118")
                                {
                                    var output = new StreamWriter(Console.OpenStandardOutput());
                                    output.WriteLine("[onLogin][{\"status\":\"New Packet\"}]");
                                    output.Flush();
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }
    }
}