using Assets.Scripts.Deck;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Hand : MonoBehaviour
{


    [SerializeField] private int defaultLayer;
    [SerializeField] int selectedCardLayer;

    [SerializeField] Card cardPrefab;
    [SerializeField] int handCount;
    [SerializeField] float maxHandCount;
    [SerializeField] float spacing;

    [SerializeField] int mousePositionToHandPosition = 350;
    [SerializeField] float offScreenHandDestination;
    [SerializeField] float defaultHandPosition;
    public List<Transform> Cards = new List<Transform>();
    public float radius = 2.0f;
    public float angleRange = 180f;
    public Transform selectedCard;


    private void Awake()
    {
        //this.transform.position = new Vector3(0, defaultHandPosition, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < handCount; i++)
        {
            Card card = Instantiate<Card>(cardPrefab, this.transform);
            Cards.Add(card.transform);
        }
    }

    public void PlaceCardsInHand()
    {
        Vector3 handPosition = this.transform.position;
        float cardWidth = cardPrefab.GetComponent<BoxCollider2D>().bounds.size.x;//.GetBorderBounds().size.x;
        float totalWidth = (Cards.Count - 1) * spacing;


        for (int i = 0; i < Cards.Count; i++)
            totalWidth += Cards[i].GetComponent<Card>().GetBorderBounds().size.x;

        float startX = -totalWidth / 2f;

        for (int i = 0; i < Cards.Count; i++)
        {
            //float normalizedAngle = Mathf.Lerp(0, angleRange, i / (float)(cards.Count - 1));
            //float y = Mathf.Sin(normalizedAngle * Mathf.Deg2Rad) * radius;
            //float x = -Mathf.Cos(normalizedAngle * Mathf.Deg2Rad) * radius + (i * (cardWidth + spacing));
            float normalizedAngle = Mathf.Lerp(0, angleRange, i / (float)(Cards.Count - 1));
            float y = Mathf.Sin(normalizedAngle * Mathf.Deg2Rad) * radius;
            float x = startX + (i * (Cards[i].GetComponent<Card>().GetBorderBounds().size.x + spacing));

            if (float.IsNaN(y))
            {
                y = handPosition.x;
            }
            if (float.IsNaN(x))
            {
                x = handPosition.y;
            }
            Vector3 currentCardPosition = new Vector3(x, y + handPosition.y, 0);
            //new Vector3(handPosition.x + x, handPosition.y + y, 0);
            Cards[i].GetComponent<Card>().SetHandPosition(currentCardPosition);
            //this.transform.d
            Cards[i].transform.DOMove(currentCardPosition, .25f);//.position = currentCardPosition;
            Debug.Log("Tested after dot tween issue part 2");

        }


    }



    // Update is called once per frame
    void Update()
    {

       // Debug.Log("test in update");
        //if (mousePosition.y < )

       
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Test");
          //  this.DrawCards(1, true);
        }

        //if (selectedCard != null)
        //    Debug.Log(selectedCard.gameObject.layer);
       
    }

    void FixedUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10.0f;

        Vector3 mouseToWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        if (selectedCard != null)
        {
            //Vector3 mousePos = Input.mousePosition;
            //mousePos.z = 10.0f;
            //Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            selectedCard.transform.position = mouseToWorldPosition;
        }
    }

    public void SelectCard(Transform transform)
    {
        this.selectedCard = transform;
        this.selectedCard.GetComponent<Card>().ChangeLayerAndAllChildren(selectedCardLayer);
        this.selectedCard.transform.SetParent(null);
        this.Cards.Remove(selectedCard);
    }

    public void DeselectCard(bool destroy = false)
    {
        this.selectedCard.GetComponent<Card>().ChangeLayerAndAllChildren(defaultLayer);
        this.selectedCard.SetParent(transform);

        if( destroy)
        {
            Destroy(this.selectedCard.gameObject);
            //this.Cards.Remove(this.selectedCard);
        }
        else
            this.Cards.Add(this.selectedCard);

        this.selectedCard = null;
    }
}
