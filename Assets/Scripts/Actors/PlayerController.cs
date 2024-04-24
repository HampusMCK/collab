using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    public float speed;
    public float sprintSpeed;
    public float mouseSensetivity;

    [Header("UI Elements")]
    public GameObject CraftingUI;
    public GameObject InventoryUI;
    public GameObject ChestUI;
    public Inventory ChestInventory;
    public Inventory inv;

    [Header("Inventory")]
    public List<GameObjects> Inventory;

    [HideInInspector] public bool inInventory = false;
    bool hasReleasedKey = true;
    bool crafting = false;

    Transform cam;

    int InventoryLengthMemory;

    HealthSystem health;

    Rigidbody rb;

    Ray ray;
    RaycastHit hit;
    Collider other = null;

    Chest chest;

    WorldSC world;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
        rb = GetComponent<Rigidbody>();
        health = GetComponent<HealthSystem>();
        world = GameObject.Find("World").GetComponent<WorldSC>();
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
        if (!world.inUI)
            getPlayerInput();
        else
            getUIInputs();

        if (Input.GetAxisRaw("Action") == 0 && Input.GetAxisRaw("Inventory") == 0)
            hasReleasedKey = true;

        if (Inventory.Count != InventoryLengthMemory)
            UpdateInventory();
        InventoryLengthMemory = Inventory.Count;
    }

    private void getUIInputs()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        getInventory();

        if (Input.GetAxisRaw("Inventory") > 0 && hasReleasedKey && !CraftingUI.activeSelf)
        {
            hasReleasedKey = false;
            closeInventory();
            world.inUI = false;
        }

        if (Input.GetAxisRaw("Action") > 0 && hasReleasedKey && CraftingUI.activeSelf)
        {
            hasReleasedKey = false;
            CraftingUI.SetActive(false);
            closeInventory();
            world.inUI = false;
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
            InventoryUI.SetActive(true);
            openInventory();
            world.inUI = true;
        }

        if (Input.GetAxisRaw("Inventory") > 0 && hasReleasedKey) // Open Inventory UI
        {
            hasReleasedKey = false;
            InventoryUI.SetActive(true);
            openInventory();
            world.inUI = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (chest != null) //If Right MouseButton pressed while looking at a Chest
            {
                ChestUI.SetActive(true);
                InventoryUI.SetActive(true);
                openInventory();
                world.inUI = true;
                chest.OpenChest(this);
            }
        }
    }

    private void UpdateInventory()
    {
        for (int x = 0; x < Inventory.Count; x++)
        {
            if (Inventory[x].amount > 0 && Inventory[x].amount < 99)
            {
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
        }
        Inventory.RemoveAll(item => item.amount <= 0);
    }

    private void getInventory()
    {
        if (!crafting)
        {
            Inventory.Clear();
            foreach (UIItemSlot slot in inv.slots)
            {
                if (slot.HasItem)
                {
                    Inventory.Add(slot.Item);
                }
            }
        }
    }

    private void openInventory()
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (!inv.slots[i].HasItem)
                inv.slots[i].Item = Inventory[i];
        }
    }

    public void closeInventory()
    {
        ItemMoving im = GameObject.Find("UI").GetComponent<ItemMoving>();
        if (im.cursorSlot.HasItem)
        {
            im.ReturnStack();
            getInventory();
        }
        foreach (UIItemSlot slot in inv.slots)
        {
            if (slot.HasItem)
            {
                slot.Item = null;
            }
        }
        InventoryUI.SetActive(false);
    }

    public void AddCraftedItem(GameObjects _item)
    {
        crafting = true;
        Inventory.Add(_item);
        foreach (UIItemSlot slot in inv.slots)
        {
            if (!slot.HasItem)
            {
                slot.Item = _item;
                break;
            }
        }
        crafting = false;
    }
}