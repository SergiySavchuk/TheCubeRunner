using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Class for controlling the player
public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CubeCounterController cubeCounterController;

    [SerializeField] private Animator animator;

    [SerializeField] private float horizontalMovementSpeed = 10f;
    [SerializeField] private float verticalMovementSpeed = 20f;

    [SerializeField] private LayerMask cubeMask;

    private float step;
    private int targetLane = 3;
    private int curentLane = 3;

    //For reseting player position at the restaring of the game
    private Vector3 startPosition;
    //For skipping first frame after player touched button "Try again"
    private bool skipframeOnRestart = false;

    private Vector3 jumpPosition = Vector3.zero;
    private bool animatingJump;

    private Stack<GameObject> playersCubes = new();

    private int cubesAdding = -1;

    private readonly float startingY = 1.5f;
    private readonly int cubeSize = 2;

    private readonly int jumpHash = Animator.StringToHash("Jump");

    private void Awake()
    {
        step = Screen.width / LevelSpawner.maximumLanes;

        //For reseting player position at the restaring of the game
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        gameManager.OnGameStart += ResetPosiion;
    }

    private void OnDisable()
    {
        gameManager.OnGameStart -= ResetPosiion;
    }

    //For reseting player position at the restaring of the game
    private void ResetPosiion()
    {
        jumpPosition = Vector3.zero;
        transform.position = startPosition;
        targetLane = curentLane = 3;
        skipframeOnRestart = true;
    }

    private void Update()
    {
        if (gameManager.Pause) return;

        //For skipping first frame after player touched button "Try again"
        if (skipframeOnRestart)
        {
            skipframeOnRestart = false;
            return;
        }

        //For more smoothing jump
        if (jumpPosition != Vector3.zero)
        {
            transform.position = Vector3.Lerp(transform.position, jumpPosition, Time.deltaTime * verticalMovementSpeed);

            if (transform.position == jumpPosition)
                jumpPosition = Vector3.zero;

            return;
        }

        animatingJump = false;

        float touchPosition = 0f;

        //Counting what lane player is touching
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            touchPosition = Mathf.Clamp(Input.mousePosition.x, 0 , Screen.width - 1);
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position.x;
#endif
            targetLane = (int)(touchPosition / step) + 1;
        }

        //Counting coordinate of the lane by formula y = a * x + b
        float coordinateOfTargetLane = 6f - 2f * targetLane;

        if (transform.position.z != coordinateOfTargetLane)
        {
            Vector3 targetPosition = transform.position;
            targetPosition.z = coordinateOfTargetLane;

            curentLane = GetCurrentLane();

            int GetCurrentLane()
            {
                float curentPosition = transform.position.z;

                if (curentPosition > 3) return 1;
                if (curentPosition > 1) return 2;
                if (curentPosition > -1) return 3;
                if (curentPosition > -3) return 4;
                return 5;
            }

            float coordinateOfCurentLane = 6f - 2f * curentLane;

            int sign = targetLane < curentLane ? 1 : -1;
            //Checking if player will collide with cubes if he moving the way he wants (cubes of the player have default mask)
            if (Physics.CheckBox(new Vector3(transform.position.x, 1.5f, coordinateOfCurentLane + sign * 2), new Vector3(2.5f, 0.5f, 0.5f), Quaternion.identity, cubeMask))
                targetPosition.z = coordinateOfCurentLane;

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * horizontalMovementSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Cheking only cubes that is on players lane
        if (other.transform.position.z != (6f - 2f * curentLane)) return;

        if (other.CompareTag("GoodCube"))
        {
            Cube otherCube = other.GetComponent<Cube>();

            //Checking if collider "other" has component "Cube" and if cube layer mask contains layer of collider "other" gameObject
            if (otherCube == null || (cubeMask.value & (1 << other.gameObject.layer)) == 0) return;

            animator.SetTrigger(jumpHash);

            cubesAdding++;

            //For smooth jumping
            jumpPosition = (jumpPosition == Vector3.zero ? transform.position : jumpPosition) + new Vector3(0, cubeSize);
            //Start to check if cube's centre is under player's centre 
            otherCube.StartCheckingPlayerPosiotion(transform.position.x, TakeCube);
            //Setting cube's mask as a default so physics check wont count it
            other.gameObject.layer = 0;
        }
        else if (other.CompareTag("BadCube"))
        {
            //When player hit more than one bad cube animation should play once
            if (!animatingJump)
            {
                animator.SetTrigger(jumpHash);
                animatingJump = true;
            }

            other.gameObject.SetActive(false);

            //If player hit bad cube before taking at least one good one
            if (playersCubes.Count == 0)
            {
                gameManager.GameOver();
                return;
            }

            playersCubes.Pop().SetActive(false);

            //For smooth jumping
            jumpPosition = (jumpPosition == Vector3.zero ? transform.position : jumpPosition) - new Vector3(0, cubeSize);

            if (playersCubes.Count == 0)
            {
                transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
                gameManager.GameOver();
            }
        }
    }

    //Callback to take cube
    private void TakeCube(Transform cube)
    {
        playersCubes.Push(cube.gameObject);

        cube.transform.SetParent(transform);
        cube.transform.localPosition = Vector3.zero;
        cube.transform.position = new Vector3(cube.transform.position.x, startingY + cubesAdding * cubeSize, cube.transform.position.z);

        cubesAdding--;

        cubeCounterController.AddCubeToCounter();
    }
}
