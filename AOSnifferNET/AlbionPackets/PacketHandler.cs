﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhotonPackageParser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace AOSnifferNET
{
    public class PacketHandler : PhotonParser
    {
        public event RequestMove OnRequestMove;
        public event RequestMount OnRequestMount;
        public event RequestHarvestStart OnRequestHarvestStart;
        public event RequestAuctionGetOffers OnRequestAuctionGetOffers;
        public event RequestAuctionGetRequests OnRequestAuctionGetRequests;
        public event RequestRegisterToObject OnRequestRegisterToObject;
        public event RequestUnRegisterFromObject OnRequestUnRegisterFromObject;

        public event ResponseJoin OnResponseJoin;
        public event ResponseAuctionGetOffers OnResponseAuctionGetOffers;
        public event ResponseAuctionGetRequests OnResponseAuctionGetRequests;
        public event ResponseAuctionGetItemAverageValue OnResponseAuctionGetItemAverageValue;

        public event EventMove OnEventMove;
        public event EventCharacterEquipmentChanged OnEventCharacterEquipmentChanged;
        public event EventNewExit OnEventNewExit;
        public event EventInventoryPutItem OnEventInventoryPutItem;
        public event EventNewItem OnNewEquipmentItem;
        public event EventNewItem OnNewSimpleItem;
        public event EventNewItem OnNewFurnitureItem;
        public event EventNewItem OnNewJournalItem;
        public event EventNewItem OnNewLaborerItem;
        public event EventAttachItemContainer OnEventAttachItemContainer;
        public event EventNewLoot OnEventNewLoot;
        public event EventNewCharacter OnEventNewCharacter;
        public event EventLeave OnEventevLeave;
        public event EventMounted OnEventMounted;
        public event EventNewMountObject OnEventNewMountObject;
        public event EventNewMob OnEventNewMob;
        public event EventJoinFinished OnEventJoinFinished;
        public event EventUpdateMoney OnEventUpdateMoney;
        public event EventHarvestableObject OnEventNewHarvestableObject;
        public event EventHarvestableObject OnEventNewSimpleHarvestableObject;
        public event EventHarvestableObjectList OnEventNewSimpleHarvestableObjectList;
        public event EventHarvestableChangeState OnEventHarvestableChangeState;
        public event EventMobChangeState OnEventMobChangeState;
        public event EventInCombatStateUpdate OnEventInCombatStateUpdate;
        public event EventHealthUpdate OnEventHealthUpdate;
        public event EventAttack OnEventAttack;
        public event EventNewPortalExit OnEventNewPortalExit;
        public event EventActiveSpellEffectsUpdate OnEventActiveSpellEffectsUpdate;
        public event EventHarvestStart OnEventHarvestStart;
        public event EventHarvestFinished OnEventHarvestFinished;
        public event EventNewFloatObject OnEventNewFloatObject;
        public event EventNewFishingZoneObject OnEventNewFishingZoneObject;
        public event EventFishingMiniGame OnEventFishingMiniGame;
        public event EventNewBuilding OnEventNewBuilding;
        public event EventEasyAntiCheatMessageToClient OnEventEasyAntiCheatMessageToClient;
        public event EventCastHits OnEventCastHits;
        public event EventCastStart OnEventCastStart;
        public event EventCastTimeUpdate OnEventCastTimeUpdate;

        public PacketHandler() { }

        protected override void OnEvent(byte code, Dictionary<byte, object> parameters)
        {
            try
            {
                EventCodes evCode = 0;

                if (code == 1)
                {
                    parameters.TryGetValue((byte)252, out object val);

                    if (val == null) return;

                    if (!int.TryParse(val.ToString(), out int iCode)) return;

                    evCode = (EventCodes)iCode;
                }
                else
                {
                    evCode = (EventCodes)code;
                }

                switch (evCode)
                {
                    case EventCodes.Move:
                        onEventMove(parameters);
                        break;

                    case EventCodes.CharacterEquipmentChanged:
                        onCharacterEquipmentChanged(parameters);
                        break;

                    case EventCodes.NewExit:
                        onNewExit(parameters);
                        break;

                    case EventCodes.InventoryPutItem:
                        onInventoryPutItem(parameters);
                        break;

                    case EventCodes.NewEquipmentItem:
                        onNewEquipmentItem(parameters, evCode);
                        break;

                    case EventCodes.NewSimpleItem:
                        onNewSimpleItem(parameters, evCode);
                        break;

                    case EventCodes.NewFurnitureItem:
                        onNewFurnitureItem(parameters, evCode);
                        break;

                    case EventCodes.NewJournalItem:
                        onNewJournalItem(parameters, evCode);
                        break;

                    case EventCodes.NewLaborerItem:
                        onNewLaborerItem(parameters, evCode);
                        break;

                    case EventCodes.AttachItemContainer:
                        onAttachItemContainer(parameters);
                        break;

                    case EventCodes.NewLoot:
                        onNewLoot(parameters);
                        break;

                    case EventCodes.NewCharacter:
                        onNewCharacter(parameters);
                        break;

                    case EventCodes.Leave:
                        onLeaveEvent(parameters);
                        break;

                    case EventCodes.Mounted:
                        onMounted(parameters);
                        break;

                    case EventCodes.NewMountObject:
                        onNewMountObject(parameters);
                        break;

                    case EventCodes.NewMob:
                        onNewMob(parameters);
                        break;

                    case EventCodes.JoinFinished:
                        onJoinFinished(parameters);
                        break;

                    case EventCodes.UpdateMoney:
                        onUpdateSilver(parameters);
                        break;

                    case EventCodes.NewSimpleHarvestableObjectList:
                        onNewSimpleHarvestableObjectList(parameters);
                        break;

                    case EventCodes.NewSimpleHarvestableObject:
                        onNewSimpleHarvestableObject(parameters);
                        break;

                    case EventCodes.NewHarvestableObject:
                        onNewHarvestableObject(parameters);
                        break;

                    case EventCodes.HarvestableChangeState:
                        onHarvestableChangeState(parameters);
                        break;

                    case EventCodes.MobChangeState:
                        onMobChangeState(parameters);
                        break;

                    case EventCodes.InCombatStateUpdate:
                        onInCombatStateUpdate(parameters);
                        break;

                    case EventCodes.HealthUpdate:
                        onHealthUpdate(parameters);
                        break;

                    case EventCodes.Attack:
                        onAttack(parameters);
                        break;

                    case EventCodes.NewRandomDungeonExit:
                        onNewPortalExit(parameters);
                        break;

                    case EventCodes.ActiveSpellEffectsUpdate:
                        onActiveSpellEffectsUpdate(parameters);
                        break;

                    case EventCodes.HarvestStart:
                        onHarvestStart(parameters);
                        break;

                    case EventCodes.HarvestFinished:
                        onHarvestFinished(parameters);
                        break;

                    case EventCodes.NewFloatObject:
                        onNewFloatObject(parameters);
                        break;

                    case EventCodes.NewFishingZoneObject:
                        onNewFishingZoneObject(parameters);
                        break;

                    case EventCodes.FishingMiniGame:
                        onFishingMiniGame(parameters);
                        break;

                    case EventCodes.NewBuilding:
                        onNewBuilding(parameters);
                        break;

                    case EventCodes.EasyAntiCheatMessageToClient:
                        onEventEasyAntiCheatMessageToClient(parameters);
                        break;

                    case EventCodes.CastHits:
                        onEventCastHits(parameters);
                        break;

                    case EventCodes.CastStart:
                        onEventCastStart(parameters);
                        break;

                    case EventCodes.CastTimeUpdate:
                        onEventCastTimeUpdate(parameters);
                        break;

                    default:
                        debugPacket("OnEvent", code, parameters);
                        break;
                }
            }
            catch (Exception ex)
            {
                debugPacket("OnEventEx", code, parameters);
            }
        }

        protected override void OnRequest(byte operationCode, Dictionary<byte, object> parameters)
        {
            try
            {
                parameters.TryGetValue((byte)253, out object val);

                if (val == null) return;
                if (!int.TryParse(val.ToString(), out int iCode)) return;

                OperationCodes opCode = (OperationCodes)iCode;

                switch (opCode)
                {
                    case OperationCodes.Move:
                        onRequestMove(parameters);
                        break;

                    case OperationCodes.AuctionGetOffers:
                        onRequestAuctionGetOffers(parameters);
                        break;

                    case OperationCodes.AuctionGetRequests:
                        onRequestAuctionGetRequests(parameters);
                        break;

                    case OperationCodes.RegisterToObject:
                        onRequestRegisterToObject(parameters);
                        break;

                    case OperationCodes.UnRegisterFromObject:
                        onRequestUnRegisterFromObject(parameters);
                        break;

                    case OperationCodes.Mount:
                        onRequestMount(parameters);
                        break;

                    case OperationCodes.HarvestStart:
                        onReqHarvestStart(parameters);
                        break;

                    //case OperationCodes.HarvestCancel:
                    //    // No llega al minar un cuerpo
                    //    printOperationInfo(parameters, opCode, "onRequest");
                    //    break;

                    //case OperationCodes.AttackStart:
                    //    printOperationInfo(parameters, opCode, "onRequest");
                    //    break;

                    //case OperationCodes.MountCancel:
                    //    printOperationInfo(parameters, opCode, "onRequest");
                    //    break;

                    //case OperationCodes.ChangeCluster:
                    //    printOperationInfo(parameters, opCode, "onRequest");
                    //    break;

                    //case OperationCodes.GetGameServerByCluster:
                    //    // Cuando le pide al servidor la direccion dns del cluster
                    //    // GetGameServerByCluster: {"0":"0000","255":10,"253":16}
                    //    printOperationInfo(parameters, opCode, "onRequest");
                    //    break;

                    //case OperationCodes.GetReferralLink:
                    //    printOperationInfo(parameters, opCode, "onRequest");
                    //    break;

                    //case OperationCodes.EasyAntiCheatMessageToServer:
                    //    printOperationInfo(parameters, opCode, "onRequest");
                    //    break;

                    default:
                        debugPacket("OnRequest", operationCode, parameters);
                        break;
                }
            }
            catch (Exception ex)
            {
                debugPacket("OnRequestEx", operationCode, parameters);
            }
        }

        protected override void OnResponse(byte operationCode, short returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            try
            {
                parameters.TryGetValue((byte)253, out object val);

                if (val == null) return;
                if (!int.TryParse(val.ToString(), out int iCode)) return;

                OperationCodes opCode = (OperationCodes)iCode;

                switch (opCode)
                {
                    case OperationCodes.Join:
                        onResponseJoin(parameters);
                        break;

                    case OperationCodes.AuctionGetOffers:
                        onResponseAuctionGetOffers(parameters);
                        break;

                    case OperationCodes.AuctionGetRequests:
                        onResponseAuctionGetRequests(parameters);
                        break;

                    case OperationCodes.AuctionGetItemAverageValue:
                        onResponseAuctionGetItemAverageValue(parameters);
                        break;

                    //case OperationCodes.HarvestStart:
                    //    printOperationInfo(parameters, opCode, "onResponse");
                    //    break;

                    //case OperationCodes.HarvestCancel:
                    //    printOperationInfo(parameters, opCode, "onResponse");
                    //    break;

                    //case OperationCodes.ChangeCluster:
                    //    printOperationInfo(parameters, opCode, "onResponse");
                    //    break;

                    //case OperationCodes.GetGameServerByCluster:
                    //    //GetGameServerByCluster: {"0":"live01-win-28.dc02.albion.zone:5056","255":10,"253":16}
                    //    printOperationInfo(parameters, opCode, "onResponse");
                    //    break;

                    default:
                        debugPacket("OnResponse", operationCode, parameters);
                        break;
                }
            }
            catch (Exception ex)
            {
                debugPacket("OnResponseEx", operationCode, parameters);
            }
        }

        private void debugPacket(string type, int iCode, Object obj)
        {
            string jsonPacket = JsonConvert.SerializeObject(obj);
            string outLine = $"type:{type} iCode:{iCode} jsonPacket:{jsonPacket}";
            
            Console.WriteLine(outLine);
            Console.Out.Flush();
        }

        #region onOperation Event

        private void onNewBuilding(Dictionary<byte, object> parameters)
        {
            int packetID = int.Parse(parameters[0].ToString());
            string name = parameters[3].ToString();
            Single[] pos = (Single[])parameters[4];

            var nb = new evNewBuilding(packetID, name, pos);
            OnEventNewBuilding?.Invoke(nb);
        }

        private void onCharacterEquipmentChanged(Dictionary<byte, object> parameters)
        {
            short[] items = new short[10];
            short[] skills = new short[6];

            int index = 0;

            if (parameters[2].GetType() == typeof(Byte[]))
            {
                Byte[] itemList = (Byte[])parameters[2];

                foreach (Byte b in itemList)
                {
                    if (index >= 10)
                        break;

                    items[index] = Convert.ToInt16(b);
                    index++;
                }
            }
            else
            {
                Int16[] itemList = (Int16[])parameters[2];

                foreach (Int16 b in itemList)
                {
                    if (index >= 10)
                        break;

                    items[index] = b;
                    index++;
                }
            }

            index = 0;

            if (parameters[5].GetType() == typeof(Byte[]))
            {
                Byte[] skillList = (Byte[])parameters[5];

                foreach (Byte b in skillList)
                {
                    if (index >= 6)
                        break;

                    skills[index] = Convert.ToInt16(b);
                    index++;
                }
            }
            else
            {
                Int16[] skillList = (Int16[])parameters[5];

                foreach (Int16 b in skillList)
                {
                    if (index >= 6)
                        break;

                    skills[index] = b;
                    index++;
                }
            }

            var eqc = new evCharacterEquipmentChanged(items, skills);
            OnEventCharacterEquipmentChanged?.Invoke(eqc);
        }
        
        private void onNewExit(Dictionary<byte, object> parameters)
        {
            Single[] entryPos = new Single[2];

            if (parameters.ContainsKey(2))
            {
                entryPos = (Single[])parameters[2];
                
                var ne = new evNewExit(entryPos);
                OnEventNewExit?.Invoke(ne);
            }
        }
        
        private void onMobChangeState(Dictionary<byte, object> parameters)
        {
            int mobID = int.Parse(parameters[0].ToString());
            short enchantment = short.Parse(parameters[1].ToString());

            var cs = new evMobChangeState(mobID, enchantment);
            OnEventMobChangeState?.Invoke(cs);
        }
        
        private void onInventoryPutItem(Dictionary<byte, object> parameters)
        {
            int itemID = int.Parse(parameters[0].ToString());

            var putItem = new evInventoryPutItem(itemID);
            OnEventInventoryPutItem?.Invoke(putItem);            
        }

        private void onNewFishingZoneObject(Dictionary<byte, object> parameters)
        {
            //NewFishingZoneObject: { "0":1182,"1":[253.4,52.8],"2":3,"3":2,"4":"FishingNodeSwarm","252":341} 0: objectID 1: zone pos 2:charges (not present when empty) 3:times fished from 4:zone tipe
            
            int objectID = int.Parse(parameters[0].ToString());
            Single[] zonePos = (Single[])parameters[1];

            short charges = 0;            

            if (parameters.ContainsKey(2))
                charges = short.Parse(parameters[2].ToString());

            short fished = 0;

            if (parameters.ContainsKey(3))
                fished = short.Parse(parameters[3].ToString());

            string zoneType = parameters[4].ToString();

            var fishingZone = new evNewFishingZoneObject(objectID, zonePos, charges, fished, zoneType);
            OnEventNewFishingZoneObject?.Invoke(fishingZone);
        }

        private void onFishingMiniGame(Dictionary<byte, object> parameters)
        {
            int id = int.Parse(parameters[0].ToString());
            Single[] pos = (Single[])parameters[1];
            String type = (string)parameters[3].ToString();

            var portal = new evFishingMiniGame();
            OnEventFishingMiniGame?.Invoke(portal);
        }

        private void onNewFloatObject(Dictionary<byte, object> parameters)
        {
            // NewFloatObject: { "0":665769,"1":[-190.193344,59.3336449],"2":298.5448,"3":632262,"4":1,"252":340} 0: bobber ID 1: bobber pos 2:angle
            // 3:player ID 4:fishing state (1: bobber landed 2:fish bitten 3:playing minigam 4:catched a fish 5:failed)

            int bobberID = int.Parse(parameters[0].ToString());
            Single[] bobberPos = (Single[])parameters[1];
            float angleFromPlayer = float.Parse(parameters[2].ToString());
            int playerId = int.Parse(parameters[3].ToString());
            short fishingState = short.Parse(parameters[4].ToString());
            string readableState;

            switch (fishingState)
            {
                case 1:
                    readableState = "Floating";
                    break;

                case 2:
                    readableState = "Bitten";
                    break;

                case 3:
                    readableState = "Minigame";
                    break;

                case 4:
                    readableState = "Catched";
                    break;

                case 5:
                    readableState = "Lost";
                    break;

                default:
                    readableState = "";
                    break;
            }

            var bobber = new evNewFloatObject(bobberID, bobberPos, angleFromPlayer, playerId, readableState);
            OnEventNewFloatObject?.Invoke(bobber);
        }
        
        private void onHarvestStart(Dictionary<byte, object> parameters)
        {
            //HarvestStart: { "0":144653,"1":638045717068106818,"2":638045717068106818,"3":1263,"5":2.0,"6":-1,"7":-1,"252":54} 5: tiempo para terminar de harvestear
            
            int harvestableId = int.Parse(parameters[3].ToString());
            float time = float.Parse(parameters[5].ToString());

            var hs = new evHarvestStart(harvestableId, time);
            OnEventHarvestStart?.Invoke(hs);
        }
        
        private evNewSimpleItem onNewItem(Dictionary<byte, object> parameters)
        {
            int objectID = int.Parse(parameters[0].ToString());
            int itemID = int.Parse(parameters[1].ToString());
            short amount = short.Parse(parameters[2].ToString());
            long avgValue = 0;

            if (parameters.ContainsKey(4))
            {
                avgValue = long.Parse(parameters[4].ToString());
                avgValue = (long)avgValue / 10000;
            }

            var newItem = new evNewSimpleItem(objectID, itemID, amount, avgValue);
            return newItem;
        }
        
        private void onNewEquipmentItem(Dictionary<byte, object> parameters, EventCodes evCode)
        {
            var data = onNewItem(parameters);
            OnNewEquipmentItem?.Invoke(data);
        }
        
        private void onNewSimpleItem(Dictionary<byte, object> parameters, EventCodes evCode)
        {
            var data = onNewItem(parameters);
            OnNewSimpleItem?.Invoke(data);
        }
        
        private void onNewFurnitureItem(Dictionary<byte, object> parameters, EventCodes evCode)
        {
            var data = onNewItem(parameters);
            OnNewFurnitureItem?.Invoke(data);
        }
        
        private void onNewJournalItem(Dictionary<byte, object> parameters, EventCodes evCode)
        {
            var data = onNewItem(parameters);
            OnNewJournalItem?.Invoke(data);
        }
        
        private void onNewLaborerItem(Dictionary<byte, object> parameters, EventCodes evCode)
        {
            var data = onNewItem(parameters);
            OnNewLaborerItem?.Invoke(data);
        }
        
        private void onAttachItemContainer(Dictionary<byte, object> parameters)
        {
            // Estos itemsID son los objectID de eventos como
            // NewEquipmentItem: {"0":203114,"1":2984,"2":1,"4":6204632,"5":"Bettooo","6":2,"7":48000000,"8":[-1],"9":[-1],"252":28}
            // donde 0 es el objectID

            int objectID = int.Parse(parameters[0].ToString());
            byte[] ownerMarkID = (byte[])parameters[1];
            int[] itemsID = (int[])parameters[3];

            var itemContainer = new evAttachItemContainer(objectID, ownerMarkID, itemsID);
            OnEventAttachItemContainer?.Invoke(itemContainer);
        }
        
        private void onNewLoot(Dictionary<byte, object> parameters)
        {
            int lootId = int.Parse(parameters[0].ToString());
            Single[] pos = (Single[])parameters[4];

            var newloot = new evNewLoot(lootId, pos);
            OnEventNewLoot?.Invoke(newloot);
        }
        
        private void onHarvestFinished(Dictionary<byte, object> parameters)
        {
            //map[0:686375 1:637959518682696481 2:637959518687702927 3:2580 4:2 5:2 6:1 8:[] 9:[] 252:54]

            int playerId = int.Parse(parameters[0].ToString());
            int harvestableId = int.Parse(parameters[3].ToString());
            short gathered = short.Parse(parameters[4].ToString());

            short yield = 0;

            if (parameters.ContainsKey(5))
            {
                yield = short.Parse(parameters[5].ToString());
            }

            short premium = 0;

            if (parameters.ContainsKey(6))
            {
                premium = short.Parse(parameters[6].ToString());
            }

            short charges = 0;

            if (parameters.ContainsKey(7))
            {
                charges = short.Parse(parameters[7].ToString());
            }

            var hf = new evHarvestFinished(playerId, harvestableId, gathered, yield, premium, charges);
            OnEventHarvestFinished?.Invoke(hf);
        }
        
        private void onMounted(Dictionary<byte, object> parameters)
        {
            int playerId = int.Parse(parameters[0].ToString());
            bool isMounted = false;

            if (parameters.ContainsKey(2))
                isMounted = true;

            var mounted = new evMounted(playerId, isMounted);
            OnEventMounted?.Invoke(mounted);
        }
        
        public void onAttack(Dictionary<byte, object> parameters)
        {
            int attackerID = int.Parse(parameters[0].ToString());
            int targetID = int.Parse(parameters[2].ToString());

            var attackEv = new evAttack(attackerID, targetID);
            OnEventAttack?.Invoke(attackEv);
        }
        
        private void onNewPortalExit(Dictionary<byte, object> parameters)
        {
            int id = int.Parse(parameters[0].ToString());
            Single[] pos = (Single[])parameters[1];
            String type = (string)parameters[3].ToString();

            var portal = new evNewPortalExit(id, pos, type);
            OnEventNewPortalExit?.Invoke(portal);
        }

        private void onActiveSpellEffectsUpdate(Dictionary<byte, object> parameters)
        {
            // [10]evActiveSpellEffectsUpdate - map[0:655873 1:[685 279 339 509 399] 2:[100 389.11096 369.137 100 389.11096] 3:[100 233.6818 226.1142 100 233.6818] 4:[637945635939432036 637945670488995997 637945670488995997 637903462984937618 637945670488995997] 5:[1 10 10 1 10] 7:[75 - 110 36 73 - 110 36 73 - 110 36 9] 8:[208406056] 9:[9] 10:[0 0] 252:10]

            var ase = new evActiveSpellEffectsUpdate();
            OnEventActiveSpellEffectsUpdate?.Invoke(ase);
        }

        private void onNewHarvestableObject(Dictionary<byte, object> parameters)
        {
            // NewHarvestableObject: { "0":211813,"1":211239,"2":637903348349073726,"3":"+Z47IXg2YUWs1j2NjqNMZQ==","5":24,"6":112,"7":3,"8":[-374.0117,209.0445],"9":319.8794,"11":0,"252":36}
            
            int id = int.Parse(parameters[0].ToString());
            byte type = byte.Parse(parameters[5].ToString());
            byte tier = byte.Parse(parameters[7].ToString());
            Single[] pos = (Single[])parameters[8];
            parameters.TryGetValue((byte)10, out object _charges);
            byte charges = _charges == null ? (byte)0 : byte.Parse(_charges.ToString()); 
            byte enchantment = byte.Parse(parameters[11].ToString());

            var nho = new HarvestableObject(id, type, tier, pos, charges, enchantment);
            OnEventNewHarvestableObject?.Invoke(nho);
        }

        private void onNewSimpleHarvestableObject(Dictionary<byte, object> parameters)
        {
            // NewHarvestableObject: { "0":211813,"1":211239,"2":637903348349073726,"3":"+Z47IXg2YUWs1j2NjqNMZQ==","5":24,"6":112,"7":3,"8":[-374.0117,209.0445],"9":319.8794,"11":0,"252":36}

            int id = int.Parse(parameters[0].ToString());
            byte type = byte.Parse(parameters[5].ToString());
            byte tier = byte.Parse(parameters[7].ToString());
            Single[] pos = (Single[])parameters[8];
            parameters.TryGetValue((byte)10, out object _charges);
            byte charges = _charges == null ? (byte)0 : byte.Parse(_charges.ToString());
            byte enchantment = byte.Parse(parameters[11].ToString());

            var nho = new HarvestableObject(id, type, tier, pos, charges, enchantment);
            OnEventNewSimpleHarvestableObject?.Invoke(nho);
        }

        private void onHealthUpdate(Dictionary<byte, object> parameters)
        {
            parameters.TryGetValue((byte)0, out object _attEntityID);
            int attackerEntityId = _attEntityID == null ? 0 : int.Parse(_attEntityID.ToString());
            parameters.TryGetValue((byte)2, out object _tHU);
            int targetHealthUpdate = _tHU == null ? 0 : int.Parse(_tHU.ToString());
            parameters.TryGetValue((byte)3, out object _tH);
            int targetHealth = _tH == null ? 0 : int.Parse(_tH.ToString());
            int targetEntityId = int.Parse(parameters[6].ToString());

            var hu = new evHealthUpdate(attackerEntityId, targetHealthUpdate, targetHealth, targetEntityId);
            OnEventHealthUpdate?.Invoke(hu);
        }
        
        private void onLeaveEvent(Dictionary<byte, object> parameters)
        {
            int entityId = int.Parse(parameters[0].ToString());
            
            var leave = new evLeave(entityId);
            OnEventevLeave?.Invoke(leave);
        }
        
        private void onJoinFinished(Dictionary<byte, object> parameters)
        {
            evJoinFinished jf = new evJoinFinished();
            OnEventJoinFinished?.Invoke(jf);
        }
        
        private void onUpdateSilver(Dictionary<byte, object> parameters)
        {
            int id = int.Parse(parameters[0].ToString());
            string silver = parameters[1].ToString();
            int len = silver.Length - 4;
            long currentSilver = long.Parse(silver.Substring(0, len));

            var us = new evUpdateSilver(id, currentSilver);
            OnEventUpdateMoney?.Invoke(us);
        }
        
        private void onNewSimpleHarvestableObjectList(Dictionary<byte, object> parameters)
        {
            List<HarvestableObject> harvestableList = new List<HarvestableObject>();

            List<int> idList = new List<int>();

            if (parameters[0].GetType() == typeof(Byte[]))
            {
                Byte[] typeListByte = (Byte[])parameters[0]; //list of types

                foreach (Byte b in typeListByte)
                    idList.Add(b);
            }
            else if (parameters[0].GetType() == typeof(Int16[]))
            {
                Int16[] typeListByte = (Int16[])parameters[0]; //list of types

                foreach (Int16 b in typeListByte)
                    idList.Add(b);
            }
            else
            {
                Console.WriteLine("onNewSimpleHarvestableObjectList type error: " + parameters[0].GetType());
                return;
            }

            Byte[] typesList = (Byte[])parameters[1]; //list of types
            Byte[] tiersList = (Byte[])parameters[2]; //list of tiers
            Single[] posList = (Single[])parameters[3]; //list of positions X1, Y1, X2, Y2 ...
            Byte[] chargeList = (Byte[])parameters[4]; //charge

            for (int i = 0; i < idList.Count; i++)
            {
                int id = int.Parse(idList.ElementAt(i).ToString());
                byte type = byte.Parse(typesList[i].ToString());
                byte tier = byte.Parse(tiersList[i].ToString());
                Single posX = (Single)posList[i * 2];
                Single posY = (Single)posList[i * 2 + 1];
                Single[] pos = new Single[2] { posX, posY };
                Byte charges = byte.Parse(chargeList[i].ToString());
                byte enchantent = (byte)0;

                var hObject = new HarvestableObject(id, type, tier, pos, charges, enchantent);
                harvestableList.Add(hObject);
            }

            var jol = new HarvestableObjectList(harvestableList);
            OnEventNewSimpleHarvestableObjectList?.Invoke(jol);
        }
        
        private void onHarvestableChangeState(Dictionary<byte, object> parameters)
        {
            int id = int.Parse(parameters[0].ToString());

            int charges = 0;

            if (parameters.ContainsKey(1))
            {
                charges = int.Parse(parameters[1].ToString());
            }

            int enchantment = 0;

            if (parameters.ContainsKey(2))
            {
                enchantment = int.Parse(parameters[2].ToString());
            }

            var change = new HarvestableChangeState(id, charges, enchantment);
            OnEventHarvestableChangeState?.Invoke(change);
        }
        
        private void onInCombatStateUpdate(Dictionary<byte, object> parameters)
        {
            long playerId = long.Parse(parameters[0].ToString());

            parameters.TryGetValue((byte)1, out object _playerStatus);
            bool playerAttacking = _playerStatus == null ? false : bool.Parse(_playerStatus.ToString());

            parameters.TryGetValue((byte)2, out object _enemyStatus);
            bool enemyAttacking = _enemyStatus == null ? false: bool.Parse(_enemyStatus.ToString());

            var comUp = new InCombatStateUpdate(playerId, playerAttacking, enemyAttacking);
            OnEventInCombatStateUpdate?.Invoke(comUp);            
        }
        
        private void onNewMountObject(Dictionary<byte, object> parameters)
        {
            int id = int.Parse(parameters[0].ToString());
            float[] pos = (float[])parameters[3];
            byte[] ownerMarkId = (byte[])parameters[5];

            var newMount = new evNewMountObject(id, pos, ownerMarkId);
            OnEventNewMountObject?.Invoke(newMount);            
        }
        
        private void onNewMob(Dictionary<byte, object> parameters)
        {
            //if (!parameters.ContainsKey(13))
            //    return;

            int id = int.Parse(parameters[0].ToString());
            int typeId = int.Parse(parameters[1].ToString());
            Single[] pos = (Single[])parameters[7];
            int health;

            if (parameters.ContainsKey(13))
            {
                health = int.Parse(parameters[13].ToString());
            }
            else
            {
                health = int.Parse(parameters[14].ToString());
            }

            int rarity = int.Parse(parameters[22].ToString());

            var mob = new evNewMob(id, typeId, pos, health, rarity);
            OnEventNewMob?.Invoke(mob);
        }
        
        private void onNewCharacter(Dictionary<byte, object> parameters)
        {
            int id = int.Parse(parameters[0].ToString());
            string nick = parameters[1].ToString();
            object oGuild = "";
            object oAlliance = "";

            parameters.TryGetValue((byte)8, out oGuild);
            parameters.TryGetValue((byte)44, out oAlliance);

            string guild = oGuild == null ? "" : oGuild.ToString();
            string alliance = oAlliance == null ? "" : oAlliance.ToString();
            Single[] pos = (Single[])parameters[14];
            short[] items = new short[10];
            short[] skills;
            
            try { skills = new short[6]; } catch { return; }

            int faction = int.Parse(parameters[51].ToString());
            int currentHealth = int.Parse(parameters[20].ToString());
            int maxHealth = int.Parse(parameters[21].ToString());

            int index = 0;

            if (parameters[38].GetType() == typeof(Byte[]))
            {
                Byte[] itemList = (Byte[])parameters[38];

                foreach (Byte b in itemList)
                {
                    if (index >= 10)
                        break;

                    items[index] = Convert.ToInt16(b);
                    index++;
                }
            }
            else
            {
                Int16[] itemList = (Int16[])parameters[38];

                foreach (Int16 b in itemList)
                {
                    if (index >= 10)
                        break;

                    items[index] = b;
                    index++;
                }
            }

            index = 0;

            if (parameters[41].GetType() == typeof(Byte[]))
            {
                Byte[] skillList = (Byte[])parameters[41];

                foreach (Byte b in skillList)
                {
                    if (index >= 6)
                        break;

                    skills[index] = Convert.ToInt16(b);
                    index++;
                }
            }
            else
            {
                Int16[] skillList = (Int16[])parameters[41];

                foreach (Int16 b in skillList)
                {
                    if (index >= 6)
                        break;

                    skills[index] = b;
                    index++;
                }
            }

            var newChar = new evNewCharacter(id, nick, guild, alliance, pos, items, skills, faction, currentHealth, maxHealth);
            OnEventNewCharacter?.Invoke(newChar);
        }
        
        private void onEventMove(Dictionary<byte, object> parameters)
        {
            int id = int.Parse(parameters[0].ToString());
            Byte[] a = (Byte[])parameters[1];
            Single posX = BitConverter.ToSingle(a, 9);
            Single posY = BitConverter.ToSingle(a, 13);

            var ent = new Entity(id, posX, posY);
            OnEventMove?.Invoke(ent);
        }

        private void onEventEasyAntiCheatMessageToClient(Dictionary<byte, object> parameters)
        {
            int packetID = int.Parse(parameters[0].ToString());

            var nb = new evEasyAntiCheatMessageToClient();
            OnEventEasyAntiCheatMessageToClient?.Invoke(nb);
        }

        private void onEventCastHits(Dictionary<byte, object> parameters)
        {
            // {"0":88105,"1":95538,"2":4414,"3":1,"4":1,"252":20}
            // 0: target id 1: attacker id

            int packetID = int.Parse(parameters[0].ToString());

            var nb = new evCastHits();
            OnEventCastHits?.Invoke(nb);            
        }

        private void onEventCastStart(Dictionary<byte, object> parameters)
        {
            // CastStart: {"0":89514,"1":18668197,"2":[201.960464,220.702423],"4":18669497,"5":4400,"6":-1,"8":7,"9":0,"252":13}
            // 0: caster id 2:Spell direction pos

            int packetID = int.Parse(parameters[0].ToString());

            var nb = new evCastStart();
            OnEventCastStart?.Invoke(nb);            
        }

        private void onEventCastTimeUpdate(Dictionary<byte, object> parameters)
        {
            int packetID = int.Parse(parameters[0].ToString());

            var nb = new evCastTimeUpdate();
            OnEventCastTimeUpdate?.Invoke(nb);            
        }

        #endregion

        #region onOperation Request

        private void onReqHarvestStart(Dictionary<byte, object> parameters)
        {
            int harvest_id = int.Parse(parameters[1].ToString());

            var hs = new opHarvestStart(harvest_id);
            OnRequestHarvestStart?.Invoke(hs);
        }
        
        private void onRequestMount(Dictionary<byte, object> parameters)
        {
            parameters.TryGetValue((byte)2, out object _isMounting);
            bool isMounting = _isMounting == null ? false : bool.Parse(_isMounting.ToString());
            parameters.TryGetValue((byte)3, out object _quickMount);
            bool quickMount = _quickMount == null ? false: bool.Parse(_quickMount.ToString());

            var opm = new opMount(isMounting, quickMount);
            OnRequestMount?.Invoke(opm);            
        }
        
        private void onRequestMove(Dictionary<byte, object> parameters)
        {
            if (parameters == null)
                return;

            long timestamp = long.Parse(parameters[0].ToString());

            /***
            long today_tick = DateTime.Now.Ticks;
            DateTime fechaHora_server = new DateTime(timestamp, DateTimeKind.Utc);
            // Diferencia entre servidor y local
            //.AddMonths(-1).AddDays(30).AddHours(21).AddMinutes(-2).AddSeconds(50).AddMilliseconds(-250);
            Console.WriteLine("Hora Server: " + fechaHora_server);
            DateTime fechaHora_local = new DateTime(today_tick, DateTimeKind.Utc);
            Console.WriteLine("Hora Local: " + fechaHora_local);
            long diff = timestamp - today_tick;
            long espected_timestamp = today_tick + diff;
            ***/

            float[] pos = (float[])parameters[1];
            float angle = (float)parameters[2];
            float[] target = (float[])parameters[3];
            float speed = (float)parameters[4];

            opMove mov = new opMove(timestamp, pos, angle, target, speed);
            OnRequestMove?.Invoke(mov);
        }
        
        private void onRequestAuctionGetOffers(Dictionary<byte, object> parameters)
        {
            int id = int.Parse(parameters[0].ToString());
            Int16[] itemsID = new short[10];

            if (parameters.ContainsKey(6))
            {
                itemsID = (Int16[])parameters[6];
            }

            string category = parameters[1].ToString();
            string subcategory = parameters[2].ToString();

            short quality;
            try { quality = short.Parse(parameters[3].ToString()); }
            catch (FormatException) { quality = 0; }

            short tier;
            try { tier = short.Parse(parameters[5].ToString()); }
            catch (FormatException) { tier = 0; }

            short enchantment;
            try { enchantment = short.Parse(parameters[8].ToString()); }
            catch (FormatException) { enchantment = -1; }

            var agor = new opAuctionGetAny(id, itemsID, category, subcategory, quality, tier, enchantment);
            OnRequestAuctionGetOffers?.Invoke(agor);            
        }
        
        private void onRequestAuctionGetRequests(Dictionary<byte, object> parameters)
        {
            // {"0":867,"1":"accessories","2":"bag","3":"5","4":0,"5":"8","7":"3","8":0,"9":1,"10":50,"12":true,"253":77}
            int id = int.Parse(parameters[0].ToString());
            Int16[] itemsID = new short[10];

            if (parameters.ContainsKey(6))
            {
                itemsID = (Int16[])parameters[6];
            }

            string category = parameters[1].ToString();
            string subcategory = parameters[2].ToString();

            short quality;
            try { quality = short.Parse(parameters[3].ToString()); }
            catch (FormatException) { quality = 0; }

            short tier;
            try { tier = short.Parse(parameters[5].ToString()); }
            catch (FormatException) { tier = 0; }

            short enchantment;
            try { enchantment = short.Parse(parameters[7].ToString()); }
            catch (FormatException) { enchantment = -1; }

            var agrr = new opAuctionGetAny(id, itemsID, category, subcategory, quality, tier, enchantment);
            OnRequestAuctionGetRequests?.Invoke(agrr);
        }
        
        private void onRequestRegisterToObject(Dictionary<byte, object> parameters)
        {
            long id = long.Parse(parameters[0].ToString());

            var reg = new opRegisterToObject(id);
            OnRequestRegisterToObject?.Invoke(reg);
        }
        
        private void onRequestUnRegisterFromObject(Dictionary<byte, object> parameters)
        {
            long id = long.Parse(parameters[0].ToString());
            
            var unreg = new UnRegisterFromObject(id);
            OnRequestUnRegisterFromObject?.Invoke(unreg);
        }
        
        #endregion

        #region onOperation Response

        private void onResponseJoin(Dictionary<byte, object> parameters)
        {
            // [onResponse][2] Join: {"0":229611,"1":"HldB6eRwfU6UYy3c3Cuenw==","2":"Anyalgo","3":9,"4":22,"5":1,"6":"AwAJAw==","7":"AA8CAA==",
            // "8":"4203","9":[-79.5,-363.5],"11":3083.0,"12":3083.0,"14":86.31288,"15":638371386969141790,"16":387.0,"17":387.0,"19":4.83620453,"20":638371386969141790,"21":885.0,"22":885.0,"24":8.85,"25":638371386969141790,"26":30000.0,"27":30000.0,"29":0.115740739,"30":638371386961696661,"32":2100836716,"34":20028876730,"35":{},"36":370000,"37":638371385370000000,"38":9999990000,"39":28800000,"40":1899.15479,"41":638371386969141790,"42":"AAAAAAA=","43":2,"44":"","45":0,"46":"","47":0,"48":true,"49":"Pq+miG0ZgEevySATGhQ+bQ==","50":[229616,0,229617,229613,229618,229620,229615,229621,229614,229619],"52":"kh9EpT9bZU6nL1vdJFcBLw==","53":[229622,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,229624,229623],"55":0,"60":"4000","64":"4000","65":"","66":638371386969141790,"67":-2019027515,"76":"","78":"","80":47740000,"81":0,"83":638389385370000000,"84":33,"85":638349760192334510,"87":"","88":[],"89":"","91":true,"92":true,"94":638371386918888800,"95":638371383266745275,"96":12,"97":{},"98":"AAAAAAAA","99":0,"100":100258296,"101":12000000,"102":[-1,-1,-1,-1,-1,-1,-1],"103":0,"111":1,"112":"4000","253":2}
            int cId = int.Parse(parameters[0].ToString());
            byte[] markId = (byte[])parameters[1];
            string cName = parameters[2].ToString();
            string cluster = parameters[8].ToString();
            float[] pos = (float[])parameters[9];

            float angle = 0;

            if (parameters.ContainsKey(10))
                angle = float.Parse(parameters[10].ToString());
            else
            {
                if (parameters.ContainsKey(14))
                    angle = float.Parse(parameters[14].ToString());
            }

            int currentHealth = int.Parse(parameters[11].ToString());
            int maxHealth = int.Parse(parameters[12].ToString());
            int currentEnergy = int.Parse(parameters[16].ToString());
            int maxEnergy = int.Parse(parameters[17].ToString());

            int faction = int.Parse(parameters[48].ToString());

            var jo = new OpJoin(cId, markId, cName, cluster, pos, angle, currentHealth, maxHealth, currentEnergy, maxEnergy, faction);
            OnResponseJoin?.Invoke(jo);
        }

        private void onResponseAuctionGetOffers(Dictionary<byte, object> parameters)
        {
            string line = JsonConvert.SerializeObject(parameters[0]);
            JArray offers = JArray.Parse(line);
            List<AuctionItems> offerList = new List<AuctionItems>();

            foreach (JValue value in offers)
            {
                JObject offer = JObject.Parse(value.ToString());
                long id = ((long)offer.GetValue("Id"));
                long unitPrice = ((long)offer.GetValue("UnitPriceSilver")) / 10000;
                long totalPrice = ((long)offer.GetValue("TotalPriceSilver")) / 10000;
                int amount = ((int)offer.GetValue("Amount"));
                short tier = ((short)offer.GetValue("Tier"));
                bool isFinished = ((bool)offer.GetValue("IsFinished"));
                string auctionType = offer.GetValue("IsFinished").ToString();
                bool hasBuyerFetched = ((bool)offer.GetValue("HasBuyerFetched"));
                bool hasSellerFetched = ((bool)offer.GetValue("HasSellerFetched"));
                string sellerCharacterId = offer.GetValue("SellerCharacterId").ToString();
                string sellerName = offer.GetValue("SellerName").ToString();
                string buyerCharacterId = offer.GetValue("BuyerCharacterId").ToString();
                string buyerName = offer.GetValue("BuyerName").ToString();
                string itemTypeId = offer.GetValue("ItemTypeId").ToString();
                string ItemGroupTypeId = offer.GetValue("ItemGroupTypeId").ToString();
                short enchantmentLevel = ((short)offer.GetValue("EnchantmentLevel"));
                short qualityLevel = ((short)offer.GetValue("QualityLevel"));
                string expires = offer.GetValue("Expires").ToString();
                string referenceId = offer.GetValue("ReferenceId").ToString();

                var auctionOffer = new AuctionItems(id, unitPrice, totalPrice, amount, tier, isFinished, auctionType, hasBuyerFetched, hasSellerFetched, sellerCharacterId, sellerName, buyerCharacterId, buyerName, itemTypeId, ItemGroupTypeId, enchantmentLevel, qualityLevel, expires, referenceId);
                offerList.Add(auctionOffer);
            }

            var response = new opAuctionGetOffersResponse(offerList);
            OnResponseAuctionGetOffers?.Invoke(response);
        }

        private void onResponseAuctionGetRequests(Dictionary<byte, object> parameters)
        {
            string line = JsonConvert.SerializeObject(parameters[0]);
            JArray requests = JArray.Parse(line);
            List<AuctionItems> requestList = new List<AuctionItems>();

            foreach (JValue value in requests)
            {
                JObject request = JObject.Parse(value.ToString());
                long id = ((long)request.GetValue("Id"));
                long unitPrice = ((long)request.GetValue("UnitPriceSilver")) / 10000;
                long totalPrice = ((long)request.GetValue("TotalPriceSilver")) / 10000;
                int amount = ((int)request.GetValue("Amount"));
                short tier = ((short)request.GetValue("Tier"));
                bool isFinished = ((bool)request.GetValue("IsFinished"));
                string auctionType = request.GetValue("IsFinished").ToString();
                bool hasBuyerFetched = ((bool)request.GetValue("HasBuyerFetched"));
                bool hasSellerFetched = ((bool)request.GetValue("HasSellerFetched"));
                string sellerCharacterId = request.GetValue("SellerCharacterId").ToString();
                string sellerName = request.GetValue("SellerName").ToString();
                string buyerCharacterId = request.GetValue("BuyerCharacterId").ToString();
                string buyerName = request.GetValue("BuyerName").ToString();
                string itemTypeId = request.GetValue("ItemTypeId").ToString();
                string ItemGroupTypeId = request.GetValue("ItemGroupTypeId").ToString();
                short enchantmentLevel = ((short)request.GetValue("EnchantmentLevel"));
                short qualityLevel = ((short)request.GetValue("QualityLevel"));
                string expires = request.GetValue("Expires").ToString();
                string referenceId = request.GetValue("ReferenceId").ToString();

                var auctionRequest = new AuctionItems(id, unitPrice, totalPrice, amount, tier, isFinished, auctionType, hasBuyerFetched, hasSellerFetched, sellerCharacterId, sellerName, buyerCharacterId, buyerName, itemTypeId, ItemGroupTypeId, enchantmentLevel, qualityLevel, expires, referenceId);
                requestList.Add(auctionRequest);
            }

            var response = new opAuctionGetRequestsResponse(requestList);
            OnResponseAuctionGetRequests?.Invoke(response);
        }

        private void onResponseAuctionGetItemAverageValue(Dictionary<byte, object> parameters)
        {
            string line = parameters[0].ToString();
            int len = line.Length;
            long average = long.Parse(line.Substring(0, len - 4));

            var averageObj = new opAuctionGetItemAverageValue(average);
            OnResponseAuctionGetItemAverageValue?.Invoke(averageObj);
        }

        #endregion
    }
}
