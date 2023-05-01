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
        //public CardCollection CardCollection;
        [SerializeField] private Hand hand;
        [SerializeField] private List<Card> deck;
        [SerializeField] private int deckSize;
        [SerializeField] private Card cardPrefab;
        [SerializeField] private TMP_Text currentCount;

        [SerializeField] private TMP_Text currentWaterCount;
        [SerializeField] private TMP_Text currentWeaponCount;
        [SerializeField] private TMP_Text currentFoodCount;
        [SerializeField] private TMP_Text currentWoodCount;
        private void Awake()
        {
            deck = new List<Card>();
            for(int i = 0; i < deckSize; i++)
            {
                Card tmp = Instantiate<Card>(cardPrefab, this.transform);
                List<CardData> data = CardCollection.Instance.RetrieveRandomCardData(CardType.Card, 1, true);//GameManager.Instance.
                tmp.InitializeCard(data[0]);
                tmp.gameObject.SetActive(false);
                //tmp.transform.position = 
                deck.Add(tmp);
            }
            UpdateDeckUI();
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
            if (this.deck.Count <= 0) { return; }
            if (this.hand.Cards.Count >= this.hand.MaxHandCount) { return; }


            List<Card> drawnCards = new List<Card>();

            for(int i = 0 ; i < count; i++) 
            {
                Card card = deck[0];
                deck.RemoveAt(0);
                drawnCards.Add(card);
            }


            foreach (Card card in drawnCards)
            {
                hand.Cards.Add(card);
                card.transform.SetParent(hand.transform);
                card.gameObject.SetActive(true);
            }
            hand.PlaceCardsInHand();
            UpdateDeckUI();
        }

        public void ResetHand()
        {
            AddCards(this.hand.Cards);
            this.hand.Cards = new List<Card>();
        }

        public void AddCards(List<Card> cards)
        {
            foreach (Card card in cards)
            {
                Card tmp = card.GetComponent<Card>();
                tmp.transform.SetParent(this.transform);
                tmp.transform.position = this.transform.position;
                tmp.gameObject.SetActive(false);
                this.deck.Add(tmp);
            }
            this.deck.Shuffle();
            UpdateDeckUI();
        }

        /// <summary>
        /// awful code, but I needed to get this done
        /// </summary>
        /// <param name="type"></param>
        /// <param name="count"></param>
        public void RemoveCardsOfResourceType(ResourceType resourceType, int count)
        {
            List<Card> cardsToDelete = deck
                .Where(c => c.ResourceType == resourceType)
                .Take(count)
                .ToList();

            if (cardsToDelete.Count < count)
            {
                cardsToDelete.AddRange(hand.Cards
                    .Where(c => c.GetComponent<Card>().ResourceType == resourceType)
                    .Take(count - cardsToDelete.Count));
            }

            foreach (Card card in cardsToDelete)
            {
                if (deck.Contains(card))
                {
                    deck.Remove(card);
                }
                else if (hand.Cards.Contains(card))
                {
                    hand.Cards.Remove(card);
                }

                Destroy(card.gameObject);
            }
            this.UpdateDeckUI();
        }

        public void UpdateDeckUI()
        {
            // int count = this.deck.Count;// + hand.Cards.Count;
            //if (hand.selectedCard != null )
            //{
            //    count += 1;
            //}

            int count = this.deck.Count() + this.hand.Cards.Count();

            if (count > deckSize)
                this.currentCount.color = Color.red;
            else
                this.currentCount.color = Color.white;
            this.currentCount.text = $"{this.deck.Count}/{deckSize}";

            int waterCount = this.deck.Where(x => x.ResourceType == ResourceType.Water).Count() + this.hand.Cards.Where(x => x.ResourceType == ResourceType.Water).Count();
            int foodCount = this.deck.Where(x => x.ResourceType == ResourceType.Food).Count() + this.hand.Cards.Where(x => x.ResourceType == ResourceType.Food).Count();
            int weaponCount = this.deck.Where(x => x.ResourceType == ResourceType.Weapon).Count() + this.hand.Cards.Where(x => x.ResourceType == ResourceType.Weapon).Count();
            int woodCount = this.deck.Where(x => x.ResourceType == ResourceType.Wood).Count() + this.hand.Cards.Where(x => x.ResourceType == ResourceType.Wood).Count();


            this.currentWaterCount.text = $": {waterCount}";
            this.currentFoodCount.text = $": {foodCount}";
            this.currentWeaponCount.text = $": {weaponCount}";
            this.currentWoodCount.text = $": {woodCount}";
        }


        public bool Excess()
        {
            return (this.hand.Cards.Count + this.deck.Count > this.deckSize);
        }



    }
}

public static class Extensions
{
    public static void Shuffle<T>(this List<T> stack)
    {
        var values = stack.ToArray();
        stack.Clear();
        stack.AddRange(values.OrderBy(x => UnityEngine.Random.Range(0, stack.Count)));
    }
}


