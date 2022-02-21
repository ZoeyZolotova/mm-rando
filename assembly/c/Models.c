#include <stdbool.h>
#include <z64.h>
#include "BaseRupee.h"
#include "Scopecoin.h"
#include "Game.h"
#include "Items.h"
#include "ItemOverride.h"
#include "LoadedModels.h"
#include "Misc.h"
#include "MMR.h"
#include "Models.h"
#include "Objheap.h"
#include "Player.h"
#include "Util.h"

#define OBJHEAP_SLOTS (24)
#define OBJHEAP_SIZE  (0x20000)

struct ObjheapItem gObjheapItems[OBJHEAP_SLOTS] = { 0 };
struct Objheap gObjheap = { 0 };

static SceneObject* FindObject(GlobalContext* ctxt, s16 objectId) {
    for (int i = 0; i < 35; i++) {
        if (ctxt->sceneContext.objects[i].id == objectId) {
            return &ctxt->sceneContext.objects[i];
        }
    }
    return NULL;
}

static void ScaleTopMatrix(f32 scaleFactor) {
    f32* matrix = z2_GetMatrixStackTop();
    for (int i = 0; i < 3; i++) {
        for (int j = 0; j < 3; j++) {
            matrix[4*i + j] *= scaleFactor;
        }
    }
}

static void SetObjectSegment(GlobalContext* ctxt, const void* buf) {
    // Write to polyXlu.
    DispBuf* xlu = &ctxt->state.gfxCtx->polyXlu;
    gSPSegment(xlu->p++, 6, (u32)buf);
    // Write to polyOpa.
    DispBuf* opa = &ctxt->state.gfxCtx->polyOpa;
    gSPSegment(opa->p++, 6, (u32)buf);
}

static void DrawModelLowLevel(Actor* actor, GlobalContext* ctxt, s8 graphicIdMinus1) {
    z2_PreDraw1(actor, ctxt, 0);
    z2_PreDraw2(actor, ctxt, 0);
    z2_BaseDrawGiModel(ctxt, graphicIdMinus1);
}

static void DrawModel(struct Model model, Actor* actor, GlobalContext* ctxt, f32 baseScale) {
    // If both graphic & object are 0, draw nothing.
    if (model.graphicId == 0 && model.objectId == 0) {
        return;
    }

    struct ObjheapItem* object = Objheap_Allocate(&gObjheap, model.objectId);
    if (object) {
        // Update RDRAM segment table with object pointer during the draw function.
        // This is required by Moon's Tear (and possibly others), which programatically resolves a
        // segmented address using that table when writing instructions to the display list.
        gRspSegmentPhysAddrs.currentObject = (u32)object->buf & 0xFFFFFF;
        // Scale matrix and call low-level draw functions.
        SetObjectSegment(ctxt, object->buf);
        ScaleTopMatrix(baseScale);
        DrawModelLowLevel(actor, ctxt, model.graphicId - 1);
    }
}

/**
 * "Fix" the graphic Id used in the Get-Item table.
 **/
static u8 FixGraphicId(u8 graphic) {
    if (graphic >= 0x80) {
        return (u8)(0x100 - (u16)graphic);
    } else {
        return graphic;
    }
}

/**
 * Get the Get-Item table entry for a specific index, and optionally load relevant entry values
 * into a model structure for drawing.
 **/
static GetItemEntry* PrepareGiEntry(struct Model* model, GlobalContext* ctxt, u16 giIndex, bool resolve) {
    if (resolve) {
        giIndex = MMR_GetNewGiIndex(ctxt, 0, giIndex, false);
    }
    GetItemEntry* entry = MMR_GetGiEntry(giIndex);

    if (model != NULL) {
        u16 gfx, obj;
        if (ItemOverride_GetGraphic(giIndex, &gfx, &obj)) {
            model->graphicId = FixGraphicId((u8)gfx);
            model->objectId = obj;
        } else {
            u8 graphic = FixGraphicId(entry->graphic);
            model->objectId = entry->object;
            model->graphicId = graphic;
        }
    }

    return entry;
}

/**
 * Load information from the Get-Item table using an index and draw the corresponding model.
 **/
