﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDeadEffect : MonoBehaviour {
    [ExecuteInEditMode]
    public Material EffectMat;
    

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, EffectMat);
    }

}
