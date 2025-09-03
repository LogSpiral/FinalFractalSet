using FinalFractalSet.REAL_NewVersions.Stone;
using FinalFractalSet.REAL_NewVersions.Wood;
using FinalFractalSet.REAL_NewVersions.Zenith;
using FinalFractalSet.Weapons;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing.RenderDrawingContents;
using LogSpiralLibrary.CodeLibrary.Utilties;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.Audio;
using Terraria.Graphics;
using static FinalFractalSet.REAL_NewVersions.Iron.SteelSpecialAttack;
using static LogSpiralLibrary.CodeLibrary.Utilties.Extensions.VectorMethods;
using static Terraria.Utils;

namespace FinalFractalSet.REAL_NewVersions.Final;

public abstract class FinalFractalAssistantProjectile : ModProjectile
{
    public const string FractalTexturePath = "FinalFractalSet/REAL_NewVersions/Final/FinalFractalProjectile";
    public override string Texture => FractalTexturePath;
}

// Unused
/*
public class FractalStormSpawner : FinalFractalAssistantProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 16;
        Projectile.height = 16;
        Projectile.aiStyle = -1;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.penetrate = 1;
        Projectile.light = 0.5f;
        Projectile.timeLeft = 180;
        Projectile.alpha = 255;
        Projectile.friendly = true;
    }
    UltraStab stab;
    public override void AI()
    {
        Projectile.localAI[0]++;
        if (Projectile.localAI[0] >= 50)
        {
            Projectile.localAI[0] = 50;
        }
        float num = Projectile.light;
        float num2 = Projectile.light;
        float num3 = Projectile.light;
        if (Projectile.ai[1] == 0f)
        {
            Projectile.ai[1] = 1f;
            SoundEngine.PlaySound(SoundID.Item60, Projectile.position);
        }
        Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 0.785f;
        num *= 0.2f;
        num3 *= 0.6f;
        Lighting.AddLight((int)((Projectile.position.X + Projectile.width / 2) / 16f), (int)((Projectile.position.Y + Projectile.height / 2) / 16f), num, num2, num3);

        if (Main.dedServ) return;
        if (stab == null)
        {
            stab = UltraStab.NewUltraStab(Color.Violet, 30, 360, null, ModAsset.bar_19.Value, xscaler: 3, colorVec: new(0.16667f, 0.33333f, 0.5f));// UltraSwoosh.NewUltraSwoosh(Color.Violet, 30, 1, null, ModAsset.bar_19.Value, colorVec: new(0.16667f, 0.33333f, 0.5f));
            stab.ModityAllRenderInfo(FinalFractal_NewVer_Proj.RenderDrawInfos);
            stab.weaponTex = TextureAssets.Item[Main.player[Projectile.owner].HeldItem.type].Value;
            stab.gather = false;
        }
        stab.xScaler = MathHelper.Clamp(12 - Projectile.localAI[0] / 6, 3, 12);
        stab.center = Projectile.Center - Projectile.velocity * 3;
        stab.rotation = Projectile.rotation - MathHelper.PiOver4;
        stab.scaler = 360 + 32 * Projectile.localAI[0];
        stab.center -= Projectile.velocity * .5f * Projectile.localAI[0];
        stab.xScaler *= stab.scaler / 360f;
        if (stab.scaler == 1)
            stab.scaler = 360;
        if (!stab.OnSpawn)
            stab.timeLeft++;
    }
    public override Color? GetAlpha(Color lightColor)
    {
        if (Projectile.localAI[1] >= 15f)
        {
            return new Color(255, 255, 255, Projectile.alpha);
        }
        if (Projectile.localAI[1] < 5f)
        {
            return Color.Transparent;
        }
        int num7 = (int)((Projectile.localAI[1] - 5f) / 10f * 255f);
        return new Color(num7, num7, num7, num7);
    }
    public override void OnKill(int timeLeft)
    {
        for (int n = 0; n < 3; n++)
            Projectile.NewProjectile(base.Projectile.GetSource_FromThis(),
                Projectile.Center + new Vector2(64, 0).RotatedBy(Projectile.rotation - MathHelper.PiOver4 + MathHelper.TwoPi / 3 * n + MathHelper.Pi),
                new Vector2(-1, 0).RotatedBy(Projectile.rotation - MathHelper.PiOver4 + MathHelper.TwoPi / 3 * n + MathHelper.Pi),
                ModContent.ProjectileType<FractalStorm>(), Projectile.damage, 0, Projectile.owner, 1f, 3f);

        //if (Main.dedServ) return;
        //stab.timeLeft = 0;
    }
    public override bool PreDraw(ref Color lightColor)
    {
        if (LogSpiralLibraryMod.FinalFractalTailEffect == null) return false; if (LogSpiralLibraryMod.ColorfulEffect == null) return false;

        SpriteBatch spriteBatch = Main.spriteBatch;
        var tex = TextureAssets.Projectile[Projectile.type].Value;
        spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, tex.Frame(), Color.White, Projectile.rotation, tex.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
        return false;
    }
}

public class FractalStorm : FinalFractalAssistantProjectile
{
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.immune[Projectile.owner] = 0;
        base.OnHitNPC(target, hit, damageDone);
    }
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 45;
        base.SetStaticDefaults();
    }
    public override void SetDefaults()
    {
        Projectile.width = 16;
        Projectile.height = 16;
        Projectile.aiStyle = -1;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.penetrate = -1;
        Projectile.alpha = 255;
        Projectile.timeLeft = 300;
        Projectile.tileCollide = false;
        Projectile.friendly = true;
    }
    private const int startTime = 5;
    private const int flyTime = 5;
    private float Timer
    {
        get => Projectile.ai[0];
        set => Projectile.ai[0] = value;
    }
    public override bool ShouldUpdatePosition()
    {
        return Timer > startTime;
    }
    FractalStromSwoosh swoosh;
    public override void AI()
    {
        float[] array = Projectile.localAI;
        int num4 = 0;
        float num5 = array[num4] + 1f;
        array[num4] = num5;
        if (Projectile.localAI[0] >= 50)
            Projectile.localAI[0] = 50;

        Timer++;

        if (Projectile.velocity != Vector2.Zero)
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi / 4;

        if (Timer == startTime)
            Projectile.velocity = Projectile.velocity.SafeNormalize(default) * 32;

        if (Timer == startTime + 5)
        {
            Vector2 vec = Projectile.velocity;
            vec.Normalize();
            Projectile.velocity = vec * 0;
        }
        if (Timer == startTime + flyTime && Projectile.ai[1] > 0)
        {
            for (int n = 0; n < 3; n++)
                Projectile.NewProjectileDirect(base.Projectile.GetSource_FromThis(), Projectile.Center +
                    new Vector2(64, 0).RotatedBy(Projectile.rotation - MathHelper.PiOver4 + MathHelper.TwoPi / 3 * n),
                    new Vector2(-1, 0).RotatedBy(Projectile.rotation - MathHelper.PiOver4 + MathHelper.TwoPi / 3 * n),
                    Type, Projectile.damage, 0, Projectile.owner, 0, Projectile.ai[1] - 1).scale = 0.3f * (Projectile.ai[1] - 1);
        }
        if (Timer == startTime + flyTime + Projectile.ai[1] * (startTime + flyTime))
            Projectile.velocity = Projectile.rotation.ToRotationVector2() * 32;

        if (Timer > startTime + flyTime + Projectile.ai[1] * (startTime + flyTime))
            // && (Main.player[Projectile.owner].name == "WitherStorm" || Main.player[Projectile.owner].name == "FFT")
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.TwoPi / 30 * MathF.Pow(Projectile.timeLeft / 300f, 4));
        //return;
        if (Main.dedServ) return;
        if (swoosh == null)
        {
            swoosh = UltraSwoosh
            swoosh = UltraSwoosh.NewUltraSwoosh<FractalStromSwoosh>(Color.Violet, RenderCanvasSystem.RenderDrawingContents, 30, 1, null, ModAsset.bar_19.Value, colorVec: new(0.16667f, 0.33333f, 0.5f));
            swoosh.heatMap = Main.rand.Next([ModAsset.bar_16, ModAsset.bar_17, ModAsset.bar_18, ModAsset.bar_19, ModAsset.bar_20, ModAsset.bar_25]).Value;
            swoosh.autoUpdate = false;
            swoosh.ModityAllRenderInfo([new BloomEffectInfo(0, 1f, 1f, 3, true) with { UseModeMK = true }]);
            //swoosh.ModityAllRenderInfo(FinalFractal_NewVer_Proj.RenderDrawInfos);
            swoosh.weaponTex = TextureAssets.Item[Main.player[Projectile.owner].HeldItem.type].Value;
        }
        if (swoosh != null)
        {
            swoosh.autoUpdate = false;
            swoosh.timeLeft = swoosh.timeLeftMax * 3 / 4;
        }
        for (int n = 44; n > 0; n--)
        {
            Projectile.oldPos[n] = Projectile.oldPos[n - 1];
            Projectile.oldRot[n] = Projectile.oldRot[n - 1];
        }
        Projectile.oldPos[0] = Projectile.Center;
        Projectile.oldRot[0] = Projectile.rotation + MathHelper.PiOver4;//Projectile.velocity.ToRotation() + Projectile.ai[0] * (Projectile.localAI[0] / 60).Lerp(-180, 90, true);
        float lightScaler = MathHelper.SmoothStep(0, 1, 1 - Math.Abs(45 - Projectile.localAI[0]) / 45f);
        switch (Projectile.localAI[0])
        {
            case < 4:
                return;

            case >= 45:
                for (int n = 0; n < 45; n++)
                {
                    float k = n / 44f;
                    Color c = Color.Violet * MathHelper.SmoothStep(0, 1, 4 * k) * lightScaler;
                    Vector2 unit = Projectile.oldRot[n].ToRotationVector2();
                    Vector2 vec = Projectile.oldPos[n];
                    swoosh.VertexInfos[2 * n] = new(vec - unit * 15, c, new(k, 1, 1));
                    swoosh.VertexInfos[2 * n + 1] = new(vec + unit * 45, c, new(0, 0, 1));
                }
                return;

            default:
                int t = (int)Projectile.localAI[0];
                Vector2[] vecOuter = new Vector2[t];
                Vector2[] vecInner = new Vector2[t];
                for (int n = 0; n < t; n++)
                {
                    Vector2 unit = Projectile.oldRot[n].ToRotationVector2();
                    Vector2 vec = Projectile.oldPos[n];

                    vecOuter[n] = vec - unit * 15;
                    vecInner[n] = vec + unit * 45;
                }
                vecOuter = vecOuter.CatMullRomCurve(45 - t);
                vecInner = vecInner.CatMullRomCurve(45 - t);
                for (int n = 0; n < 45; n++)
                {
                    float k = n / 44f;
                    Color c = Color.Violet * MathHelper.SmoothStep(0, 1, 4 * k) * lightScaler;
                    swoosh.VertexInfos[2 * n] = new(vecOuter[n], c, new(k, 1, 1));
                    swoosh.VertexInfos[2 * n + 1] = new(vecInner[n], c, new(0, 0, 1));
                }
                return;
        }
    }
    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);
        //if (Main.dedServ) return;
        //swoosh.timeLeft = 0;
    }
    public override bool PreDraw(ref Color lightColor)
    {
        SpriteBatch spriteBatch = Main.spriteBatch;
        var tex = TextureAssets.Projectile[Projectile.type].Value;
        spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, tex.Frame(),
            Color.White, Projectile.rotation, tex.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);

        spriteBatch.Draw(LogSpiralLibrary.ModAsset.Misc_16.Value, Projectile.Center - Main.screenPosition, null, Color.White with { A = 0 } * .5f, Projectile.rotation + MathHelper.PiOver4, new Vector2(27, 80), 1f, 0, 0);

        for (int n = 0; n < 5; n++)
            spriteBatch.Draw(LogSpiralLibrary.ModAsset.Misc_16.Value, Projectile.oldPos[n] - Main.screenPosition, null, Color.White with { A = 0 } * (.5f - n * .1f) * .5f, Projectile.oldRot[n], new Vector2(27, 80), .75f - n * .15f, 0, 0);
        return false;
    }
}
*/

