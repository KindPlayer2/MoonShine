using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{

    public AudioClip ShipHorn;

    public GameObject GruntPrefab;
    public GameObject ScampPrefab;
    public GameObject RoverPrefab;
    public GameObject ShotgunnerPrefab;
    public GameObject BanditPrefab;
    public GameObject SheriffPrefab;
    public GameObject RobotPrefab;
    public GameObject TankPrefab;
    public GameObject GeneralPrefab;

    public PlayerController playa;
    public int counter;


    private int randomThingEasy;
    private int randomThingMedium;
    private int randomThingHard;

    public enum EnemyType
    {
        Grunt,
        Scamp,
        Rover,
        Shotgunner,
        Sheriff,
        Bandit,
        Robot,
        General,
        Tank
    }

    private enum MoonPhase
    {
        NewMoon,
        WaxingCrescent,
        FirstQuarter,
        WaxingGibbous,
        FullMoon,
        WaningGibbous,
        LastQuarter,
        WaningCrescent
    }

    private bool isWave = false;
    public Animator moonPhaseAnimator; 
    private int monthCounter = 1;

    private int phaseCounter = 0;


    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }



    private void Update()
    {
        if(!isWave && counter == 0 && !playa.isInside)
        {
            StartCoroutine(StartWave());

            Debug.Log("NEXT WAVE STARTING");
        }
        
        if(counter > 0)
        {
            isWave = false;
        }
        
    }



    IEnumerator StartWave()
    {
        
        isWave = true;

        //The Idea here is the wave UI recharges between waves 9 segments 20 seconds each is 3 minutes
        for(int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(2f); // 3 minutes wait between waves
            moonPhaseAnimator.SetTrigger("Reset");
        }
        

        // Update moon phase New wave has officially begun
        moonPhaseAnimator.SetTrigger("NextPhase");
        playa.waveCounter++;
        audioSource.PlayOneShot(ShipHorn, 0.1f);
        phaseCounter++;
        MoonPhase currentPhase = (MoonPhase)((phaseCounter) % 8);
        Debug.Log(currentPhase);

        // Determine spawn rates based on moon phase and month
        int easyCount = 0;
        int mediumCount = 0;
        int hardCount = 0;

        switch (currentPhase)
        {
            case MoonPhase.NewMoon:
                // Do nothing for this case, just wait
                break;

            case MoonPhase.WaxingCrescent:
            case MoonPhase.WaningCrescent:
                easyCount = 2;
                mediumCount = 1;
                break;

            case MoonPhase.FirstQuarter:
            case MoonPhase.LastQuarter:
                easyCount = 3;
                mediumCount = 2;
                break;

            case MoonPhase.WaxingGibbous:
            case MoonPhase.WaningGibbous:
                easyCount = 4;
                mediumCount = 3;
                hardCount = 1;
                break;

            case MoonPhase.FullMoon:
                easyCount = 5;
                mediumCount = 4;
                hardCount = 2;
                break;
        }

        // Multiply spawn rates by the month counter
        easyCount *= monthCounter;
        mediumCount *= monthCounter;
        hardCount *= monthCounter;

        // Spawn easy enemies
        for (int i = 0; i < easyCount; i++)
        {
            randomThingEasy = Random.Range(1, 4);
            switch (randomThingEasy)
            {
                   case 1:
                    // Spawn Grunt
                    SpawnEnemy(EnemyType.Grunt, 1);
                    break;
                case 2:
                    // Spawn Rover
                    SpawnEnemy(EnemyType.Rover, 1);
                    break;
                case 3:
                    // Spawn Scamp
                    SpawnEnemy(EnemyType.Scamp, 1);
                    break;
            }
        }

            //spawn medium enemies
        for (int i = 0; i < mediumCount; i++)
        {
            randomThingMedium = Random.Range(4, 7);
            switch (randomThingMedium)
            {
                case 4:
                    // Spawn Grunt
                    SpawnEnemy(EnemyType.Shotgunner, 1);
                    break;

                case 5:
                    // Spawn Grunt
                    SpawnEnemy(EnemyType.Bandit, 1);
                    break;

                case 6:
                    // Spawn Grunt
                    SpawnEnemy(EnemyType.Sheriff, 1);
                    break;
            }
        }

        //spawn hard enemies
        for (int i = 0; i < hardCount; i++)
        {
            randomThingHard = Random.Range(7, 10);
            switch (randomThingHard)
            {
                case 7:
                    // Spawn Grunt
                    SpawnEnemy(EnemyType.Robot, 1);
                    break;

                case 8:
                    // Spawn Grunt
                    SpawnEnemy(EnemyType.General, 1);
                    break;

                case 9:
                    // Spawn Grunt
                    SpawnEnemy(EnemyType.Tank, 1);
                    break;
            }
        }


        // Increase the month counter after a full loop of moon phases
        if (currentPhase == MoonPhase.NewMoon)
        {
                monthCounter++;

                StartCoroutine(StartWave());

                Debug.Log("NEXT WAVE STARTING");

        }
        
    }

    void SpawnEnemy(EnemyType enemyType, int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            // Determine a random spawn point within one of the rectangles
            Vector2 spawnPoint = Vector2.zero;

            switch (Random.Range(1, 5))
            {
                case 1:
                    spawnPoint = new Vector2(Random.Range(-170, -127), Random.Range(8, 13));
                    break;

                case 2:
                    spawnPoint = new Vector2(Random.Range(-170, -163), Random.Range(-15, 13));
                    break;

                case 3:
                    spawnPoint = new Vector2(Random.Range(-170, -124), Random.Range(-15, -11));
                    break;

                case 4:
                    spawnPoint = new Vector2(Random.Range(-135, -127), Random.Range(-15, 11));
                    break;
            }

            // Instantiate the enemy at the determined spawn point
            GameObject enemy = InstantiateEnemy(enemyType, spawnPoint);
            // Add the spawned enemy to the list
            counter++;
            isWave = false;
            
        }

        
        
    
    }

    GameObject InstantiateEnemy(EnemyType enemyType, Vector2 spawnPoint)
    {
        GameObject enemyPrefab = null;

        // Choose the prefab based on the enemy type
        switch (enemyType)
        {
            case EnemyType.Grunt:
                enemyPrefab = GruntPrefab;
                break;

            case EnemyType.Scamp:
                enemyPrefab = ScampPrefab;
                break;

            case EnemyType.Rover:
                enemyPrefab = RoverPrefab;
                break;

            case EnemyType.Shotgunner:
                enemyPrefab = ShotgunnerPrefab;
                break;

            case EnemyType.Sheriff:
                enemyPrefab = SheriffPrefab;
                break;

            case EnemyType.Bandit:
                enemyPrefab = BanditPrefab;
                break;

            case EnemyType.Robot:
                enemyPrefab = RobotPrefab;
                break;

            case EnemyType.General:
                enemyPrefab = GeneralPrefab;
                break;

            case EnemyType.Tank:
                enemyPrefab = TankPrefab;
                break;

            // Add cases for other enemy types...

            default:
                // Default case, in case a new enemy type is added
                Debug.LogError("Unknown enemy type: " + enemyType);
                return null;
        }

        // Instantiate the enemy prefab at the specified spawn point
        if (enemyPrefab != null)
        {
            return Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        }

        return null;
    }

}
