using FinalFractalSet.REAL_NewVersions.Wood;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing.RenderDrawingContents;
using LogSpiralLibrary.CodeLibrary.Utilties;
using LogSpiralLibrary.CodeLibrary.Utilties.BaseClasses;
using System;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.Utilities;
using static LogSpiralLibrary.LogSpiralLibraryMod;
using static Terraria.ModLoader.ModContent;
using static Terraria.Utils;

namespace FinalFractalSet.Weapons;

public class LightTree
{
    public class Node(float rad, float size, float length)
    {
        public float rad = rad, size = size, length = length;
        public List<Node> children = [];
        public Texture2D leafTex;
        public bool drawLeaf;
        public Rectangle leafFrame;
    };

    public Node root;
    private readonly UnifiedRandom random;

    public LightTree(UnifiedRandom random)
    {
        cnt = 0;
        root = null;
        this.random = random;
    }

    public LightTree(Node node, UnifiedRandom random)
    {
        cnt = 0;
        root = node;
        this.random = random;
    }

    private Vector2 target;
    private int cnt;
    public List<Vector2> keyPoints;

    public void SpawnProjectile(Projectile projectile, Vector2 position, Vector2 velocity, Node node, float chance = .2f)
    {
        float r = velocity.ToRotation();
        Vector2 unit = (r + node.rad).ToRotationVector2();
        if (Main.rand.NextFloat(0, 1) < chance)
        {
            bool flag = false;
            if (projectile.type == ModContent.ProjectileType<WitheredTree>())
                flag = (int)projectile.ai[0] / 2 == 1;

            var rand = flag ? Main.rand.Next(4) : 0;
            int index = -1;
            if (rand != 0 && Main.rand.NextBool(3))
            {
                foreach (var npc in Main.npc)
                {
                    if (npc.active && npc.CanBeChasedBy() && !npc.friendly && (npc.Center - position).Length() <= 768)
                    {
                        index = npc.whoAmI;
                        break;
                    }
                }
            }
            var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), position, new Vector2(this.Rand() * 2 - 1, 0) * 4, ModContent.ProjectileType<WitheredWood_NewVer>(), projectile.damage / 4, projectile.knockBack * .5f, projectile.owner, rand, index);
            proj.rotation = r + node.rad;//this.rand() * MathHelper.TwoPi
            if (proj.ModProjectile is WitheredWood_NewVer wood)
            {
                wood.tree = new LightTree(node, random);
            }
        }
        else
            foreach (var child in node.children)
            {
                SpawnProjectile(projectile, position + unit * node.length, unit, child, chance + Main.rand.NextFloat(-.05f, .15f));
            }
    }

    public void Generate(Vector2 pos, Vector2 vel, Vector2 target, bool hasLeaf)
    {
        // 根节点生成，朝向0，粗细1，长度随机50中选
        root = new Node(0, 1f, (Rand() * .25f + .75f) * 128);
        keyPoints = [];
        this.target = target;
        root = Build(root, pos, vel, true, hasLeaf);
        // Main.NewText($"生成了一个{cnt}个节点的树状结构");
    }

    public void Generate(Vector2 pos, Vector2 vel, Vector2 target, float lengthStart, bool hasLeaf)
    {
        // 根节点生成，朝向0，粗细1，长度随机50中选
        root = new Node(0, 1f, lengthStart);
        keyPoints = [];
        this.target = target;
        root = Build(root, pos, vel, true, hasLeaf);
        // Main.NewText($"生成了一个{cnt}个节点的树状结构");
    }

    public void Generate(Vector2 pos, Vector2 vel, Vector2 target, float lengthStart, float minSize, float minLength, float minDistance, float randAngleMain, float randAngleBranch, float chance, float decreaseSize, float decreaseLength, float decreaseSizeB, float decreaseLengthB, bool hasLeaf)
    {
        // 根节点生成，朝向0，粗细1，长度随机50中选
        root = new Node(0, 1f, lengthStart);
        keyPoints = [];
        this.target = target;
        root = Build(root, pos, vel, true, minSize, minLength, minDistance, randAngleMain, randAngleBranch, chance, decreaseSize, decreaseLength, decreaseSizeB, decreaseLengthB, hasLeaf);
        // Main.NewText($"生成了一个{cnt}个节点的树状结构");
    }

    private Node Build(Node node, Vector2 pos, Vector2 vel, bool root, bool hasLeaf)
    {
        keyPoints.Add(pos);
        cnt++;
        if (node.size < 0.1f || node.length < 1 || Vector2.Distance(pos, target) < 10)
        {
            if (hasLeaf && !Main.rand.NextBool(3))
            {
                try
                {
                    Main.instance.LoadTiles(5);
                    Main.instance.LoadGore(385);
                    Main.instance.LoadGore(384);
                }
                catch { }
                int index = Main.rand.Next(3);
                node.leafTex = index switch
                {
                    0 => TextureAssets.TreeBranch[9].Value,
                    1 => TextureAssets.Gore[385].Value,
                    _ or 2 => TextureAssets.Gore[384].Value
                };
                node.leafFrame = index switch
                {
                    0 => new Rectangle(42, 0, 42, 42),
                    1 => new Rectangle(0, 0, 36, 34),
                    _ or 2 => new Rectangle(0, 0, 40, 28)
                };
            }
            return node;
        }
        var r2 = (target - pos).ToRotation() - vel.ToRotation();
        var r = r2 * .5f + Rand(MathHelper.Pi / 4f) * 1.5f;
        //var r = rand(MathHelper.Pi / 4f);
        var unit = (vel.ToRotation() + r).ToRotationVector2();
        Node rchild = new(r, node.size * .9f, node.length * (cnt == 1 ? .25f : .975f));
        // 闪电树主节点（树干）
        node.children.Add(Build(rchild, pos + unit * node.length, unit, root, hasLeaf));
        if (root || Rand() > 0.35f * node.size * (1 - node.size) * 4)
        {
            for (int i = 0; i < 3; i++)
            {
                if (Rand() > 0.75f)
                {
                    r = Rand(MathHelper.Pi / 3f);
                    unit = (vel.ToRotation() + r).ToRotationVector2();
                    Node child = new(r, (.5f + Rand() * .5f) * node.size * 0.65f, node.length * (cnt == 1 ? .25f : .6f));
                    node.children.Add(Build(child, pos + unit * node.length, unit, false, hasLeaf));
                }
            }
        }
        return node;
    }

    private Node Build(Node node, Vector2 pos, Vector2 vel, bool root, float minSize, float minLength, float minDistance, float randAngleMain, float randAngleBranch, float chance, float decreaseSize, float decreaseLength, float decreaseSizeB, float decreaseLengthB, bool hasLeaf)
    {
        keyPoints.Add(pos);
        cnt++;
        if (node.size < minSize || node.length < minLength || Vector2.Distance(pos, target) < minDistance)
        {
            if (hasLeaf && !Main.rand.NextBool(3))
            {
                try
                {
                    Main.instance.LoadTiles(5);
                    Main.instance.LoadGore(385);
                    Main.instance.LoadGore(384);
                }
                catch { }
                int index = Main.rand.Next(3);
                node.leafTex = index switch
                {
                    0 => TextureAssets.TreeBranch[9].Value,
                    1 => TextureAssets.Gore[385].Value,
                    _ or 2 => TextureAssets.Gore[384].Value
                };
                node.leafFrame = index switch
                {
                    0 => new Rectangle(42, 0, 42, 42),
                    1 => new Rectangle(0, 0, 36, 34),
                    _ or 2 => new Rectangle(0, 0, 40, 28)
                };
            }
            return node;
        }
        var r2 = (target - pos).ToRotation() - vel.ToRotation();
        var r = r2 * .5f + Rand(randAngleMain) * 1.5f;
        //var r = rand(MathHelper.Pi / 4f);
        var unit = (vel.ToRotation() + r).ToRotationVector2();
        Node rchild = new(r, node.size * decreaseSize, node.length * (cnt == 1 ? .25f : decreaseLength));
        // 闪电树主节点（树干）
        node.children.Add(Build(rchild, pos + unit * node.length, unit, root, minSize, minLength, minDistance, randAngleMain, randAngleBranch, chance, decreaseSize, decreaseLength, decreaseSizeB, decreaseLengthB, hasLeaf));
        if (root || Rand() > chance * node.size * (1 - node.size) * 4)
        {
            for (int i = 0; i < 3; i++)
            {
                if (Rand() > 0.75f)
                {
                    r = Rand(randAngleBranch);
                    unit = (vel.ToRotation() + r).ToRotationVector2();
                    Node child = new(r, (.5f + Rand() * .5f) * node.size * decreaseSizeB, node.length * (cnt == 1 ? .25f : decreaseLengthB));
                    node.children.Add(Build(child, pos + unit * node.length, unit, false, minSize, minLength, minDistance, randAngleMain, randAngleBranch, chance, decreaseSize, decreaseLength, decreaseSizeB, decreaseLengthB, hasLeaf));
                }
            }
        }
        return node;
    }

    public float Rand()
    {
        double u = -2 * Math.Log(random.NextDouble());
        double v = 2 * Math.PI * random.NextDouble();
        return (float)Math.Max(0, Math.Sqrt(u) * Math.Cos(v) * 0.3 + 0.5);
    }

    private float Rand(float range)
    {
        return random.NextFloatDirection() * range;
    }

    public void Draw(SpriteBatch sb, Vector2 pos, Vector2 vel, float factor)
    {
        //sb.End();
        //sb.Begin(SpriteSortMode.Deferred, BlendState.Additive);
        //_draw(sb, pos, vel, root, Color.Cyan * 0.4f, 8f, factor);
        Draw(sb, pos, vel, root, Color.White, 16f, factor);// * 0.6f
        //sb.End();
        //sb.Begin();
    }

    public void Draw(SpriteBatch sb, Texture2D tex, Rectangle frame, float width, Texture2D branch, Vector2 pos, Vector2 vel, float factor)
    {
        Draw(sb, tex, frame, width, branch, pos, vel, root, Color.White, factor);
    }

    public void Draw(SpriteBatch sb, Texture2D branch, Vector2 pos, Vector2 vel, Color c, float width, float factor)
    {
        Draw(sb, branch, pos, vel, root, c, width, factor);
    }

    public void Draw(SpriteBatch sb, Vector2 pos, Vector2 vel, Color c, float width, float factor)
    {
        Draw(sb, pos, vel, root, c, width, factor);
    }

    public void SpawnDust(Vector2 pos, Vector2 vel)
    {
        Dust(pos, vel, root);
    }

    private static void Draw(SpriteBatch sb, Texture2D branch, Vector2 pos, Vector2 vel, Node node, Color c, float width, float factor)
    {
        // 树枝实际的方向向量
        Vector2 unit = (vel.ToRotation() + node.rad).ToRotationVector2();
        // 类似激光的线性绘制方法，绘制出树枝

        //TODO 绘制树

        //for (float i = 0; i <= node.length; i += 0.3f)
        //    sb.Draw(Main.magicPixel, pos + unit * i, new Rectangle(0, 0, 1, 1), c, 0,
        //        new Vector2(0.5f, 0.5f), Math.Max(node.size * factor, 0.3f), SpriteEffects.None, 0f);
        // 递归到子节点进行绘制
        var _fac = MathHelper.Clamp(factor, 0, 1);
        try
        {
            if (!TextureAssets.Tile[5].IsLoaded) Main.instance.LoadTiles(5);
        }
        catch { }
        var tex = TextureAssets.Tile[5].Value;
        sb.Draw(tex, pos + unit * node.length * .5f * _fac, new Rectangle(0, 0, 16, 20), c, vel.ToRotation() + node.rad + MathHelper.PiOver2, new Vector2(8, 10 * _fac), new Vector2(width * MathF.Sqrt(node.size) / 16f, node.length / 20f * 1.05f) * _fac, 0, 0);
        //var tex = TextureAssets.Item[664].Value;
        //sb.Draw(tex, pos + unit * node.length * .5f * _fac, null, c, vel.ToRotation() + node.rad, new Vector2(8 * _fac, 8), new Vector2(node.length / 16f, width * node.size / 16f) * _fac, 0, 0);
        //sb.Draw(TextureAssets.MagicPixel.Value, pos + unit * node.length * .5f * _fac, new Rectangle(0, 0, 1, 1), c, vel.ToRotation() + node.rad, new Vector2(.5f * _fac, .5f), new Vector2(node.length, width * node.size) * _fac, 0, 0);
        if (branch != null && (node.children == null || node.children.Count == 0))
        {
            _fac = MathHelper.Clamp(factor - 1, 0, 1);
            //tex = branch ?? TextureAssets.TreeBranch[9].Value;
            sb.Draw(branch, pos + unit * node.length, new Rectangle(42, 0, 42, 42), c, vel.ToRotation() + node.rad, new Vector2(2, 22), width * MathF.Sqrt(node.size) * _fac / 4f, 0, 0);
            sb.Draw(branch, pos + unit * node.length, new Rectangle(42, 0, 42, 42), c, vel.ToRotation() + node.rad + MathHelper.Pi / 6, new Vector2(2, 22), width * MathF.Sqrt(node.size) * _fac / 8f, 0, 0);
            sb.Draw(branch, pos + unit * node.length, new Rectangle(42, 0, 42, 42), c, vel.ToRotation() + node.rad - MathHelper.Pi / 6, new Vector2(2, 22), width * MathF.Sqrt(node.size) * _fac / 8f, 0, 0);
        }
        else
            foreach (var child in node.children)
            {
                // 传递给子节点真实的位置和方向向量
                if (factor > 1)
                    Draw(sb, branch, pos + unit * node.length, unit, child, c, width, factor - 1);//.RotatedBy(MathF.Cos((float)Main.time / 24f) * MathHelper.PiOver4 / 3)
                factor -= .1f;
            }
    }

    private static void Draw(SpriteBatch sb, Texture2D tex, Rectangle frame, float width, Texture2D branch, Vector2 pos, Vector2 vel, Node node, Color c, float factor)
    {
        // 树枝实际的方向向量
        Vector2 unit = (vel.ToRotation() + node.rad).ToRotationVector2();
        // 类似激光的线性绘制方法，绘制出树枝

        //TODO 绘制树

        //for (float i = 0; i <= node.length; i += 0.3f)
        //    sb.Draw(Main.magicPixel, pos + unit * i, new Rectangle(0, 0, 1, 1), c, 0,
        //        new Vector2(0.5f, 0.5f), Math.Max(node.size * factor, 0.3f), SpriteEffects.None, 0f);
        // 递归到子节点进行绘制
        var _fac = MathHelper.Clamp(factor, 0, 1);
        //var tex = TextureAssets.Tile[5].Value;
        sb.Draw(tex, pos + unit * node.length * .5f * _fac, frame, c, vel.ToRotation() + node.rad + MathHelper.PiOver2, new Vector2(frame.Width * .5f, frame.Height * .5f * _fac), new Vector2(width * MathF.Sqrt(node.size) / frame.Width, node.length / frame.Height * 1.05f) * _fac, 0, 0);
        //var tex = TextureAssets.Item[664].Value;
        //sb.Draw(tex, pos + unit * node.length * .5f * _fac, null, c, vel.ToRotation() + node.rad, new Vector2(8 * _fac, 8), new Vector2(node.length / 16f, width * node.size / 16f) * _fac, 0, 0);
        //sb.Draw(TextureAssets.MagicPixel.Value, pos + unit * node.length * .5f * _fac, new Rectangle(0, 0, 1, 1), c, vel.ToRotation() + node.rad, new Vector2(.5f * _fac, .5f), new Vector2(node.length, width * node.size) * _fac, 0, 0);
        if (node.children == null || node.children.Count == 0)
        {
            _fac = MathHelper.Clamp(factor - 1, 0, 1);
            sb.Draw(branch, pos + unit * node.length, new Rectangle(42, 0, 42, 42), c, vel.ToRotation() + node.rad, new Vector2(2, 22), width * MathF.Sqrt(node.size) * _fac / 4f, 0, 0);
            sb.Draw(branch, pos + unit * node.length, new Rectangle(42, 0, 42, 42), c, vel.ToRotation() + node.rad + MathHelper.Pi / 6, new Vector2(2, 22), width * MathF.Sqrt(node.size) * _fac / 8f, 0, 0);
            sb.Draw(branch, pos + unit * node.length, new Rectangle(42, 0, 42, 42), c, vel.ToRotation() + node.rad - MathHelper.Pi / 6, new Vector2(2, 22), width * MathF.Sqrt(node.size) * _fac / 8f, 0, 0);
        }
        else
            foreach (var child in node.children)
            {
                // 传递给子节点真实的位置和方向向量
                if (factor > 1)
                    Draw(sb, tex, frame, width, branch, pos + unit * node.length, unit, child, c, factor - 1);//
                factor -= .1f;
            }
    }

    private static void Draw(SpriteBatch sb, Vector2 pos, Vector2 vel, Node node, Color c, float width, float factor)
    {
        // 树枝实际的方向向量
        Vector2 unit = (vel.ToRotation() + node.rad).ToRotationVector2();
        var _fac = MathHelper.Clamp(factor, 0, 1);
        var tex = TextureAssets.Tile[5].Value;
        var branch = node.leafTex;
        sb.Draw(tex, pos + unit * node.length * .5f * _fac, new Rectangle(0, 0, 16, 20), c, vel.ToRotation() + node.rad + MathHelper.PiOver2, new Vector2(8, 10 * _fac), new Vector2(width * MathF.Sqrt(node.size) / 16f, node.length / 20f * 1.05f) * _fac, 0, 0);
        if (branch != null && (node.children == null || node.children.Count == 0))
        {
            _fac = MathHelper.Clamp(factor - .33f, 0, 1);
            //tex = branch ?? TextureAssets.TreeBranch[9].Value;
            sb.Draw(branch, pos + unit * node.length, node.leafFrame, c, vel.ToRotation() + node.rad, new Vector2(2, 22), width * MathF.Sqrt(node.size) * _fac / 4f, 0, 0);
            sb.Draw(branch, pos + unit * node.length, node.leafFrame, c, vel.ToRotation() + node.rad + MathHelper.Pi / 6, new Vector2(2, 22), width * MathF.Sqrt(node.size) * _fac / 8f, 0, 0);
            sb.Draw(branch, pos + unit * node.length, node.leafFrame, c, vel.ToRotation() + node.rad - MathHelper.Pi / 6, new Vector2(2, 22), width * MathF.Sqrt(node.size) * _fac / 8f, 0, 0);
        }
        else
            foreach (var child in node.children)
            {
                // 传递给子节点真实的位置和方向向量
                if (factor > 1)
                    Draw(sb, pos + unit * node.length, unit, child, c, width, factor - 1);//.RotatedBy(MathF.Cos((float)Main.time / 24f) * MathHelper.PiOver4 / 3)
                factor -= .1f;
            }
    }

    private void Dust(Vector2 pos, Vector2 vel, Node node)
    {
        float r = vel.ToRotation();
        Vector2 unit = (r + node.rad).ToRotationVector2();
        for (float i = 0; i <= node.length; i += 4f)
        {
            var dust = Terraria.Dust.NewDustDirect(pos + unit * i, 0, 0,
                MyDustId.Wood, 0, 0, 100, Color.White, 1.5f);
            //dust.noGravity = true;
            dust.velocity *= 0.25f;
            dust.velocity += new Vector2(Rand() * 2 - 1, 2);

            dust.position = pos + unit * i;
        }
        foreach (var child in node.children)
        {
            Dust(pos + unit * node.length, unit, child);
        }
    }

    public bool Check(Rectangle hitbox, Vector2 pos, Vector2 vel, float factor)
    {
        return Check(hitbox, pos, vel, root, factor);
    }

    private static bool Check(Rectangle hitbox, Vector2 pos, Vector2 vel, Node node, float factor)
    {
        Vector2 unit = (vel.ToRotation() + node.rad).ToRotationVector2();
        var _fac = MathHelper.Clamp(factor, 0, 1);
        if (hitbox.Contains(pos.ToPoint()) || hitbox.Contains((pos + unit * node.length * .5f * _fac).ToPoint())) return true;
        else
            foreach (var child in node.children)
            {
                if (factor < 1) return false;
                if (Check(hitbox, pos + unit * node.length, unit.RotatedBy(MathF.Cos((float)Main.time / 24f) * MathHelper.PiOver4 / 3), child, factor - 1)) return true;
                factor -= .1f;
            }
        return false;
    }

    public bool Check(Rectangle hitbox)
    {
        foreach (var pt in keyPoints)
            if (hitbox.Contains(pt.ToPoint())) return true;
        return false;
    }
}

