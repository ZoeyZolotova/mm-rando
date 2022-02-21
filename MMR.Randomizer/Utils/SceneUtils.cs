using MMR.Randomizer.Models.Rom;
using System.Collections.Generic;
using System.Linq;
using MMR.Randomizer.Extensions;

namespace MMR.Randomizer.Utils
{

    public class SceneUtils
    {
        const int SCENE_TABLE = 0xC5A1E0;
        const int SCENE_FLAG_MASKS = 0xC5C500;
        public static void ResetSceneFlagMask()
        {
            ReadWriteUtils.WriteToROM(SCENE_FLAG_MASKS, (uint)0);
            ReadWriteUtils.WriteToROM(SCENE_FLAG_MASKS + 0xC, (uint)0);
        }

        public static void UpdateSceneFlagMask(int num)
        {
            int offset = num >> 3;
            if (num >= 0x380) // skip scene 7 (Grottos)
            {
                offset += 0x10;
            }
            // TODO maybe skip Cutscene Map?
            int mod = offset % 16;
            if (mod < 4)
            {
                offset += 8;
            }
            else if (mod < 12)
            {
                offset -= 4;
            }

            int bit = 1 << (num & 7);
            int f = RomUtils.GetFileIndexForWriting(SCENE_FLAG_MASKS);
            int addr = SCENE_FLAG_MASKS - RomData.MMFileList[f].Addr + offset;
            RomData.MMFileList[f].Data[addr] |= (byte)bit;
        }

        public static void ReadSceneTable()
        {
            RomData.SceneList = new List<Scene>();
            int f = RomUtils.GetFileIndexForWriting(SCENE_TABLE);
            int _SceneTable = SCENE_TABLE - RomData.MMFileList[f].Addr;
            int i = 0;
            while (true)
            {
                Scene s = new Scene();
                uint saddr = ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, _SceneTable + i);
                if (saddr > 0x4000000)
                {
                    break;
                }
                if (saddr != 0)
                {
                    s.File = RomUtils.AddrToFile((int)saddr);
                    s.Number = i >> 4;
                    RomData.SceneList.Add(s);
                }
                i += 16;
            }
        }

