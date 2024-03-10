using UnityEngine;
using TMPro;

public class Upgrades : MonoBehaviour, Interactable
{
    public GameObject GLC;
    public GameObject TobyPre;
    public GameObject TobyPost;
    public GameObject ShipPre;
    public GameObject ShipPost;
    
    public PlayerController playerController;
    public GameObject upgradeBenchUI;
    public TextMeshProUGUI scrapCounterText;
    public TextMeshProUGUI starCounterText;
    public TextMeshProUGUI robotheadCounterText;
    public TextMeshProUGUI canonCounterText;
    public TextMeshProUGUI tychoniteCounterText;

    public GameObject speedBoostSlot;
    public GameObject fishBoostSlot;
    public GameObject warpBoostSlot;
    public GameObject reloadBoostSlot;
    public GameObject itemBoostSlot;
    public GameObject healthBoostSlot;

    [SerializeField] private int speedBoostCost = 10;
    [SerializeField] private int fishBoostCost = 20;
    [SerializeField] private int warpBoostCost = 30;
    [SerializeField] private int reloadBoostCost = 40;
    [SerializeField] private int itemBoostCost = 50;
    [SerializeField] private int healthBoostCost = 60;
    [SerializeField] private int GLCCost = 250;
    [SerializeField] private int GLCRobotHeadCost = 10;
    [SerializeField] private int GLCCanonCost = 10;
    [SerializeField] private int GLCStarCost = 30;
    [SerializeField] private int shipRepairCost = 1000;

    private void Start()
    {
        DeactivateUpgradeSlots();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeactivateUpgradeBench();
        }

        UpdateCounters();
    }

    private void UpdateCounters()
    {
        scrapCounterText.text = playerController.scrapCounter.ToString();
        starCounterText.text = playerController.starCounter.ToString();
        robotheadCounterText.text = playerController.RobotHeadCounter.ToString();
        canonCounterText.text = playerController.canonCounter.ToString();
        tychoniteCounterText.text = playerController.tychoCounter.ToString();
    }

    public void Interact()
    {
        ActivateUpgradeBench();
    }

    public void BuySpeedBoost()
    {
        BuyUpgrade("SpeedBoost", speedBoostCost, tychoniteCounterText, speedBoostSlot, ref playerController.speedBoost, ref playerController.tychoCounter);
    }

    public void BuyFishBoost()
    {
        BuyUpgrade("FishBoost", fishBoostCost, tychoniteCounterText, fishBoostSlot, ref playerController.fishBoost, ref playerController.tychoCounter);
    }

    public void BuyWarpBoost()
    {
        BuyUpgrade("WarpBoost", warpBoostCost, tychoniteCounterText, warpBoostSlot, ref playerController.warpBoost, ref playerController.tychoCounter);
    }

    public void BuyReloadBoost()
    {
        BuyUpgrade("ReloadBoost", reloadBoostCost, tychoniteCounterText, reloadBoostSlot, ref playerController.reloadBoost, ref playerController.tychoCounter);
    }

    public void BuyItemBoost()
    {
        BuyUpgrade("ItemBoost", itemBoostCost, tychoniteCounterText, itemBoostSlot, ref playerController.itemBoost, ref playerController.tychoCounter);
    }

    public void BuyHealthBoost()
    {
        BuyUpgrade("HealthBoost", healthBoostCost, tychoniteCounterText, healthBoostSlot, ref playerController.healthBoost, ref playerController.tychoCounter);
    }

    public void BuyGLC()
    {
        if (CanBuyGLC())
        {
            playerController.tychoCounter -= GLCCost;
            playerController.RobotHeadCounter -= GLCRobotHeadCost;
            playerController.canonCounter -= GLCCanonCost;
            playerController.starCounter -= GLCStarCost;

            GLC.SetActive(true);
        }
    }

    public void BuyShipRepair()
    {
        if (CanBuyShipRepair())
        {
            playerController.tychoCounter -= shipRepairCost;
            tychoniteCounterText.text = playerController.tychoCounter.ToString();

            TobyPre.SetActive(false);
            TobyPost.SetActive(true);
            ShipPre.SetActive(false);
            ShipPost.SetActive(true);
        }
    }

    private void ActivateUpgradeBench()
    {
        upgradeBenchUI.SetActive(true);
    }

    public void DeactivateUpgradeBench()
    {
        upgradeBenchUI.SetActive(false);
    }

    private void DeactivateUpgradeSlots()
    {
        speedBoostSlot.SetActive(false);
        fishBoostSlot.SetActive(false);
        warpBoostSlot.SetActive(false);
        reloadBoostSlot.SetActive(false);
        itemBoostSlot.SetActive(false);
        healthBoostSlot.SetActive(false);
    }

    private void BuyUpgrade(string upgradeName, int cost, TextMeshProUGUI counterText, GameObject upgradeSlot, ref bool playerBool, ref int playerCounter)
    {
        if (playerCounter >= cost && !playerBool)
        {
            playerCounter -= cost;
            counterText.text = playerCounter.ToString();
            upgradeSlot.SetActive(true);
            playerBool = true;
        }
    }

    private bool CanBuyGLC()
    {
        return playerController.tychoCounter >= GLCCost &&
               playerController.RobotHeadCounter >= GLCRobotHeadCost &&
               playerController.canonCounter >= GLCCanonCost &&
               playerController.starCounter >= GLCStarCost;
    }

    private bool CanBuyShipRepair()
    {
        return playerController.tychoCounter >= shipRepairCost;
    }
}
