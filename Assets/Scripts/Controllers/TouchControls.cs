using UnityEngine;

public class TouchControls : MonoBehaviour
{
    private CharacterController CC;
    private DesktopControls player;
    private int attackToucheInputValue;
    private int thrustToucheInputValue;
    private bool isTouching;
    private GameManager GM;
    private MainGameManager MGM;
    private void Start()
    {
        MGM = FindObjectOfType<MainGameManager>();
        GM = FindObjectOfType<GameManager>();

        CC = GetComponent<CharacterController>();
        player = GetComponent<DesktopControls>();

    }
    void Update()
    {
        thrustToucheInputValue = 0;
        if (GM.gameOn)
        {
            if (MGM.isMobile)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        //thrust
                        if (touch.position.x < Screen.width / 2)
                        {
                            thrustToucheInputValue = 1;
                        }
                        //push
                        else if (touch.position.x > Screen.width / 2)
                        {
                            isTouching = true;
                        }
                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        attackToucheInputValue = 0;
                        isTouching = false;
                    }
                }

                CC.Push(attackToucheInputValue);
                CC.Thrust(thrustToucheInputValue);

                if (isTouching)
                {
                    attackToucheInputValue = 1;
                }
            }
        }
    }
}
