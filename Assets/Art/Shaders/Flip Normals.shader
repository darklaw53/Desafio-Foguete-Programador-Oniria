Shader "Flip Normals" {
    Properties{
        _MainTex("Color (RGB) Alpha (A)", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
    }
        SubShader{

            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

            Cull Off

            CGPROGRAM

            #pragma surface surf Lambert alpha vertex:vert
            sampler2D _MainTex;
            float4 _Color;

            struct Input {
                float2 uv_MainTex;
                float4 color : COLOR;
            };

            void vert(inout appdata_full v) {
                v.normal.xyz = v.normal * -1;
            }

            void surf(Input IN, inout SurfaceOutput o) {
                 fixed4 result = tex2D(_MainTex, IN.uv_MainTex) * (_Color * 1.5f);
                 o.Albedo = result.rgb;
                 o.Alpha = _Color.a;
            }

            ENDCG

    }

        Fallback "Diffuse"
}