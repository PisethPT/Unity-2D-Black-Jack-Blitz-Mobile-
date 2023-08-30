using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using System;
public class GameManager : MonoBehaviour
{
    [SerializeField] private List<CardsSprite> cardSprites = new List<CardsSprite>();
    private List<int> CardsCountValues = new List<int>();
    private int[] Pintock = new int[8];
    private int[] numberOfCardsPlay = new int[52];
    int coinBack = 1;
    public int coinDisplay;
    public int money = 1000, bets = 0, youWin, coinValueStore;
    private int playertotalScore, machintotalScore;
    int isCoins;
    int cardsValues;
    float ClockLoading;
    public float Clock = 0.5f;
    private bool TimeON = true;
    bool action;
    private bool IsHitBtnOn, dealOnClick, IsStandBtnOn, IsCallAgainOn;
    public Text textTimer;
    public Text MoneyStorage;
    public Text textMinute, textSecond;
    public Text textplayerScoreShow, textmachineScoreShow;
  //  public Button ClickOn;
    [SerializeField] public Button hitbtnClick, machinebtnClick, standbtn, callcardsAgain;
    public Button callGameStartBtn, undoCoins, callMuiltCards;
    [SerializeField] public Button[] coinNormal;
    public GameObject showPane;
    [SerializeField] public GameObject hitObject, callcardAgainOject;
    [SerializeField] public GameObject[] coinModel;
    [SerializeField] public GameObject callStartGameEnable, undoBtnEnable;
    public GameObject equalPlayer, playerwinner, machinewinner;
    public GameObject[] playerCardsObject;
    public GameObject[] machineCardsObject;
    public Animator cardAnimate;
    public Animator[] coinAinmate;
    public Animator equalAnimate, playerWinAnimate, playerLoseAnimate;
    public Image[] CardsForPlayer;
    public Image[] CardForMachinePlay;
    public Sprite backCard;
    // public List<Cashs> GroupCash = new List<Cashs>();

    // TODO: Start is called before the first frame update
    void Start()
    {
        textMinute.text = textSecond.text = "00";
        FindAnyObjectByType<AudioManage>().StartPlayMusic("background");
        StartCoroutine(cardsOfGameStant());
        hitbtnClick.enabled = false;
        hitbtnClick.image.color = Color.gray;
        machinebtnClick.enabled = false;
        machinebtnClick.image.color = Color.gray;
        standbtn.enabled = false;
        standbtn.image.color = Color.gray;
        undoCoins.enabled = false;
        undoCoins.image.color = Color.gray;
        callGameStartBtn.enabled = false;
        callGameStartBtn.image.color = Color.gray;
    }

    // TODO: Update is called once per frame
    void Update()
    {
        GameStartRunning();
        MoneyStorage.text = money.ToString() + " k";

        // timer
        ClockLoading = Time.time;
        int minutes = (int)ClockLoading / 60;
        int seconds = (int)ClockLoading % 60;

        textMinute.text = minutes.ToString();
        textSecond.text = seconds.ToString();

    }

    // TODO: Wait chard
    IEnumerator cardsOfGameStant()
    {
        showPane.SetActive(true);
        cardAnimate.Play("bannerShow");
        yield return new WaitForSeconds(1f);
        playerCardsObject[0].SetActive(false);
        playerCardsObject[1].SetActive(false);
        playerCardsObject[2].SetActive(false);
        playerCardsObject[3].SetActive(false);

        machineCardsObject[0].SetActive(false);
        machineCardsObject[1].SetActive(false);
        machineCardsObject[2].SetActive(false);
        machineCardsObject[3].SetActive(false);
        showPane.SetActive(false);
    }

    // TODO: Random cards
    void GameStartProcessing()
    {
        for (int i = 0; i < 8; i++)
        {
            int Rand = Random.Range(0, 52);
            while (CardsCountValues.Contains(Rand))
            {
                Rand = Random.Range(0, 52);
            }
            CardsCountValues.Add(Rand);
            print("////////// index of cards: " + CardsCountValues[i] + ", Card Name: " + cardSprites[Rand].CardsName);
        }

    }

