using UnityEngine;

public class RecoilPattern : MonoBehaviour
{

    //Mouse Input
    private Vector3 mousePos;
    private Vector3 clickPos;

    //Bullet Prefab
    public GameObject shootPrefab;

    //Square in the Middle
    public GameObject middleSquare;
    public GameObject[] uiElements;

    //Last Position
    private Vector3 lastPosition;

    //The Initial Position
    public Transform initialPosition;
    
    //Original Initial Position save for Reset()
    private Transform originalInitial;

    [Header("Settings")]
    public Color bulletColor;
    public Vector3 size = new Vector3(2,2,2);
    public bool showMiddle = true;
    public bool showUIElements;

    //Our Recoil Pattern
    public Vector3[] recoilPattern;

    private int maxAmmo;
    private int counter = 0;

    private void Start()
    {
        //Set MaxAmmo to RecoilPattern.Lenght
        maxAmmo = recoilPattern.Length;
        //Set Orig. IniPos.
        originalInitial = initialPosition;

        //set custom color.
        initialPosition.GetComponent<Renderer>().material.color = bulletColor;

        //Set Size of the Initial Position
        initialPosition.transform.localScale = size;    
    }

    private void Update()
    {
        //Get Our Mouse Position in Screen Space
        mousePos = Input.mousePosition;

        //Convert the Mouse Position into World coordinates
        clickPos = Camera.main.ScreenToWorldPoint(mousePos);

        PlaceShoot();
        ResetPattern();
        HandleUI();
      
        //Check if RecoilArray <= 0 to Warn the Player.
        if (recoilPattern.Length <= 0)
        {
            Debug.LogError("PLEASE INCREASE RECOIL ARRAY: Hierarchy - Gamemanager - RecoilPattern.cs - recoilPattern");
        }
    }

    public void PlaceShoot()
    {
        if (recoilPattern.Length <= 0) return;

        //Check if we Press LMB
        if (Input.GetMouseButtonDown(0) && counter < maxAmmo)
        {
            //Instantiate the Bullet Prefab
            GameObject bullet = Instantiate(shootPrefab, clickPos, Quaternion.identity);

            bullet.transform.localScale = size;
            bullet.GetComponent<Renderer>().material.color = bulletColor;

            //Make Sure bullet is on Z = 0
            bullet.transform.position = new Vector3(bullet.transform.position.x, bullet.transform.position.y, 0f);           
            

            //Set the Last Position to this Bullet.
            lastPosition = bullet.transform.position;

            //Compare Distance relative to initialPosition
            Vector3 pos = ReturnDifference(lastPosition, initialPosition.transform.position);

            //Set Pos in Array
            recoilPattern[counter] = pos;

            //After Set the Position in Array set the Initial Position to the last bullet transform.
            //Remove this if you want the position from an absolut position.
            initialPosition = bullet.transform;
            
            //Increase the Array Counter
            counter++;
        }

    }

    public void ResetPattern()
    {

        //Check if user Press R
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Clear Array
            for (int i = 0; i < recoilPattern.Length; i++)
            {
                recoilPattern[i] = Vector3.zero;
            }

            //Delete Graphics
            GameObject[] shots = GameObject.FindGameObjectsWithTag("Shot");

            foreach (var shot in shots)
            {
                Destroy(shot);
            }
            //Reset Counter
            counter = 0;

            //Reset Initial Position
            initialPosition = originalInitial;
        }

       
    }

    public void HandleUI()
    {
        if (showMiddle)
        {
            middleSquare.SetActive(true);
        }
        else
        {
            middleSquare.SetActive(false);
        }

        if (showUIElements)
        {
            foreach (GameObject ui in uiElements)
            {
                ui.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject ui in uiElements)
            {
                ui.SetActive(false);
            }
        }
    }

    public Vector3 ReturnDifference(Vector3 pos1, Vector3 pos2)
    {
        return (pos1 - pos2);
    }
}