public class WitheredWoodSword : ModItem
{
    public override bool IsLoadingEnabled(Mod mod) => false;

    public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < 1;

    public override void SetDefaults()
    {
        Item.damage = 30;
        Item.crit = 21;
        Item.width = 50;
        Item.height = 54;
        Item.rare = ItemRarityID.LightRed;
        Item.useTime = 30;
        Item.useAnimation = 30;
        Item.knockBack = 6;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.autoReuse = true;
        Item.noUseGraphic = true;
        Item.DamageType = DamageClass.Melee;
        Item.channel = true;
        Item.noMelee = true;
        Item.shootSpeed = 16f;
        Item.value = Item.sellPrice(0, 0, 0, 0);
        Item.shoot = ProjectileType<WitheredWoodSword_Blade>();
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

    public override bool AltFunctionUse(Player player)
    {
        return true;
    }
}

public class LivingWoodSword : WitheredWoodSword
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.width = 52;
        Item.height = 70;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.damage = 60;
        Item.rare = ItemRarityID.Yellow;
    }

    public override void AddRecipes()
    {
        var recipe = CreateRecipe();
        recipe.AddIngredient<WitheredWoodSword>();
        recipe.AddIngredient(ItemID.BrokenHeroSword);
        recipe.AddTile(TileID.MythrilAnvil);
        recipe.ReplaceResult(this);
        recipe.Register();
    }
}

