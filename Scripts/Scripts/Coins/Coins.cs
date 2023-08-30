using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Coins : MonoBehaviour
{
    [SerializeField] public List<Sprite> coins;
    [SerializeField] public GameObject[] coinsActive;
    [SerializeField] public Image[] coinsImage;
    public  int i = 0, index, coinSuffer, CoinS = 0;
    [SerializeField]  public Text txtCoins;
    [SerializeField] public GameObject txtCoinActive, coinsEnoug;
    public List<int> coinStorage;
   // int credits = FindAnyObjectByType<GameManager>().creditCoins;
    public void getCoinSuffer()
    {
        string coinsBtn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        coinsBtn.ToLower();
        txtCoinActive.SetActive(true);
        switch (coinsBtn)
        {
            case "1k":
                coinSuffer = 1;
                index = 0;
                break;
            case "5k":
                coinSuffer = 5;
                index = 1;
                break;
            case "10k":
                coinSuffer = 10;
                index = 2;
                break;
            case "20k":
                coinSuffer = 20;
                index = 3;
                break;
            case "50k":
                coinSuffer = 50;
                index = 4;
                break;
            case "100k":
                coinSuffer = 100;
                index = 5;
                break;
        }
        print("credits: " + FindAnyObjectByType<GameManager>().money);
        print("coinValue: " + FindAnyObjectByType<GameManager>().coinValueStore);
        print("coinSuffer: " + coinSuffer);
        if (i == coinsActive.Length - 1)
        {
            StartCoroutine(coiness());
            FindAnyObjectByType<GameManager>().MakeCoinAction(false, false, 0);
            coinsActive[i].SetActive(false);
        }

        if (FindAnyObjectByType<GameManager>().money < coinSuffer)
        {
            StartCoroutine(coiness());
            coinsActive[i].SetActive(false);
        }
        else if(FindAnyObjectByType<GameManager>().money >= coinSuffer && i < coinsActive.Length​​)
        {
            FindAnyObjectByType<GameManager>().money -= coinSuffer;
            
            FindAnyObjectByType<GameManager>().bets += coinSuffer;
            FindAnyObjectByType<GameManager>().coinDisplay += coinSuffer;
            txtCoins.text = FindAnyObjectByType<GameManager>().bets.ToString() + " k";
            print("i: " + i + "Le: " + coinsActive.Length);
            FindAnyObjectByType<GameManager>().undoCoins.enabled = true;
            FindAnyObjectByType<GameManager>().undoCoins.image.color = new Color(255, 255, 255, 255);
            FindAnyObjectByType<GameManager>().callGameStartBtn.enabled = true;
            FindAnyObjectByType<GameManager>().callGameStartBtn.image.color = new Color(255, 255, 255, 255);
            // CoinS += coinSuffer;
            coinsActive[i].SetActive(true);
            coinsImage[i].sprite = coins[index];
            coinStorage.Add(coinSuffer);
            i++;
        }
        print("credits ofter: " + FindAnyObjectByType<GameManager>().money);

    }
   public IEnumerator coiness()
    {
        coinsEnoug.SetActive(true);
        yield return new WaitForSeconds(2f);
        coinsEnoug.SetActive(false);
    }
}
