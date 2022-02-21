#include <stdbool.h>
#include <z64.h>
#include "Color.h"
#include "HudColors.h"
#include "Reloc.h"

struct HudColorConfig HUD_COLOR_CONFIG = {
    .magic = HUD_COLOR_CONFIG_MAGIC,
    .version = 3,

    // Version 0
    .buttonA              = { 0x64, 0xC8, 0xFF },
    .buttonB              = { 0x64, 0xFF, 0x78 },
    .buttonC              = { 0xFF, 0xF0, 0x00 },
    .buttonStart          = { 0xFF, 0x82, 0x3C },
    .clockEmblem          = { 0x00, 0xAA, 0x64 },
    .clockEmblemInverted1 = { 0x64, 0xCD, 0xFF },
    .clockEmblemInverted2 = { 0x00, 0x9B, 0xFF },
    .clockEmblemSun       = { 0xFF, 0xFF, 0x6E },
    .clockMoon            = { 0xFF, 0xFF, 0x37 },
    .clockSun             = { 0xFF, 0x64, 0x6E },
    .heart                = { 0xFF, 0x46, 0x32 },
    .heartDd              = { 0xC8, 0x00, 0x00 },
    .magicNormal          = { 0x00, 0xC8, 0x00 },
    .magicInf             = { 0x00, 0x00, 0xC8 },
    .map                  = { 0x00, 0xFF, 0xFF },
    .mapEntranceCursor    = { 0xC8, 0x00, 0x00 },
    .mapPlayerCursor      = { 0xC8, 0xFF, 0x00 },
    .rupee                = {
                            { 0xC8, 0xFF, 0x64 },
                            { 0xAA, 0xAA, 0xFF },
                            { 0xFF, 0x69, 0x69 },
    },

    // Version 1
    .buttonIconA          = { 0x50, 0x5A, 0xFF },
    .buttonIconB          = { 0x46, 0xFF, 0x50 },
    .buttonIconC          = { 0xFF, 0xFF, 0x32 },
    .menuAInner1          = { 0x64, 0x96, 0xFF },
    .menuAInner2          = { 0x64, 0xFF, 0xFF },
    .menuAOuter1          = { 0x00, 0x00, 0x64 },
    .menuAOuter2          = { 0x00, 0x96, 0xFF },
    .menuCInner1          = { 0xFF, 0xFF, 0x00 },
    .menuCInner2          = { 0xFF, 0xFF, 0x00 },
    .menuCOuter1          = { 0x00, 0x00, 0x00 },
    .menuCOuter2          = { 0xFF, 0xA0, 0x00 },
    .noteA1               = { 0x50, 0x96, 0xFF },
    .noteA2               = { 0x64, 0xC8, 0xFF },
    .noteAShadow1         = { 0x0A, 0x0A, 0x0A },
    .noteAShadow2         = { 0x32, 0x32, 0xFF },
    .noteC1               = { 0xFF, 0xFF, 0x32 },
    .noteC2               = { 0xFF, 0xFF, 0xB4 },
    .noteCShadow1         = { 0x0A, 0x0A, 0x0A },
    .noteCShadow2         = { 0x6E, 0x6E, 0x32 },
    .pauseTitleA          = { 0x00, 0x64, 0xFF },
    .pauseTitleC          = { 0xFF, 0x96, 0x00 },
    .prompt1              = { 0x00, 0x50, 0xC8 },
    .prompt2              = { 0x32, 0x82, 0xFF },
    .promptGlow           = { 0x00, 0x82, 0xFF },
    .scoreLines           = { 0xFF, 0x00, 0x00 },
    .scoreNote            = { 0xFF, 0x64, 0x00 },

    // Version 2
    .dpad                 = { 0x80, 0x80, 0x80 },

    // Version 3
    .menuBorder1          = { 0xB4, 0xB4, 0x78 },
    .menuBorder2          = { 0x96, 0x8C, 0x5A },
    .menuSubtitleText     = { 0xFF, 0xC8, 0x00 },
    .shopCursor1          = { 0x00, 0x00, 0xFF },
    .shopCursor2          = { 0x00, 0x50, 0xFF },