    // TODO: clubs (♣), diamonds (♦), hearts (♥) and spades (♠) 
    // hit card 1 by 1 card
    int GameCompareSpriteCards(int fistIndex, int lastIndex, int getSpriteIndex, Image[] getImage, GameObject[] gameObject, List<int> listOfCards)
    {
        int numbersOfCards = 0;
        for (int item1 = fistIndex; item1 < lastIndex; item1++)
        {
            for (int item2 = 0; item2 < cardSprites.Count; item2++)
            {
                if (listOfCards[item1] == item2)
                {
                    listOfCards[item1] %= 13;
                    cardsValues = numbersOfCards = listOfCards[item1];
                    if (listOfCards[item1] > 10)
                    {
                        numbersOfCards = 10;
                        numberOfCardsPlay[item2] = numbersOfCards;
                    }
                    else if (listOfCards[item1] == 0)
                    {
                        numbersOfCards = 1;
                        numberOfCardsPlay[item2] = numbersOfCards;
                    }
                    else if (listOfCards[item1] == 10)
                        numberOfCardsPlay[item2] = numbersOfCards;
                    else
                        numberOfCardsPlay[item2] = numbersOfCards++;
                    numberOfCardsPlay[item2] = numbersOfCards++;
                    Pintock[fistIndex] = numberOfCardsPlay[item2];
                    gameObject[getSpriteIndex].SetActive(true);
                    getImage[getSpriteIndex].sprite = cardSprites[item2].Cards;
                    print("////////// index of cards: " + numberOfCardsPlay[item2] + ", Card Name: " + cardSprites[item2].CardsName);
                }
            }
        }
        // make for check value of card spites 
        return cardsValues;
    }

    // TODO: Start play
    void GameStartRunning()
    {
        if (TimeON)
        {
            Clock -= Time.deltaTime;
            textTimer.text = Mathf.Round(Clock).ToString();
        }
        if (Clock <= 0 && TimeON)
        {
            TimeON = false;
            CardsCountValues.Clear();
            getBetsCoins();
        }
    }

    // TODO: Game play again
    IEnumerator GameProccesContinue()
    {
        coinAinmate[0].Play("1ks");
        coinAinmate[1].Play("5ks");
        coinAinmate[2].Play("10ks");
        coinAinmate[3].Play("20ks");
        coinAinmate[4].Play("50ks");
        coinAinmate[5].Play("100ks");

        CardsCountValues.Clear();
        GameStartProcessing();

        yield return new WaitForSeconds(1f);
        cardAnimate.Play("playerFirstCard");
        yield return new WaitForSeconds(1f);
        cardAnimate.Play("player1");
        FindAnyObjectByType<AudioManage>().StartPlaySFX("card");
        GameCompareSpriteCards(0, 1, 0, CardsForPlayer, playerCardsObject, CardsCountValues);
        playertotalScore += Pintock[0];
        textplayerScoreShow.text = playertotalScore.ToString();
        print("------- Score: " + Pintock[0]);

        yield return new WaitForSeconds(1f);
        cardAnimate.Play("dealer1");
        yield return new WaitForSeconds(0.5f);
        cardAnimate.Play("player1");
        FindAnyObjectByType<AudioManage>().StartPlaySFX("card");
        GameCompareSpriteCards(1, 2, 0, CardForMachinePlay, machineCardsObject, CardsCountValues);
        machintotalScore += Pintock[1];
        textmachineScoreShow.text = machintotalScore.ToString();
        print("------- Score: " + Pintock[1]);

        yield return new WaitForSeconds(1f);
        cardAnimate.Play("playerSecondCard 1");
        yield return new WaitForSeconds(1f);
        cardAnimate.Play("player1");
        FindAnyObjectByType<AudioManage>().StartPlaySFX("card");
        GameCompareSpriteCards(2, 3, 1, CardsForPlayer, playerCardsObject, CardsCountValues);
        playertotalScore += Pintock[2];
        textplayerScoreShow.text = playertotalScore.ToString();
        print("------- Score: " + Pintock[2]);

        yield return new WaitForSeconds(1f);
        cardAnimate.Play("dealer2");
        yield return new WaitForSeconds(0.5f);
        cardAnimate.Play("player1");
        FindAnyObjectByType<AudioManage>().StartPlaySFX("card");
        machineCardsObject[1].SetActive(true);
        CardForMachinePlay[1].sprite = backCard;

        yield return new WaitForSeconds(0.5f);

        hitbtnClick.enabled = true;
        hitbtnClick.image.color = new Color(255, 255, 255, 255);
        machinebtnClick.enabled = false;
        machinebtnClick.image.color = Color.gray;
        standbtn.enabled = true;
        standbtn.image.color = new Color(255, 255, 255, 255);
        StartCoroutine(getButtonHitAction());
    }

