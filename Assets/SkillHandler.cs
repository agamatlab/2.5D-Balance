using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SkillHandler : MonoBehaviour
{
    Material runeMaterial;
    GameObject rune;
    int alphaID;
    float timePressed;
    bool isHolding = false;
    bool isActivated = false;
    float requiredHoldTime = 1.5f;
    Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        alphaID = Shader.PropertyToID("_alpha");
        rune = GameObject.FindWithTag("Rune");
        runeMaterial = rune.GetComponent<Renderer>().material;
        originalScale = rune.transform.localScale;
        rune.transform.localScale = Vector3.zero; 
        // Ensure rune starts invisible (alpha = 1 means hidden)
        runeMaterial.SetFloat(alphaID, 1f);
        rune.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // When right mouse button is pressed
        if (Input.GetMouseButtonDown(1))
        {
            timePressed = Time.time;
            isHolding = true;
            // Don't activate rune yet
        }

        // While right mouse button is held down
        if (Input.GetMouseButton(1) && isHolding)
        {
            float holdTime = Time.time - timePressed;

            LeanTween.scale(rune, originalScale, 0.5f)
                .setEase(LeanTweenType.easeOutQuad);

            // Only activate after holding for 3 seconds
            if (holdTime >= requiredHoldTime && !isActivated)
            {
                isActivated = true;
                rune.SetActive(true);
                // Decrease alpha from 1 to 0 to make it visible
                LeanTween.value(rune, 1f, 0f, 0.5f)
                    .setEase(LeanTweenType.easeOutQuad)
                    .setOnUpdate((float val) =>
                    {
                        runeMaterial.SetFloat(alphaID, val);
                    });
            }
        }

        // When right mouse button is released
        if (Input.GetMouseButtonUp(1))
        {
            isHolding = false;

            if (isActivated)
            {
                LeanTween.cancel(rune);
                LeanTween.cancel(gameObject);

                LeanTween.scale(rune, Vector3.zero, 0.5f)
                    .setEase(LeanTweenType.easeInQuad);

                LeanTween.value(rune, 0f, 1f, 0.5f)
                    .setEase(LeanTweenType.easeInQuad)
                    .setOnUpdate((float val) => {
                        runeMaterial.SetFloat(alphaID, val);
                    })
                    .setOnComplete(() => {
                        rune.SetActive(false);
                        isActivated = false;
                    });
            }
        }
    }
}