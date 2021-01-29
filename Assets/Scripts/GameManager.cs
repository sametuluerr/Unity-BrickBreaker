using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public BallManager initialBall;
    public BallManager ballprefab;
    public PaddleManager paddle;

    public static bool startGame;
    public static bool gameStarted = false;
    public static bool isDead = false;
    public static int gameScore = 0;
    public static int brokenBrickCount;
    public int startBrickCount;

    public Text endScreen;
    public Text brickCount;
    public Text scoreText;
    public Text ToastText;

    public Button PlayAgain;

    GameObject[] blueblocks;
    GameObject[] orangeblocks;
    GameObject[] greenblocks;
    GameObject[] walls;
    GameObject ball;
    GameObject gamePaddle;

    void Start()
    {
        InitializeBall();
        showToast(SceneManager.GetActiveScene().name, 3);

        orangeblocks = GameObject.FindGameObjectsWithTag("OrangeBlock");
        blueblocks = GameObject.FindGameObjectsWithTag("BlueBlock");
        greenblocks = GameObject.FindGameObjectsWithTag("GreenBlock");
        walls = GameObject.FindGameObjectsWithTag("Walls");
        ball = GameObject.FindGameObjectWithTag("Ball");
        gamePaddle = GameObject.FindGameObjectWithTag("Paddle");

        PlayAgain.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        brickCount.gameObject.SetActive(true);
        walls[0].SetActive(true);
        ball.SetActive(true);
        gamePaddle.SetActive(true);

        brokenBrickCount = 0;
        gameStarted = false;
        startGame = false;

        startBrickCount = blueblocks.Length + orangeblocks.Length + greenblocks.Length;
    }

    void Update()
    {
        PreStartGame();
        Winner();
        Loser();
        WriteBrickInfo();
    }

    // Oyun başlamadan topun konumunu ayarlayan ve oyunu başlatan fonksiyon
    void PreStartGame()
    {
        Vector3 paddlePos = paddle.gameObject.transform.position;
        Vector3 ballPos = new Vector3(paddlePos.x, paddlePos.y + .25f, 0);

        if (Input.GetKeyDown(KeyCode.Space) && gameStarted == false)
            startGame = true;

        if (!startGame)
            initialBall.transform.position = ballPos;
    }

    // Seviye kontrolünü yapan ve oyunu kazandınız ekranını gösteren fonksiyon
    void Winner()
    {
        blueblocks = GameObject.FindGameObjectsWithTag("BlueBlock");
        orangeblocks = GameObject.FindGameObjectsWithTag("OrangeBlock");
        greenblocks = GameObject.FindGameObjectsWithTag("GreenBlock");

        if ((blueblocks.Length + orangeblocks.Length + greenblocks.Length) == 0)
        {
            if (SceneManager.GetActiveScene().name.Equals("Level 1"))
                SceneManager.LoadScene("Level 2");
            else if (SceneManager.GetActiveScene().name.Equals("Level 2"))
                SceneManager.LoadScene("Level 3");
            else
            {
                endScreen.text = "Kazandınız tebrikler!\nSkorunuz: " + gameScore + "\nKırılan Tuğla Sayısı: 70";
                gamePaddle.SetActive(false);
                ball.SetActive(false);
                walls[0].SetActive(false);
                scoreText.gameObject.SetActive(false);
                brickCount.gameObject.SetActive(false);
                PlayAgain.gameObject.SetActive(true);
            }
        }
    }

    // Oyunu kaybettiniz ekranını gösteren fonksiyon
    void Loser()
    {
        if (isDead == true)
        {
            if (SceneManager.GetActiveScene().name.Equals("Level 2"))
                brokenBrickCount += 15;
            else if (SceneManager.GetActiveScene().name.Equals("Level 3"))
                brokenBrickCount += 40;
            showToast("", 1);
            endScreen.text = "Kaybettiniz!\nSkorunuz: " + gameScore + "\nKırılan Tuğla Sayısı: " + brokenBrickCount;
            PlayAgain.gameObject.SetActive(true);
            scoreText.gameObject.SetActive(false);
            brickCount.gameObject.SetActive(false);
            gamePaddle.SetActive(false);
            ball.SetActive(false);
            walls[0].SetActive(false);

            startGame = false;
            isDead = false;
            gameScore = 0;
        }
    }

    // Tuğla ve skor hakkındaki bilgileri ekrana yazdıran fonksiyon
    void WriteBrickInfo()
    {
        brickCount.text = "Hedef: " + brokenBrickCount + "/" + startBrickCount;
        scoreText.text = "Skor: " + gameScore;
    }

    // Başlangıçta topun oluşturulmasını sağlayan fonksiyon
    void InitializeBall()
    {
        Vector3 paddlePos = paddle.gameObject.transform.position;
        Vector3 ballPos = new Vector3(paddlePos.x, paddlePos.y + .25f, 0);
        initialBall = Instantiate(ballprefab, ballPos, Quaternion.identity);
    }

    // Oyunun ilk sahnesinin yüklenmesini sağlayan fonksiyon
    public void LoadScene()
    {
        gameStarted = false;
        brokenBrickCount = 0;
        SceneManager.LoadScene("Level 1");
    }

    // Ekrana gönderilen mesajı toast mesajı şeklinde yazan fonksiyon
    public void showToast(string text, int duration)
    {
        StartCoroutine(showToastCOR(text, duration));
    }

    // Ekrana gönderilen mesajı toast mesajı şeklinde yazan yazının ayarlarını yapan fonksiyon
    private IEnumerator showToastCOR(string text, int duration)
    {
        Color orginalColor = ToastText.color;

        ToastText.text = text;
        ToastText.enabled = true;

        //Fade in efekti
        yield return fadeInAndOut(ToastText, true, 0.5f);

        //süre kadar bekle
        float counter = 0;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        //Fade out efekti
        yield return fadeInAndOut(ToastText, false, 0.5f);

        ToastText.enabled = false;
        ToastText.color = orginalColor;
    }

    //  Ekrana gönderilen mesajı toast mesajı şeklinde yazan yazının ekranda kalma süresini ayarlayan fonksiyon
    IEnumerator fadeInAndOut(Text targetText, bool fadeIn, float duration)
    {
        //fadein mi değil mi?
        float a, b;

        if (fadeIn)
        {
            a = 0f;
            b = 1f;
        }
        else
        {
            a = 1f;
            b = 0f;
        }

        Color currentColor = Color.white;
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;

            float alpha = Mathf.Lerp(a, b, counter / duration);

            targetText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
    }
}