using FinalFractalSet.REAL_NewVersions.Stone;
using FinalFractalSet.Weapons;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Melee;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
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
            CreateRecipe()
                .AddIngredient<FirstFractal_Remastered>()
                .AddIngredient(ItemID.LunarBar, 100)
                .AddIngredient(ItemID.FragmentSolar, 100)
                .AddIngredient(ItemID.FragmentStardust, 100)
                .AddIngredient(ItemID.FragmentNebula, 100)
                .AddIngredient(ItemID.FragmentVortex, 100)
                .AddIngredient(ItemID.MartianConduitPlating, 500)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
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
        public override bool LabeledAsCompleted => false;
        [SequenceDelegate]
        static void ShootFirstZenith(MeleeAction action)
        {
            if (action.Projectile.ModProjectile is not FirstZenith_NewVer_Proj proj) return;
            if (proj.HitCausedZenithCooldown <= 0)
            {
                ShootFirstZenithViaPosition(action, action.Rotation.ToRotationVector2() * 128 + action.Owner.Center, false);
                proj.HitCausedZenithCooldown = 15;
            }
            else
                proj.HitCausedZenithCooldown -= 3;
        }

        [SequenceDelegate]
        static void ShootFirstZenithViaStab(MeleeAction action)
        {
            Projectile.NewProjectile(action.Projectile.GetSource_FromThis(), action.Owner.Center, action.Rotation.ToRotationVector2() * 32, ModContent.ProjectileType<FirstZenithProj>(), action.CurrentDamage, action.Projectile.knockBack, Main.myPlayer, Main.rand.NextFloat(-.01f, .01f), Main.rand.NextFloat());

        }

        public static void ShootFirstZenithViaPosition(MeleeAction action, Vector2 vector, bool randomOffset)
        {
            if (action.Owner is not Player player || player.whoAmI != Main.myPlayer) return;
            Vector2 vector32 = vector - player.Center;
            Vector2 vector33 = Main.rand.NextVector2CircularEdge(1f, 1f);
            float num78 = 1f;
            int num79 = 1;
            for (int num80 = 0; num80 < num79; num80++)
            {
                if (!randomOffset)
                {
                    vector += Main.rand.NextVector2Circular(24f, 24f);
                    if (vector32.Length() > 700f)
                    {
                        vector32 *= 700f / vector32.Length();
                        vector = player.Center + vector32;
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
                Vector2 position1 = vector + value6;
                Projectile.NewProjectile(action.Projectile.GetSource_FromThis(), position1, vector34, ModContent.ProjectileType<FirstZenithProj>(), action.CurrentDamage, action.Projectile.knockBack, player.whoAmI, num83, Main.rand.NextFloat());
            }
        }

        int HitCausedZenithCooldown;
        public override void AI()
        {
            HitCausedZenithCooldown--;
            base.AI();
        }
    }
    public class FirstZenithSpecialAttack : FinalFractalSetAction
    {
        public override float CompositeArmRotation => base.CompositeArmRotation;
        public override float offsetRotation => base.offsetRotation + MathHelper.SmoothStep(MathHelper.Pi * Owner.direction, 0, MathHelper.Clamp((1 - Factor) / .25f, 0, 1));
        public override float offsetSize => base.offsetSize;
        public override Vector2 offsetCenter => base.offsetCenter;
        public override Vector2 offsetOrigin => base.offsetOrigin;
        public override float offsetDamage => base.offsetDamage;
        public override bool Attacktive => Factor < .75f;
        public override void OnEndSingle()
        {
            base.OnEndSingle();
        }
        public override void OnStartAttack()
        {
            VectorMethods.GetClosestVectorsFromNPC(Owner.Center, 15, 2048, out var indexs, out _);
            int q = 0;
            while (q < 15 && indexs[q] != -1) q++;
            if (q < 6)
                for (int n = 0; n < 15; n++)
                    FirstZenith_NewVer_Proj.ShootFirstZenithViaPosition(this, n / 3 >= q ? Main.MouseWorld : Main.npc[indexs[n / 3]].Center, n / 3 >= q);
            else
                for (int n = 0; n < 15; n++)
                    FirstZenith_NewVer_Proj.ShootFirstZenithViaPosition(this, n >= q ? Main.MouseWorld : Main.npc[indexs[n]].Center, n >= q);
            SoundEngine.PlaySound(SoundID.Item92);
            var u = UltraSwoosh.NewUltraSwoosh(standardInfo.standardColor, 60, 150, Owner.Center, null, !flip, Rotation, 2, (1.7f, -.2f), 3, 7);
            u.ModityAllRenderInfo(standardInfo.vertexStandard.renderInfos);

            base.OnStartAttack();
        }
        public override void OnCharge()
        {

            base.OnCharge();
        }
        public override void OnStartSingle()
        {
            base.OnStartSingle();
        }
        public override void Update(bool triggered)
        {
            flip = Owner.direction == 1;
            base.Update(triggered);
        }
    }
    public class FirstZenithRainAttack : FinalFractalSetAction
    {
        public override float CompositeArmRotation => base.CompositeArmRotation;
        public override float offsetRotation => MathHelper.SmoothStep(0, 1, MathF.Pow(1 - Factor, 2)) * (-MathHelper.PiOver2 - (Rotation > MathHelper.PiOver2 ? Rotation - MathHelper.TwoPi : Rotation));
        public override float offsetSize => base.offsetSize;
        public override Vector2 offsetCenter => base.offsetCenter;
        public override Vector2 offsetOrigin => base.offsetOrigin;
        public override float offsetDamage => base.offsetDamage;
        public override bool Attacktive => timer == 1;
        public override void OnEndSingle()
        {
            base.OnEndSingle();
        }
        public override void OnStartAttack()
        {
            base.OnStartAttack();
        }
        public override void OnCharge()
        {

            base.OnCharge();
        }
        public override void OnStartSingle()
        {
            base.OnStartSingle();
        }
        public override void OnAttack()
        {
            Vector2 position = Owner.Center + new Vector2(Main.rand.NextFloat(-1280, 1280), -960);
            Vector2 unit = default;
            if (!Main.rand.NextBool(3) && Owner is Player plr)
                unit = (plr.GetModPlayer<LogSpiralLibraryPlayer>().targetedMousePosition - position).SafeNormalize(default);
            else
                unit = Main.rand.NextFloat(MathHelper.Pi / 3, MathHelper.Pi / 3 * 2).ToRotationVector2();
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, unit * 32, ModContent.ProjectileType<FirstZenithProj>(), CurrentDamage, Projectile.knockBack, Main.myPlayer, Main.rand.NextFloat(-.01f, .01f), Main.rand.NextFloat());

            OtherMethods.FastDust(Owner.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 4) + (Rotation + offsetRotation).ToRotationVector2() * Main.rand.NextFloat(0, 64), standardInfo.standardColor, Main.rand.NextFloat(1, 2));

            base.OnAttack();
        }
        public override bool Collide(Rectangle rectangle) => false;
        public override void Update(bool triggered)
        {
            standardInfo = standardInfo with { extraLight = 3 * MathF.Pow(1 - Factor, 4f) };
            flip = Owner.direction == -1;
            switch (Owner)
            {
                case Player player:
                    {
                        //SoundEngine.PlaySound(SoundID.Item71);
                        var tarpos = player.GetModPlayer<LogSpiralLibraryPlayer>().targetedMousePosition;
                        player.direction = Math.Sign(tarpos.X - player.Center.X);
                        Rotation = (tarpos - Owner.Center).ToRotation();
                        break;
                    }

            }
            base.Update(triggered);
            if (timer > 0)
                for (int n = 0; n < 4; n++)
                {
                    Vector2 unit = (MathHelper.PiOver2 * n + 4 * Factor).ToRotationVector2();
                    OtherMethods.FastDust(Owner.Center + unit * (MathF.Exp(Factor) - 1) * 128, default, standardInfo.standardColor, 2f);

                    OtherMethods.FastDust(Owner.Center + new Vector2(unit.X + unit.Y, -unit.X + unit.Y) * (MathF.Exp(Factor) - 1) * 128, default, standardInfo.standardColor, 1.5f);

                }
            if (timer == 1 && counter == Cycle)
            {
                timer = 0;
                SoundEngine.PlaySound(SoundID.Item84);
                for (int n = 0; n < 40; n++)
                {
                    OtherMethods.FastDust(Owner.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 32), standardInfo.standardColor, Main.rand.NextFloat(1, 4));
                    OtherMethods.FastDust(Owner.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 4) + (Rotation + offsetRotation).ToRotationVector2() * Main.rand.NextFloat(0, 64), standardInfo.standardColor, Main.rand.NextFloat(1, 2));

                }
            }
            if (timer < 2 && counter == Cycle)
            {

                if (triggered)
                    timer++;
            }
            if (!triggered && timer != 0)
            {
                timer = 0;
                SoundEngine.PlaySound(MySoundID.MagicStaff);
            }
        }
    }
}
