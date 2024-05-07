using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    public float speed;
    public float sprintSpeed;
    public float mouseSensetivity;
    public float jumpForce;

    [Header("Enemy Sensing")]
    float sound;
    public float Sound
    {
        get => sound;
        set
        {
            sound = value;
            if (sound > 20) sound = 20;    
        }
    }

    [Header("UI Elements")]
    public GameObject CraftingUI;
    public GameObject InventoryUI;
    public GameObject ChestUI;
    public Inventory ChestInventory;
    public Inventory inv;
    public Slider healthBar;
    public Image healthbarFill;
    public Gradient healthGradient;
    public hotbar hotbar;
    public GameObject PauseMenu;

    [Header("Inventory")]
    public List<GameObjects> Inventory;

    [HideInInspector] public bool inInventory = false;
    bool hasReleasedKey = true;
    bool crafting = false;
    bool jumping = false;

    Transform cam;

    int InventoryLengthMemory;
    int hotbarSlot;

    HealthSystem health;

    Rigidbody rb;

    Ray ray;
    RaycastHit hit;
    Collider other = null;

    Chest chest;
    Item itemLookingAt;
    BreakableItem breakableItem;

    WorldSC world;
    private float sound1;

    void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
        rb = GetComponent<Rigidbody>();
        health = GetComponent<HealthSystem>();
        world = GameObject.Find("World").GetComponent<WorldSC>();
        healthBar.maxValue = health.maxHP;
        updateHealthBar();
    }

    private void FixedUpdate()
    {
        if (jumping)
        {
            Jump();
            jumping = false;
        }
    }

    void Update()
    {
        raycasting();
        if (!world.inUI)
            getPlayerInput();
        else
            getUIInputs();

        if (Input.GetAxisRaw("Action") == 0 && Input.GetAxisRaw("Inventory") == 0 && Input.GetAxisRaw("Jump") == 0 && Input.GetAxisRaw("esc") == 0)
            hasReleasedKey = true;

        if (Inventory.Count != InventoryLengthMemory)
            UpdateInventory();
        InventoryLengthMemory = Inventory.Count;

        DampenSound();
    }

    void raycasting()
    {
        other = null;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
            other = hit.collider;

        if (other != null)
        {
            if (other.gameObject.tag == "Chest")
                chest = other.gameObject.GetComponent<Chest>();

            if (other.gameObject.tag == "Pickupable")
            {
                itemLookingAt = other.gameObject.GetComponent<Item>();
            }

            if (other.gameObject.tag == "BreakableItem")
            {
                breakableItem = other.gameObject.GetComponent<BreakableItem>();
            }

            if (other.gameObject.tag == "Button")
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Button b = other.gameObject.GetComponent<Button>();
                    b.onClick.Invoke();
                }
            }
        }
        else
        {
            chest = null;
            itemLookingAt = null;
            breakableItem = null;
        }
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

        Vector3 movement = new Vector3(moveX, 0, moveZ) * Time.deltaTime;
        if (Input.GetAxisRaw("Sprint") > 0)
            transform.Translate(movement * sprintSpeed);
        else
            transform.Translate(movement * speed);

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

        if (Input.GetAxisRaw("esc") > 0 && hasReleasedKey)
        {
            hasReleasedKey = false;
            PauseMenu.SetActive(true);
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

        if (Input.GetMouseButtonUp(0))
        {
            if (itemLookingAt != null)
            {
                pickupItem(itemLookingAt.ID);
            }

            if (breakableItem != null)
            {
                print("Punching");
                breakableItem.punch((int)Random.Range(10, 15));
            }
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                hotbarSlot--;
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                hotbarSlot++;
            }

            if (hotbarSlot < 0)
                hotbarSlot = 8;
            if (hotbarSlot > 8)
                hotbarSlot = 0;

            hotbar.setHighlighted(hotbarSlot);
        }

        if (Input.GetAxisRaw("Jump") > 0 && hasReleasedKey && canJump)
        {
            jumping = true;
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        MakeSound(99);
    }

    bool canJump
    {
        get
        {
            if (rb.velocity.y == 0)
                return true;
            else
                return false;
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

    public void updateHealthBar()
    {
        healthBar.value = health.GetHealth();
        healthbarFill.color = hpColor(health.GetHealth() / health.maxHP);
    }

    Color hpColor(float value)
    {
        return healthGradient.Evaluate(value);
    }

    public void damage(int value)
    {
        health.ApplyDamage(value);
        updateHealthBar();
    }

    public void Heal(int value)
    {
        health.ApplyDamage(-value);
        updateHealthBar();
    }

    public void MakeSound(int magnitude)
    {
        Sound = magnitude;
    }

    void DampenSound()
    {
        if (Sound > 0)
        {
            Sound -= 2 * Time.deltaTime;
            print(Sound);
        }
    }

    public void pickupItem(byte ID)
    {
        GameObjects ItemToPickup = new()
        {
            amount = 1,
            ID = world.ItemsInGame[ID].ID,
            Name = world.ItemsInGame[ID].Name,
            prefab = world.ItemsInGame[ID].prefab,
            sprite = world.ItemsInGame[ID].sprite,
            recepie = world.ItemsInGame[ID].recepie,
            Craftable = world.ItemsInGame[ID].Craftable,
            buildingID = world.ItemsInGame[ID].buildingID,
            StackAmount = world.ItemsInGame[ID].StackAmount,
            isPlaceable = world.ItemsInGame[ID].isPlaceable,
            amountWhenCrafted = world.ItemsInGame[ID].amountWhenCrafted
        };

        Inventory.Add(ItemToPickup);

        itemLookingAt.pickup();
    }
}