public class WitheredWoodSword_Blade : HandMeleeProj
{
    public virtual T UpgradeValue<T>(T normal, T extra, T defaultValue = default)
        => sourceItem.ModItem switch
        {
            LivingWoodSword => extra,
            WitheredWoodSword => normal,
            _ => defaultValue
        };

    public override bool Charged => base.Charged;
    public override bool Charging => base.Charging;
    public override SpriteEffects flip => Player.direction == -1 ^ (controlState == 1 && controlTier % 2 == 1) ? SpriteEffects.FlipHorizontally : 0;
    public override Rectangle? frame => projTex.Frame(2, 1, UpgradeValue(0, 1));//
    public override Vector2 projCenter => base.projCenter;
    public override Vector2 scale => base.scale;

    public override float timeCount
    {
        get => base.timeCount;
        set => base.timeCount = value;
    }

    public override string ProjName => "WitheredWoodSword";

    public override float MaxTime => controlState == 2 ? 48 : (controlTier % 5) switch
    {
        4 => 32,
        _ => 16
    };

    public override float factor => base.factor;
    public override Vector2 CollidingSize => base.CollidingSize;
    public override Vector2 CollidingCenter => base.CollidingCenter;
    public override Vector2 DrawOrigin => new(12, size.Y / FrameMax.Y - 12);
    public override Color color => base.color;
    public override float MaxTimeLeft => 8;

