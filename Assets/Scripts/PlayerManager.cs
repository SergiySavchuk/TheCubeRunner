using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CubeCounterController cubeCounterController;

    [SerializeField] private Animator animator;

    private int playersLane = 3;

    private Stack<GameObject> playersCubes;

    private int cubesAdding = -1;

    private readonly float startingY = 1.5f;
    private readonly int cubeSize = 2;

    private readonly int jumpHash = Animator.StringToHash("Jump");

    private void Awake()
    {
        playersCubes = new Stack<GameObject>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            MovePlayer(true);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            MovePlayer(false);
    }

    public void MovePlayer(bool right)
    {
        if (right && playersLane == 5) return;
        if (!right && playersLane == 1) return;

        if (gameManager.Pause) return;

        int sign = right ? 1 : -1;

        if (Physics.CheckBox(new Vector3(transform.position.x, 1.5f, transform.position.z - sign * 2), new Vector3(1.5f, 0.5f, 0.5f)))
            return;

        playersLane += sign;

        transform.position -= new Vector3(0, 0, 2 * sign);
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("GoodCube"))
        {
            Cube otherCube = other.GetComponent<Cube>();

            if (otherCube == null || playersCubes.Contains(other.gameObject)) return;

            animator.SetTrigger(jumpHash);

            cubesAdding++;

            transform.position += new Vector3(0, 2);

            otherCube.StartCheckingPlayerPosiotion(transform.position.x, TakeCube);
        }
        else if (other.CompareTag("BadCube"))
        {
            if (playersCubes.Count == 0)
            {
                gameManager.GameOver();
            }
            else
            {
                animator.SetTrigger(jumpHash);

                other.gameObject.SetActive(false);
                playersCubes.Pop().SetActive(false);

                transform.position -= new Vector3(0, 2);
            }
        }
    }

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