    // TODO: Wait for clear
    IEnumerator getButtonHitAction()
    {
        yield return new WaitForSeconds(3f);

        if (IsHitBtnOn)
        {
            IsHitBtnOn = false;
            hitbtnClick.enabled = true;
            hitbtnClick.image.color = new Color(255, 255, 255, 255);
            if (IsStandBtnOn)
            {
                hitbtnClick.enabled = false;
                hitbtnClick.image.color = Color.gray;
                standbtn.enabled = true;
                standbtn.image.color = new Color(255, 255, 255, 255);
                IsStandBtnOn = false;
            }
            hitbtnClick.enabled = false;
            hitbtnClick.image.color = Color.gray;
        }

    }

    // TODO: Wait this
    IEnumerator PlayerCard4Action()
    {
        print("Player card value: " + playertotalScore);
        yield return new WaitForSeconds(0.3f);
        hitObject.SetActive(false);
        callcardAgainOject.SetActive(true);
        if (IsCallAgainOn)
        {
            IsCallAgainOn = false;
            callcardAgainOject.SetActive(false);
        }
    }

    // TODO: Controll hit again button
    public void CallAgainButtonAction()
    {
        FindAnyObjectByType<AudioManage>().StartPlaySFX("button");
        IsCallAgainOn = true;
        IEnumerator waits()
        {
            callMuiltCards.enabled = false;
            yield return new WaitForSeconds(0.5f);
            cardAnimate.Play("playerFourCard");
            yield return new WaitForSeconds(1f);
            cardAnimate.Play("player1");
            FindAnyObjectByType<AudioManage>().StartPlaySFX("card");
            int playerAceThirdCard = GameCompareSpriteCards(6, 7, 3, CardsForPlayer, playerCardsObject, CardsCountValues);
            if (playertotalScore < 11 && playerAceThirdCard == 0)
            {
                Pintock[6] += 10;
                playertotalScore += Pintock[6];
                print("Player Ace Third Card : " + playerAceThirdCard);
            }
            else
            {
                playertotalScore += Pintock[6];
            }
            print("------playerCardsScore: ----" + playertotalScore);
            textplayerScoreShow.text = playertotalScore.ToString();
            print("------- Score: " + Pintock[6]);

            yield return new WaitForSeconds(1f);
            callcardAgainOject.SetActive(false);
            hitObject.SetActive(true);
        }
        StartCoroutine(waits());


    }

    // TODO: Controll hit button
    IEnumerator PlayerControllCardsAction()
    {
        cardAnimate.Play("playerThirdCard");
        yield return new WaitForSeconds(1f);
 
        cardAnimate.Play("player1");
        FindAnyObjectByType<AudioManage>().StartPlaySFX("card");
        int playerAceThirdCard = GameCompareSpriteCards(3, 4, 2, CardsForPlayer, playerCardsObject, CardsCountValues);
        if (playertotalScore < 11 && playerAceThirdCard == 0)
        {
            Pintock[3] += 10;
            playertotalScore += Pintock[3];
            print("Player Ace Third Card : " + playerAceThirdCard);
        }
        else
        {
            playertotalScore += Pintock[3];
        }
        textplayerScoreShow.text = playertotalScore.ToString();
        print("------- Score: " + Pintock[3]);
        print("Player Score: " + playertotalScore);
        if (playertotalScore <= 20)
        {
            StartCoroutine(PlayerCard4Action());

        }
    }

