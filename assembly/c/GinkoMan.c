#include <macro.h>
#include <z64.h>
#include "Misc.h"
#include "Reloc.h"

typedef enum {
    /* 0 */ GINKO_FLOORSMACKING,
    /* 1 */ GINKO_SITTING,
    /* 2 */ GINKO_REACHING,
    /* 3 */ GINKO_AMAZED,
    /* 4 */ GINKO_ADVERTISING,
} GinkoAnimationIndex;

static inline ActorAnimationEntry* GetActorAnimations() {
    return (ActorAnimationEntry*)Reloc_ResolveActorOverlay(&gActorOverlayTable[ACTOR_EN_GINKO_MAN], 0x80A65D60);
}

static inline bool BankRewardFlag1(void) {
    return gSaveContext.perm.weekEventReg.bytes[59] & 0x40;
}

static inline bool BankRewardFlag2(void) {
    return gSaveContext.perm.weekEventReg.bytes[59] & 0x80;
}

static inline bool BankRewardFlag3(void) {
    return gSaveContext.perm.weekEventReg.bytes[60] & 1;
}

static inline s16 BankRewardRupees1(void) {
    return *(const s16*)(Reloc_ResolveActorOverlay(&gActorOverlayTable[ACTOR_EN_GINKO_MAN], 0x80A648D6));
}

static inline s16 BankRewardRupees2(void) {
    return *(const s16*)(Reloc_ResolveActorOverlay(&gActorOverlayTable[ACTOR_EN_GINKO_MAN], 0x80A648DE));
}

static inline s16 BankRewardRupees3(void) {
    return *(const s16*)(Reloc_ResolveActorOverlay(&gActorOverlayTable[ACTOR_EN_GINKO_MAN], 0x80A6492A));
}

static inline s16 GetBankRupees(void) {
    return (s16)(gSaveContext.perm.bankRupees & 0xFFFF);
}

static bool PassedThreshold(const ActorEnGinkoMan* actor, s16 amount) {
    return (GetBankRupees() >= amount) && (actor->previousBankValue < amount);
}

static int GetNextAwardIndex(const ActorEnGinkoMan* actor) {
    if (PassedThreshold(actor, BankRewardRupees1()) && !BankRewardFlag1()) {
        return 1;
    } else if (PassedThreshold(actor, BankRewardRupees2()) && !BankRewardFlag2()) {
        return 2;
    } else if (PassedThreshold(actor, BankRewardRupees3()) && !BankRewardFlag3()) {
        return 3;
    } else {
        return 0;
    }
}

/**
 * Hook function used to change animation post-award for award #2.
 *
 * Only changes animation if this is the final award being granted.
 */
void GinkoMan_AfterAward_ChangeAnimation(SkelAnime* skelAnime, ActorAnimationEntry* animations, s32 index) {
    const ActorEnGinkoMan* actor = FIELD_TO_STRUCT_CONST(ActorEnGinkoMan, skelAnime, skelAnime);

    if (!MISC_CONFIG.speedups.multiBank || GetNextAwardIndex(actor) == 0) {
        z2_Actor_ChangeAnimation(skelAnime, animations, index);
    }
}

/**
 * Hook function used to display post-award message for award #1 and #2.
 *
 * If non-final award being granted, uses modified text which doesn't terminate with 0xBF control char. This means
 * we can stay in dialogue state for 0x47A and 0x47B message Ids and process other rewards.
 */
void GinkoMan_AfterAward_DisplayText(GlobalContext* ctxt, u16 messageId, ActorEnGinkoMan* actor) {
    if (MISC_CONFIG.speedups.multiBank && GetNextAwardIndex(actor) != 0) {
        if (messageId == 0x47A) {
            // Show replacement message for 0x47A (Bank post-reward #1).
            z2_ShowMessage(ctxt, 0x1B67, &actor->base);
        } else {
            // Show replacement message for 0x47B (Bank post-reward #2).
            z2_ShowMessage(ctxt, 0x1B77, &actor->base);
        }
    } else {
        // Vanilla behavior: Show the original message with 0xBF control char to close message state.
        z2_ShowMessage(ctxt, messageId, &actor->base);
    }
}
