﻿Shader "Custom/NewSurfaceShader"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Gloss (A)", 2D) = "white"
	}


	Category
	{
		SubShader
		{
			Tags { "Queue" = "Overlay+1" }
			UsePass "Specular/BASE"
			Pass
			{
				ZTest Greater
				Lighting Off
				Color[_Color]
			}
			Pass
			{
				ZTest Less
				SetTexture[_MainTex] {combine texture}
			}
		}
	}
		FallBack "Specular", 1
}