using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Deck
{
    public class EventCard : CardBase
    {
        // public bool eventCard;

        [SerializeField] TMP_Text rewardText;
        [SerializeField] TMP_Text runText;
        [SerializeField] TMP_Text requirementText;
        [SerializeField] private Image rewardImage;
        [SerializeField] private Image runImage;
        [SerializeField] private Image requirementImage;
       

        private void Awake()
        {
          
        }

        public override void InitializeCard(CardData data)
        {
            base.InitializeCard(data);
            this.requirementImage.sprite = data.RequirementImage;
            this.rewardImage.sprite = data.RewardImage;
            this.runImage.sprite = data.RunImage;
     
            this.RequirementValue = UnityEngine.Random.Range(1, 4);
            this.RunValue = UnityEngine.Random.Range(1, 4);
            //this.rewardData = CardCollection.Instance.RetrieveCardsOfSpecificType(this.RewardType, this.RewardValue);
            if (this.Boon)
            {
                this.RequirementTransform.gameObject.SetActive(false);
            }
            this.UpdateInformationText();
         //   this.RewardData = CardCollection.Instance.RetrieveRandomCardData(CardType.Card, UnityEngine.Random.Range(0, this.RewardValue), true);
        }

        private void Update()
        {
            //if (Input.GetMouseButtonUp(0))
            //{
            //    Debug.Log("Released mouse above event card");
            //}
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
           // Debug.Log("Entered event card");
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            //Debug.Log("Exited event card");            
        }

        public override List<CardData> Interact(CardBase card)
        {
            List<CardData> results = base.Interact(card);
            this.UpdateInformationText();
            return results;
        }

        void UpdateInformationText()
        {
            this.rewardText.text = $"{this.RewardValue.ToString()}";
            this.runText.text = $"{this.RunValue.ToString()}";
            this.requirementText.text = this.RequirementValue.ToString();
        }

        private void OnDestroy()
        {
            
        }

        /// <summary>
        /// Action called when calling graveyard actions.
        /// </summary>
        /// <param name="value"></param>
        public void RunAction(int value)
        {
            this.RunValue -= value;
            this.UpdateInformationText();
        }
    }
}
