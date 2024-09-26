using System;

namespace AOSnifferNET
{
    public delegate void RequestMove(opMove data);
    public delegate void RequestMount(opMount data);
    public delegate void RequestHarvestStart(opHarvestStart data);
    public delegate void RequestAuctionGetOffers(opAuctionGetAny data);
    public delegate void RequestAuctionGetRequests(opAuctionGetAny data);
    public delegate void RequestRegisterToObject(opRegisterToObject data);
    public delegate void RequestUnRegisterFromObject(UnRegisterFromObject data);

    public delegate void ResponseJoin(OpJoin data);
    public delegate void ResponseAuctionGetOffers(opAuctionGetOffersResponse data);
    public delegate void ResponseAuctionGetRequests(opAuctionGetRequestsResponse data);
    public delegate void ResponseAuctionGetItemAverageValue(opAuctionGetItemAverageValue data);

    public delegate void EventMove(Entity data);
    public delegate void EventCharacterEquipmentChanged(evCharacterEquipmentChanged data);
    public delegate void EventNewExit(evNewExit data);
    public delegate void EventInventoryPutItem(evInventoryPutItem data);
    public delegate void EventNewItem(evNewSimpleItem data);
    public delegate void EventAttachItemContainer(evAttachItemContainer data);
    public delegate void EventNewLoot(evNewLoot data);
    public delegate void EventNewCharacter(evNewCharacter data);
    public delegate void EventLeave(evLeave data);
    public delegate void EventMounted(evMounted data);
    public delegate void EventNewMountObject(evNewMountObject data);
    public delegate void EventNewMob(evNewMob data);
    public delegate void EventJoinFinished(evJoinFinished data);
    public delegate void EventUpdateMoney(evUpdateSilver data);
    public delegate void EventHarvestableObject(HarvestableObject data);
    public delegate void EventHarvestableObjectList(HarvestableObjectList data);
    public delegate void EventHarvestableChangeState(HarvestableChangeState data);
    public delegate void EventMobChangeState(evMobChangeState data);
    public delegate void EventInCombatStateUpdate(InCombatStateUpdate data);
    public delegate void EventHealthUpdate(evHealthUpdate data);
    public delegate void EventAttack(evAttack data);
    public delegate void EventNewPortalExit(evNewPortalExit data);
    public delegate void EventActiveSpellEffectsUpdate(evActiveSpellEffectsUpdate data);
    public delegate void EventHarvestStart(evHarvestStart data);
    public delegate void EventHarvestFinished(evHarvestFinished data);
    public delegate void EventNewFloatObject(evNewFloatObject data);
    public delegate void EventNewFishingZoneObject(evNewFishingZoneObject data);
    public delegate void EventFishingMiniGame(evFishingMiniGame data);
    public delegate void EventNewBuilding(evNewBuilding data);
    public delegate void EventEasyAntiCheatMessageToClient(evEasyAntiCheatMessageToClient data);
    public delegate void EventCastHits(evCastHits data);
    public delegate void EventCastStart(evCastStart data);
    public delegate void EventCastTimeUpdate(evCastTimeUpdate data);

    public delegate void RespondHarvestStart(string Class, string Method, Exception Ex);
}
