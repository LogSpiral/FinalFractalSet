using FinalFractalSet.REAL_NewVersions.Stone;
using FinalFractalSet.Weapons;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Contents.Melee;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Core;
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
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
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
                //heatMap = ModAsset.bar_25.Value,
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
            if (action.Projectile.ModProjectile is not FirstZenith_NewVer_Proj proj) return;
            if (proj.HitCausedZenithCooldown <= 0)
            {
                int m = Main.rand.Next(0, Main.rand.Next(1, 3)) + 1;
                for (int n = 0; n < m; n++)
                    ShootFirstZenithViaPosition(action, action.Rotation.ToRotationVector2() * 128 + action.Owner.Center, false);
                proj.HitCausedZenithCooldown = 15;
            }
            else
                proj.HitCausedZenithCooldown -= 3;

        }

        [SequenceDelegate]
        static void ShootFirstZenithViaStab(MeleeAction action)
        {
            //Vector2 unit = action.Owner is Player plr ?
            //     (plr.GetModPlayer<LogSpiralLibraryPlayer>().targetedMousePosition - action.Owner.Center).SafeNormalize(default) : action.Rotation.ToRotationVector2();
            Vector2 unit = action.Rotation.ToRotationVector2();
            Projectile.NewProjectile(action.Projectile.GetSource_FromThis(), action.Owner.Center, unit * 32, ModContent.ProjectileType<FractalDash>(), action.Projectile.damage, action.Projectile.knockBack, Main.myPlayer, 0, Main.rand.NextFloat(), 2);// Main.rand.NextFloat(-.002f, .002f)

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
                Projectile.NewProjectile(action.Projectile.GetSource_FromThis(), position1, vector34, ModContent.ProjectileType<FirstZenithProj>(), action.Projectile.damage, action.Projectile.knockBack, player.whoAmI, num83, Main.rand.NextFloat());
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
    public class FractalDash : ModProjectile
    {
        public override string Texture => base.Texture.Replace("FractalDash", "FirstZenith_NewVer");
        //public override bool IsLoadingEnabled(Mod mod) => FinalFractalSetConfig.OldVersionEnabled;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[projectile.owner] = 0;
            if (projectile.ai[2] > 0 && Main.rand.NextBool(10))
            {
                if (!Main.rand.NextBool(3))
                    projectile.ai[2]--;
                var velocity = Main.rand.NextVector2Unit() * 32;
                Projectile.NewProjectile(projectile.GetProjectileSource_OnHit(target, Type),
                    target.Center + Main.rand.NextVector2Unit() * Main.rand.NextFloat(target.Hitbox.Size().Length() * .25f) - velocity * 15, velocity, Type, projectile.damage, projectile.knockBack, projectile.owner, 0, Main.rand.NextFloat(), projectile.ai[2] - 1);
            }
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP) target.immune = false;
            base.OnHitPlayer(target, info);
        }
        Projectile projectile => Projectile;
        public Player drawPlayer;
        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 64;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.DamageType = DamageClass.Melee;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 3;
            projectile.usesLocalNPCImmunity = true;
            projectile.manualDirectionChange = true;
            projectile.penetrate = -1;
            //ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 45;
            base.SetStaticDefaults();
        }
        public float drawColor => projectile.ai[1];
        UltraStab stab;
        public override void OnSpawn(IEntitySource source)
        {
            projectile.frame = Main.rand.Next(15);
            drawPlayer = new Player();
            if (Main.netMode == NetmodeID.Server) return;
            stab = UltraStab.NewUltraStab(Color.Violet, 30, 1, null, ModAsset.bar_19.Value, xscaler: 3, colorVec: new(0.16667f, 0.33333f, 0.5f));// UltraSwoosh.NewUltraSwoosh(Color.Violet, 30, 1, null, ModAsset.bar_19.Value, colorVec: new(0.16667f, 0.33333f, 0.5f));
            stab.weaponTex = TextureAssets.Item[Main.player[projectile.owner].HeldItem.type].Value;
            stab.gather = false;
            stab.ModityAllRenderInfo([[new AirDistortEffectInfo(4, 0, 0.5f)], [new MaskEffectInfo(LogSpiralLibraryMod.Mask[2].Value, Color.Violet, 0.15f, 0.2f, new Vector2((float)LogSpiralLibraryMod.ModTime), true, false), new BloomEffectInfo(0, 1f, 1, 3, true) { useModeMK = true, downSampleLevel = 2 }]]);

        }
        public override void AI()
        {
            //if (Main.player[projectile.owner].name == "FFT") 
            //{
            //    projectile.extraUpdates = 3;
            //}
            Main.projFrames[projectile.type] = 15;
            float num = 60f;
            projectile.localAI[0]++;
            if (projectile.localAI[0] >= 300)
                projectile.Kill();
            projectile.velocity = projectile.velocity.RotatedBy((double)projectile.ai[0], default(Vector2));
            projectile.Opacity = Utils.GetLerpValue(0f, 12f, projectile.localAI[0], true) * Utils.GetLerpValue(num, num - 12f, projectile.localAI[0], true);
            projectile.direction = projectile.velocity.X > 0f ? 1 : -1;
            projectile.spriteDirection = projectile.direction;
            projectile.rotation = projectile.velocity.ToRotation();
            if (projectile.localAI[0] > 7f)
            {
                if (Main.rand.NextBool(15))
                {
                    Dust dust = Dust.NewDustPerfect(projectile.Center, MyDustId.CyanBubble, null, 100, Color.Lerp(Main.hslToRgb(drawColor, 1f, 0.5f), Color.White, Main.rand.NextFloat() * 0.3f), 1f);
                    dust.scale = 0.7f;
                    dust.noGravity = true;
                    dust.velocity *= 0.5f;
                    dust.velocity += projectile.velocity * 2f;
                }
            }
            if (Main.netMode == NetmodeID.Server) return;
            if (stab == null)
            {
                stab = UltraStab.NewUltraStab(Color.Violet, 30, 360, null, ModAsset.bar_19.Value, xscaler: 3, colorVec: new(0.16667f, 0.33333f, 0.5f));// UltraSwoosh.NewUltraSwoosh(Color.Violet, 30, 1, null, ModAsset.bar_19.Value, colorVec: new(0.16667f, 0.33333f, 0.5f));
                stab.ModityAllRenderInfo([[new AirDistortEffectInfo(4, 0, 0.5f)], [new MaskEffectInfo(LogSpiralLibraryMod.Mask[2].Value, Color.Violet, 0.15f, 0.2f, new Vector2((float)LogSpiralLibraryMod.ModTime), true, false), new BloomEffectInfo(0, 1f, 1, 3, true) { useModeMK = true, downSampleLevel = 2 }]]);
                stab.weaponTex = TextureAssets.Item[Main.player[projectile.owner].HeldItem.type].Value;
                stab.gather = false;
            }
            stab.xScaler = MathHelper.Clamp(12 - projectile.localAI[0] / 6, 3, 12);
            stab.center = projectile.Center - projectile.velocity * 3;
            stab.rotation = projectile.rotation;
            stab.scaler = 360 + 32 * projectile.localAI[0];
            stab.center -= projectile.velocity * .5f * projectile.localAI[0];
            stab.xScaler *= stab.scaler / 360f;
            if (stab.scaler == 1)
                stab.scaler = 360;
            if (!stab.OnSpawn)
                stab.timeLeft++;
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.netMode == NetmodeID.Server) return;

            stab.timeLeft = 0;
            base.OnKill(timeLeft);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            DrawOthers();
            DrawSword();
            return false;
        }

        public void DrawOthers()
        {
            drawPlayer ??= new Player();
            Player player = drawPlayer;
            if (player == null) { }
            player.CopyVisuals(Main.player[projectile.owner]);
            player.isFirstFractalAfterImage = true;
            player.firstFractalAfterImageOpacity = projectile.Opacity;
            //player.ResetEffects();
            player.ResetVisibleAccessories();
            player.UpdateDyes();
            player.DisplayDollUpdate();
            player.UpdateSocialShadow();
            player.itemAnimationMax = 60;
            player.itemAnimation = (int)projectile.localAI[0];
            player.itemRotation = projectile.velocity.ToRotation();
            player.Center = projectile.Center;
            player.direction = projectile.velocity.X > 0f ? 1 : -1;
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * (float)player.direction, projectile.velocity.X * (float)player.direction);
            player.velocity.Y = 0.01f;
            player.wingFrame = 2;
            player.PlayerFrame();
            player.socialIgnoreLight = true;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, projectile.rotation - MathHelper.PiOver2);
            try
            {
                Main.PlayerRenderer.DrawPlayer(Main.Camera, player, player.position, 0f, player.fullRotationOrigin);
            }
            catch
            {
            }
            SpriteEffects spriteEffects = projectile.ai[0] > 0 ? 0 : SpriteEffects.FlipHorizontally;
            Vector2 vector71 = projectile.position + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
            //projectile.DrawProjWithStarryTrail(Main.spriteBatch, drawColor, Color.White);
            var color84 = Color.White * projectile.Opacity * 0.9f;
            color84.A /= 2;
            //projectile.DrawPrettyStarSparkle(Main.spriteBatch, spriteEffects, vector71, color84, Main.hslToRgb(drawColor, 1f, 0.5f));
        }
        public void DrawSword()
        {
            SpriteEffects spriteEffects = drawPlayer.direction > 0 ? 0 : SpriteEffects.FlipHorizontally;
            Texture2D texture2D4 = TextureAssets.Projectile[ModContent.ProjectileType<FirstZenithProj>()].Value;
            var color84 = Color.White * projectile.Opacity * 0.9f;
            color84.A /= 2;
            var rectangle29 = texture2D4.Frame(15, 1, projectile.frame, 0, 0, 0);
            var origin = texture2D4.Size() / new Vector2(15, 1);
            origin *= spriteEffects == 0 ? new Vector2(0.1f, 0.9f) : new Vector2(0.9f, 0.9f);
            var rot = projectile.velocity.ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4 * drawPlayer.direction;
            Main.spriteBatch.Draw(texture2D4, projectile.Center - projectile.velocity * .5f - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(rectangle29), color84, rot, origin, 1, spriteEffects, 0);
        }
    }
}
