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
        [SerializeField] private Image portraitSpriteRenderer;
        [SerializeField] private Image borderRenderer;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text titleText;
        private BoxCollider2D boxCollider2D;
        private Canvas canvas;
        private RectTransform rectTransform;

        protected Image PortraitSpriteRenderer { get { return this.portraitSpriteRenderer; } }
        protected Image BorderRenderer { get { return this.borderRenderer; } }
        protected TMP_Text TitleText { get { return this.titleText; } }
        protected TMP_Text DescriptionText { get { return this.descriptionText; } }
        protected BoxCollider2D BoxCollider2D { get { if (this.boxCollider2D == null) { this.boxCollider2D = this.GetComponent<BoxCollider2D>(); } return this.boxCollider2D; } }
        
        protected RectTransform RectTransform { get { if (this.rectTransform == null) { this.rectTransform = this.GetComponent<RectTransform>(); } return this.rectTransform; } }
        protected Canvas Canvas { get { if (this.canvas == null) { this.canvas = this.GetComponent<Canvas>(); this.canvas.worldCamera = Camera.main; } return this.canvas; } }
    
    }
}
