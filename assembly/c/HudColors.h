#ifndef HUD_COLORS_H
#define HUD_COLORS_H

#include <z64.h>

// Magic number for HudColorConfig: "HUDC"
#define HUD_COLOR_CONFIG_MAGIC 0x48554443

struct HudColorConfig {
    u32 magic;
    u32 version;

    // Version 0
    Color buttonA;
    Color buttonB;
    Color buttonC;
    Color buttonStart;
    Color clockEmblem;
    Color clockEmblemInverted1;
    Color clockEmblemInverted2;
    Color clockEmblemSun;
    Color clockMoon;
    Color clockSun;
    Color heart;
    Color heartDd;
    Color magicNormal;
    Color magicInf;
    Color map;
    Color mapEntranceCursor;
    Color mapPlayerCursor;
    Color rupee[3];

    // Version 1
    Color buttonIconA;
    Color buttonIconB;
    Color buttonIconC;
    Color menuAInner1;
    Color menuAInner2;
    Color menuAOuter1;
    Color menuAOuter2;
    Color menuCInner1;
    Color menuCInner2;
    Color menuCOuter1;
    Color menuCOuter2;
    Color noteA1;
    Color noteA2;
    Color noteAShadow1;
    Color noteAShadow2;
    Color noteC1;
    Color noteC2;
    Color noteCShadow1;
    Color noteCShadow2;
    Color pauseTitleA;
    Color pauseTitleC;
    Color prompt1;
    Color prompt2;
    Color promptGlow;
    Color scoreLines;
    Color scoreNote;

    // Version 2
    Color dpad;

    // Version 3
    Color menuBorder1;
    Color menuBorder2;
    Color menuSubtitleText;
    Color shopCursor1;
    Color shopCursor2;

    Color royalRupee;
};

extern struct HudColorConfig HUD_COLOR_CONFIG;

void HudColors_Init(void);
void HudColors_FileChooseInit(void);
void HudColors_UpdatePauseMenuColors(GlobalContext* ctxt);

#endif // HUD_COLORS_H
