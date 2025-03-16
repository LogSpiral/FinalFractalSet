using FinalFractalSet.Weapons;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Melee;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using static System.Net.Mime.MediaTypeNames;

namespace FinalFractalSet.REAL_NewVersions.Pure
{
    public class PureFractal_NewVer : MeleeSequenceItem<PureFractal_NewVer_Proj>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 240;
        }
        public override void AddRecipes()
        {

        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Item.ShaderItemEffectInventory(spriteBatch, position, origin, LogSpiralLibraryMod.Misc[1].Value, Color.Lerp(new Color(0, 162, 232), new Color(34, 177, 76), (float)Math.Sin(MathHelper.Pi / 60 * LogSpiralLibraryMod.ModTime) / 2 + 0.5f), scale);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.ShaderItemEffectInWorld(spriteBatch, LogSpiralLibraryMod.Misc[1].Value, Color.Lerp(new Color(0, 162, 232), new Color(34, 177, 76), (float)Math.Sin(MathHelper.Pi / 60 * LogSpiralLibraryMod.ModTime) / 2 + 0.5f), rotation);
        }
        public override bool AltFunctionUse(Player player) => true;
    }
    public class PureFractal_NewVer_Proj : MeleeSequenceProj
    {
        public override string Texture => base.Texture.Replace("_Proj", "");
        public override StandardInfo StandardInfo => base.StandardInfo with
        {
            standardColor = Color.Green * .5f,
            vertexStandard = new()
            {
                active = true,
                scaler = 90,
                timeLeft = 15,
                alphaFactor = 2f,
                renderInfos = [[new AirDistortEffectInfo(4, 0, 0.5f)],
                    [new ArmorDyeInfo(ItemID.VortexDye),
                    new BloomEffectInfo(0, 1f, 1, 3, true) { useModeMK = true, downSampleLevel = 2 }]]
            },
            itemType = ModContent.ItemType<PureFractal_NewVer>()
        };
        public override bool LabeledAsCompleted => true;
        [SequenceDelegate]
        static void ShootPurefractalProj_Few(MeleeAction action)
        {
            if (action.Owner is not Player plr || plr.whoAmI != Main.myPlayer) return;
            for (int n = 0; n < 3; n++)
                ShootSingle(action, plr);
        }
        [SequenceDelegate]
        static void ShootPurefractalProj_Lots(MeleeAction action)
        {
            if (action.Owner is not Player plr || plr.whoAmI != Main.myPlayer) return;
            for (int n = 0; n < 7; n++)
                ShootSingle(action, plr);
        }
        static void ShootSingle(MeleeAction action, Player plr)
        {
            Vector2 vector = plr.RotatedRelativePoint(plr.MountedCenter, true, true);
            float num6 = Main.mouseX + Main.screenPosition.X - vector.X;
            float num7 = Main.mouseY + Main.screenPosition.Y - vector.Y;
            int num166 = (plr.itemAnimationMax - plr.itemAnimation) / plr.itemTime;
            Vector2 velocity_ = new Vector2(num6, num7);
            Vector2 value7 = Main.MouseWorld - plr.MountedCenter;
            if (num166 == 1 || num166 == 2)
            {
                int num168;
                bool zenithTarget = PureFractal_Old.GetZenithTarget(Main.MouseWorld, 400f, plr, out num168);
                if (zenithTarget)
                {
                    value7 = Main.npc[num168].Center - plr.MountedCenter;
                }
                bool flag8 = num166 == 2;
                if (num166 == 1 && !zenithTarget)
                {
                    flag8 = true;
                }
                if (flag8)
                {
                    value7 += Main.rand.NextVector2Circular(150f, 150f);
                }
            }
            velocity_ = value7 / 2f;
            float ai5 = Main.rand.Next(-100, 101);//
            //if(plr.ownedProjectileCounts[type] < 1)
            var proj = Projectile.NewProjectileDirect(action.Projectile.GetItemSource_FromThis(), plr.Center, velocity_, ModContent.ProjectileType<PureFractalProj>(), action.CurrentDamage, action.Projectile.knockBack, plr.whoAmI, ai5);
            proj.frame = Main.rand.Next(26);
            proj.localAI[0] -= Main.rand.Next(0, Main.rand.Next(0, 120));
            proj.netUpdate = true;
            proj.netUpdate2 = true;
        }
    }
}
