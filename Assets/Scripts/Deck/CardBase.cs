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
    public abstract class CardBase : MonoBehaviour
    {
        [SerializeField] private ResourceType resourceType;
        [SerializeField] private int rewardValue;
        [SerializeField] private int runValue;
        [SerializeField] private int requirementValue;
        [SerializeField] private Image portraitImage;
        [SerializeField] private Image borderImage;
        [SerializeField] private Image titleImage;
        [SerializeField] private Image descriptionImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private bool boon;
        [SerializeField] private CardType cardType;
        private BoxCollider2D boxCollider2D;
        private Canvas canvas;
        private RectTransform rectTransform;
        

        public virtual void InitializeCard(CardData data)
        {
            this.PortraitImage.sprite = data.Image;
            this.DescriptionText.text = data.Description;
            this.BorderImage.sprite = data.BorderImage;
            this.BackgroundImage.sprite = data.BackgroundImage;
            this.TitleImage.sprite = data.TitleImage;
            this.rewardValue = data.RewardValue;
            this.requirementValue = data.RequirementValue;
            this.runValue = data.RunValue;
            this.name = data.Name;
            this.TitleText.text = data.Name;
            this.resourceType = data.ResourceType;
            this.boon = data.Boon;
            this.cardType = data.CardType;
        }

        /// <summary>
        /// Returns a Card for loot.
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public virtual CardData Interact(CardBase card)
        {
            if (this.Boon) return CardCollection.Instance.RetrieveCardOfSpecificType(this.ResourceType, 1);
            if (card == null) return null; 
            if (card.ResourceType !=  this.ResourceType) return null;
            this.requirementValue -= card.requirementValue;
            if (this.requirementValue <= 0) return CardCollection.Instance.RetrieveRandomCardData(CardType.Card, 1, true).First();
            return null;
        }

        public bool Boon { get { return this.boon; } }
        public CardType CardType { get { return this.cardType; } }
        public ResourceType ResourceType { get { return resourceType; }  }
        public int RewardValue { get { return this.rewardValue; } }
        public int RunValue { get { return this.runValue; }  }
        public int RequirementValue { get { return requirementValue; } }
        protected Image PortraitImage { get { return this.portraitImage; } }
        protected Image BorderImage { get { return this.borderImage; } }
        protected Image TitleImage { get { return this.titleImage; } }
        protected Image BackgroundImage { get { return this.backgroundImage; } }    

        protected TMP_Text TitleText { get { return this.titleText; } }
        protected TMP_Text DescriptionText { get { return this.descriptionText; } }
        protected BoxCollider2D BoxCollider2D { get { if (this.boxCollider2D == null) { this.boxCollider2D = this.GetComponent<BoxCollider2D>(); } return this.boxCollider2D; } }
        
        protected RectTransform RectTransform { get { if (this.rectTransform == null) { this.rectTransform = this.GetComponent<RectTransform>(); } return this.rectTransform; } }
        protected Canvas Canvas { get { if (this.canvas == null) { this.canvas = this.GetComponent<Canvas>(); this.canvas.worldCamera = Camera.main; } return this.canvas; } }
    
    }

}