static void DrawFromGiTable(Actor* actor, GlobalContext* ctxt, f32 scale, u16 giIndex) {
    struct Model model;
    GetItemEntry* entry = PrepareGiEntry(&model, ctxt, giIndex, true);
    z2_CallSetupDList(gGlobalContext.state.gfxCtx);
    DrawModel(model, actor, ctxt, scale);
}

/**
 * Load the actor model information for later reference if not already stored, and return in model
 * parameter.
 **/
static bool SetLoadedActorModel(struct Model* model, Actor* actor, GlobalContext* ctxt, u16 giIndex) {
    if (!LoadedModels_GetActorModel(model, NULL, actor)) {
        GetItemEntry* entry = PrepareGiEntry(model, ctxt, giIndex, true);
        LoadedModels_AddActorModel(*model, entry, actor);
        return true;
    } else {
        return false;
    }
}

/**
 * Cause model to "float" using rotation value.
 **/
static void ApplyHoverFloat(Actor* actor, f32 base, f32 multiplier) {
    f32 rot = z2_Math_Sins(actor->shape.rot.y);
    actor->shape.yDisplacement = (rot * multiplier) + base;
}

/**
 * Check if a model should rotate backwards (trap item).
 **/
static bool ShouldRotateBackwards(GlobalContext* ctxt, u16 giIndex) {
    // Only rotate ice traps backwards if Ice Trap Quirks enabled.
    if (MISC_CONFIG.flags.iceTrapQuirks) {
        struct Model model;
        GetItemEntry* entry = PrepareGiEntry(&model, ctxt, giIndex, true);
        return entry->item == CUSTOM_ITEM_ICE_TRAP;
    } else {
        return false;
    }
}

/**
 * Rotate an actor model by a specific amount.
 **/
static void RotateActor(Actor* actor, GlobalContext* ctxt, u16 giIndex, u16 amount) {
    if (!ShouldRotateBackwards(ctxt, giIndex)) {
        actor->shape.rot.y += amount;
    } else {
        actor->shape.rot.y -= amount;
    }
}

/**
 * Hook function for drawing Heart Piece actors as their new item.
 **/
void Models_DrawHeartPiece(Actor* actor, GlobalContext* ctxt) {
    if (MISC_CONFIG.flags.freestanding) {
        u16 index = actor->params + 0x80;
        DrawFromGiTable(actor, ctxt, 22.0, index);
    } else {
        z2_DrawHeartPiece(actor, ctxt);
    }
}

/**
 * Hook function for drawing En_Item00 actors as their new item.
 **/
bool Models_DrawItem00(ActorEnItem00* actor, GlobalContext* ctxt) {
    if (actor->unkState == 0x23 && Rupee_GetGiIndex(&actor->base) > 0) {
        if (actor->disappearCountdown == 0x0F) {
            return true;
        }
    }

    if ((actor->disappearCountdownCopy & actor->renderFrameMask) != 0) {
        return true;
    }

    if (MISC_CONFIG.flags.freestanding) {
        u16 giIndex = Rupee_GetGiIndex(&actor->base);
        if (giIndex > 0) {
            if (actor->unkState != 0x23) {
                u16 drawGiIndex = MMR_GetNewGiIndex(ctxt, 0, giIndex, false);
                Rupee_SetDrawGiIndex(&actor->base, drawGiIndex);
            }
            u16 giIndexToDraw = Rupee_GetDrawGiIndex(&actor->base);

            // TODO render rupees as rupees?
            struct Model model;
            GetItemEntry* entry = PrepareGiEntry(&model, ctxt, giIndexToDraw, false);

            z2_CallSetupDList(ctxt->state.gfxCtx);
            DrawModel(model, &actor->base, ctxt, 22.0);

            return true;
        }
    }

    return false;
}
/**
 * Hook function for setting Item00 scale during constructor.
 **/
bool Models_Item00_SetActorSize(GlobalContext* ctxt, Actor* actor) {
    if (MISC_CONFIG.flags.freestanding) {
        if (Rupee_GetGiIndex(actor) > 0) {
            // Size set as if this is a Piece of Heart
            return true;
        }
    }

    return false;
}

/**
 * Hook function for rotating En_Item00 actors (Heart Piece).
 **/
