using System;
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

namespace Saper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String[,] bombs;
        Button[,] buttons;
        int rows;
        int columns;
        int mines;
        int zajete = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Create_Method(object sender, RoutedEventArgs e)
        {
            
            int x;
            int y;

            Field.RowDefinitions.Clear();
            Field.ColumnDefinitions.Clear();
            rows = Int32.Parse((FindName("Rows") as TextBox).Text);
            columns = Int32.Parse((FindName("Columns") as TextBox).Text);
            mines = Int32.Parse((FindName("Mines") as TextBox).Text);
            bombs = new String[rows,columns];
            buttons = new Button[rows,columns];
            RowDefinition row;
            ColumnDefinition column;
            Button button;
            Set_Mines();
            Fill_Field();
            
            for (int i = 0; i < rows; i++) {
                row = new RowDefinition();
                row.Height = new GridLength(1.0, GridUnitType.Star);
                Field.RowDefinitions.Add(row);
                
            }
            for (int i = 0; i < columns; i++)
            {
                column = new ColumnDefinition();
                Field.ColumnDefinitions.Add(column);
            }
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns;j++) {
                    button = new Button();
                    Field.Children.Add(button);
                    button.Name = $"Button_{i}_{j}";
                    button.Content = "";
                    button.Click += new RoutedEventHandler(Button_Left);
                    button.MouseRightButtonDown += new MouseButtonEventHandler(Button_Right);
                    buttons[i, j] = button;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                }
            }
            
        }

        private void Button_Right(object sender, RoutedEventArgs e)
        {
            var text = (Label)this.FindName("Licznik");
            if ((sender as Button).Content == "?") {
                (sender as Button).Content = "";
                zajete--;
            }
            else
            {
                (sender as Button).Content = "?";
                zajete++;
            }
            text.Content = zajete;
            
        }

        private void Button_Left(object sender, RoutedEventArgs e)
        {
            Button button1 = sender as Button;
            String name = button1.Name;
            String[] split_name = name.Split("_");
            int x = Int32.Parse(split_name[1]);
            int y = Int32.Parse(split_name[2]);
            if (bombs[x, y].Equals("#")) {
                button1.Content = bombs[x, y];
                button1.Background = new SolidColorBrush(Color.FromArgb(200,255, 0, 0));
            }
            else {
                 Find_Zeros(x, y);
                 button1.Content = bombs[Int32.Parse(split_name[1]), Int32.Parse(split_name[2])];
                 button1.Background = new SolidColorBrush(Color.FromArgb(150, 30, 255, 10));
            }            
        }

        private void Set_Mines() {
            if (mines <= columns * rows)
            {
                Random rnd = new Random();
                int x;
                int y;
                for (int m = 0; m < mines; m++)
                {
                    x = rnd.Next(rows);
                    y = rnd.Next(columns);
                    if (Is_Taken(x, y))
                    {
                        m--;
                        continue;
                    }
                    bombs[x, y] = "#";
                }
            }
        }

        private bool Is_Taken(int x,int y) {
            if (bombs[x, y] == "#")
            {
                return true;
            }
            return false;
        }

        private void Fill_Field() {
            int licznik = 0;
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++)
                {
                    if (bombs[i, j] == "#") {
                        continue;
                    }
                    if (i - 1 >= 0) {
                        if (Is_Taken(i - 1, j)) licznik++;
                    }
                    if (i + 1 < rows) {
                        if (Is_Taken(i + 1, j)) licznik++;
                    }
                    if (j - 1 >= 0) {
                        if (Is_Taken(i, j-1)) licznik++;
                    }
                    if (j + 1 < columns) {
                        if (Is_Taken(i, j+1)) licznik++;
                    }
                    if (i - 1 >= 0 && j - 1 >=0) {
                        if (Is_Taken(i - 1, j-1)) licznik++;
                    }
                    if(i-1 >=0 && j+1 < columns) {
                        if (Is_Taken(i - 1, j+1)) licznik++;
                    }
                    if(i+1 <rows && j-1 >= 0) {
                        if (Is_Taken(i + 1, j-1)) licznik++;
                    }
                    if(i+1 < rows && j+1 < columns) {
                        if (Is_Taken(i + 1, j+1)) licznik++;
                    }
                    bombs[i, j] = licznik.ToString();
                    licznik = 0;
                }
            }
        }

        public void Find_Zeros(int x, int y) {
            if (x >= 0 && y >= 0 && x < rows && y < columns) {
                var button = buttons[x, y];
                if (bombs[x, y].Equals("0"))
                {
                    if (button.Content.Equals(""))
                    {
                        button.Background = new SolidColorBrush(Color.FromArgb(150, 30, 255, 10));
                        button.Content = bombs[x, y];
                        Find_Zeros(x + 1, y);
                        Find_Zeros(x - 1, y);
                        Find_Zeros(x, y + 1);
                        Find_Zeros(x, y - 1);
                    }
                }
            }
        }
    }
}
