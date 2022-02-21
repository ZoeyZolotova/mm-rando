#ifndef EXTERNAL_EFFECTS_H
#define EXTERNAL_EFFECTS_H

#include <z64.h>

// Magic number for external_effects: "EXFX"
#define EXTERNAL_EFFECTS_MAGIC 0x45584658

typedef struct {
    u32 magic;
    u32 version;

    // Effects added in version 0
    u8 cameraOverlook;
    u8 chateau;
    u8 fairy;
    u8 damageEffect;

    u8 icePhysics;
    u8 jinx;
    u8 noZ;
    u8 reverseControls;

    u16 playSfx;
    u16 lowHpSfx;

    u8 yellowIframe;
    u8 swapButtons;
    u8 pad[2];
} ExternalEffectsConfig;

void ExternalEffects_Handle(ActorPlayer* player, GlobalContext* ctxt);

extern ExternalEffectsConfig gExternalEffects;

#endif // EXTERNAL_EFFECTS_H
