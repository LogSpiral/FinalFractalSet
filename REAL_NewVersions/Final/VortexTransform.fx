sampler uImage0 : register(s0);
float uFactor;
float2 uCenter;
float uKvalue;
float2 VortF(float2 q, float2 c)
{
	float2 d = q - c; // ����������ָ��Ŀ��������
	d /= uFactor;
  
	return 0.25 * float2(d.y, -d.x) / (dot(d, d) + 0.05) * uFactor;
  // ��������������Ա�ĽǶȵģ�����vec2 (d.y + d.x,d.y - d.x)
  // Ч��ͦ�����((
  // ÿ�ε�����ԭ���ĵ�������ϴζ��ᱻ����c��תһ��ǶȲ�������һ��
  // ��Խ����c�㣬���Ӱ��Խ���Ǹ�0.05����һ��������ƫ������ȥ���Ļ�����ƫ�Ƴ̶Ȼ�ǿ�ܶ࣬���������ĸ�����
  // ��Ϊ�����Ǹ������������ɫ��������������Ƶ�ЧӦ�������ˣ����������
}

float2 FlowField(float2 q)
{
  // c�ǵ�ǰ��������
  // vr���ۻ�ƫ������
  // c������p���ܵ������ƫ��Ӱ��
  // ����c��Զ�ĵ��������û�ܵ�Ӱ��
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
	return tex2D(uImage0, p); // ���
}
technique Technique1
{
	pass VortexTransform
	{
		PixelShader = compile ps_3_0 VortexTrasform();
	}
}