public class FractalTear : FinalFractalAssistantProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 16;
        Projectile.height = 16;
        Projectile.aiStyle = -1;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.penetrate = -1;
        Projectile.light = 0.5f;
        Projectile.timeLeft = 60;
        Projectile.alpha = 255;
        Projectile.friendly = true;
        base.SetDefaults();
    }

    private UltraSwoosh swoosh;

    public override void AI()
    {
        if (Projectile.timeLeft % 10 == 0)
        {
            VectorMethods.GetClosestVectorsFromNPC(Main.player[Projectile.owner].Center, 3, 2048, out var indexs, out _);
            for (int n = 0; n < 3; n++)
            {
                int index = indexs[n];
                if (index == -1) break;
                var velocity = Main.rand.NextVector2Unit() * 32;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                Main.npc[index].Center - velocity * 30, velocity, ModContent.ProjectileType<FractalDash>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, Main.rand.NextFloat(), 1);
                if (!Main.rand.NextBool(3)) break;
            }
        }
        if (Main.dedServ) return;
        if (swoosh == null)
        {
            var u = swoosh = UltraSwoosh.NewUltraSwoosh(FinalFractal_NewVer_Proj.CanvasName, 30, 240, Projectile.Center, (-1.125f, 0.7125f));
            u.heatMap = ModAsset.bar_19.Value;
            u.negativeDir = Projectile.velocity.X < 0;
            u.rotation = Projectile.velocity.ToRotation();
            u.xScaler = 3;
            u.ColorVector = new(0.16667f, 0.33333f, 0.5f);
            u.aniTexIndex = 3;
            u.baseTexIndex = 7;
            swoosh.autoUpdate = false;
            swoosh.weaponTex = TextureAssets.Item[Main.player[Projectile.owner].HeldItem.type].Value;
            //SoundEngine.PlaySound(MySoundID.Scythe, Projectile.Center);
        }
        if (Projectile.timeLeft > 45)
            swoosh.timeLeft = 2 * (60 - Projectile.timeLeft) + 2;
        else
            swoosh.timeLeft++;

        base.AI();
    }

    public override bool PreDraw(ref Color lightColor) => false;

    public override bool ShouldUpdatePosition() => false;
}

public class PythagoreanTree
{
    private class Node()
    {
        private Node branchLeft;
        private Node branchRight;
        private float angle;
        private float lengthScaler;
        private float width;

        public Node(float width, float length, float angle) : this()
        {
            this.width = width;
            this.lengthScaler = length / width;
            this.angle = angle;
        }

        public void BuildTree(int depth)
        {
            if (depth > 0 && width > 1f)
            {
                branchLeft = new Node()
                {
                    angle = (float)Main.rand.GaussianRandom(MathHelper.PiOver2, MathHelper.Pi / 6),
                    lengthScaler = (float)Main.rand.GaussianRandom(3f, 0.5f),
                    width = this.width * MathF.Cos(angle * .5f)
                };
                branchLeft.BuildTree(depth - 1);

                branchRight = new Node()
                {
                    angle = (float)Main.rand.GaussianRandom(MathHelper.PiOver2, MathHelper.Pi / 6),
                    lengthScaler = (float)Main.rand.GaussianRandom(3f, 0.5f),
                    width = this.width * MathF.Sin(angle * .5f)
                };
                branchRight.BuildTree(depth - 1);
            }
        }

