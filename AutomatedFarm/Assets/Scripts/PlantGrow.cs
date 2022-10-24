using DG.Tweening;
using MyEnums;
using System;
using System.Collections;
using UnityEngine;

namespace AutomatedFarm
{
    ///<summary>
    /// Grow plants. Simple
    ///</summary>
    public class PlantGrow : MonoBehaviour
    {
        [Header("Options")]
        public LayerMask soilLayer;
        string myName;
        float time = 11;
        public ResourceType type;
        Seed seedSettings;
        public bool canBeHarvested { get; private set; }    
        public static Action<GameObject> OnPlantReady;
        public static Action OnPlantPlaced;
        public bool ignore;
        public bool istarget;
        public S_SoilState currentSoil;
        

        [HideInInspector] public bool isAssignedToGraber;

        private void Start()
        {
            if(ignore) return;
            OnPlantPlaced?.Invoke();

            if (Library.Instance.GetType(type) != null)
            {
                seedSettings = Library.Instance.GetType(type);
                myName = seedSettings.name;
                time = seedSettings.timeToGrow;
            }        

            transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            transform.DOScale(new Vector3(0.33f, 0.33f, 0.33f), 1).SetEase(Ease.InOutBack);

            //transform.DOScale(Vector3.one, time).SetEase(Ease.Linear).OnComplete((CompletePlant));
            //LeanTween.scale(gameObject, desiredScale, time);

            Ray ray = new Ray(transform.position + new Vector3(0,3,0), Vector3.down);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 10f, soilLayer))
            {
                currentSoil = hit.collider.gameObject?.GetComponent<S_SoilState>();
            }

            StartCoroutine(Grow());
        }

        IEnumerator Grow()
        {
            float timeLeft = time;

            for (int i = 0; i < time / 2; i++)
            {
                if (timeLeft <= 0) break;
                timeLeft--;
                yield return new WaitForSeconds(1);
            }

            transform.DOScale(new Vector3(0.66f, 0.66f, 0.66f), 1).SetEase(Ease.InOutBack);

            for (int i = 0; i < time / 2; i++)
            {
                if (timeLeft <= 0) break;
                timeLeft--;
                yield return new WaitForSeconds(1);
            }

            transform.DOScale(Vector3.one, 1).SetEase(Ease.InOutBack).OnComplete((CompletePlant));
        }

        void CompletePlant()
        {
            if(ignore) return;
            canBeHarvested = true;
            OnPlantReady?.Invoke(gameObject);
        }

        public void AssignToGraber() => isAssignedToGraber = true;

        public bool Harvest()
        {
            if(ignore) return false;

            if (canBeHarvested)
            {
                if(currentSoil!= null)
                    currentSoil.ChangeStateToFree();
                    
                Destroy(gameObject);
                /*transform.localScale = Vector3.zero;
                canBeHarvested = false;*/
                return true;
            }
            return false;
        }
    }
}


