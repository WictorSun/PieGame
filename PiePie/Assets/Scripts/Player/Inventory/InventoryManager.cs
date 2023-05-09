using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class InventoryManager : MonoBehaviour
{
    public static int _pizzas;
    public static int _money;
    public static int _currentCash;
    public int _currentPizzas;
    public GameManager _GM;
    public Animator _25DollarsAnim;
    public Animator _bikePrize;
    public Animator _jetPackAnim;
    public Animator _bagAnim;

    public static bool HasBike;
    public static bool HasBag;
    public static bool HasJetPack;

    public bool buy;
    public static bool _start20;
    public static bool _boughtBike;
    public static bool _canBuyBike;
    public static bool _canBuyJetPack;
    public static bool _gotBag;

    private void Awake()
    {
        _money = 5;
        _pizzas = 5;

    }
    public void Update()
    {
        _currentCash = _money;
        _currentPizzas = _pizzas;
        //Debug.Log(_currentCash);
        //Debug.Log(_currentPizzas);
        if (buy)
        {
            AddingMoneyPizza();
            buy = false;
        }
        if (_money == 8)
        {
            _canBuyBike = true;
        }
        if (HasBike)
        {
            _GM.hasBike = true;
            
        }
        if(HasJetPack)
        {
            _GM._hasJetPack = true;
        }
        if (HasBag)
        {
            _GM.hasBag = true;
        }
        if (_start20)
        {
            StartCoroutine(Playtwentyfive(.3f));
        }
        if (_canBuyBike)
        {
            StartCoroutine(GetBike(.3f));
        }
        if (_canBuyJetPack)
        {
            StartCoroutine(GetJetPack(.3f));
        }
        if (_gotBag)
        {
            StartCoroutine(GotTheBag(0.3f));
        }
    }
    [YarnFunction("MyMoney")]
    public static int GetHowMuchMoneyIHave()
    {
        return _money;
    }

    [YarnCommand("addMoney")]
    public static void AddingMoneyPizza()
    {
        _money += 25;
        _start20 = true;

    }
    [YarnCommand("BuyTheBike")]
    public static void TakingMoneyForrBike()
    {
        _money -= 5;
        HasBike = true;
        _canBuyBike = true;
    }
     [YarnCommand("BuyTheJetPack")]
    public static void TakingMoneyForrJetPack()
    {
        _money -= 5;
        HasJetPack = true;
        _canBuyJetPack = true;
    }
    [YarnCommand("BuyTheBag")]
    public static void TakingMoneyForBag()
    {
        _money -= 3;
        HasBag = true;
        _gotBag = true;

    }
    [YarnCommand("ShowBike")]
    public static void IGotTheBike()
    {
        _canBuyBike = true;
    }
    [YarnCommand("TakePizza")]
    public static void TakePizza()
    {
        _pizzas -= 1;
    }

   IEnumerator Playtwentyfive(float sec)
    {
        _25DollarsAnim.SetBool("Got25", true);

        yield return new WaitForSeconds(sec);

        _25DollarsAnim.SetBool("Got25", false);
        _start20 = false;
    }
    IEnumerator GetBike(float sec)
    {
        _bikePrize.SetBool("GotBike", true);
        yield return new WaitForSeconds(sec);
        _bikePrize.SetBool("GotBike", false);
        _canBuyBike = false;
    }
    IEnumerator GetJetPack(float sec)
    {
        _jetPackAnim.SetBool("GotJetPack", true);
        yield return new WaitForSeconds(sec);
        _jetPackAnim.SetBool("GotJetPack", false);
        _canBuyJetPack = false;
    }
    IEnumerator GotTheBag(float sec)
    {
        _bagAnim.SetBool("GotBag", true);
        yield return new WaitForSeconds(sec);
        _bagAnim.SetBool("GotBag", false);
        _gotBag = false;
    }
}
