using Assets.Scripts.Deck;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : CardBase
{
  
    [SerializeField] int value;
    [SerializeField] private float hoverHeight;
    private Vector3 handPosition;
    
    private bool isHovering;
    public void InitializeCard(CardData data)
    {
        this.PortraitSpriteRenderer.sprite = data.Image;
        this.DescriptionText.text = data.Description;
        this.value = data.Value;
        this.name = data.Name;
        this.TitleText.text = data.Name;
    }

    private void Awake()
    {
        //bug
        this.GetComponent<Canvas>().worldCamera = Camera.main;
        //this.BoxCollider2D = this.GetComponent<BoxCollider2D>();
        ///this.Canvas = this.GetComponent<Canvas>();
        //this.RectTransform = this.GetComponent<RectTransform>();
        //this.RectTransform.sizeDelta = new Vector2(0, 0);//.height = 0;
    }

    //matter to other objs
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public void SetHandPosition(Vector3 newPositon)
    {
        this.handPosition = newPositon;
    }

    private void OnMouseEnter()
    {
        //isHovering = true;   
        if (this.GetComponentInParent<Hand>() != null && this.GetComponentInParent<Hand>().selectedCard == null)
            transform.position = handPosition + new Vector3(0, hoverHeight, 0);
    }

    private void OnMouseExit()
    {
        
        if (this.GetComponentInParent<Hand>() != null && this.GetComponentInParent<Hand>().selectedCard == null)
            transform.DOMoveY(handPosition.y, 0.2f);//.position = handPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //overlapping of graveyard
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       // if (collision == null) return;
       // Debug.Log("Card Exited Overlap with: " + collision.gameObject.name);
    }

    public void ChangeLayerAndAllChildren(int layer)
    {
        this.Canvas.sortingOrder = layer;
    }

    public Bounds GetBorderBounds()
    {
        return this.BoxCollider2D.bounds;
    }
}