    public override float RealRotation
    {
        get
        {
            if (controlState == 1)
            {
                int tier = controlTier;
                float k = 3f;
                float _factor = factor;
                if (controlTier % 5 == 4)
                {
                    float key = MaxTime - MaxTimeLeft;
                    _factor = timeCount < key ? GetLerpValue(0, key, timeCount) * .5f : GetLerpValue(key, MaxTime, timeCount) * .5f + .5f;
                    _factor = (k - 1) * _factor * _factor + (2 - k) * _factor;
                    if (tier % 2 == 1) _factor = 1 - _factor;
                    return MathHelper.Lerp(MathHelper.Pi * 6 / 8, -MathHelper.Pi * 6 / 8, _factor);
                }
                else
                {
                    _factor = MathF.Pow(_factor, 2);
                }

                if (tier % 2 == 1) _factor = 1 - _factor;
                return MathHelper.Hermite(MathHelper.Pi * 6 / 8, 0, -MathHelper.Pi * 6 / 8, 0, _factor);
            }
            return base.RealRotation;
        }
    }

    public override bool UseRight => true;
    public override bool UseLeft => true;
    public override (int X, int Y) FrameMax => (2, 1);

    public override void OnKill(int timeLeft)
    {
        if (Charged && Player.CheckMana(50, true))
        {
            var cen = projCenter - (CollidingCenter - DrawOrigin).RotatedBy(RealRotation) * new Vector2(Player.direction, 1) * 1.5f + new Vector2(0, -24);
            Projectile.NewProjectile(projectile.GetSource_FromThis(), cen, default, ProjectileType<WitheredTree>(), Projectile.damage * 4, Projectile.knockBack, Projectile.owner, UpgradeValue(1, 3));
            for (int n = 0; n < 30; n++)
            {
                Dust.NewDustPerfect(cen, UpgradeValue(MyDustId.Wood, MyDustId.GreenGrass), (MathHelper.TwoPi / 30 * n).ToRotationVector2() * Main.rand.NextFloat(2, 8));
            }
        }
    }

    public override void OnEndAttack()
    {
        bool large = controlState == 2 || controlTier % 5 == 4;

        var u = UltraSwoosh.NewUltraSwooshOnDefaultCanvas(large ? 30 : 15, TextureAssets.Item[Player.HeldItem.type].Size().Length(), Player.Center, (Player.direction == 1 ? -1.125f : 2.125f, Player.direction == 1 ? 3f / 8 : 0.625f));
        u.negativeDir = controlTier % 2 == 1;
        u.rotation = 0;
        u.xScaler = large ? 1.25f : 1f;
        base.OnEndAttack();
    }

