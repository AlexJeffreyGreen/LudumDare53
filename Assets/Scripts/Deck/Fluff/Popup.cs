using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Popup : MonoBehaviour
{
    private TMP_Text popUpText;
    private float disappearTimer = 1f;
    // Start is called before the first frame update

    private void Awake()
    {
        popUpText = GetComponent<TMP_Text>();
    }

    void Start()
    {
       
       // this.popUpText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            //float disappearSpeed = 13f;
            //textColor.a -= disappearSpeed * Time.deltaTime;
            //textMesh.color = textColor;
            //if (textColor.a < 0)
            //{
            popUpText.SetText("");
                //Destroy(gameObject);
           // }
        }
    }

    public void SetTextOnChange(string sign, string text, float timer)
    {
        if (sign == "+")
            popUpText.color = Color.green;
        else if (sign == "")
            popUpText.color = Color.red;

        this.disappearTimer = timer;
        popUpText.SetText($"{sign}{text}");

    }
}
