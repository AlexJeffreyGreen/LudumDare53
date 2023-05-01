using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    public static TextPopup Create(Vector3 position, int amount)
    {
        Transform textPopup = Instantiate(GameManager.Instance.textPopup, Vector3.zero, Quaternion.identity);
        TextPopup popup = textPopup.GetComponent<TextPopup>();
        popup.Setup(amount);
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

    public void Setup(int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
        textColor = textMesh.color;
        disappearTimer = 1f;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveYSpeed = 2f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0) 
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