    public override void OnCharging(bool left, bool right)
    {
        SACoolDown--;
        if (left)
        {
            projectile.ai[0]++;
            if (projectile.ai[0] >= MaxTime)
            {
                OnEndAttack();
            }
        }
        else
        {
            projectile.ai[0] += projectile.ai[0] < MaxTime ? 1 : 0;
        }

        var when = controlTier % 5 == 4 ? (int)(MaxTime - MaxTimeLeft) : (int)MaxTime / 4;
        if ((int)projectile.ai[0] == when && left)
        {
            SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
        }
        projectile.friendly = projectile.ai[0] > when;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.immune[projectile.owner] = 5;
        if (SACoolDown < 0 && Main.rand.NextBool(controlTier % 5 == 4 ? 2 : 5))
        {
            var cen = target.Center + new Vector2(0, 24);
            Projectile.NewProjectile(projectile.GetSource_FromThis(), cen, default, ProjectileType<WitheredTree>(), Projectile.damage * 2, Projectile.knockBack, Projectile.owner, UpgradeValue(0, 2));
            SACoolDown = 15;
            for (int n = 0; n < 30; n++)
            {
                Dust.NewDustPerfect(cen, UpgradeValue(MyDustId.Wood, MyDustId.GreenGrass), (MathHelper.TwoPi / 30 * n).ToRotationVector2() * Main.rand.NextFloat(2, 8));
            }
        }
        base.OnHitNPC(target, hit, damageDone);
    }

    public override void OnRelease(bool charged, bool left)
    {
        SACoolDown--;
        base.OnRelease(charged, left);
    }

    public Item sourceItem;
    public int SACoolDown;

    public override void OnSpawn(IEntitySource source)
    {
        if (source is EntitySource_ItemUse_WithAmmo itemSource)
        {
            sourceItem = itemSource.Item;
        }
    }
}

public class WitheredTree : ModProjectile
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
        tree.Generate(Projectile.Center, new Vector2(0, -.5f), new Vector2(0, -2048) + Projectile.Center, ((int)Projectile.ai[0] % 2 == 0 ? 64 : 128) * (tree.Rand() * .25f + .75f), Projectile.ai[0] > 1);
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

public class WitheredWood : ModProjectile
{
    public override string Texture => "Terraria/Images/Item_1";

    public override void SetDefaults()
    {
        Projectile.timeLeft = 120;
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
                if (tree.Rand() < .5f)
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
        //try
        //{
        //    if (!TextureAssets.Tile[5].IsLoaded) Main.instance.LoadTiles(5);
        //    if (!TextureAssets.Gore[385].IsLoaded) Main.instance.LoadGore(385);
        //    if (!TextureAssets.Gore[384].IsLoaded) Main.instance.LoadGore(384);
        //}
        //catch { }

        //Texture2D tex = (int)Projectile.ai[0] switch
        //{
        //    0 or 1 => TextureAssets.Tile[5].Value,
        //    2 => TextureAssets.TreeBranch[9].Value,
        //    3 => TextureAssets.Gore[385].Value,
        //    _ or 4 => TextureAssets.Gore[384].Value
        //};
        //Rectangle frame = (int)Projectile.ai[0] switch
        //{
        //    0 or 1 => new Rectangle(66, 0, 22, 22),
        //    2 => new Rectangle(42, 0, 42, 42),
        //    3 => new Rectangle(0, 0, 36, 34),
        //    _ or 4 => new Rectangle(0, 0, 40, 28)
        //};
        //Color color = (int)Projectile.ai[0] switch
        //{
        //    0 or 1 => Color.Brown,
        //    2 or 3 => Color.Green,
        //    _ or 4 => Color.Pink
        //};
        //for (int n = 9; n > -1; n--)
        //{
        //    var c = Lighting.GetColor((Projectile.Center / 16).ToPoint(), n == 0 ? Color.White : color);
        //    if (n != 0) c = c with { A = 0 } * (1 - n * .1f) * .5f;
        //    Main.EntitySpriteDraw(tex, Projectile.oldPos[n] - Main.screenPosition, frame, c, Projectile.oldRot[n], frame.Size() * .5f, (1 - n * .1f) * 1.5f, 0, 0);// * 1.5f//with { A = 0 } * (1 - n * .1f) * .5f)

        //}
        for (int n = 9; n > -1; n--)
        {
            var c = Lighting.GetColor((Projectile.Center / 16).ToPoint(), Color.White);// n == 0 ? Color.White : color
            if (n != 0)
            {
                var fac = (1 - n * .1f) * .5f;
                c = c * fac * fac;
                c.A = (byte)(c.A * (9 - n) / 9f);
            }
            tree?.Draw(Main.spriteBatch, Projectile.oldPos[n] - Main.screenPosition, Projectile.oldRot[n].ToRotationVector2(), c, 16f, 10);
        }
        return false;
    }
}

public class WitheredWoodSword_Blade_GivenUp : VertexHammerProj
{
    public virtual T UpgradeValue<T>(T normal, T extra, T defaultValue = default)
    {
        //var type = Player.HeldItem.type;
        var type = sourceItem.type;

        if (type == ItemType<WitheredWoodSword>())
        {
            return normal;
        }

        if (type == ItemType<LivingWoodSword>())
        {
            return extra;
        }

        return defaultValue;
    }

    public override bool Charged => base.Charged;
    public override bool Charging => base.Charging;
    public override SpriteEffects flip => (int)((timeCount - 1) / MaxTime) % 2 == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
    public override Rectangle? frame => projTex.Frame(2, 1, UpgradeValue(0, 1));//
    public override Vector2 projCenter => base.projCenter;
    public override bool RedrawSelf => base.RedrawSelf;
    public override Vector2 scale => base.scale;

    public override float timeCount
    {
        get => controlState == 2 ? base.timeCount : projectile.ai[0];
        set
        {
            if (controlState == 2)
                base.timeCount = value;
            else
            {
                projectile.ai[0] = value;
            }
        }
    }

    public override bool WhenVertexDraw => base.WhenVertexDraw;
    public override string Texture => base.Texture.Replace("_Blade_GivenUp", "Proj");
    public override string HammerName => "WitheredWoodSword";
    public override float MaxTime => 16;
    public override float Factor => (projectile.ai[0] % MaxTime == 0 && projectile.ai[0] > 0 ? MaxTime - 1 : projectile.ai[0] % MaxTime) / MaxTime;
    public override Vector2 CollidingSize => base.CollidingSize;

    //public override Vector2 projCenter => base.projCenter + new Vector2(Player.direction * 16, -16);
    public override Vector2 CollidingCenter => base.CollidingCenter;//new Vector2(projTex.Size().X / 3 - 16, 16)

    public override Vector2 DrawOrigin => base.DrawOrigin + new Vector2(-12, 12);
    public override Color color => base.color;

    //public override Color VertexColor(float time) => Color.Lerp(Color.DarkGreen, UpgradeValue(Color.Brown, Color.Green), time);//Color.Lerp(UpgradeValue(Color.Brown, Color.Green), Color.DarkGreen, time)
    public override float MaxTimeLeft => 8;

