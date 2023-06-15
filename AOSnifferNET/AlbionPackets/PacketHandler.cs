using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhotonPackageParser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AOSnifferNET
{
    class PacketHandler : PhotonParser
    {
        public PacketHandler(){}

        protected override void OnEvent(byte code, Dictionary<byte, object> parameters)
        {
            if (code == 3 || code == 2)
            {
                // Bien
                // Movimiento de cualquier entidad en el juego
                onEntityMovementEvent(parameters);
            }

            parameters.TryGetValue((byte)252, out object val);
            if (val == null) return;

            if (!int.TryParse(val.ToString(), out int iCode)) return;
            EventCodes evCode = 0;
            try
            {
                evCode = (EventCodes)iCode;
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                debugPacket(parameters, iCode);
            }
            
            switch (evCode)
            {
                case EventCodes.InventoryPutItem:
                    // InventoryPutItem: {"0":278105,"1":7,"2":"DVOdrtd7f0WXPVIb3ayDew==","3":7,"252":25} 0: ItemID
                    // Item puesto en el inventario
                    onInventoryPutItem(parameters);
                    break;
                case EventCodes.NewEquipmentItem:
                    // Bien
                    onNewGeneralItem(parameters, evCode);
                    break;
                case EventCodes.NewSimpleItem:
                    // Bien
                    onNewGeneralItem(parameters, evCode);
                    break;
                case EventCodes.NewFurnitureItem:
                    // Bien
                    onNewGeneralItem(parameters, evCode);
                    break;
                case EventCodes.NewJournalItem:
                    // Bien
                    onNewGeneralItem(parameters, evCode);
                    break;
                case EventCodes.NewLaborerItem:
                    // Bien
                    onNewGeneralItem(parameters, evCode);
                    break;
                case EventCodes.AttachItemContainer:
                    // Bien
                    // Contenedor con items, ya sea loot o un chest
                    onAttachItemContainer(parameters);
                    break;
                case EventCodes.NewLoot:
                    // Bien
                    // Donde ha aparecido una bolsa de loot en el mapa
                    onNewLoot(parameters);
                    break;
                case EventCodes.NewCharacter:
                    // Bien
                    // Un nuevo personaje ha aparecido cerca y sus datos correspondientes
                    onNewCharacter(parameters);
                    break;
                case EventCodes.Leave:
                    // Bien
                    // Quien ha salido del mapa o del campo de vision
                    onLeaveEvent(parameters);
                    break;
                case EventCodes.Mounted:
                    // Bien
                    // Cuando cualquiera en el mapa se monta o desmonta
                    onMounted(parameters);
                    break;
                case EventCodes.NewMountObject:
                    // Cambio a 295
                    // Cuando una montura aparece en el mapa y sus datos correspondientes
                    onNewMountObject(parameters);
                    break;
                case EventCodes.NewMob:
                    // Bien
                    // Cuando una Mob aparece en el mapa y sus datos correspondientes
                    onNewMob(parameters);
                    break;
                case EventCodes.JoinFinished:
                    // Cuando se ha terminado la transicion de un mapa a otro
                    //onJoinFinished();
                    break;
                case EventCodes.UpdateSilver:
                    // Bien
                    // Cuando se gana o pierde silver se actualiza la cantidad de silver que tenemos
                    onUpdateSilver(parameters);
                    break;
                case EventCodes.NewSimpleHarvestableObjectList:
                    // Bien
                    // Una lista de los nodos farmeables en el mapa y sus datos correspondientes
                    onNewSimpleHarvestableObjectList(parameters);
                    break;
                case EventCodes.NewHarvestableObject:
                    // Bien
                    // Cuando un solo nodo farmeable aparece en el mapa y sus datos correspondientes
                    onNewHarvestableObject(parameters);
                    break;
                case EventCodes.HarvestableChangeState:
                    // Bien
                    // Cuando un nodo farmeable cambia de estado o sea cambia la cantidad de cargas que tiene (encantamiento?)
                    onHarvestableChangeState(parameters);
                    break;
                case EventCodes.MobChangeState:
                    // MobChangeState: {"0":718898,"1":1,"252":43}
                    // Cuando una Mob cambia de estado de encantamiento
                    onMobChangeState(parameters);
                    break;
                case EventCodes.InCombatStateUpdate:
                    // Bien
                    // El estado de En Combate de alguna entidad
                    onInCombatStateUpdate(parameters);
                    break;
                case EventCodes.HealthUpdate:
                    // Bien
                    // Registra el cambio de salud de cualquier entidad y de quien se recive el cambio
                    onHealthUpdate(parameters);
                    break;
                case EventCodes.Attack:
                    // Attack: { "0":361885,"1":40045690,"2":359223,"3":1,"4":40045810,"5":40045810,"6":1,"252":12}
                    onAttack(parameters);
                    //printEventInfo(parameters, evCode);
                    break;
                case EventCodes.NewRandomDungeonExit:
                    // Cambio a 308
                    onNewPortalExit(parameters);
                    break;
                case EventCodes.ActiveSpellEffectsUpdate:
                    // sin usar
                    // [10]evActiveSpellEffectsUpdate - map[0:655873 1:[685 279 339 509 399] 2:[100 389.11096 369.137 100 389.11096] 3:[100 233.6818 226.1142 100 233.6818] 4:[637945635939432036 637945670488995997 637945670488995997 637903462984937618 637945670488995997] 5:[1 10 10 1 10] 7:[75 - 110 36 73 - 110 36 73 - 110 36 9] 8:[208406056] 9:[9] 10:[0 0] 252:10]
                    // 9 parece ser el tiempo restante
                    //printEventInfo(parameters, evCode);
                    break;
                case EventCodes.HarvestStart:
                    //HarvestStart: { "0":144653,"1":638045717068106818,"2":638045717068106818,"3":1263,"5":2.0,"6":-1,"7":-1,"252":54} 5: tiempo para terminar de harvestear
                    // Bien
                    onHarvestStart(parameters);
                    break;
                case EventCodes.HarvestFinished:
                    // Bien
                    //onHarvestFinished(parameters);
                    break;
                case EventCodes.NewFloatObject:
                    // NewFloatObject: { "0":665769,"1":[-190.193344,59.3336449],"2":298.5448,"3":632262,"4":1,"252":340} 0: bobber ID 1: bobber pos 2:angle 3:player ID 4:fishing state (1: bobber landed 2:fish bitten 3:playing minigam 4:catched a fish 5:failed)
                    //onNewFloatObject(parameters);
                    break;
                case EventCodes.NewFishingZoneObject:
                    //NewFishingZoneObject: { "0":1182,"1":[253.4,52.8],"2":3,"3":2,"4":"FishingNodeSwarm","252":341} 0: objectID 1: zone pos 2:charges (not present when empty) 3:times fished from 4:zone tipe
                    //onNewFishingZoneObject(parameters);
                    break;
                case EventCodes.FishingMiniGame:
                    //printEventInfo(parameters, evCode);
                    break;
                default:
                    //printEventInfo(parameters, evCode);
                    break;
            }
            
        }

        protected override void OnRequest(byte operationCode, Dictionary<byte, object> parameters)
        {
            parameters.TryGetValue((byte)253, out object val);
            if (val == null) return;

            if (!int.TryParse(val.ToString(), out int iCode)) return;

            OperationCodes opCode = 0;
            try
            {
                opCode = (OperationCodes)iCode;
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                debugPacket(parameters, iCode);
            }
            
            switch (opCode)
            {
                case OperationCodes.Move:
                    // Bien
                    onMoveOperation(parameters);
                    break;
                case OperationCodes.AuctionGetOffers:
                    // Revisar
                    //onAuctionGetOffers_Req(parameters);
                    break;
                case OperationCodes.AuctionGetRequests:
                    // Revisar
                    //onAuctionGetRequests_Req(parameters);
                    break;
                case OperationCodes.RegisterToObject:
                    // Revisar
                    onRegisterToObject(parameters);
                    break;
                case OperationCodes.UnRegisterFromObject:
                    // Revisar
                    onUnRegisterFromObject(parameters);
                    break;
                case OperationCodes.AttackStart:
                    // Bien
                    printOperationInfo(parameters, opCode, "onRequest");
                    break;
                case OperationCodes.Mount:
                    // Bien
                    onMount(parameters);
                    break;
                case OperationCodes.MountCancel:
                    // Bien
                    printOperationInfo(parameters, opCode, "onRequest");
                    break;
                case OperationCodes.HarvestStart:
                    // 1: id del harvestable
                    // Bien
                    onReqHarvestStart(parameters);
                    break;
                case OperationCodes.HarvestCancel:
                    // No llega al minar un cuerpo
                    // Bien
                    printOperationInfo(parameters, opCode, "onRequest");
                    break;
                default:
                    //printOperationInfo(parameters, opCode, "onRequest");
                    break;
            }            
        }

        protected override void OnResponse(byte operationCode, short returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            parameters.TryGetValue((byte)253, out object val);
            if (val == null) return;

            if (!int.TryParse(val.ToString(), out int iCode)) return;

            OperationCodes opCode = (OperationCodes)iCode;
                        
            switch (opCode)
            {
                case OperationCodes.Join:
                    // Bien
                    onJoinResponse(parameters);
                    break;
                case OperationCodes.AuctionGetOffers:
                    onAuctionGetOffers_Res(parameters);
                    break;
                case OperationCodes.AuctionGetRequests:
                    onAuctionGetRequests_Res(parameters);
                    break;
                case OperationCodes.AuctionGetItemAverageValue:
                    onAuctionGetItemAverageValue(parameters);
                    break;
                case OperationCodes.HarvestStart:
                    // usando solo el request
                    // Bien
                    //printOperationInfo(parameters, opCode, "onResponse");
                    break;
                case OperationCodes.HarvestCancel:
                    // Bien
                    printOperationInfo(parameters, opCode, "onResponse");
                    break;
                default :
                    //printOperationInfo(parameters, opCode, "onResponse");
                    break;
            }            
        }

        private void printEventInfo(Object obj, EventCodes evCode)
        {
            string jsonPacket;
            jsonPacket = JsonConvert.SerializeObject(obj);
            string outLine = "[onEvent][" + (int)evCode + "] " + evCode + ": " + jsonPacket;
            Console.WriteLine(outLine);
        }
        private void printOperationInfo(Object obj, OperationCodes opCode, String typeInfo)
        {
            string jsonPacket;
            jsonPacket = JsonConvert.SerializeObject(obj);
            string outLine = "[" + typeInfo + "][" + (int)opCode + "] " + opCode + ": " + jsonPacket;
            Console.WriteLine(outLine);
        }

        private void debugPacket(Object obj, int iCode)
        {
            string jsonPacket;
            jsonPacket = JsonConvert.SerializeObject(obj);
            string outLine = iCode + ": " + jsonPacket;
            Console.WriteLine(outLine);
        }

        #region OnEvent
        private void onMobChangeState(Dictionary<byte, object> parameters)
        {
            int mobID = int.Parse(parameters[0].ToString());
            short enchantment = short.Parse(parameters[1].ToString());

            var cs = new evMobChangeState(mobID, enchantment);
            printEventInfo(cs, EventCodes.MobChangeState);
        }
        private void onInventoryPutItem(Dictionary<byte, object> parameters)
        {
            int itemID = int.Parse(parameters[0].ToString());
            var putItem = new evInventoryPutItem(itemID);
            printEventInfo(putItem, EventCodes.InventoryPutItem);
        }
        private void onNewFishingZoneObject(Dictionary<byte, object> parameters)
        {
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
            printEventInfo(fishingZone, EventCodes.NewFishingZoneObject);

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
            printEventInfo(bobber, EventCodes.NewFloatObject);
        }
        private void onHarvestStart(Dictionary<byte, object> parameters)
        {
            int harvestableId = int.Parse(parameters[3].ToString());
            float time = float.Parse(parameters[5].ToString());

            var hs = new evHarvestStart(harvestableId, time);
            printEventInfo(hs, EventCodes.HarvestStart);
        }
        private void onNewGeneralItem(Dictionary<byte, object> parameters, EventCodes evCode)
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
            printEventInfo(newItem, evCode);

        }
        private void onAttachItemContainer(Dictionary<byte, object> parameters)
        {
            int objectID = int.Parse(parameters[0].ToString());
            byte[] ownerMarkID = (byte[])parameters[1];
            int[] itemsID = (int[])parameters[3];
            // Estos itemsID son los objectID de eventos como
            // NewEquipmentItem: {"0":203114,"1":2984,"2":1,"4":6204632,"5":"Bettooo","6":2,"7":48000000,"8":[-1],"9":[-1],"252":28}
            // donde 0 es el objectID

            var itemContainer = new evAttachItemContainer(objectID, ownerMarkID, itemsID);
            printEventInfo(itemContainer, EventCodes.AttachItemContainer);
        }
        private void onNewLoot(Dictionary<byte, object> parameters)
        {
            int lootId = int.Parse(parameters[0].ToString());
            Single[] pos = (Single[])parameters[4];

            var newloot = new evNewLoot(lootId, pos);
            printEventInfo(newloot, EventCodes.NewLoot);
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
            printEventInfo(hf, EventCodes.HarvestFinished);
            
        }
        private void onMounted(Dictionary<byte, object> parameters)
        {
            int playerId = int.Parse(parameters[0].ToString());
            bool isMounted = false;

            if (parameters.ContainsKey(2))
                isMounted = true;

            var mounted = new evMounted(playerId, isMounted);
            printEventInfo(mounted, EventCodes.Mounted);

        }
        public void onAttack(Dictionary<byte, object> parameters)
        {
            int attackerID = int.Parse(parameters[0].ToString());
            int targetID = int.Parse(parameters[2].ToString());

            var attackEv = new evAttack(attackerID, targetID);
            printEventInfo(attackEv, EventCodes.Attack);
        }
        private void onNewPortalExit(Dictionary<byte, object> parameters)
        {
            int id = int.Parse(parameters[0].ToString());
            Single[] pos = (Single[])parameters[1];
            String type = (string)parameters[3].ToString();

            var portal = new evNewPortalExit(id, pos, type);
            printEventInfo(portal, EventCodes.NewRandomDungeonExit);
        }

        private void onNewHarvestableObject(Dictionary<byte, object> parameters)
        {
            // NewHarvestableObject: { "0":211813,"1":211239,"2":637903348349073726,"3":"+Z47IXg2YUWs1j2NjqNMZQ==","5":24,"6":112,"7":3,"8":[-374.0117,209.0445],"9":319.8794,"11":0,"252":36}
            int id = int.Parse(parameters[0].ToString());
            byte type = byte.Parse(parameters[5].ToString());
            byte tier = byte.Parse(parameters[7].ToString());
            Single[] pos = (Single[])parameters[8];
            byte charges;
            try { charges = byte.Parse(parameters[10].ToString()); }
            catch (Exception) { charges = 0; }
            byte enchantment = byte.Parse(parameters[11].ToString());

            var nho = new HarvestableObject(id, type, tier, pos, charges, enchantment);
            printEventInfo(nho, EventCodes.NewHarvestableObject);
        }
        private void onHealthUpdate(Dictionary<byte, object> parameters)
        {   
            try
            {
                int attackerEntityId;
                try{ attackerEntityId = int.Parse(parameters[0].ToString()); }
                catch (Exception) { attackerEntityId = 0; }
                int targetHealthUpdate = int.Parse(parameters[2].ToString());
                int targetCurrentHealth;
                try { targetCurrentHealth = int.Parse(parameters[3].ToString()); }
                catch (Exception) { targetCurrentHealth = 0; }
                int targetEntityId = int.Parse(parameters[6].ToString());

                var hu = new evHealthUpdate(attackerEntityId, targetHealthUpdate, targetCurrentHealth, targetEntityId);
                printEventInfo(hu, EventCodes.HealthUpdate);
            }
            catch (Exception e) {
                Debug.WriteLine(e.StackTrace); 
            }
        }
        private void onLeaveEvent(Dictionary<byte, object> parameters)
        {
            int entityId = int.Parse(parameters[0].ToString());
            var leave = new evLeave(entityId);

            printEventInfo(leave, EventCodes.Leave);
        }

        private void onJoinFinished()
        {
            evJoinFinished jf = new evJoinFinished();
            printEventInfo(jf, EventCodes.JoinFinished);
        }

        private void onUpdateSilver(Dictionary<byte, object> parameters)
        {
            int id = int.Parse(parameters[0].ToString());
            string silver = parameters[1].ToString();
            int len = silver.Length - 4;
            long currentSilver = long.Parse(silver.Substring(0, len));

            var us = new evUpdateSilver(id, currentSilver);

            printEventInfo(us, EventCodes.UpdateSilver);
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

            try
            {
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

            }
            catch (Exception e)
            {
                Debug.WriteLine("eL: " + e.ToString());
            }

            var jol = new HarvestableObjectList(harvestableList);
            printEventInfo(jol, EventCodes.NewSimpleHarvestableObjectList);
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
            printEventInfo(change, EventCodes.HarvestableChangeState);
        }

        private void onInCombatStateUpdate(Dictionary<byte, object> parameters)
        {
            long playerId = long.Parse(parameters[0].ToString());
            
            bool playerAttacking;
            try { playerAttacking = bool.Parse(parameters[1].ToString()); }
            catch (System.Collections.Generic.KeyNotFoundException) { playerAttacking = false; }

            bool enemyAttacking;
            try { enemyAttacking = bool.Parse(parameters[2].ToString()); }
            catch { enemyAttacking = false; }

            var comUp = new InCombatStateUpdate(playerId, playerAttacking, enemyAttacking);
            printEventInfo(comUp, EventCodes.InCombatStateUpdate);
        }

        private void onNewMountObject(Dictionary<byte, object> parameters)
        {
            int id = int.Parse(parameters[0].ToString());
            float[] pos = (float[])parameters[3];
            byte[] ownerMarkId = (byte[])parameters[5];

            var newMount = new evNewMountObject(id, pos, ownerMarkId);
            printEventInfo(newMount, EventCodes.NewMountObject);
        }

        private void onNewMob(Dictionary<byte, object> parameters)
        {
            if (!parameters.ContainsKey(13))
                return;

            int id = int.Parse(parameters[0].ToString());
            int typeId = int.Parse(parameters[1].ToString());
            Single[] pos = (Single[])parameters[8];
            int health = int.Parse(parameters[13].ToString());
            int rarity = int.Parse(parameters[20].ToString());

            var mob = new evNewMob(id, typeId, pos, health, rarity);
            printEventInfo(mob, EventCodes.NewMob);
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
            Single[] pos = (Single[])parameters[13];
            short[] items = new short[10];
            short[] skills = new short[6];
            int faction = int.Parse(parameters[46].ToString());
            int currentHealth = int.Parse(parameters[19].ToString());
            int maxHealth = int.Parse(parameters[20].ToString());

            int index = 0;
            if (parameters[34].GetType() == typeof(Byte[]))
            {
                Byte[] itemList = (Byte[])parameters[34];
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
                Int16[] itemList = (Int16[])parameters[34];
                foreach (Int16 b in itemList)
                {
                    if (index >= 10)
                        break;

                    items[index] = b;
                    index++;
                }
            }

            index = 0;
            if (parameters[36].GetType() == typeof(Byte[]))
            {
                Byte[] skillList = (Byte[])parameters[36];
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
                Int16[] skillList = (Int16[])parameters[36];
                foreach (Int16 b in skillList)
                {
                    if (index >= 6)
                        break;

                    skills[index] = b;
                    index++;
                }
            }

            var newChar = new evNewCharacter(id, nick, guild, alliance, pos, items, skills, faction, currentHealth, maxHealth);
            printEventInfo(newChar, EventCodes.NewCharacter);
        }

        private void onEntityMovementEvent(Dictionary<byte, object> parameters)
        {
            int id = int.Parse(parameters[0].ToString());
            Byte[] a = (Byte[])parameters[1];
            Single posX = BitConverter.ToSingle(a, 9);
            Single posY = BitConverter.ToSingle(a, 13);

            var ent = new Entity(id, posX, posY);

            string strJson;
            strJson = JsonConvert.SerializeObject(ent);

            Console.WriteLine("[onEntityMovementEvent]: {0}", strJson);
        }
        #endregion

        #region onOperation Request
        private void onReqHarvestStart(Dictionary<byte, object> parameters)
        {
            int harvest_id = int.Parse(parameters[1].ToString());
            var hs = new opHarvestStart(harvest_id);
            printOperationInfo(hs, OperationCodes.HarvestStart, "onRequest");

        }
        private void onMount(Dictionary<byte, object> parameters)
        {
            bool isMounting;
            try { isMounting = bool.Parse(parameters[2].ToString()); }
            catch { isMounting = false; }
            bool quickMount;
            try { quickMount = bool.Parse(parameters[3].ToString()); }
            catch { quickMount = false; }

            var opm = new opMount(isMounting, quickMount);
            printOperationInfo(opm, OperationCodes.Mount, "onRequest");
        }
        private void onMoveOperation(Dictionary<byte, object> parameters)
        {
            long id = long.Parse(parameters[0].ToString());
            float[] pos = (float[])parameters[1];
            float angle = (float)parameters[2];
            float[] target = (float[])parameters[3];
            float speed = (float)parameters[4];

            opMove _ = new opMove(id, pos, angle, target, speed);
            printOperationInfo(_, OperationCodes.Move, "onRequest");
        }

        private void onAuctionGetOffers_Req(Dictionary<byte, object> parameters)
        {
            int id = int.Parse(parameters[0].ToString());
            string queryText = "Unknown Text";
            if (!parameters.ContainsKey(6))
                queryText = "";

            string category = parameters[1].ToString();
            string subcategory = parameters[2].ToString();
            short quality;
            try { quality = short.Parse(parameters[3].ToString()); }
            catch (System.FormatException) { quality = 0; }

            short tier;
            try { tier = short.Parse(parameters[5].ToString()); }
            catch (System.FormatException) { tier = 0; }

            short enchantment;
            try { enchantment = short.Parse(parameters[8].ToString()); }
            catch (System.FormatException) { enchantment = -1; }

            var agor = new opAuctionGetAny(id, queryText, category, subcategory, quality, tier, enchantment);
            
            printOperationInfo(agor, OperationCodes.AuctionGetOffers, "onRequest");
        }

        private void onAuctionGetRequests_Req(Dictionary<byte, object> parameters)
        {
            // {"0":867,"1":"accessories","2":"bag","3":"5","4":0,"5":"8","7":"3","8":0,"9":1,"10":50,"12":true,"253":77}
            int id = int.Parse(parameters[0].ToString());
            string queryText = "Unknown Text";
            if (!parameters.ContainsKey(6))
                queryText = "";
            string category = parameters[1].ToString();
            string subcategory = parameters[2].ToString();
            
            short quality;
            try { quality = short.Parse(parameters[3].ToString()); } 
            catch (System.FormatException) { quality = 0; }

            short tier;
            try { tier = short.Parse(parameters[5].ToString()); }
            catch (System.FormatException) { tier = 0; }

            short enchantment;
            try { enchantment = short.Parse(parameters[7].ToString()); }
            catch (System.FormatException) { enchantment = -1; }

            var agrr = new opAuctionGetAny(id, queryText, category, subcategory, quality, tier, enchantment);

            printOperationInfo(agrr, OperationCodes.AuctionGetRequests, "onRequest");
        }

        private void onRegisterToObject(Dictionary<byte, object> parameters)
        {
            long id = long.Parse(parameters[0].ToString());
            var reg = new opRegisterToObject(id);
            printOperationInfo(reg, OperationCodes.RegisterToObject, "onRequest");
        }

        private void onUnRegisterFromObject(Dictionary<byte, object> parameters)
        {
            long id = long.Parse(parameters[0].ToString());
            var unreg = new UnRegisterFromObject(id);
            printOperationInfo(unreg, OperationCodes.UnRegisterFromObject, "onRequest");
        }
        #endregion

        #region onOperation Response
        private void onJoinResponse(Dictionary<byte, object> parameters)
        {
            int cId = int.Parse(parameters[0].ToString());
            byte[] markId = (byte[])parameters[1];
            string cName = parameters[2].ToString();
            string cluster = parameters[8].ToString();
            float[] pos = (float[])parameters[9];

            int currentHealth = int.Parse(parameters[11].ToString());
            int maxHealth = int.Parse(parameters[12].ToString());
            int currentEnergy = int.Parse(parameters[15].ToString());
            int maxEnergy = int.Parse(parameters[16].ToString());

            var jo = new OpJoin(cId, markId, cName, cluster, pos, currentHealth, maxHealth, currentEnergy, maxEnergy);
            printOperationInfo(jo, OperationCodes.Join, "onResponse");
        }

        private void onAuctionGetOffers_Res(Dictionary<byte, object> parameters)
        {
            string line = JsonConvert.SerializeObject(parameters[0]);
            JArray offers = JArray.Parse(line);
            List<AuctionItems> offerList = new List<AuctionItems>();
            foreach (JValue value in offers)
            {
                JObject offer = JObject.Parse(value.ToString());
                long id = ((long)offer.GetValue("Id"));
                long unitPrice = ((long)offer.GetValue("UnitPriceSilver"));
                long totalPrice = ((long)offer.GetValue("TotalPriceSilver"));
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
            printOperationInfo(response, OperationCodes.AuctionGetOffers, "onResponse");
        }

        private void onAuctionGetRequests_Res(Dictionary<byte, object> parameters)
        {
            string line = JsonConvert.SerializeObject(parameters[0]);
            JArray requests = JArray.Parse(line);
            List<AuctionItems> requestList = new List<AuctionItems>();
            foreach (JValue value in requests)
            {
                JObject request = JObject.Parse(value.ToString());
                long id = ((long)request.GetValue("Id"));
                long unitPrice = ((long)request.GetValue("UnitPriceSilver"));
                long totalPrice = ((long)request.GetValue("TotalPriceSilver"));
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
            printOperationInfo(response, OperationCodes.AuctionGetRequests, "onResponse");
        }

        private void onAuctionGetItemAverageValue(Dictionary<byte, object> parameters)
        {
            string line = parameters[0].ToString();
            int len = line.Length;
            long average = long.Parse(line.Substring(0, len - 4));
            var averageObj = new opAuctionGetItemAverageValue(average);
            printOperationInfo(averageObj, OperationCodes.AuctionGetItemAverageValue, "onResponse");

        }
        #endregion
    }
}
