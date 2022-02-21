#include <z64.h>
#include "Misc.h"
#include "MMR.h"
#include "Reloc.h"

inline bool SwampReward1Flag() {
    return gSaveContext.perm.weekEventReg.bytes[59] & 0x10;
}

inline bool SwampReward2Flag() {
    return gSaveContext.perm.weekEventReg.bytes[32] & 2;
}

inline bool TownReward1Flag() {
    return gSaveContext.perm.weekEventReg.bytes[59] & 0x20;
}

inline bool TownReward2Flag() {
    return gSaveContext.perm.weekEventReg.bytes[32] & 4;
}

static inline bool ShouldGiveSwampReward2() {
    // NOTE: Assumes swamp archery double reward feature is enabled.
    if (MISC_CONFIG.internal.vanillaLayout) {
        return !SwampReward1Flag();
    } else {
        return !SwampReward1Flag() || !MMR_IsRecoveryHeart(0x24);
    }
}

static inline bool ShouldGiveTownReward2() {
    // NOTE: Assumes town archery double reward feature is enabled.
    if (MISC_CONFIG.internal.vanillaLayout) {
        return !TownReward1Flag();
    } else {
        return !TownReward1Flag() || !MMR_IsRecoveryHeart(0x23);
    }
}

static inline bool IsTownPerfectScoreMessage(u16 messageId) {
    return (messageId == 0x405) || (messageId == 0x406);
}

/**
 * Action function used to handle giving second swamp archery reward.
 **/
static void SyatekiMan_Swamp_HandleGiveSecondItem(ActorEnSyatekiMan* actor, GlobalContext* ctxt) {
    if (z2_800B84D0(&actor->base, ctxt)) {
        z2_ShowMessage(ctxt, 0x23E, &actor->base);
        actor->recentMessageId = 0xA34;
        actor->actionFunc = Reloc_ResolveActorOverlay(&gActorOverlayTable[ACTOR_EN_SYATEKI_MAN], 0x809C6E30);
        // Set special score value to indicate processing second reward.
        actor->currentScore = 0x7FFF;
    } else {
        z2_800B85E0(&actor->base, ctxt, 500.0f, -1);
    }
}

/**
 * Hook function to determine the next action function after giving a swamp archery reward.
 **/
void* SyatekiMan_Swamp_DetermineActionFunctionAfterGiveItem(ActorEnSyatekiMan* actor, GlobalContext* ctxt) {
    // Check if we are set up to grant second swamp archery reward.
    if (MISC_CONFIG.speedups.doubleArchery && actor->currentScore >= 0x884 && ShouldGiveSwampReward2() && actor->currentScore != 0x7FFF) {
        return SyatekiMan_Swamp_HandleGiveSecondItem;
    } else {
        // Set default function pointer.
        return Reloc_ResolveActorOverlay(&gActorOverlayTable[ACTOR_EN_SYATEKI_MAN], 0x809C7C14);
    }
}

/**
 * Whether or not the quiver reward should be given.
 **/
static inline bool SyatekiMan_Swamp_ShouldGiveQuiverReward(ActorEnSyatekiMan* actor, GlobalContext* ctxt) {
    if (MISC_CONFIG.speedups.doubleArchery) {
        // Check for special score value to prevent giving quiver reward again.
        return ShouldGiveSwampReward2() && actor->currentScore != 0x7FFF;
    } else {
        // Default behavior, only give quiver reward if lesser reward flag not set.
        return !SwampReward1Flag();
    }
}

/**
 * Hook function to check if the quiver reward should be skipped in conditionals.
 **/
bool SyatekiMan_Swamp_ShouldNotGiveQuiverReward(ActorEnSyatekiMan* actor, GlobalContext* ctxt) {
    return !SyatekiMan_Swamp_ShouldGiveQuiverReward(actor, ctxt);
}

/**
 * Action function used to handle giving second town archery reward.
 **/
static void SyatekiMan_Town_HandleGiveSecondItem(ActorEnSyatekiMan* actor, GlobalContext* ctxt) {
    if (z2_800B84D0(&actor->base, ctxt)) {
        z2_ShowMessage(ctxt, 0x405, &actor->base);
        actor->recentMessageId = 0x405;
        actor->actionFunc = Reloc_ResolveActorOverlay(&gActorOverlayTable[ACTOR_EN_SYATEKI_MAN], 0x809C7990);
    } else {
        z2_800B85E0(&actor->base, ctxt, 500.0f, -1);
    }
}

/**
 * Hook function to determine the next action function after giving a town archery reward.
 **/
void* SyatekiMan_Town_DetermineActionFunctionAfterGiveItem(ActorEnSyatekiMan* actor, GlobalContext* ctxt) {
    // Check if we are set up to grant second town archery reward.
    if (MISC_CONFIG.speedups.doubleArchery && !IsTownPerfectScoreMessage(actor->recentMessageId) && actor->currentScore == 50 && ShouldGiveTownReward2()) {
        return SyatekiMan_Town_HandleGiveSecondItem;
    } else {
        // Set default function pointer.
        return Reloc_ResolveActorOverlay(&gActorOverlayTable[ACTOR_EN_SYATEKI_MAN], 0x809C7EB4);
    }
}

static inline bool SyatekiMan_Town_ShouldGiveLesserReward(ActorEnSyatekiMan* actor, GlobalContext* ctxt) {
    if (MISC_CONFIG.speedups.doubleArchery) {
        // Check if lesser reward should be granted.
        return !TownReward1Flag() || actor->currentScore < 50 || (actor->currentScore == 50 && ShouldGiveTownReward2());
    } else {
        // Default behavior, only give greater reward if lesser reward has been received.
        return !TownReward1Flag();
    }
}

/**
 * Hook function to check if the initial lesser reward check should be skipped in conditionals.
 **/
bool SyatekiMan_Town_ShouldNotGiveLesserReward(ActorEnSyatekiMan* actor, GlobalContext* ctxt) {
    return !SyatekiMan_Town_ShouldGiveLesserReward(actor, ctxt);
}
