#include <stdbool.h>
#include <z64.h>
#include "ExternalEffects.h"
#include "Fairy.h"
#include "FloorPhysics.h"
#include "Icetrap.h"

// Fairy spawn cooldown (in frames).
#define FAIRY_COOLDOWN 0x10

// Maximum amount of fairies in queue.
#define FAIRY_MAX 12

// Jinx unit amount (multiplied by value in structure, then added to timer).
#define JINX_AMOUNT 0x100

// Maximum jinx timer value in memory.
#define JINX_MAX 0xFFFF

ExternalEffectsConfig gExternalEffects = {
    .magic = EXTERNAL_EFFECTS_MAGIC,
    .version = 0,
    .cameraOverlook = 0,
    .chateau = 0,
    .fairy = 0,
    .damageEffect = 0,
    .icePhysics = 0,
    .jinx = 0,
    .noZ = 0,
    .reverseControls = 0,
    .playSfx = 0,
    .lowHpSfx = 0,
    .yellowIframe = 0,
    .swapButtons = 0,
};

// Specifies camera states that can be changed via an effect.
static const bool gFreecamModes[0x5B] = {
    1, // 0x00 NONE
    1, // 0x01 NORMAL0
    1, // 0x02 NORMAL3
    1, // 0x03 CIRCLE5
    1, // 0x04 HORSE0
    1, // 0x05 ZORA0
    1, // 0x06 PREREND0
    1, // 0x07 PREREND1
    1, // 0x08 DOORC
    1, // 0x09 DEMO0
    1, // 0x0A FREE0
    1, // 0x0B FUKAN0
    1, // 0x0C NORMAL1
    1, // 0x0D NANAME
    1, // 0x0E CIRCLE0
    1, // 0x0F FIXED0
    1, // 0x10 SPIRAL
    1, // 0x11 DUNGEON0
    1, // 0x12 ITEM0
    1, // 0x13 ITEM1
    1, // 0x14 ITEM2
    1, // 0x15 ITEM3
    1, // 0x16 NAVI
    1, // 0x17 WARP0
    1, // 0x18 DEATH
    1, // 0x19 REBIRTH
    1, // 0x1A TREASURE
    1, // 0x1B TRANSFORM
    1, // 0x1C ATTENTION
    1, // 0x1D WARP1
    1, // 0x1E DUNGEON1
    1, // 0x1F FIXED1
    1, // 0x20 FIXED2
    1, // 0x21 MAZE
    1, // 0x22 REMOTEBOMB
    1, // 0x23 CIRCLE1
    1, // 0x24 CIRCLE2
    1, // 0x25 CIRCLE3
    1, // 0x26 CIRCLE4
    1, // 0x27 FIXED3
    1, // 0x28 TOWER0
    1, // 0x29 PARALLEL0
    1, // 0x2A NORMALD
    1, // 0x2B SUBJECTD
    1, // 0x2C START0
    1, // 0x2D START2
    1, // 0x2E STOP0
    1, // 0x2F JCRUISING
    1, // 0x30 CLIMEMAZE
    1, // 0x31 SIDED
    1, // 0x32 DUNGEON2
    1, // 0x33 BOSS_SHIGE
    1, // 0x34 KEEPBACK
    1, // 0x35 CIRCLE6
    1, // 0x36 CIRCLE7
    1, // 0x37 CHUBOSS
    1, // 0x38 RFIXED1
    1, // 0x39 TRESURE1
    1, // 0x3A BOMBBASKET
    1, // 0x3B CIRCLE8
    1, // 0x3C FUKAN1
    1, // 0x3D DUNGEON3
    1, // 0x3E TELESCOPE
    1, // 0x3F ROOM0
    1, // 0x40 RCIRC0
    1, // 0x41 CIRCLE9
    1, // 0x42 ONTHEPOLE
    1, // 0x43 INBUSH
    1, // 0x44 BOSS_LAST
    1, // 0x45 BOSS_INI
    1, // 0x46 BOSS_HAK
    1, // 0x47 BOSS_KON
    1, // 0x48 CONNECT0
    1, // 0x49 MORAY
    1, // 0x4A NORMAL2
    1, // 0x4B BOMBBOWL
    1, // 0x4C CIRCLEa
    1, // 0x4D WHIRLPOOL
    1, // 0x4E KOKKOGAME
    1, // 0x4F GIANT
    1, // 0x50 SCENE0
    1, // 0x51 ROOM1
    1, // 0x52 WATER2
    1, // 0x53 SOKONASI
    1, // 0x54 FORCEKEEP
    1, // 0x55 PARALLEL1
    1, // 0x56 START1
    1, // 0x57 ROOM2
    1, // 0x58 NORMAL4
    1, // 0x59 SHELL
    1, // 0x5A DUNGEON4
};

