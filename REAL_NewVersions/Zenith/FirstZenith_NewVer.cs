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
using static System.Net.Mime.MediaTypeNames;

namespace FinalFractalSet.REAL_NewVersions.Zenith
{
    public class FirstZenith_NewVer : MeleeSequenceItem<FirstZenith_NewVer_Proj>
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
            Item.ShaderItemEffectInventory(spriteBatch, position, origin, LogSpiralLibraryMod.Misc[0].Value, Color.Lerp(new Color(99, 74, 187), new Color(20, 120, 118), (float)Math.Sin(MathHelper.Pi / 60 * LogSpiralLibraryMod.ModTime) / 2 + 0.5f), scale);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.ShaderItemEffectInWorld(spriteBatch, LogSpiralLibraryMod.Misc[0].Value, Color.Lerp(new Color(99, 74, 187), new Color(20, 120, 118), (float)Math.Sin(MathHelper.Pi / 60 * LogSpiralLibraryMod.ModTime) / 2 + 0.5f), rotation);
        }
        public override bool AltFunctionUse(Player player) => true;
    }
    public class FirstZenith_NewVer_Proj : MeleeSequenceProj
    {
        public override string Texture => base.Texture.Replace("_Proj", "");
        public override StandardInfo StandardInfo => base.StandardInfo with
        {
            standardColor = Color.Cyan * .25f,
            vertexStandard = new()
            {
                active = true,
                scaler = 90,
                timeLeft = 15,
                alphaFactor = 2f,
                renderInfos = [[new AirDistortEffectInfo(4, 0, 0.5f)],
                    [new MaskEffectInfo(LogSpiralLibraryMod.Mask[2].Value, Color.Blue, 0.15f, 0.2f, new Vector2((float)LogSpiralLibraryMod.ModTime), true, false),
                    new BloomEffectInfo(0, 1f, 1, 3, true) { useModeMK = true, downSampleLevel = 2 }]]
            },
            itemType = ModContent.ItemType<FirstZenith_NewVer>()
        };
        public override bool LabeledAsCompleted => true;
        [SequenceDelegate]
        static void ShootFirstZenith(MeleeAction action)
        {
            if (action.Owner is not Player player || player.whoAmI != Main.myPlayer) return;

            for (int k = 0; k < 3; k++)
            {

                Vector2 value5 = Main.MouseWorld;
                List<NPC> list2;
                bool sparkleGuitarTarget2 = FirstZenith_Old.GetSparkleGuitarTarget(out list2);
                if (sparkleGuitarTarget2)
                {
                    NPC NPC2 = list2[Main.rand.Next(list2.Count)];
                    value5 = NPC2.Center + NPC2.velocity * 20f;
                }
                Vector2 vector32 = value5 - player.Center;
                Vector2 vector33 = Main.rand.NextVector2CircularEdge(1f, 1f);
                float num78 = 1f;
                int num79 = 1;
                for (int num80 = 0; num80 < num79; num80++)
                {
                    if (!sparkleGuitarTarget2)
                    {
                        value5 += Main.rand.NextVector2Circular(24f, 24f);
                        if (vector32.Length() > 700f)
                        {
                            vector32 *= 700f / vector32.Length();
                            value5 = player.Center + vector32;
                        }
                        float num81 = Utils.GetLerpValue(0f, 6f, player.velocity.Length(), true) * 0.8f;
                        vector33 *= 1f - num81;
                        //vector33 += player.velocity * num81;
                        vector33 = vector33.SafeNormalize(Vector2.UnitX);
                    }
                    float num82 = 60f;
                    float num83 = Main.rand.NextFloatDirection() * 3.14159274f * (1f / num82) * 0.5f * num78;
                    float num84 = num82 / 2f;
                    float scaleFactor3 = 12f + Main.rand.NextFloat() * 2f;
                    Vector2 vector34 = vector33 * scaleFactor3;
                    Vector2 vector35 = new Vector2(0f, 0f);
                    Vector2 vector36 = vector34;
                    int num85 = 0;
                    while (num85 < num84)
                    {
                        vector35 += vector36;
                        vector36 = vector36.RotatedBy(num83, default);
                        num85++;
                    }
                    Vector2 value6 = -vector35;
                    Vector2 position1 = value5 + value6;
                    float lerpValue2 = Utils.GetLerpValue(player.itemAnimationMax, 0f, player.itemAnimation, true);
                    Projectile.NewProjectile(action.Projectile.GetSource_FromThis(), position1, vector34, ModContent.ProjectileType<FirstZenithProj>(), action.CurrentDamage, action.Projectile.knockBack, player.whoAmI, num83, lerpValue2);
                }
            }
        }
    }
}
