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
        [SerializeField] Image rewardImage;
        [SerializeField] Image runImage;
        [SerializeField] Image requirementImage;
        public List<CardData> RewardData { get; private set; }

        private void Awake()
        {
          
        }

        public override void InitializeCard(CardData data)
        {
            base.InitializeCard(data);
            this.UpdateInformationText();
            this.RewardData = CardCollection.Instance.RetrieveRandomCardData(CardType.Card, UnityEngine.Random.Range(0, this.RequirementValue), true);
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

        public override CardData Interact(CardBase card)
        {
            CardData result = base.Interact(card);
            this.UpdateInformationText();
            return result;
        }

        void UpdateInformationText()
        {
            this.rewardText.text = $"+ {this.RewardValue.ToString()}";
            this.runText.text = $"- {this.RunValue.ToString()}";
            this.requirementText.text = this.RequirementValue.ToString();
        }

        private void OnDestroy()
        {
            
        }

    }
}
