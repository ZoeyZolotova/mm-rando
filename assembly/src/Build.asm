.n64
.relativeinclude on

;; Force armips version 0.11 or later for fix to MIPS LO/HI ELF symbol relocation.
.if (version() < 110)
.notice version()
.error "Detected armips build is too old. Please install https://github.com/Kingcom/armips version 0.11 or later."
.endif

.create "../roms/patched.z64", 0
.incbin "../roms/base.z64"

;==================================================================================================
; Constants
;==================================================================================================

.include "Constants.asm"
.include "dmadata.asm"
.include "Symbols.asm"

;==================================================================================================
; RAM translation for "code" (file 31)
;==================================================================================================

.headersize G_CODE_DELTA // ROM != VROM for all tools

;==================================================================================================
; Base game editing region
;==================================================================================================

.include "Boot.asm"
.include "Hacks.asm"
.include "Actor.Patch.asm"
.include "ArrowMagic.Patch.asm"
.include "BankAmount.Patch.asm"
.include "BusinessScrub.Patch.asm"
.include "Chest.Patch.asm"
.include "Cows.Patch.asm"
.include "Dampe.Patch.asm"
.include "DekuScrubPlaygroundElevator.Patch.asm"
.include "DekuScrubPlaygroundRupee.Patch.asm"
.include "DonGero.Patch.asm"
.include "DoorWarp.Patch.asm"
.include "Dpad.Patch.asm"
.include "ElegySpeedup.Patch.asm"
.include "ExtendedObjects.Patch.asm"
.include "ExternalEffects.Patch.asm"
.include "FileSelect.Patch.asm"
.include "FloorPhysics.Patch.asm"
.include "Game.Patch.asm"
.include "GinkoMan.Patch.asm"
.include "HitTag.Patch.asm"
.include "HudColors.Patch.asm"
.include "Input.Patch.asm"
.include "InvisibleRupee.Patch.asm"
.include "Item00.Patch.asm"
.include "Items.Patch.asm"
.include "KeatonGrassCluster.Patch.asm"
.include "Knight.Patch.asm"
.include "Message.Patch.asm"
.include "MessageTable.Patch.asm"
.include "Mikau.Patch.asm"
.include "Misc.Patch.asm"
.include "MMR.Patch.asm"
.include "Models.Patch.asm"
.include "Music.Patch.asm"
.include "MusicStaff.Patch.asm"
.include "Pause.Patch.asm"
.include "Pictobox.Patch.asm"
.include "PlayerActor.Patch.asm"
.include "Pushblock.Patch.asm"
.include "QuestItems.Patch.asm"
.include "Room.Patch.asm"
.include "RupeeCluster.Patch.asm"
.include "Rupeecrow.Patch.asm"
.include "Savedata.Patch.asm"
.include "Scarecrow.Patch.asm"
.include "Scene.Patch.asm"
.include "Scopecoin.Patch.asm"
.include "ScRuppe.Patch.asm"
.include "Shops.Patch.asm"
.include "SoftSoilPrize.Patch.asm"
.include "SongState.Patch.asm"
.include "Speedups.Patch.asm"
.include "SpinAttackEffect.Patch.asm"
.include "StrayFairyGroup.Patch.asm"
.include "SwordSchoolGong.Patch.asm"
.include "SyatekiMan.Patch.asm"
.include "TargetHealth.Patch.asm"
.include "Thiefbird.Patch.asm"
.include "WorldColors.Patch.asm"
.include "ZoraLand.Patch.asm"

;==================================================================================================
; New code region
;==================================================================================================

.headersize (G_PAYLOAD_ADDR - G_PAYLOAD_VROM)

.org G_PAYLOAD_ADDR
.area (G_PAYLOAD_SIZE - G_C_HEAP_SIZE) // Payload max memory
PAYLOAD_START:

.include "Init.asm"
.include "Actor.asm"
.include "ArrowMagic.asm"
.include "BankAmount.asm"
.include "BusinessScrub.asm"
.include "Chest.asm"
.include "Cows.asm"
.include "Dampe.asm"
.include "DekuScrubPlaygroundElevator.asm"
.include "DekuScrubPlaygroundRupee.asm"
.include "DoorWarp.asm"
.include "Dpad.asm"
.include "ElegySpeedup.asm"
.include "ExtendedObjects.asm"
.include "FileSelect.asm"
.include "Game.asm"
.include "HudColors.asm"
.include "Input.asm"
.include "Item00.asm"
.include "Items.asm"
.include "Knight.asm"
.include "MessageTable.asm"
.include "Mikau.asm"
.include "Misc.asm"
.include "Models.asm"
.include "Music.asm"
.include "MusicStaff.asm"
.include "Pause.asm"
.include "PlayerActor.asm"
.include "Pushblock.asm"
.include "QuestItems.asm"
.include "Room.asm"
.include "RupeeCluster.asm"
.include "Rupeecrow.asm"
.include "Savedata.asm"
.include "Scarecrow.asm"
.include "ScRuppe.asm"
.include "Speedups.asm"
.include "SpinAttackEffect.asm"
.include "StrayFairyGroup.asm"
.include "SyatekiMan.asm"
.include "WorldColors.asm"
.include "ZoraLand.asm"
.importobj "../build/bundle.o"

.align 8
DPAD_TEXTURE:
.incbin "../resources/dpad32.bin"
FONT_TEXTURE:
.incbin "../resources/font.bin"

.align 0x10
PAYLOAD_END:
.endarea // Payload max memory

.close
