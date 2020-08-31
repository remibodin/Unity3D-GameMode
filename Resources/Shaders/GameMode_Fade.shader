Shader "Hidden/Nrtx/Fade"
{
    SubShader
    {
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZTest Always
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 _Color;

            float4 vert( float4 vertex : POSITION ) : SV_POSITION
            {
                return vertex;
            }

            float4 frag() : SV_Target
            {
                return _Color.rgba;
            }
            ENDCG
        }
    }
}