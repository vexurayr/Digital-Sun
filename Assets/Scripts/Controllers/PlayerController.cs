using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Text worldToolTipUI;
    [SerializeField] private Text inventoryToolTipUI;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float mouseSensitivityX;
    [SerializeField] private float mouseSensitivityY;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float gravity;
    [SerializeField] [Range(0.0f, 0.5f)] private float moveSmoothTime;
    [SerializeField] [Range(0.0f, 0.5f)] private float mouseSmoothTime;
    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    // Range at which the player can interact with InventoryItems
    [SerializeField] private float pickupRange;
    [SerializeField] private float pickupMinDropDistance;
    [SerializeField] private float pickupMaxDropDistance;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode pickUpKey = KeyCode.E;
    [SerializeField] private KeyCode inventoryKey = KeyCode.Tab;
    // For testing purposes
    [SerializeField] private KeyCode selfKillKey = KeyCode.K;
    [SerializeField] private KeyCode selfLiveKey = KeyCode.L;

    private InventoryUI inventoryUI;

    private bool isMovementLocked = false;
    private bool isCameraMovementLocked = false;
    private bool isInventoryActive = false;

    private bool isCursorLocked = true;
    private float cameraPitch = 0.0f;
    private float velocityY = 0.0f;
    private bool isJumping;
    CharacterController playerController = null;

    private Vector2 currentDirection = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;

    private Vector2 currentMouseDelta = Vector2.zero;
    private Vector2 currentMouseDeltaVelocity = Vector2.zero;

    private void Start()
    {
        // Lets the character controller handle movement and collision, applies the value to this component
        playerController = GetComponent<CharacterController>();

        if (isCursorLocked)
        {
            // Locks the cursor to the center of the screen
            Cursor.lockState = CursorLockMode.Locked;

            // Makes the cursor invisible
            Cursor.visible = false;
        }

        inventoryUI = GetComponent<PlayerInventory>().GetInventoryUI();
    }

    private void Update()
    {
        if (!isCameraMovementLocked)
        {
            UpdateMouseLook();
        }
        if (!isMovementLocked)
        {
            UpdateMovement();
        }
        if (isInventoryActive)
        {
            UpdateInventoryToolTip();
        }
        CheckForPickups();
        CheckOtherInputs();
    }

    private void UpdateMouseLook()
    {
        // Saves the x and y position of the mouse on the screen
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // Smooths camera movement
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        // Saves the camera's vertical rotation as the inverse of mouse movement
        cameraPitch -= currentMouseDelta.y * mouseSensitivityY;

        // Guarentees the camera does not rotate beyond looking straight up or straight down
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        // Rotates the camera vertically
        playerCamera.transform.localEulerAngles = Vector3.right * cameraPitch;

        // Rotates the parent object left and right based on the mouse's x (horizontal) position
        this.gameObject.transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivityX);
    }

    private void UpdateMovement()
    {
        Vector3 playerVelocity = Vector3.zero;

        // Storing the axis
        Vector2 targetDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Normalize the vector so every direction is cappped at the same speed
        targetDirection.Normalize();

        // Eases into movement so it's not instantaneous
        currentDirection = Vector2.SmoothDamp(currentDirection, targetDirection, ref currentDirVelocity, moveSmoothTime);

        // Reset the rate the player falls if they are touching the ground
        if (playerController.isGrounded)
        {
            velocityY = 0.0f;
        }

        // The downward acceleration
        velocityY += gravity * Time.deltaTime;

        // Sets the player's speed using the vectors scaled by their axis
        if (Input.GetKey(sprintKey) && GetComponent<Stamina>().GetCurrentValue() > 0)
        {
            GetComponent<Stamina>().DecCurrentValueOverTime();

            playerVelocity = (transform.forward * currentDirection.y + transform.right * currentDirection.x)
                * sprintSpeed + Vector3.up * velocityY;
        }
        else
        {
            playerVelocity = (transform.forward * currentDirection.y + transform.right * currentDirection.x)
                * walkSpeed + Vector3.up * velocityY;
        }

        // The character controller will do its thing
        playerController.Move(playerVelocity * Time.deltaTime);

        JumpInput();
    }

    private void JumpInput()
    {
        // Player can jump
        if (Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent()
    {
        playerController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            playerController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
            // Stops the player from going any higher if they hit a ceiling
        } while (!playerController.isGrounded && playerController.collisionFlags != CollisionFlags.Above);

        isJumping = false;
        playerController.slopeLimit = 45.0f;
    }

    private void CheckForPickups()
    {
        Vector3 cameraDirection = playerCamera.transform.forward;

        // Bit shift the index of layer 6 to get a bit mask
        int layerMask = 1 << 6;

        // Inverse the mask to ignore anything in layer 6 (ignore the player)
        layerMask = ~layerMask;

        Ray ray = new Ray(playerCamera.transform.position, cameraDirection);
        RaycastHit hit = new RaycastHit();

        bool isHitSuccess = Physics.Raycast(ray.origin, ray.direction, out hit, pickupRange, layerMask);

        Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.5f);

        worldToolTipUI.text = "";

        if (!isHitSuccess)
        {
            return;
        }

        InventoryItem hitInventoryItem = hit.collider.gameObject.GetComponent<InventoryItem>();
        PlayerInventory inventory = this.gameObject.GetComponent<PlayerInventory>();

        if (hitInventoryItem == null || inventory == null)
        {
            return;
        }

        worldToolTipUI.text = hitInventoryItem.GetItemCount() + " " + hitInventoryItem.GetItem();

        if (Input.GetKeyDown(pickUpKey))
        {
            hitInventoryItem.PickItemUp(inventory);
            Destroy(hitInventoryItem.gameObject);

            inventory.RefreshInventoryVisuals();
        }
    }

    public void CheckOtherInputs()
    {
        if (Input.GetKeyDown(inventoryKey))
        {
            ToggleInventoryUI();
        }

        if (Input.GetKeyDown(selfKillKey))
        {
            GetComponent<Health>().DecCurrentValue(10f);
        }
        else if (Input.GetKeyDown(selfLiveKey))
        {
            GetComponent<Health>().IncCurrentValue(5f);
        }
    }

    public void ToggleInventoryUI()
    {
        isInventoryActive = !isInventoryActive;

        if (isInventoryActive)
        {
            foreach (GameObject obj in inventoryUI.GetInvSlotsUI())
            {
                obj.gameObject.SetActive(true);
            }
            foreach (GameObject obj in inventoryUI.GetInvItemsUI())
            {
                obj.gameObject.SetActive(true);
            }

            inventoryUI.GetInvItemDiscardUI().SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            isCameraMovementLocked = true;
        }
        else
        {
            inventoryToolTipUI.text = "";

            foreach (GameObject obj in inventoryUI.GetInvSlotsUI())
            {
                obj.gameObject.SetActive(false);
            }
            foreach (GameObject obj in inventoryUI.GetInvItemsUI())
            {
                obj.gameObject.SetActive(false);
            }

            inventoryUI.GetInvItemDiscardUI().SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            isCameraMovementLocked = false;
        }    
    }
    
    public void UpdateInventoryToolTip()
    {
        int invItemIndex = GetInvItemIndexFromMouse();
        bool isInvItemIndexInvHand = IsInvItemIndexInvHand();
        
        inventoryToolTipUI.text = "";

        if (invItemIndex == -1)
        {
            return;
        }

        Inventory playerInventory = this.gameObject.GetComponent<Inventory>();
        List<InventoryItem> invItems = playerInventory.GetInvItemList();
        List<InventoryItem> invHandItems = playerInventory.GetInvHandItemList();
        InventoryItem invItem = null;

        if (!isInvItemIndexInvHand)
        {
            invItem = invItems[invItemIndex];
        }
        else
        {
            invItem = invHandItems[invItemIndex];
        }

        inventoryToolTipUI.gameObject.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 25,
                Input.mousePosition.z);

        if (invItem.GetItem() == InventoryItem.Item.None)
        {
            inventoryToolTipUI.text = "";
        }
        else
        {
            inventoryToolTipUI.text = invItem.GetItem().ToString();
        }
    }
    
    public int GetInvItemIndexFromMouse()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.GetComponent<IndexValue>() != null)
            {
                return raycastResults[i].gameObject.GetComponent<IndexValue>().GetIndexValue();
            }
        }
        
        return -1;
    }

    public bool IsInvItemIndexInvHand()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.GetComponent<IsInvHandItem>())
            {
                return true;
            }
        }

        return false;
    }

    public float GetPickupMinDropDistance()
    {
        return pickupMinDropDistance;
    }

    public float GetPickupMaxDropDistance()
    { 
        return pickupMaxDropDistance;
    }
}