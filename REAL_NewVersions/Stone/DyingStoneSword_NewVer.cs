using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Melee;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using FinalFractalSet.REAL_NewVersions.Wood;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using FinalFractalSet.Weapons;
using Terraria;
using System.ComponentModel;
using System.Xml;

namespace FinalFractalSet.REAL_NewVersions.Stone
{
    public class DyingStoneSword_NewVer : MeleeSequenceItem<DyingStoneSword_NewVer_Proj>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 60;
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
        public override bool AltFunctionUse(Player player) => true;
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (player.HasBuff<StoneBuffBoost>())
                damage *= .75f;
            base.ModifyWeaponDamage(player, ref damage);
        }
    }
    public class DyingStoneSword_NewVer_Proj : MeleeSequenceProj
    {
        public override bool LabeledAsCompleted => true;
        public override string Texture => base.Texture.Replace("_Proj", "");
        public override StandardInfo StandardInfo => base.StandardInfo with
        {
            standardColor = Color.DeepSkyBlue * .25f,
            vertexStandard = new()
            {
                active = true,
                scaler = 100,
                timeLeft = 15,
                alphaFactor = 2f,
                //renderInfos = [[new AromrDyeInfo(ItemID.SilverDye)]]
            },
            itemType = ModContent.ItemType<DyingStoneSword_NewVer>()
        };
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (player.HasBuff<StoneBuffBoost>())
            {
                target.AddBuff(ModContent.BuffType<StoneBuff>(), Main.rand.Next(30, 90));
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center + Vector2.UnitY * 32, -Vector2.UnitY + Vector2.UnitX * Main.rand.NextFloat(-.5f, .5f), ModContent.ProjectileType<SharpStoneTears>(), Projectile.damage / 4, Projectile.knockBack, player.whoAmI, 0f, Main.rand.NextFloat() * 0.5f + 0.5f);
            }
            base.OnHitNPC(target, hit, damageDone);
        }
    }
    public class CrystalStoneSword_NewVer : MeleeSequenceItem<CrystalStoneSword_NewVer_Proj>
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 90;
        }
        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            recipe.AddIngredient<DyingStoneSword_NewVer>();
            recipe.AddIngredient(ItemID.BrokenHeroSword);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.ReplaceResult(this);
            recipe.Register();
        }
        public override bool AltFunctionUse(Player player) => true;
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (player.HasBuff<StoneBuffBoost>())
                damage *= .85f;
            base.ModifyWeaponDamage(player, ref damage);
        }
    }
    public class CrystalStoneSword_NewVer_Proj : MeleeSequenceProj
    {
        public override bool LabeledAsCompleted => true;
        public override string Texture => base.Texture.Replace("_Proj", "");
        public override StandardInfo StandardInfo => base.StandardInfo with
        {
            standardColor = Color.DeepSkyBlue * .5f,
            vertexStandard = new()
            {
                active = true,
                scaler = 120,
                timeLeft = 15,
                alphaFactor = 2f,
                //renderInfos = [[new AromrDyeInfo(ItemID.SilverDye)]]
            },
            itemType = ModContent.ItemType<DyingStoneSword_NewVer>()
        };
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (player.HasBuff<StoneBuffBoost>())
            {
                target.AddBuff(ModContent.BuffType<StoneBuff>(), Main.rand.Next(60, 120));
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center + Vector2.UnitY * 32, -Vector2.UnitY + Vector2.UnitX * Main.rand.NextFloat(-.5f, .5f), ModContent.ProjectileType<SharpStoneTears>(), Projectile.damage / 2, Projectile.knockBack, player.whoAmI, 0f, Main.rand.NextFloat() * 0.5f + 0.5f);
            }
            base.OnHitNPC(target, hit, damageDone);
        }
    }
    public class StoneSpecialAttack : FinalFractalSetAction
    {
        public override float CompositeArmRotation => base.CompositeArmRotation;
        public override float offsetRotation => base.offsetRotation + MathHelper.SmoothStep(MathHelper.Pi * Owner.direction, 0, MathHelper.Clamp((1 - Factor) / .25f, 0, 1));
        public override float offsetSize => base.offsetSize;
        public override Vector2 offsetCenter => base.offsetCenter;
        public override Vector2 offsetOrigin => base.offsetOrigin;
        public override float offsetDamage => base.offsetDamage;
        public override bool Attacktive => Factor < .75f;
        public override void OnEndSingle()
        {
            base.OnEndSingle();
        }
        public override void OnStartAttack()
        {
            float r = Main.rand.NextFloat(0, MathHelper.TwoPi);
            int[] indexs = [-1, -1, -1];
            float[] Dists = [float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity];
            foreach (var target in Main.npc)
            {
                if (target.friendly || !target.CanBeChasedBy() || !target.active) continue;
                float d = (target.Center - Owner.Center).Length();
                if (d > 1024) continue;
                for (int k = 0; k < 3; k++)
                {
                    if (d < Dists[k])
                    {
                        for (int j = 3 - 1 - k; j > 0; j--)
                        {
                            indexs[j] = indexs[j - 1];
                            Dists[j] = Dists[j - 1];
                        }
                        indexs[k] = target.whoAmI;
                        Dists[k] = d;
                        break;
                    }
                }
            }



            SoundEngine.PlaySound(MySoundID.Scythe);
            for (int n = 0; n < 3; n++)
            {
                Vector2 cen = Owner.Center + (r + MathHelper.TwoPi / 3 * n).ToRotationVector2() * 128;
                Vector2 velocity = default;
                if (indexs[n] == -1)
                {
                    if (Owner is Player plr)
                    {
                        velocity = plr.GetModPlayer<LogSpiralLibraryPlayer>().targetedMousePosition - cen;
                        velocity = velocity.SafeNormalize(default) * 16;
                    }
                }
                else
                {
                    velocity = Main.npc[indexs[n]].Center - cen;
                    velocity = velocity.SafeNormalize(default) * 16;
                }


                for (int k = 0; k < 20; k++)
                    Dust.NewDustPerfect(cen, MyDustId.GreyStone, Main.rand.NextVector2Circular(4, 4), 0, default, Main.rand.NextFloat(1, 2)).noGravity = true;
                Projectile.NewProjectile(Owner.GetSource_FromThis(), cen, velocity, ModContent.ProjectileType<StoneSAProjectile>(), CurrentDamage, Projectile.knockBack, Projectile.owner);
            }
            if (Upgraded) 
            {
                for (int n = 0; n < 3; n++)
                {
                    Vector2 cen = Owner.Center + (r + MathHelper.TwoPi / 3 * n + MathHelper.Pi / 3).ToRotationVector2() * 144;
                    Vector2 velocity = default;
                    if (indexs[n] == -1)
                    {
                        if (Owner is Player plr)
                        {
                            velocity = plr.GetModPlayer<LogSpiralLibraryPlayer>().targetedMousePosition - cen;
                            velocity = velocity.SafeNormalize(default) * 16;
                        }
                    }
                    else
                    {
                        velocity = Main.npc[indexs[n]].Center - cen;
                        velocity = velocity.SafeNormalize(default) * 16;
                    }


                    for (int k = 0; k < 20; k++)
                        Dust.NewDustPerfect(cen, MyDustId.GreyStone, Main.rand.NextVector2Circular(4, 4), 0, default, Main.rand.NextFloat(1, 2)).noGravity = true;
                    Projectile.NewProjectile(Owner.GetSource_FromThis(), cen, velocity, ModContent.ProjectileType<StoneSAProjectile>(), CurrentDamage, Projectile.knockBack, Projectile.owner);
                }
            }
            var u = UltraSwoosh.NewUltraSwoosh(standardInfo.standardColor, 60, 150, Owner.Center, null, !flip, Rotation, 2, (1.7f, -.2f), 3, 7);
            u.ModityAllRenderInfo([[new AirDistortEffectInfo(12)], [new ArmorDyeInfo(ItemID.FogboundDye)]]);

            if (Owner is Player player)
                player.AddBuff(ModContent.BuffType<StoneBuffBoost>(), 300);

            base.OnStartAttack();
        }
        public override void OnCharge()
        {

            base.OnCharge();
        }
        public override void OnStartSingle()
        {
            base.OnStartSingle();
        }
        public override void Update(bool triggered)
        {

            flip = Owner.direction == 1;
            base.Update(triggered);
        }
        [ElementCustomData]
        [DefaultValue(false)]
        public bool Upgraded;

        public override void LoadAttribute(XmlReader xmlReader)
        {
            Upgraded = bool.Parse(xmlReader[nameof(Upgraded)]);
            base.LoadAttribute(xmlReader);
        }
        public override void SaveAttribute(XmlWriter xmlWriter)
        {
            xmlWriter.WriteAttributeString(nameof(Upgraded), Upgraded.ToString());
            base.SaveAttribute(xmlWriter);
        }
    }
    public class StoneSAProjectile : ModProjectile
    {
        public override string Texture => base.Texture.Replace("StoneSAProjectile", "DyingStoneSword_NewVer");

        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 100;
            Projectile.hide = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.extraUpdates = 3;
            base.SetDefaults();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<StoneBuff>(), 300);
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.damage *= 3;
            Projectile.damage /= 4;

            Projectile.width = Projectile.height = 80;
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<StoneBuff>(), 60);
            base.OnHitPlayer(target, info);
        }
        public override void AI()
        {
            if (ultraStab != null)
            {
                ultraStab.center = Projectile.Center;
                ultraStab.autoUpdate = false;
                if (Projectile.ai[2] == 0)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation();
                    ultraStab.rotation = Projectile.rotation;
                    ultraStab.Uptate();
                    ultraStab.timeLeft = ultraStab.timeLeftMax;
                }
                ultraStab.Uptate();
                ultraStab.timeLeft = ultraStab.timeLeftMax;
            }
            Dust.NewDustPerfect(Projectile.Center, MyDustId.GreyStone, Main.rand.NextVector2Unit(), 0, default, Main.rand.NextFloat(.5f, 1.5f)).noGravity = true;
            base.AI();
        }
        public override void OnKill(int timeLeft)
        {
            ultraStab.timeLeft = 0;
            base.OnKill(timeLeft);
        }
        UltraStab ultraStab;
        public override void OnSpawn(IEntitySource source)
        {
            if (Main.netMode == NetmodeID.Server) return;
            ultraStab = UltraStab.NewUltraStab(Color.Gray, 30, 250, Projectile.Center, null, Main.rand.NextBool(2), 0, 4f);
            ultraStab.ModityAllRenderInfo([[new ArmorDyeInfo(ItemID.FogboundDye)]]);
            ultraStab.Uptate();
            ultraStab.timeLeft = 30;
            ultraStab.autoUpdate = false;
            base.OnSpawn(source);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
    public class StoneBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            base.SetStaticDefaults();
        }
        public override string Texture => $"Terraria/Images/Buff_{BuffID.WitheredArmor}";
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 10;
            npc.velocity *= .95f;
            if ((int)LogSpiralLibraryMod.ModTime2 % 3 == 0)
                Dust.NewDustPerfect(npc.Center, MyDustId.GreyStone, Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 2));
            base.Update(npc, ref buffIndex);
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.velocity *= .9f;
            player.lifeRegen -= 2;
            base.Update(player, ref buffIndex);
        }
    }
    public class StoneBuffBoost : ModBuff
    {
        public override string Texture => $"Terraria/Images/Buff_{BuffID.Stoned}";
        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance += 10;
            base.Update(player, ref buffIndex);
        }
    }
}