    // TODO: Controll hit button
    public void HitButtonAction()
    {
        FindAnyObjectByType<AudioManage>().StartPlaySFX("button");
        IsHitBtnOn = true;
        hitbtnClick.enabled = false;
        hitbtnClick.image.color = Color.gray;
        StartCoroutine(PlayerControllCardsAction());
    }

    // TODO: Controll stand button
    public void UsedStandButtonAction()
    {
        FindAnyObjectByType<AudioManage>().StartPlaySFX("button");
        hitObject.SetActive(true);
        callcardAgainOject.SetActive(false);
        IsStandBtnOn = true;
        hitbtnClick.enabled = false;
        hitbtnClick.image.color = Color.gray;
        machinebtnClick.enabled = true;
        machinebtnClick.image.color = new Color(255, 255, 255, 255);
        if (dealOnClick)
        {
            hitbtnClick.enabled = false;
            hitbtnClick.image.color = Color.gray;
            standbtn.enabled = false;
            standbtn.image.color = Color.gray;
            machinebtnClick.enabled = true;
            machinebtnClick.image.color = new Color(255, 255, 255, 255);

            dealOnClick = false;
        }
        standbtn.enabled = false;
        standbtn.image.color = Color.gray;
        IsStandBtnOn = false;
    }

    // TODO: Controll deal button
    public void UsedMachineAutoAction()
    {
        FindAnyObjectByType<AudioManage>().StartPlaySFX("button");
        dealOnClick = true;
        machinebtnClick.enabled = false;
        machinebtnClick.image.color = Color.gray;
        callMuiltCards.enabled = true;
        FindAnyObjectByType<AudioManage>().StartPlaySFX("card");
        GameCompareSpriteCards(4, 5, 1, CardForMachinePlay, machineCardsObject, CardsCountValues);
        machintotalScore += Pintock[4];
        textmachineScoreShow.text = machintotalScore.ToString();
        print("------- Score: " + Pintock[4]);
        if (machintotalScore < playertotalScore && machintotalScore < 20 && playertotalScore < 21)
        {
            cardAnimate.Play("dealer3");
            Invoke("wiatDealereCard3", 0.5f);
        }
        Invoke("CalculateWinner", 2.5f);
        Invoke("GameUpdate", 4f);
    }

    // TODO: Wait dealer card3 
    void getMachineCard3Action()
    {
        cardAnimate.Play("player1");
        int dealerAceThirdCard = GameCompareSpriteCards(5, 6, 2, CardForMachinePlay, machineCardsObject, CardsCountValues);
        if (machintotalScore < 11 && dealerAceThirdCard == 0)
        {
            Pintock[5] += 10;
            machintotalScore += Pintock[5];
            print("Dealer Ace Third Card : " + dealerAceThirdCard);
        }
        else
        {
            machintotalScore += Pintock[5];
        }
        textmachineScoreShow.text = machintotalScore.ToString();
        print("deal score in card3: " + machintotalScore);
        //
        if (machintotalScore < playertotalScore && machintotalScore < 20 && playertotalScore < 21)
        {
            cardAnimate.Play("dealer4");
            Invoke("waitDealerCard4", 0.5f);
        }
    }