void Models_RotateEnItem00(Actor* actor, GlobalContext* ctxt) {
    u16 index = 0;
    if (MISC_CONFIG.flags.freestanding) {
        // MMR Heart Pieces use masked variable 0x1D or greater.
        if ((actor->params & 0xFF) >= 0x1D) {
            index = actor->params + 0x80;
        } else {
            index = Rupee_GetGiIndex(actor);
        }
    }
    if (index > 0) {
        RotateActor(actor, ctxt, index, 0x3C0);
    } else {
        actor->shape.rot.y += 0x3C0;
    }
}

bool Models_ShouldEnItem00Rotate(ActorEnItem00* actor, GlobalContext* ctxt) {
    if (actor->base.params < 3) {
        return true;
    }
    if (actor->base.params == 3 && actor->disappearCountdown < 0) {
        return true;
    }
    if (actor->base.params == 6 || actor->base.params == 7) {
        return true;
    }
    if (actor->base.params >= 0x1D) {
        return true;
    }
    if (MISC_CONFIG.flags.freestanding && Rupee_GetDrawGiIndex(&actor->base) > 0) {
        return true;
    }
    return false;
}

void Models_DrawItem00Shield(GlobalContext* ctxt, s8 graphicIdMinus1) {
    struct ObjheapItem* object = Objheap_Allocate(&gObjheap, OBJECT_GI_SHIELD_2);
    if (object) {
        // Update RDRAM segment table with object pointer during the draw function.
        // This is required by Moon's Tear (and possibly others), which programatically resolves a
        // segmented address using that table when writing instructions to the display list.
        gRspSegmentPhysAddrs.currentObject = (u32)object->buf & 0xFFFFFF;
        SetObjectSegment(ctxt, object->buf);
        z2_BaseDrawGiModel(ctxt, graphicIdMinus1);
    }
}

/**
 * Get the Get-Item index for a Skulltula Token actor.
 **/
static u16 GetSkulltulaTokenGiIndex(Actor* actor, GlobalContext* ctxt) {
    u16 chestFlag = (actor->params & 0xFC) >> 2;
    // Checks if Swamp Spider House scene
    u16 baseIndex = ctxt->sceneNum == 0x27 ? 0x13A : 0x158;
    u16 giIndex = baseIndex + chestFlag;
    return giIndex;
}

/**
 * Hook function for drawing Skulltula Token actors as their new item.
 **/
void Models_DrawSkulltulaToken(Actor* actor, GlobalContext* ctxt) {
    if (MISC_CONFIG.flags.freestanding) {
        u16 giIndex = GetSkulltulaTokenGiIndex(actor, ctxt);
        DrawFromGiTable(actor, ctxt, 1.0, giIndex);
    } else {
        DrawModelLowLevel(actor, ctxt, GRAPHIC_ST_TOKEN - 1);
    }
}

/**
 * Hook function for rotating Skulltula Token actors.
 **/
void Models_RotateSkulltulaToken(Actor* actor, GlobalContext* ctxt) {
    if (MISC_CONFIG.flags.freestanding) {
        u16 giIndex = GetSkulltulaTokenGiIndex(actor, ctxt);
        RotateActor(actor, ctxt, giIndex, 0x38E);
    } else {
        actor->shape.rot.y += 0x38E;
    }
}

/**
 * Check whether or not a model draws a Stray Fairy.
 **/
static bool IsStrayFairyModel(struct Model model) {
    return model.graphicId == 0x4F;
}

/**
 * Get the Get-Item index for a Stray Fairy.
 **/
static u16 GetStrayFairyGiIndex(Actor* actor, GlobalContext* ctxt) {
    if ((actor->params & 0xF) == 3) {
        // Clock Town stray fairy
        return 0x3B;
    } else {
        // Dungeon stray fairies
        u16 curDungeonOffset = *(u16*)0x801F3F38;
        u16 chestFlag = ((actor->params & 0xFE00) >> 9) & 0x1F;
        return 0x16D + (curDungeonOffset * 0x14) + chestFlag;
    }
}

/**
 * Check if a Stray Fairy actor should be drawn as its Get-Item.
 **/
static bool ShouldOverrideStrayFairyDraw(Actor* actor, GlobalContext* ctxt) {
    u16 flag = actor->params & 0xF;

    // Check if a Stray Fairy is in a Great Fairy fountain:
    // 1 is used for Stray Fairies in the Great Fairy fountain.
    // 8 is used for animating Stray Fairies when being given to the fountain.
    // Optionally check Great Fairy fountain scene: 0x26
    return (flag != 1) && (flag != 8);
}

