using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalFractalSet.REAL_NewVersions.Stone;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using Terraria.Audio;
using System.IO;
using System.ComponentModel;
using System.Xml;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Contents.Melee;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Core;

namespace FinalFractalSet.REAL_NewVersions.Iron
{
    public class RustySteelBlade_NewVer : MeleeSequenceItem<RustySteelBlade_NewVer_Proj>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 40;
        }
        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            recipe.QuickAddIngredient(
            ItemID.CopperBroadsword,
            ItemID.TinBroadsword,
            ItemID.IronBroadsword,
            ItemID.LeadBroadsword,
            ItemID.SilverBroadsword,
            ItemID.TungstenBroadsword,
            ItemID.GoldBroadsword,
            ItemID.PlatinumBroadsword,
            ItemID.Gladius,
            ItemID.Katana,
            ItemID.DyeTradersScimitar);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        public override bool AltFunctionUse(Player player) => true;
    }
    public class RustySteelBlade_NewVer_Proj : MeleeSequenceProj
    {
        public override bool LabeledAsCompleted => true;
        public override string Texture => base.Texture.Replace("_Proj", "");
        public override StandardInfo StandardInfo => base.StandardInfo with
        {
            standardColor = Color.Lerp(Color.SandyBrown, Color.Brown, .25f) * .5f,
            vertexStandard = new()
            {
                active = true,
                scaler = 120,
                timeLeft = 15,
                alphaFactor = 2f,
            },
            itemType = ModContent.ItemType<RustySteelBlade_NewVer>()
        };
    }
    public class RefinedSteelBlade_NewVer : MeleeSequenceItem<RefinedSteelBlade_NewVer_Proj>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 100;
        }
        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            recipe.AddIngredient<RustySteelBlade_NewVer>();
            recipe.AddIngredient(ItemID.BrokenHeroSword);
            recipe.QuickAddIngredient(
            ItemID.CobaltSword,
            ItemID.PalladiumSword,
            ItemID.MythrilSword,
            ItemID.OrichalcumSword,
            ItemID.BreakerBlade,
            ItemID.Cutlass,
            ItemID.AdamantiteSword,
            ItemID.TitaniumSword,
            ItemID.ChlorophyteSaber,
            ItemID.ChlorophyteClaymore);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        public override bool AltFunctionUse(Player player) => true;
    }
    public class RefinedSteelBlade_NewVer_Proj : MeleeSequenceProj
    {
        public override bool LabeledAsCompleted => true;
        public override string Texture => base.Texture.Replace("_Proj", "");
        public override StandardInfo StandardInfo => base.StandardInfo with
        {
            standardColor = Color.Gray * .5f,
            vertexStandard = new()
            {
                active = true,
                scaler = 120,
                timeLeft = 15,
                alphaFactor = 2f,
            },
            itemType = ModContent.ItemType<RefinedSteelBlade_NewVer>()
        };
    }
    public class SteelSpecialAttack : FinalFractalSetAction
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
            int t = 0;
            Vector2 unit = Rotation.ToRotationVector2() * 16;
            try
            {
                while (t < 80)
                {
                    t++;
                    var tile = Main.tile[(Owner.Center + unit * t).ToTileCoordinates()];
                    if (tile.HasTile && Main.tileSolid[tile.TileType])
                        break;
                }
            }
            catch
            { }

            for (int n = 0; n < 300; n++)
            {
                OtherMethods.FastDust(Owner.Center + n / 300f * unit * t, Vector2.Lerp(Main.rand.NextVector2Unit(), Main.rand.NextVector2Unit() * 4 - unit, n / 300f), standardInfo.standardColor);
            }

            t -= 2;
            if (t > 0)
                //Owner.velocity += unit * t / 15f;
                if (Owner is Player plr)
                {
                    plr.Teleport(plr.Center + unit * t, 1);
                }
            SoundEngine.PlaySound(SoundID.Item92);


            //Owner.Center += unit * t;

            var u = UltraSwoosh.NewUltraSwoosh(standardInfo.standardColor, 30, 8 * t + 90, Owner.Center - unit * t * .25f, null, !flip, Rotation, t * .25f + 2, (1.7f, -.2f), 3, 7);
            if (standardInfo.vertexStandard.renderInfos != null && standardInfo.vertexStandard.renderInfos.Length > 0)
                u.ModityAllRenderInfo(standardInfo.vertexStandard.renderInfos);
            else
                u.ResetAllRenderInfo();
            for (int n = 0; n < 4; n++)
            {
                var cen = Owner.Center - unit * .5f * t + Main.rand.NextVector2Unit() * unit.Length() * Main.rand.NextFloat(0, t * .15f);
                Projectile.NewProjectile(Projectile.GetItemSource_FromThis(), cen, unit.RotatedByRandom(MathHelper.Pi * 3) / 16f * Main.rand.NextFloat(160, 400) * Main.rand.NextFloat(-1, 1), ModContent.ProjectileType<SteelSAProjectile>(), CurrentDamage * 2 / 3, Projectile.knockBack, Projectile.owner, Main.rand.Next(-25, 26), cen.X, cen.Y);
            }
            if (Upgraded)
            {
                int[] indexs = [-1, -1, -1, -1];
                float[] Dists = [float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity];
                foreach (var target in Main.npc)
                {
                    if (target.friendly || !target.CanBeChasedBy() || !target.active) continue;
                    float d = (target.Center - Owner.Center).Length();
                    if (d > 1024) continue;
                    for (int k = 0; k < 4; k++)
                    {
                        if (d < Dists[k])
                        {
                            for (int j = 4 - 1 - k; j > 0; j--)
                            {
                                indexs[j] = indexs[j - 1];
                                Dists[j] = Dists[j - 1];
                            }
                            indexs[k] = target.whoAmI;
                            Dists[k] = d;
                            break;
                        }
                    }
                }
                for (int n = 0; n < 4; n++)
                {
                    var cen = Owner.Center - unit * .5f * t + Main.rand.NextVector2Unit() * unit.Length() * Main.rand.NextFloat(0, t * .15f);
                    if (indexs[n] != -1)
                    {
                        cen = Main.npc[indexs[n]].Center;
                    }
                    Projectile.NewProjectile(Projectile.GetItemSource_FromThis(), cen, unit.RotatedByRandom(MathHelper.Pi * 3) / 16f * Main.rand.NextFloat(160, 400) * Main.rand.NextFloat(-1, 1), ModContent.ProjectileType<SteelSAProjectile>(), CurrentDamage, Projectile.knockBack, Projectile.owner, Main.rand.Next(-25, 26), cen.X, cen.Y);
                }
            }
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
        [ElementCustomData]
        [DefaultValue(false)]
        public bool Upgraded;
        public class SteelSAProjectile : ModProjectile
        {
            public override string Texture => base.Texture.Replace("SteelSAProjectile", "RustySteelBlade_NewVer");

            Projectile projectile => Projectile;
            public override void PostAI()
            {
                Vector2 value1 = Main.player[projectile.owner].position - Main.player[projectile.owner].oldPosition;
                for (int num31 = projectile.oldPos.Length - 1; num31 > 0; num31--)
                {
                    projectile.oldPos[num31] = projectile.oldPos[num31 - 1];
                    projectile.oldRot[num31] = projectile.oldRot[num31 - 1];
                    projectile.oldSpriteDirection[num31] = projectile.oldSpriteDirection[num31 - 1];
                    if (projectile.numUpdates == 0 && projectile.oldPos[num31] != Vector2.Zero)
                    {
                        projectile.oldPos[num31] += value1;
                    }
                }
                projectile.oldPos[0] = projectile.Center;
                projectile.oldRot[0] = projectile.rotation;
                projectile.oldSpriteDirection[0] = projectile.spriteDirection;

                if (Main.netMode == NetmodeID.Server) return;
                if (swooshes[0] == null)
                {
                    for (int n = 0; n < 4; n++)
                    {
                        swooshes[n] = UltraSwoosh.NewUltraSwoosh(newColor, 30, 1, Main.player[projectile.owner].Center, null, false, colorVec: new(0.1667f, 0.3333f, 0.5f));//0.25f, 0.25f, 0.5f//0.1667f, 0.3333f, 0.5f
                        swooshes[n].autoUpdate = false;
                        swooshes[n].weaponTex = TextureAssets.Item[Main.player[projectile.owner].HeldItem.type].Value;
                    }
                    //swooshes[0].ModityAllRenderInfo([[new AirDistortEffectInfo(4, 0, 0.5f)], [new BloomEffectInfo(0, 1f, 1, 3, true) { useModeMK = true, downSampleLevel = 2 }]]);
                }
                for (int n = 0; n < 4; n++)
                    if (swooshes[n] != null) swooshes[n].autoUpdate = false;
                int timePassed = MathHelper.Clamp(3600 - projectile.timeLeft, 0, 60);
                if (timePassed < 4) return;
                Vector2[] vecOuter = new Vector2[timePassed];
                Vector2[] vecInner = new Vector2[timePassed];
                float weaponL = 90;
                for (int n = 0; n < timePassed; n++)
                {
                    Vector2 offset = weaponL * (projectile.oldRot[n] - MathHelper.PiOver2).ToRotationVector2() * .5f;
                    vecOuter[n] = projectile.oldPos[n] + offset * 2;
                    vecInner[n] = projectile.oldPos[n];
                }
                vecOuter = vecOuter.CatMullRomCurve(2 * timePassed);
                vecInner = vecInner.CatMullRomCurve(2 * timePassed);

                int m = Math.Clamp(timePassed / 15 + 1, 1, 4);
                Player player = Main.player[projectile.owner];
                Vector2 center = player.MountedCenter + projectile.velocity;
                for (int n = 0; n < m; n++)
                {
                    var swoosh = swooshes[n];
                    swoosh.center = center;
                    var vtxs = swoosh.VertexInfos;
                    swoosh.weaponTex ??= TextureAssets.Projectile[Type].Value;
                    int c = n == m - 1 && timePassed != 60 ? Math.Min(swoosh.Counts, timePassed % 15 * 3) : swoosh.Counts;
                    for (int i = 0; i < c; i++)
                    {
                        float k = i / (c - 1f);
                        Color color = newColor * MathHelper.SmoothStep(0, 1, 4 * k) * MathF.Pow(1 - .2f * n, 2);
                        vtxs[2 * i] = new CustomVertexInfo(vecOuter[i + 45 * n], color, new(k, 1, 1));
                        vtxs[2 * i + 1] = new CustomVertexInfo(vecInner[i + 45 * n], color, new(0, 0, 1));
                    }
                }
            }
            public override void AI()
            {
                projectile.localNPCHitCooldown = projectile.frame == 1 ? 5 : 15;
                Player player = Main.player[projectile.owner];
                Vector2 mountedCenter = new Vector2(projectile.ai[1], projectile.ai[2]);//坐骑上玩家的中心
                float lerpValue = Utils.GetLerpValue(900f, 0f, projectile.velocity.Length() * 2f, true);//获取线性插值的t值
                float num = MathHelper.Lerp(0.7f, 2f, lerpValue);//速度的模长的两倍越接近900，这个值越接近0.7f
                projectile.localAI[0] += num;
                Main.projFrames[projectile.type] = 25;
                if (projectile.localAI[0] >= 60f)
                {
                    projectile.Kill();
                    return;
                }
                //目标离你越近，剑气的飞行时间越短
                float lerpValue2 = Utils.GetLerpValue(0f, 1f, projectile.localAI[0] / 60f, true);//??这不就单纯clamp了一下
                float num3 = projectile.ai[0];
                float num4 = projectile.velocity.ToRotation();
                float num5 = 3.14159274f;
                float num6 = projectile.velocity.X > 0f ? 1 : -1;
                float num7 = num5 + num6 * lerpValue2 * 6.28318548f;
                float num8 = projectile.velocity.Length() + Utils.GetLerpValue(0.5f, 1f, lerpValue2, true) * 40f;
                float num9 = 60f;
                if (num8 < num9)
                {
                    num8 = num9;//保证半长轴最短是60
                }
                Vector2 value = mountedCenter + projectile.velocity;//椭圆中心
                Vector2 spinningpoint = new Vector2(1f, 0f).RotatedBy(num7, default) * new Vector2(num8, num3 * MathHelper.Lerp(2f, 1f, lerpValue));//插值生成椭圆轨迹
                Vector2 value2 = value + spinningpoint.RotatedBy(num4, default);//加上弹幕自身旋转量
                Vector2 value3 = (1f - Utils.GetLerpValue(0f, 0.5f, lerpValue2, true)) * new Vector2((projectile.velocity.X > 0f ? 1 : -1) * -num8 * 0.1f, -projectile.ai[0] * 0.3f);//坐标修改偏移量
                float num10 = num7 + num4;
                projectile.rotation = num10 + 1.57079637f;//弹幕绘制旋转量
                projectile.Center = value2 + value3;
                projectile.spriteDirection = projectile.direction = projectile.velocity.X > 0f ? 1 : -1;
                if (num3 < 0f)//小于零就反向
                {
                    projectile.rotation = num5 + num6 * lerpValue2 * -6.28318548f + num4;
                    projectile.rotation += 1.57079637f;
                    projectile.spriteDirection = projectile.direction = projectile.velocity.X > 0f ? -1 : 1;
                }
                projectile.Opacity = Utils.GetLerpValue(0f, 5f, projectile.localAI[0], true) * Utils.GetLerpValue(120f, 115f, projectile.localAI[0], true);//修改透明度
            }
            UltraSwoosh[] swooshes = new UltraSwoosh[4];
            Color newColor => projectile.frame == 1 ? Color.Gray : Color.SandyBrown;
            public override void OnSpawn(IEntitySource source)
            {
                if (Main.netMode == NetmodeID.Server) return;
                for (int n = 0; n < 4; n++)
                {
                    swooshes[n] = UltraSwoosh.NewUltraSwoosh(newColor, 30, 1, Main.player[projectile.owner].Center, null, false, colorVec: new(0.1667f, 0.3333f, 0.5f));//0.25f, 0.25f, 0.5f//0.1667f, 0.3333f, 0.5f
                    swooshes[n].autoUpdate = false;
                    swooshes[n].weaponTex = TextureAssets.Item[Main.player[projectile.owner].HeldItem.type].Value;

                }
                swooshes[0].ModityAllRenderInfo([[new AirDistortEffectInfo(4, 0, 0.5f)], [new BloomEffectInfo(0, 1f, 1, 3, true) { useModeMK = true, downSampleLevel = 2 }]]);
                base.OnSpawn(source);
            }
            public override void OnKill(int timeLeft)
            {
                if (Main.netMode == NetmodeID.Server) return;
                for (int n = 0; n < 4; n++)
                {
                    swooshes[n].timeLeft = 0;
                }
                base.OnKill(timeLeft);
            }
            public override bool PreDraw(ref Color lightColor)
            {
                return false;
            }
            public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
            {
                Rectangle _lanceHitboxBounds = new Rectangle(0, 0, 300, 300);
                float num2 = 0f;
                float scaleFactor = 40f;
                for (int i = 14; i < projectile.oldPos.Length; i += 15)
                {
                    float num3 = projectile.localAI[0] - i;
                    if (num3 >= 0f && num3 <= 60f)
                    {
                        Vector2 vector2 = projectile.oldPos[i];
                        Vector2 value2 = (projectile.oldRot[i] + 1.57079637f).ToRotationVector2();
                        _lanceHitboxBounds.X = (int)vector2.X - _lanceHitboxBounds.Width / 2;
                        _lanceHitboxBounds.Y = (int)vector2.Y - _lanceHitboxBounds.Height / 2;
                        if (_lanceHitboxBounds.Intersects(targetHitbox) && Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), vector2 - value2 * scaleFactor, vector2 + value2 * scaleFactor, 20f, ref num2))
                        {
                            return true;
                        }
                    }
                }
                Vector2 value3 = (projectile.rotation + 1.57079637f).ToRotationVector2();
                _lanceHitboxBounds.X = (int)projectile.position.X - _lanceHitboxBounds.Width / 2;
                _lanceHitboxBounds.Y = (int)projectile.position.Y - _lanceHitboxBounds.Height / 2;
                return _lanceHitboxBounds.Intersects(targetHitbox) && Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center - value3 * scaleFactor, projectile.Center + value3 * scaleFactor, 20f, ref num2);
            }
            public override void SetDefaults()
            {
                projectile.width = 32;
                projectile.height = 32;
                projectile.aiStyle = -1;
                projectile.friendly = true;
                projectile.DamageType = DamageClass.Melee;
                projectile.tileCollide = false;
                projectile.ignoreWater = true;
                projectile.alpha = 255;
                projectile.extraUpdates = 1;
                projectile.usesLocalNPCImmunity = true;
                projectile.manualDirectionChange = true;
                projectile.localNPCHitCooldown = 15;
                projectile.penetrate = -1;
            }
            public override void SendExtraAI(BinaryWriter writer)
            {
                writer.Write((byte)projectile.frame);
            }
            public override void ReceiveExtraAI(BinaryReader reader)
            {
                projectile.frame = reader.ReadByte();
            }
            public override Color? GetAlpha(Color lightColor)
            {
                lightColor = Color.White * projectile.Opacity;
                return lightColor;
            }
            public override void SetStaticDefaults()
            {
                ProjectileID.Sets.TrailCacheLength[projectile.type] = 60;
            }
        }
    }
}