    .royalRupee           = { 0xA0, 0x60, 0xFF },
};

struct PauseCursorColors {
    /* 0x00 */ ColorRGB16 defaultInner1;
    /* 0x06 */ ColorRGB16 defaultInner2;
    /* 0x0C */ ColorRGB16 yellowInner1;
    /* 0x12 */ ColorRGB16 yellowInner2;
    /* 0x18 */ ColorRGB16 blueInner1;
    /* 0x1E */ ColorRGB16 blueInner2;
    /* 0x24 */ ColorRGB16 defaultOuter1;
    /* 0x2A */ ColorRGB16 defaultOuter2;
    /* 0x30 */ ColorRGB16 yellowOuter1;
    /* 0x36 */ ColorRGB16 yellowOuter2;
    /* 0x3C */ ColorRGB16 blueOuter1;
    /* 0x42 */ ColorRGB16 blueOuter2;
}; // size = 0x48

static void ColorTo16(ColorRGB16* dest, Color src) {
    dest->r = src.r;
    dest->g = src.g;
    dest->b = src.b;
}

u32 HudColors_GetMagicMeterColor(bool inf) {
    u8 alpha = gGlobalContext.interfaceCtx.alphas.magicRupees & 0xFF;
    if (inf) {
        return Color_ConvertToIntWithAlpha(HUD_COLOR_CONFIG.magicInf, alpha);
    } else {
        return Color_ConvertToIntWithAlpha(HUD_COLOR_CONFIG.magicNormal, alpha);
    }
}

u32 HudColors_GetMapColor(void) {
    return Color_ConvertToIntWithAlpha(HUD_COLOR_CONFIG.map, 0xA0);
}

u32 HudColors_GetMapPlayerCursorColor(void) {
    u8 alpha = gGlobalContext.interfaceCtx.alphas.minimap & 0xFF;
    return Color_ConvertToIntWithAlpha(HUD_COLOR_CONFIG.mapPlayerCursor, alpha);
}

u32 HudColors_GetMapEntranceCursorColor(void) {
    u8 alpha = gGlobalContext.interfaceCtx.alphas.minimap & 0xFF;
    return Color_ConvertToIntWithAlpha(HUD_COLOR_CONFIG.mapEntranceCursor, alpha);
}

u32 HudColors_GetClockEmblemColor(void) {
    u8 alpha = (u8)(*(u16*)(0x801BFB2C));
    return Color_ConvertToIntWithAlpha(HUD_COLOR_CONFIG.clockEmblem, alpha);
}

u16 HudColors_GetClockEmblemInvertedColor(u8 idx) {
    Color colors;
    s16 mode = *(s16*)0x801BFBE8;

    if (idx > 2) {
        return 0;
    }

    // Mode should be either 0 (first color) or 1 (second color)
    if (!mode) {
        colors = HUD_COLOR_CONFIG.clockEmblemInverted1;
    } else {
        colors = HUD_COLOR_CONFIG.clockEmblemInverted2;
    }

    return colors.bytes[idx];
}

u32 HudColors_GetClockEmblemSunColor(u16 alpha) {
    return Color_ConvertToIntWithAlpha(HUD_COLOR_CONFIG.clockEmblemSun, alpha & 0xFF);
}

u32 HudColors_GetClockSunColor(void) {
    u8 alpha = (*(u16*)0x801BFB2C) & 0xFF;
    return Color_ConvertToIntWithAlpha(HUD_COLOR_CONFIG.clockSun, alpha);
}

u32 HudColors_GetClockMoonColor(void) {
    u8 alpha = (*(u16*)0x801BFB2C) & 0xFF;
    return Color_ConvertToIntWithAlpha(HUD_COLOR_CONFIG.clockMoon, alpha);
}

u32 HudColors_GetAButtonColor(u8 alpha) {
    return Color_ConvertToIntWithAlpha(HUD_COLOR_CONFIG.buttonA, alpha);
}

u32 HudColors_GetBButtonColor(void) {
    // Alpha won't be used but set it anyway
    u8 alpha = gGlobalContext.interfaceCtx.alphas.buttonB & 0xFF;
    return Color_ConvertToIntWithAlpha(HUD_COLOR_CONFIG.buttonB, alpha);
}

