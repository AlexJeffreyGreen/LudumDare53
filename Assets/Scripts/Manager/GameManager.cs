using Assets.Scripts.Deck;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private int maxReputation;
    [SerializeField] private int reputationPoints;
    [SerializeField] private EventCard eventCardPrefab;
    [SerializeField] private Vector3 eventCardPosition = Vector3.zero;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Canvas navigationalCanvas;
    [SerializeField] private Button[] navigationalButtons;
    [SerializeField] private Button runButton;
    [SerializeField] private TMP_Text reputationText;
    private int numberOfSteps;
    private Queue<string> steps= new Queue<string>();
    [SerializeField] private Button[] huntingNavigationalButtons;
    public EventCard CurrentEvent;
    private GameMode gameMode = GameMode.Navigation; // Default
    [SerializeField] private Deck deck;
    [SerializeField] private Graveyard graveyard;
    private Hand hand;
    


    void Start()
    {
        hand = deck.GetHand();
        SetGameMode(GameMode.Navigation);
    }

    // Update is called once per frame
    void Update()
    {

        


        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero);
            if (hit.collider != null)
            {
                switch (hit.collider.gameObject.tag)
                {
                    case "Card":
                        hand.SelectCard(hit.collider.gameObject.GetComponent<Card>());
                        break;
                    case "Event":
                        if (CurrentEvent.Boon)
                        {
                            this.InteractWithEvent();
                            Evaluate();
                        }
                        break;
                }

            }
        }

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
                    this.InteractWithEvent(hand.selectedCard);
                    
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

        if (Input.GetKeyDown(KeyCode.A))
        {
            reputationPoints++;
            UpdateUI();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            reputationPoints--;
            UpdateUI();
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
            case "North":
            case "West":
            case "East":
                tmp = GameMode.Hunt;
                break;
            case "Run":
                tmp = GameMode.Navigation;
                if(this.CurrentEvent != null)
                {
                    Debug.Log($"Running away from {this.CurrentEvent.name} and losing {this.CurrentEvent.RunValue} points");
                    if (this.CurrentEvent.CardType == CardType.Request)
                        this.reputationPoints -= this.CurrentEvent.RunValue;
                    else
                    {
                        this.deck.RemoveCardsOfResourceType(this.CurrentEvent.ResourceType, this.CurrentEvent.RunValue);
                    }
                }
                Debug.Log("Running away.");
                break;
            default:
                tmp = GameMode.Navigation;
                break;
        }
        Debug.Log(tmp.ToString());
        this.SetGameMode(tmp);
    
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
        if (this.CurrentEvent == null) return;
        CardData result = this.CurrentEvent.Interact(card);
        if (result != null)
        {
            if (this.CurrentEvent.RequirementValue <= 0 || this.CurrentEvent.Boon)
            {
               
                if (this.hand.selectedCard != null)
                    this.hand.DeselectCard(!this.CurrentEvent.Boon);
               
                if (result != null)
                {
                    this.hand.AddCard(result);
                    if (this.CurrentEvent.CardType == CardType.Request) {
                        this.reputationPoints += this.CurrentEvent.RewardValue;
                    }
                }

                Destroy(this.CurrentEvent.gameObject);
                this.CurrentEvent = null;
            }
        }
        else
        {
            this.hand.DeselectCard(false);
        }

        this.deck.UpdateDeckUI(); // this is just pure laziness and lack of time

        return;
    }

    public GameMode GetGameMode()
    {
        return this.gameMode;
    }
    public void SetGameMode(GameMode gameMode)
    {
        //last minute hack
        if (this.reputationPoints > 10)
            gameMode = GameMode.Win;
        if (this.reputationPoints <= 0)
            gameMode = GameMode.GameOver;

        this.gameMode = gameMode;
        foreach (Button button in navigationalButtons)
            button.gameObject.SetActive(false);
        this.runButton.gameObject.SetActive(false);
        //this.navigationalCanvas.gameObject.SetActive(false);
        switch (this.gameMode)
        {
            case GameMode.Navigation:
                this.deck.ResetHand();//(this.deck.GetHand());
                if(this.CurrentEvent != null)
                {
                    Destroy(this.CurrentEvent.gameObject);
                }
                break;
            case GameMode.Win:
                Debug.Log("You win.");
                break;
            case GameMode.GameOver:
                Debug.Log("You lose.");
                break;
            default:
                this.deck.ResetHand();
                break;
        }

        if (this.gameMode != GameMode.Navigation)
            GenerateNewHunt();

        UpdateUI();
    }

    public void UpdateUI()
    {
        switch (this.gameMode)
        {
            case GameMode.Village:
            case GameMode.Hunt:
                if (this.CurrentEvent != null && !this.CurrentEvent.Boon)
                    this.runButton.gameObject.SetActive(true);
                this.deck.gameObject.SetActive(true);
                this.graveyard.gameObject.SetActive(true);
                break;
            case GameMode.Navigation:
                foreach (Button button in navigationalButtons)
                    button.gameObject.SetActive(true);
                runButton.gameObject.SetActive(false);
                this.deck.gameObject.SetActive(false);
                this.graveyard.gameObject.SetActive(false);
                break;
        }
        this.deck.UpdateDeckUI();
        this.reputationText.text = $"Reputation: {this.reputationPoints}/{maxReputation}";
    }

}


public enum GameMode
{
    Village,
    Hunt,
    Loot,
    Run,
    Navigation,
    GameOver,
    Win
}
