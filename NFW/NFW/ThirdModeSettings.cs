﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    class ThirdModeSettings
    {
        public Canvas mainCanvas;
        public Window mainWindow;
        private Thickness margin = new Thickness();
        private Label helpInfo = new Label() { Content = "Укажите типы участков поля посредством нажатия на них", VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center, Foreground = Brushes.Red };
        private Label enterFieldPropInfo = new Label() { Content = "Задайте размеры поля", HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
        private Label enterFieldHeightInfo = new Label() { Content = "Высота:", HorizontalContentAlignment = HorizontalAlignment.Right, VerticalContentAlignment = VerticalAlignment.Center };
        private TextBox enterFieldHeightBox = new TextBox() { VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center, FontWeight = FontWeights.Medium };
        private Label enterFieldWidthInfo = new Label() { Content = "Ширина:", HorizontalContentAlignment = HorizontalAlignment.Right, VerticalContentAlignment = VerticalAlignment.Center };
        private TextBox enterFieldWidthBox = new TextBox() { VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center, FontWeight = FontWeights.Medium };
        private Label enterCordInfo = new Label() { Content = "Задайте координаты\nгородов", HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
        private Label lineInfo = new Label() { Content = "Строка", HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
        private Label columnInfo = new Label() { Content = "Столбец", HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
        private Label firstInfo = new Label() { Content = "Первый", HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
        private Label secondInfo = new Label() { Content = "Второй", HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
        private TextBox firstLineBox = new TextBox() { HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Medium };
        private TextBox firstColumnBox = new TextBox() { HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Medium };
        private TextBox secondLineBox = new TextBox() { HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Medium };
        private TextBox secondColumnBox = new TextBox() { HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Medium };
        private Button applySettings = new Button() { Content = "Применить настройки" };
        private Button startButton = new Button() { Content = "Начать игру" };
        private Button backButton = new Button() { Content = "Назад" };
        private HexField hexField;
        public void Build()
        {
            mainCanvas.Height = mainWindow.ActualHeight - 30;
            mainCanvas.Width = mainWindow.ActualWidth - 4;
            mainCanvas.Children.Add(helpInfo);
            mainWindow.SizeChanged += WindowSizeChanged;
            mainCanvas.Children.Add(enterFieldPropInfo);
            mainCanvas.Children.Add(enterFieldHeightInfo);
            mainCanvas.Children.Add(enterFieldHeightBox);
            mainCanvas.Children.Add(enterFieldWidthInfo);
            mainCanvas.Children.Add(enterFieldWidthBox);
            mainCanvas.Children.Add(enterCordInfo);
            mainCanvas.Children.Add(lineInfo);
            mainCanvas.Children.Add(columnInfo);
            mainCanvas.Children.Add(firstInfo);
            mainCanvas.Children.Add(secondInfo);
            mainCanvas.Children.Add(firstLineBox);
            mainCanvas.Children.Add(firstColumnBox);
            mainCanvas.Children.Add(secondLineBox);
            mainCanvas.Children.Add(secondColumnBox);
            mainCanvas.Children.Add(applySettings);
            mainCanvas.Children.Add(startButton);
            mainCanvas.Children.Add(backButton);

            backButton.Click += Back;
             
            hexField = new HexField() { FieldHeight = 20, FieldWidth = 20, mainCanvas = mainCanvas, mainWindow = mainWindow, Height = mainCanvas.Height * 14 / 15, Width = mainCanvas.Width * 3 / 4 };
            hexField.Build();
            WindowSizeChanged(null, null);

        }
        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Много однородного кода. Желательно не читать 
            mainCanvas.Height = mainWindow.ActualHeight - 30;
            mainCanvas.Width = mainWindow.ActualWidth - 4;
            int fontSize = (int)Math.Min(mainCanvas.Width / 45, mainCanvas.Height / 27);
            margin.Left = mainCanvas.Width / 4;
            margin.Top = 0;
            helpInfo.Margin = margin;
            helpInfo.Height = mainCanvas.Height / 15;
            helpInfo.Width = mainCanvas.Width * 3 / 4;
            helpInfo.FontSize = fontSize;
            margin.Top = mainCanvas.Height / 10;
            margin.Left = 0;
            enterFieldPropInfo.Margin = margin;
            enterFieldPropInfo.Width = mainCanvas.Width / 4;
            enterFieldPropInfo.Height = mainCanvas.Height / 15;
            enterFieldPropInfo.FontSize = fontSize;
            margin.Top += mainCanvas.Height / 15;
            enterFieldHeightInfo.Margin = margin;
            enterFieldHeightInfo.Width = mainCanvas.Width / 8;
            enterFieldHeightInfo.Height = mainCanvas.Height / 15;
            enterFieldHeightInfo.FontSize = fontSize;
            margin.Left = mainCanvas.Width / 8;
            margin.Top += mainCanvas.Height / 60;
            enterFieldHeightBox.Margin = margin;
            enterFieldHeightBox.Width = mainCanvas.Width / 20;
            enterFieldHeightBox.Height = mainCanvas.Height / 25;
            enterFieldHeightBox.FontSize = (int)Math.Min(mainCanvas.Width / 45, mainCanvas.Height / 32);
            margin.Left = 0;
            margin.Top = mainCanvas.Height * 7 / 30;
            enterFieldWidthInfo.Margin = margin;
            enterFieldWidthInfo.Width = mainCanvas.Width / 8;
            enterFieldWidthInfo.Height = mainCanvas.Height / 15;
            enterFieldWidthInfo.FontSize = fontSize;
            margin.Left = mainCanvas.Width / 8;
            margin.Top += mainCanvas.Height / 60;
            enterFieldWidthBox.Margin = margin;
            enterFieldWidthBox.Width = mainCanvas.Width / 20;
            enterFieldWidthBox.Height = mainCanvas.Height / 25;
            enterFieldWidthBox.FontSize = (int)Math.Min(mainCanvas.Width / 45, mainCanvas.Height / 32);
            margin.Left = 0;
            margin.Top = mainCanvas.Height * 0.3;
            enterCordInfo.Margin = margin;
            enterCordInfo.Height = mainCanvas.Height / 9;
            enterCordInfo.Width = mainCanvas.Width / 4;
            enterCordInfo.FontSize = fontSize;
            margin.Top = mainCanvas.Height * 2 / 5;
            margin.Left = mainCanvas.Width / 14;
            lineInfo.Margin = margin;
            lineInfo.Height = mainCanvas.Height / 15;
            lineInfo.Width = mainCanvas.Width / 12;
            lineInfo.FontSize = fontSize;
            margin.Left = mainCanvas.Width * 13 / 84;
            columnInfo.Margin = margin;
            columnInfo.Height = mainCanvas.Height / 15;
            columnInfo.Width = mainCanvas.Width * 2 / 21;
            columnInfo.FontSize = fontSize;
            margin.Top = mainCanvas.Height * 7 / 15;
            margin.Left = 0;
            firstInfo.Margin = margin;
            firstInfo.Height = mainCanvas.Height / 15;
            firstInfo.Width = mainCanvas.Width / 8;
            firstInfo.FontSize = fontSize;
            margin.Left = mainCanvas.Width / 8;
            margin.Top += mainCanvas.Height / 60;
            firstLineBox.Margin = margin;
            firstLineBox.Height = mainCanvas.Height / 25;
            firstLineBox.Width = mainCanvas.Width / 20;
            firstLineBox.FontSize = fontSize;
            margin.Left = mainCanvas.Width * 3 / 16;
            firstColumnBox.Margin = margin;
            firstColumnBox.Height = mainCanvas.Height / 25;
            firstColumnBox.Width = mainCanvas.Width / 20;
            firstColumnBox.FontSize = fontSize;
            margin.Top = mainCanvas.Height * 8 / 15;
            margin.Left = 0;
            secondInfo.Margin = margin;
            secondInfo.Height = mainCanvas.Height / 15;
            secondInfo.Width = mainCanvas.Width / 8;
            secondInfo.FontSize = fontSize;
            margin.Top += mainCanvas.Height / 60;
            margin.Left = mainCanvas.Width / 8;
            secondLineBox.Margin = margin;
            secondLineBox.Height = mainCanvas.Height / 25;
            secondLineBox.Width = mainCanvas.Width / 20;
            secondLineBox.FontSize = fontSize;
            margin.Left = mainCanvas.Width * 3 / 16;
            secondColumnBox.Height = mainCanvas.Height / 25;
            secondColumnBox.Width = mainCanvas.Width / 20;
            secondColumnBox.Margin = margin;
            secondColumnBox.FontSize = fontSize;
            margin.Top = mainCanvas.Height * 3 / 5;
            margin.Left = mainCanvas.Width / 50;
            applySettings.Margin = margin;
            applySettings.Height = mainCanvas.Height / 15;
            applySettings.Width = mainCanvas.Width / 4 * 24 / 25;
            applySettings.FontSize = fontSize;
            margin.Top += mainCanvas.Height / 15;
            startButton.Margin = margin;
            startButton.Height = mainCanvas.Height / 14;
            startButton.Width = mainCanvas.Width * 6 / 25;
            startButton.FontSize = fontSize;
            margin.Top = mainCanvas.Height / 15;
            margin.Left = mainCanvas.Width * 33 / 128;
            hexField.Margin = margin;
            hexField.Height = mainCanvas.Height * 14 / 15;
            hexField.Width = mainCanvas.Width * 95 / 128;
            margin.Top = mainCanvas.Height * 31 / 42;
            margin.Left = mainCanvas.Width / 50;
            backButton.Margin = margin;
            backButton.Height = mainCanvas.Height / 14;
            backButton.Width = mainCanvas.Width * 6 / 25;
            backButton.FontSize = fontSize;
        }
        private void Back(object sender, RoutedEventArgs e)
        {
            mainWindow.SizeChanged -= WindowSizeChanged;
            StartGameMenu startGameMenu = new StartGameMenu() { mainCanvas = mainCanvas, mainWindow = mainWindow };
            startGameMenu.Build();
        }
    }
}