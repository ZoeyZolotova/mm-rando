#ifndef MISC_H
#define MISC_H

#include <z64.h>

enum CritWiggleState {
    CRIT_WIGGLE_DEFAULT,
    CRIT_WIGGLE_ALWAYS_ON,
    CRIT_WIGGLE_ALWAYS_OFF,
};

enum QuestConsumeState {
    QUEST_CONSUME_DEFAULT,
    QUEST_CONSUME_ALWAYS,
    QUEST_CONSUME_NEVER,
};

enum AutoInvertState {
    AUTO_INVERT_NEVER,
    AUTO_INVERT_FIRST_CYCLE,
    AUTO_INVERT_ALWAYS,
};

// Magic number for misc_config: "MISC"
#define MISC_CONFIG_MAGIC 0x4D495343

typedef struct {
    // Version 0 flags
    u32 critWiggle          : 2;
    u32 drawHash            : 1;
    u32 fastPush            : 1;
    u32 ocarinaUnderwater   : 1;
    u32 questItemStorage    : 1;
    // Version 1 flags
    u32 closeCows           : 1;
    u32 freestanding        : 1;
    u32 questConsume        : 2;
    u32 arrowCycle          : 1;
    u32 arrowMagicShow      : 1;
    // Version 2 flags
    u32 elegySpeedup        : 1;
    u32 continuousDekuHop   : 1;
    u32 shopModels          : 1;
    u32 progressiveUpgrades : 1;
    u32 iceTrapQuirks       : 1;
    u32 mikauEarlyBeach     : 1;
    u32 fairyChests         : 1;
    u32 targetHealth        : 1;
    u32 climbAnything       : 1;
    u32 freeScarecrow       : 1;
    u32 fillWallet          : 1;
    u32 autoInvert          : 2;
    u32                     : 7;
} MiscFlags;

typedef union {
    u8 bytes[16];
    u32 value;
    u32 words[4];
} MiscHash;

typedef struct {
    // Version 1 flags
    u32 vanillaLayout             : 1;
    u32                           : 31;
} MiscInternal;

typedef struct {
    // Version 3 flags
    u32 soundCheck     : 1;
    u32 blastMaskThief : 1;
    u32 fishermanGame  : 1;
    u32 boatArchery    : 1;
    u32 donGero        : 1;
    u32 fastBankRupees : 1;
    u32 doubleArchery  : 1;
    u32 multiBank      : 1;
    u32                : 24;
} MiscSpeedups;

typedef struct {
    u16 collectableTableFileIndex;
    u16 bankWithdrawFee;
} MiscShorts;

struct MiscConfig {
    /* 0x00 */ u32 magic;
    /* 0x04 */ u32 version;
    /* 0x08 */ MiscHash hash;
    /* 0x18 */ MiscFlags flags;
    /* 0x1C */ MiscInternal internal;
    /* 0x20 */ MiscSpeedups speedups;
    /* 0x24 */ MiscShorts shorts;
}; // size = 0x28

extern struct MiscConfig MISC_CONFIG;

void Misc_Init(void);

#endif // MISC_H
