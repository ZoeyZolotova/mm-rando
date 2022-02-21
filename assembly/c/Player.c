#include <stdbool.h>
#include <z64.h>
#include "ArrowCycle.h"
#include "ArrowMagic.h"
#include "DekuHop.h"
#include "Dpad.h"
#include "ExternalEffects.h"
#include "Icetrap.h"
#include "Reloc.h"
#include "Misc.h"
#include "enums.h"

bool Player_BeforeDamageProcess(ActorPlayer* player, GlobalContext* ctxt) {
    return Icetrap_Give(player, ctxt);
}

void Player_BeforeUpdate(ActorPlayer* player, GlobalContext* ctxt) {
    Dpad_BeforePlayerActorUpdate(player, ctxt);
    ExternalEffects_Handle(player, ctxt);
    ArrowCycle_Handle(player, ctxt);
    ArrowMagic_Handle(player, ctxt);
    DekuHop_Handle(player, ctxt);
}

bool Player_CanReceiveItem(GlobalContext* ctxt) {
    ActorPlayer* player = GET_PLAYER(ctxt);
    if ((player->stateFlags.state1 & PLAYER_STATE1_AIM) != 0) {
        return false;
    }
    bool result = false;
    switch (player->currentAnimation.id) {
        case 0xE208: // rolling - Goron
            result = ctxt->state.input[0].current.buttons.a != 0;
            break;
        case 0xDD28: // rolling - Human, Goron, Zora
        case 0xDF20: // idle - Human with sword
        case 0xDF28: // idle - Human, Deku
        case 0xE260: // idle - Goron
        case 0xE410: // idle - Zora
        case 0xD988: // idle - Fierce Deity
        case 0xD918: // walking with sword
        case 0xDE40: // walking - Human, Deku, Goron, Zora
        case 0xD920: // walking - Fierce Deity
        case 0xD800: // backwalking after backflip - Human, Goron, Zora, Fierce Deity
        case 0xDD18: // backwalking after backflip - Deku
        case 0xD928: // sidewalking - Fierce Deity
        case 0xDE78: // sidewalking - Human, Deku, Goron, Zora
            result = true;
            break;
    }
    return result;
}

void Player_Pause(GlobalContext* ctxt) {
    ActorPlayer* player = GET_PLAYER(ctxt);
    if (!(player->stateFlags.state1 & PLAYER_STATE1_GROTTO_IN)) {
        player->stateFlags.state1 |= PLAYER_STATE1_TIME_STOP_2;
    }
}

void Player_Unpause(GlobalContext* ctxt) {
    ActorPlayer* player = GET_PLAYER(ctxt);
    player->stateFlags.state1 &= ~PLAYER_STATE1_TIME_STOP_2;
}

/**
 * Helper function used to perform effects if entering water, and update the player swim flag.
 **/
static void HandleEnterWater(ActorPlayer* player, GlobalContext* ctxt) {
    // Check water distance to determine if in water.
    if (player->tableA68[11] < player->base.waterSurfaceDist) {
        // If swim flag not set, perform effects (sound + visual) for entering water.
        if ((player->stateFlags.state1 & PLAYER_STATE1_SWIM) == 0) {
            z2_PerformEnterWaterEffects(ctxt, player);
        }
        // Set swim flag.
        player->stateFlags.state1 |= PLAYER_STATE1_SWIM;
    }
}

/**
 * Helper function called to check if the player is in water.
 **/
static bool InWater(ActorPlayer* player) {
    return ((player->stateFlags.state1 & PLAYER_STATE1_SWIM) != 0 ||
            (player->tableA68[11] < player->base.waterSurfaceDist));
}

/**
 * Hook function called before handling "frozen" player state.
 **/
void Player_BeforeHandleFrozenState(ActorPlayer* player, GlobalContext* ctxt) {
    HandleEnterWater(player, ctxt);
    // If frozen while swimming, should float on water.
    if (InWater(player)) {
        z2_PlayerHandleBuoyancy(player);
    }
}

/**
 * Hook function called before handling "voiding" player state.
 **/
void Player_BeforeHandleVoidingState(ActorPlayer* player, GlobalContext* ctxt) {
    HandleEnterWater(player, ctxt);
    // Note: Later in the function of this hook, is where the "frozen" player state flag is set.
    // Since we can't check the player state flags, do the same check this function does instead.
    bool frozen = player->currentAnimation.value == 0;
    bool zora = player->form == 2;
    // If Zora is voiding (frozen) and swimming, should float in water.
    if (zora && frozen && InWater(player)) {
        z2_PlayerHandleBuoyancy(player);
    }
}

/**
 * Hook function to check whether or not freezing should void Zora.
 **/
bool Player_ShouldIceVoidZora(ActorPlayer* player, GlobalContext* ctxt) {
    switch (ctxt->sceneNum) {
        case 0x1F: // Odolwa Boss Room
        case 0x44: // Goht Boss Room
        case 0x5F: // Gyorg Boss Room
        case 0x36: // Twinmold Boss Room
            return false;
        default:
            return true;
    }
}

/**
 * Hook function called to determine if swim state should be prevented from being restored.
 **/
bool Player_ShouldPreventRestoringSwimState(ActorPlayer* player, GlobalContext* ctxt, void* function) {
    void* handleFrozen = Reloc_ResolvePlayerOverlay(&s801D0B70.playerActor, 0x808546D0); // Offset: 0x26C40
    return function == handleFrozen;
}

static u32 lastClimbFrame = 0;
static u32 startClimbingTimer = 5;
u32 Player_GetCollisionType(ActorPlayer* player, GlobalContext* ctxt, u32 collisionType) {
    if (!MISC_CONFIG.flags.climbAnything) {
        return collisionType;
    }

    if ((player->stateFlags.state1 & PLAYER_STATE1_SWIM) && !(player->stateFlags.state2 & PLAYER_STATE2_DIVING_2)) {
        return 8;
    }

    u32 currentClimbFrame = ctxt->state.frames;

    if (currentClimbFrame == lastClimbFrame + 1) {
        startClimbingTimer--;
    } else {
        startClimbingTimer = 5;
    }

    if (startClimbingTimer == 0) {
        return 8;
    }

    lastClimbFrame = currentClimbFrame;

    return collisionType;
}
