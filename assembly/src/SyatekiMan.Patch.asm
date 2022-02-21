;==================================================================================================
; Swamp Archery Double Reward
;==================================================================================================

.headersize G_EN_SYATEKI_MAN_DELTA

; Replaces:
;   lui     v0, 0x801F
;   addiu   v0, v0, 0xF670
;   lbu     v1, 0x0F33 (v0)
;   lui     t1, hi(0x809C7C14)
;   addiu   t1, t1, lo(0x809C7C14)
;   andi    t6, v1, 0x0010
.org 0x809C7AB8 ; Offset: 0x15F8
    jal     SyatekiMan_Swamp_DetermineActionFunctionAfterGiveItem_Hook
    nop
    lui     v0, 0x801F
    addiu   v0, v0, 0xF670
    lbu     v1, 0x0F33 (v0)
    andi    t6, v1, 0x0010

; Replaces:
;   lui     at, 0x42C8
;   andi    t7, t6, 0x0010
.org 0x809C7B4C ; Offset: 0x168C
    jal     SyatekiMan_Swamp_ShouldNotGiveQuiverReward_Hook
    nop

; Patch relocations.
; Replaces:
;   .dw 0x45001604
;   .dw 0x46001608
.org 0x809C9734 ; Offset: 0x3274
    .dw 0
    .dw 0

;==================================================================================================
; Town Archery Double Reward
;==================================================================================================

.headersize G_EN_SYATEKI_MAN_DELTA

; Replaces:
;   lbu     t9, 0x0F33 (a3)
;   or      a0, s1, r0
;   or      a2, s0, r0
;   andi    t0, t9, 0x0020
;   bnez    t0, 0x809C8CAC
;   nop
.org 0x809C8C80 ; Offset: 0x27C0
    jal     SyatekiMan_Town_ShouldNotGiveLesserReward_Hook
    or      a0, s0, r0
    or      a0, s1, r0
    or      a2, s0, r0
    bnez    at, 0x809C8CAC
    addiu   a1, r0, 0x0032

; Replaces:
;   lh      v0, 0x0284 (a0)
;   addiu   at, r0, 0x0407
;   lui     t1, hi(0x809C7EB4)
;   bne     v0, at, 0x809C7D70
;   addiu   t1, t1, lo(0x809C7EB4)
.org 0x809C7D3C ; Offset: 0x187C
    jal     SyatekiMan_Town_DetermineActionFunctionAfterGiveItem_Hook
    nop
    lh      v0, 0x0284 (a0)
    bne     v0, at, 0x809C7D70
    nop

; Patch relocations.
; Replaces:
;   .dw 0x45001884
;   .dw 0x4600188C
.org 0x809C9744 ; Offset: 0x3284
    .dw 0
    .dw 0