u32 HudColors_GetCButtonColor(u16 alpha) {
    return Color_ConvertToIntWithAlpha(HUD_COLOR_CONFIG.buttonC, alpha & 0xFF);
}

u32 HudColors_GetStartButtonColor(u16 alpha) {
    return Color_ConvertToIntWithAlpha(HUD_COLOR_CONFIG.buttonStart, alpha & 0xFF);
}

/**
 * Hook function used to get the "A" or "C" button color when used as a song note icon.
 **/
u32 HudColors_GetNoteButtonColor(u8 index, u8 alpha) {
    if (index == 0) {
        return Color_ConvertToIntWithAlpha(HUD_COLOR_CONFIG.noteA1, alpha);
    } else {
        return Color_ConvertToIntWithAlpha(HUD_COLOR_CONFIG.noteC1, alpha);
    }
}

/**
 * Hook function used to get the pause menu primary border color.
 **/
u32 HudColors_GetMenuBorder1Color(void) {
    return Color_ConvertToIntWithAlpha(HUD_COLOR_CONFIG.menuBorder1, 0xFF);
}

/**
 * Hook function used to get the pause menu secondary border color.
 **/
u32 HudColors_GetMenuBorder2Color(void) {
    return Color_ConvertToIntWithAlpha(HUD_COLOR_CONFIG.menuBorder2, 0xFF);
}

/**
 * Hook function used to get the pause menu subtitle text color.
 **/
u32 HudColors_GetMenuSubtitleTextColor(void) {
    return Color_ConvertToIntWithAlpha(HUD_COLOR_CONFIG.menuSubtitleText, 0xFF);
}

void HudColors_UpdateHeartColors(GlobalContext* ctxt) {
    // Normal heart colors
    ColorRRGGBB16* heart = &(gGlobalContext.interfaceCtx.heartInnerColor);
    ColorRGB16* heartBeating = &(gGlobalContext.interfaceCtx.heartbeatInnerColor);

    // Double defense heart colors
    ColorRGB16* heartDd = &(gSaveContext.heartDdRgb);
    ColorRGB16* heartDdBeating = &(gSaveContext.heartDdBeatingRgb);

    // This function writes constant values to where the heart colors are stored.
    // It might also do other things.
    z2_WriteHeartColors(ctxt);

    // Update heart colors (normal)
    heart->r1 = HUD_COLOR_CONFIG.heart.r;
    heart->g1 = HUD_COLOR_CONFIG.heart.g;
    heart->b1 = HUD_COLOR_CONFIG.heart.b;
    heartBeating->r = HUD_COLOR_CONFIG.heart.r;
    heartBeating->g = HUD_COLOR_CONFIG.heart.g;
    heartBeating->b = HUD_COLOR_CONFIG.heart.b;

    // Update heart colors (double defense)
    heartDd->r = HUD_COLOR_CONFIG.heartDd.r;
    heartDd->g = HUD_COLOR_CONFIG.heartDd.g;
    heartDd->b = HUD_COLOR_CONFIG.heartDd.b;
    heartDdBeating->r = HUD_COLOR_CONFIG.heartDd.r;
    heartDdBeating->g = HUD_COLOR_CONFIG.heartDd.g;
    heartDdBeating->b = HUD_COLOR_CONFIG.heartDd.b;
}

/**
 * Hook function which writes button note colors to RDRAM.
 *
 * Calls original function which writes the colors, then overwrites certain colors with our own
 * (A & C button note colors when used in a song).
 **/
