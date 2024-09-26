using Newtonsoft.Json;
using PhotonPackageParser;
using System.IO;
using System;

namespace AOSnifferNET
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PacketReciever packetReciever = new PacketReciever();

            packetReciever.photonParser.OnRequestMove += PhotonParser_OnRequestMove;
            packetReciever.photonParser.OnRequestMount += PhotonParser_OnRequestMount;
            packetReciever.photonParser.OnRequestAuctionGetOffers += PhotonParser_OnRequestAuctionGetOffers;
            packetReciever.photonParser.OnRequestAuctionGetRequests += PhotonParser_OnRequestAuctionGetRequests;
            packetReciever.photonParser.OnRequestRegisterToObject += PhotonParser_OnRequestRegisterToObject;
            packetReciever.photonParser.OnRequestUnRegisterFromObject += PhotonParser_OnRequestUnRegisterFromObject;

            packetReciever.photonParser.OnResponseJoin += PhotonParser_OnResponseJoin;
            packetReciever.photonParser.OnResponseAuctionGetOffers += PhotonParser_OnResponseAuctionGetOffers;
            packetReciever.photonParser.OnResponseAuctionGetRequests += PhotonParser_OnResponseAuctionGetRequests;
            packetReciever.photonParser.OnResponseAuctionGetItemAverageValue += PhotonParser_OnResponseAuctionGetItemAverageValue;          


            packetReciever.photonParser.OnEventMove += PhotonParser_OnEventMove;
            packetReciever.photonParser.OnEventCharacterEquipmentChanged += PhotonParser_OnEventCharacterEquipmentChanged;
            packetReciever.photonParser.OnEventNewExit += PhotonParser_OnEventNewExit;
            packetReciever.photonParser.OnEventInventoryPutItem += PhotonParser_OnEventInventoryPutItem;
            packetReciever.photonParser.OnNewEquipmentItem += PhotonParser_OnNewEquipmentItem;
            packetReciever.photonParser.OnNewSimpleItem += PhotonParser_OnNewSimpleItem;
            packetReciever.photonParser.OnNewFurnitureItem += PhotonParser_OnNewFurnitureItem;
            packetReciever.photonParser.OnNewJournalItem += PhotonParser_OnNewJournalItem;
            packetReciever.photonParser.OnNewLaborerItem += PhotonParser_OnNewLaborerItem;
            packetReciever.photonParser.OnEventAttachItemContainer += PhotonParser_OnEventAttachItemContainer;
            packetReciever.photonParser.OnEventNewLoot += PhotonParser_OnEventNewLoot;
            packetReciever.photonParser.OnEventNewCharacter += PhotonParser_OnEventNewCharacter;
            packetReciever.photonParser.OnEventevLeave += PhotonParser_OnEventevLeave;
            packetReciever.photonParser.OnEventMounted += PhotonParser_OnEventMounted;
            packetReciever.photonParser.OnEventNewMountObject += PhotonParser_OnEventNewMountObject;
            packetReciever.photonParser.OnEventNewMob += PhotonParser_OnEventNewMob;
            packetReciever.photonParser.OnEventJoinFinished += PhotonParser_OnEventJoinFinished;
            packetReciever.photonParser.OnEventUpdateMoney += PhotonParser_OnEventUpdateMoney;
            packetReciever.photonParser.OnEventNewHarvestableObject += PhotonParser_OnEventNewHarvestableObject;
            packetReciever.photonParser.OnEventNewSimpleHarvestableObject += PhotonParser_OnEventNewSimpleHarvestableObject;
            packetReciever.photonParser.OnEventNewSimpleHarvestableObjectList += PhotonParser_OnEventNewSimpleHarvestableObjectList;
            packetReciever.photonParser.OnEventHarvestableChangeState += PhotonParser_OnEventHarvestableChangeState;
            packetReciever.photonParser.OnEventMobChangeState += PhotonParser_OnEventMobChangeState;
            packetReciever.photonParser.OnEventInCombatStateUpdate += PhotonParser_OnEventInCombatStateUpdate;
            packetReciever.photonParser.OnEventHealthUpdate += PhotonParser_OnEventHealthUpdate;
            packetReciever.photonParser.OnEventAttack += PhotonParser_OnEventAttack;
            packetReciever.photonParser.OnEventNewPortalExit += PhotonParser_OnEventNewPortalExit;
            packetReciever.photonParser.OnEventActiveSpellEffectsUpdate += PhotonParser_OnEventActiveSpellEffectsUpdate;
            packetReciever.photonParser.OnEventHarvestStart += PhotonParser_OnEventHarvestStart;
            packetReciever.photonParser.OnEventHarvestFinished += PhotonParser_OnEventHarvestFinished;
            packetReciever.photonParser.OnEventNewFloatObject += PhotonParser_OnEventNewFloatObject;
            packetReciever.photonParser.OnEventNewFishingZoneObject += PhotonParser_OnEventNewFishingZoneObject;
            packetReciever.photonParser.OnEventFishingMiniGame += PhotonParser_OnEventFishingMiniGame;
            packetReciever.photonParser.OnEventNewBuilding += PhotonParser_OnEventNewBuilding;
            packetReciever.photonParser.OnEventEasyAntiCheatMessageToClient += PhotonParser_OnEventEasyAntiCheatMessageToClient;
            packetReciever.photonParser.OnEventCastHits += PhotonParser_OnEventCastHits;
            packetReciever.photonParser.OnEventCastStart += PhotonParser_OnEventCastStart;
            packetReciever.photonParser.OnEventCastTimeUpdate += PhotonParser_OnEventCastTimeUpdate;
        }

        private static void printOperationInfo(string type, OperationCodes opCode, Object obj)
        {
            string jsonPacket;
            jsonPacket = JsonConvert.SerializeObject(obj);
            string outLine = "[onRequest][" + (int)opCode + "] " + opCode + ": " + jsonPacket;
            
            var output = new StreamWriter(Console.OpenStandardOutput());
            output.WriteLine(outLine);
            output.Flush();

            //Console.WriteLine(outLine);
            //Console.Out.Flush();
        }

        private static void printOperationInfo(EventCodes opCode, Object obj)
        {
            string jsonPacket;
            jsonPacket = JsonConvert.SerializeObject(obj);
            string outLine = "[OnEvent][" + (int)opCode + "] " + opCode + ": " + jsonPacket;

            var output = new StreamWriter(Console.OpenStandardOutput());
            output.WriteLine(outLine);
            output.Flush();

            //Console.WriteLine(outLine);
            //Console.Out.Flush();
        }

        #region OnEvent

        private static void PhotonParser_OnEventMove(Entity data)
        {
            printOperationInfo(EventCodes.Move, data);
        }

        private static void PhotonParser_OnEventCharacterEquipmentChanged(evCharacterEquipmentChanged data)
        {
            printOperationInfo(EventCodes.CharacterEquipmentChanged, data);
        }

        private static void PhotonParser_OnEventNewExit(evNewExit data)
        {
            printOperationInfo(EventCodes.NewExit, data);
        }

        private static void PhotonParser_OnEventInventoryPutItem(evInventoryPutItem data)
        {
            printOperationInfo(EventCodes.InventoryPutItem, data);
        }

        private static void PhotonParser_OnNewEquipmentItem(evNewSimpleItem data)
        {
            printOperationInfo(EventCodes.NewEquipmentItem, data);
        }

        private static void PhotonParser_OnNewSimpleItem(evNewSimpleItem data)
        {
            printOperationInfo(EventCodes.NewSimpleItem, data);
        }

        private static void PhotonParser_OnNewFurnitureItem(evNewSimpleItem data)
        {
            printOperationInfo(EventCodes.NewFurnitureItem, data);
        }

        private static void PhotonParser_OnNewJournalItem(evNewSimpleItem data)
        {
            printOperationInfo(EventCodes.NewJournalItem, data);
        }

        private static void PhotonParser_OnNewLaborerItem(evNewSimpleItem data)
        {
            printOperationInfo(EventCodes.NewLaborerItem, data);
        }

        private static void PhotonParser_OnEventAttachItemContainer(evAttachItemContainer data)
        {
            printOperationInfo(EventCodes.AttachItemContainer, data);
        }

        private static void PhotonParser_OnEventNewLoot(evNewLoot data)
        {
            printOperationInfo(EventCodes.NewLoot, data);
        }

        private static void PhotonParser_OnEventNewCharacter(evNewCharacter data)
        {
            printOperationInfo(EventCodes.NewCharacter, data);
        }

        private static void PhotonParser_OnEventevLeave(evLeave data)
        {
            printOperationInfo(EventCodes.Leave, data);
        }

        private static void PhotonParser_OnEventMounted(evMounted data)
        {
            printOperationInfo(EventCodes.Mounted, data);
        }

        private static void PhotonParser_OnEventNewMountObject(evNewMountObject data)
        {
            printOperationInfo(EventCodes.NewMountObject, data);
        }

        private static void PhotonParser_OnEventNewMob(evNewMob data)
        {
            printOperationInfo(EventCodes.NewMob, data);
        }

        private static void PhotonParser_OnEventJoinFinished(evJoinFinished data)
        {
            printOperationInfo(EventCodes.JoinFinished, data);
        }

        private static void PhotonParser_OnEventUpdateMoney(evUpdateSilver data)
        {
            printOperationInfo(EventCodes.UpdateMoney, data);
        }

        private static void PhotonParser_OnEventNewHarvestableObject(HarvestableObject data)
        {
            printOperationInfo(EventCodes.NewHarvestableObject, data);
        }

        private static void PhotonParser_OnEventNewSimpleHarvestableObject(HarvestableObject data)
        {
            printOperationInfo(EventCodes.NewSimpleHarvestableObject, data);
        }

        private static void PhotonParser_OnEventNewSimpleHarvestableObjectList(HarvestableObjectList data)
        {
            printOperationInfo(EventCodes.NewSimpleHarvestableObjectList, data);
        }

        private static void PhotonParser_OnEventHarvestableChangeState(HarvestableChangeState data)
        {
            printOperationInfo(EventCodes.HarvestableChangeState, data);
        }

        private static void PhotonParser_OnEventMobChangeState(evMobChangeState data)
        {
            printOperationInfo(EventCodes.MobChangeState, data);
        }

        private static void PhotonParser_OnEventInCombatStateUpdate(InCombatStateUpdate data)
        {
            printOperationInfo(EventCodes.InCombatStateUpdate, data);
        }

        private static void PhotonParser_OnEventHealthUpdate(evHealthUpdate data)
        {
            printOperationInfo(EventCodes.HealthUpdate, data);
        }

        private static void PhotonParser_OnEventAttack(evAttack data)
        {
            printOperationInfo(EventCodes.Attack, data);
        }

        private static void PhotonParser_OnEventNewPortalExit(evNewPortalExit data)
        {
            printOperationInfo(EventCodes.NewPortalExit, data);
        }

        private static void PhotonParser_OnEventActiveSpellEffectsUpdate(evActiveSpellEffectsUpdate data)
        {
            printOperationInfo(EventCodes.ActiveSpellEffectsUpdate, data);
        }

        private static void PhotonParser_OnEventHarvestStart(evHarvestStart data)
        {
            printOperationInfo(EventCodes.HarvestStart, data);
        }

        private static void PhotonParser_OnEventHarvestFinished(evHarvestFinished data)
        {
            printOperationInfo(EventCodes.HarvestFinished, data);
        }

        private static void PhotonParser_OnEventNewFloatObject(evNewFloatObject data)
        {
            printOperationInfo(EventCodes.NewFloatObject, data);
        }

        private static void PhotonParser_OnEventNewFishingZoneObject(evNewFishingZoneObject data)
        {
            printOperationInfo(EventCodes.NewFishingZoneObject, data);
        }

        private static void PhotonParser_OnEventFishingMiniGame(evFishingMiniGame data)
        {
            printOperationInfo(EventCodes.FishingMiniGame, data);
        }

        private static void PhotonParser_OnEventNewBuilding(evNewBuilding data)
        {
            printOperationInfo(EventCodes.NewBuilding, data);
        }

        private static void PhotonParser_OnEventEasyAntiCheatMessageToClient(evEasyAntiCheatMessageToClient data)
        {
            printOperationInfo(EventCodes.EasyAntiCheatMessageToClient, data);
        }

        private static void PhotonParser_OnEventCastHits(evCastHits data)
        {
            printOperationInfo(EventCodes.CastHits, data);
        }

        private static void PhotonParser_OnEventCastStart(evCastStart data)
        {
            printOperationInfo(EventCodes.CastStart, data);
        }

        private static void PhotonParser_OnEventCastTimeUpdate(evCastTimeUpdate data)
        {
            printOperationInfo(EventCodes.CastTimeUpdate, data);
        }

        #endregion

        #region onOperation Request

        private static void PhotonParser_OnRequestMove(opMove data)
        {
            printOperationInfo("OnRequest", OperationCodes.Move, data);
        }

        private static void PhotonParser_OnRequestMount(opMount data)
        {
            printOperationInfo("OnRequest", OperationCodes.Mount, data);
        }

        private static void PhotonParser_OnRequestAuctionGetOffers(opAuctionGetAny data)
        {
            printOperationInfo("OnRequest", OperationCodes.AuctionGetOffers, data);
        }

        private static void PhotonParser_OnRequestAuctionGetRequests(opAuctionGetAny data)
        {
            printOperationInfo("OnRequest", OperationCodes.AuctionGetRequests, data);
        }

        private static void PhotonParser_OnRequestRegisterToObject(opRegisterToObject data)
        {
            printOperationInfo("OnRequest", OperationCodes.RegisterToObject, data);
        }

        private static void PhotonParser_OnRequestUnRegisterFromObject(UnRegisterFromObject data)
        {
            printOperationInfo("OnRequest", OperationCodes.UnRegisterFromObject, data);
        }

        #endregion

        #region onOperation Response

        private static void PhotonParser_OnResponseJoin(OpJoin data)
        {
            printOperationInfo("OnResponse", OperationCodes.Join, data);
        }

        private static void PhotonParser_OnResponseAuctionGetOffers(opAuctionGetOffersResponse data)
        {
            printOperationInfo("OnResponse", OperationCodes.AuctionGetOffers, data);
        }

        private static void PhotonParser_OnResponseAuctionGetRequests(opAuctionGetRequestsResponse data)
        {
            printOperationInfo("OnResponse", OperationCodes.AuctionGetRequests, data);
        }

        private static void PhotonParser_OnResponseAuctionGetItemAverageValue(opAuctionGetItemAverageValue data)
        {
            printOperationInfo("OnResponse", OperationCodes.AuctionGetItemAverageValue, data);
        }

        #endregion
    }
}
