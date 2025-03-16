using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Melee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace FinalFractalSet.REAL_NewVersions.Final
{
    public class FinalFractal_NewVer : MeleeSequenceItem<FinalFractal_NewVer_Proj>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 350;
        }
        public override void AddRecipes()
        {

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
}
