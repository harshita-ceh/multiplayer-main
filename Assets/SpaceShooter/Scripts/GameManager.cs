using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public sealed class GameManager : MonoBehaviour, iDataPersistence
    {
        private Player player;
        private Invaders invaders;
        private MysteryShip mysteryShip;
        private Bunker[] bunkers;
        private List<GameData.PlayerData> leaderBoard;

        public GameObject gameOverUI;
        public Text scoreText;
        public Text livesText;
        public List<Text> leaderBoardTexts;
        

        public string MainGameLevel;

        public int score { get; private set; }
        public int lives { get; private set; }

        private void Awake()
        {
            player = FindObjectOfType<Player>();
            invaders = FindObjectOfType<Invaders>();
            mysteryShip = FindObjectOfType<MysteryShip>();
            bunkers = FindObjectsOfType<Bunker>();
        }

        public void LoadData(GameData data)
        {
            this.leaderBoard = data.leaderBoard;
        }

        public void SaveData(ref GameData data)
        {
            data.leaderBoard = this.leaderBoard;
        }
        private void Start()
        {
            player.killed += OnPlayerKilled;
            mysteryShip.killed += OnMysteryShipKilled;
            invaders.killed += OnInvaderKilled;

            NewGame();
        }

        private void Update()
        {
            if (lives <= 0 && Input.GetKeyDown(KeyCode.Return))
            {
                NewGame();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                NewGame();
                UpdateLevel();
            }
        }

        private void UpdateLevel()
        {
            LevelManager.Intance.MainScene.Invoke();
        }

        private void NewGame()
        {
            gameOverUI.SetActive(false);

            SetScore(0);
            SetLives(1);
            NewRound();
        }

        private void NewRound()
        {
            invaders.ResetInvaders();
            invaders.gameObject.SetActive(true);

            for (int i = 0; i < bunkers.Length; i++)
            {
                bunkers[i].ResetBunker();
            }

            Respawn();
        }

        private void Respawn()
        {
            Vector3 position = player.transform.position;
            position.x = 0f;
            player.transform.position = position;
            player.gameObject.SetActive(true);
        }

        private void GameOver()
        {
            if (leaderBoard.Count < 3)
            {
                leaderBoard.Add(new GameData.PlayerData("", score));
            }
            else
            {
                leaderBoard.Reverse();
                if (score > leaderBoard[0].Score)
                {
                    leaderBoard.RemoveAt(0);
                    leaderBoard.Add(new GameData.PlayerData("", score));
                }
            }
            leaderBoard.Sort(new GameData());
            leaderBoard.Reverse();

            int i = 0;
            foreach (GameData.PlayerData np in leaderBoard)
            {
                leaderBoardTexts[i].text = (i + 1).ToString() + ". " + np.Score.ToString();
                i++;
            }



            gameOverUI.SetActive(true);
            invaders.gameObject.SetActive(false);
            DataPersistenceManager.instance.SaveGame();
        }

        private void SetScore(int score)
        {
            this.score = score;
            scoreText.text = score.ToString().PadLeft(4, '0');
        }

        private void SetLives(int lives)
        {
            this.lives = Mathf.Max(lives, 0);
            livesText.text = lives.ToString();
        }

        private void OnPlayerKilled()
        {
            SetLives(lives - 1);

            player.gameObject.SetActive(false);

            if (lives > 0)
            {
                Invoke(nameof(NewRound), 1f);
            }
            else
            {
                GameOver();
            }
        }

        private void OnInvaderKilled(Invader invader)
        {
            SetScore(score + invader.score);

            if (invaders.AmountKilled == invaders.TotalAmount)
            {
                NewRound();
            }
        }

        private void OnMysteryShipKilled(MysteryShip mysteryShip)
        {
            SetScore(score + mysteryShip.score);
        }

    }
}