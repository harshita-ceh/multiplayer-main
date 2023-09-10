using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class danceanimation : MonoBehaviour
{
    Animator anim;
    int currencyPoints = 0;
    public TMP_Text currencyPointsText; // Reference to the TextMeshPro Text component

    private void Start()
    {
        anim = GetComponent<Animator>();
        UpdateCurrencyPointsText();
    }

    private void Update()
    {
        // Use Input.GetKey instead of Input.GetKeyDown for continuous currency points addition while holding the "1" key.
        if (Input.GetKey(KeyCode.Alpha1))
        {
            anim.SetBool("dance1", true);
            AddCurrencyPoints(13);
        }
        else
        {
            anim.SetBool("dance1", false);
        }
    }

    // Method to add currency points
    private void AddCurrencyPoints(int pointsToAdd)
    {
        currencyPoints += pointsToAdd;
        UpdateCurrencyPointsText(); // Update the displayed currency points
    }

    // Method to update the TextMeshPro Text component with the current currency points
    private void UpdateCurrencyPointsText()
    {
        currencyPointsText.text = "Currency Points: " + currencyPoints.ToString();
    }
}
