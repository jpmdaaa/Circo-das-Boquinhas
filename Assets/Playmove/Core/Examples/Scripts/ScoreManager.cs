using System.Collections.Generic;
using System.Linq;
using Playmove.Metrics.API;
using Playmove.Metrics.API.Models;
using UnityEngine;

public class Student
{
    public string Name;
    public int Score;

    public Student()
    {
        Name = "";
        Score = 0;
    }

    public Student(string name, int score)
    {
        Name = name;
        Score = score;
    }
}

namespace Playmove.Metrics.API.Models
{
    public partial class Score
    {
        public Dictionary<int, Student> ScoresByGameMode = new();

        public bool ContainsScoresInGameMode(int gameMode)
        {
            return ScoresByGameMode.ContainsKey(gameMode);
        }

        public Student GetStudentScoreByGameMode(int gameMode)
        {
            if (!ScoresByGameMode.ContainsKey(gameMode))
                return null;
            return ScoresByGameMode[gameMode];
        }

        public void SetScore(Student student, int gameMode)
        {
            if (!ScoresByGameMode.ContainsKey(gameMode)) ScoresByGameMode.Add(gameMode, student);

            if (student.Score > ScoresByGameMode[gameMode].Score) ScoresByGameMode[gameMode].Score = student.Score;

            GlobalScore = ScoresByGameMode.Sum(item => item.Value.Score);
        }
    }

    // Utiliza a base da classe ranking e adiciona itens específicos
    public partial class Ranking
    {
        public List<Student> GetStudentsByGameMode(int gameMode)
        {
            var students = ScoreInfo.Where(item => item.Value.ContainsScoresInGameMode(gameMode))
                .Select(item => item.Value.GetStudentScoreByGameMode(gameMode)).ToList();
            students = students.OrderByDescending(item => item.Score).ToList();
            return students;
        }

        public Student GetStudentByGameMode(int gameMode, string playerGUID)
        {
            if (!ScoreInfo.ContainsKey(playerGUID) && ScoreInfo[playerGUID].ContainsScoresInGameMode(gameMode))
                return ScoreInfo[playerGUID].GetStudentScoreByGameMode(gameMode);
            return null;
        }

        public void SetScore(string playerGUID, Student studen, int gameMode)
        {
            if (!ScoreInfo.ContainsKey(playerGUID))
                ScoreInfo.Add(playerGUID, new Score());
            // ---
            ScoreInfo[playerGUID].SetScore(studen, gameMode);
        }
    }
}

namespace Playmove.Core.Examples
{
    public class ScoreManager : MonoBehaviour
    {
        public static int MaxScoresNumber = 50;
        public static int DifficultyLevels = 3;
        public static Ranking ranking;
        private static List<Student>[] students;

        public static int Difficulty { get; set; }

        public static int Score { get; set; }

        // Inicializa a classe
        public static void Initialize()
        {
            if (ranking == null)
                ranking = new Ranking();
            if (students != null)
                return;

            students = new List<Student>[DifficultyLevels];
            for (var i = 0; i < students.Length; i++) students[i] = new List<Student>();

            Difficulty = 0;
            Score = 0;
            Load(null);
        }

        // Carrega os dados, se esxistirem, do banco de dados;
        public static void Load(AsyncCallback<bool> completed)
        {
            MetricsAPI.Score.GetRanking(result =>
            {
                ranking = result.Data;
                completed?.Invoke(new AsyncResult<bool>(true, string.Empty));
            });
        }

        public static void Save()
        {
            MetricsAPI.Score.SaveRanking(ranking, null);
        }

        public static void DeleteAll()
        {
            MetricsAPI.Score.DeleteRanking(ranking,
                result => { Debug.Log("Ranking deletado: " + result.Data); }
            );
        }

        public static void RegisterStudent(string playerGUID, Student student, int difficulty)
        {
            ranking.SetScore(playerGUID, student, difficulty);
        }
    }
}