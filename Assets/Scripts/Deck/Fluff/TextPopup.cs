using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace Assets.Scripts.Deck.Fluff {


    public class TextPopup : MonoBehaviour
    {
        
        public static TextPopup Create(Vector3 position, int amount, bool positive)
        {
            Transform textPopup = Instantiate(CardCollection.Instance.textPopupPrefab, position, Quaternion.identity);
            TextPopup popup = textPopup.GetComponent<TextPopup>();
            popup.Setup(amount, positive);
            return popup;
        }
        public static TextPopup Create(Transform parentTransform, string text, bool positive)
        {
            Transform textPopup = Instantiate(CardCollection.Instance.textPopupPrefab, parentTransform.position, Quaternion.identity, parentTransform);
            TextPopup popup = textPopup.GetComponent<TextPopup>();
            popup.Setup(text, positive);
            return popup;
        }

        private float disappearTimer;
        private TMP_Text textMesh;
        private Color textColor;
        [SerializeField] private Color greenColor;
        [SerializeField] private Color redColor;
        private void Awake()
        {
            textMesh = GetComponent<TMP_Text>();
        }

        public void Setup(int damageAmount, bool positive)
        {
            textMesh.SetText(damageAmount.ToString());
            if (positive)
                textMesh.color = greenColor;
            else
                textMesh.color = redColor;

            textColor = textMesh.color;
            disappearTimer = 1f;
        }

        public void Setup(string text, bool positive)
        {
            textMesh.SetText(text);
            if (positive)
                textMesh.color = greenColor;
            else
                textMesh.color = redColor;

            textColor = textMesh.color;
            disappearTimer = 10f;
        }



        // Start is called before the first frame update
        void Start()
        {
            if (this.disappearTimer == 0f)
                this.disappearTimer = 15f;
        }

        // Update is called once per frame
        void Update()
        {
            float moveYSpeed = .2f;
            this.GetComponent<RectTransform>().position += new Vector3(0, moveYSpeed) * Time.deltaTime;
            disappearTimer -= Time.deltaTime;
            if (disappearTimer < 0)
            {
                float disappearSpeed = 13f;
                textColor.a -= disappearSpeed * Time.deltaTime;
                textMesh.color = textColor;
                if (textColor.a < 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
