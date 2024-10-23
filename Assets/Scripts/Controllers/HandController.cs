using TMPro;
using UnityEngine;

namespace Controllers
{
    public class HandController : MonoBehaviour
    {
        public RewardManager rewardManager;
        public TextMeshProUGUI rewardToShow;
        public Animator handAnim;
        public float foundBonus;
        public void Start()
        {
            handAnim = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("rewardNo"))
            {
                var childIndex = other.transform.GetSiblingIndex() + 1;
                foundBonus = rewardManager.currentBonus * childIndex;
                rewardToShow.text = (foundBonus).ToString("F0");
            }
        }

        public void StopStartHandAnimation(bool activeness)
        {
            handAnim.enabled = activeness;
        }
    }
}