    public override float Rotation
    {
        get
        {
            var theta = float.Lerp(MathHelper.Pi / 8 * 3 * (projectile.ai[0] > MaxTime ? -1 : 1), -MathHelper.PiOver2 - MathHelper.Pi / 8, Math.Clamp((float)Math.Pow(Factor, 2), 0, 1));
            if (projectile.ai[1] > 0)
            {
                if (Charged)
                {
                    theta = float.Lerp(theta, MathHelper.Pi / 8 * 3, Math.Clamp(projectile.ai[1] / MaxTimeLeft / Factor, 0, 1));
                }
                else
                {
                    theta = float.Lerp(MathHelper.Pi / 8 * 3, -MathHelper.PiOver2, Math.Clamp((timeCount - projectile.ai[1]) / MaxTime, 0, 1));
                }
            }
            // theta = -MathHelper.PiOver2;
            //if(timeCount / MaxTime % 2 == 1)theta = -theta;
            return Player.direction == -1 ? MathHelper.Pi * 1.5f - theta : theta;
        }
    }

    public override bool UseRight => true;
    public override bool UseLeft => true;
    public override (int X, int Y) FrameMax => (2, 1);

    public override void OnKill(int timeLeft)
    {
        //if (factor == 1)
        //{
        //    Projectile.NewProjectile(projectile.GetSource_FromThis(), vec, default, ModContent.ProjectileType<HolyExp>(), player.GetWeaponDamage(player.HeldItem) * 3, projectile.knockBack, projectile.owner);
        //}
        if (Charged || controlState == 1 && (timeCount - 3) % MaxTime >= MaxTime * .75f)
        {
            bool large = controlState == 2 || (int)(timeCount / MaxTime) % 5 == 4;
            var u = UltraSwoosh.NewUltraSwooshOnDefaultCanvas(large ? 30 : 15, TextureAssets.Item[Player.HeldItem.type].Size().Length() * (large ? 2 : 1), Player.Center, (Player.direction == 1 ? -1.125f : 2.125f, Player.direction == 1 ? 3f / 8 : 0.625f));
            u.negativeDir = false;
            u.rotation = large ? MathHelper.Pi : 0;
            u.xScaler = large ? 1.25f : 1f;
        }
    }

    public override void OnCharging(bool left, bool right)
    {
        if (left)
        {
            if (timeCount % MaxTime == MaxTime - 1)
            {
                projectile.ai[1]++;
            }
        }
    }

    public override void OnRelease(bool charged, bool left)
    {
        if (Charged)
        {
            if ((int)projectile.ai[1] == 1)
            {
                OnChargedShoot();
            }
        }
        if ((int)projectile.ai[1] == 1)
        {
            projectile.damage = 0;
            if (Charged || controlState == 1 && timeCount % MaxTime >= MaxTime * .75f)
            {
                projectile.damage = (int)(Player.GetWeaponDamage(Player.HeldItem) * (controlState == 2 || (int)(timeCount / MaxTime) % 4 == 3 ? 4f : 2f));
                SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
            }
        }
        projectile.ai[1]++;
        if (projectile.ai[1] > (Charged ? MaxTimeLeft * Factor : timeCount % MaxTime))
        {
            if (left && Player.controlUseItem)
            {
                projectile.ai[1] = 0;
                projectile.ai[0]++;
                OnKill(projectile.timeLeft);
            }
            else
            {
                projectile.Kill();
            }
        }
    }

    public Item sourceItem;

    public override void OnSpawn(IEntitySource source)
    {
        if (source is EntitySource_ItemUse_WithAmmo itemSource)
        {
            sourceItem = itemSource.Item;
        }
        base.OnSpawn(source);
    }

    public override void VertexInfomation(ref bool additive, ref int indexOfGreyTex, ref float endAngle, ref bool useHeatMap, ref int p)
    {
        p = 2;
    }
}

public class DyingStoneSword : WitheredWoodSword
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.damage = 40;
        Item.crit = 26;
        Item.DamageType = DamageClass.Melee;
        Item.width = 48;
        Item.height = 48;
        Item.rare = ItemRarityID.Pink;
        Item.useTime = 25;
        Item.useAnimation = 25;
        Item.knockBack = 8;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.autoReuse = true;
        Item.shoot = ProjectileType<DyingStoneSword_Blade>();
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

public class CrystalStoneSword : DyingStoneSword
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.damage = 70;
        Item.width = 50;
        Item.rare = ItemRarityID.Yellow;
        Item.useTime = 18;
        Item.useAnimation = 18;
    }

    public override void AddRecipes()
    {
        var recipe = CreateRecipe();
        recipe.AddIngredient<DyingStoneSword>();
        recipe.AddIngredient(ItemID.BrokenHeroSword);
        recipe.AddTile(TileID.MythrilAnvil);
        recipe.ReplaceResult(this);
        recipe.Register();
    }
}

public class DyingStoneSword_Blade : WitheredWoodSword_Blade
{
    public override bool Charged => base.Charged;
    public override bool Charging => base.Charging;
    public override SpriteEffects flip => Player.direction == -1 ^ (controlState == 1 && controlTier % 2 == 1) ? SpriteEffects.FlipHorizontally : 0;//(int)(projectile.ai[0] / MaxTime)
    public override Rectangle? frame => projTex.Frame(2, 1, UpgradeValue(0, 1));//
    public override Vector2 projCenter => base.projCenter;
    public override Vector2 scale => base.scale;

    public override float timeCount
    {
        get => base.timeCount;
        set => base.timeCount = value;
    }

    //public override string Texture => base.Texture.Replace("_Blade", "Proj");
    public override string ProjName => "WitheredWoodSword";

    public override float MaxTime => controlState == 2 ? 48 : (controlTier % 5) switch
    {
        4 => 32,
        _ => 16
    };

    public override float factor => base.factor;
    public override Vector2 CollidingSize => base.CollidingSize;

    //public override Vector2 projCenter => base.projCenter + new Vector2(Player.direction * 16, -16);
    public override Vector2 CollidingCenter => base.CollidingCenter;//new Vector2(projTex.Size().X / 3 - 16, 16)

    public override Vector2 DrawOrigin => new(12, size.Y / FrameMax.Y - 12);// + new Vector2(-12, 12)
    public override Color color => base.color;

    //public override Color VertexColor(float time) => Color.Lerp(Color.DarkGreen, UpgradeValue(Color.Brown, Color.Green), time);//Color.Lerp(UpgradeValue(Color.Brown, Color.Green), Color.DarkGreen, time)
    public override float MaxTimeLeft => 8;

