#ifndef STATIC_ADDRESS_H
#define STATIC_ADDRESS_H

#include "Dpad.h"
#include "ExternalEffects.h"
#include "HudColors.h"
#include "Misc.h"
#include "MMR.h"
#include "Music.h"
#include "SaveFile.h"
#include "WorldColors.h"

// Magic number for StaticAddressConfig: "STAD"
#define STATIC_ADDRESS_CONFIG_MAGIC 0x53544144

typedef struct {
    u32 magic;
    u32 version;
    ExternalEffectsConfig* externalEffectsConfig;
    struct DpadConfig* dpadConfig;
    struct HudColorConfig* hudColorConfig;
    struct MiscConfig* miscConfig;
    struct MMRConfig* mmrConfig;
    struct MusicConfig* musicConfig;
    struct SaveFileConfig* saveFileConfig;
    struct WorldColorConfig* worldColorConfig;
} StaticAddressConfig;

void StaticAddress_Init(void);

#endif // STATIC_ADDRESS_H
