using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_AnimationHandler : MonoBehaviour
{
    public GameObject[] vfxs;
    public Animator anim;


    public void StopMachine()
    {
        if(vfxs.Length > 0)
            for (int i = 0; i < vfxs.Length; i++)
                vfxs[i].SetActive(false);
        
        if(anim != null) anim.speed = 0;
    }

    public void ResumeMachine()
    {
        if(vfxs.Length > 0)
            for (int i = 0; i < vfxs.Length; i++)
                vfxs[i].SetActive(true);

        if(anim != null) anim.speed = 1;

    }

}