    public override float RealRotation
    {
        get
        {
            //return MathHelper.PiOver4;
            if (controlState == 1)
            {
                int tier = controlTier;//(int)(projectile.ai[0] / MaxTime)
                float k = 3f;
                float _factor = factor;
                if (controlTier % 5 == 4)
                {
                    float key = MaxTime - MaxTimeLeft;
                    _factor = timeCount < key ? GetLerpValue(0, key, timeCount) * .5f : GetLerpValue(key, MaxTime, timeCount) * .5f + .5f;
                    _factor = (k - 1) * _factor * _factor + (2 - k) * _factor;
                    if (tier % 2 == 1) _factor = 1 - _factor;
                    return MathHelper.Lerp(MathHelper.Pi * 6 / 8, -MathHelper.Pi * 6 / 8, _factor);
                }
                else
                {
                    _factor = MathF.Pow(_factor, 2);
                }

                if (tier % 2 == 1) _factor = 1 - _factor;
                return MathHelper.Hermite(MathHelper.Pi * 6 / 8, 0, -MathHelper.Pi * 6 / 8, 0, _factor);//MathHelper.SmoothStep(MathHelper.Pi * 6 / 8, -MathHelper.Pi * 6 / 8, _factor)
            }
            return base.RealRotation;
        }
    }

    public override float Rotation
    {
        get
        {
            ////Main.NewText(timeCount);
            //if (Player.controlUseTile)
            //{
            //    var factor = MathHelper.Clamp(projectile.ai[0] / MaxTime, 0, 1);
            //    var theta = ((float)Math.Pow(factor, 2)).Lerp(MathHelper.Pi / 8 * 3, -MathHelper.PiOver2 - MathHelper.Pi / 8);
            //    if (projectile.ai[1] > 0)
            //    {
            //        if (Charged)
            //        {
            //            //Main.NewText(projectile.ai[1] / MaxTimeLeft / factor);
            //            theta = (projectile.ai[1] / MaxTimeLeft / factor).Lerp(theta, MathHelper.Pi / 8 * 3);
            //            //return player.direction == -1 ? MathHelper.Pi * 1.5f - theta : theta;
            //        }
            //        else
            //        {
            //            theta = ((timeCount - projectile.ai[1]) / MaxTime).Lerp(MathHelper.Pi / 8 * 3, -MathHelper.PiOver2);
            //            //return player.direction == -1 ? MathHelper.Pi * 1.5f - theta : theta;
            //        }
            //    }
            //    return theta;
            //}
            //return base.Rotation;
            return base.Rotation;
        }
    }

    public override bool UseRight => true;
    public override bool UseLeft => true;
    public override (int X, int Y) FrameMax => (2, 1);

    public override void OnKill(int timeLeft)
    {
        //TODO 枯石蓄力攻击
        //if (Charged && Player.CheckMana(50, true))
        //{
        //    var cen = projCenter - (CollidingCenter - DrawOrigin).RotatedBy(RealRotation) * new Vector2(Player.direction, 1) * 1.5f + new Vector2(0, -24);
        //    Projectile.NewProjectile(projectile.GetSource_FromThis(), cen, default, ModContent.ProjectileType<WitheredTree>(), Projectile.damage * 3 / 2, Projectile.knockBack, Projectile.owner, UpgradeValue(1, 3));
        //    for (int n = 0; n < 30; n++)
        //    {
        //        Dust.NewDustPerfect(cen, UpgradeValue(MyDustId.Wood, MyDustId.GreenGrass), (MathHelper.TwoPi / 30 * n).ToRotationVector2() * Main.rand.NextFloat(2, 8));
        //    }
        //}
    }

    public override void OnEndAttack()
    {
        base.OnEndAttack();
    }

    public override void OnCharging(bool left, bool right)
    {
        SACoolDown--;
        if (left)
        {
            projectile.ai[0]++;
            if (projectile.ai[0] >= MaxTime)
            {
                OnEndAttack();
            }
        }
        else
        {
            projectile.ai[0] += projectile.ai[0] < MaxTime ? 1 : 0;
        }

        var when = controlTier % 5 == 4 ? (int)(MaxTime - MaxTimeLeft) : (int)MaxTime / 4;
        if ((int)projectile.ai[0] == when && left)
        {
            SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
        }
        projectile.friendly = projectile.ai[0] > when;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.immune[projectile.owner] = 5;
        //TODO 枯石特殊攻击
        if (SACoolDown < 0 && Main.rand.NextBool(controlTier % 5 == 4 ? 2 : 5))
        {
            var max = UpgradeValue(8, 12);
            for (int n = 0; n < max; n++)
            {
                if (Main.rand.NextBool(UpgradeValue(3, 2)))
                {
                    projectile.damage *= 2;
                    DyingStoneSwordProj.ShootSharpTears(target.Center + new Vector2((n - max / 2f) * 8, 24), Player, projectile);
                    projectile.damage /= 2;
                }
            }
        }
    }

    public override void OnRelease(bool charged, bool left)
    {
        if (Charged && Player.CheckMana(5, true))
        {
            int max = UpgradeValue(1, 3);
            for (int n = 0; n < max; n++)
            {
                Vector2 pointPoisition2 = Player.Center + new Vector2(128 * Player.direction, 0) * ((projectile.ai[1] + (float)n / max) / MaxTimeLeft) * max;
                DyingStoneSwordProj.ShootSharpTears(pointPoisition2, Player, projectile);
            }
        }
        base.OnRelease(charged, left);
    }

    public override T UpgradeValue<T>(T normal, T extra, T defaultValue = default)
        => sourceItem.ModItem switch
        {
            CrystalStoneSword => extra,
            DyingStoneSword => normal,
            _ => defaultValue
        };
}

public class RustySteelBlade : WitheredWoodSword
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.damage = 70;
        Item.crit = 26;
        Item.DamageType = DamageClass.Melee;
        Item.width = 66;
        Item.height = 74;
        Item.rare = ItemRarityID.LightPurple;
        Item.useTime = 21;
        Item.useAnimation = 21;
        Item.knockBack = 8;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.autoReuse = true;
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

public class RefinedSteelBlade : RustySteelBlade
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.damage = 90;
        Item.rare = ItemRarityID.Yellow;
        Item.useTime = 15;
        Item.useAnimation = 15;
    }

    public override void AddRecipes()
    {
        var recipe = CreateRecipe();
        recipe.AddIngredient<RustySteelBlade>();
        recipe.AddIngredient(ItemID.BrokenHeroSword, 3);
        recipe.AddTile(TileID.LunarCraftingStation);
        recipe.ReplaceResult(this);
        recipe.Register();
    }
}

