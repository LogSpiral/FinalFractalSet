using FinalFractalSet.REAL_NewVersions.Pure;
using FinalFractalSet.REAL_NewVersions.Zenith;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Contents.Melee;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Contents.Melee.ExtendedMelee;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Core;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.System;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;

namespace FinalFractalSet.REAL_NewVersions.Final
{
    public class FinalFractal_NewVer : MeleeSequenceItem<FinalFractal_NewVer_Proj>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Purple;
            Item.damage = 350;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<PureFractal_NewVer>().AddIngredient<FirstZenith_NewVer>().AddTile(TileID.LunarCraftingStation).Register();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
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
    public class FinalFractal_NewVer_Proj : MeleeSequenceProj
    {
        public override bool LabeledAsCompleted => true; 

        public override string Texture => base.Texture.Replace("_Proj", "");

        public static IRenderDrawInfo[][] RenderDrawInfos = [[new AirDistortEffectInfo(16, 0, 0.5f)],
                    [new MaskEffectInfo(LogSpiralLibraryMod.Mask[2].Value, Color.Violet, 0.15f, 0.2f, new Vector2((float)LogSpiralLibraryMod.ModTime), true, false),
                    new ArmorDyeInfo(ItemID.StardustDye),
                    new BloomEffectInfo(0, 1f, 1, 3, true) { useModeMK = true, downSampleLevel = 2 }]];

        public override StandardInfo StandardInfo => base.StandardInfo with
        {
            standardColor = Color.MediumPurple * .5f,
            vertexStandard = new()
            {
                active = true,
                scaler = 140,
                timeLeft = 45,
                alphaFactor = 2f,
                renderInfos = RenderDrawInfos
            },
            itemType = ModContent.ItemType<FinalFractal_NewVer>()
        };

        // 给左键第一斩用
        [SequenceDelegate]
        static void FInalFractalChop(MeleeAction action)
        {
            Projectile.NewProjectile(
                action.Projectile.GetSource_FromThis(),
                action.Owner.Center,
                action.Rotation.ToRotationVector2() * 64,
                ModContent.ProjectileType<FractalStormSpawner>(),
                action.Projectile.damage,
                action.Projectile.knockBack,
                action.Projectile.owner);
        }

        // 左键第二斩
        [SequenceDelegate]
        static void FinalFractalCut(MeleeAction action)
        {
            Projectile.NewProjectile(
                action.Projectile.GetSource_FromThis(),
                action.Owner.Center,
                action.Rotation.ToRotationVector2(),
                ModContent.ProjectileType<FractalTear>(),
                action.Projectile.damage,
                action.Projectile.knockBack,
                action.Projectile.owner);
        }

        static void ShootSinglgDash(MeleeAction action,float angle) 
        {
            Vector2 unit = (action.Rotation + angle).ToRotationVector2();
            Projectile.NewProjectile(action.Projectile.GetSource_FromThis(),
    action.Owner.Center, unit.RotatedBy(angle) * 32, ModContent.ProjectileType<FractalDash>(),
    action.Projectile.damage, action.Projectile.knockBack, Main.myPlayer, 0, Main.rand.NextFloat(), 2);
        }

        [SequenceDelegate]
        static void FinalFractalStab(MeleeAction action)
        {
            int count = 3;
            float angleMax = MathHelper.Pi / 8;
            ShootSinglgDash(action, 0);
            for (int i = 0; i < count; i++)
            {
                float angle = angleMax * MathF.Pow((i + 1f) / count, 2);
                ShootSinglgDash(action, angle);
                ShootSinglgDash(action, -angle);
            }
        }
    }
    public class FractalCloudMowingSword : StormInfo
    {
        #region 辅助字段
        int HitCausedZenithCooldown;
        bool IsColliding;
        #endregion

        #region 重写属性
        public override string Category => "";

        public override float Factor => 1 - MathF.Pow(1 - base.Factor, 2);

        public override float offsetSize => IsColliding ? 4 : 1;

        public override bool Attacktive => base.Attacktive;

        public override bool OwnerHitCheek => false;
        #endregion

        #region 重写函数

        public override void OnStartAttack()
        {
            base.OnStartAttack();
            if (swoosh != null)
                swoosh.scaler *= 4f;
        }

        public override bool Collide(Rectangle rectangle)
        {
            IsColliding = true;
            var flag = base.Collide(rectangle);
            IsColliding = false;
            return flag;
        }


