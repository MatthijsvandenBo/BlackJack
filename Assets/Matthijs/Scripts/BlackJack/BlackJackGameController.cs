using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class BlackJackGameController : MonoBehaviour
{
    int dealersFirstCard = -1;

    public CardStack player;
    public CardStack dealer;
    public CardStack deck;

    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;

    public Text winnerText;
    private uint pot = 0;
    private uint selectpot = 0;

    public TMP_Text selectpotText;
    public List<GameObject> buttons = new List<GameObject>();
    private Chips Chips;


    private void Start()
    {
        Chips = FindObjectOfType<Chips>();
        hitButton.interactable = false;
        stickButton.interactable = false;
    }

    public void Hit()
    {
        player.Push(deck.Pop());
        if (player.HandValue() > 21)
        {
            hitButton.interactable = false;
            stickButton.interactable = false;
            StartCoroutine(DealersTurn());
        }
    }

    public void Stick()
    {
        hitButton.interactable = false;
        stickButton.interactable = false;
        StartCoroutine(DealersTurn());
    }

    public void PlayAgain()
    {
        playAgainButton.interactable = false;

        player.GetComponent<CardStackView>().Clear();
        dealer.GetComponent<CardStackView>().Clear();
        deck.GetComponent<CardStackView>().Clear();
        deck.CreateDeck();

        winnerText.text = "";

        hitButton.interactable = false;
        stickButton.interactable = false;

        dealersFirstCard = -1;

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].SetActive(true);
        }
        selectpot = 0;
        pot = 0;

    }

    private void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            player.Push(deck.Pop());
            HitDealer();
        }
    }

    private void HitDealer()
    {
        int card = deck.Pop();

        if (dealersFirstCard < 0)
        {
            dealersFirstCard = card;
        }

        dealer.Push(card);
        if (dealer.CardCount >= 2)
        {
            CardStackView view = dealer.GetComponent<CardStackView>();
            view.Toggle(card, true);
        }
    }

    private IEnumerator DealersTurn()
    {
        hitButton.interactable = false;
        stickButton.interactable = false;

        CardStackView view = dealer.GetComponent<CardStackView>();
        view.Toggle(dealersFirstCard, true);
        view.ShowCards();
        yield return new WaitForSeconds(1f);

        while (dealer.HandValue() < 17)
        {
            HitDealer();
            yield return new WaitForSeconds(1f);
        }

        if (dealer.HandValue() == (player.HandValue()))
        {
            winnerText.text = "draw";
            int vOut = Convert.ToInt32(pot);
            Chips.scoreChangePlus(vOut);
        }
        else if (player.HandValue() > 21 || (dealer.HandValue() > player.HandValue() && dealer.HandValue() <= 21))
        {
            winnerText.text = "Sorry-- you lose";
        }
        else if (dealer.HandValue() > 21 || (player.HandValue() <= 21 && player.HandValue() > dealer.HandValue()))
        {
            winnerText.text = "Winner, winner! Chicken dinner";
            uint winnerpot;
            winnerpot = pot;
            pot += winnerpot;
            int vOut = Convert.ToInt32(pot);
            Chips.scoreChangePlus(vOut);
        }
        else
        {
            winnerText.text = "The house wins!";
        }

        yield return new WaitForSeconds(1f);
        playAgainButton.interactable = true;
    }

    public void Add()
    {
        selectpot += 10;
    }

    public void subtract()
    {
        selectpot -= 10;
    }

    public void select()
    {

        if (selectpot != 0)
        {
            if (selectpot <= Chips.chips)
            {
                pot = selectpot;
                hitButton.interactable = true;
                stickButton.interactable = true;
                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].SetActive(false);
                }
                StartGame();
                Bet();
            }
        }
    }

    private void Update()
    {
        selectpotText.text = selectpot.ToString();
    }

    private void Bet()
    {
        int vOut = Convert.ToInt32(pot);
        Chips.scoreChangeMin(vOut);
    }
}

