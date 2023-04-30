using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Deck
{
    public class CardCollection : MonoBehaviour
    {
        [SerializeField] private List<CardData> cards;
        [SerializeField] private List<CardData> eventCards;
        [SerializeField] private List<CardData> villagerCards;

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
    }
}