    // TODO: Wait dealer card4 
    void getMachineCard4Action()
    {
        cardAnimate.Play("player1");
        FindAnyObjectByType<AudioManage>().StartPlaySFX("card");
        int dealerAceThirdCard = GameCompareSpriteCards(7, 8, 3, CardForMachinePlay, machineCardsObject, CardsCountValues);
        if (machintotalScore < 11 && dealerAceThirdCard == 0)
        {
            Pintock[7] += 10;
            machintotalScore += Pintock[7];
            print("Dealer Ace Third Card : " + dealerAceThirdCard);
        }
        else
        {
            machintotalScore += Pintock[7];
        }
        textmachineScoreShow.text = machintotalScore.ToString();
        print("deal score in card4: " + machintotalScore);
    }

    // TODO: Clear coin items
    void MasterCleanItems()
    {
        for (int i = 0; i < coinNormal.Length; i++)
        {
            coinNormal[i].enabled = true;
            coinNormal[i].image.color = new Color(255, 255, 255, 255);
            coinModel[i].SetActive(false);
        }
        // TODO: Clear all items in Coins class
        for (int i = 0; i < (FindAnyObjectByType<Coins>().coinsActive.Length); i++)
        {
            FindAnyObjectByType<Coins>().coinsActive[i].SetActive(false);
        }
        FindAnyObjectByType<Coins>().coinSuffer = 0;
        FindAnyObjectByType<Coins>().CoinS = 0;
        FindAnyObjectByType<Coins>().txtCoinActive.SetActive(false);
        FindAnyObjectByType<Coins>().i = 0;
    }

    // TODO: Wait place your bets
    void getBetsCoins()
    {
        if (action)
        {
            StartCoroutine(GameProccesContinue());
            action = false;
        }
    }

    // TODO: Undo coin button
    public void ControllOnCoinsBackUp()
    {
        MakeCoinAction(true, false, 1);
        FindAnyObjectByType<AudioManage>().StartPlaySFX("button");
        if (FindAnyObjectByType<Coins>().coinStorage.Count > 0)
        {
            int i = FindAnyObjectByType<Coins>().coinStorage.Count;
            i -= coinBack;
            print("Coin: " + i);
            isCoins = FindAnyObjectByType<Coins>().coinStorage[i];
            print("coin storage " + isCoins);
            FindAnyObjectByType<Coins>().coinsActive[i].SetActive(false);
            FindAnyObjectByType<Coins>().CoinS -= isCoins;
            bets -= isCoins;
            coinDisplay -= isCoins;
            money += isCoins;
            print("balance -: " + bets);
            print("coinsS -: " + coinDisplay);
            print("COINS: " + isCoins);
            // FindAnyObjectByType<Coins>().coinStorage.RemoveAt(coins);
            FindAnyObjectByType<Coins>().coinStorage.RemoveAt(i);
            print("Remove coins: " + FindAnyObjectByType<Coins>().coinStorage.Count);
            FindAnyObjectByType<Coins>().i = FindAnyObjectByType<Coins>().coinStorage.Count;
            FindAnyObjectByType<Coins>().txtCoins.text = coinDisplay.ToString() + " k";
            //FindAnyObjectByType<Coins>().CoinS.ToString() + " k";
            MoneyStorage.text = money.ToString() + " k";

        }
        else if (coinDisplay == 0)
        {
            FindAnyObjectByType<Coins>().CoinS = FindAnyObjectByType<Coins>().coinSuffer = 0;
            FindAnyObjectByType<Coins>().txtCoinActive.SetActive(false);
            bets = 0;
            coinDisplay = 0;
            print("balance 0: " + bets);
            print("coinsS 0: " + coinDisplay);
            FindAnyObjectByType<Coins>().coinsActive[0].SetActive(false);
            FindAnyObjectByType<Coins>().txtCoinActive.SetActive(false);
            FindAnyObjectByType<Coins>().coinSuffer = 0;
            undoCoins.enabled = false;
            undoCoins.image.color = Color.gray;
            callGameStartBtn.enabled = false;
            callGameStartBtn.image.color = Color.gray;
        }
    }

