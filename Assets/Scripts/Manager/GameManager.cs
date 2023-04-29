using Assets.Scripts.Deck;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private CardData[] CardCollection;
    //[SerializeField] private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public CardData[] RetrieveRandomCardData(int count = 1, bool random = false)
    {
        if (CardCollection == null) throw new System.Exception("Missing cards");
        CardData[] returnData = new CardData[count];
        for (int i = 0; i < count; i++)
        {
            int index = 0;
            if (random)
                index = Random.Range(0, CardCollection.Length);
            else
                index = CardCollection.Length - i;
            
            returnData[i] = CardCollection[index];
        }    
        //Random.ne
        return returnData;
    }
}
