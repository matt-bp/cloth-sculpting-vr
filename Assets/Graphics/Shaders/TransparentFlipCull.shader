Shader "Wright/Transparent Shader Flip Cull"
{
    Properties
    {
        _Tint("Tint", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Front // When the geometry is inside-out
        
        Pass
        {
            CGPROGRAM
            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram

            #include "UnityCG.cginc"

            float4 _Tint;
            sampler2D _MainTex;
            float4 _MainTex_ST; // Getting tiling and offset information

            struct Interpolators
            {
                float4 position : SV_POSITION;
                // float3 localPosition : TEXCOORD0;
                float2 uv : TEXCOORD0;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            struct VertexData
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            Interpolators MyVertexProgram(VertexData v)
            {
                Interpolators i;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(Interpolators, i);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(i);
                
                // i.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw; // Use below instead
                i.uv = TRANSFORM_TEX(v.uv, _MainTex); // This appends _ST during the pre-processing step
                i.position = UnityObjectToClipPos(v.position);
                return i;
            }

            float4 MyFragmentProgram(Interpolators i) : SV_TARGET
            {
                return tex2D(_MainTex, i.uv) * _Tint;
            }
            ENDCG
        }
    }
}