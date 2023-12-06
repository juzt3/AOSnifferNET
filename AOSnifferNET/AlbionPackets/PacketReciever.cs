using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;
using System;
using System.Linq;
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

            var allDevices = LibPcapLiveDeviceList.Instance;
            if (allDevices.Count < 1)
            {
                throw new Exception("No interfaces found! Make sure NPcap is installed.");
            }

            Console.WriteLine("Start");
            // Escuche todos los dispositivos en la máquina local.
            foreach (ILiveDevice deviceSelected in allDevices.ToList())
            {
                Thread tPackets = new Thread(() =>
                {
                    Console.WriteLine($"Open... {deviceSelected.Description}");
                    deviceSelected.OnPacketArrival += new PacketArrivalEventHandler(PacketHandler);
                    deviceSelected.Open(DeviceModes.Promiscuous, 1);
                    deviceSelected.StartCapture();
                })
                { };
                tPackets.Start();
            }
            Console.Read();
        }

        private void PacketHandler(object sender, PacketCapture e)
        {
            try
            {
                UdpPacket packet = Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data).Extract<UdpPacket>();
                if (packet != null && (packet.SourcePort == 5056 || packet.DestinationPort == 5056))
                {
                    photonParser.ReceivePacket(packet.PayloadData);
                }
            }
            catch
            {
                return;
            }
        }
    }
}
