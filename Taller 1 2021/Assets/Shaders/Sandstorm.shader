Shader "Custom/Sandstorm"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    _ParticleSize("Particle Size", Range(1.0, 10.0)) = 2.0
    _ParticleSlowness("Particle Slowing Factor", Range(1.0, 1000)) = 1000
    _ColorA("Sand Color A", Color) = (0.95, 0.75, 0.45, 1)
    _ColorB("Sand Color B", Color) = (0.9, 0.65, 0.40, 1)
    _ColorC("Sand Color C", Color) = (0.5, 0.25, 0.18, 1)
    
    _FadeInTime("Fade In Time", Range(0.1, 15.0)) = 5.0
    _FadeOutTime("Fade Out Time", Range(0.1, 5.0)) = 1.0
    _MaxFade("Maximum Effect Fade", Range(0.0, 1.0)) = 0.95            // Causes an apparent flash when the fade goes all the way to 1.

    }
        SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
    float _ParticleSlowness;
    float _ParticleSize;
    float4 _ColorA;
    float4 _ColorB;
    float4 _ColorC;
     
    float _FadeInTime;
    float _FadeOutTime;
    float _MaxFade;

     float rand(float2 pix)
     {
           return frac(sin(dot(pix.xy, float2(12.345, 67.89))) * 98765.1234);
     }
    float rand(float2 pix)
    {
           return frac(sin(dot(pix.xy, float2(12.345, 67.89))) * 98765.1234 + _Time.w);
    }
    float rand(float2 pix)
    {
        return frac(sin(dot(pix.xy, float2(12.345, 67.89))) * 98765.1234 + _Time.w / _ParticleSlowness);
    }

    float4 frag(v2f_img input) : COLOR
    {
        float4 base = tex2D(_MainTex, input.uv);

        base = rand(input.uv);
        base = rand(floor(input.uv * _ParticleSize * float2(160, 90)));

        base = base < 0.33 ? _ColorA : base < 0.66 ? _ColorB : _ColorC;
        base = base < 0.97 ? _ColorA : base < 0.99 ? _ColorB : _ColorC;

        float staticNoise = rand(floor(input.uv * _ParticleSize * float2(160, 90)));
        float4 sandstorm = staticNoise < 0.97 ? _ColorA : staticNoise < 0.99 ? _ColorB : _ColorC;
         
        float fadeTime = fmod(_Time.w, _FadeInTime + _FadeOutTime);        // between 0, _TimeIn, and _TimeIn + _TimeOut
        float fadingOut = step(_FadeInTime, fadeTime);                    // 0 if fading in, 1 if fading out
        float fadeAmount = (1 - fadingOut) * (fadeTime / _FadeInTime)
                   + (fadingOut) * (1 - (fadeTime - _FadeInTime) / _FadeOutTime);
        
        base = lerp(base, sandstorm, min(fadeAmount, _MaxFade));

        return base;
    }
    ENDCG
}
    }
}