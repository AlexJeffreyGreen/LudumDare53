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
        public Sprite Image; 
    }
}
