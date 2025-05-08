global using Terraria.ModLoader;
global using Terraria;
global using Terraria.ID;
global using Microsoft.Xna.Framework.Graphics;
global using Microsoft.Xna.Framework;
global using Terraria.GameContent;
global using Terraria.DataStructures;
global using LogSpiralLibrary;
global using LogSpiralLibrary.CodeLibrary;
global using static LogSpiralLibrary.CodeLibrary.Utilties.Extensions.RecipeMethods;
global using static LogSpiralLibrary.CodeLibrary.Utilties.Extensions.DrawingMethods;
using Terraria.ModLoader.Config;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Contents.Melee;

namespace FinalFractalSet
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class FinalFractalSet : Mod
    {
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();
            switch (msgType)
            {
                /*case MessageType.PureFractalFrameSync: 
                    {
                        short projIndex = reader.ReadInt16();
                        byte projFrame = reader.ReadByte();
                        if (Main.netMode == NetmodeID.Server) 
                        {
                            ModPacket packet = this.GetPacket();
                            packet.Write((byte)msgType);
                            packet.Write(projIndex);
                            packet.Write(projFrame);
                            packet.Send(-1, whoAmI);
                        }
                        Main.projectile[projIndex].frame = projFrame;
                        break;
                    }*/
                case MessageType.FinalFractalFieldsSync:
                    {
                        //bool holdingFinalFractal = false;
                        //int usingFinalFractal = 0;
                        //bool usedFinalFractal = false;
                        //int waitingFinalFractal = 0;
                        //int finalFractalTier = 0;
                        //int firstTierCounter;
                        //if (Main.netMode == NetmodeID.Server)
                        //{
                        //    ModPacket packet = this.GetPacket();
                        //    packet.Write((byte)msgType);
                        //    packet.Write(projIndex);
                        //    packet.Write(projFrame);
                        //    packet.Send(-1, whoAmI);
                        //}
                        break;
                    }
            }
            ;
            base.HandlePacket(reader, whoAmI);
        }
        public override void Load()
        {
            Instance = this;
            base.Load();
        }



        public static FinalFractalSet Instance;
    }
    public enum MessageType
    {
        FinalFractalFieldsSync
    }

    public class FinalFractalSetConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [ReloadRequired]
        [DefaultValue(true)]
        public bool LoadOldVersionWeapons;

        public static FinalFractalSetConfig instance;

        public static bool OldVersionEnabled => instance.LoadOldVersionWeapons;

        public override void OnLoaded()
        {
            instance = this;
            base.OnLoaded();
        }
    }
    public abstract class FinalFractalSetAction : MeleeAction
    {
        public override string Category => "";
    }
}
