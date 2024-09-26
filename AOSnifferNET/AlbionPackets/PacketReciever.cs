﻿using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace AOSnifferNET
{
    public class PacketReciever
    {
        readonly public PacketHandler photonParser;
        readonly Thread photonThread;

        public PacketReciever()
        {
            photonParser = new PacketHandler();

            try
            {
                photonThread = new Thread(() => CreateListener()) { };
                photonThread.Start();
            }
            catch (Exception ea)
            {
                Console.WriteLine(ea.ToString());
            }
        }

        private void CreateListener()
        {

            var allDevices = CaptureDeviceList.Instance;

            if (allDevices.Count < 1)
            {
                throw new Exception("No interfaces found! Make sure NPcap is installed.");
            }

            List<ILiveDevice> devicesOpened = new List<ILiveDevice>();
            Console.WriteLine("Start");

            // Escuche todos los dispositivos en la máquina local.
            foreach (ILiveDevice deviceSelected in allDevices)
            {
                if (!string.IsNullOrEmpty(deviceSelected.Description))
                {
                    if (deviceSelected.Description.ToLower().Contains("virtual"))
                        continue;

                    if (deviceSelected.Description.ToLower().Contains("loopback"))
                        continue;

                    if (deviceSelected.Description.ToLower().Contains("wan"))
                        continue;

                    if (deviceSelected.Description.ToLower().Contains("bluetooth"))
                        continue;

                    if (deviceSelected.Description.ToLower().Contains("pseudo"))
                        continue;

                    if (deviceSelected.Description.ToLower().Contains("filter"))
                        continue;
                }


                Console.WriteLine($"Open... {deviceSelected.Description}");
                deviceSelected.OnPacketArrival += PacketHandler;
                deviceSelected.Open(DeviceModes.Promiscuous, 1);
                deviceSelected.StartCapture();
                devicesOpened.Add(deviceSelected);
            }

            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("Ctrl+C or Ctrl+Break has been pressed. Performing closing tasks...");

                // Stoping Capture
                foreach (ILiveDevice device in devicesOpened)
                {
                    if (device != null && device.Started)
                    {
                        device.StopCapture();
                        device.OnPacketArrival -= PacketHandler;
                        device.Close();
                    }
                }

                e.Cancel = true;
            };

            AppDomain.CurrentDomain.DomainUnload += (sender, e) =>
            {
                Console.WriteLine("Managing SIGTERM. Performing closing tasks...");

                // Stoping Capture
                foreach (ILiveDevice device in devicesOpened)
                {
                    if (device != null && device.Started)
                    {
                        device.StopCapture();
                        device.OnPacketArrival -= PacketHandler;
                        device.Close();
                    }
                }

                Console.WriteLine("Closure completed.");
            };

            Console.Read();
            Console.Read();
        }

        private void PacketHandler(object sender, PacketCapture e)
        {
            try
            {
                var packet = Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data);

                UdpPacket udp_packet = packet.Extract<UdpPacket>();

                if (udp_packet != null && (udp_packet.SourcePort == 5056 || udp_packet.DestinationPort == 5056))
                {
                    photonParser.ReceivePacket(udp_packet.PayloadData);
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
    }
}
