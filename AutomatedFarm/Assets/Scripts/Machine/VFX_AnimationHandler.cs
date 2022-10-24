using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_AnimationHandler : MonoBehaviour
{
    public GameObject[] vfxs;
    public Animator anim;
    ParticleSystem ps;
    float rateOverTime;

    private void Start() {
        StopMachine();
    }

    public void StopMachine()
    {
        if(vfxs.Length > 0)
            for (int i = 0; i < vfxs.Length; i++)
            {
                ps = vfxs[i].GetComponent<ParticleSystem>();
                var temp = ps.emission;
                temp.enabled = false;
            }
        if(anim != null) anim.speed = 0;
    }

    public void ResumeMachine()
    {
        if(vfxs.Length > 0)
            for (int i = 0; i < vfxs.Length; i++)
            {
                ps = vfxs[i].GetComponent<ParticleSystem>();
                var temp = ps.emission;
                temp.enabled = true;
            }
        if(anim != null) anim.speed = 1;

    }

}