/**
 * Hook function called before Stray Fairy actor's main function.
 **/
void Models_BeforeStrayFairyMain(Actor* actor, GlobalContext* ctxt) {
    // If not a Stray Fairy, rotate like En_Item00 does.
    bool draw = ShouldOverrideStrayFairyDraw(actor, ctxt);
    if (MISC_CONFIG.flags.freestanding && draw) {
        GetItemEntry* entry;
        struct Model model;
        u16 giIndex = GetStrayFairyGiIndex(actor, ctxt);
        SetLoadedActorModel(&model, actor, ctxt, giIndex);
        if (LoadedModels_GetActorModel(&model, (void**)&entry, actor)) {
            // Check that we are not drawing a stray fairy.
            if (!IsStrayFairyModel(model)) {
                // Rotate at the same speed of a Heart Piece actor.
                RotateActor(actor, ctxt, giIndex, 0x3C0);
            }
        }
    }
}

/**
 * Hook function for drawing Stray Fairy actors as their new item.
 *
 * Return true if overriding functionality, false if using original functionality.
 **/
bool Models_DrawStrayFairy(Actor* actor, GlobalContext* ctxt) {
    bool draw = ShouldOverrideStrayFairyDraw(actor, ctxt);
    if (MISC_CONFIG.flags.freestanding && draw) {
        GetItemEntry* entry;
        struct Model model;
        u16 giIndex = GetStrayFairyGiIndex(actor, ctxt);
        SetLoadedActorModel(&model, actor, ctxt, giIndex);
        if (!LoadedModels_GetActorModel(&model, (void**)&entry, actor)) {
            return false;
        }
        // Check if we are drawing a stray fairy.
        if (IsStrayFairyModel(model)) {
            // Update stray fairy actor according to type, and perform original draw.
            ActorEnElforg* elforg = (ActorEnElforg*)actor;
            u8 fairyType = entry->type >> 4;
            elforg->color = fairyType;
            return false;
        } else {
            z2_CallSetupDList(gGlobalContext.state.gfxCtx);
            DrawModel(model, actor, ctxt, 25.0);
            return true;
        }
    } else {
        return false;
    }
}

/**
 * Get the Get-Item index for a Heart Container actor.
 **/
static u16 GetHeartContainerGiIndex(GlobalContext* ctxt) {
    // This is a (somewhat) reimplementation of MMR function at: 0x801DC138
    // The original function returns in A2 and A3 to setup calling a different function.
    if (ctxt->sceneNum == 0x1F) {
        return 0x11A;
    } else if (ctxt->sceneNum == 0x44) {
        return 0x11B;
    } else if (ctxt->sceneNum == 0x5F) {
        return 0x11C;
    } else {
        return 0x11D;
    }
}

/**
 * Hook function for drawing Heart Container actors as their new item.
 *
 * Return true if overriding functionality, false if using original functionality.
 **/
bool Models_DrawHeartContainer(Actor* actor, GlobalContext* ctxt) {
    if (MISC_CONFIG.flags.freestanding) {
        u16 index = GetHeartContainerGiIndex(ctxt);
        DrawFromGiTable(actor, ctxt, 1.0, index);
        return true;
    } else {
        return false;
    }
}

/**
 * Hook function for rotating Heart Container actors.
 **/
void Models_RotateHeartContainer(Actor* actor, GlobalContext* ctxt) {
    if (MISC_CONFIG.flags.freestanding) {
        u16 giIndex = GetHeartContainerGiIndex(ctxt);
        RotateActor(actor, ctxt, giIndex, 0x400);
    } else {
        actor->shape.rot.y += 0x400;
    }
}

/**
 * Hook function for replacing original behaviour of the Get-Item draw function for Boss Remains,
 * which wrote the segmented address instruction (for the object) in the function itself, instead
 * of the caller.
 **/
