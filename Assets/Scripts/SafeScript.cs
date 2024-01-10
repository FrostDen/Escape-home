//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using TMPro;

//public class SafeScript : MonoBehaviour
//{
//    [Tooltip("If it is false door can't be used")]
//    public bool Locked = false;
//    [Tooltip("It is true for remote control only")]
//    public bool Remote = false;
//    [Space]
//    [Tooltip("Door can be opened")]
//    public bool CanOpen = true;
//    [Tooltip("Door can be closed")]
//    public bool CanClose = true;
//    [Space]
//    [Tooltip("Door locked by red key (use key script to declarate any object as key)")]
//    public bool RedLocked = false;
//    public bool BlueLocked = false;
//    [Tooltip("It is used for key script working")]
//    InventoryItemController inventoryItemController;
//    [Space]
//    public bool isOpened = false;
//    [Range(0f, 4f)]
//    [Tooltip("Speed for door opening, degrees per sec")]
//    public float OpenSpeed = 3f;

//    // NearView()
//    float distance;
//    float angleView;
//    Vector3 direction;

//    // Hinge
//    [HideInInspector]
//    public Rigidbody rbDoor;
//    HingeJoint hinge;
//    JointLimits hingeLim;
//    float currentLim;

//    [SerializeField] public string correctCode = "14789";
//    private string inputCode = "";
//    private bool isInputtingCode = false;
//    public TMP_Text displayText;

//    public GameObject PinKeypad;

//    public RectTransform MobileDesktop;
//    public Vector3 hiddenPosition; // Off-screen position
//    public Vector3 originalPosition;
//    private bool isVisible;


//    void Start()
//    {
//        rbDoor = GetComponent<Rigidbody>();
//        hinge = GetComponent<HingeJoint>();
//        inventoryItemController = FindObjectOfType<InventoryItemController>();
//        isOpened = false;
//    }

//    void Update()
//    {
//            if (!Locked /*&& NearView()*/)
//            {
//                if (!isOpened && CanOpen)
//                {
//                    StartInputtingCode(); // Start inputting code when the safe is interacted with
//                    isOpened = false;
//                    rbDoor.AddRelativeTorque(new Vector3(0, 0, 20f));

//                    // You might trigger the puzzle entry here as well
//                }
//            }

//        // Check for user input
//        if (isInputtingCode)
//        {
//            // Your code to read input and check the combination...
//            // Example logic to read numpad input
//            if (Input.GetKeyDown(KeyCode.Alpha1))
//            {
//                AddToInputCode("1");
//            }
//            else if (Input.GetKeyDown(KeyCode.Alpha2))
//            {
//            AddToInputCode("2");
//            }
//            else if (Input.GetKeyDown(KeyCode.Alpha3))
//            {
//            AddToInputCode("3");
//            }
//            else if (Input.GetKeyDown(KeyCode.Alpha4))
//            {
//            AddToInputCode("4");
//            }
//            else if (Input.GetKeyDown(KeyCode.Alpha5))
//            {
//            AddToInputCode("5");
//            }
//            else if (Input.GetKeyDown(KeyCode.Alpha6))
//            {
//            AddToInputCode("6");
//            }
//            else if (Input.GetKeyDown(KeyCode.Alpha7))
//            {
//            AddToInputCode("7");
//            }
//        }
//    }

//    private void AddToInputCode(string value)
//    {
//        inputCode += value;

//        // Check if the input code matches the correct code
//        if (inputCode.Length <= correctCode.Length)
//        {
//            Debug.Log("Key pressed: " + value); // Check if the method is being called
//            displayText.text = inputCode; // Update displayText with the entered code

//            if (inputCode.Length == correctCode.Length)
//            {
//                if (inputCode == correctCode)
//                {
//                    if (PinKeypad != null)
//                    {
//                        // Get the off-screen position
//                        hiddenPosition = new Vector3(Screen.width * 40, 0, 0);
//                        // Move the menu off-screen initially
//                        PinKeypad.localPosition = hiddenPosition;
//                    }
//                    originalPosition = MobileDesktop.localPosition;
//                }
//                else
//                {
//                    // Code is incorrect, reset input
//                    StartCoroutine(ResetInput());
//                }
//            }
//        }
//    }

//    private IEnumerator ResetInput()
//    {
//        // Wait for a short duration before resetting the input code
//        yield return new WaitForSeconds(1f);
//        inputCode = "";
//        Debug.Log("Wrong code entered. Please try again.");
//        // You can add more actions here after an incorrect input
//    }

//    // Function to start inputting the code
//    private void StartInputtingCode()
//    {
//        // Start inputting the code
//        isInputtingCode = true;
//        inputCode = "";
//    }

//    // Function to stop inputting the code
//    private void StopInputtingCode()
//    {
//        // Stop inputting the code
//        isInputtingCode = false;
//    }

//    private void FixedUpdate() // door is physical object
//    {
//        if (isOpened)
//        {
//            currentLim = 0f;
//        }
//        else if (!isDoorFrozen) // Check if the door is not frozen
//        {
//            // Apply the hinge limits only when the door is not frozen
//            if (hingeLim.min > 1f)
//                hingeLim.min -= .5f * OpenSpeed;

//            // using values to door object
//            hingeLim.max = currentLim;
//            hingeLim.min = -100f; // Adjust this to your desired closed position
//            hinge.limits = hingeLim;
//        }
//    }

//    bool NearView() // it is true if you near interactive object
//    {
//        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
//        direction = transform.position - Camera.main.transform.position;
//        angleView = Vector3.Angle(Camera.main.transform.forward, direction);
//        if (angleView < 45f && distance < 2f) return true;
//        else return false;
//    }
//}
