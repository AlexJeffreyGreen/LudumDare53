using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Deck
{
    public class CardCollection : MonoBehaviour
    {
        [SerializeField] private List<CardData> cards;
        [SerializeField] private List<CardData> eventCards;
        [SerializeField] private List<CardData> villagerCards;

        private static CardCollection _instance;
        public static CardCollection Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CardCollection>();
                    if (_instance == null)
                    {
                        GameObject gameObject = new GameObject();
                        _instance = gameObject.AddComponent<CardCollection>();
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            _instance = this as CardCollection;
            DontDestroyOnLoad(_instance.transform.gameObject);
        }


        public List<CardData> Cards
        {
            get
            {
                return cards;
            }
        }

        public List<CardData> EventCards
        {
            get
            {
                return eventCards;
            }
        }

        public List<CardData> RetrieveRandomCardData(CardType type, int count = 1, bool random = false)
        {
            if (cards == null || eventCards == null) throw new System.Exception("Missing cards");
            List<CardData> returnData = new List<CardData>();
            List<CardData> tmpCollection;
            switch (type)
            {
                case CardType.Event:
                    tmpCollection = eventCards;
                    break;
                case CardType.Request:
                    tmpCollection = villagerCards;
                    break;
                default:
                    tmpCollection = cards;
                    break;
            }
            for (int i = 0; i < count; i++)
            {
                int index = 0;
                if (random)
                    index = UnityEngine.Random.Range(0, tmpCollection.Count);
                else
                    index = tmpCollection.Count - i;

                returnData.Add(tmpCollection[index]);
            }
            //Random.ne
            return returnData;
        }

        public List<CardData> RetrieveCardsOfSpecificType(ResourceType type, int amount)
        {
            if (cards == null) throw new Exception("Missing cards");
            if (type == ResourceType.Rep) return null;
            List<CardData> data = new List<CardData>();
            for (int i = 0; i < amount; i++)
                data.Add(cards.Where(x => x.ResourceType == type).First());
            return data;
        }
    }
}