void HudColors_UpdateButtonNoteColors(GlobalContext* ctxt) {
    // Call original function.
    z2_InitButtonNoteColors(ctxt);

    // Write "A" button colors (order in RDRAM is RBG instead of RGB).
    s16* aNoteColors = (s16*)0x801F6B0C;
    aNoteColors[0] = HUD_COLOR_CONFIG.noteA1.r;
    aNoteColors[1] = HUD_COLOR_CONFIG.noteA1.b;
    aNoteColors[2] = HUD_COLOR_CONFIG.noteA1.g;
    aNoteColors[3] = HUD_COLOR_CONFIG.noteAShadow1.r;
    aNoteColors[4] = HUD_COLOR_CONFIG.noteAShadow1.g;
    aNoteColors[5] = HUD_COLOR_CONFIG.noteAShadow1.b;

    // Write "C" button colors, same ordering as above.
    s16* cNoteColors = (s16*)0x801F6B18;
    cNoteColors[0] = HUD_COLOR_CONFIG.noteC1.r;
    cNoteColors[1] = HUD_COLOR_CONFIG.noteC1.b;
    cNoteColors[2] = HUD_COLOR_CONFIG.noteC1.g;
    cNoteColors[3] = HUD_COLOR_CONFIG.noteCShadow1.r;
    cNoteColors[4] = HUD_COLOR_CONFIG.noteCShadow1.g;
    cNoteColors[5] = HUD_COLOR_CONFIG.noteCShadow1.b;

    // Write "A" button colors used when animating.
    s16* aNoteBlink = (s16*)0x801D0334;
    aNoteBlink[0] = HUD_COLOR_CONFIG.noteA1.r;
    aNoteBlink[1] = HUD_COLOR_CONFIG.noteA1.g;
    aNoteBlink[2] = HUD_COLOR_CONFIG.noteA1.b;
    aNoteBlink[3] = HUD_COLOR_CONFIG.noteA2.r;
    aNoteBlink[4] = HUD_COLOR_CONFIG.noteA2.g;
    aNoteBlink[5] = HUD_COLOR_CONFIG.noteA2.b;
    aNoteBlink[6] = HUD_COLOR_CONFIG.noteAShadow1.r;
    aNoteBlink[7] = HUD_COLOR_CONFIG.noteAShadow1.g;
    aNoteBlink[8] = HUD_COLOR_CONFIG.noteAShadow1.b;
    aNoteBlink[9] = HUD_COLOR_CONFIG.noteAShadow2.r;
    aNoteBlink[10] = HUD_COLOR_CONFIG.noteAShadow2.g;
    aNoteBlink[11] = HUD_COLOR_CONFIG.noteAShadow2.b;

    // Write "C" button colors used when animating.
    s16* cNoteBlink = (s16*)0x801D034C;
    cNoteBlink[0] = HUD_COLOR_CONFIG.noteC1.r;
    cNoteBlink[1] = HUD_COLOR_CONFIG.noteC1.g;
    cNoteBlink[2] = HUD_COLOR_CONFIG.noteC1.b;
    cNoteBlink[3] = HUD_COLOR_CONFIG.noteC2.r;
    cNoteBlink[4] = HUD_COLOR_CONFIG.noteC2.g;
    cNoteBlink[5] = HUD_COLOR_CONFIG.noteC2.b;
    cNoteBlink[6] = HUD_COLOR_CONFIG.noteCShadow1.r;
    cNoteBlink[7] = HUD_COLOR_CONFIG.noteCShadow1.g;
    cNoteBlink[8] = HUD_COLOR_CONFIG.noteCShadow1.b;
    cNoteBlink[9] = HUD_COLOR_CONFIG.noteCShadow2.r;
    cNoteBlink[10] = HUD_COLOR_CONFIG.noteCShadow2.g;
    cNoteBlink[11] = HUD_COLOR_CONFIG.noteCShadow2.b;
}

/**
 * Helper function to update the prompt colors used for messageboxes.
 **/
