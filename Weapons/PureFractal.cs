using Terraria.Localization;
using System.Collections.Generic;
using System;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using static Terraria.ModLoader.ModContent;
using System.IO;
using System.Reflection;

namespace FinalFractalSet.Weapons
{
    public class PureFractal_Old : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var time = ((float)Math.Sin(LogSpiralLibraryMod.ModTime / 60f * MathHelper.TwoPi) + 1) * .5f;
            Color color;
            if (time < 0.5f) color = Color.Lerp(Color.Cyan, Color.Green, time * 2f);
            else color = Color.Lerp(Color.Green, Color.Yellow, time * 2f - 1);
            tooltips.Add(new TooltipLine(Mod, "PureSuggestion", Language.GetOrRegister("Mods.FinalFractalSet.FinalFractalTip.0").Value) { OverrideColor = color });
        }

        public Texture2D tex => TextureAssets.Item[Item.type].Value;
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Item.ShaderItemEffectInventory(spriteBatch, position, origin, LogSpiralLibraryMod.Misc[1].Value, Color.Lerp(new Color(0, 162, 232), new Color(34, 177, 76), (float)Math.Sin(MathHelper.Pi / 60 * LogSpiralLibraryMod.ModTime) / 2 + 0.5f), scale);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.ShaderItemEffectInWorld(spriteBatch, LogSpiralLibraryMod.Misc[1].Value, Color.Lerp(new Color(0, 162, 232), new Color(34, 177, 76), (float)Math.Sin(MathHelper.Pi / 60 * LogSpiralLibraryMod.ModTime) / 2 + 0.5f), rotation);
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 56;
            Item.height = 56;
            Item.UseSound = SoundID.Item169;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.shoot = ProjectileType<PureFractalProj>();
            Item.useAnimation = 30;
            Item.useTime = Item.useAnimation / 3;
            Item.shootSpeed = 16f;
            Item.damage = 240;
            Item.knockBack = 6.5f;
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.crit = 10;
            Item.rare = ItemRarityID.Purple;
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }
        //public override void AddRecipes() => CreateRecipe().AddIngredient(ItemID.Zenith).AddIngredient<FirstFractal_CIVE>().AddTile(TileID.LunarCraftingStation).Register();
        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            recipe.QuickAddIngredient(ItemID.Zenith);
            recipe.AddIngredient<FirstFractal_Remastered>();
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
        public static bool GetZenithTarget(Vector2 searchCenter, float maxDistance, Player player, out int npcTargetIndex)
        {
            npcTargetIndex = 0;
            int? num = null;
            float num2 = maxDistance;
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy(player, false))
                {
                    float num3 = Vector2.Distance(searchCenter, npc.Center);
                    if (num2 > num3)
                    {
                        num = new int?(i);
                        num2 = num3;
                    }
                }
            }
            if (num == null)
            {
                return false;
            }
            npcTargetIndex = num.Value;
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
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
                bool zenithTarget = GetZenithTarget(Main.MouseWorld, 400f, player, out num168);
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
            float ai5 = Main.rand.Next(-100, 101);//
            //if(player.ownedProjectileCounts[type] < 1)
            var proj = Projectile.NewProjectileDirect(source, player.Center, velocity_, type, damage, knockback, player.whoAmI, ai5);
            proj.frame = Main.rand.Next(26);
            proj.netUpdate = true;
            proj.netUpdate2 = true;
            return false;
        }
    }
    public class PureFractalProj : ModProjectile
    {
        static (Color, float) GetOtherInfo(int frame) => frame switch
        {
            _ => (Color.White, 1)
        };
        static Texture2D GetPureFractalHeatMaps_Internal(int frame) => LogSpiralLibraryMod.HeatMap[0].Value;
        static Texture2D GetPureFractalHeatMaps(int frame)
        {
            if (HeatMapTextures[frame] == null)
                RefreshData();
            return HeatMapTextures[frame];
        }
        static Texture2D GetPureFractalProjTexs_Internal(int frame)
        {
            if (Main.netMode == NetmodeID.Server) return null;
            if (frame > 21)
                return (frame switch
                {
                    22 => ModAsset.Weapons_Tizona,
                    23 => ModAsset.Weapons_TrueTerraBlade,
                    24 => ModAsset.PureFractal,
                    25 => ModAsset.FinalFractal,
                    _ => null,
                })?.Value;
            int type = frame switch
            {
                0 => ItemID.CopperShortsword,
                1 => ItemID.Starfury,
                2 => ItemID.EnchantedSword,
                3 => ItemID.BeeKeeper,
                4 => ItemID.LightsBane,
                5 => ItemID.BloodButcherer,
                6 => ItemID.Muramasa,
                7 => ItemID.BladeofGrass,
                8 => ItemID.FieryGreatsword,
                9 => ItemID.NightsEdge,
                10 => ItemID.Excalibur,
                11 => ItemID.TrueNightsEdge,
                12 => ItemID.TrueExcalibur,
                13 => ItemID.TerraBlade,
                14 => ItemID.Seedler,
                15 => ItemID.TheHorsemansBlade,
                16 => ItemID.InfluxWaver,
                17 => ItemID.StarWrath,
                18 => ItemID.Meowmere,
                19 => ItemID.Zenith,
                20 => ItemID.Terragrim,
                21 or _ => ItemID.Arkhalis
            };
            if (!TextureAssets.Item[type].IsLoaded)
                Main.instance.LoadItem(type);
            return TextureAssets.Item[type].Value;
        }
        static Texture2D GetPureFractalProjTexs(int frame)
        {
            if (ItemTextures[frame] == null)
                RefreshData();
            return ItemTextures[frame];
        }
        static Texture2D[] ItemTextures = new Texture2D[26];
        static Texture2D[] HeatMapTextures = new Texture2D[26];
        static Color[] PureFractalColors = new Color[26];
        static float[] PureFractalAirFactors = new float[26];
        static void RefreshData()
        {
            if (Main.netMode == NetmodeID.Server) return;
            for (int n = 0; n < 26; n++)
            {
                (PureFractalColors[n], PureFractalAirFactors[n]) = GetOtherInfo(n);
                ItemTextures[n] = GetPureFractalProjTexs_Internal(n);
                HeatMapTextures[n] = GetPureFractalHeatMaps_Internal(n);
            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)projectile.frame);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            projectile.frame = reader.ReadByte();
        }
        Projectile projectile => Projectile;
        Color newColor => PureFractalColors[Projectile.frame];
        float airFactor => PureFractalAirFactors[Projectile.frame];
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
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Vector2 mountedCenter = player.MountedCenter;//坐骑上玩家的中心
            float lerpValue = Utils.GetLerpValue(900f, 0f, projectile.velocity.Length() * 2f, true);//获取线性插值的t值
            float num = MathHelper.Lerp(0.7f, 2f, lerpValue);//速度的模长的两倍越接近900，这个值越接近0.7f
            projectile.localAI[0] += num;
            Main.projFrames[projectile.type] = 25;
            if (projectile.localAI[0] >= 120f)
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
        public void DrawSword()
        {
            var max = 0;
            Texture2D currentTex = GetPureFractalProjTexs(projectile.frame);
            float _scaler = currentTex.Size().Length();
            var multiValue = 1 - projectile.localAI[0] / 120f;
            var spEffect = projectile.ai[0] * projectile.velocity.X > 0 ? 0 : SpriteEffects.FlipHorizontally;
            var size = 1;
            for (int n = 0; n < Projectile.oldPos.Length; n++)
            {
                if (projectile.oldPos[n] == default) { max = n; break; }
            }
            for (int n = 15; n < max; n += 15)
            {
                var _fac = 1 - (float)n / max;
                //_fac *= _fac * _fac;
                var _color = newColor * _fac;//newColor 
                _color.A = 0;
                Main.spriteBatch.Draw(currentTex, projectile.oldPos[n - 1] - Main.screenPosition, null, _color * multiValue, projectile.oldRot[n - 1] - MathHelper.PiOver4 + (spEffect == 0 ? 0 : MathHelper.PiOver2), currentTex.Size() * new Vector2(spEffect == 0 ? 0 : 1, 1), size, spEffect, 0);
                projectile.DrawPrettyStarSparkle(Main.spriteBatch, 0, projectile.oldPos[n - 1] + (projectile.oldRot[n - 1] - MathHelper.PiOver2).ToRotationVector2() * _scaler * size - Main.screenPosition, Color.White, _color);

            }
            Main.spriteBatch.Draw(currentTex, projectile.oldPos[0] - Main.screenPosition, null, Color.White * multiValue, projectile.oldRot[0] - MathHelper.PiOver4 + (spEffect == 0 ? 0 : MathHelper.PiOver2), currentTex.Size() * new Vector2(spEffect == 0 ? 0 : 1, 1), size, spEffect, 0);
            projectile.DrawPrettyStarSparkle(Main.spriteBatch, 0, projectile.oldPos[0] + (projectile.oldRot[0] - MathHelper.PiOver2).ToRotationVector2() * _scaler * size - Main.screenPosition, Color.White, newColor);
        }
        public void DrawSwoosh()
        {
            var ShaderSwooshUL = LogSpiralLibraryMod.ShaderSwooshUL;
            int max = 60;
            Texture2D currentTex = GetPureFractalProjTexs(projectile.frame);
            var hsl = Main.rgbToHsl(newColor);
            for (int n = 0; n < Projectile.oldPos.Length; n++)
            {
                if (projectile.oldPos[n] == default) { max = n; break; }
            }

            if (!Main.gamePaused && projectile.localAI[0] < 60f)
            {
                Vector2 center = projectile.oldPos[0];
                int num11 = 1 + (int)(projectile.velocity.Length() / 100f);
                var lerpValue2 = MathHelper.Clamp(projectile.localAI[0] / 60f, 0, 1);
                num11 = (int)((float)num11 * Utils.GetLerpValue(0f, 0.5f, lerpValue2, true) * Utils.GetLerpValue(1f, 0.5f, lerpValue2, true));
                if (num11 < 1)
                {
                    num11 = 1;
                }
                Player player = Main.player[projectile.owner];
                var unit = (projectile.rotation - MathHelper.PiOver2).ToRotationVector2();

                for (int i = 0; i < num11 + 5; i++)
                {

                    if (Main.rand.NextBool(9))
                    {
                        int _num = Main.rand.Next(1, 4);
                        for (int k = 0; k < _num; k++)
                        {
                            Dust dust = Dust.NewDustPerfect(center + unit * .5f * 50, 278, null, 100, Color.Lerp(newColor, Color.White, Main.rand.NextFloat() * 0.3f), 1f);
                            dust.scale = 0.4f;
                            dust.fadeIn = 0.4f + Main.rand.NextFloat() * 0.3f;
                            dust.noGravity = true;
                            dust.velocity += unit * (3f + Main.rand.NextFloat() * 4f);
                        }
                    }
                    Vector3 value5 = Vector3.Lerp(Vector3.One, newColor.ToVector3(), 0.7f);
                    Lighting.AddLight(projectile.Center, newColor.ToVector3() * 0.5f * projectile.Opacity);
                    Lighting.AddLight(player.MountedCenter, value5 * projectile.Opacity * 0.15f);
                }
            }

            #region 快乐顶点绘制_1(在原来的基础上叠加，亮瞎了)
            if (LogSpiralLibraryMod.ShaderSwooshUL == null) return;
            if (LogSpiralLibraryMod.RenderEffect == null) return;
            if (Main.GameViewMatrix == null) return;
            var trans = Main.GameViewMatrix != null ? Main.GameViewMatrix.TransformationMatrix : Matrix.Identity;
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
            SamplerState sampler = SamplerState.LinearWrap;
            #endregion
            #region 快乐顶点绘制_2(现在才进入正题)
            float _scaler = currentTex.Size().Length() * airFactor;
            var bars = new List<CustomVertexInfo>();
            var multiValue = 1 - projectile.localAI[0] / 120f;
            multiValue = 4 * multiValue / (3 * multiValue + 1);
            for (int i = 0; i < max; i++)
            {
                int _i = i % 15;
                var _value = max / 15 * 15;
                var f = i > _value ? _i / (max - _value - 1f) : _i / 15f;
                f = 1 - f;
                var alphaLight = 0.5f;
                var _f = f * f;
                _f = MathHelper.Clamp(_f, 0, 1);
                bars.Add(new CustomVertexInfo(projectile.oldPos[i] + (projectile.oldRot[i] - MathHelper.PiOver2).ToRotationVector2() * _scaler, newColor with { A = (byte)(_f * 255 * (float)Math.Pow(1 - i / 15 / 5f, 2)) } * multiValue, new Vector3(1 - f, 1, alphaLight)));
                bars.Add(new CustomVertexInfo(projectile.oldPos[i], newColor with { A = 0 } * multiValue, new Vector3(1 - f, 0, alphaLight)));
            }
            List<CustomVertexInfo> _triangleList = [];
            if (bars.Count > 2)
            {
                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    //if (i == 28) continue;
                    _triangleList.Add(bars[i]);
                    _triangleList.Add(bars[i + 2]);
                    _triangleList.Add(bars[i + 1]);
                    _triangleList.Add(bars[i + 1]);
                    _triangleList.Add(bars[i + 2]);
                    _triangleList.Add(bars[i + 3]);
                }
                ShaderSwooshUL.Parameters["uTransform"].SetValue(model * trans * projection);
                ShaderSwooshUL.Parameters["uTime"].SetValue(-(float)LogSpiralLibraryMod.ModTime * 0.03f);
                ShaderSwooshUL.Parameters["checkAir"].SetValue(true);
                ShaderSwooshUL.Parameters["airFactor"].SetValue(airFactor);
                ShaderSwooshUL.Parameters["gather"].SetValue(true);
                ShaderSwooshUL.Parameters["alphaFactor"].SetValue(2);
                ShaderSwooshUL.Parameters["heatMapAlpha"].SetValue(false);
                ShaderSwooshUL.Parameters["heatRotation"].SetValue(Matrix.Identity);
                ShaderSwooshUL.Parameters["lightShift"].SetValue(0);
                ShaderSwooshUL.Parameters["distortScaler"].SetValue(0);
                ShaderSwooshUL.Parameters["AlphaVector"].SetValue(new Vector3(0.25f, 0.25f, 0.5f));

                Main.graphics.GraphicsDevice.Textures[0] = LogSpiralLibraryMod.BaseTex_Swoosh[7].Value;
                Main.graphics.GraphicsDevice.Textures[1] = LogSpiralLibraryMod.AniTex_Swoosh[3].Value;
                Main.graphics.GraphicsDevice.Textures[2] = currentTex;
                Main.graphics.GraphicsDevice.Textures[3] = GetPureFractalHeatMaps(Projectile.frame);
                Main.graphics.GraphicsDevice.SamplerStates[0] = sampler;
                Main.graphics.GraphicsDevice.SamplerStates[1] = sampler;
                Main.graphics.GraphicsDevice.SamplerStates[2] = sampler;
                Main.graphics.GraphicsDevice.SamplerStates[3] = SamplerState.AnisotropicClamp;
                ShaderSwooshUL.CurrentTechnique.Passes[7].Apply();
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _triangleList.ToArray(), 0, _triangleList.Count / 3);
                Main.graphics.GraphicsDevice.RasterizerState = originalState;
            }
            #endregion
            return;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            DrawSwoosh();
            DrawSword();
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
            projectile.localNPCHitCooldown = 3;
            projectile.penetrate = -1;

            RefreshData();
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
