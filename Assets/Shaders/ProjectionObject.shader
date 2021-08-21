Shader "Hidden/ProjectionObject"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
                float4 ray : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 uv_depth : TEXCOORD1;
                float4 ray : TEXCOORD2;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

				o.uv_depth = v.uv.xy;
                o.ray = v.ray;
                
                return o;
            }

            sampler2D _MainTex;
            sampler2D _ProjTex;
            sampler2D _CameraDepthTexture;

            float3 _ProjOrigin;
            float3 _ProjDir;
            float4x4 _ViewProjectionMat;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 mainColor = tex2D(_MainTex, i.uv);

				float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv_depth));
                float linearDepth = Linear01Depth(rawDepth);
				float4 worldDir = linearDepth * i.ray;
				float4 worldPos = float4((_WorldSpaceCameraPos + worldDir).xyz, 1);

                float3 originDir = normalize(worldPos - _ProjOrigin);
                if(dot(originDir, _ProjDir) < 0) return mainColor;

                float4 projected = mul(_ViewProjectionMat, worldPos);
                float2 projUV = (projected.xy / projected.w) * 0.5 + 0.5;
                projUV = 1 - projUV;
                projUV = saturate(projUV);
                // return float4(projUV,0,1);

                fixed4 projColor = tex2D(_ProjTex, projUV);
                // return projColor;

                fixed4 color = (projColor * projColor.a) + (mainColor * (1 - projColor.a));
                return color;
            }
            ENDCG
        }
    }
}
