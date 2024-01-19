Shader "Unlit/SpurtShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Thick ("Thiccness", Float) = 160
        _USpreadTime ("Universal Spread Time", float) = 0.2
    }
    SubShader
    {
        Tags 
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Thick;
            
            float4 particlePos[50];
            float elapsed;
            float3 targetCell;
            float cellArea;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 pixelVal = {0,0,0,0};
                
                for (float p=0; p<50; p++)
                {
                    float fVal = (3 - cellArea) * 0.6;
                    float dis = 1 / (length(i.worldPos.xy - particlePos[(int)p].xy) * _Thick);
                    pixelVal += dis;
                    p += fVal;
                    
                }
                //pixelVal.xyz = smoothstep(0.06, 0.07, pixelVal.x) * _Color;
                //pixelVal.w = step(0.1, pixelVal.x);
                float4 steppedDf = step(0.8, pixelVal.x);
                float4 edgeDf = step(0.77, pixelVal.x);
                edgeDf = edgeDf - steppedDf;
                
                
                fixed4 col = steppedDf * _Color + edgeDf * float4(0,0,0,1);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
