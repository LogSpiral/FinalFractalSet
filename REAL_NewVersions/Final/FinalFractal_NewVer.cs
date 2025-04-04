using FinalFractalSet.REAL_NewVersions.Pure;
using FinalFractalSet.REAL_NewVersions.Zenith;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Contents.Melee;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Contents.Melee.StandardMelee;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Core;
using System;
using System.Collections.Generic;
using System.IO;

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
        public override string Texture => base.Texture.Replace("_Proj", "");
        public override StandardInfo StandardInfo => base.StandardInfo with
        {
            standardColor = Color.MediumPurple * .5f,
            vertexStandard = new()
            {
                active = true,
                scaler = 140,
                timeLeft = 45,
                alphaFactor = 2f,
                renderInfos = [[new AirDistortEffectInfo(16, 0, 0.5f)],
                    [new MaskEffectInfo(LogSpiralLibraryMod.Mask[2].Value, Color.Violet, 0.15f, 0.2f, new Vector2((float)LogSpiralLibraryMod.ModTime), true, false),
                    new ArmorDyeInfo(ItemID.StardustDye),
                    new BloomEffectInfo(0, 1f, 1, 3, true) { useModeMK = true, downSampleLevel = 2 }]]
            },
            itemType = ModContent.ItemType<FinalFractal_NewVer>()
        };
    }
    public class FractalCloudMowingSword : FinalFractalSetAction
    {
        UltraSwoosh swoosh;
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
    public class FractalStrom : FinalFractalSetAction
    {
        UltraSwoosh swoosh;
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
    public class FractalTreeStab : FinalFractalSetAction
    {
        UltraStab stab;
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
    public class FractalChargingWing : ChargingInfo
    {

        public override string Category => "";
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
