using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SudokuCell : MonoBehaviour
{
    Board board;

    int row;
    int col;
    int value;

    string id;

    public Text t;

    public void SetValues(int _row, int _col, int value, string _id, Board _board) // Cell er nijer identity set kore
    {
        row = _row;
        col = _col; 
        id = _id;
        board = _board;

        Debug.Log(t.text);

        if (value != 0)
        {
            t.text = value.ToString();
        }
        else
        {
            t.text = " ";
        }

        if (value != 0)
        {
            GetComponentInParent<Button>().enabled = false;
        }
        else
        {
            t.color = new Color32(0, 102,187,255);
        }
    }

    public void ButtonClicked() 
    {
        InputButton.instance.ActivateInputButton(this);

        board.UnHighLightALL();
        board.HighLightSelected(row,col,true); //highlight on
    }

    public void UpdateValue(int newValue) //input value ta check kore
    {
        board.HighLightSelected(row, col, false); //highlight off

        value = newValue;
        t.text = value.ToString();
        board.LastCell = this;
        board.CheckValue(row,col,newValue,t);
    }
}
