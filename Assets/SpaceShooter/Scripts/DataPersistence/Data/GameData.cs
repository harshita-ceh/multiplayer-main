using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData : IComparer<GameData.PlayerData>
{
    public List<PlayerData> leaderBoard;
    
    public GameData()
    {
         leaderBoard = new List<PlayerData>();
    }

    public int Compare(GameData.PlayerData a, GameData.PlayerData b)
    {
        return a.Score - b.Score;
    }

    [System.Serializable]
    public struct PlayerData
    {
        public PlayerData(string Name, int Score)
        {
            this.Name = Name;
            this.Score = Score;
        }

        public string Name;
        public int Score;
    }
}
