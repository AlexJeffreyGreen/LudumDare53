using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Deck
{
    public class Deck : MonoBehaviour
    {
        public CardCollection CardCollection;
        [SerializeField] private Hand hand;
        [SerializeField] private Stack<Card> deck;
        [SerializeField] private int deckSize;
        [SerializeField] private Card cardPrefab;
        [SerializeField] private TMP_Text currentCount;

        private void Awake()
        {
            deck = new Stack<Card>();
            for(int i = 0; i < deckSize; i++)
            {
                Card tmp = Instantiate<Card>(cardPrefab, this.transform);
                List<CardData> data = CardCollection.RetrieveRandomCardData(CardType.Card, 1, true);//GameManager.Instance.
                tmp.InitializeCard(data[0]);
                tmp.gameObject.SetActive(false);
                //tmp.transform.position = 
                deck.Push(tmp);
            }
            UpdateText();
        }



        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 rayOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject.tag == "Deck")
                {
                    Debug.Log("Mouse down over deck");
                    
                    DrawCards(1);
                }
            }

            
        }

        public Hand GetHand()
        {
            return hand;
        }


        void DrawCards(int count)
        {
            //List<CardData> drawnCards = //GameManager.Instance.RetrieveRandomCardData(CardType.Card, count, random);
            if (this.deck.Count <= 0) { throw new Exception("Empty deck."); }


            List<Card> drawnCards = new List<Card>();

            for(int i = 0 ; i < count; i++) 
            { 
                drawnCards.Add(deck.Pop());
            }


            foreach (Card card in drawnCards)
            {
                hand.Cards.Add(card.transform);
                card.transform.SetParent(hand.transform);
                card.gameObject.SetActive(true);
            }
            hand.PlaceCardsInHand();
            UpdateText();
        }

        public void ResetHand()
        {
            AddCards(this.hand.Cards);
            this.hand.Cards = new List<Transform>();
        }

        public void AddCards(List<Transform> cards)
        {
            foreach (Transform card in cards)
            {
                Card tmp = card.GetComponent<Card>();
                tmp.transform.SetParent(this.transform);
                tmp.transform.position = this.transform.position;
                tmp.gameObject.SetActive(false);
                this.deck.Push(tmp);
            }
            this.deck.Shuffle();
            UpdateText();
        }

        void UpdateText()
        {
            this.currentCount.text = $"{deck.Count}/{deckSize}";
        }





    }
}

public static class Extensions
{
    public static void Shuffle<T>(this Stack<T> stack)
    {
        var values = stack.ToArray();
        stack.Clear();
        foreach (var value in values.OrderBy(x => UnityEngine.Random.Range(0, stack.Count)))
            stack.Push(value);
    }
}


public enum CardType
{
    Card,
    Event,
    Request
}
