using FinalFractalSet.REAL_NewVersions.Zenith;
using FinalFractalSet.Weapons.FinalFractal_Old;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using static Terraria.Utils;
namespace FinalFractalSet.REAL_NewVersions.Final;

public class FinalFractalAssistantProjectile : ModProjectile
{
    public const string FractalTexturePath = "FinalFractalSet/REAL_NewVersions/Final/FinalFractalProjectile";
    public override string Texture => FractalTexturePath;
}
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

        if (Main.netMode == NetmodeID.Server) return;
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

        //if (Main.netMode == NetmodeID.Server) return;
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
    public override bool IsLoadingEnabled(Mod mod) => FinalFractalSetConfig.OldVersionEnabled;
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
    UltraSwoosh swoosh;
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
        return;
        if (Main.netMode == NetmodeID.Server) return;
        if (swoosh == null)
        {
            swoosh = UltraSwoosh.NewUltraSwoosh(Color.Violet, 30, 1, null, ModAsset.bar_19.Value, colorVec: new(0.16667f, 0.33333f, 0.5f));
            swoosh.autoUpdate = false;
            swoosh.ModityAllRenderInfo(FinalFractal_NewVer_Proj.RenderDrawInfos);
            swoosh.weaponTex = TextureAssets.Item[Main.player[Projectile.owner].HeldItem.type].Value;
        }
        if (swoosh != null)
            swoosh.autoUpdate = false;
        for (int n = 44; n > 0; n--)
        {
            Projectile.oldPos[n] = Projectile.oldPos[n - 1];
            Projectile.oldRot[n] = Projectile.oldRot[n - 1];
        }
        Projectile.oldPos[0] = Projectile.Center;
        Projectile.oldRot[0] = Projectile.rotation;//Projectile.velocity.ToRotation() + Projectile.ai[0] * (Projectile.localAI[0] / 60).Lerp(-180, 90, true);
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
                    Vector2 vec = Projectile.oldPos[n] + (Projectile.oldRot[n]).ToRotationVector2() * 116;
                    swoosh.VertexInfos[2 * n] = new(vec, c, new(k, 1, 1));
                    swoosh.VertexInfos[2 * n + 1] = new(Projectile.oldPos[n], c, new(0, 0, 1));
                }
                return;
            default:
                int t = (int)Projectile.localAI[0];
                Vector2[] vecOuter = new Vector2[t];
                Vector2[] vecInner = new Vector2[t];
                for (int n = 0; n < t; n++)
                {
                    vecOuter[n] = Projectile.oldPos[n] + (Projectile.oldRot[n]).ToRotationVector2() * 116;
                    vecInner[n] = Projectile.oldPos[n];
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
        //if (Main.netMode == NetmodeID.Server) return;
        //swoosh.timeLeft = 0;
    }
    public override bool PreDraw(ref Color lightColor)
    {
        SpriteBatch spriteBatch = Main.spriteBatch;
        var tex = TextureAssets.Projectile[Projectile.type].Value;
        spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, tex.Frame(),
            Color.White, Projectile.rotation, tex.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
        return false;
    }
}

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
    UltraSwoosh swoosh;
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
        if (Main.netMode == NetmodeID.Server) return;
        if (swoosh == null)
        {
            swoosh = UltraSwoosh.NewUltraSwoosh(Color.Violet, 30, 240, Main.player[Projectile.owner].Center + Projectile.velocity * 64, ModAsset.bar_19.Value, Projectile.velocity.X < 0, Projectile.velocity.ToRotation(), 3, colorVec: new(0.16667f, 0.33333f, 0.5f));
            swoosh.autoUpdate = false;
            swoosh.ModityAllRenderInfo(FinalFractal_NewVer_Proj.RenderDrawInfos);
            swoosh.weaponTex = TextureAssets.Item[Main.player[Projectile.owner].HeldItem.type].Value;
            //SoundEngine.PlaySound(MySoundID.Scythe, Projectile.Center);
        }
        if (Projectile.timeLeft > 45)
            swoosh.timeLeft = 2 * (60 - Projectile.timeLeft);
        else
            swoosh.timeLeft++;

        base.AI();
    }
    public override bool PreDraw(ref Color lightColor) => false;
    public override bool ShouldUpdatePosition() => false;
}