    // TODO: Coin color active
    public void MakeCoinAction(bool cas1, bool cas2, int des)
    {
        for (int i = 0; i < coinNormal.Length; i++)
        {
            coinNormal[i].enabled = cas1;
            if (des == 0) coinNormal[i].image.color = Color.gray;
            else if (des == 1) coinNormal[i].image.color = new Color(255, 255, 255, 255);
            coinModel[i].SetActive(cas2);
        }
    }

    // TODO: Call dealer button
    public void ButtonStartGotoPlayGame()
    {
        if (coinDisplay > 0)
        {
            FindAnyObjectByType<AudioManage>().StartPlaySFX("button");
            action = true;
            callGameStartBtn.enabled = false;
            callStartGameEnable.SetActive(false);
            undoCoins.enabled = false;
            undoBtnEnable.SetActive(false);

            coinAinmate[0].Play("1ks");
            coinAinmate[1].Play("5ks");
            coinAinmate[2].Play("10ks");
            coinAinmate[3].Play("20ks");
            coinAinmate[4].Play("50ks");
            coinAinmate[5].Play("100ks");
            FindAnyObjectByType<Coins>().coinStorage.Clear();
            MakeCoinAction(false, false, 0);
            TimeON = true;
            Clock = 0.5f;
            CardsCountValues.Clear();
            playertotalScore = 0;
            textplayerScoreShow.text = playertotalScore.ToString();
            machintotalScore = 0;
            textmachineScoreShow.text = machintotalScore.ToString();
        }
    }

    // TODO: Clear button
    void GameStartUpdateAllProccessing()
    {
        playertotalScore = 0;
        machintotalScore = 0;
        bets = coinValueStore = FindAnyObjectByType<Coins>().CoinS = coinDisplay = 0;
        textmachineScoreShow.text = machintotalScore.ToString();
        textplayerScoreShow.text = playertotalScore.ToString();
        playerCardsObject[0].SetActive(false);
        playerCardsObject[1].SetActive(false);
        playerCardsObject[2].SetActive(false);
        playerCardsObject[3].SetActive(false);

        machineCardsObject[0].SetActive(false);
        machineCardsObject[1].SetActive(false);
        machineCardsObject[2].SetActive(false);
        machineCardsObject[3].SetActive(false);

        hitbtnClick.enabled = false;
        hitbtnClick.image.color = Color.gray;
        standbtn.enabled = false;
        standbtn.image.color = Color.gray;
        machinebtnClick.enabled = false;
        machinebtnClick.image.color = Color.gray;
        IsHitBtnOn = dealOnClick = IsStandBtnOn = false;

        undoCoins.enabled = false;
        undoCoins.image.color = Color.gray;
        callGameStartBtn.enabled = false;
        callGameStartBtn.image.color = Color.gray;

        coinAinmate[0].Play("1k");
        coinAinmate[1].Play("5k");
        coinAinmate[2].Play("10k");
        coinAinmate[3].Play("20k");
        coinAinmate[4].Play("50k");
        coinAinmate[5].Play("100k");
        MasterCleanItems();
    }

