using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Deck
{
    [CreateAssetMenu (fileName = "CardData", menuName = "Cards", order = 1)]
    public class CardData : ScriptableObject
    {
        public string Name;
        public string Description;
        public int RewardValue;
        public int RequirementValue;
        public int RunValue;
        public Sprite Image;
        public Sprite BackgroundImage;
        public Sprite TitleImage;
        public Sprite DescriptionImage;
        public Sprite BorderImage;
        public Sprite RewardImage;
        public Sprite RequirementImage;
        public Sprite RunImage;
        public ResourceType ResourceType;
        public ResourceType RewardType;
        public bool Boon;
        public CardType CardType;
    }

    public enum ResourceType
    {
        Wood,
        Food,
        Water,
        Weapon,
        Rep
    }

    public enum CardType
    {
        Card,
        Event,
        Request
    }

}