        public void AddToVertexs(IList<CustomVertexInfo> vertexInfos, Color color, Vector2 startCoord, float startRotation, float factor)
        {
            Vector2 normal = startRotation.ToRotationVector2();
            Vector2 unit = new(-normal.Y, normal.X);

            normal *= width * .5f;
            unit *= width * lengthScaler * MathHelper.Clamp(factor, 0, 1f);
            CustomVertexInfo[] rectangleVertexs = new CustomVertexInfo[6];
            rectangleVertexs[0] = new(startCoord - normal, color, new(0, 0, 1));
            rectangleVertexs[1] = new(startCoord + normal, color, new(1, 0, 1));
            rectangleVertexs[2] = new(startCoord + normal + unit, color, new(1, 1, 1));
            rectangleVertexs[3] = rectangleVertexs[2];
            rectangleVertexs[4] = new(startCoord - normal + unit, color, new(0, 1, 1));
            rectangleVertexs[5] = rectangleVertexs[0];

            foreach (var v in rectangleVertexs)
                vertexInfos.Add(v);

            Vector2 target = default;
            Vector3 hsl = Main.rgbToHsl(color);
            Color c1 = Main.hslToRgb(hsl with { X = hsl.X + .05f });
            Color c2 = Main.hslToRgb(hsl with { X = hsl.X - .05f });
            c1.A = color.A;
            c2.A = color.A;
            if (branchLeft != null || branchRight != null)
            {
                Color c = Color.Lerp(c1, c2, .5f);
                target = startCoord + unit + MathF.Cos(angle) * normal + MathF.Sin(angle) * unit.SafeNormalize(default) * width * .5f;
                vertexInfos.Add(rectangleVertexs[2] with { Color = c });
                vertexInfos.Add(new CustomVertexInfo(target, c, new(1, 0, 1)));
                vertexInfos.Add(rectangleVertexs[4] with { Color = c });
            }
            branchLeft?.AddToVertexs(vertexInfos, c1, (target + rectangleVertexs[4].Position) * .5f, startRotation + angle * .5f, factor - .5f);
            branchRight?.AddToVertexs(vertexInfos, c2, (target + rectangleVertexs[2].Position) * .5f, startRotation + angle * .5f - MathHelper.PiOver2, factor - .5f);
        }

        public void RunTask(Action<Vector2, Vector2> task, Vector2 startCoord, float startRotation, int tier)
        {
            Vector2 normal = startRotation.ToRotationVector2();
            Vector2 direction = new(-normal.Y, normal.X);
            Vector2 unit = direction;
            normal *= width * .5f;
            direction *= width * lengthScaler;
            //width *= 2;

            Vector2 target = startCoord + direction + MathF.Cos(angle) * normal + MathF.Sin(angle) * unit * width * .5f;

            Vector2 v1 = startCoord - normal + direction;
            Vector2 v2 = startCoord + normal + direction;

            if (tier == 0)
            {
                task?.Invoke(startCoord, unit);
            }
            else
            {
                branchLeft?.RunTask(task, (target + v1) * .5f, startRotation + angle * .5f, tier - 1);
                branchRight?.RunTask(task, (target + v2) * .5f, startRotation + angle * .5f - MathHelper.PiOver2, tier - 1);
            }

            //width *= .5f;
        }
    }

    //float Width;
    private float Rotation;

    private Node StartNode;

    public PythagoreanTree(float width, float length, float rotation)
    {
        StartNode = new(width, length, (float)Main.rand.GaussianRandom(1.2f, 0.1f));
        //Width = width;
        Rotation = rotation;
        StartNode.BuildTree(10);
    }

    public void Draw(SpriteBatch spriteBatch, Color color, Vector2 start, float factor)
    {
        List<CustomVertexInfo> vertexInfos = [];
        StartNode?.AddToVertexs(vertexInfos, color, start, Rotation, factor * 10);

        Main.graphics.GraphicsDevice.Textures[0] = LogSpiralLibraryMod.BaseTex[8].Value;
        Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexInfos.ToArray(), 0, vertexInfos.Count / 3);
    }

    public void RunTask(Action<Vector2, Vector2> task, Vector2 start, int tier)
    {
        StartNode?.RunTask(task, start, MathHelper.Pi, tier);
    }
}

public class PythagoreanTreeProj : FinalFractalAssistantProjectile
{
    public override void AI()
    {
        if (Projectile.timeLeft is <= 60 and > 36)
        {
            if (Projectile.timeLeft % 3 == 0)
                tree?.RunTask((center, unit) =>
                {
                    //for (int n = 1; n < 5; n++)
                    //{
                    //    var dust = Dust.NewDustPerfect(center, MyDustId.RedBubble);
                    //    dust.velocity = unit * n;
                    //    dust.noGravity = true;
                    //}
                    //Projectile.NewProjectile(Projectile.GetSource_FromThis(), center, unit * 16, ModContent.ProjectileType<FractalDash>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, Main.rand.NextFloat());
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), center, unit * 16, ModContent.ProjectileType<FractalStabProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner, unit.ToRotation(), (float)Main.rand.GaussianRandom(0.5f, 0.16f));
                }, Projectile.Center, (60 - Projectile.timeLeft) / 3);
        }
        base.AI();
    }

    public override void SetDefaults()
    {
        Projectile.timeLeft = 120;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.width = Projectile.height = 1;
        //Projectile.friendly = true;
        Projectile.aiStyle = -1;
        Projectile.ignoreWater = true;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        base.SetDefaults();
    }

    public override void OnSpawn(IEntitySource source)
    {
        tree = new PythagoreanTree(32, 256, MathHelper.Pi);

        base.OnSpawn(source);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        float factor1 = MathF.Pow(Utils.GetLerpValue(120, 60, Projectile.timeLeft, true), 4.0f);
        float factor2 = MathHelper.SmoothStep(0, 1, Utils.GetLerpValue(0, 30, Projectile.timeLeft, true));

        //float k = (1 - MathF.Cos(MathHelper.TwoPi * MathF.Sqrt(1 - Projectile.timeLeft / 60f))) * .5f;

        tree?.Draw(Main.spriteBatch, Color.Blue with { A = 64 } * factor2, Projectile.Center - Main.screenPosition, factor1);
        return false;
    }

    private PythagoreanTree tree;
}

public class FractalStabProj : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.timeLeft = 30;
        Projectile.width = Projectile.height = 32;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.aiStyle = -1;
        Projectile.ignoreWater = true;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 1;

        base.SetDefaults();
    }

    public override void AI()
    {
        //Projectile.velocity *= 1.05f;
        if (Projectile.timeLeft == 20)
            SoundEngine.PlaySound(MySoundID.LaserBeam with { volume = .5f, MaxInstances = -1 });

        base.AI();
    }

    public override string Texture => "Terraria/Images/Extra_98";

    public override bool PreDraw(ref Color lightColor)
    {
        float t = Projectile.timeLeft / 30f;
        float fac = (1 - MathF.Cos(MathHelper.TwoPi * MathF.Sqrt(t))) * .5f;
        //Vector2 unit = Projectile.ai[0].ToRotationVector2();
        float fac2 = MathHelper.SmoothStep(0, 1, MathF.Pow(t, 0.5f));
        Vector2 scaler = new Vector2(fac2, 1 / fac2);
        Color mainColor = Main.hslToRgb(new(Projectile.ai[1], 1, 0.5f));
        Main.EntitySpriteDraw(TextureAssets.Extra[98].Value, Projectile.Center - Main.screenPosition, null, mainColor with { A = 0 } * fac, Projectile.ai[0] + MathHelper.PiOver2, new(36), scaler * new Vector2(1, 4) * fac, 0, 0);
        Main.EntitySpriteDraw(TextureAssets.Extra[98].Value, Projectile.Center - Main.screenPosition, null, Color.White with { A = 0 } * fac, Projectile.ai[0] + MathHelper.PiOver2, new(36), scaler * new Vector2(1, 4) * fac * .75f, 0, 0);
        //Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 1, 1), Color.HotPink, 0, new Vector2(.5f), 16, 0, 0);
        return false;
    }
}

