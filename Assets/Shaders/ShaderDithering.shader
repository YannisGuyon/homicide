Shader "Custom/ShaderDithering"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				float2 screen_pos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.screen_pos = ComputeScreenPos(o.vertex);
                o.uv = v.uv;
                return o;
            }

			sampler2D _MainTex;
			fixed4 _Color;

			float GetDitheringMultiplier(float screen_pos)
			{
				float multiplier = 1;
				if (screen_pos % 2 == 0) multiplier += 0.01f;
				if (screen_pos % 5 == 0) multiplier -= 0.01f;
				if (screen_pos % 11 == 0) multiplier += 0.015f;
				if (screen_pos % 17 == 0) multiplier += 0.015f;
				if (screen_pos % 23 == 0) multiplier -= 0.02f;
				if (screen_pos % 27 == 0) multiplier += 0.02f;
				if (screen_pos % 37 == 0) multiplier += 0.02f;
				if (screen_pos % 47 == 0) multiplier -= 0.02f;
				return multiplier;
			}

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

				float2 screen_pos = i.screen_pos;
				screen_pos.xy = floor(screen_pos.xy * _ScreenParams.xy);

				col.rgb *= GetDitheringMultiplier(screen_pos.x + screen_pos.y);
				col.rgb *= GetDitheringMultiplier(screen_pos.x - screen_pos.y);
				col.rgb *= GetDitheringMultiplier(screen_pos.x);
				col.rgb *= GetDitheringMultiplier(screen_pos.y);

			    
				const float quant = 5;
				col.r = floor(col.r * quant) / quant;
				col.g = floor(col.g * quant) / quant;
				col.b = floor(col.b * quant) / quant;

				col.rgb *= _Color.rgb;

				//float3 yuv;
				//yuv.r = 0.299 * col.r + 0.587 * col.g + 0.114 * col.b;
				//yuv.g = -0.147 * col.r - 0.289 * col.g + 0.436 * col.b;
				//yuv.b = 0.615 * col.r - 0.515 * col.g - 0.100 * col.b;
				//yuv.rgb = round(yuv.rgb * 4) / 4;
				//R = Y + 1.140V
				//G = Y - 0.395U - 0.581V
				//B = Y + 2.032U
                return col;
            }
            ENDCG
        }
    }
}
