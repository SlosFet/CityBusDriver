using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class PlayerData : MonoBehaviour
{
    //This script made it for datas but we dont have any data exept money.So using it for money

    [SerializeField] private int _money;
    [SerializeField] private Animation _moneyAnim;
    [SerializeField] private TextMeshProUGUI _moneyText;

    /// <summary>
    /// The given value added to Money.If you wanna decrease money you should give a negative number. 
    /// </summary>
    public static UnityEvent<int> MoneyChangeEvent = new UnityEvent<int>();

    private void OnEnable()
    {
        MoneyChangeEvent.AddListener(MoneyChange);
    }

    private void OnDisable()
    {
        MoneyChangeEvent.RemoveListener(MoneyChange);
    }

    private void MoneyChange(int value)
    {
        _moneyAnim.Stop();
        _moneyAnim.Play("Money");
        _money += value;
        if (_money <= 0)
            _money = 0;
        _moneyText.text = _money.ToString() + "$";
    }
}
