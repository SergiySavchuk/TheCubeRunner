using System.Collections;
using TMPro;
using UnityEngine;

//Class for representatin how many cubes player get
public class CubeCounterController : MonoBehaviour
{
    [SerializeField] private Animator cubeCounterAnimator;
    [SerializeField] private TextMeshProUGUI cubeCounterText;
    [SerializeField] private ParticleSystem puffEffect;

    private int cubeCounter = 0;

    private readonly int startHash = Animator.StringToHash("Start");

    public void AddCubeToCounter()
    {
        cubeCounter ++;

        cubeCounterText.text = $"+{cubeCounter}";

        StartCoroutine(StartAnimation());
    }

    //Waiting for the end of the frame so all cubes that player get at one row will be shown
    private IEnumerator StartAnimation() 
    {
        yield return null;

        puffEffect.Play();

        cubeCounter = 0;

        cubeCounterAnimator.SetTrigger(startHash);
    }
}
