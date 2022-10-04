using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets
{
    public class ScoreScreen : MonoBehaviour
    {   
        public TextMeshProUGUI totalScoreTextObject;
        public TextMeshProUGUI curScoreTextObject;
        public TextMeshProUGUI killTextObject;
        public TextMeshProUGUI damageTextObject;
        public TextMeshProUGUI waveTextObject;




        public void SetText(int totalScore, int currrentScore, int kill, int damage, int wave)
        {
            totalScoreTextObject.text = totalScore.ToString();
            curScoreTextObject.text = currrentScore.ToString();
            killTextObject.text = kill.ToString();
            damageTextObject.text = damage.ToString();
            waveTextObject.text = wave.ToString();
        }
    }
}