void Models_WriteBossRemainsObjectSegment(GlobalContext* ctxt, u32 graphicIdMinus1) {
    DispBuf* opa = &ctxt->state.gfxCtx->polyOpa;
    // Get index of object, and use it to get the data pointer
    s8 index = z2_GetObjectIndex(&ctxt->sceneContext, OBJECT_BSMASK);
    // Only write segment instruction if object found in ctxt's object list.
    // Otherwise, load it.
    if (index >= 0) {
        void* data = ctxt->sceneContext.objects[index].vramAddr;
        // Write segmented address instruction
        gSPSegment(opa->p++, 6, (u32)data);
    } else {
        struct ObjheapItem* object = Objheap_Allocate(&gObjheap, OBJECT_BSMASK);
        if (object) {
            // Scale matrix and call low-level draw functions.
            SetObjectSegment(ctxt, object->buf);
            //ScaleTopMatrix(baseScale);
        }
    }
}

/**
 * Hook function for drawing Boss Remain actors as their new item.
 **/
void Models_DrawBossRemains(Actor* actor, GlobalContext* ctxt, u32 graphicIdMinus1) {
    if (MISC_CONFIG.flags.freestanding) {
        //DrawFromGiTable(actor, ctxt, 1.0, 0x448 + actor->params);

        u16 giIndex = 0x448 + actor->params;
        if (actor->parent->parent == NULL || actor->parent->parent->id != 0) {
            u16 drawGiIndex = MMR_GetNewGiIndex(ctxt, 0, giIndex, false);
            Rupee_SetDrawGiIndex(actor, drawGiIndex);
        }
        u16 giIndexToDraw = Rupee_GetDrawGiIndex(actor);

        struct Model model;
        GetItemEntry* entry = PrepareGiEntry(&model, ctxt, giIndexToDraw, false);

        z2_CallSetupDList(ctxt->state.gfxCtx);
        DrawModel(model, actor, ctxt, 1.0);
    } else {
        DrawModelLowLevel(actor, ctxt, graphicIdMinus1);
    }
}

/**
 * Check whether or not a model draws a Moon's Tear.
 **/
static bool IsMoonsTearModel(struct Model model) {
    return model.graphicId == 0x5A && model.objectId == 0x1B1;
}

/**
 * Check if a Moon's Tear actor should be drawn as its Get-Item.
 **/
static bool ShouldOverrideMoonsTearDraw(Actor* actor, GlobalContext* ctxt) {
    // Check if a vanilla Moon's Tear is being drawn.
    struct Model model;
    GetItemEntry* entry = PrepareGiEntry(&model, ctxt, 0x96, true);
    return !IsMoonsTearModel(model);
}

/**
 * Hook function called before a Moon's Tear actor's main function.
 **/
void Models_BeforeMoonsTearMain(Actor* actor, GlobalContext* ctxt) {
    if (MISC_CONFIG.flags.freestanding) {
        if (ShouldOverrideMoonsTearDraw(actor, ctxt)) {
            // If the Moon's Tear on display, reposition and rotate.
            if (actor->params == 0) {
                actor->currPosRot.pos.x = 157.0;
                actor->currPosRot.pos.y = -32.0;
                actor->currPosRot.pos.z = -103.0;
                RotateActor(actor, ctxt, 0x96, 0x3C0);
                ApplyHoverFloat(actor, 30.0, 18.0);
            }
        }
    }
}

/**
 * Hook function for drawing Moon's Tear actor as its new item.
 **/
bool Models_DrawMoonsTear(Actor* actor, GlobalContext* ctxt) {
    if (MISC_CONFIG.flags.freestanding) {
        if (ShouldOverrideMoonsTearDraw(actor, ctxt)) {
            struct Model model;
            bool resolve;
            if (actor->params == 0) {
                // Moon's Tear on display in observatory (not collectible).
                resolve = false;
            } else {
                // Moon's Tear on ground outside observatory (collectible).
                resolve = true;
            }
            GetItemEntry* entry = PrepareGiEntry(&model, ctxt, 0x96, resolve);
            z2_CallSetupDList(gGlobalContext.state.gfxCtx);
            DrawModel(model, actor, ctxt, 1.0);
            return true;
        }
    }
    return false;
}

/**
 * Hook function for drawing Lab Fish Heart Piece actor as its new item.
 **/
bool Models_DrawLabFishHeartPiece(Actor* actor, GlobalContext* ctxt) {
    if (MISC_CONFIG.flags.freestanding) {
        DrawFromGiTable(actor, ctxt, 25.0, 0x112);
        return true;
    } else {
        return false;
    }
}

/**
 * Hook function for rotating Lab Fish Heart Piece actor.
 **/
