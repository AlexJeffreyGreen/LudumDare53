using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Deck
{
    public class EventCard : CardBase
    {
        // public bool eventCard;

        [SerializeField] TMP_Text HealthBarText;
        [SerializeField] TMP_Text RunBarText;
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

        public bool Interact(Card card = null)
        {
            if (card == null && this.Boon) { return true; }
            if (card == null && !this.Boon) { return false; }
            if (card.ResourceType != this.ResourceType) { return false; }
            this.Value -= card.Value;
            this.UpdateInformationText();
            return true;
        }

        void UpdateInformationText()
        {
            this.HealthBarText.text = this.Value.ToString();
            this.RunBarText.text = this.RequirementValue.ToString();
        }

        private void OnDestroy()
        {
            
        }

    }
}
