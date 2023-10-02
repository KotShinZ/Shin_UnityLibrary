Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _Stencil("Ref", Int) = 1
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            ZWrite Off
            ColorMask 0

            Stencil
            {
                Ref [_Stencil]
                Comp Always
                Pass Replace
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            void frag (v2f i)
            {
            }
            ENDCG
        }
    }
}
