;==================================================================================================
; Draw C Button Amounts Fix
;==================================================================================================

.headersize G_CODE_DELTA

; Fixes B & C button item amounts and text being drawn in green if magic has not yet been obtained.

; Fix for B button icon.
; Replaces:
;   jal     0x80118084
.org 0x80120214
    jal     Hacks_DrawBButtonIconColorFix

; Fix for C button icons.
; Replaces:
;   jal     0x80118890
.org 0x8012021C
    jal     Hacks_DrawCButtonIconsColorFix

;==================================================================================================
; Royal Wallet Fixes
;==================================================================================================

; Draw 3 digits for royal wallet rupee count.
; Replaces:
;   .dh 0x0000
.org 0x801BFD2A
    .dh 0x0003

; Allow 999 rupees for royal wallet capacity.
; Replaces:
;   .dh 0x01F4
.org 0x801C1E32
    .dh 0x03E7
