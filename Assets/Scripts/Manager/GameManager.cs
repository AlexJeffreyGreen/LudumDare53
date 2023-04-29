using Assets.Scripts.Deck;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private List<CardData> cardCollection;
    [SerializeField] private List<CardData> eventCardCollection;
    [SerializeField] private EventCard eventCardPrefab;
    [SerializeField] private Vector3 eventCardPosition = Vector3.zero;
    public EventCard CurrentEvent;
    public GameMode GameMode = GameMode.Hunt; // Default

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.GameMode == GameMode.Hunt)
        {
            if (Input.GetKeyDown(KeyCode.D) && CurrentEvent == null)
            {
                List<CardData> cards = RetrieveRandomCardData(CardType.Event, 1, true);
                EventCard card = Instantiate<EventCard>(eventCardPrefab);
                card.InitializeCard(cards[0]);
                card.transform.position = eventCardPosition;
                CurrentEvent = card;
            }
        }
    }

   

    public List<CardData> RetrieveRandomCardData(CardType type, int count = 1, bool random = false)
    {
        if (cardCollection == null || eventCardCollection == null) throw new System.Exception("Missing cards");
        List<CardData> returnData = new List<CardData>();
        List<CardData> tmpCollection;
        switch (type)
        {
            case CardType.Event:
                tmpCollection = eventCardCollection;
                break;
            case CardType.Request:
                throw new System.NotImplementedException("Requests not implemented yet");
                break;
            default:
                tmpCollection = cardCollection;
                break;
        }
        for (int i = 0; i < count; i++)
        {
            int index = 0;
            if (random)
                index = Random.Range(0, tmpCollection.Count);
            else
                index = tmpCollection.Count - i;
            
            returnData.Add(tmpCollection[index]);
        }    
        //Random.ne
        return returnData;
    }

    public void InteractWithEvent(Card card)
    {
        if (CurrentEvent == null) return;
        CurrentEvent.Interact(card);
        if (CurrentEvent.Value <= 0)
        {
            Destroy(CurrentEvent.gameObject);
            CurrentEvent = null;
        }
    }
}

public enum CardType
{
    Card,
    Event,
    Request
}

public enum GameMode
{
    Village,
    Hunt,
    Loot,
    Run
}
