;==================================================================================================
; Spawn green rupee in loop
;==================================================================================================

.headersize G_OBJ_MURE3_DELTA

; Replaces:
;   jal     0x800A7AD4
.org 0x8098F2C8
    jal     RupeeCluster_SpawnRupee_Hook

;==================================================================================================
; Spawn red rupee
;==================================================================================================

; Replaces:
;   jal     0x800A7AD4
.org 0x8098F320
    jal     RupeeCluster_SpawnRupee_Hook
