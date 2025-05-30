using System.Collections.Generic;
using Terraria.Localization;
using static Terraria.Utils;
using Terraria.Enums;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using Terraria.Audio;
using System.IO;
using Terraria.ID;
using FinalFractalSet.REAL_NewVersions.Wood;
using FinalFractalSet.REAL_NewVersions.Stone;
using FinalFractalSet.REAL_NewVersions.Iron;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing.RenderDrawingContents;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing.RenderDrawingEffects;
using LogSpiralLibrary.CodeLibrary.Utilties;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using LogSpiralLibrary.CodeLibrary.Utilties.BaseClasses;
using FinalFractalSet.REAL_NewVersions.Zenith;

namespace FinalFractalSet.Weapons
{
    public class FirstFractal_Remastered : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 24;
            Item.height = 24;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
            Item.noMelee = true;
            Item.useAnimation = 35;
            Item.useTime = Item.useAnimation / 5;
            Item.shootSpeed = 16f;
            Item.damage = 240;
            Item.knockBack = 6.5f;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.crit = 10;
            Item.rare = ItemRarityID.Red;
            Item.shoot = ProjectileID.FirstFractal;
            Item.glowMask = 271;
        }
        private bool GetSparkleGuitarTarget(out List<NPC> validTargets)
        {
            Player player = Main.player[Main.myPlayer];
            validTargets = [];
            Rectangle value = CenteredRectangle(player.Center, new Vector2(1000f, 800f));
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy(this, false) && npc.Hitbox.Intersects(value))
                {
                    validTargets.Add(npc);
                }
            }
            return validTargets.Count != 0;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 value5 = Main.MouseWorld;
            List<NPC> list2;
            bool sparkleGuitarTarget2 = GetSparkleGuitarTarget(out list2);
            if (sparkleGuitarTarget2)
            {
                NPC NPC2 = list2[Main.rand.Next(list2.Count)];
                value5 = NPC2.Center + NPC2.velocity * 20f;
            }
            Vector2 vector32 = value5 - player.Center;
            Vector2 vector33 = Main.rand.NextVector2CircularEdge(1f, 1f);
            float num78 = 1f;
            int num79 = 1;
            for (int num80 = 0; num80 < num79; num80++)
            {
                if (!sparkleGuitarTarget2)
                {
                    value5 += Main.rand.NextVector2Circular(24f, 24f);
                    if (vector32.Length() > 700f)
                    {
                        vector32 *= 700f / vector32.Length();
                        value5 = player.Center + vector32;
                    }
                    float num81 = GetLerpValue(0f, 6f, player.velocity.Length(), true) * 0.8f;
                    vector33 *= 1f - num81;
                    vector33 += player.velocity * num81;
                    vector33 = vector33.SafeNormalize(Vector2.UnitX);
                }
                float num82 = 60f;
                float num83 = Main.rand.NextFloatDirection() * 3.14159274f * (1f / num82) * 0.5f * num78;
                float num84 = num82 / 2f;
                float scaleFactor3 = 12f + Main.rand.NextFloat() * 2f;
                Vector2 vector34 = vector33 * scaleFactor3;
                Vector2 vector35 = new Vector2(0f, 0f);
                Vector2 vector36 = vector34;
                int num85 = 0;
                while (num85 < num84)
                {
                    vector35 += vector36;
                    vector36 = vector36.RotatedBy(num83, default);
                    num85++;
                }
                Vector2 value6 = -vector35;
                Vector2 position1 = value5 + value6;
                float lerpValue2 = GetLerpValue(player.itemAnimationMax, 0f, player.itemAnimation, true);
                Projectile.NewProjectile(source, position1, vector34, type, damage, knockback, player.whoAmI, num83, lerpValue2);
            }
            return false;
        }
        public override void AddRecipes()
        {
            if (FinalFractalSetConfig.instance.LoadOldVersionWeapons)
            {
                Recipe recipe = CreateRecipe();
                recipe.QuickAddIngredient(
                ItemID.TerraBlade,
                ItemID.Meowmere,
                ItemID.StarWrath,
                ItemID.InfluxWaver,
                ItemID.TheHorsemansBlade,
                ItemID.Seedler,
                ItemID.EnchantedSword,
                ItemID.BeeKeeper,
                ItemID.Starfury,
                ItemID.CopperShortsword);
                recipe.AddTile(TileID.MythrilAnvil);
                recipe.ReplaceResult(this);
                recipe.Register();

                recipe = CreateRecipe();//Recipe
                recipe.AddIngredient<LivingWoodSword_Old>();
                recipe.AddIngredient<MossStoneSword_Old>();
                recipe.AddIngredient<RefinedSteelBlade_Old>();
                recipe.QuickAddIngredient(3258, 3823, 676, 3106, 671, 1928, 3827, 4923);

                recipe.AddTile(TileID.LunarCraftingStation);
                recipe.ReplaceResult(this);
                recipe.Register();

                recipe = CreateRecipe();
                recipe.AddIngredient(ItemID.Zenith);
                recipe.ReplaceResult(this);
                recipe.Register();

                recipe = CreateRecipe();
                recipe.AddIngredient(this);
                recipe.ReplaceResult(ItemID.Zenith);
                recipe.Register();
            }
            CreateRecipe()
                .AddIngredient<LivingWoodSword_NewVer>()
                .AddIngredient<CrystalStoneSword_NewVer>()
                .AddIngredient<RefinedSteelBlade_NewVer>()
                .AddTile(TileID.LunarCraftingStation)
                .QuickAddIngredient
                (
                    ItemID.SlapHand, ItemID.DD2SquireDemonSword,
                    ItemID.Frostbrand, ItemID.PsychoKnife, ItemID.Keybrand,
                    ItemID.ChristmasTreeSword, ItemID.DD2SquireBetsySword, ItemID.PiercingStarlight
                )
                .Register();
        }
    }
    public class FirstZenith_Old : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod) => FinalFractalSetConfig.OldVersionEnabled;
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Item.ShaderItemEffectInventory(spriteBatch, position, origin, LogSpiralLibraryMod.Misc[0].Value, Color.Lerp(new Color(99, 74, 187), new Color(20, 120, 118), (float)Math.Sin(MathHelper.Pi / 60 * LogSpiralLibraryMod.ModTime) / 2 + 0.5f), scale);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.ShaderItemEffectInWorld(spriteBatch, LogSpiralLibraryMod.Misc[0].Value, Color.Lerp(new Color(99, 74, 187), new Color(20, 120, 118), (float)Math.Sin(MathHelper.Pi / 60 * LogSpiralLibraryMod.ModTime) / 2 + 0.5f), rotation);
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 58;
            Item.height = 64;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
            Item.noMelee = true;
            Item.useAnimation = 40;
            Item.useTime = Item.useAnimation / 10;// 
            Item.shootSpeed = 16f;
            Item.damage = 500;
            Item.knockBack = 6.5f;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.crit = 31;
            Item.rare = ItemRarityID.Purple;
            Item.shoot = ModContent.ProjectileType<FirstZenithProj>();
            //Item.glowMask = 271;
        }
        public static bool GetSparkleGuitarTarget(out List<NPC> validTargets)
        {
            Player player = Main.player[Main.myPlayer];
            validTargets = [];
            Rectangle value = CenteredRectangle(player.Center, new Vector2(1000f, 800f));
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy(false) && npc.Hitbox.Intersects(value))
                {
                    validTargets.Add(npc);
                }
            }
            return validTargets.Count != 0;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 value5 = Main.MouseWorld;
            List<NPC> list2;
            bool sparkleGuitarTarget2 = GetSparkleGuitarTarget(out list2);
            if (sparkleGuitarTarget2)
            {
                NPC NPC2 = list2[Main.rand.Next(list2.Count)];
                value5 = NPC2.Center + NPC2.velocity * 20f;
            }
            Vector2 vector32 = value5 - player.Center;
            Vector2 vector33 = Main.rand.NextVector2CircularEdge(1f, 1f);
            float num78 = 1f;
            int num79 = 1;
            for (int num80 = 0; num80 < num79; num80++)
            {
                if (!sparkleGuitarTarget2)
                {
                    value5 += Main.rand.NextVector2Circular(24f, 24f);
                    if (vector32.Length() > 700f)
                    {
                        vector32 *= 700f / vector32.Length();
                        value5 = player.Center + vector32;
                    }
                    float num81 = GetLerpValue(0f, 6f, player.velocity.Length(), true) * 0.8f;
                    vector33 *= 1f - num81;
                    vector33 += player.velocity * num81;
                    vector33 = vector33.SafeNormalize(Vector2.UnitX);
                }
                float num82 = 60f;
                float num83 = Main.rand.NextFloatDirection() * 3.14159274f * (1f / num82) * 0.5f * num78;
                float num84 = num82 / 2f;
                float scaleFactor3 = 12f + Main.rand.NextFloat() * 2f;
                Vector2 vector34 = vector33 * scaleFactor3;
                Vector2 vector35 = new Vector2(0f, 0f);
                Vector2 vector36 = vector34;
                int num85 = 0;
                while (num85 < num84)
                {
                    vector35 += vector36;
                    vector36 = vector36.RotatedBy(num83, default);
                    num85++;
                }
                Vector2 value6 = -vector35;
                Vector2 position1 = value5 + value6;
                float lerpValue2 = GetLerpValue(player.itemAnimationMax, 0f, player.itemAnimation, true);
                Projectile.NewProjectile(source, position1, vector34, type, damage, knockback, player.whoAmI, num83, lerpValue2);
            }
            return false;
        }
        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Zenith);
            recipe.AddIngredient<FirstFractal_Remastered>();
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient<PureFractal_Old>();
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(this);
            recipe.ReplaceResult<PureFractal_Old>();
            recipe.Register();
        }
    }
    public class FirstZenithProj : ModProjectile
    {
        //public override bool IsLoadingEnabled(Mod mod) => FinalFractalSetConfig.OldVersionEnabled;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[projectile.owner] = 0;
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP) target.immune = false;
            base.OnHitPlayer(target, info);
        }
        Projectile projectile => Projectile;
        public Player drawPlayer;
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.DamageType = DamageClass.Melee;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 3;
            projectile.usesLocalNPCImmunity = true;
            projectile.manualDirectionChange = true;
            projectile.penetrate = -1;
            //ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 45;
            base.SetStaticDefaults();
        }
        public float drawColor => projectile.ai[1];
        UltraSwoosh swoosh;
        public override void OnSpawn(IEntitySource source)
        {
            projectile.frame = Main.rand.Next(15);
            drawPlayer = new Player();
            if (Main.netMode == NetmodeID.Server) return;

            var u = swoosh = UltraSwoosh.NewUltraSwoosh(FirstZenith_NewVer_Proj.CanvasName, 30, 1, default, (-1.125f, 0.7125f));
            u.heatMap = ModAsset.bar_19.Value;
            u.ColorVector = new(0.16667f, 0.33333f, 0.5f);
            u.weaponTex = TextureAssets.Item[Main.player[projectile.owner].HeldItem.type].Value;
            u.autoUpdate = false;
        }
        public override void AI()
        {
            //if (Main.player[projectile.owner].name == "FFT") 
            //{
            //    projectile.extraUpdates = 3;
            //}
            Main.projFrames[projectile.type] = 15;
            float num = 60f;
            projectile.localAI[0]++;
            if (projectile.localAI[0] >= num)
            {
                //projectile.Kill();
                //return;
                projectile.position -= projectile.velocity;
                projectile.Opacity = 0;
                if (projectile.localAI[0] >= 90)
                {
                    projectile.Kill();
                    return;
                }
            }
            else
            {
                projectile.velocity = projectile.velocity.RotatedBy((double)projectile.ai[0], default(Vector2));
                projectile.Opacity = GetLerpValue(0f, 12f, projectile.localAI[0], true) * GetLerpValue(num, num - 12f, projectile.localAI[0], true);
                projectile.direction = projectile.velocity.X > 0f ? 1 : -1;
                projectile.spriteDirection = projectile.direction;
                projectile.rotation = 0.7853982f * projectile.spriteDirection + projectile.velocity.ToRotation();
                if (projectile.spriteDirection == -1)
                {
                    projectile.rotation += 3.14159274f;
                }
                if (projectile.localAI[0] > 7f)
                {
                    //projectile.Center -= new Vector2((float)num4);
                    if (Main.rand.NextBool(15))
                    {
                        Dust dust = Dust.NewDustPerfect(projectile.Center, MyDustId.CyanBubble, null, 100, Color.Lerp(Main.hslToRgb(drawColor, 1f, 0.5f), Color.White, Main.rand.NextFloat() * 0.3f), 1f);
                        dust.scale = 0.7f;
                        dust.noGravity = true;
                        dust.velocity *= 0.5f;
                        dust.velocity += projectile.velocity * 2f;
                    }
                }
            }
            for (int n = 44; n > 0; n--)
            {
                projectile.oldPos[n] = projectile.oldPos[n - 1];
                projectile.oldRot[n] = projectile.oldRot[n - 1];
            }
            projectile.oldPos[0] = projectile.Center - projectile.velocity.SafeNormalize(Vector2.Zero) * 42f;
            projectile.oldRot[0] = projectile.velocity.ToRotation() + projectile.ai[0] * float.Lerp(-180, 90, MathHelper.Clamp(projectile.localAI[0] / 60, 0, 1));
            float lightScaler = MathHelper.SmoothStep(0, 1, 1 - Math.Abs(45 - projectile.localAI[0]) / 45f);
            if (Main.netMode == NetmodeID.Server) return;
            if (swoosh == null)
            {
                var u = swoosh = UltraSwoosh.NewUltraSwoosh(FirstZenith_NewVer_Proj.CanvasName, 30, 1, default, (-1.125f, 0.7125f));
                u.heatMap = ModAsset.bar_19.Value;
                u.ColorVector = new(0.16667f, 0.33333f, 0.5f);
                u.weaponTex = TextureAssets.Item[Main.player[projectile.owner].HeldItem.type].Value;
                u.autoUpdate = false;
            }
            swoosh.autoUpdate = false;
            if (projectile.ai[0] != 0)
                swoosh.center = projectile.Center + new Vector2(-projectile.velocity.Y, projectile.velocity.X) / projectile.ai[0];
            switch (projectile.localAI[0])
            {
                case < 4:
                    return;
                case >= 45:
                    for (int n = 0; n < 45; n++)
                    {
                        float k = n / 44f;
                        Color c = Color.Violet * MathHelper.SmoothStep(0, 1, 4 * k) * lightScaler;
                        Vector2 vec = projectile.oldPos[n] + (projectile.oldRot[n]).ToRotationVector2() * 116;
                        swoosh.VertexInfos[2 * n] = new(vec, c, new(k, 1, 1));
                        swoosh.VertexInfos[2 * n + 1] = new(projectile.oldPos[n], c, new(0, 0, 1));
                    }
                    return;
                default:
                    int t = (int)projectile.localAI[0];
                    Vector2[] vecOuter = new Vector2[t];
                    Vector2[] vecInner = new Vector2[t];
                    for (int n = 0; n < t; n++)
                    {
                        vecOuter[n] = projectile.oldPos[n] + (projectile.oldRot[n]).ToRotationVector2() * 116;
                        vecInner[n] = projectile.oldPos[n];
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
            if (Main.netMode == NetmodeID.Server) return;

            swoosh.timeLeft = 0;
            base.OnKill(timeLeft);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            DrawOthers();
            DrawSword();
            return false;
        }

        public void DrawOthers()
        {
            if (drawPlayer == null) drawPlayer = new Player();
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
            player.Center = projectile.oldPos[0];
            player.direction = projectile.velocity.X > 0f ? 1 : -1;
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * (float)player.direction, projectile.velocity.X * (float)player.direction);
            player.velocity.Y = 0.01f;
            player.wingFrame = 2;
            player.PlayerFrame();
            player.socialIgnoreLight = true;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, projectile.oldRot[0] - MathHelper.PiOver2);
            try
            {
                Main.PlayerRenderer.DrawPlayer(Main.Camera, player, player.position, 0f, player.fullRotationOrigin);
            }
            catch
            {
            }
            SpriteEffects spriteEffects = projectile.ai[0] > 0 ? 0 : SpriteEffects.FlipHorizontally;
            Vector2 vector71 = projectile.position + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
            projectile.DrawProjWithStarryTrail(Main.spriteBatch, drawColor, Color.White);
            var color84 = Color.White * projectile.Opacity * 0.9f;
            color84.A /= 2;
            projectile.DrawPrettyStarSparkle(Main.spriteBatch, spriteEffects, vector71, color84, Main.hslToRgb(drawColor, 1f, 0.5f));
        }
        public void DrawSword()
        {
            SpriteEffects spriteEffects = projectile.ai[0] > 0 ? 0 : SpriteEffects.FlipHorizontally;
            Texture2D texture2D4 = TextureAssets.Projectile[projectile.type].Value;
            var color84 = Color.White * projectile.Opacity * 0.9f;
            color84.A /= 2;
            var rectangle29 = texture2D4.Frame(15, 1, projectile.frame, 0, 0, 0);
            var origin = texture2D4.Size() / new Vector2(15, 1);
            origin *= spriteEffects == 0 ? new Vector2(0.1f, 0.9f) : new Vector2(0.9f, 0.9f);
            var rot = projectile.oldRot[0] + MathHelper.PiOver4;
            rot += projectile.ai[0] < 0 ? MathHelper.Pi / 2 : 0;
            Main.spriteBatch.Draw(texture2D4, projectile.oldPos[0] - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(rectangle29), color84, rot, origin, 1, spriteEffects, 0);
        }
    }
    public class WitheredWoodSword_Old : ModItem
    {
        public Item item => Item;
        public override bool IsLoadingEnabled(Mod mod) => FinalFractalSetConfig.OldVersionEnabled;

        public override void SetDefaults()
        {
            item.damage = 30;
            item.crit = 21;
            item.DamageType = DamageClass.Melee;
            item.width = 50;
            item.height = 66;
            item.rare = ItemRarityID.LightRed;
            item.useTime = 30;
            item.useAnimation = 30;
            item.knockBack = 6;
            item.useStyle = ItemUseStyleID.Swing;
            item.autoReuse = true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.shoot = ModContent.ProjectileType<WitheredWoodSwordProj>();
                item.shootSpeed = 1f;
                item.noUseGraphic = true;
                item.noMelee = true;
                item.mana = 50;
            }
            else
            {
                item.shoot = ProjectileID.None;
                item.shootSpeed = 0;
                item.noUseGraphic = false;
                item.noMelee = false;
                item.mana = 0;

            }
            return player.ownedProjectileCounts[item.shoot] < 1/* && player.name == ""*/;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
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
            ItemID.PearlwoodSword,
            ItemID.CactusSword);
            recipe.AddIngredient(ItemID.Mushroom, 50);
            recipe.AddIngredient(ItemID.GlowingMushroom, 50);
            recipe.AddIngredient(ItemID.Acorn, 50);
            recipe.AddIngredient(ItemID.BambooBlock, 15);
            recipe.AddTile(TileID.LivingLoom);
            recipe.ReplaceResult(this);
            recipe.Register();
        }
    }
    public class LivingWoodSword_Old : WitheredWoodSword_Old
    {
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.shoot = ModContent.ProjectileType<WitheredWoodSwordProj>();
                item.shootSpeed = 1f;
                item.noUseGraphic = true;
                item.noMelee = true;
                item.mana = 30;
            }
            else
            {
                item.shoot = ProjectileID.None;
                item.shootSpeed = 0;
                item.noUseGraphic = false;
                item.noMelee = false;
                item.mana = 0;

            }
            return player.ownedProjectileCounts[item.shoot] < 1/* && player.name == ""*/;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 58;
            Item.height = 72;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.damage = 60;
            Item.rare = ItemRarityID.Yellow;
        }
        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            recipe.AddIngredient<WitheredWoodSword_Old>();
            recipe.AddIngredient(ItemID.BrokenHeroSword);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.ReplaceResult(this);
            recipe.Register();
        }
    }
    public class WitheredWoodSwordProj : VertexHammerProj
    {
        public override bool IsLoadingEnabled(Mod mod) => FinalFractalSetConfig.OldVersionEnabled;
        public override string HammerName => base.HammerName;
        public override float MaxTime => (controlState == 2 ? 2f : 1f) * UpgradeValue(12, 9);
        public override float Factor => base.Factor;
        public override Vector2 CollidingSize => base.CollidingSize;
        //public override Vector2 projCenter => base.projCenter + new Vector2(Player.direction * 16, -16);
        public override Vector2 CollidingCenter => base.CollidingCenter;//new Vector2(projTex.Size().X / 3 - 16, 16)
        public override Vector2 DrawOrigin => base.DrawOrigin + new Vector2(-12, 12);
        public override Color color => base.color;
        public override Color VertexColor(float time) => Color.Lerp(Color.DarkGreen, UpgradeValue(Color.Brown, Color.Green), time);//Color.Lerp(UpgradeValue(Color.Brown, Color.Green), Color.DarkGreen, time)
        public override float MaxTimeLeft => (controlState == 2 ? 0.75f : 1f) * UpgradeValue(8, 7);
        public override float Rotation => base.Rotation;
        public override bool UseRight => true;
        public override bool UseLeft => false;
        public override (int X, int Y) FrameMax => (2, 1);
        public override void OnKill(int timeLeft)
        {

            //Lighting.add

            int max = (int)(30 * Factor);
            var vec = (CollidingCenter - DrawOrigin).RotatedBy(Rotation) + projCenter;
            if (Factor > 0.75f)
            {
                for (int n = 0; n < max; n++)
                {
                    Dust.NewDustPerfect(vec, UpgradeValue(MyDustId.Wood, Main.rand.Next(new int[] { MyDustId.Wood, MyDustId.GreenGrass })), (MathHelper.TwoPi / max * n).ToRotationVector2() * Main.rand.NextFloat(2, 8)).noGravity = true;
                }
            }
            //if (factor == 1)
            //{
            //    Projectile.NewProjectile(projectile.GetSource_FromThis(), vec, default, ModContent.ProjectileType<HolyExp>(), player.GetWeaponDamage(player.HeldItem) * 3, projectile.knockBack, projectile.owner);
            //}
            base.OnKill(timeLeft);
        }
        public override void OnChargedShoot()
        {
            SoundEngine.PlaySound(SoundID.Item60, projectile.position);

            int count = 0;
            int max = UpgradeValue(5, 15);
            foreach (var npc in Main.npc)
            {
                if (!npc.friendly && npc.active && npc.CanBeChasedBy() && (npc.Center - Player.Center).Length() < UpgradeValue(512, 768))
                {
                    npc.velocity *= UpgradeValue(0.25f, 0.125f);
                    if (count < max)
                    {
                        count++;
                        var unit = Main.rand.NextVector2Unit();
                        Projectile.NewProjectile(projectile.GetSource_FromThis(), npc.Center - unit * 64, unit * UpgradeValue(16, 32), ProjectileID.NettleBurstRight, Projectile.damage / 3, projectile.knockBack, projectile.owner);
                    }
                }
            }
            if (Player.whoAmI == Main.myPlayer)
            {
                Player.velocity += (Player.GetModPlayer<LogSpiralLibraryPlayer>().targetedMousePosition - Player.Center).SafeNormalize(default) * UpgradeValue(16, 24) * new Vector2(1, 0.25f);
                NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, Player.whoAmI);
                NetMessage.SendData(MessageID.ShotAnimationAndSound, -1, -1, null, Player.whoAmI);
            }
        }
        public override void OnRelease(bool charged, bool left)
        {
            if (Charged)
            {
                if (Math.Abs(Player.velocity.Y) < 1f)
                {
                    Player.velocity.X *= 0.95f;
                }
                if ((int)projectile.ai[1] == 1)
                {
                    OnChargedShoot();
                }
            }
            //Main.NewText(new NPCs.Baron.Baron().CanTownNPCSpawn(10, 10));
            base.OnRelease(charged, left);
        }
        public override Rectangle? frame => projTex.Frame(2, 1, UpgradeValue(0, 1));
        public bool Extra;
        public virtual int ExtraItemType => ModContent.ItemType<LivingWoodSword_Old>();
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Extra);
            base.SendExtraAI(writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Extra = reader.ReadBoolean();
            base.ReceiveExtraAI(reader);
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo itemSource)
                Extra = itemSource.Item.type == ExtraItemType;
            base.OnSpawn(source);
        }
        public T UpgradeValue<T>(T normal, T extra, T defaultValue = default)
        {
            return Extra ? extra : normal;
        }
        public override void VertexInfomation(ref bool additive, ref int indexOfGreyTex, ref float endAngle, ref bool useHeatMap, ref int passCount)
        {
            base.VertexInfomation(ref additive, ref indexOfGreyTex, ref endAngle, ref useHeatMap, ref passCount);
        }
    }
    public class DyingStoneSword_Old : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod) => FinalFractalSetConfig.OldVersionEnabled;
        public Item item => Item;
        public override void SetDefaults()
        {
            item.damage = 40;
            item.crit = 26;
            item.DamageType = DamageClass.Melee;
            item.width = 48;
            item.height = 48;
            item.rare = ItemRarityID.Pink;
            item.useTime = 25;
            item.useAnimation = 25;
            item.knockBack = 8;
            item.useStyle = ItemUseStyleID.Swing;
            item.autoReuse = true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.shoot = ModContent.ProjectileType<DyingStoneSwordProj>();
                item.shootSpeed = 1f;
                item.noUseGraphic = true;
                item.noMelee = true;
                item.mana = 50;
            }
            else
            {
                item.shoot = ProjectileID.None;
                item.shootSpeed = 0;
                item.noUseGraphic = false;
                item.noMelee = false;
                item.mana = 0;

            }
            return player.ownedProjectileCounts[item.shoot] < 1/* && player.name == ""*/;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            for (int n = 0; n < 6; n++)
                recipe.AddIngredient(3764 + n);//六种晶光刃
            recipe.AddIngredient(ItemID.OrangePhasesaber);
            recipe.AddIngredient(ItemID.BoneSword);
            recipe.AddIngredient(ItemID.AntlionClaw);
            recipe.AddIngredient(ItemID.BeamSword);
            recipe.AddIngredient(ItemID.PurpleClubberfish);
            recipe.AddIngredient(ItemID.Bladetongue);
            recipe.AddIngredient(ItemID.StoneBlock, 500);
            recipe.AddIngredient(ItemID.EbonstoneBlock, 500);
            recipe.AddIngredient(ItemID.CrimstoneBlock, 500);
            recipe.AddIngredient(ItemID.PearlstoneBlock, 500);
            recipe.AddIngredient(ItemID.Sandstone, 500);
            recipe.AddIngredient(ItemID.CorruptSandstone, 500);
            recipe.AddIngredient(ItemID.CrimsonSandstone, 500);
            recipe.AddIngredient(ItemID.HallowSandstone, 500);
            recipe.AddIngredient(ItemID.Granite, 500);
            recipe.AddIngredient(ItemID.Obsidian, 50);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.ReplaceResult(this);
            recipe.Register();
        }
    }
    public class MossStoneSword_Old : DyingStoneSword_Old
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 70;
            item.width = 50;
            item.rare = ItemRarityID.Yellow;
            item.useTime = 18;
            item.useAnimation = 18;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.shoot = ModContent.ProjectileType<DyingStoneSwordProj>();
                item.shootSpeed = 1f;
                item.noUseGraphic = true;
                item.noMelee = true;
                item.mana = 40;
            }
            else
            {
                item.shoot = ProjectileID.None;
                item.shootSpeed = 0;
                item.noUseGraphic = false;
                item.noMelee = false;
                item.mana = 0;

            }
            return player.ownedProjectileCounts[item.shoot] < 1/* && player.name == ""*/;
        }
        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            recipe.AddIngredient<DyingStoneSword_Old>();
            recipe.AddIngredient(ItemID.BrokenHeroSword);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.ReplaceResult(this);
            recipe.Register();
        }
    }
    public class DyingStoneSwordProj : WitheredWoodSwordProj
    {
        public override bool IsLoadingEnabled(Mod mod) => FinalFractalSetConfig.OldVersionEnabled;
        public static void ShootSharpTears(Vector2 targetPos, Player player, Projectile projectile)
        {
            player.LimitPointToPlayerReachableArea(ref targetPos);
            Vector2 vector22 = targetPos + Main.rand.NextVector2Circular(8f, 8f);
            Vector2 value7 = player.FindSharpTearsSpot(vector22).ToWorldCoordinates(Main.rand.Next(17), Main.rand.Next(17));
            if ((player.Center - value7).Length() < 48) value7 = targetPos;
            Vector2 vector23 = (vector22 - value7).SafeNormalize(-Vector2.UnitY) * 16f;
            Projectile.NewProjectile(projectile.GetSource_FromThis(), value7.X, value7.Y, vector23.X, vector23.Y, ModContent.ProjectileType<SharpStoneTears>(), projectile.damage / 4, projectile.knockBack, player.whoAmI, 0f, Main.rand.NextFloat() * 0.5f + 0.5f);
        }
        public override void OnRelease(bool charged, bool left)
        {
            if (Charged)
            {
                int max = UpgradeValue(1, 3);
                for (int n = 0; n < max; n++)
                {
                    Vector2 pointPoisition2 = Player.Center + new Vector2(128 * Player.direction, 0) * ((projectile.ai[1] + (float)n / max) / MaxTimeLeft) * max;
                    ShootSharpTears(pointPoisition2, Player, projectile);
                }
            }
            if ((int)projectile.ai[1] == 0)
            {
                projectile.damage = 0;
                if (Charged)
                {
                    projectile.damage = (int)(Player.GetWeaponDamage(Player.HeldItem) * (3 * Factor * Factor));
                    SoundEngine.PlaySound(SoundID.Item71);
                }
            }
            projectile.ai[1]++;
            if (projectile.ai[1] > (Charged ? MaxTimeLeft * Factor : timeCount))
            {
                projectile.Kill();
            }
        }
        public override Color VertexColor(float time) => Color.Lerp(Color.DarkGray, UpgradeValue(Color.Gray, Color.Green), time);
        public override int ExtraItemType => ModContent.ItemType<MossStoneSword_Old>();
    }
    public class SharpStoneTears : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 25;
        }
        public override void OnKill(int timeLeft)
        {
            for (float num6 = 0f; num6 < 1f; num6 += 0.025f)
            {
                Dust dust2 = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(16f, 16f) * Projectile.scale + Projectile.velocity.SafeNormalize(Vector2.UnitY) * num6 * 200f * Projectile.scale, MyDustId.GreyStone, Main.rand.NextVector2Circular(3f, 3f));
                dust2.velocity.Y += -0.3f;
                Dust dust = dust2;
                dust.velocity += Projectile.velocity * 0.2f;
                dust2.scale = .5f;
                dust2.alpha = 100;
            }
        }
        Projectile projectile => Projectile;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D value33 = TextureAssets.Projectile[projectile.type].Value;
            Rectangle value34 = value33.Frame(1, 6, 0, projectile.frame);
            Vector2 origin11 = new Vector2(16f, value34.Height / 2);
            Color alpha4 = projectile.GetAlpha(lightColor);
            Vector2 scale8 = new Vector2(projectile.scale);
            float lerpValue4 = GetLerpValue(30f, 25f, projectile.ai[0], clamped: true);
            scale8.Y *= lerpValue4;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            Main.EntitySpriteDraw(TextureAssets.Extra[98].Value, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY) - projectile.velocity * projectile.scale * 0.5f, null, projectile.GetAlpha(lightColor * 0.262745f) * 1f, projectile.rotation + (float)Math.PI / 2f, TextureAssets.Extra[98].Value.Size() / 2f, projectile.scale * 0.9f, spriteEffects, 0);
            Main.EntitySpriteDraw(value33, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), value34, alpha4, projectile.rotation, origin11, scale8, spriteEffects, 0);
            return false;
        }
        public override void AI()
        {
            int num = MyDustId.GreyStone;
            float scaleFactor = .75f;
            int num2 = 30;
            int num3 = 30;
            int num4 = 2;
            int num5 = 2;
            int maxValue = 6;
            float dustScaler = .25f;
            bool flag = Projectile.ai[0] < 20;
            bool flag2 = Projectile.ai[0] >= 20;
            bool flag3 = Projectile.ai[0] >= 30;
            Projectile.ai[0] += 1f;
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.frame = Main.rand.Next(maxValue);
                for (int i = 0; i < num2; i++)
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(24f, 24f), num, Projectile.velocity * scaleFactor * MathHelper.Lerp(0.2f, 0.7f, Main.rand.NextFloat()));
                    dust.velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);
                    dust.scale = 0.8f + Main.rand.NextFloat() * 0.5f;
                    dust.scale *= dustScaler;
                }

                for (int j = 0; j < num3; j++)
                {
                    Dust dust2 = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(24f, 24f), num, Main.rand.NextVector2Circular(2f, 2f) + Projectile.velocity * scaleFactor * MathHelper.Lerp(0.2f, 0.5f, Main.rand.NextFloat()));
                    dust2.velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);
                    dust2.scale = 0.8f + Main.rand.NextFloat() * 0.5f;
                    dust2.fadeIn = 1f;
                    dust2.scale *= dustScaler;

                }

                SoundEngine.PlaySound(SoundID.Item60, Projectile.Center);
            }

            if (flag)
            {
                Projectile.Opacity += 0.1f;
                Projectile.scale = Projectile.Opacity * Projectile.ai[1];
                for (int k = 0; k < num4; k++)
                {
                    Dust dust3 = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(16f, 16f), num, Projectile.velocity * scaleFactor * MathHelper.Lerp(0.2f, 0.5f, Main.rand.NextFloat()));
                    dust3.velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);
                    dust3.velocity *= 0.5f;
                    dust3.scale = 0.8f + Main.rand.NextFloat() * 0.5f;
                    dust3.scale *= dustScaler;

                }

            }

            if (flag2)
            {
                Projectile.Opacity -= 0.2f;
                for (int l = 0; l < num5; l++)
                {
                    Dust dust4 = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(16f, 16f), num, Projectile.velocity * scaleFactor * MathHelper.Lerp(0.2f, 0.5f, Main.rand.NextFloat()));
                    dust4.velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);
                    dust4.velocity *= 0.5f;
                    dust4.scale = 0.8f + Main.rand.NextFloat() * 0.5f;
                    dust4.scale *= dustScaler;

                }

            }

            if (flag3)
                Projectile.Kill();
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 200f * Projectile.scale, 22f * Projectile.scale, DelegateMethods.CutTiles);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint15 = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 200f * Projectile.scale, 22f * Projectile.scale, ref collisionPoint15))
                return true;
            return false;
        }
        public override bool ShouldUpdatePosition() => false;
    }
    public class RustySteelBlade_Old : ModItem
    {
        public Item item => Item;
        public override bool IsLoadingEnabled(Mod mod) => FinalFractalSetConfig.OldVersionEnabled;
        public override void SetDefaults()
        {
            item.damage = 70;
            item.crit = 26;
            item.DamageType = DamageClass.Melee;
            item.width = 66;
            item.height = 74;
            item.rare = ItemRarityID.LightPurple;
            item.useTime = 21;
            item.useAnimation = 21;
            item.knockBack = 8;
            item.useStyle = ItemUseStyleID.Swing;
            item.autoReuse = true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.shoot = ModContent.ProjectileType<RustySteelBladeProj>();
                item.shootSpeed = 1f;
                item.noUseGraphic = true;
                item.noMelee = true;
                item.mana = 80;
            }
            else
            {
                item.shoot = ProjectileID.None;
                item.shootSpeed = 0;
                item.noUseGraphic = false;
                item.noMelee = false;
                item.mana = 0;

            }
            return player.ownedProjectileCounts[item.shoot] < 1/* && player.name == ""*/;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
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
            ItemID.DyeTradersScimitar,
            ItemID.FalconBlade,
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
            recipe.ReplaceResult(this);
            recipe.Register();
        }
    }
    public class RefinedSteelBlade_Old : RustySteelBlade_Old
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 90;
            item.rare = ItemRarityID.Yellow;
            item.useTime = 15;
            item.useAnimation = 15;
        }
        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            recipe.AddIngredient<RustySteelBlade_Old>();
            recipe.AddIngredient(ItemID.BrokenHeroSword, 3);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.ReplaceResult(this);
            recipe.Register();
        }
    }
    public class RustySteelBladeProj : WitheredWoodSwordProj
    {
        public override bool IsLoadingEnabled(Mod mod) => FinalFractalSetConfig.OldVersionEnabled;
        public override void OnRelease(bool charged, bool left)
        {
            if (Charged)
            {
                int max = UpgradeValue(1, 3);
                var player = Player;
                if ((int)projectile.ai[1] % 5 == 0)
                    for (int n = 0; n < max; n++)

                    {
                        Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true, true);
                        float num6 = Main.mouseX + Main.screenPosition.X - vector.X;
                        float num7 = Main.mouseY + Main.screenPosition.Y - vector.Y;
                        int num166 = (player.itemAnimationMax - player.itemAnimation) / player.itemTime;
                        Vector2 velocity_ = new Vector2(num6, num7);
                        Vector2 value7 = Main.MouseWorld - player.MountedCenter;
                        if (num166 == 1 || num166 == 2)
                        {
                            int num168;
                            bool zenithTarget = PureFractal_Old.GetZenithTarget(Main.MouseWorld, 400f, player, out num168);
                            if (zenithTarget)
                            {
                                value7 = Main.npc[num168].Center - player.MountedCenter;
                            }
                            bool flag8 = num166 == 2;
                            if (num166 == 1 && !zenithTarget)
                            {
                                flag8 = true;
                            }
                            if (flag8)
                            {
                                value7 += Main.rand.NextVector2Circular(150f, 150f);
                            }
                        }
                        velocity_ = value7 / 2f;
                        float ai5 = Main.rand.Next(-100, 101);
                        var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), player.Center, velocity_, ProjectileID.FinalFractal, projectile.damage / 6, projectile.knockBack, player.whoAmI, ai5);
                        proj.localNPCHitCooldown = 20;
                    }
            }
            if ((int)projectile.ai[1] == 0)
            {
                projectile.damage = 0;
                if (Charged)
                {
                    projectile.damage = (int)(Player.GetWeaponDamage(Player.HeldItem) * (3 * Factor * Factor));
                    SoundEngine.PlaySound(SoundID.Item71);
                }
            }
            projectile.ai[1]++;
            if (projectile.ai[1] > (Charged ? MaxTimeLeft * Factor : timeCount))
            {
                projectile.Kill();
            }
        }
        public override Color VertexColor(float time) => Color.Lerp(Color.DarkGray, UpgradeValue(Color.Brown, Color.Gray), time);
        public override int ExtraItemType => ModContent.ItemType<RefinedSteelBlade_Old>();
    }
}
