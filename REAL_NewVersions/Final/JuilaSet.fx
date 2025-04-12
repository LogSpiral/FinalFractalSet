float2 uConst;
float4 uRange;
float2 ComplexEXP(float2 z)
{
	return exp(z.x) * float2(cos(z.y), sin(z.y));
}
float2 ComplexCosh(float2 z)
{
	return (ComplexEXP(z) + ComplexEXP(-z)) * .5;
}
float2 ComplexCos(float2 z)
{
	z = float2(-z.y, z.x);
	z = ComplexCosh(z);
	return z;
}
float3 palette(float t)
{
	float3 a = float3(0.5, 0.75, 4.000);
	float3 b = float3(0.5, 0.75, 4.000);
	float3 c = float3(0.5, 0.5, 0.500);
	float3 d = float3(0.5, 0.5, 0.500);
	return a + b * cos(6.28318 * (c * t + d));
}
float4 JuilaSetFunction(float2 coord : TEXCOORD0, float4 color : COLOR0) : COLOR0
{
	float2 z0 = lerp(uRange.xy, uRange.zw, coord); //通过插值获取z0
	float2 z = z0;
	int n;
	for (n = 0; n < 30; n++)
	{
		z = ComplexCos(z) + uConst;
		if (length(z) > 16)
			break;
	}
	return float4(palette(n / 29.), 1) * color; //迭代完成塞回数据
}
technique Technique1
{
	pass JuilaSet
	{
		PixelShader = compile ps_3_0 JuilaSetFunction();
	}
}