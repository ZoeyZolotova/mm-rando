#include <z64.h>

// Other Pictobox related flags:
// - s16 0x801BF884: State flag: 0 = None, 1 = Camera Mode, 2 = Processing Photo, 3 = Prompt

enum PictoFlags {
    PICTOBOX_SWAMP = 1 << 1,
    PICTOBOX_MONKEY = 1 << 2,
    PICTOBOX_BIG_OCTO = 1 << 3,
    PICTOBOX_LULU1 = 1 << 4,
    PICTOBOX_LULU2 = 1 << 5,
    PICTOBOX_LULU3 = 1 << 6,
    PICTOBOX_SCARECROW = 1 << 7,
    PICTOBOX_TINGLE = 1 << 8,
    PICTOBOX_PIRATE_GOOD = 1 << 9,
    PICTOBOX_DEKU_KING = 1 << 10,
    PICTOBOX_PIRATE_BAD = 1 << 11,
};

/**
 * Helper function to get the current text for the Pictobox prompt.
 **/
const char* Pictobox_CurrentText(void) {
    static const u32 lulu = PICTOBOX_LULU1 | PICTOBOX_LULU2 | PICTOBOX_LULU3;
    u32 cur = gSaveContext.perm.pictoFlags0;
    if ((cur & PICTOBOX_MONKEY) != 0) {
        return "picture of a monkey";
    } else if ((cur & PICTOBOX_BIG_OCTO) != 0) {
        return "picture of a big octo";
    } else if ((cur & PICTOBOX_DEKU_KING) != 0) {
        return "picture of the Deku King";
    } else if ((cur & lulu) == lulu) {
        return "good picture of Lulu";
    } else if ((cur & PICTOBOX_LULU1) != 0) {
        return "bad picture of Lulu";
    } else if ((cur & PICTOBOX_SCARECROW) != 0) {
        return "picture of a scarecrow";
    } else if ((cur & PICTOBOX_TINGLE) != 0) {
        return "picture of Tingle";
    } else if ((cur & PICTOBOX_PIRATE_GOOD) != 0) {
        return "good picture of a pirate";
    } else if ((cur & PICTOBOX_PIRATE_BAD) != 0) {
        return "bad picture of a pirate";
    } else if ((cur & PICTOBOX_SWAMP) != 0) {
        return "picture of the swamp";
    } else {
        return "picture";
    }
}

/**
 * Hook function to prepare the pictograph message which prompts the player about keeping the picture.
 **/
void Pictobox_PreparePromptMessage(GlobalContext* ctxt, u16 messageId, u8 zero) {
    // Note: This is used to display the message both after taking the picture, and when viewing a previous picture.
    // If photo was taken, update flags before determining prompt message.
    s16 photoTakenFlag = *(s16*)(0x801BF888);
    if (photoTakenFlag == 1) {
        z2_UpdatePictoFlags(ctxt);
    }
    z2_ShowMessage(ctxt, messageId, (Actor*)((u32)zero));
}
