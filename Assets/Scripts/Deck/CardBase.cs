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
        [SerializeField] private int value;
        [SerializeField] private int requirementValue;
        [SerializeField] private Image portraitImage;
        [SerializeField] private Image borderImage;
        [SerializeField] private Image titleImage;
        [SerializeField] private Image descriptionImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private bool boon;
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
            this.Value = data.Value;
            this.RequirementValue = data.RequirementValue;
            this.name = data.Name;
            this.TitleText.text = data.Name;
            this.ResourceType = data.ResourceType;
            this.boon = data.Boon;
        }

        public bool Boon { get { return this.boon; } protected set { this.boon = value; } }
        public ResourceType ResourceType { get { return resourceType; } private set { this.resourceType = value; } }
        public int Value { get { return value; } protected set { this.value = value; } }
        public int RequirementValue { get { return requirementValue; } private set { this.requirementValue = value; } }
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
