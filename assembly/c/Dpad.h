#ifndef DPAD_H
#define DPAD_H

#include <stdbool.h>
#include <z64.h>

// Magic number for DpadConfig: "DPAD"
#define DPAD_CONFIG_MAGIC 0x44504144

typedef enum {
    DPAD_DISPLAY_NONE,
    DPAD_DISPLAY_LEFT,
    DPAD_DISPLAY_RIGHT,
} DpadDisplay;

typedef enum {
    DPAD_STATE_DISABLED, // 0
    DPAD_STATE_ENABLED,  // 1, enabled with DPAD_CONFIG values
    DPAD_STATE_DEFAULTS, // 2, enabled with DPAD_DEFAULT values
} DpadStateType;

typedef union DpadItems {
    struct {
        /* 0x0 */ u8 du; // Up.
        /* 0x1 */ u8 dr; // Right.
        /* 0x2 */ u8 dd; // Down.
        /* 0x3 */ u8 dl; // Left.
    };
    u8 values[4];
} DpadItems; // size = 0x4

struct DpadConfig {
    /* 0x00 */ u32 magic;
    /* 0x04 */ u32 version;
    /* 0x08 */ DpadItems primary;
    /* 0x0C */ DpadItems alts[3];
    /* 0x18 */ u8 state;
    /* 0x19 */ u8 display;
    /* 0x1A */ u8 reserved[2];
}; // size = 0x1C

extern struct DpadConfig DPAD_CONFIG;

void Dpad_BeforePlayerActorUpdate(ActorPlayer* player, GlobalContext* ctxt);
void Dpad_ClearItemTextures(void);
void Dpad_Init(void);

#endif // DPAD_H
