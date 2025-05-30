﻿using FinalFractalSet.REAL_NewVersions.Zenith;
using LogSpiralLibrary.CodeLibrary.DataStructures.SequenceStructures.Contents.Melee.ExtendedMelee;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using System;
using Terraria.Audio;

namespace FinalFractalSet.REAL_NewVersions.Final;
public class FractalCloudMowingSword : StormInfo
{
    #region 辅助字段
    int HitCausedZenithCooldown;
    bool IsColliding;
    #endregion

    #region 重写属性
    public override string Category => "FinalFractal";

    public override float Factor => 1 - MathF.Pow(1 - base.Factor, 2);

    public override float offsetSize => IsColliding ? 4 : 1;

    public override bool Attacktive => base.Attacktive;

    public override bool OwnerHitCheek => false;
    #endregion

    #region 重写函数

    public override void OnStartAttack()
    {
        base.OnStartAttack();
        if (swoosh != null)
            swoosh.scaler *= 4f;
    }

    public override bool Collide(Rectangle rectangle)
    {
        IsColliding = true;
        var flag = base.Collide(rectangle);
        IsColliding = false;
        return flag;
    }


    public override void OnHitEntity(Entity victim, int damageDone, object[] context)
    {
        base.OnHitEntity(victim, damageDone, context);
        Projectile.localNPCHitCooldown = 0;
        if (HitCausedZenithCooldown <= 0)
        {
            int m = Main.rand.Next(0, Main.rand.Next(1, 3)) + 1;
            for (int n = 0; n < m; n++)
                FirstZenith_NewVer_Proj.ShootFirstZenithViaPosition(this, Rotation.ToRotationVector2() * 128 + Owner.Center, false);
            HitCausedZenithCooldown = 4;
        }
        else
            HitCausedZenithCooldown -= 2;
    }
    #endregion
}
public class FractalStrom : StormInfo
{
    public override string Category => "FinalFractal";

    #region 重写函数
    public override void OnStartAttack()
    {
        Owner.velocity += Rotation.ToRotationVector2() * 64;

        base.OnStartAttack();
    }

    public override void OnAttack()
    {
        if (timer % Math.Max(timerMax / 9, 1) == 0)
        {
            var velocity = Main.rand.NextVector2Unit() * 32;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(),
            Owner.Center - velocity * 30, velocity, ModContent.ProjectileType<FractalDash>(), CurrentDamage, Projectile.knockBack, Projectile.owner, 0, Main.rand.NextFloat(), 1);
        }
        base.OnAttack();
    }

    public override void OnHitEntity(Entity victim, int damageDone, object[] context)
    {
        base.OnHitEntity(victim, damageDone, context);
        Projectile.localNPCHitCooldown = 2;
    }
    #endregion
}
public class FractalTreeStab : PunctureInfo
{
    public override string Category => "FinalFractal";

    public override void OnBurst(float fallFac)
    {
        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Owner.Center + Vector2.UnitY * 8 + Vector2.UnitX * 16 * Owner.direction, default, ModContent.ProjectileType<PythagoreanTreeProj>(),
            CurrentDamage, Projectile.knockBack, Projectile.owner);
        base.OnBurst(fallFac);
    }
}
public class FractalChargingWing : ChargingInfo
{

    public override string Category => "FinalFractal";

    public override void OnDeactive()
    {
        base.OnDeactive();
        int type = ModContent.ProjectileType<FractalChargingWingProj>();
        foreach (var proj in Main.projectile)
        {
            if (proj.type == type && proj.ai[2] < 2)
            {
                proj.ai[2] = 2;
                proj.frameCounter = 0;
            }
        }

    }
    public override void OnStartAttack()
    {
        base.OnStartAttack();
        var action = this;
        SoundEngine.PlaySound(SoundID.Item84);
        for (int n = 0; n < 20; n++)
        {
            MiscMethods.FastDust(action.Owner.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 32), action.standardInfo.standardColor, Main.rand.NextFloat(1, 4));
        }
        float m = 4 * action.counter;
        //m = Math.Min(m, 12);
        for (int n = 0; n < m; n++)
        {
            float t = (n + 1f) / m;
            var proj = Projectile.NewProjectileDirect(action.Projectile.GetProjectileSource_FromThis(),
                action.Owner.Center, default, ModContent.ProjectileType<FractalChargingWingProj>(),
                action.CurrentDamage, action.Projectile.damage / 4, Main.myPlayer, t, action.counter);
            proj.netUpdate = true;
            proj.frame = 25;

            proj = Projectile.NewProjectileDirect(action.Projectile.GetProjectileSource_FromThis(),
    action.Owner.Center, default, ModContent.ProjectileType<FractalChargingWingProj>(),
    action.CurrentDamage, action.Projectile.damage / 4, Main.myPlayer, t + 1, action.counter);

            proj.netUpdate = true;
            proj.frame = 25;
        }
    }
}
public class FractalSnowFlake : FinalFractalSetAction
{
    public override string Category => "FinalFractal";
    public override bool Attacktive => Factor < .1f;
    public override float offsetRotation => -Rotation - MathHelper.PiOver2;
    public override bool Collide(Rectangle rectangle) => false;
    public override void OnStartAttack()
    {
        for (int n = 0; n < 60; n++)
        {
            var unit = Main.rand.NextVector2Unit();
            MiscMethods.FastDust(Owner.Center, unit * 16, Main.hslToRgb(Main.rand.NextFloat(.4f, .6f), 1, .75f));
        }
        for (int n = 0; n < 40; n++)
        {
            MiscMethods.FastDust(Owner.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 32), standardInfo.standardColor, Main.rand.NextFloat(1, 4));
            MiscMethods.FastDust(Owner.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 4) + (Rotation + offsetRotation).ToRotationVector2() * Main.rand.NextFloat(0, 64), standardInfo.standardColor, Main.rand.NextFloat(1, 2));

        }
        SoundEngine.PlaySound(SoundID.Item92);
        base.OnStartAttack();
    }
    public override void OnStartSingle()
    {
        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Owner.Center, default,
            ModContent.ProjectileType<FractalSnowFlakeProj>(), CurrentDamage, Projectile.knockBack, Projectile.owner);
        for (int n = 0; n < 60; n++)
        {
            var unit = Main.rand.NextVector2Unit();
            MiscMethods.FastDust(Owner.Center + unit * 256, -unit * 16, Main.hslToRgb(Main.rand.NextFloat(.4f, .6f), 1, .75f));
        }
        base.OnStartSingle();
    }
}
public class FractalJuilaSet : PunctureInfo
{
    public override string Category => "FinalFractal";
    public override void OnBurst(float fallFac)
    {
        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Owner.Center, default, ModContent.ProjectileType<FractalJuilaSetProj>(), CurrentDamage, Projectile.knockBack, Projectile.owner);
        base.OnBurst(fallFac);
    }
}