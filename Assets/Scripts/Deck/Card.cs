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
  
    //[SerializeField] int value;
    [SerializeField] private float hoverHeight;
    private Vector3 handPosition;
    
    private bool isHovering;
    public bool HoveringOverEvent;
    public bool HoveringOverGraveyard;

    public override void InitializeCard(CardData data)
    {
        base.InitializeCard(data);
        this.Boon = true;
    }

    private void Awake()
    {
        this.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    //matter to other objs
    void Start()
    {
       // Debug.Log(this.ResourceType.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Hovering over graveyard: " + HoveringOverGraveyard);
        //if (Input.GetMouseButtonUp(0) && HoveringOverEvent)
        //{
        //    Destroy(this.gameObject);
        //}


        //if (Input.GetMouseButtonUp(0))
        //{
        //    Vector2 rayOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero);
        //    if (hit.collider != null && hit.collider.gameObject.tag == "Event")
        //    {
        //        Debug.Log("Over event card and let go.");
        //    }
        //}
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
        if (collision.gameObject.tag == "Event")
            HoveringOverEvent = true;
        if (collision.gameObject.tag == "Graveyard")
            HoveringOverGraveyard = true;

        //overlapping of graveyard
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Event")
            HoveringOverEvent = false;
        if (collision.gameObject.tag == "Graveyard")
            HoveringOverGraveyard = false;
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
