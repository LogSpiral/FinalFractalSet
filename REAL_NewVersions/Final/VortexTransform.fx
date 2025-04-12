sampler uImage0 : register(s0);
float uFactor;
float2 uCenter;
float uKvalue;
float2 VortF(float2 q, float2 c)
{
	float2 d = q - c; // 从涡旋中心指向目标点的向量
	d /= uFactor;
  
	return 0.25 * float2(d.y, -d.x) / (dot(d, d) + 0.05) * uFactor;
  // 这里这个可以试试别的角度的，比如vec2 (d.y + d.x,d.y - d.x)
  // 效果挺神奇的((
  // 每次迭代，原来的点相较于上次都会被关于c旋转一点角度并往外推一点
  // 而越靠近c点，这个影响越大，那个0.05算是一个削弱性偏移量，去掉的话向量偏移程度会强很多，尤其是中心附近的
  // 因为这里是根据坐标采样颜色，所以这个往外推的效应被反向了，变成往内吸
}

float2 FlowField(float2 q)
{
  // c是当前涡旋中心
  // vr是累积偏移坐标
  // c附近的p会受到更大的偏移影响
  // 而离c很远的点可以视作没受到影响
	float2 c = uCenter;
	return -VortF(q, c);
}


float4 VortexTrasform(float2 coord : TEXCOORD0) : COLOR0
{
	float2 p = coord;
	p.x *= uKvalue;
	for (int i = 0; i < 10; i++)
		p -= FlowField(p) * 0.12;
	p.x /= uKvalue;
	return tex2D(uImage0, p); // 输出
}
technique Technique1
{
	pass VortexTransform
	{
		PixelShader = compile ps_3_0 VortexTrasform();
	}
}