using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Unity.Cinemachine;
using System;
using System.Drawing;
using static UnityEngine.EventSystems.EventTrigger;

public class GameManager : MonoBehaviour

{
    [Serializable]
    public class PlayerPointEntry
    {
        public PlayerColor playerColor;
        public int points;
    }
    public static GameManager Instance { get; private set; }

    [SerializeField] private List<PlayerPointEntry> playerPointEntries;
    private Dictionary<PlayerColor, int> playerScores = new();
    private Dictionary<PlayerColor, TMP_Text> scoreTexts = new();

    [SerializeField] private TMP_Text redScore, blueScore, greenScore, yellowScore;

    public ObjectPool pool;
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;

    [SerializeField] private AnimationController playerRedCon;
    [SerializeField] private AnimationController playerBlueCon;
    [SerializeField] private AnimationController playerGreenCon;
    [SerializeField] private AnimationController playerYellowCon;

    [SerializeField] private GameObject deathWallRed;
    [SerializeField] private GameObject deathWallBlue;
    [SerializeField] private GameObject deathWallGreen;
    [SerializeField] private GameObject deathWallYellow;

    private List<Coroutine> spawnerCoroutines = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        FillDictionary();
        
        scoreTexts[PlayerColor.Red] = redScore;
        scoreTexts[PlayerColor.Blue] = blueScore;
        scoreTexts[PlayerColor.Green] = greenScore;
        scoreTexts[PlayerColor.Yellow] = yellowScore;
        UpdateScoreText(PlayerColor.Blue);
        UpdateScoreText(PlayerColor.Green);
        UpdateScoreText(PlayerColor.Red);
        UpdateScoreText(PlayerColor.Yellow);
    }

    private void Start()
    {
        StartSpawning();
    }

    private void StartSpawning()
    {
        spawnerCoroutines.Add(StartCoroutine(SpawnerColor()));
        spawnerCoroutines.Add(StartCoroutine(SpawnerGold()));
    }

    public void RemovePoints(PlayerColor color, int points)
    {
        if (!playerScores.ContainsKey(color)) return;

        playerScores[color] -= points;
        UpdateScoreText(color);

        if (playerScores[color] > 0)
        {
            ActivateHurtAnim(color);
        }
        else
        {
            ActivateDieAnim(color);
            ActivateDeathWall(color);
        }
        
        CheckWinCondition();
    }
    private void UpdateScoreText(PlayerColor color)
    {
        scoreTexts[color].text = playerScores[color].ToString();
    }
    private void CheckWinCondition()
    {
        var remainingPlayers = playerScores.Where(p => p.Value > 0).ToList();
        if (remainingPlayers.Count == 1)
        {
            EndGame(remainingPlayers[0].Key);
        }
    }

    private void EndGame(PlayerColor winningPlayer)
    {
        StopSpawning();

        ActivateWinAnim(winningPlayer);
        ClearAllBalls();

        // Adjust the camera to focus on the winner
        AnimationController winnerController = GetController(winningPlayer);
        cinemachineCamera.Follow = winnerController?.transform.GetChild(0).GetChild(0);
        cinemachineCamera.LookAt = winnerController?.transform.GetChild(0).GetChild(0);
        cinemachineCamera.gameObject.SetActive(true);
    }

    private void StopSpawning()
    {
        foreach (Coroutine coroutine in spawnerCoroutines)
        {
            StopCoroutine(coroutine);
        }
    }

    private void ClearAllBalls()
    {
        foreach (GameObject ball in pool.GetAllActiveBalls())
        {
            ball.GetComponent<IPoolItem>().ReturnToPool();
        }
    }

    private AnimationController GetController(PlayerColor color)
    {
        return color switch
        {
            PlayerColor.Red => playerRedCon,
            PlayerColor.Blue => playerBlueCon,
            PlayerColor.Green => playerGreenCon,
            PlayerColor.Yellow => playerYellowCon,
            _ => null
        };
    }

    private void ActivateDeathWall(PlayerColor color)
    {
        switch (color)
        {
            case PlayerColor.Red:
                deathWallRed.SetActive(true);
                break;
            case PlayerColor.Blue:
                deathWallBlue.SetActive(true);
                break;
            case PlayerColor.Green:
                deathWallGreen.SetActive(true);
                break;
            case PlayerColor.Yellow:
                deathWallYellow.SetActive(true);
                break;
        }
    }

    private void FillDictionary()
    {
        playerScores.Clear();
        foreach (var entry in playerPointEntries)
        {
            playerScores[entry.playerColor] = entry.points;
            
        }
        
    }

    private IEnumerator SpawnerColor()
    {
        while (true)
        {
            pool.GetFromPool(ObjectPool.PoolType.Color);
            yield return new WaitForSecondsRealtime(4);
        }
    }

    private IEnumerator SpawnerGold()
    {
        while (true)
        {
            pool.GetFromPool(ObjectPool.PoolType.Gold);
            yield return new WaitForSecondsRealtime(11);
        }
    }

    private void ActivateHurtAnim(PlayerColor color) => GetController(color)?.HurtAnim();
    private void ActivateWinAnim(PlayerColor color) => GetController(color)?.WinAnim();
    private void ActivateDieAnim(PlayerColor color) => GetController(color)?.DieAnim();
}