// Whether or not to refill some magic on this frame.
static bool gRefillMagic = false;

// Whether or not we are in our special jinx state.
static bool gJinxed = false;

// Previous jinx value in our special jinx state.
static u16 gPreviousJinxValue = 0;

// Cooldown for spawning fairies, in frames.
static u32 gFairyCooldown = 0;

// Most recent scene, tracked for fairy usage.
static u16 gFairyScene = 0;

static void HandleCameraOverlookEffect(GlobalContext* ctxt, ActorPlayer* player) {
    // Handle "Camera Overlook" effect.
    if (gExternalEffects.cameraOverlook) {
        s16 curstate = ctxt->activeCameras[0].state;
        if (gFreecamModes[curstate] && curstate != CAMERA_STATE_FUKAN1) {
            ctxt->activeCameras[0].state = CAMERA_STATE_FUKAN1;
            // Camera mode used while Z-targetting, it should trigger the camera to begin drifting over Link
            ctxt->activeCameras[0].mode = CAMERA_MODE_PARALLEL;
        }
    }
}

static void HandleChateauEffect(GlobalContext* ctxt, ActorPlayer* player) {
    // Handle "Chateau" effect
    if (gExternalEffects.chateau) {
        gSaveContext.perm.weekEventReg.hasInfiniteMagic = true;
        gRefillMagic = true;
        gExternalEffects.chateau = 0;
    }

    // Refill magic
    if (gRefillMagic) {
        u8 total = (u8)gSaveContext.extra.magicMeterSize;
        u8 remaining = total - gSaveContext.perm.unk24.currentMagic;

        // Increment by 2 magic points per frame
        if (remaining >= 2) {
            gSaveContext.perm.unk24.currentMagic += 2;
            z2_PlaySfx(0x401F);
        } else if (remaining == 1) {
            gSaveContext.perm.unk24.currentMagic += 1;
            z2_PlaySfx(0x401F);
        }

        // If full, stop refilling magic
        if (gSaveContext.perm.unk24.currentMagic >= total) {
            gSaveContext.perm.unk24.currentMagic = total; // Just in case
            gRefillMagic = false;
        }
    }
}

static void HandleFairyEffect(GlobalContext* ctxt, ActorPlayer* player) {
    // Reset fairy instance usages when scene changes
    if (ctxt->sceneNum != gFairyScene) {
        Fairy_ResetInstanceUsage();
        gFairyScene = ctxt->sceneNum;
        gFairyCooldown = 0;
    }

    // Enforce a maximum of fairies in queue
    if (gExternalEffects.fairy > FAIRY_MAX) {
        gExternalEffects.fairy = FAIRY_MAX;
    }

    // Check state type to see if we can receive a fairy during this frame
    if (gExternalEffects.fairy && gFairyCooldown == 0 && Fairy_CanInteractWith(ctxt, player)) {
        // Spawn fairy on top of Link, and call the function to interact
        Actor* fairy = Fairy_SpawnNextActor(ctxt, player->base.initPosRot.pos);
        if (fairy) {
            if (fairy->update != NULL) {
                fairy->update(fairy, ctxt);
            }
            gFairyCooldown = FAIRY_COOLDOWN;
            gExternalEffects.fairy -= 1;
        }
    }

    // Decrement cooldown per frame
    if (gFairyCooldown > 0) {
        gFairyCooldown -= 1;
    }
}

static void HandleDamageEffect(GlobalContext* ctxt, ActorPlayer* player) {
    // Handle "Damage Effect" effect.
    if (gExternalEffects.damageEffect) {
        Icetrap_PushPending(gExternalEffects.damageEffect);
        gExternalEffects.damageEffect = 0;
    }
}

static void HandleIcePhysicsEffect(GlobalContext* ctxt, ActorPlayer* player) {
    // Handle "Ice Physics" effect.
    if (gExternalEffects.icePhysics) {
        FloorPhysics_OverrideType(true, FLOOR_PHYSICS_ICE);
    } else {
        FloorPhysics_OverrideType(false, 0);
    }
}