        public override void OnHitEntity(Entity victim, int damageDone, object[] context)
        {
            base.OnHitEntity(victim, damageDone, context);
            Projectile.localNPCHitCooldown = 2;
            if (HitCausedZenithCooldown <= 0)
            {
                int m = Main.rand.Next(0, Main.rand.Next(1, 3)) + 1;
                for (int n = 0; n < m; n++)
                    FirstZenith_NewVer_Proj.ShootFirstZenithViaPosition(this, Rotation.ToRotationVector2() * 128 + Owner.Center, false);
                HitCausedZenithCooldown = 9;
            }
            else
                HitCausedZenithCooldown -= 3;
        }
        #endregion
    }
    public class FractalStrom : StormInfo
    {
        public override string Category => "";
        #region 重写函数
        public override void OnStartAttack()
        {
            Owner.velocity += Rotation.ToRotationVector2() * 64;

            base.OnStartAttack();
        }

        public override void OnAttack()
        {
            if (timer % Math.Max(timerMax / 15, 1) == 0)
            {
                var velocity = Main.rand.NextVector2Unit() * 32;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                Owner.Center - velocity * 30, velocity, ModContent.ProjectileType<FractalDash>(), CurrentDamage, Projectile.knockBack, Projectile.owner, 0, Main.rand.NextFloat(), 1);
            }
            base.OnAttack();
        }

        public override void OnHitEntity(Entity victim, int damageDone, object[] context)
        {
            base.OnHitEntity(victim, damageDone, context);
            Projectile.localNPCHitCooldown = 2;
        }
        #endregion
    }
    public class FractalTreeStab : PunctureInfo
    {
        public override string Category => "";
        public override void OnBurst(float fallFac)
        {
            base.OnBurst(fallFac);
        }
    }
    public class FractalChargingWing : ChargingInfo
    {

        public override string Category => "";
        public override void OnDeactive()
        {
            base.OnDeactive();
            int type = ModContent.ProjectileType<PureFractalRotatingBlade>();
            foreach (var proj in Main.projectile)
            {
                if (proj.type == type)
                {
                    proj.ai[2] = 2;
                    proj.velocity = proj.rotation.ToRotationVector2() * 16;
                    proj.localNPCHitCooldown = 0;
                    proj.extraUpdates = 3;
                    proj.timeLeft = 240;
                    proj.damage *= 4;
                }
            }

        }
        public override void OnStartAttack()
        {
            base.OnStartAttack();
            var action = this;
            SoundEngine.PlaySound(SoundID.Item84);
            for (int n = 0; n < 20; n++)
            {
                OtherMethods.FastDust(action.Owner.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 32), action.standardInfo.standardColor, Main.rand.NextFloat(1, 4));
            }
            float m = 8 + 8 * action.counter;
            for (int n = 0; n < m; n++)
            {
                float t = n / m;
                var proj = Projectile.NewProjectileDirect(action.Projectile.GetProjectileSource_FromThis(),
                    action.Owner.Center, default, ModContent.ProjectileType<PureFractalRotatingBlade>(),
                    action.CurrentDamage / 4, action.Projectile.damage, Main.myPlayer, t, action.counter);
                proj.netUpdate = true;
                proj.frame = Main.rand.Next(26);
            }
        }
    }
    public class FractalSnowFlake : FinalFractalSetAction
    {
        public override bool Attacktive => base.Attacktive;
        public override float CompositeArmRotation => base.CompositeArmRotation;
        public override float Factor => base.Factor;
        public override Vector2 offsetCenter => base.offsetCenter;
        public override float offsetDamage => base.offsetDamage;
        public override Vector2 offsetOrigin => base.offsetOrigin;
        public override float offsetRotation => base.offsetRotation;
        public override float offsetSize => base.offsetSize;
        public override bool Collide(Rectangle rectangle)
        {
            return base.Collide(rectangle);

        }
        public override void OnActive()
        {
            base.OnActive();
        }
        public override void OnAttack()
        {
            base.OnAttack();
        }
        public override void OnCharge()
        {
            base.OnCharge();
        }
        public override void OnDeactive()
        {
            base.OnDeactive();
        }
        public override void OnEndAttack()
        {
            base.OnEndAttack();
        }
        public override void OnEndSingle()
        {
            base.OnEndSingle();
        }
        public override void OnHitEntity(Entity victim, int damageDone, object[] context)
        {
            base.OnHitEntity(victim, damageDone, context);
        }
        public override void OnStartAttack()
        {
            base.OnStartAttack();
        }
        public override void OnStartSingle()
        {
            base.OnStartSingle();
        }
    }
    public class FractalChargingWingProj : ModProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public override void AI()
        {
            base.AI();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }
        public override string Texture => $"Terraria/Images/Item_{ItemID.TerraBlade}";
    }

}