public class FractalChargingWingProj : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.DamageType = DamageClass.Melee;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 10;
        Projectile.width = Projectile.height = 64;
        Projectile.timeLeft = 60;
        Projectile.tileCollide = false;

        base.SetDefaults();
    }

    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 20;
        base.SetStaticDefaults();
    }

    private int attackTimer;

    private int targetIndex;

    private void StartAndIdle()
    {
        int counter = Projectile.frameCounter;
        Player plr = Main.player[Projectile.owner];

        Projectile.timeLeft = 60;
        float factor = counter < 18 ? MathHelper.SmoothStep(0, 1.2f, counter / 18f) : MathHelper.SmoothStep(1.2f, 1, (counter - 18) / 12f);

        float s = plr.velocity.Y != 0 ? 128f : 64f;
        float t = (Projectile.ai[0] % 1) * factor;
        Vector2 offset = new Vector2(0, s);
        Vector2 target;
        float targetRotation;
        switch ((int)Projectile.ai[1])
        {
            case 1:
                {
                    float x = MathHelper.Lerp(0f, 1f, t);
                    float y = 0.25f * x * x + 1;
                    target = -new Vector2(x * plr.direction, y) * s + offset;
                    targetRotation = -x * .5f;
                    break;
                }
            case 2:
                {
                    float x = MathHelper.Lerp(0, 2f, t) + .125f;
                    float y = 1f;
                    target = -new Vector2(x * plr.direction, y) * s + offset;
                    targetRotation = MathHelper.PiOver2 - MathHelper.Lerp(0, 2f, t);
                    break;
                }
            case 3:
                {
                    float x = MathHelper.Lerp(0f, 3f, t);
                    float y = 0.6f + 0.25f * MathF.Pow(x - 1.5f, 2);
                    target = -new Vector2(x * plr.direction, y) * s + offset;
                    targetRotation = MathF.Pow(x - 2, 2.0f) * .5f;
                    break;
                }
            case 4:
                {
                    float x = MathHelper.Lerp(0f, 3f, t);
                    float y = Math.Min(.25f / x, 1.0f) + 0.2f * x;
                    target = -new Vector2(x * plr.direction * 1.33f, y) * s + offset;
                    targetRotation = MathF.Pow(x - 2, 2.0f) * .5f + .2f;
                    break;
                }
            case 5:
                {
                    float x = MathHelper.Lerp(0f, 3f, t);
                    float y = Math.Min(.25f / x, 1.0f);
                    target = -new Vector2(x * plr.direction, y) * s + offset;
                    targetRotation = MathF.Pow(x - 2, 2.0f) * .5f + .4f;
                    break;
                }
            default:
                goto case 1;
        }
        float totalRotation = (Projectile.ai[0] > 1 ? MathHelper.Pi / 6 : 0) + MathHelper.PiOver4 * .75f;
        if (plr.velocity.Y != 0f)
        {
            totalRotation += MathHelper.PiOver4 / 2f * MathF.Cos((counter / 1200f + Main.GlobalTimeWrappedHourly) * MathHelper.TwoPi);
        }
        totalRotation *= plr.direction;

        target = target.RotatedBy(totalRotation);
        targetRotation -= totalRotation * plr.direction;

        if (plr.direction == 1)
            targetRotation = MathHelper.Pi - targetRotation;

        Projectile.Center = Vector2.Lerp(Projectile.Center, target + plr.Center, 0.3f);
        Projectile.rotation = Utils.AngleLerp(Projectile.rotation, targetRotation, 0.3f);
    }

    private void OldDataUpdates()
    {
        for (int n = 19; n > 0; n--)
        {
            Projectile.oldPos[n] = Projectile.oldPos[n - 1];
            Projectile.oldRot[n] = Projectile.oldRot[n - 1];
        }
        Projectile.oldPos[0] = Projectile.position + Projectile.rotation.ToRotationVector2() * 45 * (Projectile.ai[0] > 1 ? 1 : 1.5f);
        Projectile.oldRot[0] = Projectile.rotation + MathHelper.PiOver2 + .05f;
    }

    private void AttackTarget()
    {
        int max1 = 90;
        int max2 = 60;

        bool skipBodyCheck = true;
        int mode = 0;
        int currentModeTimeMax = max2 - 1;
        int checkTime = 0;
        if (attackTimer >= (float)max2 + 1)
        {
            mode = 1;
            currentModeTimeMax = max1 - 1;
            checkTime = max2 + 1;
        }

        //下面这两个老样子，目标没用就找新的，不然就飞回来
        int targetIndex = this.targetIndex;
        if (!Main.npc.IndexInRange(targetIndex))
        {
            int newTarget = AI_156_TryAttackingNPCs(Projectile, skipBodyCheck);
            if (newTarget != -1)
            {
                attackTimer = Main.rand.NextFromList(max1, max2);
                this.targetIndex = newTarget;
                Projectile.netUpdate = true;
                Projectile.ai[2] = 1;
            }
            else
            {
                attackTimer = 0;
                this.targetIndex = 0;
                Projectile.netUpdate = true;
                Projectile.ai[2] = 0;
            }

            return;
        }
        var projectile = Projectile;
        NPC nPC2 = Main.npc[targetIndex];
        if (!nPC2.CanBeChasedBy(projectile))
        {
            int newTarget = AI_156_TryAttackingNPCs(projectile, skipBodyCheck);
            if (newTarget != -1)
            {
                attackTimer = Main.rand.NextFromList(max1, max2);
                this.targetIndex = newTarget;
                Projectile.netUpdate = true;
                Projectile.ai[2] = 1;
            }
            else
            {
                attackTimer = 0;
                this.targetIndex = 0;
                Projectile.netUpdate = true;
                Projectile.ai[2] = 0;
            }

            return;
        }

        attackTimer--;
        if (attackTimer >= (float)currentModeTimeMax)
        {
            projectile.direction = ((projectile.Center.X < nPC2.Center.X) ? 1 : (-1));
            if (attackTimer == (float)currentModeTimeMax)
            {
                projectile.localAI[0] = projectile.Center.X;
                projectile.localAI[1] = projectile.Center.Y;
            }
        }

        float factor = Utils.GetLerpValue(currentModeTimeMax, checkTime, attackTimer, clamped: true);
        //挥砍模式
        //也许叫旋转模式更合适
        if (mode == 0)
        {
            Vector2 vector3 = new(projectile.localAI[0], projectile.localAI[1]);
            if (factor >= 0.5f)
                vector3 = Vector2.Lerp(nPC2.Center, Main.player[projectile.owner].Center, 0.5f);

            Vector2 center3 = nPC2.Center;
            float num20 = (center3 - vector3).ToRotation();
            float num21 = -MathHelper.Pi;//((projectile.direction == 1) ? (-(float)Math.PI) : ((float)Math.PI));
            float num22 = num21 + (0f - num21) * factor * 2f;
            Vector2 spinningpoint2 = num22.ToRotationVector2();
            spinningpoint2.Y *= 0.5f;
            spinningpoint2.Y *= 0.8f + (float)Math.Sin((float)projectile.identity * 2.3f) * 0.2f;
            spinningpoint2 = spinningpoint2.RotatedBy(num20);
            float num23 = (center3 - vector3).Length() / 2f;
            Vector2 center4 = Vector2.Lerp(vector3, center3, 0.5f) + spinningpoint2 * num23;
            projectile.Center = center4;
            float num24 = MathHelper.WrapAngle(num20 + num22 + 0f);
            projectile.rotation = num24;
            Vector2 vector4 = num24.ToRotationVector2() * 10f;
            projectile.velocity = vector4;
            projectile.position -= projectile.velocity;
        }

        //戳刺模式
        //处于这个模式时会以冲向目标的形式发起攻击
        if (mode == 1)
        {
            Vector2 vector5 = new(projectile.localAI[0], projectile.localAI[1]);
            vector5 += new Vector2(0f, Utils.GetLerpValue(0f, 0.4f, factor, clamped: true) * -100f);
            Vector2 v = nPC2.Center - vector5;
            Vector2 vector6 = v.SafeNormalize(Vector2.Zero) * MathHelper.Clamp(v.Length(), 60f, 150f);
            Vector2 value = nPC2.Center + vector6;
            float lerpValue3 = Utils.GetLerpValue(0.4f, 0.6f, factor, clamped: true);
            float lerpValue4 = Utils.GetLerpValue(0.6f, 1f, factor, clamped: true);
            float targetAngle = v.SafeNormalize(Vector2.Zero).ToRotation();
            projectile.rotation = projectile.rotation.AngleTowards(targetAngle, (float)Math.PI / 5f);
            projectile.Center = Vector2.Lerp(vector5, nPC2.Center, lerpValue3);
            if (lerpValue4 > 0f)
                projectile.Center = Vector2.Lerp(nPC2.Center, value, lerpValue4);
        }

        if (attackTimer == (float)checkTime)
        {
            int newTarget = AI_156_TryAttackingNPCs(projectile, skipBodyCheck);
            if (newTarget != -1)
            {
                attackTimer = Main.rand.NextFromList(max1, max2);
                this.targetIndex = newTarget;
                Projectile.netUpdate = true;
                Projectile.ai[2] = 1;
            }
            else
            {
                attackTimer = 0;
                this.targetIndex = 0;
                Projectile.netUpdate = true;
                Projectile.ai[2] = 0;
            }
        }
    }

    internal static int AI_156_TryAttackingNPCs(Projectile projectile, bool skipBodyCheck = false)
    {
        Vector2 center = Main.player[projectile.owner].Center;
        int result = -1;
        float num = -1f;
        NPC ownerMinionAttackTargetNPC = projectile.OwnerMinionAttackTargetNPC;//获取弹幕拥有者标记的NPC
        if (ownerMinionAttackTargetNPC != null && ownerMinionAttackTargetNPC.CanBeChasedBy(projectile))
        {
            bool flag = true;
            //if (!ownerMinionAttackTargetNPC.boss)//是boss或者不在黑名单里面
            //    flag = false;

            if (ownerMinionAttackTargetNPC.Distance(center) > 1000f)//离目标距离小于1000像素
                flag = false;

            if (!skipBodyCheck && !projectile.CanHitWithOwnBody(ownerMinionAttackTargetNPC))//跳过身体检测或者检测通过
                flag = false;

            if (flag)
                return ownerMinionAttackTargetNPC.whoAmI;//直接将标记目标作为攻击目标
        }
        HashSet<int> indexs = [];

        for (int i = 0; i < 200; i++)
            indexs.Add(i);
        for (int k = 0; k < 200; k++)
        {
            int i = Main.rand.Next(indexs.ToArray());
            indexs.Remove(i);
            NPC nPC = Main.npc[i];
            if (nPC.CanBeChasedBy(projectile))//类似上面的检测条件
            {
                float num2 = nPC.Distance(center);
                if (!(num2 > 1000f) && (!(num2 > num) || num == -1f) && (skipBodyCheck || projectile.CanHitWithOwnBody(nPC)))
                {
                    num = num2;
                    result = i;
                    break;
                }
            }
        }

        return result;
    }

    public override void AI()
    {
        Projectile.frameCounter++;
        Player player = Main.player[Projectile.owner];

        switch ((int)Projectile.ai[2])
        {
            case 0:
                {
                    Projectile.velocity = default;
                    StartAndIdle();

                    float chance = 20 + 100f / (Projectile.frameCounter + 1f);
                    if (Main.rand.NextBool((int)chance))
                    {
                        int targetIndex = AI_156_TryAttackingNPCs(Projectile);
                        if (targetIndex != -1)
                        {
                            attackTimer = 90;
                            this.targetIndex = targetIndex;
                            Projectile.netUpdate = true;
                            Projectile.ai[2] = 1;
                        }
                    }
                    break;
                }
            case 1:
                {
                    Projectile.timeLeft = 60;
                    AttackTarget();
                    if (player.active && Vector2.Distance(player.Center, Projectile.Center) > 2000f)//如果离得太远就把ai数组两个元素设为0
                    {
                        Projectile.ai[2] = 0f;
                        Projectile.netUpdate = true;
                    }
                    break;
                }
            case 2:
                {
                    Projectile.timeLeft = 60;
                    Vector2 center;
                    if (Projectile.whoAmI % 2 == 0)
                        center = Projectile.Center;
                    else
                        center = player.MountedCenter;
                    float rotation = (player.GetModPlayer<LogSpiralLibraryPlayer>().targetedMousePosition - center).ToRotation();
                    if (Projectile.whoAmI % 5 == 1)
                        rotation += MathHelper.Pi / 6;
                    if (Projectile.whoAmI % 5 == 2)
                        rotation -= MathHelper.Pi / 6;
                    Projectile.rotation = MathHelper.Lerp(Projectile.rotation, rotation, 0.2f);
                    if (Projectile.frameCounter >= (Projectile.ai[0] % 1 + Projectile.ai[1]) * 15)
                    {
                        Projectile.ai[2] = 3;
                        var proj = Projectile;
                        //proj.velocity = proj.rotation.ToRotationVector2() * 16;
                        proj.localNPCHitCooldown = 0;
                        proj.extraUpdates = 3;
                        proj.timeLeft = 240;
                        proj.damage *= 4;
                        proj.netImportant = true;
                        Projectile.frameCounter = 0;
                    }
                    break;
                }
            case 3:
                {
                    var proj = Projectile;
                    var counter = proj.frameCounter;
                    var unit = proj.rotation.ToRotationVector2();
                    switch (counter)
                    {
                        case < 48:
                            proj.velocity = unit * MathHelper.SmoothStep(0, -12, counter / 48f);
                            break;

                        case < 61:
                            proj.velocity = unit * MathHelper.SmoothStep(-12, 16, (counter - 48) / 12f);
                            break;

                        case 61:
                            for (int n = 0; n < 10; n++)
                                MiscMethods.FastDust(proj.Center,
                                    Main.rand.NextVector2Unit() * Main.rand.NextFloat() * 2 - proj.velocity * Main.rand.NextFloat(),
                                    Main.hslToRgb(Main.rand.NextFloat(0.4f, 0.6f), 1f, 0.75f),
                                    Main.rand.NextFloat(1, 1.5f) * .5f);
                            SoundEngine.PlaySound(MySoundID.LaserBeam);
                            break;
                    }
                    break;
                }
        }
        OldDataUpdates();
        base.AI();
    }

    public override bool PreDraw(ref Color lightColor)
    {
        var tex = PureFractalProj.ItemTextures[Projectile.frame];
        float alpha = 1;
        if (Projectile.ai[2] == 0 && Projectile.frameCounter < 15)
            alpha = Projectile.frameCounter / 15f;
        float scaler = 1;
        lightColor = Color.White;
        if (Projectile.ai[0] > 1)
        {
            var a = lightColor.A;
            lightColor *= .5f;
            lightColor = lightColor with { A = a };

            scaler = .75f;
        }

        Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor * alpha, Projectile.rotation + MathHelper.PiOver4, tex.Size() * Vector2.UnitY, scaler, 0, 0);
        if (Projectile.ai[2] < 3)
        {
            EmpressBladeDrawer empressBladeDrawer = default(EmpressBladeDrawer);
            float num12 = Main.GlobalTimeWrappedHourly % 3f / 3f;
            Player player = Main.player[Projectile.owner];
            float num13 = MathHelper.Max(1f, player.maxMinions);
            float num14 = (float)Projectile.identity % num13 / num13 + num12;
            Color fairyQueenWeaponsColor = Projectile.GetFairyQueenWeaponsColor(0f, 0f, num14 % 1f);
            Color fairyQueenWeaponsColor2 = Projectile.GetFairyQueenWeaponsColor(0f, 0f, (num14 + 0.5f) % 1f);
            empressBladeDrawer.ColorStart = fairyQueenWeaponsColor;
            empressBladeDrawer.ColorEnd = fairyQueenWeaponsColor2;
            //DrawProj_EmpressBlade(proj, num14);
            empressBladeDrawer.Draw(Projectile);
        }
        else
        {
            float factor = MathHelper.SmoothStep(0, 1, Projectile.frameCounter / 120f - .2f);
            Color c = Main.hslToRgb(.5f + .5f * MathF.Sin(Projectile.whoAmI), 1, .5f);
            Main.spriteBatch.Draw(TextureAssets.Extra[98].Value, Projectile.Center + Projectile.velocity * 4 - Main.screenPosition, null, c with { A = 0 } * factor, Projectile.rotation + MathHelper.PiOver2, new(36), new Vector2(1, 8 + Projectile.frameCounter * .05f) * factor, 0, 0);
            Main.spriteBatch.Draw(TextureAssets.Extra[98].Value, Projectile.Center + Projectile.velocity * 4 - Main.screenPosition, null, Color.White with { A = 0 } * factor, Projectile.rotation + MathHelper.PiOver2, new(36), new Vector2(1, 4 + Projectile.frameCounter * .05f) * .75f * factor, 0, 0);
        }

        return false;
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
        Projectile.frame = reader.ReadInt32();
        Projectile.frameCounter = reader.ReadInt32();
        base.ReceiveExtraAI(reader);
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write(Projectile.frame);
        writer.Write(Projectile.frameCounter);
        base.SendExtraAI(writer);
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        if (Projectile.ai[0] > 1)
        {
            behindNPCsAndTiles.Add(index);
            Projectile.hide = true;
        }
        base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
    }

    public override string Texture => $"Terraria/Images/Item_{ItemID.TerraBlade}";
}

