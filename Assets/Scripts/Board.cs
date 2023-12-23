using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Board : MonoBehaviour
{
    // Sudoku Grid Create kora
    int[,] grid = new int[9,9]; 
    int[,] puzzle = new int[9, 9];
    SudokuCell[,] CellsList = new SudokuCell[9, 9];

    public int difficulty = 15;
    public SudokuCell LastCell;
    

    public Transform square00, square01, square02, 
                     square10, square11, square12, 
                     square20, square21, square22;
    public GameObject SudokuCell_Prefab;
    public GameObject winMenu;
    [SerializeField] GameObject loseText;
    public GameObject timerObj;

    // Start call hoy first frame update er agea
    void Start()
    {
        winMenu.SetActive(false);
       
        //Debug.Log("Difficulty is: " + difficulty);
        CreateGrid();
        CreatePuzzle();

        VisualizeButtons();
    }

    bool ColumnContainsValue(int col, int value)
    {
        for (int i = 0; i < 9; i++)
        {
            if (grid[i, col] == value)
            {
                return true;
            }
        }

        return false;
    }

    bool RowContainsValue(int row, int value) //row er moddhea value ta check kore
    {
        for (int i = 0; i < 9; i++)
        {
            if (grid[row, i] == value)
            {
                return true;
            }
        }

        return false;
    }

    bool SquareContainsValue(int row, int col, int value) //Square er moddhea value ta check kore
    {
        //blocks are 0-2, 3-5, 6-8
        //row / 3 first grid coordinate * 3 
        //ints 

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (grid[ row / 3 * 3 + i , col / 3 * 3 + j ] == value)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void ColumnHighLight(int col, bool on_off)
    {
        for (int i = 0; i < 9; i++)
        {
            if (on_off == false)
            {
                CellsList[i, col].GetComponent<Image>().color = new Color32(152, 206, 214,255);
            }
            else
            {
                CellsList[i, col].GetComponent<Image>().color = new Color32(182, 236, 244,255);
            }          
        }   
    }

    void RowHighLight(int row, bool on_off) 
    {
        for (int i = 0; i < 9; i++)
        {
            if (on_off == false)
            {
                CellsList[row, i].GetComponent<Image>().color = new Color32(152, 206, 214, 255);
            }
            else
            {
                CellsList[row, i].GetComponent<Image>().color = new Color32(182, 236, 244, 255);
            }
        }
    }

    void SquareHighLight(int row, int col, bool on_off) 
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (on_off == false)
                {
                    CellsList[row / 3 * 3 + i, col / 3 * 3 + j].GetComponent<Image>().color = new Color32(152, 206, 214, 255);
                }
                else
                {
                    CellsList[row / 3 * 3 + i, col / 3 * 3 + j].GetComponent<Image>().color = new Color32(182, 236, 244, 255);
                }
                   
            }
        }
    }

    bool CheckAll(int row, int col, int value) //row, col, Square er moddhea value ta check kore
    {
        if (ColumnContainsValue(col,value)) {
            //Debug.Log(row + " " + col);
            return false;
        }
        if (RowContainsValue(row, value))
        {
            //Debug.Log(row + " " + col);
            return false;
        }
        if (SquareContainsValue(row, col, value))
        {
            //Debug.Log(row + " " + col);
            return false;
        }

        return true;
    }
    public void UnHighLightALL()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                CellsList[i,j].GetComponent<Image>().color = new Color32(152, 206, 214, 255);
            }
        }
    }
    public void HighLightSelected(int row, int col, bool on_off) 
    {
        ColumnHighLight(col, on_off);
        RowHighLight(row, on_off);
        SquareHighLight(row, col, on_off);
    }

    bool IsValid() // grid er moddhea zero achea ki na check kore
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid[i,j] == 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    void CreateGrid() //grid create kore
    {
        List<int> rowList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        List<int> colList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        int value = rowList[Random.Range(0, rowList.Count)];
        grid[0, 0] = value;
        rowList.Remove(value);
        colList.Remove(value);

        for (int i = 1; i < 9; i++)
        {
            value = rowList[Random.Range(0, rowList.Count)];
            grid[i, 0] = value;
            rowList.Remove(value);
        }

        for (int i = 1; i < 9; i++)
        {
            value = colList[Random.Range(0, colList.Count)];
            if (i < 3)
            {
                while(SquareContainsValue(0, 0, value))
                {
                    value = colList[Random.Range(0, colList.Count)]; // reroll
                }
            }
            grid[0, i] = value;
            colList.Remove(value);
        }

        for (int i = 6; i < 9; i++)
        {
            value = Random.Range(1, 10);
            while (SquareContainsValue(0, 8, value) || SquareContainsValue(8, 0, value) || SquareContainsValue(8, 8, value)) //check
            {
                value = Random.Range(1, 10);
            }
            grid[i, i] = value;
        }


        GenerateSudoku();
    }

    bool GenerateSudoku() //Sudoku create kore recursive way te
    {
        int row = 0;
        int col = 0;

        if (IsValid())
        {
            return true;
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid[i, j] == 0)
                {
                    row = i;
                    col = j;
                }
            }
        }

        for (int i = 1; i <=9; i++)
        {
            if (CheckAll(row, col, i)) {
                grid[row, col] = i;
                
               
                if (GenerateSudoku())
                {
                    return true;
                }
                else
                {
                    grid[row, col] = 0;
                }
            }
        }
        return false;
    }

    void CreatePuzzle() //visiual matrix create kore
    {
        System.Array.Copy(grid, puzzle, grid.Length);

        // Remove cells
        for (int i = 0; i < difficulty; i++)
        {
            int row = Random.Range(0, 9);
            int col = Random.Range(0, 9);

            while (puzzle[row, col] == 0)
            {
                row = Random.Range(0, 9);
                col = Random.Range(0, 9);
            }

            puzzle[row, col] = 0;
        }

        // Sure korte hobe shob puzzle e 8 ta diff numbers achea. Aita sure kore je puzzle er ektai solution
        List<int> onBoard = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        RandomizeList(onBoard);

        for (int i = 0; i < 9; i++)   //akhane visible list theke onboard check kore
        {
            for (int j = 0; j < 9; j++)
            {
                for (int k = 0; k < onBoard.Count - 1; k++)
                {
                    if (onBoard[k] == puzzle[i, j])
                    {
                        onBoard.RemoveAt(k);
                    }
                }
            }
        }

        while (onBoard.Count - 1 > 1) //baki onboard gula visible kore grid theake
        {
            int row = Random.Range(0, 9);
            int col = Random.Range(0, 9);

            if (grid[row, col] == onBoard[0])
            {
                puzzle[row, col] = grid[row, col];
                onBoard.RemoveAt(0);
            }

        }


    }

    void RandomizeList(List<int> l) //jei kono list er value gula ke randomize kore
    {
        
        for (var i = 0; i < l.Count - 1; i++)
        {
            int rand = Random.Range(i, l.Count);
            int temp = l[i];
            l[i] = l[rand];
            l[rand] = temp;
        }
    }

        void VisualizeButtons() //visualize
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                GameObject newButton = Instantiate(SudokuCell_Prefab);
                SudokuCell sudokuCell = newButton.GetComponent<SudokuCell>();
                sudokuCell.SetValues(i, j, puzzle[i, j], i + "," + j, this);
                newButton.name = i.ToString() + j.ToString();

                CellsList[i, j] = sudokuCell; //cell list e cell gulo add

                if (i < 3)
                {
                    if (j < 3)
                    {
                        newButton.transform.SetParent(square00, false);
                    }
                    if (j > 2 && j < 6)
                    {
                        newButton.transform.SetParent(square01, false);
                    }
                    if (j >= 6)
                    {
                        newButton.transform.SetParent(square02, false);
                    }
                }

                if (i >= 3 && i < 6)
                {
                    if (j < 3)
                    {
                        newButton.transform.SetParent(square10, false);
                    }
                    if (j > 2 && j < 6)
                    {
                        newButton.transform.SetParent(square11, false);
                    }
                    if (j >= 6)
                    {
                        newButton.transform.SetParent(square12, false);
                    }
                }

                if (i >= 6)
                {
                    if (j < 3)
                    {
                        newButton.transform.SetParent(square20, false);
                    }
                    if (j > 2 && j < 6)
                    {
                        newButton.transform.SetParent(square21, false);
                    }
                    if (j >= 6)
                    {
                        newButton.transform.SetParent(square22, false);
                    }
                }
        
            }
        }
    }

    
    public void CheckValue(int i,int j,int value,Text t)
    {
        if (value == grid[i, j]) // Value thik ki na check kore
        {
            puzzle[i, j] = value;
            LastCell.GetComponentInParent<Button>().enabled = false;
        }
        else
        {
            t.color = new Color32(255, 0, 0, 255);     //value thik na hole text ta red kore dei              
            Invoke("deleteTextTimer", 1);
        }

        WinCOnditionCheck();
    }

    public void deleteTextTimer() //cell back to original
    {
        LastCell.t.text = "";
        LastCell.t.color = new Color32(0, 102, 187, 255);
    }

    public void WinCOnditionCheck() //koita blank achea check kore r blank na thakle win
    {
        int NumberOfEmptyCells = 0;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (puzzle[i, j] == 0)
                {
                    NumberOfEmptyCells++;
                }
            }
        }
        if (NumberOfEmptyCells == 0)
        {
            timerObj.SetActive(false);
            winMenu.SetActive(true);
        }
    }

}