static void UpdateMsgboxPromptColors(bool initial) {
    // Update first color used by color cycler.
    ColorRGB16* prompt1 = (ColorRGB16*)0x801CFCD8;
    prompt1->r = HUD_COLOR_CONFIG.prompt1.r;
    prompt1->g = HUD_COLOR_CONFIG.prompt1.g;
    prompt1->b = HUD_COLOR_CONFIG.prompt1.b;

    // Update second color used by color cycler.
    ColorRGB16* prompt2 = (ColorRGB16*)0x801CFCDE;
    prompt2->r = HUD_COLOR_CONFIG.prompt2.r;
    prompt2->g = HUD_COLOR_CONFIG.prompt2.g;
    prompt2->b = HUD_COLOR_CONFIG.prompt2.b;

    // Update glow color.
    ColorRGB16* promptGlow = (ColorRGB16*)0x801CFCEA;
    promptGlow->r = HUD_COLOR_CONFIG.promptGlow.r;
    promptGlow->g = HUD_COLOR_CONFIG.promptGlow.g;
    promptGlow->b = HUD_COLOR_CONFIG.promptGlow.b;

    // Update number cursor colors.
    ColorRGB16* numberCursor = (ColorRGB16*)0x801CFD10;
    ColorTo16(&numberCursor[0], HUD_COLOR_CONFIG.prompt1);
    ColorTo16(&numberCursor[1], HUD_COLOR_CONFIG.prompt2);
    ColorTo16(&numberCursor[3], HUD_COLOR_CONFIG.promptGlow);

    // Update initial values of color cycler.
    if (initial) {
        // Initial color of message prompt icon when cycled.
        s16* promptInit = (s16*)0x801CFCF0;
        promptInit[0] = HUD_COLOR_CONFIG.prompt1.r;
        promptInit[2] = HUD_COLOR_CONFIG.prompt1.g;
        promptInit[4] = HUD_COLOR_CONFIG.prompt1.b;

        // Initial color of number cursor when cycled.
        s16* numberCursorInit = (s16*)0x801CFD28;
        numberCursorInit[0] = HUD_COLOR_CONFIG.prompt1.r;
        numberCursorInit[2] = HUD_COLOR_CONFIG.prompt1.g;
        numberCursorInit[4] = HUD_COLOR_CONFIG.prompt1.b;
    }
}

/**
 * Helper function for updating the pause menu colors.
 **/
void HudColors_UpdatePauseMenuColors(GlobalContext* ctxt) {
    // Only try to update colors if kaleido_scope is loaded.
    if (s801D0B70.kaleidoScope.loadedRamAddr != NULL) {
        // Resolve address of colors in kaleido_scope (pause) data.
        u32 vram = 0x808160A0 + 0x158A8;
        void* addr = Reloc_ResolvePlayerOverlay(&s801D0B70.kaleidoScope, vram);
        if (addr != NULL) {
            // Update pause menu cursor icon colors.
            struct PauseCursorColors* colors = (struct PauseCursorColors*)addr;
            ColorTo16(&colors->blueInner1, HUD_COLOR_CONFIG.menuAInner1);
            ColorTo16(&colors->blueInner2, HUD_COLOR_CONFIG.menuAInner2);
            ColorTo16(&colors->blueOuter1, HUD_COLOR_CONFIG.menuAOuter1);
            ColorTo16(&colors->blueOuter2, HUD_COLOR_CONFIG.menuAOuter2);
            ColorTo16(&colors->yellowInner1, HUD_COLOR_CONFIG.menuCInner1);
            ColorTo16(&colors->yellowInner2, HUD_COLOR_CONFIG.menuCInner2);
            ColorTo16(&colors->yellowOuter1, HUD_COLOR_CONFIG.menuCOuter1);
            ColorTo16(&colors->yellowOuter2, HUD_COLOR_CONFIG.menuCOuter2);
        }

        u8* bgDList = (u8*)ctxt->pauseCtx.bgDList;
        if (bgDList != NULL) {
            // Update pause menu subtitle icon colors.
            ColorRGBA8* subtitleC = (ColorRGBA8*)(bgDList + 0x13C);
            ColorRGBA8* subtitleA = (ColorRGBA8*)(bgDList + 0x194);
            subtitleA->r = HUD_COLOR_CONFIG.pauseTitleA.r;
            subtitleA->g = HUD_COLOR_CONFIG.pauseTitleA.g;
            subtitleA->b = HUD_COLOR_CONFIG.pauseTitleA.b;
            subtitleC->r = HUD_COLOR_CONFIG.pauseTitleC.r;
            subtitleC->g = HUD_COLOR_CONFIG.pauseTitleC.g;
            subtitleC->b = HUD_COLOR_CONFIG.pauseTitleC.b;
        }
    }
}

/**
 * Helper function for updating the colors of button icons in text.
 **/