void Models_RotateLabFishHeartPiece(Actor* actor, GlobalContext* ctxt) {
    if (MISC_CONFIG.flags.freestanding) {
        RotateActor(actor, ctxt, 0x112, 0x3E8);
    } else {
        actor->shape.rot.y += 0x3E8;
    }
}

/**
 * Check whether or not a model draws a Seahorse.
 **/
static bool IsSeahorseModel(struct Model model) {
    return model.graphicId == 0x63 && model.objectId == 0x1F0;
}

/**
 * Check if a Seahorse actor should be drawn as its Get-Item.
 **/
static bool ShouldOverrideSeahorseDraw(Actor* actor, GlobalContext* ctxt) {
    // Check if a vanilla Seahorse is being drawn.
    struct Model model;
    GetItemEntry* entry = PrepareGiEntry(&model, ctxt, 0x95, true);
    // Ensure that only the fishtank Seahorse is being drawn over.
    bool isFishtank = actor->params == 0xFFFF;
    return isFishtank && !IsSeahorseModel(model);
}

/**
 * Hook function called before a Seahorse actor's main function.
 **/
void Models_BeforeSeahorseMain(Actor* actor, GlobalContext* ctxt) {
    if (MISC_CONFIG.flags.freestanding) {
        if (ShouldOverrideSeahorseDraw(actor, ctxt)) {
            RotateActor(actor, ctxt, 0x95, 0x3C0);
            ApplyHoverFloat(actor, -1000.0, 1000.0);
        }
    }
}

/**
 * Hook function for drawing Seahorse actor as its new item.
 **/
bool Models_DrawSeahorse(Actor* actor, GlobalContext* ctxt) {
    if (MISC_CONFIG.flags.freestanding) {
        if (ShouldOverrideSeahorseDraw(actor, ctxt)) {
            DrawFromGiTable(actor, ctxt, 50.0, 0x95);
            return true;
        }
    }

    return false;
}

void Models_DrawShopInventory(ActorEnGirlA* actor, GlobalContext* ctxt, u32 graphicIdMinus1) {
    if (MISC_CONFIG.flags.shopModels) {
        DrawFromGiTable(&actor->base, ctxt, 1.0, actor->giIndex);
    } else {
        DrawModelLowLevel(&actor->base, ctxt, graphicIdMinus1);
    }
}

bool Models_DrawScopecoin(Actor* actor, GlobalContext* ctxt) {
    if (MISC_CONFIG.flags.freestanding) {
        u16 giIndex = Scopecoin_GetGiIndex(actor);
        if (giIndex > 0) {
            DrawFromGiTable(actor, ctxt, 25.0, giIndex);
            return true;
        }
    }

    return false;
}

void Models_RotateScopecoin(Actor* actor, GlobalContext* ctxt) {
    u16 index = 0;
    if (MISC_CONFIG.flags.freestanding) {
        index = Scopecoin_GetGiIndex(actor);
    }
    if (index > 0) {
        RotateActor(actor, ctxt, index, 0x1F4);
    } else {
        actor->shape.rot.y += 0x1F4;
    }
}

bool Models_DrawScRuppe(ActorEnScRuppe* actor, GlobalContext* ctxt) {
    // if receiving item
    if (actor->disappearCountdown == 1 && Rupee_GetGiIndex(&actor->base) > 0) {
        Player_Pause(ctxt);
    }

    if (MISC_CONFIG.flags.freestanding) {
        u16 giIndex = Rupee_GetGiIndex(&actor->base);
        u16 giIndexToDraw = Rupee_GetDrawGiIndex(&actor->base);
        if (giIndex > 0 || giIndexToDraw > 0) {
            // if not receiving item
            if (actor->base.gravity != 0) {
                giIndexToDraw = MMR_GetNewGiIndex(ctxt, 0, giIndex, false);
                Rupee_SetDrawGiIndex(&actor->base, giIndexToDraw);
            }

            // TODO render rupees as rupees?
            struct Model model;
            GetItemEntry* entry = PrepareGiEntry(&model, ctxt, giIndexToDraw, false);

            z2_CallSetupDList(ctxt->state.gfxCtx);
            DrawModel(model, &actor->base, ctxt, 25.0);
            return true;
        }
    }

    return false;
}

