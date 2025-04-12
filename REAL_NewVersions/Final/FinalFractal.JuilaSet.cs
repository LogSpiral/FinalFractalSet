using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace FinalFractalSet.REAL_NewVersions.Final;
public class FractalJuilaSetProj : FinalFractalAssistantProjectile
{
    public override void SetDefaults()
    {
        Projectile.timeLeft = 300;
        Projectile.penetrate = -1;
        Projectile.friendly = true;
        Projectile.melee = true;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 0;
        base.SetDefaults();
    }
    /// <summary>
    /// 全图判定神人弹幕!!
    /// </summary>
    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => true;

    public override bool PreDraw(ref Color lightColor)
    {
        JuilaSetScene.Active = true;
        //JuilaSetSystem.DrawFractalImage();
        /*var spriteBatch = Main.spriteBatch;

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

        var effect = ModAsset.JuilaSet.Value;
        effect.Parameters["uConst"].SetValue(new Vector2(.845f,.85f));
        effect.Parameters["uRange"].SetValue(new Vector4(-8, -8, 8, 8));
        effect.CurrentTechnique.Passes[0].Apply();

        spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);*/
        return false;
    }
    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        Projectile.hide = true;
        behindNPCsAndTiles.Add(index);
        base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
    }
    public override void OnKill(int timeLeft)
    {
        if (Main.netMode != NetmodeID.Server)
        {
            SkyManager.Instance.Deactivate("FinalFractalSet:JuilaSetScene");
            Filters.Scene["FinalFractalSet:JuilaSetVortex"].Deactivate();
        }
        base.OnKill(timeLeft);
    }
    public override void OnSpawn(IEntitySource source)
    {
        if (Main.netMode != NetmodeID.Server)
            SkyManager.Instance.Activate("FinalFractalSet:JuilaSetScene");
        base.OnSpawn(source);
    }
    public override void AI()
    {
        if (Projectile.timeLeft < 270)
        {
            JuilaSetSystem.PendingActive = true;
            JuilaScreenVortexData.VortexCenter = Projectile.Center;
        }
        base.AI();
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        var unit = Main.rand.NextVector2Unit();
        var unit2 = Main.rand.NextVector2Unit();
        if (Main.rand.NextBool(10))
            SoundEngine.PlaySound(SoundID.Item60 with { MaxInstances = -1 }, target.position);

        for (int n = 0; n < 4; n++)
            OtherMethods.FastDust(target.Center + unit2 * Main.rand.NextFloat(0, 16) - unit * 32, unit * Main.rand.NextFloat(4, 16), Color.Cyan);
        base.OnHitNPC(target, hit, damageDone);
    }
}
public class JuilaSetSystem : ModSystem
{
    public static Effect FractalEffect => LogSpiralLibrary.ModAsset.Fractal.Value;
    public static RenderTarget2D render;
    public static RenderTarget2D renderSwap;
    public override void Load()
    {
        if (Main.netMode == NetmodeID.Server) return;

        Main.OnResolutionChanged += RebuildRenderTarget;
        Main.RunOnMainThread(() => RebuildRenderTarget(Main.instance.Window.ClientBounds.Size()));


        SkyManager.Instance["FinalFractalSet:JuilaSetScene"] = new JuilaSetScene();

        var data = new JuilaScreenVortexData(ModAsset.VortexTransform, "VortexTransform");
        Filters.Scene["FinalFractalSet:JuilaSetVortex"] = new Filter(data, EffectPriority.VeryHigh);


        base.Load();
    }

    public static bool PendingActive;
    public override void PreUpdateEntities()
    {
        if (Main.netMode == NetmodeID.Server) return;

        string name = "FinalFractalSet:JuilaSetVortex";
        if (!Filters.Scene[name].IsActive() && PendingActive)
        {
            Filters.Scene.Activate(name);
            PendingActive = false;
        }
        else if (Filters.Scene[name].IsActive() && !PendingActive)
        {
            Filters.Scene.Deactivate(name);
        }
        base.PreUpdateEntities();
    }


