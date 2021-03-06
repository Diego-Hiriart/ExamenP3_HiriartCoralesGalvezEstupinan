Shader "Custom/CartoonWater"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Opacity ("Opacity", Range(0, 1)) = 1
        _AnimSpeedX ("Anim Speed (X)", Range(0, 4)) = 2
        _AnimSpeedY ("Anim Speed (Y)", Range(0, 4)) = 2
        _AnimScale ("Anim Scale", Range(0, 1)) = 1
        _AnimTiling ("Anim Tiling", Range(0, 20)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        LOD 100
        Zwrite Off
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            half _Opacity;
            float _AnimSpeedX;
            float _AnimSpeedY;
            float _AnimScale;
            float _AnimTiling;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //Distorcionar UVs
                //i.uv.x += sin((i.uv.x + i.uv.y) * _AnimTiling + _Time.y) * _AnimSpeedX;
                //i.uv.y += cos((i.uv.x - i.uv.y) * _AnimTiling + _Time.y) * _AnimSpeedY;

                i.uv.x += atan((i.uv.x - i.uv.y) * _AnimTiling + _Time.y) * _AnimSpeedX / 20;
                i.uv.y += sin((i.uv.x + i.uv.y) * _AnimTiling + _Time.y) * _AnimSpeedY / 20;



                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                //opacity
                col.a = _Opacity;
                return col;
            }
            ENDCG
        }
    }
}
