using FinalFractalSet.Weapons;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing.RenderDrawingContents;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Contents.Melee;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Contents.Melee.Core;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Core;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using System;
using System.ComponentModel;
using System.IO;
using Terraria.Audio;

namespace FinalFractalSet.REAL_NewVersions.Iron;

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

    public override void InitializeStandardInfo(StandardInfo standardInfo, VertexDrawStandardInfo vertexStandard)
    {
        standardInfo.standardColor = Color.Lerp(Color.SandyBrown, Color.Brown, .25f) * .5f;
        standardInfo.itemType = ModContent.ItemType<RefinedSteelBlade_NewVer>();

        vertexStandard.scaler = 120;
        vertexStandard.timeLeft = 15;
        vertexStandard.alphaFactor = 2f;
        base.InitializeStandardInfo(standardInfo, vertexStandard);
    }
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

    public override void InitializeStandardInfo(StandardInfo standardInfo, VertexDrawStandardInfo vertexStandard)
    {
        standardInfo.standardColor = Color.Gray * .5f;
        standardInfo.itemType = ModContent.ItemType<RefinedSteelBlade_NewVer>();

        vertexStandard.scaler = 120;
        vertexStandard.timeLeft = 15;
        vertexStandard.alphaFactor = 2f;
        base.InitializeStandardInfo(standardInfo, vertexStandard);
    }
}

public class SteelSpecialAttack : FinalFractalSetAction
{
    public override float CompositeArmRotation => base.CompositeArmRotation;
    public override float OffsetRotation => base.OffsetRotation + MathHelper.SmoothStep(MathHelper.Pi * Owner.direction, 0, MathHelper.Clamp((1 - Factor) / .25f, 0, 1));
    public override float OffsetSize => base.OffsetSize;
    public override Vector2 OffsetCenter => base.OffsetCenter;
    public override Vector2 OffsetOrigin => base.OffsetOrigin;
    public override float OffsetDamage => base.OffsetDamage;
    public override bool Attacktive => Factor < .75f;