void Models_RotateScRuppe(Actor* actor, GlobalContext* ctxt) {
    u16 index = 0;
    if (MISC_CONFIG.flags.freestanding) {
        index = Rupee_GetDrawGiIndex(actor);
    }
    if (index > 0) {
        RotateActor(actor, ctxt, index, 0x1F4);
    } else {
        actor->shape.rot.y += 0x1F4;
    }
}

bool Models_DrawDekuScrubPlaygroundRupee(ActorEnGamelupy* actor, GlobalContext* ctxt) {
    // if receiving item
    if (actor->disappearCountdown == 1 && Rupee_GetGiIndex(&actor->base) > 0) {
        Player_Pause(ctxt);
    }

    if (MISC_CONFIG.flags.freestanding) {
        u16 giIndex = Rupee_GetGiIndex(&actor->base);
        u16 giIndexToDraw = Rupee_GetDrawGiIndex(&actor->base);
        if (giIndex > 0 || giIndexToDraw > 0) {
            // if not receiving item
            if (actor->base.gravity != 0) {
                giIndexToDraw = MMR_GetNewGiIndex(ctxt, 0, giIndex, false);
                Rupee_SetDrawGiIndex(&actor->base, giIndexToDraw);
            }

            // TODO render rupees as rupees?
            struct Model model;
            GetItemEntry* entry = PrepareGiEntry(&model, ctxt, giIndexToDraw, false);

            z2_CallSetupDList(ctxt->state.gfxCtx);
            DrawModel(model, &actor->base, ctxt, 25.0);
            return true;
        }
    }

    return false;
}

void Models_RotateDekuScrubPlaygroundRupee(Actor* actor, GlobalContext* ctxt) {
    u16 index = 0;
    if (MISC_CONFIG.flags.freestanding) {
        index = Rupee_GetDrawGiIndex(actor);
    }
    if (index > 0) {
        RotateActor(actor, ctxt, index, 0x1F4);
    } else {
        actor->shape.rot.y += 0x1F4;
    }
}

void Models_DrawCutsceneItem(GlobalContext* ctxt, Actor* actor, Vec3s* posRot, Vec3s* posRot2, f32 scale, u16 giIndex) {
    // z2_PushMatrixStackCopy();

    Vec3f pos;
    Vec3s rot;

    pos.x = (f32)posRot[0].x;
    pos.y = (f32)posRot[0].y;
    pos.z = (f32)posRot[0].z;
    rot.x = posRot[1].x;
    rot.y = posRot[1].y;
    rot.z = posRot[1].z;
    z2_TransformMatrixStackTop(&pos, &rot);

    if (posRot2) {
        pos.x = (f32)posRot2[0].x;
        pos.y = (f32)posRot2[0].y;
        pos.z = (f32)posRot2[0].z;
        rot.x = posRot2[1].x;
        rot.y = posRot2[1].y;
        rot.z = posRot2[1].z;
        z2_TransformMatrixStackTop(&pos, &rot);
    }

    DrawFromGiTable(actor, ctxt, scale, giIndex);

    // z2_PopMatrixStack();
}

void Models_DrawCutsceneMask(GlobalContext* ctxt, Actor* actor, Vec3s* posRot, u16 giIndex) {
    Vec3s posRot2[2] = {
        {
            .x = 1024,
            .y = -512,
            .z = 0
        },
        {
            .x = 0,
            .y = 0xC000,
            .z = 0x8000
        }
    };

    Models_DrawCutsceneItem(ctxt, actor, posRot, posRot2, 22.0, giIndex);
}

void Models_DrawZoraMask(GlobalContext* ctxt, u32* skeleton, Vec3s* limbDrawTable, bool* overrideLimbDraw, void* postLimbDraw, Actor* actor) {
    if (!MISC_CONFIG.flags.freestanding) {
        z2_SkelAnime_DrawLimb(ctxt, skeleton, limbDrawTable, overrideLimbDraw, postLimbDraw, actor);
        return;
    }

    Models_DrawCutsceneMask(ctxt, actor, limbDrawTable, 0x7A);
}

