using FinalFractalSet.Weapons;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing.RenderDrawingEffects;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Contents.Melee;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Contents.Melee.Core;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Contents.Melee.ExtendedMelee;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using NetSimplified;
using NetSimplified.Syncing;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Shaders;

namespace FinalFractalSet.REAL_NewVersions.Pure
{
    public class PureFractal_NewVer : MeleeSequenceItem<PureFractal_NewVer_Proj>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 240;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Zenith)
                .AddIngredient<PureTestItem>()
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Item.ShaderItemEffectInventory(spriteBatch, position, origin, LogSpiralLibraryMod.Misc[1].Value, Color.Lerp(new Color(0, 162, 232), new Color(34, 177, 76), (float)Math.Sin(MathHelper.Pi / 60 * LogSpiralLibraryMod.ModTime) / 2 + 0.5f), scale);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.ShaderItemEffectInWorld(spriteBatch, LogSpiralLibraryMod.Misc[1].Value, Color.Lerp(new Color(0, 162, 232), new Color(34, 177, 76), (float)Math.Sin(MathHelper.Pi / 60 * LogSpiralLibraryMod.ModTime) / 2 + 0.5f), rotation);
        }

        public override bool AltFunctionUse(Player player) => true;
    }

    public class PureFractal_NewVer_Proj : MeleeSequenceProj
    {
        public override string Texture => base.Texture.Replace("_Proj", "");
        public const string CanvasName = "FinalFractalSet:PureFractal";

        private readonly IRenderEffect[][] _renderEffects = [[new AirDistortEffect(4,1.5f, 0, 0.5f)],
                    [new DyeEffect(ItemID.VortexDye), new BloomEffect(0, 1f, 1, 3, true,2,true) ]];

        public override void Load()
        {
            RenderCanvasSystem.RegisterCanvasFactory(CanvasName, () => new(_renderEffects));
            base.Load();
        }

        public override void InitializeStandardInfo(StandardInfo standardInfo, VertexDrawStandardInfo vertexStandard)
        {
            standardInfo.standardColor = Color.Green * .5f;
            standardInfo.itemType = ModContent.ItemType<PureFractal_NewVer>();

            vertexStandard.scaler = 90;
            vertexStandard.timeLeft = 15;
            vertexStandard.alphaFactor = 2f;
            vertexStandard.canvasName = CanvasName;
            vertexStandard.SetDyeShaderID(ItemID.VortexDye);
        }

        public override bool LabeledAsCompleted => true;

        [SequenceDelegate]
        private static void ShootPurefractalProj_Few(MeleeAction action)
        {
            if (action.Owner is not Player plr || plr.whoAmI != Main.myPlayer) return;
            ShootSingle(action, plr);
        }

        [SequenceDelegate]
        public static void ShootPurefractalProj_Lots(MeleeAction action)
        {
            if (action.Owner is not Player plr || plr.whoAmI != Main.myPlayer) return;
            for (int n = 0; n < 3; n++)
                ShootSingle(action, plr);
        }

        private static void ShootSingle(MeleeAction action, Player plr)
        {
            Vector2 vector = plr.RotatedRelativePoint(plr.MountedCenter, true, true);
            float num6 = Main.mouseX + Main.screenPosition.X - vector.X;
            float num7 = Main.mouseY + Main.screenPosition.Y - vector.Y;
            int num166 = (plr.itemAnimationMax - plr.itemAnimation) / plr.itemTime;
            Vector2 velocity_ = new(num6, num7);
            Vector2 value7 = Main.MouseWorld - plr.MountedCenter;
            if (num166 == 1 || num166 == 2)
            {
                int num168;
                bool zenithTarget = PureFractal_Old.GetZenithTarget(Main.MouseWorld, 400f, plr, out num168);
                if (zenithTarget)
                {
                    value7 = Main.npc[num168].Center - plr.MountedCenter;
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
            //if(plr.ownedProjectileCounts[type] < 1)
            var proj = Projectile.NewProjectileDirect(action.Projectile.GetItemSource_FromThis(), plr.Center, velocity_, ModContent.ProjectileType<PureFractalProj>(), action.CurrentDamage, action.Projectile.knockBack, plr.whoAmI, ai5);
            proj.frame = Main.rand.Next(26);
            proj.localAI[0] -= Main.rand.Next(0, Main.rand.Next(0, 120));
            proj.netUpdate = true;
            proj.netUpdate2 = true;
        }
    }

    public class PureFractalCharging : ChargingInfo
    {
        public override string Category => "";

        public override void OnDeactive()
        {
            base.OnDeactive();
            if (IsLocalProjectile)
                RotatingBladeShootActionSyncing.Get(Projectile.owner).Send(runLocally: true);
        }

        public override void OnStartAttack()
        {
            base.OnStartAttack();
            var action = this;
            SoundEngine.PlaySound(SoundID.Item84, Owner.Center);
            for (int n = 0; n < 20; n++)
            {
                MiscMethods.FastDust(action.Owner.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 32), action.StandardInfo.standardColor, Main.rand.NextFloat(1, 4));
            }
            if (!IsLocalProjectile) return;
            float m = 8 + 8 * action.Counter;
            for (int n = 0; n < m; n++)
            {
                float t = n / m;
                Projectile.NewProjectileDirect(action.Projectile.GetProjectileSource_FromThis(),
                    action.Owner.Center, default, ModContent.ProjectileType<PureFractalRotatingBlade>(),
                    action.CurrentDamage / 4, action.Projectile.damage, Main.myPlayer, t, action.Counter);
            }
        }
    }

    [AutoSync]
    [Obsolete]
    public class RotatingBladeSyncing : NetModule
    {
        public ushort projectileWhoAmI;
        public byte status;
        public Vector2 velocity;
        public byte hitCooldown;
        public byte extraUpdates;
        public byte timeLeft;
        public int damage;

        public static RotatingBladeSyncing Get(Projectile projectile)
        {
            return new RotatingBladeSyncing()
            {
                projectileWhoAmI = (ushort)projectile.whoAmI,
                status = (byte)projectile.ai[2],
                velocity = projectile.velocity,
                hitCooldown = (byte)projectile.localNPCHitCooldown,
                extraUpdates = (byte)projectile.extraUpdates,
                timeLeft = (byte)projectile.timeLeft,
                damage = projectile.damage,
            };
        }

        public override void Receive()
        {
            var proj = Main.projectile[projectileWhoAmI];
            proj.ai[2] = status;
            proj.velocity = velocity;
            proj.localNPCHitCooldown = hitCooldown;
            proj.extraUpdates = extraUpdates;
            proj.timeLeft = timeLeft;
            proj.damage = damage;

            if (Main.dedServ)
                Get(proj).Send(-1, Sender);
        }
    }

    [AutoSync]
    public class RotatingBladeShootActionSyncing : NetModule
    {
        public int projOwner;
        public static RotatingBladeShootActionSyncing Get(int owner)
        {
            var packet = NetModuleLoader.Get<RotatingBladeShootActionSyncing>();
            packet.projOwner = owner;
            return packet;
        }
        public override void Receive()
        {
            int type = ModContent.ProjectileType<PureFractalRotatingBlade>();
            foreach (var proj in Main.projectile)
            {
                if (proj.type == type && proj.owner == projOwner)
                {
                    proj.ai[2] = 2;
                    proj.velocity = proj.rotation.ToRotationVector2() * 16;
                    proj.localNPCHitCooldown = 0;
                    proj.extraUpdates = 3;
                    proj.timeLeft = 240;
                    proj.damage *= 4;
                }
            }
            if (Main.dedServ)
                Get(projOwner).Send(-1, Sender);
        }
    }
    public class PureFractalRotatingBlade : ModProjectile
    {
        //ai0控制每个弹幕的偏移量
        //ai1控制弹幕属于几阶
        //ai2控制弹幕处于哪个状态
        //frame控制弹幕贴图
        public override string Texture => "Terraria/Images/Item_0";

        public override bool PreDraw(ref Color lightColor)
        {
            var tex = PureFractalProj.ItemTextures[Projectile.frame];
            float alpha = 1;
            if (Projectile.ai[2] == 0 && Projectile.frameCounter < 15)
                alpha = Projectile.frameCounter / 15f;
            float scaler = 1;
            if ((int)Projectile.ai[1] is 4 or 5 && (int)Projectile.ai[2] == 1)
            {
                int counter = Projectile.frameCounter;
                float factor = Projectile.ai[0] + (MathF.Sqrt(256 + counter * counter) - 16) * .01f;
                float angle = factor * MathHelper.TwoPi;
                if (Projectile.ai[1] == 5)
                    angle += MathHelper.PiOver2;
                scaler += MathF.Abs(MathF.Sin(counter * .01f)) * MathF.Sin(angle) * .8f;
            }
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor * alpha, Projectile.rotation + MathHelper.PiOver4, tex.Size() * .5f, scaler, 0, 0);
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if ((int)Projectile.ai[1] is 4 or 5 && (int)Projectile.ai[2] == 1)
            {
                int counter = Projectile.frameCounter;
                float factor = Projectile.ai[0] + (MathF.Sqrt(256 + counter * counter) - 16) * .01f;
                float angle = factor * MathHelper.TwoPi;
                if (Projectile.ai[1] == 5)
                    angle += MathHelper.PiOver2;
                if (MathF.Sin(angle) < 0)
                    behindNPCs.Add(index);
                else
                    overPlayers.Add(index);
            }
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }

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

        public override void AI()
        {
            Projectile.hide = false;
            Projectile.frameCounter++;
            int counter = Projectile.frameCounter;
            Player plr = Main.player[Projectile.owner];
            switch ((int)Projectile.ai[2])
            {
                case 0:
                    {
                        Projectile.timeLeft = 60;
                        float factor = counter < 18 ? MathHelper.SmoothStep(0, 1.2f, counter / 18f) : MathHelper.SmoothStep(1.2f, 1, (counter - 18) / 12f);
                        float angle = Projectile.ai[0] * MathHelper.TwoPi;
                        switch ((int)Projectile.ai[1])
                        {
                            case 1:
                                {
                                    Projectile.Center = plr.Center + angle.ToRotationVector2() * 96 * factor;
                                    Projectile.rotation = angle;
                                    break;
                                }
                            case 2:
                                {
                                    angle = -angle;
                                    Projectile.Center = plr.Center + angle.ToRotationVector2() * 160 * factor;
                                    Projectile.rotation = angle + MathHelper.Pi;
                                    break;
                                }
                            case 3:
                                {
                                    Projectile.Center = plr.Center + (-angle).ToRotationVector2() * 224 * factor;
                                    Projectile.rotation = angle * 2;
                                    break;
                                }
                            case 4:
                                {
                                    Projectile.Center = plr.Center + angle.ToRotationVector2() * 320 * factor;
                                    Projectile.rotation = angle;
                                    break;
                                }
                            case 5:
                                {
                                    Projectile.Center = plr.Center + angle.ToRotationVector2() * 480 * factor;
                                    Projectile.rotation = angle;
                                    break;
                                }
                            default:
                                goto case 1;
                        }
                        if (Projectile.frameCounter == 30)
                        {
                            Projectile.ai[2] = 1;
                            Projectile.frameCounter = 0;
                        }
                        break;
                    }
                case 1:
                    {
                        Projectile.timeLeft = 60;
                        float factor = Projectile.ai[0] + (MathF.Sqrt(256 + counter * counter) - 16) * .01f;
                        switch ((int)Projectile.ai[1])
                        {
                            case 1:
                                {
                                    float angle = factor * MathHelper.TwoPi;
                                    Vector2 offset = angle.ToRotationVector2() * 96;
                                    Projectile.Center = plr.Center + offset;
                                    Projectile.rotation = angle;
                                    break;
                                }
                            case 2:
                                {
                                    float angle = -factor * MathHelper.TwoPi;
                                    Vector2 offset = angle.ToRotationVector2() * 160;
                                    Projectile.Center = plr.Center + offset;
                                    Projectile.rotation = angle + MathHelper.Pi;
                                    break;
                                }
                            case 3:
                                {
                                    float angle = factor * MathHelper.TwoPi;
                                    Vector2 offset = (-angle).ToRotationVector2() * 224;
                                    Projectile.Center = plr.Center + offset;
                                    Projectile.rotation = angle * 2 + counter * .025f;
                                    break;
                                }
                            case 4:
                                {
                                    Projectile.hide = true;
                                    float angle = factor * MathHelper.TwoPi;
                                    Vector2 offset = angle.ToRotationVector2() * 320 * new Vector2(1, MathF.Cos(counter * .01f));
                                    offset = offset.RotatedBy(counter * .005f);
                                    Projectile.Center = plr.Center + offset;
                                    if (offset != default)
                                        Projectile.rotation = offset.ToRotation();
                                    break;
                                }
                            case 5:
                                {
                                    Projectile.hide = true;
                                    float angle = -factor * MathHelper.TwoPi;
                                    Vector2 offset = angle.ToRotationVector2() * 480 * new Vector2(MathF.Cos(counter * .01f), 1);
                                    offset = offset.RotatedBy(-counter * .005f);
                                    Projectile.Center = plr.Center + offset;
                                    if (offset != default)
                                        Projectile.rotation = offset.ToRotation();
                                    break;
                                }
                            default:
                                goto case 1;
                        }
                        break;
                    }
            }
            if (Projectile.ai[2] != 2)
                Projectile.velocity = Projectile.rotation.ToRotationVector2();
            base.AI();
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.frame = reader.ReadByte();
            Projectile.frameCounter = reader.ReadInt32();
            base.ReceiveExtraAI(reader);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)Projectile.frame);
            writer.Write(Projectile.frameCounter);
            base.SendExtraAI(writer);
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(26);
        }
    }

    public class PureTestPlayer : ModPlayer
    {
        public bool PureTestPassed;

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            PureTestPassed = false;
            base.OnHitByNPC(npc, hurtInfo);
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            PureTestPassed = false;
            base.OnHitByProjectile(proj, hurtInfo);
        }

        public static Condition PureTestPassedCondition = new("Mods.FInalFractalSet.PureTest", () => Main.LocalPlayer.GetModPlayer<PureTestPlayer>().PureTestPassed);
    }

    public class PureTestGloablNPC : GlobalNPC
    {
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (npc.type == NPCID.MoonLordCore)
                Main.LocalPlayer.GetModPlayer<PureTestPlayer>().PureTestPassed = true;

            base.OnSpawn(npc, source);
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.MoonLordCore)
                npcLoot.Add(ItemDropRule.ByCondition(PureTestPlayer.PureTestPassedCondition.ToDropCondition(ShowItemDropInUI.Always), ModContent.ItemType<PureTestItem>()));

            base.ModifyNPCLoot(npc, npcLoot);
        }
    }

    public class PureTestItem : ModItem
    {
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            var color = drawColor with { A = 0 };
            float factor = (float)(Math.Cos(LogSpiralLibraryMod.ModTime * .05f) * .5 + .5);
            spriteBatch.Draw(TextureAssets.Item[Type].Value, position, frame, color * MathHelper.Lerp(.25f, .75f, factor), 0, origin, scale, 0, 0);

            color *= MathHelper.Lerp(.5f, .15f, factor);
            for (int n = 0; n < 3; n++)
                spriteBatch.Draw(TextureAssets.Item[Type].Value, position + (MathHelper.TwoPi / 3 * n + (float)LogSpiralLibraryMod.ModTime * .1f).ToRotationVector2() * 4, frame, color, 0, origin, scale, 0, 0);

            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            var color = lightColor with { A = 0 };
            float factor = (float)(Math.Cos(LogSpiralLibraryMod.ModTime * .05f) * .5 + .5);
            spriteBatch.Draw(TextureAssets.Item[Type].Value, Item.Center - Main.screenPosition, null, color * MathHelper.Lerp(.25f, .75f, factor), rotation, new Vector2(28), scale, 0, 0);

            color *= MathHelper.Lerp(.5f, .15f, factor);
            for (int n = 0; n < 3; n++)
                spriteBatch.Draw(TextureAssets.Item[Type].Value, Item.Center - Main.screenPosition + (MathHelper.TwoPi / 3 * n + (float)LogSpiralLibraryMod.ModTime * .1f).ToRotationVector2() * 4, null, color, rotation, new Vector2(28), scale, 0, 0);

            return false;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 56;
            Item.rare = ItemRarityID.Purple;

            base.SetDefaults();
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            base.SetStaticDefaults();
        }
    }
}