Shader "Unlit/SpurtShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Thick ("Thiccness", Float) = 160
        _USpreadTime ("Universal Spread Time", float) = 0.2
        _BorderThick ("Border Width", float) = 0.01
        _BorderColor ("Border Color", Color) = (0.0, 0.0, 0.0, 1.0)
    }
    SubShader
    {
        Tags 
        {
            Queue = Transparent
            RenderType = Opaque
        }
        LOD 100
        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
        

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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
            
            float4 particlePos[200];
            float elapsed;
            float3 targetCell;
            float cellArea;
            float3 originPos;
            float4 color;

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
                
                
                for (float p=0; p< min(200, 10+15*cellArea); p++)
                {

                    const float2 targetPos = particlePos[(int)p].xy;
                    float tTimeValue = min(elapsed / _USpreadTime, 1);
                    float2 currentPos = (targetPos - originPos.xy) * tTimeValue + originPos.xy;
                    
                    float disModifier = length(particlePos[(int)p].xy - originPos.xy) * _USpreadTime;
                    float timeModifier = min(1, min(elapsed, disModifier) / disModifier);
                    
                    float dis = 1 / (length(i.worldPos.xy - currentPos) * (_Thick + 100 * (1 - tTimeValue)));
                    for (int d=0; d<8; d++)
                    {
                        directionVals[d] += 1 / (length(i.worldPos.xy - (currentPos + directionVecs[d] * _BorderThick)) * (_Thick + 100 * (1 - tTimeValue)));
                    }
                    pixelVal += dis;
                }
                float3 steppedDf = step(0.8, pixelVal);
                float3 totalBorder = float3(0,0,0);

                for (int d = 0; d < 8; d++)
                {
                    totalBorder += step(0.8, directionVals[d]);
                }

                float3 finalColor = steppedDf * color + totalBorder * _BorderColor;
                
                fixed4 col = float4(finalColor, min(1, steppedDf.r + totalBorder.r));
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
