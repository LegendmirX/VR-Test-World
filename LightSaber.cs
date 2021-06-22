using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSaber : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    public void TriggerBeam()
    {
        bool isOn = animator.GetBool("IsOn");
        if(isOn == false)
        {
            SoundManager.PlaySound(SoundManager.Sound.LightSaberIgnite, this.transform.position);
            SoundManager.PlaySound(SoundManager.Sound.LightSaberConstant, this.transform.position, this.transform);
        }
        else
        {
            SoundManager.PlaySound(SoundManager.Sound.LightSaberEnd, this.transform.position);
            SoundManager.StopSound(SoundManager.Sound.LightSaberConstant);
        }

        animator.SetBool("IsOn", !isOn);
    }
}