static void HandleJinxEffect(GlobalContext* ctxt, ActorPlayer* player) {
    // Handle "Jinx" effect.
    if (gExternalEffects.jinx) {
        // Add multiple of JINX_AMOUNT to jinx timer
        u32 amount = gExternalEffects.jinx * JINX_AMOUNT;
        u32 timer = gSaveContext.owl.jinxCounter + amount;
        timer = (timer < JINX_MAX ? timer : JINX_MAX);
        gSaveContext.owl.jinxCounter = (u16)timer;

        gExternalEffects.jinx = 0;
        gJinxed = true;

        gPreviousJinxValue = gSaveContext.owl.jinxCounter;
    }

    // This is a special jinx, players cannot Song of Storms out of it.
    if (gJinxed) {
        // Calculate current expected jinx value
        u16 expected = 0;
        if (gPreviousJinxValue > 1) {
            expected = gPreviousJinxValue - 1;
        }
        // If actual value is less than expected, this means Song of Storms was played or went back in time.
        if (gSaveContext.owl.jinxCounter < expected) {
            gSaveContext.owl.jinxCounter = expected;
        } else if (gSaveContext.owl.jinxCounter == 0) {
            // Once the timer hits 0, disable our special jinx state.
            gJinxed = false;
        }
        gPreviousJinxValue = gSaveContext.owl.jinxCounter;
    }
}

static void HandleNoZEffect(GlobalContext* ctxt, ActorPlayer* player) {
    // Handle "No Z" effect.
    if (gExternalEffects.noZ) {
        ctxt->state.input[0].current.buttons.z = 0;
        ctxt->state.input[0].pressEdge.buttons.z = 0;
    }
}

static void HandleReverseControlsEffect(GlobalContext* ctxt, ActorPlayer* player) {
    // Handle "Reverse Controls" effect.
    if (gExternalEffects.reverseControls) {
        ctxt->state.input[0].current.xAxis = -ctxt->state.input[0].current.xAxis;
        ctxt->state.input[0].current.yAxis = -ctxt->state.input[0].current.yAxis;
        ctxt->state.input[0].pressEdge.xAxis = -ctxt->state.input[0].pressEdge.xAxis;
        ctxt->state.input[0].pressEdge.yAxis = -ctxt->state.input[0].pressEdge.yAxis;
        ctxt->state.input[0].releaseEdge.xAxis = -ctxt->state.input[0].releaseEdge.xAxis;
        ctxt->state.input[0].releaseEdge.yAxis = -ctxt->state.input[0].releaseEdge.yAxis;
    }
}

static void HandlePlaySfxEffect(GlobalContext* ctxt, ActorPlayer* player) {
    if (gExternalEffects.playSfx) {
        z2_PlaySfx(gExternalEffects.playSfx);
        gExternalEffects.playSfx = 0;
    }
}

static void HandleIFrameGlow(GlobalContext* ctxt, ActorPlayer* player) {
    if (gExternalEffects.yellowIframe == 1 && player->invincibilityFrames == 0) {
        gExternalEffects.yellowIframe = 0;
    }
}

#define SWAP_VALUES(T, x, y) { T temp = x; x = y; y = temp; }

static void HandleSwapButtons(GlobalContext* ctxt, ActorPlayer* player) {
    if (gExternalEffects.swapButtons) {
        SWAP_VALUES(u16, ctxt->state.input[0].current.buttons.a, ctxt->state.input[0].current.buttons.b);
        SWAP_VALUES(u16, ctxt->state.input[0].pressEdge.buttons.a, ctxt->state.input[0].pressEdge.buttons.b);
        SWAP_VALUES(u16, ctxt->state.input[0].releaseEdge.buttons.a, ctxt->state.input[0].releaseEdge.buttons.b);
    }
}

void ExternalEffects_PlayLowHpSfx(u32 id) {
    if (gExternalEffects.lowHpSfx) {
        z2_PlaySfx(gExternalEffects.lowHpSfx);
    }
    else if (id) {
        z2_PlaySfx(id);
    }
}

Gfx* ExternalEffects_IFrameGlow(Gfx* gfx, u8 r, u8 g, u8 b, u8 a, u16 unk_a5, f32 unk_a6) {
    if (gExternalEffects.yellowIframe) {
        g = 0xFF;
    }
    return z2_8012BC50(gfx, r, g, b, a, unk_a5, unk_a6);
}

void ExternalEffects_Handle(ActorPlayer* player, GlobalContext* ctxt) {
    HandleCameraOverlookEffect(ctxt, player);
    HandleChateauEffect(ctxt, player);
    HandleFairyEffect(ctxt, player);
    HandleDamageEffect(ctxt, player);
    HandleIcePhysicsEffect(ctxt, player);
    HandleJinxEffect(ctxt, player);
    HandleNoZEffect(ctxt, player);
    HandleReverseControlsEffect(ctxt, player);
    HandlePlaySfxEffect(ctxt, player);
    HandleIFrameGlow(ctxt, player);
    HandleSwapButtons(ctxt, player);
}
