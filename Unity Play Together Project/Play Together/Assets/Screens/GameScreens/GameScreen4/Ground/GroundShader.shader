Shader "Unlit/GroundShader"
{
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }
 
    SubShader {
                Tags { "RenderType"="Opaque" "Queue" = "Geometry+1"}
        LOD 200
        Stencil {
        Ref 5
        Comp NotEqual
        }
        Tags { "RenderType" = "Opaque" }
        CGPROGRAM
        #pragma surface surf Lambert fullforwardshadows 
   
        struct Input {
            float2 uv_MainTex;
        };
         
        sampler2D _MainTex;
         
        void surf (Input IN, inout SurfaceOutput o) {
            o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
        }
        ENDCG
    }
    Fallback "Diffuse"
}
