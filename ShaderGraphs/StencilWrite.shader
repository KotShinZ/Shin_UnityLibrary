Shader "Unlit/StencilWrite"
{
    Properties
    {
        _Stencil("Stencil", Range(0,100)) = 0.0
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            ZWhite Off
            ColorMask 0
            
            Stencil
            {
                Ref 2
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            void frag(v2f i)
            {

            }

            ENDCG
        }
    }
}
