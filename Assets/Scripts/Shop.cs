using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour, Interactable
{
    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private PlayerController playerController;

    [Header("Item Counters")]
    [SerializeField] private TMP_Text starCounterText;
    [SerializeField] private TMP_Text scrapCounterText;
    [SerializeField] private TMP_Text RobotHeadCounterText;
    [SerializeField] private TMP_Text canonCounterText;
    [SerializeField] private TMP_Text fishCounterText;

    [Header("Tycho Items")]
    [SerializeField] private TMP_Text tychoStarCounterText;
    [SerializeField] private TMP_Text tychoScrapCounterText;
    [SerializeField] private TMP_Text tychoRobotHeadCounterText;
    [SerializeField] private TMP_Text tychoCanonCounterText;
    [SerializeField] private TMP_Text tychoFishCounterText;

    [Header("Tycho Counters")]
    private int tychoStarCounter;
    private int tychoScrapCounter;
    private int tychoRobotHeadCounter;
    private int tychoCanonCounter;
    private int tychoFishCounter;

    [SerializeField] private TMP_Text totalTychoCounterText;

    private int totalTychoCounter;

    private void Start()
    {
        // Set the shop canvas inactive at the start
        shopCanvas.SetActive(false);

        // Initialize counters on the UI
        UpdateItemCounters();
        UpdateTychoCounters();
    }

    private void Update()
    {
        starCounterText.text = playerController.starCounter.ToString();
        scrapCounterText.text = playerController.scrapCounter.ToString();
        RobotHeadCounterText.text = playerController.RobotHeadCounter.ToString();
        canonCounterText.text = playerController.canonCounter.ToString();
        fishCounterText.text = playerController.fishCounter.ToString();
    }

    public void Interact()
    {
        // Set the shop canvas active
        shopCanvas.SetActive(true);
    }

    public void CloseShop()
    {
        // Set the shop canvas inactive
        shopCanvas.SetActive(false);
    }

    public void SellItems()
    {
        // Increase player's tychoCounter by the total tycho counter value
        playerController.tychoCounter += totalTychoCounter;

        // Reset individual tycho counters to 0
        tychoStarCounter = 0;
        tychoScrapCounter = 0;
        tychoRobotHeadCounter = 0;
        tychoCanonCounter = 0;
        tychoFishCounter = 0;

        // Reset totalTychoCounter to 0
        totalTychoCounter = 0;

        // Close the shop after selling items
        CloseShop();
    }

    #region Item Buttons

    public void IncreaseStarCounter()
    {
        // Decrease starCounter and increase tychoStarCounter if starCounter is greater than 0
        if (playerController.starCounter > 0)
        {
            playerController.starCounter--;
            for(int i = 0; i < 5; i++)
            {
                tychoStarCounter++;
                totalTychoCounter++;
            }
            
        }

        // Update UI counters
        UpdateItemCounters();
        UpdateTychoCounters();
    }

    public void IncreaseScrapCounter()
    {
        // Decrease scrapCounter and increase tychoScrapCounter if scrapCounter is greater than 0
        if (playerController.scrapCounter > 0)
        {
            playerController.scrapCounter--;
            for(int i = 0; i < 3; i++)
            {
                tychoScrapCounter++;
                totalTychoCounter++;
            }
        }

        // Update UI counters
        UpdateItemCounters();
        UpdateTychoCounters();
    }

    public void IncreaseRobotHeadCounter()
    {
        // Decrease RobotHeadCounter and increase tychoRobotHeadCounter if RobotHeadCounter is greater than 0
        if (playerController.RobotHeadCounter > 0)
        {
            playerController.RobotHeadCounter--;
            for(int i = 0; i < 20; i++)
            {
                tychoRobotHeadCounter++;
                totalTychoCounter++;
            }
        }

        // Update UI counters
        UpdateItemCounters();
        UpdateTychoCounters();
    }

    public void IncreaseCanonCounter()
    {
        // Decrease canonCounter and increase tychoCanonCounter if canonCounter is greater than 0
        if (playerController.canonCounter > 0)
        {
            playerController.canonCounter--;
            for(int i = 0; i < 20; i++)
            {
                tychoCanonCounter++;
                totalTychoCounter++;
            }
        }

        // Update UI counters
        UpdateItemCounters();
        UpdateTychoCounters();
    }

    public void IncreaseFishCounter()
    {
        // Decrease fishCounter and increase tychoFishCounter if fishCounter is greater than 0
        if (playerController.fishCounter > 0)
        {
            playerController.fishCounter--;
            tychoFishCounter++;
            totalTychoCounter++;
        }

        // Update UI counters
        UpdateItemCounters();
        UpdateTychoCounters();
    }

    public void DecreaseStarCounter()
    {
        // Increase starCounter and decrease tychoStarCounter if tychoStarCounter is greater than 0
        if (tychoStarCounter > 0)
        {
            playerController.starCounter++;
            for(int i = 0; i < 5; i++)
            {
                tychoStarCounter--;
                totalTychoCounter--;
            }
        }

        // Update UI counters
        UpdateItemCounters();
        UpdateTychoCounters();
    }

    public void DecreaseScrapCounter()
    {
        // Increase scrapCounter and decrease tychoScrapCounter if tychoScrapCounter is greater than 0
        if (tychoScrapCounter > 0)
        {
            playerController.scrapCounter++;
            for(int i = 0; i < 3; i++)
            {
                tychoScrapCounter--;
                totalTychoCounter--;
            }
        }

        // Update UI counters
        UpdateItemCounters();
        UpdateTychoCounters();
    }

    public void DecreaseRobotHeadCounter()
    {
        // Increase RobotHeadCounter and decrease tychoRobotHeadCounter if tychoRobotHeadCounter is greater than 0
        if (tychoRobotHeadCounter > 0)
        {
            playerController.RobotHeadCounter++;
            for(int i = 0; i < 20; i++)
            {
                tychoRobotHeadCounter--;
                totalTychoCounter--;
            }
        }

        // Update UI counters
        UpdateItemCounters();
        UpdateTychoCounters();
    }

    public void DecreaseCanonCounter()
    {
        // Increase canonCounter and decrease tychoCanonCounter if tychoCanonCounter is greater than 0
        if (tychoCanonCounter > 0)
        {
            playerController.canonCounter++;
            for(int i = 0; i < 20; i++)
            {
                tychoCanonCounter--;
                totalTychoCounter--;
            }
        }

        // Update UI counters
        UpdateItemCounters();
        UpdateTychoCounters();
    }

    public void DecreaseFishCounter()
    {
        // Increase fishCounter and decrease tychoFishCounter if tychoFishCounter is greater than 0
        if (tychoFishCounter > 0)
        {
            playerController.fishCounter++;
            tychoFishCounter--;
            totalTychoCounter--;
        }

        // Update UI counters
        UpdateItemCounters();
        UpdateTychoCounters();
    }

    #endregion

    private void UpdateItemCounters()
    {
        // Update UI counters for items
        starCounterText.text = playerController.starCounter.ToString();
        scrapCounterText.text = playerController.scrapCounter.ToString();
        RobotHeadCounterText.text = playerController.RobotHeadCounter.ToString();
        canonCounterText.text = playerController.canonCounter.ToString();
        fishCounterText.text = playerController.fishCounter.ToString();
    }

    private void UpdateTychoCounters()
    {
        // Update UI counters for individual tycho items
        tychoStarCounterText.text = tychoStarCounter.ToString();
        tychoScrapCounterText.text = tychoScrapCounter.ToString();
        tychoRobotHeadCounterText.text = tychoRobotHeadCounter.ToString();
        tychoCanonCounterText.text = tychoCanonCounter.ToString();
        tychoFishCounterText.text = tychoFishCounter.ToString();

        // Update totalTychoCounterText
        totalTychoCounterText.text = totalTychoCounter.ToString();
    }
}
