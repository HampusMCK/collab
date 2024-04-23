using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    HealthSystem health;
    
    [Header("Movement")]
    public float speed;
    public float sprintSpeed;
    public float mouseSensetivity;

    [Header("UI Elements")]
    public GameObject CraftingUI;
    public GameObject InventoryUI;

    [Header("Inventory")]
    public List<GameObjects> Inventory;

    bool crafting = false;
    [HideInInspector] public bool inInventory = false;
    bool hasReleasedKey = true;

    Transform cam;

    int InventoryLengthMemory;

    Rigidbody rb;

    Ray ray;
    RaycastHit hit;
    Collider other = null;

    Chest chest;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
        rb = GetComponent<Rigidbody>();
        health = GetComponent<HealthSystem>();
    }

    private void FixedUpdate()
    {
        other = null;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
            other = hit.collider;

        if (other != null)
        {
            if (other.gameObject.tag == "Chest")
                chest = other.gameObject.GetComponent<Chest>();
        }
        else
        {
            chest = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Inventory.Count != InventoryLengthMemory)
            UpdateInventory();

        if (!crafting && !inInventory)
            getPlayerInput();
        else if (crafting)
            getPlayerCraftingInput();
        else if (inInventory)
            getPlayerInventoryInput();

        if (Input.GetAxisRaw("Action") == 0 && Input.GetAxisRaw("Inventory") == 0)
            hasReleasedKey = true;

        InventoryLengthMemory = Inventory.Count;
    }

    private void getPlayerInventoryInput()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (Input.GetAxisRaw("Inventory") > 0 && hasReleasedKey)
        {
            hasReleasedKey = false;
            InventoryUI.SetActive(false);
            inInventory = false;
        }
    }

    private void getPlayerCraftingInput()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (Input.GetAxisRaw("Action") > 0 && hasReleasedKey)
        {
            hasReleasedKey = false;
            CraftingUI.SetActive(false);
            crafting = false;
        }
    }

    private void getPlayerInput()
    {
        /*--------------Input Keys-----------
        Inventory = i
        Action = e
        Horizontal = A & D
        Vertical = W & S
        MouseButton(0) = Left Mouse Button
        MouseButton(1) = Right Mouse Button
        */

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveX, 0, moveZ) * Time.deltaTime * speed;
        transform.Translate(movement);

        //Camera movement
        float mouseHorizontal = Input.GetAxisRaw("Mouse X") * mouseSensetivity;
        float mouseVertical = Input.GetAxisRaw("Mouse Y") * mouseSensetivity;

        transform.Rotate(Vector3.up * mouseHorizontal); //Rotate player body left/right
        cam.Rotate(Vector3.right * -mouseVertical); //Rotate camera up/down

        if (Input.GetAxisRaw("Action") > 0 && hasReleasedKey) // Open crafting UI
        {
            hasReleasedKey = false;
            CraftingUI.SetActive(true);
            crafting = true;
        }

        if (Input.GetAxisRaw("Inventory") > 0 && hasReleasedKey) // Open Inventory UI
        {
            hasReleasedKey = false;
            InventoryUI.SetActive(true);
            inInventory = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (chest != null) //If Right MouseButton pressed while looking at a Chest
            {
                chest.OpenChest(this);
            }
        }
    }

    private void UpdateInventory()
    {
        for (int x = 0; x < Inventory.Count; x++)
        {
            if (Inventory[x].amount > 0 && Inventory[x].amount < 99)
                for (int i = x + 1; i < Inventory.Count; i++)
                {
                    if (Inventory[x].ID == Inventory[i].ID && Inventory[i].amount > 0)
                    {
                        if (Inventory[x].amount + Inventory[i].amount > 99)
                        {
                            Inventory[i].amount -= 99 - Inventory[x].amount;
                            Inventory[x].amount = 99;
                            break;
                        }

                        Inventory[x].amount += Inventory[i].amount;
                        Inventory[i].amount = 0;
                    }
                }
        }

        Inventory.RemoveAll(item => item.amount <= 0);
    }
}