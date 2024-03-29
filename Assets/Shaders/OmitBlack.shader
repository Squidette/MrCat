Shader "Omit/Black"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AlphaFalloff ("Falloff", Float) = 0.01
        _Color ("Tint Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" }
        LOD 100
 
        Zwrite Off
        Blend SrcAlpha OneMinusSrcAlpha
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
           
            #include "UnityCG.cginc"
 
            sampler2D _MainTex;
            float _AlphaFalloff;
            fixed4 _Color;
           
            fixed4 frag (v2f_img i) : COLOR
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // Check if the color is close to black
                if (col.r <= _AlphaFalloff && col.g <= _AlphaFalloff && col.b <= _AlphaFalloff) { 
                    discard; // Discard fragment if color is close to black
                }
                return col * _Color; // Apply tint color
            }
            ENDCG
        }
    }
}