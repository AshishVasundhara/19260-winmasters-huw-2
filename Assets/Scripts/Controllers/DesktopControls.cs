using UnityEngine;

public class DesktopControls : MonoBehaviour
{
    private CharacterController characterController;
    private int attackInput = 0;
    private int thurstInput = 0;
    private GameManager GM;
    private MainGameManager MGM;
    private void Start()
    {
        GM = FindObjectOfType<GameManager>();
        MGM = FindObjectOfType<MainGameManager>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (GM.gameOn)
        {
            if (!MGM.isMobile)
            {
                if (Input.GetMouseButton(0))
                {
                    attackInput = 1;
                }
                else
                {
                    attackInput = 0;
                }
                if (Input.GetMouseButtonDown(1))
                {
                    thurstInput = 1;
                }
                else
                {
                    thurstInput = 0;
                }
                characterController.Push(attackInput);
                characterController.Thrust(thurstInput);
            }
        }
    }
}
