using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndController : MonoBehaviour {
    public UnityEngine.UI.Text stroke1;
    public UnityEngine.UI.Text stroke2;
    public UnityEngine.UI.Text total;
    private int score1;
    private int score2;
    private int totalscore;
    void Start()
    {
        score1 = GameController.stroke1 - 4;
        score2 = GameController.stroke2 - 5;
        totalscore = score1 + score2;
        if (score1 < 0)
        {
            stroke1.text = "第１ホール：" + score1.ToString();
        }
        else if (score1 > 0)
        {
            stroke1.text = "第１ホール：" + "+" + score1.ToString();
        }
        else
        {
            stroke1.text = "第１ホール：" + "±0";
        }

        if (score2 < 0)
        {
            stroke2.text = "第２ホール：" + score2.ToString();
        }
        else if (score2 > 0)
        {
            stroke2.text = "第２ホール：" + "+" + score2.ToString();
        }
        else
        {
            stroke2.text = "第２ホール：" + "±0";
        }

        if (totalscore < 0)
        {
            total.text = "合計：" + totalscore.ToString();
        }
        else if (totalscore > 0)
        {
            total.text = "合計：" + "+" + totalscore.ToString();
        }
        else
        {
            total.text = "合計：" + "±0";
        }
    }
}
