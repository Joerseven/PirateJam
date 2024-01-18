Shader "Unlit/SpurtShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [MainColor] _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _Thick ("Thiccness", Float) = 160
        _USpreadTime ("Universal Spread Time", float) = 0.2
        _BorderThick ("Border Width", float) = 0.01
        _BorderColor ("Border Color", Color) = (0.0, 0.0, 0.0, 1.0)
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
            float _USpreadTime;
            float _BorderThick;
            float4 _BorderColor;
            
            float4 particlePos[50];
            float elapsed;
            float3 targetCell;
            float cellArea;
            float3 originPos;

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

                #define DIV_SQRT_2 0.70710678118
                
                float pixelVal = 0;
                float directionVals[8] = {0,0,0,0,0,0,0,0};
                float2 directionVecs[8] = {
                    float2(1, 0),
                    float2(-1, 0),
                    float2(0, 1),
                    float2(0, -1),
                    float2(DIV_SQRT_2, DIV_SQRT_2),
                    float2(-DIV_SQRT_2, DIV_SQRT_2),
                    float2(DIV_SQRT_2, -DIV_SQRT_2),
                    float2(-DIV_SQRT_2, -DIV_SQRT_2),
                };
                
                
                for (float p=0; p<50; p++)
                {
                    float fVal = (3 - cellArea) * 0.6;
                    float originDisModifier = length(particlePos[(int)p].xy - originPos.xy) * _USpreadTime;
                    float timeModifier = min(1, min(elapsed, originDisModifier) / originDisModifier);
                    float dis = 1 / (length(i.worldPos.xy - particlePos[(int)p].xy) * _Thick) * timeModifier;
                    for (int d=0; d<8; d++)
                    {
                        directionVals[d] += 1 / (length(i.worldPos.xy - (particlePos[(int)p].xy + directionVecs[d] * _BorderThick)) * _Thick) * timeModifier;
                    }
                    pixelVal += dis;
                    p += fVal;
                    
                }
                //pixelVal.xyz = smoothstep(0.06, 0.07, pixelVal.x) * _Color;
                //pixelVal.w = step(0.1, pixelVal.x);
                float4 steppedDf = step(0.8, pixelVal);
                

                float totalBorder = 0;

                for (int d = 0; d < 8; d++)
                {
                    float steppedDirection = step(0.8, directionVals[d]);
                    totalBorder += steppedDirection;
                }
                
                
                fixed4 col = steppedDf * _Color + totalBorder * _BorderColor;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
