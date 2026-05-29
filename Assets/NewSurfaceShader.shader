Shader"Custom/RimGlowTransparent"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (0,0,0,0)

        [HDR]
        _RimColor ("Rim Color", Color) = (0,1,1,1)

        _RimPower ("Rim Power", Range(0.1, 10)) = 3
        _RimIntensity ("Rim Intensity", Range(0, 10)) = 3

        _Threshold ("Threshold", Range(0,1)) = 0.6
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        

                Blend
        SrcAlpha OneMinusSrcAlpha

        ZWrite Off

        Cull Back

        CGPROGRAM

        #pragma surface surf Standard alpha:fade

        struct Input
        {
            float3 viewDir;
        };

        fixed4 _MainColor;
        fixed4 _RimColor;

        float _RimPower;
        float _RimIntensity;
        float _Threshold;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
                    // 기본 색
            o.Albedo = _MainColor.rgb;

                    // Fresnel Rim 계산
            float rim =
                        1.0 -
                        saturate(
                            dot(
                                normalize(IN.viewDir),
                                o.Normal
                            )
                        );

                    // 림 강도 조절
            rim = pow(rim, _RimPower);

                    // Threshold 적용
            rim = step(_Threshold, rim);

                    // 발광
            o.Emission =
                        _RimColor.rgb *
                        rim *
                        _RimIntensity;

                    // 투명도
            o.Alpha = rim;

                    // 표면 옵션
            o.Smoothness = 1.0;
            o.Metallic = 0.0;
        }

        ENDCG
    }

    FallBack"Transparent/Diffuse"
}