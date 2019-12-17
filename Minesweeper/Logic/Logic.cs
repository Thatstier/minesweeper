﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MineSweeper
{
    public class Logic
    {
        static private List<int> minesList = new List<int>();
        static public List<Tile> tilesList = new List<Tile>();

        static public string dataFilePath = @"../../../Data/Data.txt";

        static private int width;
        static private int height;

        static public bool inGame = false;

        public static List<int> generateRandom(int count, int min, int max, int ignoreNum)
        {
            Random r = new Random();

            List<int> result = new List<int>();
            while (result.Count() < count)
            {
                int x = r.Next(min, max);
                if (x != ignoreNum && result.Contains(x) == false)
                {
                    result.Add(x);
                }
            }
            return result;
        }

        static public void mainMenu()
        {
            Console.WriteLine("Welcome to MineSweeper!\n");
            if (File.Exists(dataFilePath))
            {
                string[] dataFile = File.ReadAllLines(dataFilePath);
                foreach (var line in dataFile)
                {
                    if (line.Contains("width:") == false && line.Contains("height:") == false)
                    {
                        tilesList.Add(new Tile() { id = int.Parse(line.Split(';').ToList()[0]), number = int.Parse(line.Split(';').ToList()[1]), isOpen = bool.Parse(line.Split(';').ToList()[2]),  isFlagged = bool.Parse(line.Split(';').ToList()[3]) });
                        if (int.Parse(line.Split(';').ToList()[1]) == -1)
                        {
                            minesList.Add(int.Parse(line.Split(';').ToList()[0]));
                        }
                    }
                    else
                    {
                        if (line.Contains("width"))
                        {
                            width = int.Parse(line.Remove(0, 6));
                        }
                        else
                        {
                            height = int.Parse(line.Remove(0, 7));
                        }
                    }
                }
            }
            else
            {
            }
        }

        static public void onClickLeft(int clickedTile)
        {
            if (File.Exists(dataFilePath) == false)
            {
                if (inGame == false)
                {
                    minesList.Clear();
                    tilesList.Clear();
                    createField(10, 10, 20, clickedTile);
                    return;
                }
                else
                {
                    inGame = false;
                    return;
                }
            }

            if (tilesList[clickedTile].isFlagged == false)
            {
                if (tilesList[clickedTile].number == -1)
                {
                    tilesList[clickedTile].isOpen = true;
                    File.Delete(dataFilePath);
                    return;
                }
                if (tilesList[clickedTile].number == 0)
                {
                    List<int> outerList = new List<int>();
                    List<int> innerList = new List<int>();
                    innerList.Add(clickedTile);
                    openRegion(clickedTile, outerList, innerList);

                    foreach (var intac in outerList)
                    {
                        tilesList[intac].isOpen = true;
                    }
                    foreach (var intac in innerList)
                    {
                        tilesList[intac].isOpen = true;
                    }

                    checkWin();
                }
                if (tilesList[clickedTile].number != 0 && tilesList[clickedTile].number != -1)
                {
                    if (tilesList[clickedTile].isOpen == true && tilesList[clickedTile].number != 0)
                    {
                        List<int> surroundList = checkSurround(clickedTile);
                        int surroundingFlags = 0;
                        foreach (int surroundInt in surroundList)
                        {
                            if (tilesList[surroundInt].isFlagged == true)
                            {
                                surroundingFlags++;
                            }
                        }

                        if (tilesList[clickedTile].number == surroundingFlags)
                        {
                            bool hasLost = false;
                            foreach (int surroundInt in surroundList)
                            {
                                if (tilesList[surroundInt].isFlagged == false && tilesList[surroundInt].isOpen == false)
                                {
                                    if (tilesList[surroundInt].number == -1)
                                    {
                                        hasLost = true;
                                    }
                                    else
                                    {
                                        if (tilesList[surroundInt].number == 0)
                                        {
                                            List<int> outerList = new List<int>();
                                            List<int> innerList = new List<int>();
                                            innerList.Add(clickedTile);
                                            openRegion(clickedTile, outerList, innerList);

                                            foreach (var intac in outerList)
                                            {
                                                if (tilesList[intac].isFlagged == false)
                                                {
                                                    tilesList[intac].isOpen = true;

                                                }
                                            }
                                            foreach (var intac in innerList)
                                            {
                                                if (tilesList[intac].isFlagged == false)
                                                {
                                                    tilesList[intac].isOpen = true;

                                                }
                                            }
                                        }
                                        else
                                        {
                                            tilesList[surroundInt].isOpen = true;
                                        }
                                    }
                                }
                            }

                            if (hasLost == true)
                            {
                                File.Delete(dataFilePath);
                                return;
                            }
                        }
                    }
                    else
                    {
                        tilesList[clickedTile].isOpen = true;
                    }

                    checkWin();
                }
            }
        }

        static public void onClickRight(int clickedTile)
        {
            if (tilesList[clickedTile].isOpen == false)
            {
                if (tilesList[clickedTile].isFlagged == true)
                {
                    tilesList[clickedTile].isFlagged = false;
                }
                else
                {
                    tilesList[clickedTile].isFlagged = true;
                }
                saveField();
            }
        }

        static private void checkWin()
        {
            int numberOfOpenedTiles = 0;
            int numberOfPotentialMines = 0;
            foreach (Tile tile in tilesList)
            {
                if (tile.isOpen == false && minesList.Contains(tile.id))
                {
                    numberOfPotentialMines++;
                }

                if (tile.isOpen == true)
                {
                    numberOfOpenedTiles++;
                }
                
            }

            if (minesList.Count() == numberOfPotentialMines && width*height - minesList.Count() == numberOfOpenedTiles)
            {
                File.Delete(dataFilePath);
                return;
            }

            saveField();
        }

        static void openRegion(int startingTile, List<int> outerList, List<int> innerList)
        {
            List<int> newList = new List<int>();
            foreach (var j in checkSurround(startingTile))
            {
                if (tilesList[j].number != 0 && outerList.Contains(j) == false)
                {
                    outerList.Add(j);
                }
                if (tilesList[j].number == 0 && innerList.Contains(j) == false)
                {
                    newList.Add(j);
                }
            }
            foreach(int i in newList)
            {
                innerList.Add(i);
            }
            foreach (int i in newList)
            {
                openRegion(i, outerList, innerList);
            }

        }

        static List<int> checkSurround(int startTile)
        {
            List<int> result = new List<int>();
            List<int> endResult = new List<int>();
            bool right = (startTile + 1) % width != 0;
            bool left = (startTile % width) != 0;
            bool up = Enumerable.Range(0, width - 1).ToList().Contains(startTile) == false;
            bool down = Enumerable.Range((height - 1) * width, width * height - 1).ToList().Contains(startTile) == false;

            if (right == true)
            {
                result.Add(startTile + 1);
                if (up == true)
                {
                    result.Add(startTile - width + 1);
                }
                if (down == true)
                {
                    result.Add(startTile + width + 1);
                }
            }
            if (left == true)
            {
                result.Add(startTile - 1);
                if (up == true)
                {
                    result.Add(startTile - width - 1);
                }
                if (down == true)
                {
                    result.Add(startTile + width - 1);
                }
            }
            if (up == true)
            {
                result.Add(startTile - width);
            }
            if (down == true)
            {
                result.Add(startTile + width);
            }

            foreach (int i in result)
            {
                if (i >= 0 && i <= (width * height - 1))
                {
                    endResult.Add(i);
                }
            }

            return endResult;
        }

        static public void createField(int inputWidth, int inputHeight, int mines, int ignoreTile)
        {
            inGame = true;
            width = inputWidth;
            height = inputHeight;
            minesList = generateRandom(mines, 0, width * height, ignoreTile);
            for (int i = 0; i < width * height; i++)
            {
                if (minesList.Contains(i))
                {
                    tilesList.Add(new Tile() { id = i, isOpen = false, number = -1, isFlagged = false });
                }
                else
                {
                    tilesList.Add(new Tile() { id = i, isOpen = false, number = 0, isFlagged = false });
                }
            }

            List<int> additionList = new List<int>();

            foreach(var i in tilesList)
            {
                if (i.number == -1)
                {
                    foreach (var j in checkSurround(i.id))
                    {
                        additionList.Add(j);
                    }
                }
            }

            foreach(var i in additionList)
            {
                if (tilesList[i].number != -1)
                {
                    tilesList[i].number = tilesList[i].number + 1;
                }
            }

            saveField();

            onClickLeft(ignoreTile);
        }

        static private void saveField()
        {
            using (StreamWriter sw = File.CreateText(dataFilePath))
            {
                sw.Close();
            }
            string[] dataFile = File.ReadAllLines(dataFilePath);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(dataFilePath))
            {
                    file.WriteLine("width:" + width);
                    file.WriteLine("height:" + height);
                    foreach (Tile tile in tilesList)
                    {
                        file.WriteLine(ConvertToString(tile));
                    }
            }
        }

        static private string ConvertToString(Tile tile)
        {
            string str = "";
            str += tile.id;
            str += ';';
            str += tile.number;
            str += ';';
            str += tile.isOpen;
            str += ';';
            str += tile.isFlagged;
            return str;
        }
    }
}