public class FractalSnowFlakeProj : FinalFractalAssistantProjectile
{
    public override void AI()
    {
        //Projectile.timeLeft = 2;
        Projectile.ai[0]++;
        if (Projectile.owner != 255)
            Projectile.Center = Main.player[Projectile.owner].Center;
        switch (Projectile.timeLeft)
        {
            case 27:
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - Vector2.UnitY * 295.6f,
                    default, ModContent.ProjectileType<WoodSwordPlrProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                goto case 114514;
            case 18:
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(256f, 147.8f),
                    default, ModContent.ProjectileType<StoneSwordPlrProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                goto case 114514;
            case 9:
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(-256f, 147.8f),
                    default, ModContent.ProjectileType<IronSwordPlrProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                goto case 114514;
            case 114514:
                SoundEngine.PlaySound(SoundID.Item92 with { MaxInstances = -1 });
                break;
        }
        base.AI();
    }

    private static BlendState InverseDestination;

    public override void Load()
    {
        InverseDestination = new BlendState("FinalFractalSet:InverseDestination", Blend.InverseDestinationColor, Blend.One, Blend.InverseSourceAlpha, Blend.Zero);
        base.Load();
    }

    public override void Unload()
    {
        InverseDestination?.Dispose();
        base.Unload();
    }