void Models_DrawGoronMask(GlobalContext* ctxt, u32* skeleton, Vec3s* limbDrawTable, bool* overrideLimbDraw, void* postLimbDraw, Actor* actor) {
    if (!MISC_CONFIG.flags.freestanding) {
        z2_SkelAnime_DrawLimb(ctxt, skeleton, limbDrawTable, overrideLimbDraw, postLimbDraw, actor);
        return;
    }

    Models_DrawCutsceneMask(ctxt, actor, limbDrawTable, 0x79);
}

void Models_DrawGibdoMask(GlobalContext* ctxt, u32* skeleton, Vec3s* limbDrawTable, s32 dListCount, bool* overrideLimbDraw, bool* postLimbDraw, Actor* actor) {
    if (!MISC_CONFIG.flags.freestanding) {
        z2_SkelAnime_DrawLimb2(ctxt, skeleton, limbDrawTable, dListCount, overrideLimbDraw, postLimbDraw, actor);
        return;
    }

    Models_DrawCutsceneMask(ctxt, actor, limbDrawTable, 0x87);
}

void Models_DrawOcarina(GlobalContext* ctxt, u32* skeleton, Vec3s* limbDrawTable, s32 dListCount, bool* overrideLimbDraw, bool* postLimbDraw, void* unkDraw, Actor* actor) {
    if (!MISC_CONFIG.flags.freestanding) {
        z2_SkelAnime_DrawLimb3(ctxt, skeleton, limbDrawTable, dListCount, overrideLimbDraw, postLimbDraw, unkDraw, actor);
        return;
    }

    Vec3s posRot2[2] = {
        {
            .x = -384,
            .y = -384,
            .z = 384
        },
        {
            .x = 0x0000,
            .y = 0x2000,
            .z = 0x4000
        }
    };

    Models_DrawCutsceneItem(ctxt, actor, limbDrawTable, posRot2, 16.0, 0x4C);
}

void Models_DrawOcarinaLimb(GlobalContext* ctxt, Actor* actor) {
    if (!MISC_CONFIG.flags.freestanding) {
        gSPDisplayList(ctxt->state.gfxCtx->polyOpa.p++, 0x0601CAD0);
        return;
    }

    // Store backup of previous 0xDA (Mtx) instruction (for Skull Kid's hand) and overwrite it.
    // Is this safe? Probably not. :)
    Gfx backup = *(ctxt->state.gfxCtx->polyOpa.p-- - 1);

    // Perform underlying draw.
    DrawFromGiTable(actor, ctxt, 25.0, 0x4C);

    // Restore setup DList to that which Skull Kid's actor expects.
    ctxt->state.gfxCtx->polyOpa.p = z2_Gfx_CallSetupDL(ctxt->state.gfxCtx->polyOpa.p, 0x19);
    ctxt->state.gfxCtx->polyXlu.p = z2_Gfx_CallSetupDL(ctxt->state.gfxCtx->polyXlu.p, 0x19);

    // Find Skull Kid object data and restore segmented addresses.
    SceneObject* obj = FindObject(ctxt, OBJECT_STK);
    if (obj != NULL) {
        // Restore object addresses in RDRAM table and DList.
        gRspSegmentPhysAddrs.currentObject = (u32)obj->vramAddr & 0xFFFFFF;
        SetObjectSegment(ctxt, (const void*)obj->vramAddr);
    }

    // Restore matrix pointer for Skull Kid's hand.
    *(ctxt->state.gfxCtx->polyOpa.p++) = backup;
}

void Models_AfterActorDtor(Actor* actor) {
    if (MISC_CONFIG.flags.freestanding) {
        if (actor->id == ACTOR_EN_ELFORG) {
            LoadedModels_RemoveActorModel(actor);
        }
    }
}

/**
 * Reset object heap pointer and clear all loaded object slots.
 **/
void Models_ClearObjectHeap(void) {
    Objheap_Clear(&gObjheap);
}

/**
 * Initialize object heap.
 **/
void Models_Init(void) {
    void* alloc = Util_HeapAlloc(OBJHEAP_SIZE);
    Objheap_Init(&gObjheap, alloc, OBJHEAP_SIZE, gObjheapItems, OBJHEAP_SLOTS);
}

/**
 * Helper function called after preparing game's display buffers for writing (write pointer set to buffer start).
 **/
void Models_AfterPrepareDisplayBuffers(GraphicsContext* gfx) {
    if (Game_IsPlayerActor()) {
        Objheap_NextFrame(&gObjheap);
    }
}
