SyatekiMan_Swamp_DetermineActionFunctionAfterGiveItem_Hook:
    addiu   sp, sp, -0x18
    sw      ra, 0x0010 (sp)

    jal     SyatekiMan_Swamp_DetermineActionFunctionAfterGiveItem
    nop

    ; Store result in T1.
    or      t1, v0, r0

    lw      ra, 0x0010 (sp)
    addiu   sp, sp, 0x18
    lw      a0, 0x0028 (sp)
    jr      ra
    lw      a1, 0x0028 (sp)

SyatekiMan_Swamp_ShouldNotGiveQuiverReward_Hook:
    addiu   sp, sp, -0x20
    sw      a2, 0x0014 (sp)
    sw      v0, 0x0018 (sp)
    sw      ra, 0x0010 (sp)

    jal     SyatekiMan_Swamp_ShouldNotGiveQuiverReward
    nop

    ; Store result in T7.
    or      t7, v0, r0

    ; Displaced code.
    lui     at, 0x42C8

    lw      a2, 0x0014 (sp)
    lw      v0, 0x0018 (sp)
    lw      ra, 0x0010 (sp)
    addiu   sp, sp, 0x20
    lw      a0, 0x0028 (sp)
    jr      ra
    lw      a1, 0x002C (sp)

SyatekiMan_Town_DetermineActionFunctionAfterGiveItem_Hook:
    addiu   sp, sp, -0x18
    sw      ra, 0x0010 (sp)

    jal     SyatekiMan_Town_DetermineActionFunctionAfterGiveItem
    nop

    ; Store result in T1.
    or      t1, v0, r0

    ; Displaced code.
    addiu   at, r0, 0x0407

    lw      ra, 0x0010 (sp)
    addiu   sp, sp, 0x18
    lw      a0, 0x0028 (sp)
    jr      ra
    lw      a1, 0x002C (sp)

SyatekiMan_Town_ShouldNotGiveLesserReward_Hook:
    addiu   sp, sp, -0x18
    sw      v0, 0x0014 (sp)
    sw      ra, 0x0010 (sp)

    jal     SyatekiMan_Town_ShouldNotGiveLesserReward
    or      a1, s1, r0

    ; Store result in AT.
    or      at, v0, r0

    lw      v0, 0x0014 (sp)
    lw      ra, 0x0010 (sp)
    jr      ra
    addiu   sp, sp, 0x18