    private static void DrawSnowFlakePart(SpriteBatch spriteBatch, Vector2 Position, Color color, float size, float rotation, float angle, float tier)
    {
        Vector2 unit = rotation.ToRotationVector2();
        if (tier > 1)
        {
            float newSize = size / (1 + MathF.Sin(angle * .5f)) * .5f;
            Vector2 normal = new(-unit.Y, unit.X);
            DrawSnowFlakePart(spriteBatch, Position - unit * (size - newSize) * .5f, color, newSize, rotation, angle, tier - 1);
            DrawSnowFlakePart(spriteBatch, Position + unit * (size - newSize) * .5f, color, newSize, rotation, angle, tier - 1);
            //DrawSnowFlakePart(spriteBatch, Position - unit * (size * .25f - newSize * .5f) - normal * newSize * MathF.Cos(angle * .5f) * .5f, color, newSize, rotation - (MathHelper.Pi - angle) * .5f, angle, tier - 1);
            //DrawSnowFlakePart(spriteBatch, Position + unit * (size * .25f - newSize * .5f) - normal * newSize * MathF.Cos(angle * .5f) * .5f, color, newSize, rotation + (MathHelper.Pi - angle) * .5f, angle, tier - 1);
            DrawSnowFlakePart(spriteBatch, Position - unit * (size * .25f - newSize * .5f) + normal * newSize * MathF.Cos(angle * .5f), color, newSize, rotation + (MathHelper.Pi - angle) * .5f, angle, tier - 1);
            DrawSnowFlakePart(spriteBatch, Position + unit * (size * .25f - newSize * .5f) + normal * newSize * MathF.Cos(angle * .5f), color, newSize, rotation - (MathHelper.Pi - angle) * .5f, angle, tier - 1);
        }
        else
            spriteBatch.DrawLine(Position - unit * size * .5f, unit * size * tier, color, 2, true);
    }

    private static void DrawSnowFlake(SpriteBatch spriteBatch, Vector2 Position, Color color, float size, float rotation, float angle, float tier)
    {
        Vector2 unit = rotation.ToRotationVector2();
        Vector2 normal = new(-unit.Y, unit.X);

        Vector2 offset = -normal * 0.28868f * size;
        DrawSnowFlakePart(spriteBatch, Position + offset, color, size, rotation, angle, tier);
        DrawSnowFlakePart(spriteBatch, Position + offset.RotatedBy(MathHelper.TwoPi / 3), color, size, rotation + MathHelper.TwoPi / 3, angle, tier);
        DrawSnowFlakePart(spriteBatch, Position + offset.RotatedBy(-MathHelper.TwoPi / 3), color, size, rotation - MathHelper.TwoPi / 3, angle, tier);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        //float r = MathHelper.PiOver2 * (1 + MathF.Cos(Main.GlobalTimeWrappedHourly));
        //DrawSnowFlake(Main.spriteBatch, Projectile.Center - Main.screenPosition, Color.Cyan with { A = 0 }, 256, 0, MathHelper.Pi / 3, Main.GlobalTimeWrappedHourly % 6);////

        if (Projectile.ai[0] < 30)
        {
            float k = MathHelper.SmoothStep(0, 1, Projectile.ai[0] / 30);
            DrawSnowFlake(Main.spriteBatch,
                Projectile.Center - Main.screenPosition,
                Color.Cyan with { A = 0 } * k, 512, 0, MathHelper.Pi / 3, k);
        }
        else
        {
            float k1 = MathHelper.SmoothStep(0, 1, Projectile.ai[0] / 30 - 1);
            float k2 = MathHelper.SmoothStep(0, 1, Projectile.timeLeft / 45f);
            DrawSnowFlake(Main.spriteBatch,
                Projectile.Center - Main.screenPosition,
                Color.Cyan with { A = 0 } * MathF.Pow(k2, 2f), 512 * (1.2f - .2f * MathF.Pow(k2, .2f)), 0, MathHelper.Pi * (1 - k1), 4);
        }
        if (Projectile.timeLeft < 30)
        {
            var spriteBatch = Main.spriteBatch;
            InverseDestination.ColorSourceBlend = Blend.InverseDestinationColor;
            InverseDestination.ColorBlendFunction = BlendFunction.Add;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, InverseDestination, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            if (Projectile.timeLeft is < 30 and > 18)
            {
                float fac = Utils.GetLerpValue(30, 18f, Projectile.timeLeft, true);
                fac = (1 - MathF.Cos(MathHelper.TwoPi * MathF.Sqrt(fac))) * .5f;
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, Projectile.Center - Main.screenPosition, null, Color.White, 0, new Vector2(.5f, 500f), new Vector2(64 * fac, 10), 0, 0);
            }
            if (Projectile.timeLeft is < 21 and > 9)
            {
                float fac = Utils.GetLerpValue(21, 9, Projectile.timeLeft, true);
                fac = (1 - MathF.Cos(MathHelper.TwoPi * MathF.Sqrt(fac))) * .5f;
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, Projectile.Center - Main.screenPosition + Vector2.UnitX * 256, null, Color.White, 0, new Vector2(.5f, 500f), new Vector2(64 * fac, 10), 0, 0);
            }
            if (Projectile.timeLeft is < 12)
            {
                float fac = Utils.GetLerpValue(12, 0f, Projectile.timeLeft, true);
                fac = (1 - MathF.Cos(MathHelper.TwoPi * MathF.Sqrt(fac))) * .5f;
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, Projectile.Center - Main.screenPosition - Vector2.UnitX * 256, null, Color.White, 0, new Vector2(.5f, 500f), new Vector2(64 * fac, 10), 0, 0);
            }
            //spriteBatch.End();
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        return false;
    }

    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 1;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.timeLeft = 105;
        base.SetDefaults();
    }

    public override void OnSpawn(IEntitySource source)
    {
        var player = Main.player[Projectile.owner];
        var fplayer = player.GetModPlayer<FractalPlayer>();
        //if (fplayer.RecordOldDatas)
        //{
        //    Projectile.Kill();
        //    foreach (var proj in Main.projectile)
        //    {
        //        if (proj.active && proj.owner == Projectile.owner && proj.ModProjectile is PlayerLikeProjectile)
        //        {
        //            proj.timeLeft += 600;
        //        }
        //    }
        //}
        //else
        fplayer.RecordOldDatas = true;
        base.OnSpawn(source);
    }
}

