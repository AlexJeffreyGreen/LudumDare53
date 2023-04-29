using Assets.Scripts.Deck;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Card : MonoBehaviour
{
    //[SerializeField] private CardData cardScriptable;


    private Vector3 handPosition;
    [SerializeField] SpriteRenderer portraitSpriteRenderer;
    [SerializeField] SpriteRenderer borderRenderer;
    [SerializeField] TextMeshPro descriptionText;
    [SerializeField] int value;
    [SerializeField] new string name;
    [SerializeField] private float hoverHeight;
    public void InitializeCard(CardData data)
    {
        portraitSpriteRenderer.sprite = data.Image;
        descriptionText.text = data.Description;
        value = data.Value;
        name = data.Name;
    }

    private void Awake()
    {
        //bug
        this.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    //matter to other objs
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHandPosition(Vector3 newPositon)
    {
        this.handPosition = newPositon;
    }

    private void OnMouseEnter()
    {
        if (this.GetComponentInParent<Hand>() != null)
            transform.position = handPosition + new Vector3(0, hoverHeight, 0);
    }

    private void OnMouseExit()
    {
        if (this.GetComponentInParent<Hand>() != null)
            transform.position = handPosition;
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
        this.GetComponent<Canvas>().sortingOrder = layer;
        //this.gameObject.layer = layer;
        //foreach (Transform child in transform)
        //    child.gameObject.layer = layer;
    }

    public Bounds GetBorderBounds()
    {
        return this.borderRenderer.bounds;
    }
}
