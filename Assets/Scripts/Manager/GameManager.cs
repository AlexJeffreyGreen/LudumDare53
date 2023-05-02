using Assets.Scripts.Deck;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public Transform textPopup;


    [SerializeField] private AudioClip selectedCardClip;
    [SerializeField] private AudioClip[] graveyardClips;
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip runClip;
    [SerializeField] private AudioClip rewardClip;
    [SerializeField] private AudioClip[] encounterClips;

    [SerializeField] private EventCard eventCardPrefab;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Vector3 eventCardPosition = Vector3.zero;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Canvas navigationalCanvas;
    [SerializeField] private Button[] navigationalButtons;
    [SerializeField] private Button runButton;
    [SerializeField] private Button continueButton;

    [SerializeField] private List<Card> rewardCards;
    [SerializeField] private Button[] huntingNavigationalButtons;
    [SerializeField] private Deck deck;
    [SerializeField] private Graveyard graveyard;
    private int numberOfSteps;
    private Queue<string> steps= new Queue<string>();
    public EventCard CurrentEvent;
    private GameMode gameMode = GameMode.Navigation;
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
                        Card tmp = hit.collider.gameObject.GetComponent<Card>();
                        if (rewardCards.Contains(tmp))
                        {
                            rewardCards.Remove(tmp);
                        }
                        hand.SelectCard(tmp);
                        AudioManager.Instance.RandomSoundEffect(selectedCardClip);
                        break;
                    case "Event":
                        if (CurrentEvent.Boon)
                        {
                            this.InteractWithEvent();
                            Evaluate();
                            AudioManager.Instance.RandomSoundEffect(rewardClip);
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
                    AudioManager.Instance.RandomSoundEffect(attackClip);
                    //not super happy with this addition, but I will keep it because of the time restraint
                    this.InteractWithEvent(hand.selectedCard);
                    Evaluate();
       
                }
                else if (currentCard.HoveringOverGraveyard)
                {
                    Debug.Log("Hovering over graveyard and let go of mouse.");
                    hand.DeselectCard(true);
                    if (CurrentEvent != null)
                    {
                        CurrentEvent.RunAction(1);
                    }
                    Evaluate();
                    AudioManager.Instance.RandomSoundEffect(graveyardClips);
                }
                hand.PlaceCardsInHand();
            }
        }

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    reputationPoints++;
        //    UpdateUI();
        //}
        //else if (Input.GetKeyDown(KeyCode.S))
        //{
        //    reputationPoints--;
        //    UpdateUI();
        //}


        if (this.deck.CheckWin())
            this.SetGameMode(GameMode.Win);
        else if (this.deck.CheckLose())
            this.SetGameMode(GameMode.GameOver);

        Evaluate();
    }


    public void Evaluate()
    {
        if (this.CurrentEvent != null && !this.CurrentEvent.Boon && this.CurrentEvent.RunValue <= 0) 
        {
            this.runButton.gameObject.SetActive(true);
        }
        else
        {
            this.runButton.gameObject.SetActive(false);
        }
        if (this.CurrentEvent == null 
            && !this.deck.Excess() 
            && rewardCards.Count == 0 
            && gameMode != GameMode.Navigation 
            && hand.selectedCard == null)
        {
            this.continueButton.gameObject.SetActive(true);
            this.runButton.gameObject.SetActive(false);
        }
        else
        {
            this.continueButton.gameObject.SetActive(false);
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
                    if (this.CurrentEvent.RewardType == ResourceType.Rep)
                        this.deck.Reputation -= this.CurrentEvent.RewardValue;
                    Destroy(this.CurrentEvent.gameObject);
                    this.CurrentEvent = null;
                    //else
                    //{
                    //    this.deck.RemoveCardsOfResourceType(this.CurrentEvent.ResourceType, this.CurrentEvent.RunValue);
                    //}
                }
                AudioManager.Instance.RandomSoundEffect(runClip);
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
        AudioManager.Instance.RandomSoundEffect(encounterClips);
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
    /// Should be able to retrieve the card's reward data if the card is marked as complete...
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public void InteractWithEvent(Card card = null)
    {
        if (this.CurrentEvent == null) return;

        ResourceType type = this.CurrentEvent.ResourceType;

        List<CardData> results = this.CurrentEvent.Interact(card);

        //Completed Request, Boon or Not Boon



        if (results != null && results.Count > 0)
        {
            
            AudioManager.Instance.RandomSoundEffect(rewardClip);
            //this.deck.Reputation += this.CurrentEvent.RewardValue;
            foreach (CardData result in results)
            {

                Card tmp = Instantiate<Card>(cardPrefab);
                tmp.InitializeCard(result);
                tmp.transform.position = new Vector3(0, 3, 0);
                rewardCards.Add(tmp);
            }
        }
        
        //actually gross :(
        if (this.CurrentEvent.RequirementValue <= 0 || this.CurrentEvent.Boon)
        {
            if (this.CurrentEvent.RewardType == ResourceType.Rep)
            {
                this.deck.Reputation += this.CurrentEvent.RewardValue;
            }

            AudioManager.Instance.RandomSoundEffect(rewardClip);

            Destroy(this.CurrentEvent.gameObject);
            this.CurrentEvent = null;
        }

        if (card != null)
        {
            this.hand.DeselectCard((card.ResourceType == type));
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
        if (this.deck.Reputation > 10)
            gameMode = GameMode.Win;
        if (this.deck.Reputation <= 0)
            gameMode = GameMode.GameOver;

        this.gameMode = gameMode;
        foreach (Button button in navigationalButtons)
            button.gameObject.SetActive(false);
        this.runButton.gameObject.SetActive(false);
        this.continueButton.gameObject.SetActive(false);
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
            case GameMode.GameOver:
                SceneManager.LoadScene(2);
                break;
            case GameMode.Win:
                SceneManager.LoadScene(3);
               // SceneManager.LoadScene(3);
                //Debug.Log("You win.");
                break;
            default:
                this.deck.ResetHand();
                break;
        }
        if (this.gameMode == GameMode.Hunt || this.gameMode == GameMode.Village)
        {
            //if (this.gameMode != GameMode.Win 
            //    && this.gameMode != GameMode.GameOver 
            //    && this.gameMode != GameMode.Navigation)
            GenerateNewHunt();
        }
        //if (this.gameMode != GameMode.Navigation)


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
                this.deck.UpdateDeckUI();
                break;
            case GameMode.Navigation:
                foreach (Button button in navigationalButtons)
                    button.gameObject.SetActive(true);
                runButton.gameObject.SetActive(false);
                this.deck.gameObject.SetActive(false);
                this.graveyard.gameObject.SetActive(false);
                break;
        }

       // this.reputationText.text = $": {this.reputationPoints}";
    }


//    private void OnDestroy()
//    {
//        if (CardCollection.Instance != null)
//            Destroy(CardCollection.Instance.gameObject);
//    }
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