    private static void RebuildRenderTarget(Vector2 obj)
    {
        int width = (int)obj.X;
        int height = (int)obj.Y;
        Color[] colors = new Color[width * height];
        Array.Fill(colors, new Color(0.5f, 0.5f, 0));
        render?.Dispose();
        renderSwap?.Dispose();
        render = new RenderTarget2D(Main.graphics.GraphicsDevice, width, height);
        renderSwap = new RenderTarget2D(Main.graphics.GraphicsDevice, width, height);
        render.SetData(colors);
        renderSwap.SetData(colors);
    }

    public override void Unload()
    {
        if (Main.netMode == NetmodeID.Server) return;

        Main.RunOnMainThread(() =>
        {
            render?.Dispose();
            renderSwap?.Dispose();
        });

        base.Unload();
    }

    public static void UpdateFractal()
    {
        SpriteBatch spriteBatch = Main.spriteBatch;
        var gd = Main.instance.GraphicsDevice;
        gd.SetRenderTarget(render);//设置画布，将renderShift计算结果存在里面
        gd.Clear(Color.Transparent);
        spriteBatch.Draw(renderSwap, Vector2.Zero, Color.White);

        gd.SetRenderTarget(renderSwap);
        gd.Clear(Color.Transparent);
        spriteBatch.Draw(render, Vector2.Zero, Color.White);
    }

    public static void DrawFractalImage()
    {
        SpriteBatch spriteBatch = Main.spriteBatch;
        var gd = Main.instance.GraphicsDevice;
        if (Filters.Scene._captureThisFrame)
        {
            spriteBatch.End();
            spriteBatch.Begin();
            gd.SetRenderTarget(Main.screenTargetSwap);
            gd.Clear(Color.Transparent);
            spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
        }
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

        Vector2 t = VectorMethods.GetLerpValue(default, Main.ScreenSize.ToVector2(), Main.MouseScreen, true);
        t = VectorMethods.Lerp(new Vector2(-2, 2), new Vector2(2, -2), t, false);
        FractalEffect.Parameters["uM"].SetValue(t);
        FractalEffect.Parameters["uRange"].SetValue(new Vector4(-2, -2, 2, 2));
        FractalEffect.CurrentTechnique.Passes[2].Apply();
        RebuildRenderTarget(Main.ScreenSize.ToVector2());
        UpdateFractal();

        if (Filters.Scene._captureThisFrame)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            gd.SetRenderTarget(Main.screenTarget);
            gd.Clear(Color.Transparent);
            spriteBatch.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
        }
        else
            gd.SetRenderTarget(null);

        gd.Textures[1] = LogSpiralLibrary.ModAsset.HeatMap_1.Value;
        FractalEffect.CurrentTechnique.Passes[1].Apply();
        gd.SamplerStates[1] = SamplerState.AnisotropicClamp;
        spriteBatch.Draw(renderSwap, Vector2.Zero, Color.White);

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
            DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
    }
}
public class JuilaSetScene : CustomSky
{
    public static bool Active;

    public override void Activate(Vector2 position, params object[] args)
    {
        Active = true;
    }

    public override void Deactivate(params object[] args)
    {
        Active = false;
    }

    public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
    {
        spriteBatch.Draw(ModAsset.JuilaSetResult.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * Opacity);
    }

    public override bool IsActive() => !Main.gameMenu && (Active || Opacity > .001f);

    public override void Reset()
    {

    }

    public override void Update(GameTime gameTime)
    {
        Opacity = MathHelper.Lerp(Opacity, Active ? 1 : 0, 0.05f);
    }
}
public class JuilaScreenVortexData : ScreenShaderData
{
    public JuilaScreenVortexData(string passName) : base(passName)
    {
    }
    public JuilaScreenVortexData(Asset<Effect> shader, string passName) : base(shader, passName)
    {

    }
    public static Vector2 VortexCenter;
    public static float VortexFactor;

    public override void Apply()
    {
        float k = Main.screenWidth / (float)Main.screenHeight;
        var size = Main.ScreenSize.ToVector2();
        Vector2 vec = (VortexCenter - Main.screenPosition) / size - Vector2.One * .5f;
        vec *= Main.GameViewMatrix.Zoom.X;
        vec += Vector2.One * .50f;
        vec.X *= k;
        Shader.Parameters["uFactor"].SetValue(MathF.Pow(CombinedOpacity, 4) * 100);
        Shader.Parameters["uCenter"].SetValue(vec);
        Shader.Parameters["uKvalue"].SetValue(k);
        Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
        base.Apply();
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}