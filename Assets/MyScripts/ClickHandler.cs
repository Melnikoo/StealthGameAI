using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBarClickHandler : MonoBehaviour
{
    public KeyCode key;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();

    }

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            FadeToColor(button.colors.pressedColor);
            button.onClick.Invoke();
        }
        else if (Input.GetKeyUp(key))
        {
            FadeToColor(button.colors.normalColor);
        }

        void FadeToColor(Color color)
        {
            Graphic graphic = GetComponent<Graphic>();
            graphic.CrossFadeColor(color, button.colors.fadeDuration, true, true);
        }
    }
}