public class PureFractal : WitheredWoodSword
{
    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        Item.ShaderItemEffectInventory(spriteBatch, position, origin, LogSpiralLibraryMod.Misc[1].Value, Color.Lerp(new Color(0, 162, 232), new Color(34, 177, 76), (float)Math.Sin(MathHelper.Pi / 60 * ModTime) / 2 + 0.5f), scale);
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        Item.ShaderItemEffectInWorld(spriteBatch, LogSpiralLibraryMod.Misc[1].Value, Color.Lerp(new Color(0, 162, 232), new Color(34, 177, 76), (float)Math.Sin(MathHelper.Pi / 60 * ModTime) / 2 + 0.5f), rotation);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.useStyle = ItemUseStyleID.Swing;
        Item.width = 56;
        Item.height = 56;
        //item.UseSound = SoundID.Item169;
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
}

public class FirstZenith : WitheredWoodSword
{
    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        Item.ShaderItemEffectInventory(spriteBatch, position, origin, LogSpiralLibraryMod.Misc[0].Value, Color.Lerp(new Color(99, 74, 187), new Color(20, 120, 118), (float)Math.Sin(MathHelper.Pi / 60 * ModTime) / 2 + 0.5f), scale);
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        Item.ShaderItemEffectInWorld(spriteBatch, LogSpiralLibraryMod.Misc[0].Value, Color.Lerp(new Color(99, 74, 187), new Color(20, 120, 118), (float)Math.Sin(MathHelper.Pi / 60 * ModTime) / 2 + 0.5f), rotation);
    }

    public override void SetDefaults()
    {
        Item.useStyle = ItemUseStyleID.Swing;
        Item.width = 58;
        Item.height = 64;
        Item.noUseGraphic = true;
        //Item.UseSound = SoundID.Item71;
        Item.autoReuse = true;
        Item.DamageType = DamageClass.Melee;
        Item.channel = true;
        Item.noMelee = true;
        Item.useAnimation = 15;
        Item.useTime = 15;//
        Item.shootSpeed = 16f;
        Item.damage = 500;
        Item.knockBack = 6.5f;
        Item.value = Item.sellPrice(0, 0, 0, 0);
        Item.crit = 31;
        Item.rare = ItemRarityID.Purple;
        Item.shoot = ProjectileType<FirstZenith_Blade>();
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

public class FirstZenith_Blade : VertexHammerProj
{
    public override bool Charged => base.Charged;
    public override bool Charging => base.Charging;
    public override SpriteEffects flip => base.flip;
    public override Rectangle? frame => base.frame;
    public override Vector2 projCenter => base.projCenter;
    public override bool RedrawSelf => base.RedrawSelf;
    public override Vector2 scale => base.scale;
    public override float timeCount { get => base.timeCount; set => base.timeCount = value; }
    public override bool WhenVertexDraw => base.WhenVertexDraw;
    public override string Texture => base.Texture.Replace("_Blade", "_Old");
    public override string HammerName => "初源峰巅";
    public override float MaxTime => 12;
    public override float Factor => base.Factor;
    public override Vector2 CollidingSize => base.CollidingSize;

    //public override Vector2 projCenter => base.projCenter + new Vector2(Player.direction * 16, -16);
    public override Vector2 CollidingCenter => base.CollidingCenter;//new Vector2(projTex.Size().X / 3 - 16, 16)

    public override Vector2 DrawOrigin => base.DrawOrigin + new Vector2(-12, 12);
    public override Color color => base.color;

    //public override Color VertexColor(float time) => Color.Lerp(Color.DarkGreen, UpgradeValue(Color.Brown, Color.Green), time);//Color.Lerp(UpgradeValue(Color.Brown, Color.Green), Color.DarkGreen, time)
    public override float MaxTimeLeft => 8;

    public override float Rotation => base.Rotation;
    public override bool UseRight => true;
    public override bool UseLeft => true;
    public override (int X, int Y) FrameMax => (1, 1);

    public override void OnKill(int timeLeft)
    {
        //if (factor == 1)
        //{
        //    Projectile.NewProjectile(projectile.GetSource_FromThis(), vec, default, ModContent.ProjectileType<HolyExp>(), player.GetWeaponDamage(player.HeldItem) * 3, projectile.knockBack, projectile.owner);
        //}
        //CoolerSystem.UseInvertGlass = !CoolerSystem.UseInvertGlass;

        base.OnKill(timeLeft);
    }

    public override void OnCharging(bool left, bool right)
    {
    }

    public override void OnRelease(bool charged, bool left)
    {
        if (Charged)
        {
            if ((int)projectile.ai[1] == 1)
            {
                OnChargedShoot();
            }
        }
        //Main.NewText(new NPCs.Baron.Baron().CanTownNPCSpawn(10, 10));
        base.OnRelease(charged, left);
    }

    public Item sourceItem;

    public override void OnSpawn(IEntitySource source)
    {
        if (source is EntitySource_ItemUse_WithAmmo itemSource)
        {
            sourceItem = itemSource.Item;
        }
        base.OnSpawn(source);
    }

    public override void VertexInfomation(ref bool additive, ref int indexOfGreyTex, ref float endAngle, ref bool useHeatMap, ref int p)
    {
        p = 2;
    }

    public override void OnChargedShoot()
    {
    }
}

public class FinalFractal : WitheredWoodSword
{
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient<PureFractal_Old>().QuickAddIngredient(4144, 3368).AddTile(TileID.LunarCraftingStation).Register();
        CreateRecipe().AddIngredient<FirstZenith_Old>().QuickAddIngredient(4144, 3368).AddTile(TileID.LunarCraftingStation).Register();
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        Item.ShaderItemEffectInventory(spriteBatch, position, origin, LogSpiralLibraryMod.Misc[0].Value, Color.Lerp(new Color(99, 74, 187), new Color(20, 120, 118), (float)Math.Sin(MathHelper.Pi / 60 * ModTime) / 2 + 0.5f), scale);
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        Item.ShaderItemEffectInWorld(spriteBatch, LogSpiralLibraryMod.Misc[0].Value, Color.Lerp(new Color(99, 74, 187), new Color(20, 120, 118), (float)Math.Sin(MathHelper.Pi / 60 * ModTime) / 2 + 0.5f), rotation);
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        for (int n = 1; n < 4; n++)
        {
            tooltips.Add(new TooltipLine(Mod, "PureSuggestion", Language.GetOrRegister("Mods.FinalFractalSet.FinalFractalTip." + n).Value) { OverrideColor = Color.Lerp(new Color(99, 74, 187), new Color(20, 120, 118), (float)Math.Sin(MathHelper.Pi / 60 * (ModTime + 40 * n)) / 2 + 0.5f) });
        }
    }

    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.damage = 350;
        Item.DamageType = DamageClass.Melee;
        Item.width = 64;
        Item.height = 74;
        Item.rare = ItemRarityID.Purple;
        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.knockBack = 8;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.autoReuse = true;
    }
}