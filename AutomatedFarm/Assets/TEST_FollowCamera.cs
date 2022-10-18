using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TEST_FollowCamera : MonoBehaviour
{
    public GameObject target;
    bool doOnce;
    bool enableMove;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 2.75f;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null || target.activeSelf == false)
        {
            ConveyorItem[] itens = null;
            target = null;
            enableMove = false;
            doOnce = false;
            itens = FindObjectsOfType<ConveyorItem>();
            foreach (var item in itens)
            {
                if(target != null) return;

                if(item.type != MyEnums.ResourceType.cardboard && item.gameObject.activeSelf == true)
                    target = item.gameObject;

                if(target != null) return;
            }
        }

        if(doOnce == false && target != null) {
            doOnce = true;
            transform.DOMove(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), 0.25f).SetEase(Ease.InOutCirc)
            .OnComplete( () => enableMove = true);
        }
        if(target != null && enableMove == true)
            transform.position =  new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);

    }
}
