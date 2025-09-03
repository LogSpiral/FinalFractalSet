using FinalFractalSet.REAL_NewVersions.Pure;
using FinalFractalSet.REAL_NewVersions.Zenith;
using FinalFractalSet.Weapons.FinalFractal_Old;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing.RenderDrawingEffects;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Contents.Melee;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using System;
using System.Collections.Generic;
using Terraria.Localization;

namespace FinalFractalSet.REAL_NewVersions.Final;

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
        for (int n = 1; n < 4; n++)
        {
            tooltips.Add(new TooltipLine(Mod, "PureSuggestion", Language.GetOrRegister("Mods.FinalFractalSet.FinalFractalTip." + n).Value) { OverrideColor = Color.Lerp(new Color(99, 74, 187), new Color(20, 120, 118), (float)Math.Sin(MathHelper.Pi / 60 * (LogSpiralLibraryMod.ModTime + 40 * n)) / 2 + 0.5f) });
        }
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

    public static readonly IRenderEffect[][] RenderDrawInfos = [[new AirDistortEffect(16,1.5f, 0, 0.5f)],
                [new MaskEffect(LogSpiralLibraryMod.Mask[2].Value, Color.Violet, 0.15f, 0.2f, new Vector2((float)LogSpiralLibraryMod.ModTime), true, false),
                new DyeEffect(ItemID.StardustDye),
                new BloomEffect(0, 1f, 1, 3, true,2,true)]];

    public const string CanvasName = "FinalFractalSet:FinalFractal";

    public override void Load()
    {
        RenderCanvasSystem.RegisterCanvasFactory(CanvasName, () => new(RenderDrawInfos));
        base.Load();
    }

    public override void InitializeStandardInfo(StandardInfo standardInfo, VertexDrawStandardInfo vertexStandard)
    {
        standardInfo.standardColor = Color.MediumPurple * .5f;
        standardInfo.itemType = ModContent.ItemType<FinalFractal_NewVer>();

        vertexStandard.scaler = 140;
        vertexStandard.timeLeft = 45;
        vertexStandard.alphaFactor = 2f;
        vertexStandard.canvasName = CanvasName;
        base.InitializeStandardInfo(standardInfo, vertexStandard);
    }

    // 给左键第一斩用
    [SequenceDelegate]
    private static void FinalFractalChop(MeleeAction action)
    {
        Projectile.NewProjectile(
            action.Projectile.GetSource_FromThis(),
            action.Owner.Center,
            action.Rotation.ToRotationVector2() * 64,
            ModContent.ProjectileType<FinalFractalProjectile>(),//ModContent.ProjectileType<FractalStormSpawner>()
            action.Projectile.damage,
            action.Projectile.knockBack,
            action.Projectile.owner);
    }

    // 左键第二斩
    [SequenceDelegate]
    private static void FinalFractalCut(MeleeAction action)
    {
        Projectile.NewProjectile(
            action.Projectile.GetSource_FromThis(),
            action.Owner.Center + action.Rotation.ToRotationVector2() * 512,
            action.Rotation.ToRotationVector2(),
            ModContent.ProjectileType<FractalTear>(),
            action.Projectile.damage,
            action.Projectile.knockBack,
            action.Projectile.owner);

        PureFractal_NewVer_Proj.ShootPurefractalProj_Lots(action);

        VectorMethods.GetClosestVectorsFromNPC(action.Owner.Center, 3, 2048, out var indexs, out _);
        for (int n = 0; n < 3; n++)
            FirstZenith_NewVer_Proj.ShootFirstZenithViaPosition(action, indexs[n] == -1 ? Main.MouseWorld : Main.npc[indexs[n]].Center, indexs[n] == -1);
    }

    private static void ShootSinglgDash(MeleeAction action, float angle)
    {
        Vector2 unit = (action.Rotation + angle).ToRotationVector2();
        Projectile.NewProjectile(action.Projectile.GetSource_FromThis(),
action.Owner.Center, unit.RotatedBy(angle) * 32, ModContent.ProjectileType<FractalDash>(),
action.Projectile.damage, action.Projectile.knockBack, Main.myPlayer, 0, Main.rand.NextFloat(), 2);
    }

    [SequenceDelegate]
    private static void FinalFractalStab(MeleeAction action)
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