static void UpdateTextButtonIconColors(void) {
    ColorRGB16* iconA = (ColorRGB16*)0x801D0848;
    ColorRGB16* iconB = (ColorRGB16*)0x801D0842;
    ColorRGB16* iconC = (ColorRGB16*)0x801D084E;
    ColorTo16(iconA, HUD_COLOR_CONFIG.buttonIconA);
    ColorTo16(iconB, HUD_COLOR_CONFIG.buttonIconB);
    ColorTo16(iconC, HUD_COLOR_CONFIG.buttonIconC);
}

static void UpdateRupeeColors(u16* rupeeColors) {
    // Write standard rupee colors.
    for (int i = 0; i < 3; i++) {
        int idx = i * 3;
        rupeeColors[idx] = HUD_COLOR_CONFIG.rupee[i].r;
        rupeeColors[idx + 1] = HUD_COLOR_CONFIG.rupee[i].g;
        rupeeColors[idx + 2] = HUD_COLOR_CONFIG.rupee[i].b;
    }
    // Write royal wallet rupee color.
    u16* const royalColor = rupeeColors + 9;
    royalColor[0] = HUD_COLOR_CONFIG.royalRupee.r;
    royalColor[1] = HUD_COLOR_CONFIG.royalRupee.g;
    royalColor[2] = HUD_COLOR_CONFIG.royalRupee.b;
}

void HudColors_Init(void) {
    u16* rupeeColors = (u16*)0x801BFD2C;
    // The rupee colors never seem to get modified, so just update them once.
    UpdateRupeeColors(rupeeColors);
    // Update the message prompt colors.
    UpdateMsgboxPromptColors(true);
    // Update the text button icon colors.
    UpdateTextButtonIconColors();
}

void HudColors_FileChooseInit(void) {
    FileChooseData* data = Reloc_ResolveFileChooseData();
    // Update rupee colors.
    UpdateRupeeColors(data->rupeeColors);
    // Update hearts colors.
    data->lifeColor[0].r = HUD_COLOR_CONFIG.heart.r;
    data->lifeColor[0].g = HUD_COLOR_CONFIG.heart.g;
    data->lifeColor[0].b = HUD_COLOR_CONFIG.heart.b;
    data->lifeColor[1].r = HUD_COLOR_CONFIG.heartDd.r;
    data->lifeColor[1].g = HUD_COLOR_CONFIG.heartDd.g;
    data->lifeColor[1].b = HUD_COLOR_CONFIG.heartDd.b;
}

/**
 * Hook function called to write song score lines color to RDRAM.
 **/
void HudColors_UpdateScoreLinesColor(GlobalContext* ctxt) {
    // Update song score lines color.
    ctxt->msgCtx.scoreLineColor.r = HUD_COLOR_CONFIG.scoreLines.r;
    ctxt->msgCtx.scoreLineColor.g = HUD_COLOR_CONFIG.scoreLines.g;
    ctxt->msgCtx.scoreLineColor.b = HUD_COLOR_CONFIG.scoreLines.b;
}

/**
 * Hook function called to write shop cursor color values to an output array.
 **/
void HudColors_WriteShopCursorColor(Actor* actor, u32* output, u32 amountBits, u32 shopType) {
    // Hack to have f32 argument without being weird?
    f32 amount = *(f32*)&amountBits;
    // Build array of RGB values.
    u32 colors1[3], colors2[3];
    for (int i = 0; i < 3; i++) {
        colors1[i] = HUD_COLOR_CONFIG.shopCursor1.bytes[i];
        colors2[i] = HUD_COLOR_CONFIG.shopCursor2.bytes[i];
    }
    // Calculate individual color values for RGB.
    for (int i = 0; i < 3; i++) {
        u32 c1 = colors1[i], c2 = colors2[i];
        if (c1 <= c2) {
            f32 delta = (f32)(c2 - c1) * amount;
            output[i] = (u32)delta + c1;
        } else {
            f32 delta = (f32)(c1 - c2) * amount;
            output[i] = (u32)delta + c2;
        }
    }
    // Use constant alpha.
    output[3] = 0xFF;
}