public abstract class PlayerLikeProjectile : FinalFractalAssistantProjectile
{
    public Player Player => Main.player[Projectile.owner];
    public FractalPlayer FractalPlayer => Player.GetModPlayer<FractalPlayer>();

    public override void SetDefaults()
    {
        Projectile.timeLeft = 600;
        Projectile.width = Projectile.height = 1;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.penetrate = -1;
        Projectile.localNPCHitCooldown = 2;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.melee = true;
        base.SetDefaults();
    }

    public Player drawPlayer;
    public int targetIndex;

    public override void ReceiveExtraAI(BinaryReader reader)
    {
        targetIndex = reader.ReadByte();
        base.ReceiveExtraAI(reader);
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write((byte)targetIndex);
        base.SendExtraAI(writer);
    }

    public void DrawPlayer()
    {
        var projectile = Projectile;
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
        //player.wingFrame = 2;
        player.wingFrame = 2;
        player.PlayerFrame();
        player.bodyFrame = new Rectangle(0, 0, 40, 56);
        player.legFrame = new Rectangle(0, 0, 40, 56);
        ModifyDrawPlayer(player);
        player.socialIgnoreLight = true;
        projectile.spriteDirection = player.direction;
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

    public override void OnSpawn(IEntitySource source)
    {
        drawPlayer = new Player();

        base.OnSpawn(source);
    }

    public override void AI()
    {
        switch ((int)Projectile.ai[0])
        {
            case 0:
                {
                    Idle();
                    Projectile.rotation = MathHelper.Pi / 2;
                    if (Main.rand.NextBool(20))
                    {
                        int targetIndex = FractalChargingWingProj.AI_156_TryAttackingNPCs(Projectile);
                        if (targetIndex != -1)
                        {
                            Projectile.ai[1] = 90;
                            this.targetIndex = targetIndex;
                            Projectile.netUpdate = true;
                            Projectile.ai[0] = 1;
                        }
                    }
                    break;
                }
            case 1:
                {
                    Projectile.ai[2]++;
                    Projectile.ai[1]--;
                    Attack();
                    if (Projectile.ai[1] <= 0 || !Main.npc[targetIndex].active)
                    {
                        int targetIndex = FractalChargingWingProj.AI_156_TryAttackingNPCs(Projectile);
                        if (targetIndex != -1)
                        {
                            Projectile.ai[1] = 90;
                            this.targetIndex = targetIndex;
                            Projectile.netUpdate = true;
                            Projectile.ai[0] = 1;
                        }
                        else
                        {
                            Projectile.ai[1] = Projectile.ai[2] = 0;
                            this.targetIndex = 0;
                            Projectile.netUpdate = true;
                            Projectile.ai[0] = 0;
                        }
                    }
                    break;
                }
        }
        if (Projectile.timeLeft < 60)
            Projectile.Opacity = MathHelper.SmoothStep(0, 1, Projectile.timeLeft / 60f);
        if (Projectile.timeLeft == 1)
        {
            for (int n = 0; n < 10; n++)
                MiscMethods.FastDust(Projectile.Center + new Vector2(Main.rand.NextFloat(-20, 20), Main.rand.NextFloat(-28, 28)),
                    Vector2.UnitY * Main.rand.NextFloat(-8, -2), Color.White, Main.rand.NextFloat(.5f, 1f));
        }
        base.AI();
    }

    public virtual void Idle()
    { }

    public virtual void Attack()
    { }

    public virtual void ModifyDrawPlayer(Player drawPlayer)
    { }

    public override bool PreDraw(ref Color lightColor)
    {
        DrawPlayer();
        DrawSword();
        return false;
    }

    public virtual void DrawSword()
    {
    }
}

public class FractalPlayer : ModPlayer
{
    public Vector2[] OldPos = new Vector2[30];
    public int[] OldDirection = new int[30];
    public Rectangle[] OldBodyFrame = new Rectangle[30];
    public Rectangle[] OldLegFrame = new Rectangle[30];
    private bool recordOldDatas;

    public bool RecordOldDatas
    {
        get => recordOldDatas;
        set
        {
            recordOldDatas = value;
            ResetOldDatas();
        }
    }

    public void ResetOldDatas()
    {
        Array.Fill(OldPos, Vector2.Zero);
        Array.Fill(OldDirection, 0);
        Array.Fill(OldBodyFrame, default);
        Array.Fill(OldLegFrame, default);
    }

    public override void PostUpdate()
    {
        if (recordOldDatas)
        {
            if (Vector2.Distance(OldPos[0], Player.Center) > 2f)
            {
                for (int n = 29; n > 0; n--)
                {
                    OldPos[n] = OldPos[n - 1];
                    OldDirection[n] = OldDirection[n - 1];
                    OldBodyFrame[n] = OldBodyFrame[n - 1];
                    OldLegFrame[n] = OldLegFrame[n - 1];
                }
                OldPos[0] = Player.Center;
                OldDirection[0] = Player.direction;
                OldBodyFrame[0] = Player.bodyFrame;
                OldLegFrame[0] = Player.legFrame;
            }
        }
        base.PostUpdate();
    }
}

public class WoodSwordPlrProj : PlayerLikeProjectile
{
    public override void AI()
    {
        base.AI();
    }

    public override bool PreDraw(ref Color lightColor)
    {
        return base.PreDraw(ref lightColor);
    }

    public override void DrawSword()
    {
        var rotation = Projectile.rotation - MathHelper.Pi / 12 * Projectile.spriteDirection;// - MathHelper.PiOver4;
        if (Projectile.spriteDirection == -1)
            rotation += MathHelper.Pi;
        Main.spriteBatch.Draw(ModAsset.LivingWoodSword_NewVer.Value,
            Projectile.Center - Main.screenPosition - Vector2.UnitX * 8 * Projectile.spriteDirection,
            null, Color.White * Projectile.Opacity, rotation, new Vector2(Projectile.spriteDirection == -1 ? 58 : 0, 72), 1,
            Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : 0, 0);
        base.DrawSword();
    }

    public override void Attack()
    {
        if (Projectile.ai[2] == 10)
        {
            Projectile.ai[2] = 0;
            var targetPos = Main.rand.NextVector2FromRectangle(Main.npc[targetIndex].Hitbox);
            var delta = targetPos - Projectile.Center;
            Projectile.rotation = delta.ToRotation();
            Projectile.Center += delta.SafeNormalize(default) * (delta.Length() + 64);
            Projectile.velocity = delta.SafeNormalize(default);
            var stab = UltraStab.NewUltraStabOnDefaultCanvas(15, 300, Main.npc[targetIndex].Center - Projectile.velocity * 64);
            stab.negativeDir = false;
            stab.rotation = Projectile.rotation;
            stab.xScaler = 2;
            Projectile.rotation -= MathHelper.PiOver2;
            stab.weaponTex = ModAsset.LivingWoodSword_NewVer.Value;
            SoundEngine.PlaySound(MySoundID.SwooshNormal_1);
            Player.StrikeNPCDirect(Main.npc[targetIndex], new() { Damage = Projectile.damage });
            if (Projectile.spriteDirection == -1)
                Projectile.rotation += MathHelper.Pi;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Main.npc[targetIndex].Center + Vector2.UnitY * 64, default,
                ModContent.ProjectileType<ThornTree_Proj>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 2);
        }
        base.Attack();
    }

