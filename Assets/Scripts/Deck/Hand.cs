using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    [SerializeField] Card cardPrefab;
    [SerializeField] int handCount;
    [SerializeField] float spacing;
    [SerializeField] int selectedCardLayer;
    [SerializeField] int defaultLayer;
    public List<Transform> Cards = new List<Transform>();
    public float radius = 2.0f;
    public float angleRange = 180f;
    public Transform selectedCard;



    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < handCount; i++)
        {
            Card card = Instantiate<Card>(cardPrefab, this.transform);
            Cards.Add(card.transform);
        }
    }

    private void PlaceCardsInHand()
    {
        Vector3 handPosition = this.transform.position;
        float cardWidth = cardPrefab.GetBorderBounds().size.x;//cardPrefab.GetComponent<Renderer>().bounds.size.x;
        float angle = angleRange / (Cards.Count - 1);
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
            Cards[i].transform.position = currentCardPosition;


        }


    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.tag == "Card")
            {
                selectedCard = hit.collider.gameObject.transform;
                selectedCard.GetComponent<Card>().ChangeLayerAndAllChildren(selectedCardLayer);
               // selectedCard.gameObject.layer = selectedCardLayer;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //hovering over graveyard
            //selectedCard.gameObject.layer = defaultLayer;
            selectedCard.GetComponent<Card>().ChangeLayerAndAllChildren(defaultLayer);
            selectedCard = null; 
            PlaceCardsInHand();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Card card = Instantiate<Card>(cardPrefab, this.transform);
            Cards.Add(card.transform);
            PlaceCardsInHand();
        }
        //if (selectedCard != null)
        //    Debug.Log(selectedCard.gameObject.layer);
       
    }

    void FixedUpdate()
    {
        if (selectedCard != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10.0f;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            selectedCard.transform.position = worldPos;
        }
    }

}
