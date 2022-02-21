;==================================================================================================
; Bank Deposit Multi Reward
;==================================================================================================

.headersize G_EN_GINKO_MAN_DELTA

; Hook bank post-award #1 message.
; Replaces:
;   jal     0x801518B0
.org 0x80A65738 ; Offset: 0x1298
    jal     GinkoMan_AfterAward_DisplayText

; Hook bank post-award #2 animation change.
; Replaces:
;   jal     0x800BDC5C ; Actor_ChangeAnimation
.org 0x80A6574C ; Offset: 0x12AC
    jal     GinkoMan_AfterAward_ChangeAnimation

; Hook bank post-award #2 message.
; Replaces:
;   jal     0x801518B0
.org 0x80A6575C ; Offset: 0x12BC
    jal     GinkoMan_AfterAward_DisplayText

; EnGinkoMan_DepositDialogue: Patch jump table for post-award text: 0x47A, 0x47B
; Instead of jumping to end, jump to same code as 0x45A.
; Note: The function should only be reached via 0x47A or 0x47B if EndFinalTextBox (0xBF) char removed.
;
; Replaces:
;   .dw 0x80A64DB0
;   .dw 0x80A64DB0
.org 0x80A65E98
    .dw 0x80A648C4
    .dw 0x80A648C4
