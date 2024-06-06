using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioSource musicsrc;
    public AudioClip sfx1, sfx2, sfx3;

    public void Button1()
    {
        musicsrc.clip = sfx1;
        musicsrc.Play();
    }
    public void Button2()
    {
        musicsrc.clip = sfx1;
        musicsrc.Play();
    }
    public void Button3()
    {
        musicsrc.clip = sfx1;
        musicsrc.Play();
    }
}
