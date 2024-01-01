using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using TMPro;


public class Level : MonoBehaviour
{

    public GameObject GameOverPanel;
    public Button TryAgainButton;

    public GameObject UpgradePanel;

    public Button UpgradeToFrostButton;
    public Button UpgradeToFireButton;


    public GameObject scoreText;
    public GameObject livesText;

    public static Level instance;
    public Transform[] paths;
    public Transform start;
    public Transform end;

    public int Coins = 100;
    public int Lives = 10;

    public bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + Coins;
    }

    public void RemoveCoins(int amount)
    {
        Coins -= amount;
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + Coins;
    }

    public void RemoveLife()
    {
        Lives--;
        livesText.GetComponent<TextMeshProUGUI>().text = "Lives: " + Lives;
        if (Lives <= 0)
        {
            isPaused = true;
            RemoveEverything();
            GameOverPanel.SetActive(true);
        }
    }

    private void RemoveEverything()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        //Destroy all towers and bullets
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject tower in towers)
        {
            Destroy(tower);
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
        ShowHideUpgradePanel(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !TowerPick.instance.picked)
        { // Left mouse button clicked
            LayerMask layerMask = LayerMask.GetMask("Tower");
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layerMask);
            if (hit.collider != null)
            {
                Debug.Log("hit tower");
                hit.collider.gameObject.GetComponent<Tower>().OnMouseDown();
            }
        }
    }


    void Awake()
    {
        instance = this;
        isPaused = false;
        GameOverPanel.SetActive(false);
        UpgradePanel.SetActive(false);
        TryAgainButton.onClick.AddListener(Retry);
        UpgradeToFrostButton.onClick.AddListener(UpgradeToFrost);
        UpgradeToFireButton.onClick.AddListener(UpgradeToFire);

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Retry()
    {
        Debug.Log("Retry");
        isPaused = false;
        Spawner.instance.Restart();
        GameOverPanel.SetActive(false);
    }

    public void ShowHideUpgradePanel(bool isSelected)
    {
        Debug.Log("ShowHideUpgradePanel: " + isSelected);
        UpgradePanel.SetActive(isSelected);
        if (isSelected)
        {
            if (Coins >= 500)
            {
                UpgradeToFrostButton.interactable = true;
            }
            else
            {
                UpgradeToFrostButton.interactable = false;
            }
            if (Coins >= 1000)
            {
                UpgradeToFireButton.interactable = true;
            }
            else
            {
                UpgradeToFireButton.interactable = false;
            }
        }
    }
    private void UpgradeToFrost()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject tower in towers)
        {
            if (tower.GetComponent<Tower>().IsSelected())
            {
                tower.GetComponent<Tower>().SetTowerType(1);
                return;
            }
        }
        DeselectAllTowers();
        ShowHideUpgradePanel(false);
        RemoveCoins(500);

    }

    private void UpgradeToFire()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject tower in towers)
        {
            if (tower.GetComponent<Tower>().IsSelected())
            {
                tower.GetComponent<Tower>().SetTowerType(2);
                return;
            }
        }
        DeselectAllTowers();
        ShowHideUpgradePanel(false);
        RemoveCoins(1000);
    }

    public void DeselectAllTowers()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject tower in towers)
        {
            tower.GetComponent<Tower>().DeSelect();
        }
    }

}