    public override void ModifyDrawPlayer(Player drawPlayer)
    {
        var fPlr = FractalPlayer;
        int index = 29;
        var frame = fPlr.OldBodyFrame[index];
        if (frame != default)
            drawPlayer.bodyFrame = frame;
        frame = fPlr.OldLegFrame[index];
        if (frame != default)
            drawPlayer.legFrame = frame;
        if (Projectile.ai[0] == 0)
            drawPlayer.direction = fPlr.OldDirection[index];

        base.ModifyDrawPlayer(drawPlayer);
    }

    public override void Idle()
    {
        var target = FractalPlayer.OldPos[29];
        if (target != default)
            Projectile.Center = Vector2.Lerp(Projectile.Center, target, 0.5f);
        base.Idle();
    }
}

public class StoneSwordPlrProj : PlayerLikeProjectile
{
    public override void AI()
    {
        base.AI();
    }

    public override bool PreDraw(ref Color lightColor)
    {
        return base.PreDraw(ref lightColor);
    }

    public override void DrawSword()
    {
        var rotation = Projectile.rotation - MathHelper.Pi / 12 * Projectile.spriteDirection;// - MathHelper.PiOver4;
        if (Projectile.spriteDirection == -1)
            rotation += MathHelper.Pi;
        Main.spriteBatch.Draw(ModAsset.CrystalStoneSword_NewVer.Value,
            Projectile.Center - Main.screenPosition - Vector2.UnitX * 8 * Projectile.spriteDirection,
            null, Color.White * Projectile.Opacity, rotation, new Vector2(Projectile.spriteDirection == -1 ? 68 : 0, 100), 1,
            Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : 0, 0);
        base.DrawSword();
    }

    public override void Attack()
    {
        if (Projectile.ai[2] == 10)
        {
            Projectile.ai[2] = 0;
            var targetPos = Main.rand.NextVector2FromRectangle(Main.npc[targetIndex].Hitbox);
            var delta = targetPos - Projectile.Center;
            if (delta.Length() > 384)
                Projectile.Center += delta.SafeNormalize(default) * (delta.Length() + 256);
            Projectile.velocity = delta.SafeNormalize(default) * .01f;
            SoundEngine.PlaySound(MySoundID.Scythe);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, delta.SafeNormalize(default) * 16,
                ModContent.ProjectileType<StoneSAProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 2);

            var u = UltraSwoosh.NewUltraSwoosh(StoneSpecialAttack.CanvasName, 60, 150, Projectile.Center, (1.7f, -.2f));
            u.negativeDir = Projectile.velocity.X < 0;
            u.rotation = delta.ToRotation() + Main.rand.NextFloat(-1, 1);
            u.xScaler = Main.rand.NextFloat(1, 2);
            u.aniTexIndex = 3;
            u.baseTexIndex = 7;
            u.weaponTex = ModAsset.CrystalStoneSword_NewVer.Value;
        }
        Projectile.rotation = Projectile.velocity.ToRotation() + 4 * (MathF.Sin(MathHelper.SmoothStep(0, 1, Projectile.ai[2] / 10f) * MathHelper.PiOver2) + .25f) * .75f * MathF.Sign(Projectile.velocity.X);// (MathF.Sin(Projectile.ai[2] / 20f * MathHelper.TwoPi) * 4f - 4f);
        //if (Projectile.spriteDirection == -1)
        //    Projectile.rotation += MathHelper.Pi;
        base.Attack();
    }

    public override void ModifyDrawPlayer(Player drawPlayer)
    {
        var fPlr = FractalPlayer;
        int index = 19;
        var frame = fPlr.OldBodyFrame[index];
        if (frame != default)
            drawPlayer.bodyFrame = frame;
        frame = fPlr.OldLegFrame[index];
        if (frame != default)
            drawPlayer.legFrame = frame;
        if (Projectile.ai[0] == 0)
            drawPlayer.direction = fPlr.OldDirection[index];
        base.ModifyDrawPlayer(drawPlayer);
    }

    public override void Idle()
    {
        var target = FractalPlayer.OldPos[19];
        if (target != default)
            Projectile.Center = Vector2.Lerp(Projectile.Center, target, 0.5f);
        base.Idle();
    }
}

public class IronSwordPlrProj : PlayerLikeProjectile
{
    public override void AI()
    {
        base.AI();
    }

    public override bool PreDraw(ref Color lightColor)
    {
        return base.PreDraw(ref lightColor);
    }

    public override void DrawSword()
    {
        var rotation = Projectile.rotation - MathHelper.Pi / 12 * Projectile.spriteDirection;// - MathHelper.PiOver4;
        if (Projectile.spriteDirection == -1)
            rotation += MathHelper.Pi;
        Main.spriteBatch.Draw(ModAsset.RefinedSteelBlade_NewVer.Value,
            Projectile.Center - Main.screenPosition - Vector2.UnitX * 8 * Projectile.spriteDirection,
            null, Color.White * Projectile.Opacity, rotation, new Vector2(Projectile.spriteDirection == -1 ? 66 : 0, 74), 1,
            Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : 0, 0);
        base.DrawSword();
    }

    public override void Attack()
    {
        if (Projectile.ai[2] == 5)
        {
            Projectile.ai[2] = 0;
            var targetPos = Main.rand.NextVector2FromRectangle(Main.npc[targetIndex].Hitbox);
            var delta = targetPos - Projectile.Center;
            if (delta.Length() > 24)
                Projectile.Center += delta.SafeNormalize(default) * (delta.Length() + 16);
            Projectile.velocity = delta.SafeNormalize(default);
            SoundEngine.PlaySound(MySoundID.Scythe);

            var cen = targetPos;
            Projectile.NewProjectile(Projectile.GetItemSource_FromThis(), cen, delta.SafeNormalize(default).RotatedByRandom(MathHelper.Pi * 3) / 16f * Main.rand.NextFloat(160, 400) * Main.rand.NextFloat(-1, 1), ModContent.ProjectileType<SteelSAProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Main.rand.Next(-25, 26), cen.X, cen.Y);
        }
        Projectile.rotation = Projectile.velocity.ToRotation() + 4 * (MathF.Sin(MathHelper.SmoothStep(0, 1, Projectile.ai[2] / 5f) * MathHelper.PiOver2) + .25f) * .75f * MathF.Sign(Projectile.velocity.X);
        base.Attack();
    }

    public override void ModifyDrawPlayer(Player drawPlayer)
    {
        var fPlr = FractalPlayer;
        int index = 9;
        var frame = fPlr.OldBodyFrame[index];
        if (frame != default)
            drawPlayer.bodyFrame = frame;
        frame = fPlr.OldLegFrame[index];
        if (frame != default)
            drawPlayer.legFrame = frame;
        if (Projectile.ai[0] == 0)
            drawPlayer.direction = fPlr.OldDirection[index];
        base.ModifyDrawPlayer(drawPlayer);
    }

    public override void Idle()
    {
        var target = FractalPlayer.OldPos[9];
        if (target != default)
            Projectile.Center = Vector2.Lerp(Projectile.Center, target, 0.5f);
        base.Idle();
    }

    public override void OnKill(int timeLeft)
    {
        foreach (var proj in Main.projectile)
        {
            if (proj.active && proj.type == Type && proj.owner == Projectile.owner && proj.whoAmI != Projectile.whoAmI)
                return;
        }
        FractalPlayer.RecordOldDatas = false;
        base.OnKill(timeLeft);
    }
}