        public static void GetMaps()
        {
            foreach (var scene in RomData.SceneList)
            {
                int f = scene.File;
                RomUtils.CheckCompressed(f);
                int j = 0;
                while (true)
                {
                    byte cmd = RomData.MMFileList[f].Data[j];
                    if (cmd == 0x04)
                    {
                        byte mapcount = RomData.MMFileList[f].Data[j + 1];
                        int mapsaddr = (int)ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, j + 4) & 0xFFFFFF;
                        for (int k = 0; k < mapcount; k++)
                        {
                            Map m = new Map();
                            m.File = RomUtils.AddrToFile((int)ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, mapsaddr));
                            scene.Maps.Add(m);
                            mapsaddr += 8;
                        }
                        break;
                    }
                    if (cmd == 0x14)
                    {
                        break;
                    }
                    j += 8;
                }
                CheckHeaderForExits(f, 0, scene);
                if (scene.Number == 108) // avoid modifying unused setup in East Clock Town. doesn't seem to actually affect anything in-game, but best not to touch it.
                {
                    scene.Setups.RemoveAt(2);
                }
            }
        }

        public static void GetMapHeaders()
        {
            for (int i = 0; i < RomData.SceneList.Count; i++)
            {
                int maps = RomData.SceneList[i].Maps.Count;
                for (int j = 0; j < maps; j++)
                {
                    int f = RomData.SceneList[i].Maps[j].File;
                    RomUtils.CheckCompressed(f);
                    int k = 0;
                    int setupsaddr = -1;
                    int nextlowest = -1;
                    while (true)
                    {
                        byte cmd = RomData.MMFileList[f].Data[k];
                        if (cmd == 0x18)
                        {
                            setupsaddr = (int)ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, k + 4) & 0xFFFFFF;
                        }
                        else if (cmd == 0x14)
                        {
                            break;
                        }
                        else
                        {
                            if (RomData.MMFileList[f].Data[k + 4] == 0x03)
                            {
                                int p = (int)ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, k + 4) & 0xFFFFFF;
                                if (((p < nextlowest) || (nextlowest == -1)) && ((p > setupsaddr) && (setupsaddr != -1)))
                                {
                                    nextlowest = p;
                                }
                            }
                        }
                        k += 8;
                    }
                    if ((setupsaddr == -1) || (nextlowest == -1))
                    {
                        continue;
                    }
                    for (k = setupsaddr; k < nextlowest; k += 4)
                    {
                        byte s = RomData.MMFileList[f].Data[k];
                        if (s != 0x03)
                        {
                            break;
                        }
                        int p = (int)ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, k) & 0xFFFFFF;
                        Map m = new Map();
                        m.File = f;
                        m.Header = p;
                        RomData.SceneList[i].Maps.Add(m);
                    }
                }
            }
        }

        private static List<Actor> ReadMapActors(byte[] Map, int Addr, int Count)
        {
            List<Actor> Actors = new List<Actor>();
            for (int i = 0; i < Count; i++)
            {
                Actor a = new Actor();
                ushort an = ReadWriteUtils.Arr_ReadU16(Map, Addr + (i * 16));
                a.m = an & 0xF000;
                a.n = an & 0x0FFF;
                a.p.x = (short)ReadWriteUtils.Arr_ReadU16(Map, Addr + (i * 16) + 2);
                a.p.y = (short)ReadWriteUtils.Arr_ReadU16(Map, Addr + (i * 16) + 4);
                a.p.z = (short)ReadWriteUtils.Arr_ReadU16(Map, Addr + (i * 16) + 6);
                a.r.x = (short)ReadWriteUtils.Arr_ReadU16(Map, Addr + (i * 16) + 8);
                a.r.y = (short)ReadWriteUtils.Arr_ReadU16(Map, Addr + (i * 16) + 10);
                a.r.z = (short)ReadWriteUtils.Arr_ReadU16(Map, Addr + (i * 16) + 12);
                a.v = ReadWriteUtils.Arr_ReadU16(Map, Addr + (i * 16) + 14);
                Actors.Add(a);
            }
            return Actors;
        }

        private static List<int> ReadMapObjects(byte[] Map, int Addr, int Count)
        {
            List<int> Objects = new List<int>();
            for (int i = 0; i < Count; i++)
            {
                Objects.Add(ReadWriteUtils.Arr_ReadU16(Map, Addr + (i * 2)));
            }
            return Objects;
        }

        private static void WriteMapActors(byte[] Map, int Addr, List<Actor> Actors)
        {
            for (int i = 0; i < Actors.Count; i++)
            {
                ReadWriteUtils.Arr_WriteU16(Map, Addr + (i * 16), (ushort)(Actors[i].m | Actors[i].n));
                ReadWriteUtils.Arr_WriteU16(Map, Addr + (i * 16) + 2, (ushort)Actors[i].p.x);
                ReadWriteUtils.Arr_WriteU16(Map, Addr + (i * 16) + 4, (ushort)Actors[i].p.y);
                ReadWriteUtils.Arr_WriteU16(Map, Addr + (i * 16) + 6, (ushort)Actors[i].p.z);
                ReadWriteUtils.Arr_WriteU16(Map, Addr + (i * 16) + 8, (ushort)Actors[i].r.x);
                ReadWriteUtils.Arr_WriteU16(Map, Addr + (i * 16) + 10, (ushort)Actors[i].r.y);
                ReadWriteUtils.Arr_WriteU16(Map, Addr + (i * 16) + 12, (ushort)Actors[i].r.z);
                ReadWriteUtils.Arr_WriteU16(Map, Addr + (i * 16) + 14, (ushort)Actors[i].v);
            }
        }

        private static void WriteMapObjects(byte[] Map, int Addr, List<int> Objects)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                ReadWriteUtils.Arr_WriteU16(Map, Addr + (i * 2), (ushort)Objects[i]);
            }
        }

        private static void UpdateMap(Map M)
        {
            WriteMapActors(RomData.MMFileList[M.File].Data, M.ActorAddr, M.Actors);
            WriteMapObjects(RomData.MMFileList[M.File].Data, M.ObjAddr, M.Objects);
        }

        public static void UpdateScene(Scene scene)
        {
            for (int i = 0; i < scene.Maps.Count; i++)
            {
                UpdateMap(scene.Maps[i]);
            }
        }

        public static void GetActors()
        {
            for (int i = 0; i < RomData.SceneList.Count; i++)
            {
                for (int j = 0; j < RomData.SceneList[i].Maps.Count; j++)
                {
                    int f = RomData.SceneList[i].Maps[j].File;
                    RomUtils.CheckCompressed(f);
                    int k = RomData.SceneList[i].Maps[j].Header;
                    while (true)
                    {
                        byte cmd = RomData.MMFileList[f].Data[k];
                        if (cmd == 0x01)
                        {
                            byte ActorCount = RomData.MMFileList[f].Data[k + 1];
                            int ActorAddr = (int)ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, k + 4) & 0xFFFFFF;
                            RomData.SceneList[i].Maps[j].ActorAddr = ActorAddr;
                            RomData.SceneList[i].Maps[j].Actors = ReadMapActors(RomData.MMFileList[f].Data, ActorAddr, ActorCount);
                        }
                        if (cmd == 0x0B)
                        {
                            byte ObjectCount = RomData.MMFileList[f].Data[k + 1];
                            int ObjectAddr = (int)ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, k + 4) & 0xFFFFFF;
                            RomData.SceneList[i].Maps[j].ObjAddr = ObjectAddr;
                            RomData.SceneList[i].Maps[j].Objects = ReadMapObjects(RomData.MMFileList[f].Data, ObjectAddr, ObjectCount);
                        }
                        if (cmd == 0x14)
                        {
                            break;
                        }
                        k += 8;
                    }
                }
            }
        }

        private static void CheckHeaderForExits(int f, int headeraddr, Scene scene)
        {
            int j = headeraddr;
            int setupsaddr = -1;
            int nextlowest = -1;
            byte s;
            var setup = new SceneSetup();
            scene.Setups.Add(setup);
            while (true)
            {
                byte cmd = RomData.MMFileList[f].Data[j];
                if (cmd == 0x13)
                {
                    setup.ExitListAddress = (int)ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, j + 4) & 0xFFFFFF;
                }
                else if (cmd == 0x17)
                {
                    setup.CutsceneListAddress = (int)ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, j + 4) & 0xFFFFFF;
                }
                else if (cmd == 0x18)
                {
                    setupsaddr = (int)ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, j + 4) & 0xFFFFFF;
                }
                else if (cmd == 0x14)
                {
                    break;
                }
                else
                {
                    if (RomData.MMFileList[f].Data[j + 4] == 0x02)
                    {
                        int p = (int)ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, j + 4) & 0xFFFFFF;
                        if (((p < nextlowest) || (nextlowest == -1)) && ((p > setupsaddr) && (setupsaddr != -1)))
                        {
                            nextlowest = p;
                        }
                    }
                }
                j += 8;
            }
            if ((setupsaddr != -1) && nextlowest != -1)
            {
                j = setupsaddr;
                s = RomData.MMFileList[f].Data[j];
                while (s == 0x02)
                {
                    int p = (int)ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, j) & 0xFFFFFF;
                    CheckHeaderForExits(f, p, scene);
                    j += 4;
                    s = RomData.MMFileList[f].Data[j];
                }
            }
        }

        public static void ReenableNightBGMSingle(int SceneFileID, byte NewMusicByte = 0x13)
        {
            // search for the bgm music header in the scene headers and replace the night sfx with a value that plays day BGM
            RomUtils.CheckCompressed(SceneFileID);
            for (int Byte = 0; Byte < 0x10 * 70; Byte += 8)
            {
                if (RomData.MMFileList[SceneFileID].Data[Byte] == 0x15) // header command starts with 0x15
                {
                    RomData.MMFileList[SceneFileID].Data[Byte + 0x6] = NewMusicByte; // 6th/8 byte is night BGM behavior, 0x13 is daytime BGM
                    return;
                }
            }
        }

        public static void ReenableNightBGM()
        {
            // summary: there is a scene header which has a single byte that determines what plays at night, setting to 13 re-enables BGM at night

            // since scene table is only read previously on enemizer, if not loaded we have to load now
            if (RomData.SceneList == null)
            {
                ReadSceneTable();
            }

            // TODO since this is static, it can be moved
            var TargetSceneEnums = new GameObjects.Scene[] 
            { 
                GameObjects.Scene.TerminaField,
                GameObjects.Scene.RoadToSouthernSwamp,
                GameObjects.Scene.SouthernSwamp,
                GameObjects.Scene.SouthernSwampClear,
                GameObjects.Scene.PathToMountainVillage,
                GameObjects.Scene.MountainVillage,
                GameObjects.Scene.MountainVillageSpring,
                GameObjects.Scene.TwinIslands,
                GameObjects.Scene.TwinIslandsSpring,
                GameObjects.Scene.GoronRacetrack,
                GameObjects.Scene.GoronVillage,
                GameObjects.Scene.GoronVillageSpring,
                GameObjects.Scene.PathToSnowhead,
                GameObjects.Scene.Snowhead,
                GameObjects.Scene.MilkRoad,
                GameObjects.Scene.GreatBayCoast,
                GameObjects.Scene.PinnacleRock,
                GameObjects.Scene.ZoraCape,
                GameObjects.Scene.WaterfallRapids,
                GameObjects.Scene.RoadToIkana,
                GameObjects.Scene.IkanaCanyon,
                GameObjects.Scene.EastClockTown,
                GameObjects.Scene.WestClockTown,
                GameObjects.Scene.NorthClockTown,
                GameObjects.Scene.SouthClockTown,
                GameObjects.Scene.LaundryPool,
                GameObjects.Scene.Woodfall,
            }.ToList();

            foreach (var SceneEnum in TargetSceneEnums)
            {
                ReenableNightBGMSingle(RomData.SceneList.Find(u => u.Number == SceneEnum.Id()).File);
            }

            // Kamaro the dancing ghost in Termina Field breaks night music
            //   he calls a function that sets an unknown actor flag unk39 & 20, he calls this function per frame from multiple places
            // if we nop it his music never plays, and might music is never interupted by him
            var kamaroFID = 593;
            RomUtils.CheckCompressed(kamaroFID);
            var kamaroData = RomData.MMFileList[kamaroFID].Data;
            // null function call to func_800B9084 -> NOP
            ReadWriteUtils.Arr_WriteU32(kamaroData, 0x618, 0x00000000);

            // Sakon the Bomb Bag theif breaks night music
            //   on first night, after he's supposed to have stolen the bag
            //   his actor will spawn, check the time, and self-destroy, and take out BGM with it
            // if we nop that kill music command it will stop him from stopping BGM
            var sakonFID = 526;
            RomUtils.CheckCompressed(sakonFID);
            var sakonData = RomData.MMFileList[sakonFID].Data;
            // null function call to Audio_QueueSeqCmd -> NOP
            ReadWriteUtils.Arr_WriteU32(sakonData, 0x3A0C, 0x00000000);
        }
    }
}
