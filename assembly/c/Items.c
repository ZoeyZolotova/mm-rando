#include <z64.h>
#include "Icetrap.h"
#include "Items.h"
#include "QuestItems.h"
#include "Misc.h"
#include "MMR.h"

/**
 * Helper function used to process receiving a custom item.
 **/
static void HandleCustomItem(GlobalContext* ctxt, u8 item) {
    switch (item) {
        case CUSTOM_ITEM_ROYAL_WALLET:
            gSaveContext.perm.inv.upgrades.wallet = 3;
            break;
        case CUSTOM_ITEM_ICE_TRAP:
            Icetrap_PushPending(DAMAGE_EFFECT_FREEZE);
            break;
        case CUSTOM_ITEM_CRIMSON_RUPEE:
            z2_AddRupees(30);
            break;
        case CUSTOM_ITEM_SPIN_ATTACK:
            gSaveContext.perm.weekEventReg.hasGreatSpin = true;
            break;
        case CUSTOM_ITEM_MAGIC_POWER:;
            // TODO allow downgrades if they're not disabled
            // technically not vanilla behavior, as in vanilla getting double magic doesn't grant you magic if you don't have it
            bool isDoubleMagic = MMR_GetItemEntryContext->type >> 4;
            s8 magicAmount = 0x30;
            if (isDoubleMagic) {
                magicAmount = 0x60;
                gSaveContext.perm.unk24.hasDoubleMagic = true;
                gSaveContext.perm.unk24.magicLevel = 0;
            }
            gSaveContext.perm.unk24.hasMagic = true;
            if (gSaveContext.perm.unk24.currentMagic < magicAmount) {
                gSaveContext.perm.unk24.currentMagic = magicAmount;
            }
            break;
        case CUSTOM_ITEM_DOUBLE_DEFENSE:
            gSaveContext.perm.unk24.hasDoubleDefense = true;
            gSaveContext.perm.inv.defenseHearts = 20;
            break;
        case CUSTOM_ITEM_STRAY_FAIRY:;
            u8 type = MMR_GetItemEntryContext->type >> 4;
            if (type > 0) {
                gSaveContext.perm.inv.strayFairies[type-1]++;
            } else {
                gSaveContext.perm.weekEventReg.hasTownFairy = true;
            }
            break;
    }
}

static void SetRupeeCount(u16 rupees) {
    gSaveContext.owl.rupeeCounter += rupees;
}

/**
 * Helper function used to fill rupees based on wallet if enabled.
 **/
static void HandleFillWallet(u8 item) {
    if (!MISC_CONFIG.flags.fillWallet)
        return;

    switch (item) {
        case ITEM_ADULT_WALLET:
            SetRupeeCount(gItemUpgradeCapacity.walletCapacity[1]);
            break;
        case ITEM_GIANT_WALLET:
            SetRupeeCount(gItemUpgradeCapacity.walletCapacity[2]);
            break;
        case CUSTOM_ITEM_ROYAL_WALLET:
            SetRupeeCount(gItemUpgradeCapacity.walletCapacity[3]);
            break;
    }
}

/**
 * Hook function called after receiving an item.
 *
 * Used to add items into quest storage.
 **/
void Items_AfterReceive(GlobalContext* ctxt, u8 item) {
    // Handle receival quest item.
    QuestItems_AfterReceive(item);
    // Handle custom items.
    HandleCustomItem(ctxt, item);
    // Fill rupees if wallet upgrade.
    HandleFillWallet(item);
}

/**
 * Hook function called after removing an item from the inventory and buttons.
 *
 * Used to remove items from quest storage and cycle to the next storage item in the inventory.
 **/
void Items_AfterRemoval(s16 item, s16 slot) {
    // Handle removal of quest item.
    QuestItems_AfterRemoval((u8)item, (u8)slot);
}
