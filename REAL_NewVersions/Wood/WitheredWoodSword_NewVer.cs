using FinalFractalSet.Weapons;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Contents.Melee;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Contents.Melee.ExtendedMelee;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Terraria;
using Terraria.Audio;
using Terraria.Utilities;
using static Terraria.Localization.NetworkText;

namespace FinalFractalSet.REAL_NewVersions.Wood
{
    public class WitheredWoodSword_NewVer : MeleeSequenceItem<WitheredWoodSword_NewVer_Proj>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 15;
        }
        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            recipe.QuickAddIngredient(
            ItemID.WoodenSword,
            ItemID.BorealWoodSword,
            ItemID.PalmWoodSword,
            ItemID.RichMahoganySword,
            ItemID.ShadewoodSword,
            ItemID.AshWoodSword,
            ItemID.CactusSword);
            recipe.AddIngredient(ItemID.Mushroom, 50);
            recipe.AddIngredient(ItemID.GlowingMushroom, 50);
            recipe.AddIngredient(ItemID.Acorn, 50);
            recipe.AddIngredient(ItemID.BambooBlock, 15);
            recipe.AddTile(TileID.LivingLoom);
            recipe.ReplaceResult(this);
            recipe.Register();
            base.AddRecipes();
        }
        public override bool AltFunctionUse(Player player) => true;
    }
    public class WitheredWoodSword_NewVer_Proj : MeleeSequenceProj
    {
        public override bool LabeledAsCompleted => true;
        public override string Texture => base.Texture.Replace("_Proj", "");
        public override StandardInfo StandardInfo => base.StandardInfo with
        {
            standardColor = Color.SandyBrown * .25f,
            vertexStandard = new()
            {
                active = true,
                scaler = 90,
                timeLeft = 15,
                alphaFactor = 2f,
            },
            itemType = ModContent.ItemType<WitheredWoodSword_NewVer>()
        };
    }
    public class LivingWoodSword_NewVer : MeleeSequenceItem<LivingWoodSword_NewVer_Proj>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 50;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            recipe.AddIngredient<WitheredWoodSword_NewVer>();
            recipe.AddIngredient(ItemID.PearlwoodSword);
            recipe.AddIngredient(ItemID.BrokenHeroSword);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.ReplaceResult(this);
            recipe.Register();
        }
    }
    public class LivingWoodSword_NewVer_Proj : MeleeSequenceProj
    {
        public override bool LabeledAsCompleted => true;
        public override string Texture => base.Texture.Replace("_Proj", "");
        public override StandardInfo StandardInfo => base.StandardInfo with
        {
            standardColor = Color.LimeGreen * .5f,
            vertexStandard = new()
            {
                active = true,
                scaler = 100,
                timeLeft = 15,
                alphaFactor = 2f,
            },
            itemType = ModContent.ItemType<LivingWoodSword_NewVer>()
        };
        [SequenceDelegate]
        static void SpawnThorn(MeleeAction action)
        {
            Projectile.NewProjectile(action.Projectile.GetProjectileSource_FromThis(), action.Owner.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(1, 16), Main.rand.NextBool(4) ? ProjectileID.NettleBurstRight : ProjectileID.VilethornBase, action.CurrentDamage / 2, action.Projectile.knockBack, Main.myPlayer);
        }
    }
    public class WoodSpecialAttack : PunctureInfo
    {
        [ElementCustomData]
        [DefaultValue(false)]
        public bool Upgraded;

        public override string Category => "";

        public override void OnBurst(float fallFac)
        {
            Projectile.NewProjectile(Owner.GetSource_FromThis(), Owner.Center, Owner.direction * Vector2.UnitX * 4, ModContent.ProjectileType<WoodSAProjectile>(), CurrentDamage, 1, Owner is Player plr ? plr.whoAmI : Main.myPlayer, Upgraded ? 2 : 0);
            base.OnBurst(fallFac);
        }
    }
    public class WoodSAProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 300;
            Projectile.hide = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.height = 160;
            Projectile.width = 32;
            base.SetDefaults();
        }
        public override void AI()
        {
            try
            {
                Point point = Projectile.Center.ToTileCoordinates();
                if (point.X > 0 && point.X < Main.maxTilesX)
                {
                    int t = 0;
                    while (point.Y + t < Main.maxTilesY && t < 100)
                    {
                        t++;
                        var tile = Main.tile[point.X, point.Y + t];
                        if (tile.HasTile && Main.tileSolid[tile.TileType])
                            break;
                    }

                    while (point.Y + t > 0 && t > -100)
                    {
                        t--;
                        var tile = Main.tile[point.X, point.Y + t];
                        if (!tile.HasTile)
                            break;
                        if (!Main.tileSolid[tile.type])
                            break;
                    }

                    Projectile.Center += 16 * t * Vector2.UnitY;
                }
            }
            catch
            {

            }
            Projectile.ai[2]--;
            Projectile.friendly = Projectile.ai[2] <= 0;
            if (Projectile.timeLeft % 30 == 0)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Vector2.UnitY * 12, default, ModContent.ProjectileType<ThornTree_Proj>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner, Projectile.ai[0]);

            //if (Projectile.timeLeft % 5 == 0 && Main.rand.NextBool(2))
            //{
            //    var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitY * -16, ProjectileID.NettleBurstRight, Projectile.damage, Projectile.knockBack, Projectile.owner);
            //    proj.tileCollide = false;
            //}
            if (Projectile.timeLeft % 10 == 0)
                Collision.HitTiles(Projectile.Center, default, 32, 32);

            base.AI();
        }
        public override void OnKill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Vector2.UnitY * 12, default, ModContent.ProjectileType<ThornTree_Proj>(), Projectile.damage * 2 / 3, Projectile.knockBack, Projectile.owner, Projectile.ai[0] + 1);
            base.OnKill(timeLeft);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[2] = 15;
            Projectile.timeLeft -= 60;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Vector2.UnitY * 12, default, ModContent.ProjectileType<ThornTree_Proj>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner, Projectile.ai[0]);
            base.OnHitNPC(target, hit, damageDone);
        }
        public override string Texture => base.Texture.Replace("WoodSAProjectile", "WitheredWoodSword_NewVer");
    }
    public class WitheredTree_NewVer : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.timeLeft = 60;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        public override string Texture => "Terraria/Images/Item_1";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void OnKill(int timeLeft)
        {
            tree.SpawnProjectile(Projectile, Projectile.Center, new Vector2(0, -1), tree.root, -.15f);
        }
        public override void AI()
        {
            Projectile.ai[1]++;
            if (Projectile.timeLeft == 1)
            {
                tree.SpawnDust(Projectile.Center, new Vector2(0, -1));
            }
        }
        private LightTree tree;
        public override void OnSpawn(IEntitySource source)
        {
            tree = new LightTree(Main.rand);
            tree.Generate(Projectile.Center, new Vector2(0, -.5f), new Vector2(0, -2048) + Projectile.Center, ((int)Projectile.ai[0] % 2 == 0 ? 128 : 256) * (tree.rand() * .25f + .75f), Projectile.ai[0] > 1);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            tree?.Draw(Main.spriteBatch, Projectile.Center - Main.screenPosition, new Vector2(0, -1), Lighting.GetColor((Projectile.Center / 16f).ToPoint()), 16f, Projectile.ai[1] / 2f);
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            tree?.Check(targetHitbox);
            return false;
        }
    }
    public class WitheredWood_NewVer : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_1";

        public override void SetDefaults()
        {
            Projectile.timeLeft = 60;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
        }
        public LightTree tree;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (tree?.Check(targetHitbox, Projectile.Center, Projectile.rotation.ToRotationVector2(), 10) == true)
            {
                if (Projectile.penetrate == 1)
                {
                    tree.SpawnDust(Projectile.Center, Projectile.rotation.ToRotationVector2());
                    if (tree.rand() < .5f)
                    {
                        tree.SpawnProjectile(Projectile, Projectile.Center, Projectile.rotation.ToRotationVector2(), tree.root, .05f);
                    }
                }

                return true;
            }
            return false;
        }
        public override void AI()
        {
            Projectile.velocity += new Vector2(0, .5f);
            if (Projectile.ai[0] > 0 && Projectile.ai[1] != -1 && Main.npc[(int)Projectile.ai[1]].active)
            {
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, (Main.npc[(int)Projectile.ai[1]].Center - Projectile.Center).SafeNormalize(default) * 32, .025f);
            }
            Projectile.rotation += .05f;
            for (int n = 9; n > 0; n--)
            {
                Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                Projectile.oldRot[n] = Projectile.oldRot[n - 1];
            }
            Projectile.oldPos[0] = Projectile.Center;
            Projectile.oldRot[0] = Projectile.rotation;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            for (int n = 9; n > -1; n--)
            {
                var c = Lighting.GetColor((Projectile.Center / 16).ToPoint(), Color.White);// n == 0 ? Color.White : color
                if (n != 0)
                {
                    var fac = (1 - n * .1f) * .5f;
                    c = c * fac * fac;
                    c.A = (byte)(c.A * (9 - n) / 9f);
                }
                tree?.Draw(Main.spriteBatch, Projectile.oldPos[n] - Main.screenPosition, Projectile.oldRot[n].ToRotationVector2(), c * MathHelper.Clamp(Projectile.timeLeft / 30f - 1, 0, 1), 16f, 10);
            }
            return false;
        }
    }

    public class ThornTree_Proj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.timeLeft = 60;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        public override string Texture => "Terraria/Images/Item_1";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void OnKill(int timeLeft)
        {
        }
        ThornTree thornTree;
        ThornTree thornTreeTiny;
        List<CustomVertexInfo> vertexs;
        List<Vector4> endNodes;
        float Factor => (60 - Projectile.timeLeft) / 60f;
        bool huge;
        public override void AI()
        {
            //Projectile.ai[1]++;
            float k = (1 - MathF.Cos(MathHelper.TwoPi * MathF.Sqrt(Factor))) * .5f;
            vertexs = thornTree.GetTreeVertex(huge ? -MathHelper.Pi / 4 * 3 : -MathHelper.Pi / 3 * 2, Projectile.Center - Vector2.UnitX * 8, out endNodes, k);
            vertexs.AddRange(thornTreeTiny.GetTreeVertex(huge ? -MathHelper.Pi / 4 : -MathHelper.Pi / 3, Projectile.Center + Vector2.UnitX * 8, out var anotherNodes, k));
            endNodes.AddRange(anotherNodes);


            if (Projectile.timeLeft == 45 && Projectile.ai[0] > 1)
            {
                foreach (var vec in endNodes)
                {
                    for (int n = 0; n < 5; n++)
                        Dust.NewDustPerfect(new Vector2(vec.X, vec.Y), MyDustId.GreenGrass);
                    Gore.NewGore(new Vector2(vec.X, vec.Y), -vec.Z.ToRotationVector2() * 4, GoreID.TreeLeaf_Normal, 1);

                }
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            float stdSize = 64;
            huge = (int)Projectile.ai[0] % 2 == 1;
            if (huge) stdSize *= 2;
            if (Projectile.ai[0] > 1) stdSize *= 1.2f;

            bool f = Main.rand.NextBool();
            if (f)
                stdSize *= .75f;

            thornTree = new ThornTree(stdSize, stdSize * .25f, 0, new()
            {
                lengthScaler = .7f,
                widthScaler = .4f,
                stdRotation = MathHelper.Pi / 3,
                maxChildrens = 4,
                chanceToDecreaseChildren = .45f,
                mainBranchOffsetLength = .75f,
                mainBranchOffsetWidth = .9f,
                mainBranchOffsetRotationScaler = .25f,
                mainBranchExtraTier = 4,
                mainBranchOffsetChildren = .5f,
                fixedRotation = huge ? .33f : .25f
            });//
            thornTree.BuildTree(Main.rand, 8);
            if (f)
                stdSize *= 1.33f;
            thornTreeTiny = new ThornTree(stdSize, stdSize * .25f, 0, new()
            {
                lengthScaler = .7f,
                widthScaler = .4f,
                stdRotation = MathHelper.Pi / 3,
                maxChildrens = 4,
                chanceToDecreaseChildren = .45f,
                mainBranchOffsetLength = .75f,
                mainBranchOffsetWidth = .9f,
                mainBranchOffsetRotationScaler = .25f,
                mainBranchExtraTier = 4,
                mainBranchOffsetChildren = .5f,
                fixedRotation = -(huge ? .33f : .25f)
            });//
            thornTreeTiny.BuildTree(Main.rand, 8);
            //vertexs = thornTree.GetTreeVertex(-MathHelper.Pi / 3 * 2, Projectile.Center);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (vertexs == null)
                return false;

            Main.spriteBatch.ReBegin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            Main.graphics.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            var swooshUL = LogSpiralLibraryMod.ShaderSwooshUL;
            swooshUL.Parameters["uTransform"].SetValue(VertexDrawInfo.uTransform);
            var sampler = SamplerState.AnisotropicWrap;
            Main.graphics.GraphicsDevice.SamplerStates[0] = sampler;
            Main.graphics.GraphicsDevice.SamplerStates[1] = sampler;
            Main.graphics.GraphicsDevice.SamplerStates[2] = sampler;
            Main.graphics.GraphicsDevice.SamplerStates[3] = SamplerState.AnisotropicWrap;
            Main.graphics.GraphicsDevice.Textures[0] = ModAsset.WoodTile.Value;
            Main.graphics.GraphicsDevice.Textures[1] = LogSpiralLibraryMod.BaseTex_Swoosh[8].Value;
            Main.graphics.GraphicsDevice.Textures[2] = LogSpiralLibraryMod.BaseTex_Swoosh[8].Value;
            Main.graphics.GraphicsDevice.Textures[3] = LogSpiralLibraryMod.BaseTex_Swoosh[8].Value;
            swooshUL.CurrentTechnique.Passes[7].Apply();

            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
            Main.graphics.GraphicsDevice.Textures[0] = ModAsset.WoodTile.Value;
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            Main.instance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexs.ToArray(), 0, vertexs.Count / 3);

            Main.spriteBatch.ReBegin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            try
            {
                Main.instance.LoadGore(385);
            }
            catch { }
            if (Projectile.timeLeft > 30 && Projectile.ai[0] > 1 && endNodes != null)
            {
                foreach (var vec4 in endNodes)
                {
                    var vec = new Vector2(vec4.X, vec4.Y);

                    Main.spriteBatch.Draw(TextureAssets.Gore[385].Value, vec - Main.screenPosition, null, Lighting.GetColor(vec.ToTileCoordinates()).MultiplyRGB(Color.Cyan), vec4.Z, new Vector2(14), (1 - MathF.Cos(MathHelper.TwoPi * MathF.Sqrt(Factor * 2f))) * .5f * vec4.W, SpriteEffects.FlipHorizontally, 0);
                }
            }
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (vertexs == null) return false;
            int m = vertexs.Count;
            for (int n = 0; n < m / 10; n++)
            {
                if (targetHitbox.Contains(vertexs[Main.rand.Next(m)].Position.ToPoint()))
                    return true;
            }
            return false;
        }
    }
    public class Thorn_Proj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.timeLeft = 60;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            for (int n = 9; n > -1; n--)
            {
                var c = Lighting.GetColor((Projectile.Center / 16).ToPoint(), Color.White);// n == 0 ? Color.White : color
                if (n != 0)
                {
                    var fac = (1 - n * .1f) * .5f;
                    c = c * fac * fac;
                    c.A = (byte)(c.A * (9 - n) / 9f);
                }
                Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition, null, c * MathHelper.Clamp(Projectile.timeLeft / 30f - 1, 0, 1), Projectile.oldRot[n], new Vector2(0, 14), 16f, 0, 0);
            }
            return false;
        }
        public override void AI()
        {
            base.AI();
        }
    }
    public class ThornTree
    {
        public struct TreeGenerateInfo
        {
            public float lengthScaler;
            public float widthScaler;
            public float stdRotation;
            public int maxChildrens;
            public float chanceToDecreaseChildren;

            public float mainBranchOffsetLength;
            public float mainBranchOffsetWidth;
            public float mainBranchOffsetRotationScaler;
            public int mainBranchExtraTier;
            public float mainBranchOffsetChildren;

            /// <summary>
            /// 固定旋转量
            /// </summary>
            public float fixedRotation;
        }
        class Node(float length, float width, float rotation, bool main)
        {
            /// <summary>
            /// 子节点
            /// </summary>
            HashSet<Node> Children;

            /// <summary>
            /// 节点始端到末端的长度
            /// </summary>
            float length = length;

            /// <summary>
            /// 节点末端的宽度
            /// </summary>
            float width = width;

            /// <summary>
            /// 相对于父节点的旋转量
            /// </summary>
            float rotation = rotation;

            /// <summary>
            /// 是否处于主干
            /// </summary>
            bool mainBranch = main;

            float getNormalRandom(UnifiedRandom random, float factor) => (float)random.GaussianRandom(factor, factor * .25 * .33);

            public void Generate(UnifiedRandom random, TreeGenerateInfo info, int maxTier, out int depth)
            {
                depth = 1;
                if (maxTier != 0 && length > 1 && width > 1)
                {
                    int r = (int)(random.GaussianRandom(info.maxChildrens * .75f, info.maxChildrens * .25 * .33));
                    Children = [];
                    for (int n = 0; n < r; n++)
                    {
                        Node node = new(
                            length * MathHelper.Lerp(getNormalRandom(random, info.lengthScaler), 1, mainBranch ? info.mainBranchOffsetLength : 0),
                            width * MathHelper.Lerp(getNormalRandom(random, info.widthScaler), 1, mainBranch ? info.mainBranchOffsetWidth : 0),
                            (float)random.GaussianRandom(0, info.stdRotation * .33f) + info.fixedRotation,
                            n == 0);

                        Children.Add(node);
                        if (random.NextDouble() < info.chanceToDecreaseChildren * (mainBranch ? info.mainBranchOffsetChildren : 1))
                            info = info with { maxChildrens = info.maxChildrens - 1 };
                        node.Generate(random, info, maxTier - 1, out int curDepth);
                        if (curDepth > depth)
                            depth = curDepth;
                    }
                    depth++;
                }
            }

            public void AddToVertex(IList<CustomVertexInfo> vertexInfos, IList<Vector4> EndNodes, float parentWidth, float parentRotation, Vector2 nodePoint, Color color, float factor = 1f)
            {
                if (parentWidth == 0) parentWidth = width;
                float realRotation = rotation + parentRotation;

                Vector2 unit = realRotation.ToRotationVector2();
                Vector2 normalUnit = new Vector2(-unit.Y, unit.X);
                CustomVertexInfo[] results = new CustomVertexInfo[4];

                if (length * width > 64f)
                    color = Lighting.GetColor(nodePoint.ToTileCoordinates());

                if (MathF.Abs(rotation) < MathHelper.PiOver2)
                    nodePoint += MathF.Sign(rotation) * (normalUnit - (parentRotation + MathHelper.PiOver2).ToRotationVector2()) * parentWidth * .5f;



                float u = MathHelper.Clamp(factor, 0, 1);
                float realWidth = width * u * u;
                normalUnit *= .5f;
                results[0] = new CustomVertexInfo(nodePoint + normalUnit * parentWidth, color, new Vector3(0, 0, 1));
                results[1] = new CustomVertexInfo(nodePoint - normalUnit * parentWidth, color, new Vector3(1, 0, 1));
                nodePoint += length * unit * u * (2 - u);
                results[2] = new CustomVertexInfo(nodePoint + normalUnit * realWidth, color, new Vector3(0, 1, 1));
                results[3] = new CustomVertexInfo(nodePoint - normalUnit * realWidth, color, new Vector3(1, 1, 1));

                vertexInfos.Add(results[0]);
                vertexInfos.Add(results[1]);
                vertexInfos.Add(results[2]);

                vertexInfos.Add(results[1]);
                vertexInfos.Add(results[2]);
                vertexInfos.Add(results[3]);

                if (Children != null)
                    foreach (var c in Children)
                        c.AddToVertex(vertexInfos, EndNodes, realWidth, realRotation, nodePoint, color, factor - .5f);
                else
                    EndNodes.Add(new Vector4(nodePoint, realRotation, (float)Main.rand.GaussianRandom(1, 0.16f)));
            }

        }

        Node mainNode;
        TreeGenerateInfo genInfo;
        int depth;

        public ThornTree(float length, float width, float rotation, TreeGenerateInfo info)
        {
            genInfo = info;
            mainNode = new(length, width, rotation, false);
        }

        public void BuildTree(UnifiedRandom random, int maxTier)
        {
            mainNode.Generate(random, genInfo, maxTier, out depth);
        }

        public List<CustomVertexInfo> GetTreeVertex(float rotation, Vector2 start, out List<Vector4> EndNodes, float factor = 1f)
        {
            List<CustomVertexInfo> vertexInfos = [];
            EndNodes = [];
            mainNode.AddToVertex(vertexInfos, EndNodes, 0, rotation, start, Lighting.GetColor(start.ToTileCoordinates()), factor * depth * .5f);
            return vertexInfos;
        }
    }
}
