﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NFW
{
    class SecondMode
    {
        public Canvas mainCanvas;
        public Window mainWindow;
        public int LoadMode = 0;
        public int GameMode = 0;
        public string LoadWay = "";
        private HexField hexField;
        public bool isRead = false;
        public string readWay;
        private int firstCityLine = 0, firstCityColumn = 0, secondCityLine = 9, secondCityColumn = 9, AIStatus = 0, maxNumberOfSteeps = 2, playerSteep = 0, numberOfSteeps = 0;
        private Label infoLabel = new Label() { Content = "Ходит первый. Счёт 1:1", HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, Foreground = Brushes.LightGreen };
        private Button menuButton = new Button() { Content = "Меню", BorderBrush = Brushes.LightGray, Background = Brushes.Gray };
        public void Build()
        {
            mainWindow.KeyUp += KeyPressed;
            Thickness margin = new Thickness() { Top = 0, Right = 0 };
            mainCanvas.Children.Clear();
            mainWindow.SizeChanged += WindowSizeChanged;
            mainCanvas.Children.Add(infoLabel);
            hexField = new HexField() { mainCanvas = mainCanvas, mainWindow = mainWindow, FieldHeight = 10, FieldWidth = 10 };
            hexField.Build();
            hexField.HexClick += HexClick;
            menuButton.Margin = margin;
            menuButton.Click += OpenMenu;
            mainCanvas.Children.Add(menuButton);
            WindowSizeChanged(null, null);
            if (!isRead)
            {
                if (GameMode == 1)
                {
                    if (LoadMode == 0)
                    {
                        LoadMap();
                    }
                    else
                    {
                        LoadSave(LoadWay);
                    }
                }
                else
                {
                    if (LoadMode == 1)
                    {
                        LoadSave(LoadWay);
                    }
                    else
                    {
                        FirstModeLoad();
                    }
                }
            }
            else
                ReadMode();
            if(playerSteep + 1 == AIStatus)
            {
                pair AIStep = AI.SecondMode(hexField, playerSteep);
                hexField.Step(AIStep.i, AIStep.j, playerSteep);
                if (playerSteep == 1)
                    numberOfSteeps++;
                playerSteep = 1 - playerSteep;
            }
            InfoLabelChange();
        }
        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            mainCanvas.Height = mainWindow.ActualHeight - 30;
            mainCanvas.Width = mainWindow.ActualWidth - 4;
            Thickness margin = new Thickness() { Top = 0, Left = 0 };
            menuButton.Margin = margin;
            menuButton.Height = mainCanvas.Height / 15;
            menuButton.Width = mainCanvas.Width / 5;
            menuButton.FontSize = (int)Math.Min(mainCanvas.Width / 45, mainCanvas.Height / 27);
            margin.Left = mainCanvas.Width / 5;
            infoLabel.Margin = margin;
            infoLabel.Height = mainCanvas.Height / 15;
            infoLabel.FontSize = (int)Math.Min(mainCanvas.Width / 45, mainCanvas.Height / 27);
            infoLabel.Width = mainCanvas.Width * 3 / 5;
            margin.Top = mainCanvas.Height / 15;
            margin.Left = 0;
            hexField.Margin = margin;
            hexField.Height = mainCanvas.Height * 14 / 15;
            hexField.Width = mainCanvas.Width;
        }
        private void OpenMenu(object sender, RoutedEventArgs e)
        {
            mainWindow.SizeChanged -= WindowSizeChanged;
            Save();
            SecondModeMenu secondModeMenu = new SecondModeMenu() { mainCanvas = mainCanvas, mainWindow = mainWindow };
            secondModeMenu.Build();
        }
        private void Save()
        {
            string t = AIStatus.ToString();
            if (maxNumberOfSteeps < 1000)
            {
                t += "0";
                if (maxNumberOfSteeps < 100)
                {
                    t += "0";
                    if (maxNumberOfSteeps < 10)
                        t += "0";
                }
            }
            t += maxNumberOfSteeps.ToString();
            if (hexField.FieldHeight <= 9)
                t += "0";
            t += hexField.FieldHeight.ToString();
            if (hexField.FieldWidth <= 9)
                t += "0";
            t += hexField.FieldWidth.ToString();
            t += playerSteep.ToString();
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    t += (char)(hexField.field[i, j].Value + 48);
                }
            }
            System.IO.File.WriteAllText("SecondModeFastSave.map", t);
        }
        private void LoadMap()
        {
            try
            {
                string t = System.IO.File.ReadAllText("SecondMode.map");
                AIStatus = (int)t[0] - 48;
                maxNumberOfSteeps = (int)t[1] * 1000 + (int)t[2] * 100 + (int)t[3] * 10 + (int)t[4] - 53328;
                hexField.FieldHeight = (int)t[5] * 10 + t[6] - 528;
                hexField.FieldWidth = (int)t[7] * 10 + (int)t[8] - 528;
                for (int i = 0; i < 50; i++)
                    for (int j = 0; j < 50; j++)
                    {
                        hexField.SetHexValue(i, j, (int)t[i * 100 + j * 2 + 17] * 10 + (int)t[i * 100 + j * 2 + 18] - 528);
                        if (hexField.field[i, j].Value == 11) { firstCityLine = i; firstCityColumn = j; }
                        if (hexField.field[i, j].Value == 12) { secondCityLine = i; secondCityColumn = j; }
                    }
                hexField.Step(firstCityLine, firstCityColumn, 0);
                hexField.Step(secondCityLine, secondCityColumn, 1);
            }
            catch
            {
                MessageBox.Show("Файл с настройками повреждён");
                mainWindow.SizeChanged -= WindowSizeChanged;
                MainMenu mainMenu = new MainMenu() { mainCanvas = mainCanvas, mainWindow = mainWindow };
                mainMenu.Build();
            }
        }
        private void LoadSave(string way)
        {
            try
            {
                string t = System.IO.File.ReadAllText(way);
                AIStatus = (int)t[0] - 48;
                maxNumberOfSteeps = (int)t[1] * 1000 + (int)t[2] * 100 + (int)t[3] * 10 + (int)t[4] - 53328;
                hexField.FieldHeight = (int)t[5] * 10 + (int)t[6] - 528;
                hexField.FieldWidth = (int)t[7] * 10 + (int)t[8] - 528;
                playerSteep = (int)t[9] - 48;
                for (int i = 0; i < 50; i++)
                {
                    for (int j = 0; j < 50; j++)
                    {
                        hexField.SetHexValue(i, j, (int)t[i * 50 + j + 10] - 48);
                        if (hexField.field[i, j].Value == 11) { firstCityLine = i; firstCityColumn = j; }
                        if (hexField.field[i, j].Value == 12) { secondCityLine = i; secondCityColumn = j; }
                        if (hexField.field[i, j].Value > 2)
                            hexField.playerPoints[1 - hexField.field[i, j].Value % 2]++;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Файл сохранения повреждён");
                mainWindow.SizeChanged -= WindowSizeChanged;
                MainMenu mainMenu = new MainMenu() { mainCanvas = mainCanvas, mainWindow = mainWindow };
                mainMenu.Build();
            }
        }
        private void HexClick(int i, int j)
        {
            if(hexField.IsStepPossible(i, j, playerSteep))
            {
                hexField.Step(i, j, playerSteep);
                if (playerSteep == 1)
                    numberOfSteeps++;
                playerSteep = 1 - playerSteep;
                if (numberOfSteeps >= maxNumberOfSteeps)
                {
                    EndGame();
                    return;
                }
                InfoLabelChange();
                if (AIStatus > 0)
                {
                    pair AIStep = AI.SecondMode(hexField, playerSteep);
                    hexField.Step(AIStep.i, AIStep.j, playerSteep);
                    if (playerSteep == 1)
                        numberOfSteeps++;
                    playerSteep = 1 - playerSteep;
                    if (numberOfSteeps >= maxNumberOfSteeps)
                    {
                        EndGame();
                        return;
                    }
                    InfoLabelChange();
                }
            }    
        }
        private void FirstModeLoad()
        {
            try
            {
                string t = System.IO.File.ReadAllText("FirstMode.map");
                AIStatus = (int)t[0] - 48;
                maxNumberOfSteeps = (int)t[1] * 1000 + (int)t[2] * 100 + (int)t[3] * 10 + (int)t[4] - 53328;
                hexField.FieldHeight = (int)t[5] * 10 + t[6] - 528;
                hexField.FieldWidth = (int)t[7] * 10 + (int)t[8] - 528;
                firstCityLine = (int)t[9] * 10 + (int)t[10] - 528;
                firstCityColumn = (int)t[11] * 10 + (int)t[12] - 528;
                secondCityLine = (int)t[13] * 10 + (int)t[14] - 528;
                secondCityColumn = (int)t[15] * 10 + (int)t[16] - 528;
                hexField.SetHexValue(firstCityLine, firstCityColumn, 11);
                hexField.SetHexValue(secondCityLine, secondCityColumn, 12);
                hexField.Step(firstCityLine, firstCityColumn, 0);
                hexField.Step(secondCityLine, secondCityColumn, 1);
            }
            catch
            {
                MessageBox.Show("Файл с настройками повреждён");
                mainWindow.SizeChanged -= WindowSizeChanged;
                MainMenu mainMenu = new MainMenu() { mainCanvas = mainCanvas, mainWindow = mainWindow };
                mainMenu.Build();
            }
        }
        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                OpenMenu(null, null);
        }
        private void EndGame()
        {
            if(hexField.playerPoints[0] > hexField.playerPoints[1])
                MessageBox.Show(String.Format("Игра окончена\nПобедил первый со счётом {0}:{1}", hexField.playerPoints[0], hexField.playerPoints[1]));
            if(hexField.playerPoints[0] < hexField.playerPoints[1])
                MessageBox.Show(String.Format("Игра окончена\nПобедил второй со счётом {0}:{1}", hexField.playerPoints[0], hexField.playerPoints[1]));
            if (hexField.playerPoints[0] == hexField.playerPoints[1])
                MessageBox.Show(String.Format("Игра окончена\nНичья со счётом {0}:{1}", hexField.playerPoints[0], hexField.playerPoints[1]));
            MainMenu mainMenu = new MainMenu() { mainCanvas = mainCanvas, mainWindow = mainWindow };
            mainWindow.SizeChanged -= WindowSizeChanged;
            mainMenu.Build();
        }
        private void InfoLabelChange()
        {
            string t = "Ходит ";
            if (playerSteep == 0)
            {
                t += "первый.";
                infoLabel.Foreground = Brushes.LawnGreen;
            }
            else
            {
                t += "второй.";
                infoLabel.Foreground = Brushes.Red;
            }
            t += String.Format(" Счёт : {0}:{1}. Остал", hexField.playerPoints[0], hexField.playerPoints[1]);
            if (maxNumberOfSteeps - numberOfSteeps == 1)
                t += String.Format("ся 1 ход");
            else
                if (maxNumberOfSteeps - numberOfSteeps < 5)
                t += String.Format("ось {0} хода", maxNumberOfSteeps - numberOfSteeps);
            else
                t += String.Format("ось {0} ходов", maxNumberOfSteeps - numberOfSteeps);
            infoLabel.Content = t;
        }
        private void ReadMode()
        {
            try
            {
                StreamReader file = new StreamReader(readWay);
                {
                    string t = file.ReadLine();
                    string[] s = t.Split(' ');
                    hexField.FieldHeight = Int32.Parse(s[1]);
                    hexField.FieldWidth = Int32.Parse(s[0]);
                    maxNumberOfSteeps = Int32.Parse(s[2]);
                    GameMode = Int32.Parse(s[3]) - 1;
                }
                {
                    string t = file.ReadLine();
                    string[] s = t.Split(' ');
                    firstCityColumn = Int32.Parse(s[0]) - 1;
                    firstCityLine = Int32.Parse(s[1]) - 1;
                    secondCityColumn = Int32.Parse(s[2]) - 1;
                    secondCityLine = Int32.Parse(s[3]) - 1;
                    hexField.SetHexValue(firstCityLine, firstCityColumn, 11);
                    hexField.SetHexValue(secondCityLine, secondCityColumn, 12);
                }
                if (GameMode == 1)
                {
                    {
                        string t = file.ReadLine();
                        string[] s = t.Split(' ');
                        int n = Int32.Parse(s[0]);
                        for (int i = 1; i <= n; i++)
                        {
                            int j1 = Int32.Parse(s[i * 2 - 1]) - 1;
                            int i1 = Int32.Parse(s[i * 2]) - 1;
                            if ((i1 == firstCityLine && j1 == firstCityColumn) || (i1 == secondCityLine && j1 == secondCityColumn))
                            {
                                MessageBox.Show("Некорректное содержиме файла");
                                mainWindow.SizeChanged -= WindowSizeChanged;
                                MainMenu mainMenu = new MainMenu() { mainCanvas = mainCanvas, mainWindow = mainWindow };
                                mainMenu.Build();
                                return;
                            }
                            hexField.SetHexValue(i1, j1, 1);
                        }
                    }
                    {
                        string t = file.ReadLine();
                        string[] s = t.Split(' ');
                        int n = Int32.Parse(s[0]);
                        for (int i = 1; i <= n; i++)
                        {
                            int j1 = Int32.Parse(s[i * 2 - 1]) - 1;
                            int i1 = Int32.Parse(s[i * 2]) - 1;
                            if((i1 == firstCityLine && j1 == firstCityColumn) || (i1 == secondCityLine && j1 == secondCityColumn))
                            {
                                MessageBox.Show("Некорректное содержиме файла");
                                mainWindow.SizeChanged -= WindowSizeChanged;
                                MainMenu mainMenu = new MainMenu() { mainCanvas = mainCanvas, mainWindow = mainWindow };
                                mainMenu.Build();
                                return;
                            }
                            hexField.SetHexValue(i1, j1, 2);
                        }
                    }
                }
                hexField.Step(firstCityLine, firstCityColumn, 0);
                hexField.Step(secondCityLine, secondCityColumn, 1);
                for(int i = 0; i < maxNumberOfSteeps * 2; i++)
                {
                    string t = file.ReadLine();
                    string[] s = t.Split(' ');
                    //x -> column
                    int y = Int32.Parse(s[0]) - 1;
                    int x = Int32.Parse(s[1]) - 1;
                    if(hexField.IsStepPossible(x, y, i % 2))
                        hexField.Step(x , y , i % 2);
                    else
                    {
                        MessageBox.Show(String.Format("Некорректное содержиме файла   :  {0} -  {1} : {2}", i, x, y));
                        mainWindow.SizeChanged -= WindowSizeChanged;
                        MainMenu mainMenu = new MainMenu() { mainCanvas = mainCanvas, mainWindow = mainWindow };
                        mainMenu.Build();
                        return;
                    }
                }
                numberOfSteeps = 0;
                InfoLabelChange();
                EndGame();
            }
            catch
            {
                MessageBox.Show("Некорректное содержиме файла");
                mainWindow.SizeChanged -= WindowSizeChanged;
                MainMenu mainMenu = new MainMenu() { mainCanvas = mainCanvas, mainWindow = mainWindow };
                mainMenu.Build();
            }
        }
    }
}
