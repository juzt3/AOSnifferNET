using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;

namespace AOSnifferNET
{
    class PacketReciever
    {
        readonly PacketHandler photonParser;
        readonly Thread photonThread;

        public PacketReciever()
        {
            photonParser = new PacketHandler();
            try
            {
                photonThread = new Thread(() => CreateListener())
                {
                    Priority = ThreadPriority.Highest
                };
                photonThread.Start();
            }
            catch (Exception ea)
            {
                Console.WriteLine(ea.ToString());
            }
        }

        private void CreateListener()
        {
            // Tome la lista de dispositivos de la máquina local.
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            if (allDevices.Count == 0)
            {
                throw new Exception("No interfaces found! Make sure WinPcap is installed.");
            }

            // Encuentra un dispositivo que este conectado a internet
            String deviceName = "";
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                // discard because of standard reasons
                if ((ni.OperationalStatus != OperationalStatus.Up) ||
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) ||
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel))
                    continue;

                // this allow to filter modems, serial, etc.
                // I use 10000000 as a minimum speed for most cases
                if (ni.Speed < 10000000)
                    continue;

                // discard virtual cards (virtual box, virtual pc, etc.)
                if ((ni.Description.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (ni.Name.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0))
                    continue;

                // discard "Microsoft Loopback Adapter", it will not show as NetworkInterfaceType.Loopback but as Ethernet Card.
                if (ni.Description.Equals("Microsoft Loopback Adapter", StringComparison.OrdinalIgnoreCase))
                    continue;

                deviceName = ni.Id;
                //Console.WriteLine(deviceName);
                break;
            }
            // Escuche todos los dispositivos en la máquina local.
            //LivePacketDevice deviceSelected = allDevices.ToList()[0];
            //if (true)
            foreach (LivePacketDevice deviceSelected in allDevices.ToList())
            {

                //Console.WriteLine(deviceSelected.Name);
                //if (true)
                if (deviceSelected.Name.Contains(deviceName))
                {
                    Thread tPackets = new Thread(() =>
                    {
                        using (PacketCommunicator communicator = deviceSelected.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1))
                        {
                            try
                            {
                                // Compruebe la capa de enlace.Si el adaptador no es Ethernet, ignórelo.
                                if (communicator.DataLink.Kind == DataLinkKind.Ethernet)
                                {
                                    // Compila el filtro
                                    using (BerkeleyPacketFilter filter = communicator.CreateFilter("ip and udp"))
                                        communicator.SetFilter(filter);

                                    communicator.ReceivePackets(0, PacketHandler);
                                }
                            }
                            catch (Exception)
                            {
                                // Ignora si hay un error.
                            }
                        }
                    })
                    {
                        Priority = ThreadPriority.Highest
                    };
                    tPackets.Start();
                }
            }
        }

        private void PacketHandler(Packet packet)
        {
            try
            {
                IpV4Datagram ip = packet.Ethernet.IpV4;
                UdpDatagram udp = ip.Udp;

                if (udp == null || (udp.SourcePort != 5056 && udp.DestinationPort != 5056))
                {
                    if (udp.SourcePort != 5055 && udp.DestinationPort != 5055)
                    {
                        return;
                    }
                }
                photonParser.ReceivePacket(udp.Payload.ToArray());
                //Console.WriteLine(ip.Source.ToString());
                //Console.WriteLine(ip.Destination.ToString());
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
