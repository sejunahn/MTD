Shader "UI/PixelTransition"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Progress ("Progress", Range(0,1)) = 0
        _BlockSize ("Block Size", Float) = 20
    }

    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Progress;
            float _BlockSize;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float rand(float2 co)
            {
                return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 픽셀을 Block 단위로 묶기
                float2 blockUV = floor(i.uv * _BlockSize);

                // 블록마다 랜덤 값
                float blockRand = rand(blockUV);

                // _Progress 값과 비교해서 가릴지 보여줄지 결정
                if (blockRand < _Progress)
                {
                    return fixed4(0,0,0,1); // 블럭 가려짐 (검정색)
                }
                else
                {
                    return fixed4(0,0,0,0); // 블럭 안 가려짐 (투명)
                }
            }
            ENDCG
        }
    }
}
