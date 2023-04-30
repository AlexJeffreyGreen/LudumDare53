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
        public int Value;
        public int RequirementValue;
        public Sprite Image;
        public Sprite BackgroundImage;
        public Sprite TitleImage;
        public Sprite DescriptionImage;
        public Sprite BorderImage;
        public ResourceType ResourceType;
        public bool Boon;
    }

    public enum ResourceType
    {
        Wood,
        Food,
        Water,
        Weapon,
        Shield
    }
}
