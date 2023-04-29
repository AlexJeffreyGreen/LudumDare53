using Assets.Scripts.Deck;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] Image portraitSpriteRenderer;
    [SerializeField] Image borderRenderer;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] int value;
    [SerializeField] new string name;
    [SerializeField] private float hoverHeight;
    private Vector3 handPosition;
    private BoxCollider2D boxCollider2D;
    private Canvas canvas;
    private RectTransform rectTransform;
    private bool isHovering;
    public void InitializeCard(CardData data)
    {
        portraitSpriteRenderer.sprite = data.Image;
        descriptionText.text = data.Description;
        value = data.Value;
        name = data.Name;
        rectTransform = this.GetComponent<RectTransform>();
        //Rect rect = rectTransform.rect;
        //rect.height = 0;
        //rect.width = 0;
        rectTransform.sizeDelta = new Vector2(0,0);//.height = 0;

    }

    private void Awake()
    {
        //bug
        this.GetComponent<Canvas>().worldCamera = Camera.main;
        this.boxCollider2D = this.GetComponent<BoxCollider2D>();
        this.canvas = this.GetComponent<Canvas>();
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
        this.canvas.sortingOrder = layer;
    }

    public Bounds GetBorderBounds()
    {
        return this.boxCollider2D.bounds;
    }
}
