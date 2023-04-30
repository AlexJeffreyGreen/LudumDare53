using Assets.Scripts.Deck;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int reputationPoints;
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

            if (hand.selectedCard != null)
            {
                Card currentCard = hand.selectedCard.GetComponent<Card>();
                if (!currentCard.HoveringOverEvent && !currentCard.HoveringOverGraveyard)
                {
                    hand.DeselectCard();
                }

                if (currentCard.HoveringOverEvent && !CurrentEvent.Boon)
                {
                    //not super happy with this addition, but I will keep it because of the time restraint
                    this.InteractWithEvent(hand.selectedCard.GetComponent<Card>());
                    hand.DeselectCard(true);
                    Evaluate();
                
                }
                else if (currentCard.HoveringOverGraveyard)
                {
                    Debug.Log("Hovering over graveyard and let go of mouse.");
                    hand.DeselectCard(true);
                    Evaluate();
                }
                hand.PlaceCardsInHand();
            }

        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero);
            if (hit.collider != null)
            {
                switch (hit.collider.gameObject.tag)
                {
                    case "Card":
                        hand.SelectCard(hit.collider.gameObject.transform);
                        break;
                    case "Event":
                        if (CurrentEvent.Boon)
                        {
                            hand.AddCard(hit.collider.gameObject.transform);
                            this.InteractWithEvent();
                            Evaluate();
                        }
                        break;
                }

            }
        }
    }

    public void Evaluate()
    {
        // If there is no longer an event AND the deck is not to excess.
        if (this.CurrentEvent == null && !this.deck.Excess())
        {
            Debug.Log("Destroyed!");
            this.SetGameMode(GameMode.Navigation);
        }
     
    }
    public void Progress()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        string buttonName = clickedButton.name;

        Debug.Log("Clicked button " + buttonName);
        steps.Enqueue(buttonName); // maybe I will do something with this
        GameMode tmp = GameMode.Navigation;
        switch (buttonName)
        {
            case "Village":
                tmp = GameMode.Village;
                break;
            default:
                tmp = GameMode.Hunt;
                break;
        }
        Debug.Log(tmp.ToString());
        this.SetGameMode(tmp);
        GenerateNewHunt();
    }

    private void GenerateNewHunt()
    {
        CardType type = CardType.Request;
        if (this.gameMode == GameMode.Hunt) { type = CardType.Event; }
        List<CardData> cards = CardCollection.Instance.RetrieveRandomCardData(type, 1, true);
        EventCard card = Instantiate<EventCard>(eventCardPrefab);
        card.InitializeCard(cards[0]);
        card.transform.position = eventCardPosition;
        CurrentEvent = card;
    }
   
    /// <summary>
    /// This is horribly messy, but I am tired.
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public void InteractWithEvent(Card card = null)
    {
        if (CurrentEvent == null) return;
        bool result = CurrentEvent.Interact(card);


        if (result)
        {
            if (CurrentEvent.Value <= 0 || CurrentEvent.Boon)
            {
                Destroy(CurrentEvent.gameObject);
                CurrentEvent = null;
            }
        }

        return;
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
            default:
                this.deck.ResetHand();
                break;
        }
    }
}


public enum GameMode
{
    Village,
    Hunt,
    Loot,
    Run,
    Navigation,
    GameOver
}
