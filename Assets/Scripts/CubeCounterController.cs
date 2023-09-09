using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CubeCounterController : MonoBehaviour
{
    [SerializeField] private Animator cubeCounterAnimator;
    [SerializeField] private TextMeshProUGUI cubeCounterText;

    private int cubeCounter = 0;

    private readonly int startHash = Animator.StringToHash("Start");

    public void AddCubeToCounter()
    {
        cubeCounter ++;

        cubeCounterText.text = $"+{cubeCounter}";

        StartCoroutine(StartAnimation());
    }

    private IEnumerator StartAnimation() 
    {
        yield return null;

        cubeCounter = 0;

        cubeCounterAnimator.SetTrigger(startHash);
    }
}
