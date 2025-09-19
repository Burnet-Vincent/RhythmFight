using TMPro;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(Instance.gameObject);
            }
            
            Instance = this;
        }

        [SerializeField] private TextMeshProUGUI scoreText;
        
        private int _score;

        private void Start()
        {
            DisplayScore();
        }

        private void DisplayScore()
        {
            scoreText.text = "Score : " + _score;
        }

        public void AddScore(int score)
        {
            _score += score;
            DisplayScore();
        }

        public void ResetScore()
        {
            _score = 0;
            DisplayScore();
        }
        
    }
}