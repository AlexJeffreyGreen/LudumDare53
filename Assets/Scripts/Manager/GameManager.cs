using Assets.Scripts.Deck;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private EventCard eventCardPrefab;
    [SerializeField] private Vector3 eventCardPosition = Vector3.zero;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Canvas navigationalCanvas;
    private int numberOfSteps;
    private Queue<string> steps= new Queue<string>();
    //[SerializeField] private Button[] huntingNavigationalButtons;
    public EventCard CurrentEvent;
    private GameMode gameMode = GameMode.Navigation; // Default
    [SerializeField] private Deck deck;
    private Hand hand;
    


    void Start()
    {
        hand = deck.GetHand();
        SetGameMode(GameMode.Navigation);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonUp(0))
        {
            if (hand.selectedCard != null && !hand.selectedCard.GetComponent<Card>().HoveringOverEvent)
            {
                hand.DeselectCard();
            }
            else if (hand.selectedCard != null && hand.selectedCard.GetComponent<Card>().HoveringOverEvent)
            {
                this.InteractWithEvent(hand.selectedCard.GetComponent<Card>());
                if (this.CurrentEvent == null)
                {
                    Debug.Log("Destroyed!");
                    this.SetGameMode(GameMode.Navigation);
                }
                hand.DeselectCard(true);
            }
            hand.PlaceCardsInHand();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.tag == "Card")
            {
                hand.SelectCard(hit.collider.gameObject.transform);
            }
        }
    }
    public void ProgressHunt()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        string buttonName = clickedButton.name;

        Debug.Log("Clicked button " + buttonName);
        steps.Enqueue(buttonName); // maybe I will do something with this

        this.SetGameMode(GameMode.Hunt);
        GenerateNewHunt();
    }

    private void GenerateNewHunt()
    {
        List<CardData> cards = this.deck.CardCollection.RetrieveRandomCardData(CardType.Event, 1, true);
        EventCard card = Instantiate<EventCard>(eventCardPrefab);
        card.InitializeCard(cards[0]);
        card.transform.position = eventCardPosition;
        CurrentEvent = card;
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

    public GameMode GetGameMode()
    {
        return this.gameMode;
    }
    public void SetGameMode(GameMode gameMode)
    {
        this.gameMode = gameMode;
        this.navigationalCanvas.gameObject.SetActive(false);
        switch (this.gameMode)
        {
            case GameMode.Hunt:
                break;
            case GameMode.Run:
                break;
            case GameMode.Navigation:
                this.navigationalCanvas.gameObject.SetActive(true);
                this.deck.ResetHand();//(this.deck.GetHand());
                break;
            default: break;
        }
    }
}


public enum GameMode
{
    Village,
    Hunt,
    Loot,
    Run,
    Navigation
}