    // TODO: Coins choise button
    public void switchCoinsItems()
    {
        string coinString = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        coinAinmate[0].Play("1ks");
        coinAinmate[1].Play("5ks");
        coinAinmate[2].Play("10ks");
        coinAinmate[3].Play("20ks");
        coinAinmate[4].Play("50ks");
        coinAinmate[5].Play("100ks");

        switch (coinString)
        {
            case "1k":
                FindAnyObjectByType<AudioManage>().StartPlaySFX("coin");
                MoneyViewerMode(0, 1, 2, 3, 4, 5);
                coinValueStore = 1;
                break;
            case "5k":
                FindAnyObjectByType<AudioManage>().StartPlaySFX("coin");
                MoneyViewerMode(1, 0, 2, 3, 4, 5);
                coinValueStore = 5;
                break;
            case "10k":
                FindAnyObjectByType<AudioManage>().StartPlaySFX("coin");
                MoneyViewerMode(2, 0, 1, 3, 4, 5);
                coinValueStore = 10;
                break;
            case "20k":
                FindAnyObjectByType<AudioManage>().StartPlaySFX("coin");
                MoneyViewerMode(3, 0, 1, 2, 4, 5);
                coinValueStore = 20;
                break;
            case "50k":
                FindAnyObjectByType<AudioManage>().StartPlaySFX("coin");
                MoneyViewerMode(4, 0, 1, 2, 3, 5);
                coinValueStore = 50;
                break;
            case "100k":
                FindAnyObjectByType<AudioManage>().StartPlaySFX("coin");
                MoneyViewerMode(5, 0, 1, 2, 3, 4);
                coinValueStore = 100;
                break;
        }
        if (bets > 0 && coinDisplay > 0 || money >= coinValueStore)
        {
            print("balance: " + bets);
            print("coinsS: " + coinDisplay);
            // print("CoinS: " + FindAnyObjectByType<Coins>().CoinS);
            callStartGameEnable.SetActive(true);
            undoBtnEnable.SetActive(true);
            callGameStartBtn.enabled = true;
            callGameStartBtn.image.color = new Color(255, 255, 255, 255);
            undoCoins.enabled = true;
            undoCoins.image.color = new Color(255, 255, 255, 255);
        }
        FindAnyObjectByType<Coins>().CoinS = coinDisplay;
    }

    // TODO: Coin hover change img
    private void MoneyViewerMode(int clone1, params int[] varg)
    {
        coinModel[clone1].SetActive(true);
        coinNormal[clone1].enabled = false;
        foreach (int btnItem in varg)
        {
            coinNormal[btnItem].image.color = Color.gray;
            coinModel[btnItem].SetActive(false);
        }
        coinNormal[clone1].enabled = true;
    }

    // TODO: Calculate Winner, Lose and Push Score
    private void GameControllerCalculater()
    {
        print("player last Score over: " + playertotalScore);
        print("dealer last Score over: " + machintotalScore);
        if ((machintotalScore < playertotalScore && playertotalScore <= 21) || (playertotalScore < machintotalScore && machintotalScore > 21))
        {
            StartCoroutine(playerYouAreWinner());
            print("Player WINNER !!");
            money += (bets * 2);
            print("balance: " + (bets * 2));
        }
        else if ((machintotalScore > playertotalScore && machintotalScore <= 21) || (playertotalScore > machintotalScore && playertotalScore > 21))
        {
            StartCoroutine(MachineHisWinner());
            print("Dealer WINNER !!");
        }
        else if (playertotalScore == machintotalScore)
        {
            StartCoroutine(PushWithPlayer());
            print("PUSH: BETS RETURNED !!");
            money += bets;
            print("balance: " + bets);
        }
        bets = 0;
        FindAnyObjectByType<Coins>().txtCoins.text = bets.ToString() + " k";
        print("last balance: " + bets);
    }
    IEnumerator playerYouAreWinner()
    {
        yield return new WaitForSeconds(0.5f);
        FindAnyObjectByType<AudioManage>().StartPlaySFX("win");
        playerwinner.SetActive(true);
        playerWinAnimate.Play("winner");
        yield return new WaitForSeconds(2f);
        playerwinner.SetActive(false);
    }
    IEnumerator MachineHisWinner()
    {
        yield return new WaitForSeconds(0.5f);
        FindAnyObjectByType<AudioManage>().StartPlaySFX("lose");
        machinewinner.SetActive(true);
        playerLoseAnimate.Play("lose");
        yield return new WaitForSeconds(2f);
        machinewinner.SetActive(false);
    }
    IEnumerator PushWithPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        FindAnyObjectByType<AudioManage>().StartPlaySFX("push");
        equalPlayer.SetActive(true);
        equalAnimate.Play("push");
        yield return new WaitForSeconds(2f);
        equalPlayer.SetActive(false);

    }

    // Home scene
    public void GotoHomeScene()
    {
        SceneManager.LoadScene("Home Scene");
    }
}
