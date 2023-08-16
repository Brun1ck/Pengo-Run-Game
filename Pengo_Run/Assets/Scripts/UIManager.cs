using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	
	public GameObject gameOverPanel;
    public Text scoreText;
    public Image image;
    public Text coinText;

    public void UpdateCoins(int coin)
    {
        coinText.text = coin.ToString();
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score : " + score + "m";
    }
}