    public override void OnStartAttack()
    {
        int t = 0;
        Vector2 unit = Rotation.ToRotationVector2() * 16;
        try
        {
            while (t < 80)
            {
                t++;
                var tile = Framing.GetTileSafely((Owner.Center + unit * t).ToTileCoordinates());
                if (tile.HasTile && Main.tileSolid[tile.TileType])
                    break;
            }
        }
        catch
        { }

        for (int n = 0; n < 300; n++)
        {
            MiscMethods.FastDust(Owner.Center + n / 300f * unit * t, Vector2.Lerp(Main.rand.NextVector2Unit(), Main.rand.NextVector2Unit() * 4 - unit, n / 300f), StandardInfo.standardColor);
        }

        t -= 2;
        if (t > 0)
            //Owner.velocity += unit * t / 15f;
            if (Owner is Player plr)
            {
                plr.Teleport(plr.Center + unit * t, 1);
            }
        SoundEngine.PlaySound(SoundID.Item92, Owner.Center);

        //Owner.Center += unit * t;
        if (!Main.dedServ)
        {
            var u = UltraSwoosh.NewUltraSwoosh(StandardInfo.VertexStandard.canvasName, 30, 8 * t + 90, Owner.Center - unit * t * .25f, (1.7f, -.2f));
            u.negativeDir = !Flip;
            u.rotation = Rotation;
            u.xScaler = t * .25f + 2;
            u.aniTexIndex = 3;
            u.baseTexIndex = 7;
            u.ApplyStdValueToVtxEffect(StandardInfo);
        }
        if (IsLocalProjectile)
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

    public override void UpdateStatus(bool triggered)
    {
        Flip = Owner.direction == 1;
        base.UpdateStatus(triggered);
    }

    [ElementCustomData]
    [DefaultValue(false)]
    public bool Upgraded;

    public override void NetSendInitializeElement(BinaryWriter writer)
    {
        base.NetSendInitializeElement(writer);
        writer.Write(Upgraded);
    }
    public override void NetReceiveInitializeElement(BinaryReader reader)
    {
        base.NetReceiveInitializeElement(reader);
        Upgraded = reader.ReadBoolean();
    }

    public class SteelSAProjectile : ModProjectile
    {
        public override string Texture => base.Texture.Replace("SteelSAProjectile", "RustySteelBlade_NewVer");


        public override void PostAI()
        {
            Vector2 value1 = Main.player[Projectile.owner].position - Main.player[Projectile.owner].oldPosition;
            for (int num31 = Projectile.oldPos.Length - 1; num31 > 0; num31--)
            {
                Projectile.oldPos[num31] = Projectile.oldPos[num31 - 1];
                Projectile.oldRot[num31] = Projectile.oldRot[num31 - 1];
                Projectile.oldSpriteDirection[num31] = Projectile.oldSpriteDirection[num31 - 1];
                if (Projectile.numUpdates == 0 && Projectile.oldPos[num31] != Vector2.Zero)
                {
                    Projectile.oldPos[num31] += value1;
                }
            }
            Projectile.oldPos[0] = Projectile.Center;
            Projectile.oldRot[0] = Projectile.rotation;
            Projectile.oldSpriteDirection[0] = Projectile.spriteDirection;

            if (Main.dedServ) return;
            if (_swooshes[0] == null)
            {
                for (int n = 0; n < 4; n++)
                {
                    var u = _swooshes[n] = UltraSwoosh.NewUltraSwoosh(PureFractalProj.CanvasName, 30, 1, Main.player[Projectile.owner].Center, (-1.125f, 0.7125f));
                    u.negativeDir = false;
                    u.ColorVector = new(0.1667f, 0.3333f, 0.5f);
                    u.autoUpdate = false;
                    u.weaponTex = TextureAssets.Item[Main.player[Projectile.owner].HeldItem.type].Value;
                }
                //swooshes[0].ModityAllRenderInfo([[new AirDistortEffectInfo(4, 0, 0.5f)], [new BloomEffectInfo(0, 1f, 1, 3, true) { useModeMK = true, downSampleLevel = 2 }]]);
            }
            for (int n = 0; n < 4; n++)
                _swooshes[n]?.autoUpdate = false;
            int timePassed = MathHelper.Clamp(3600 - Projectile.timeLeft, 0, 60);
            if (timePassed < 4) return;
            Vector2[] vecOuter = new Vector2[timePassed];
            Vector2[] vecInner = new Vector2[timePassed];
            float weaponL = 90;
            for (int n = 0; n < timePassed; n++)
            {
                Vector2 offset = weaponL * (Projectile.oldRot[n] - MathHelper.PiOver2).ToRotationVector2() * .5f;
                vecOuter[n] = Projectile.oldPos[n] + offset * 2;
                vecInner[n] = Projectile.oldPos[n];
            }
            vecOuter = vecOuter.CatMullRomCurve(2 * timePassed);
            vecInner = vecInner.CatMullRomCurve(2 * timePassed);

            int m = Math.Clamp(timePassed / 15 + 1, 1, 4);
            Player player = Main.player[Projectile.owner];
            Vector2 center = player.MountedCenter + Projectile.velocity;
            for (int n = 0; n < m; n++)
            {
                var swoosh = _swooshes[n];
                swoosh.center = center;
                var vtxs = swoosh.VertexInfos;
                swoosh.weaponTex ??= TextureAssets.Projectile[Type].Value;
                int c = n == m - 1 && timePassed != 60 ? Math.Min(swoosh.Counts, timePassed % 15 * 3) : swoosh.Counts;
                for (int i = 0; i < c; i++)
                {
                    float k = i / (c - 1f);
                    Color color = NewColor * MathHelper.SmoothStep(0, 1, 4 * k) * MathF.Pow(1 - .2f * n, 2);
                    vtxs[2 * i] = new CustomVertexInfo(vecOuter[i + 45 * n], color, new(k, 1, 1));
                    vtxs[2 * i + 1] = new CustomVertexInfo(vecInner[i + 45 * n], color, new(0, 0, 1));
                }
            }
        }

        public override void AI()
        {
            Projectile.localNPCHitCooldown = Projectile.frame == 1 ? 5 : 15;
            Vector2 mountedCenter = new(Projectile.ai[1], Projectile.ai[2]);//坐骑上玩家的中心
            float lerpValue = Utils.GetLerpValue(900f, 0f, Projectile.velocity.Length() * 2f, true);//获取线性插值的t值
            float num = MathHelper.Lerp(0.7f, 2f, lerpValue);//速度的模长的两倍越接近900，这个值越接近0.7f
            Projectile.localAI[0] += num;
            Main.projFrames[Projectile.type] = 25;
            if (Projectile.localAI[0] >= 60f)
            {
                Projectile.Kill();
                return;
            }
            //目标离你越近，剑气的飞行时间越短
            float lerpValue2 = Utils.GetLerpValue(0f, 1f, Projectile.localAI[0] / 60f, true);//??这不就单纯clamp了一下
            float num3 = Projectile.ai[0];
            float num4 = Projectile.velocity.ToRotation();
            float num5 = 3.14159274f;
            float num6 = Projectile.velocity.X > 0f ? 1 : -1;
            float num7 = num5 + num6 * lerpValue2 * 6.28318548f;
            float num8 = Projectile.velocity.Length() + Utils.GetLerpValue(0.5f, 1f, lerpValue2, true) * 40f;
            float num9 = 60f;
            if (num8 < num9)
            {
                num8 = num9;//保证半长轴最短是60
            }
            Vector2 value = mountedCenter + Projectile.velocity;//椭圆中心
            Vector2 spinningpoint = new Vector2(1f, 0f).RotatedBy(num7, default) * new Vector2(num8, num3 * MathHelper.Lerp(2f, 1f, lerpValue));//插值生成椭圆轨迹
            Vector2 value2 = value + spinningpoint.RotatedBy(num4, default);//加上弹幕自身旋转量
            Vector2 value3 = (1f - Utils.GetLerpValue(0f, 0.5f, lerpValue2, true)) * new Vector2((Projectile.velocity.X > 0f ? 1 : -1) * -num8 * 0.1f, -Projectile.ai[0] * 0.3f);//坐标修改偏移量
            float num10 = num7 + num4;
            Projectile.rotation = num10 + 1.57079637f;//弹幕绘制旋转量
            Projectile.Center = value2 + value3;
            Projectile.spriteDirection = Projectile.direction = Projectile.velocity.X > 0f ? 1 : -1;
            if (num3 < 0f)//小于零就反向
            {
                Projectile.rotation = num5 + num6 * lerpValue2 * -6.28318548f + num4;
                Projectile.rotation += 1.57079637f;
                Projectile.spriteDirection = Projectile.direction = Projectile.velocity.X > 0f ? -1 : 1;
            }
            Projectile.Opacity = Utils.GetLerpValue(0f, 5f, Projectile.localAI[0], true) * Utils.GetLerpValue(120f, 115f, Projectile.localAI[0], true);//修改透明度
        }

        private readonly UltraSwoosh[] _swooshes = new UltraSwoosh[4];
        private Color NewColor => Projectile.frame == 1 ? Color.Gray : Color.SandyBrown;

        public override void OnKill(int timeLeft)
        {
            for (int n = 0; n < 4; n++)
                _swooshes[n]?.timeLeft = 0;

            base.OnKill(timeLeft);
        }

        public override bool PreDraw(ref Color lightColor) => false;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Rectangle _lanceHitboxBounds = new(0, 0, 300, 300);
            float num2 = 0f;
            float scaleFactor = 40f;
            for (int i = 14; i < Projectile.oldPos.Length; i += 15)
            {
                float num3 = Projectile.localAI[0] - i;
                if (num3 >= 0f && num3 <= 60f)
                {
                    Vector2 vector2 = Projectile.oldPos[i];
                    Vector2 value2 = (Projectile.oldRot[i] + 1.57079637f).ToRotationVector2();
                    _lanceHitboxBounds.X = (int)vector2.X - _lanceHitboxBounds.Width / 2;
                    _lanceHitboxBounds.Y = (int)vector2.Y - _lanceHitboxBounds.Height / 2;
                    if (_lanceHitboxBounds.Intersects(targetHitbox) && Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), vector2 - value2 * scaleFactor, vector2 + value2 * scaleFactor, 20f, ref num2))
                    {
                        return true;
                    }
                }
            }
            Vector2 value3 = (Projectile.rotation + 1.57079637f).ToRotationVector2();
            _lanceHitboxBounds.X = (int)Projectile.position.X - _lanceHitboxBounds.Width / 2;
            _lanceHitboxBounds.Y = (int)Projectile.position.Y - _lanceHitboxBounds.Height / 2;
            return _lanceHitboxBounds.Intersects(targetHitbox) && Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - value3 * scaleFactor, Projectile.Center + value3 * scaleFactor, 20f, ref num2);
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.manualDirectionChange = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.penetrate = -1;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)Projectile.frame);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.frame = reader.ReadByte();
        }

        public override Color? GetAlpha(Color lightColor)
        {
            lightColor = Color.White * Projectile.Opacity;
            return lightColor;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
        }
    }
}