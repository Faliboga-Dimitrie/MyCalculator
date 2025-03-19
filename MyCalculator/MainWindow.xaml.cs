using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
        }


        private void Button_Click_Display_Memory(object sender, RoutedEventArgs e)
        {
            if ((DataContext as CalculatorManager).MainGridVisibility == Visibility.Visible)
            {
                (DataContext as CalculatorManager).MainGridVisibility = Visibility.Collapsed;
                (DataContext as CalculatorManager).MemoryGridVisibility = Visibility.Visible;
            }
            else
            {
                (DataContext as CalculatorManager).MainGridVisibility = Visibility.Visible;
                (DataContext as CalculatorManager).MemoryGridVisibility = Visibility.Collapsed;
            }
        }
        private void MyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyListBox.SelectedIndex != -1) // Ensure an item is selected
            {
                int selectedIndex = MyListBox.SelectedIndex; // Get the index
                (DataContext as CalculatorManager).Engine.MemoryIndexChanged(selectedIndex);
            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D0 || e.Key == Key.NumPad0)
            {
                (DataContext as CalculatorManager).Engine.AddDigit("0");
            }
            else if (e.Key == Key.D1 || e.Key == Key.NumPad1)
            {
                (DataContext as CalculatorManager).Engine.AddDigit("1");
            }
            else if (e.Key == Key.D2 || e.Key == Key.NumPad2)
            {
                (DataContext as CalculatorManager).Engine.AddDigit("2");
            }
            else if (e.Key == Key.D3 || e.Key == Key.NumPad3)
            {
                (DataContext as CalculatorManager).Engine.AddDigit("3");
            }
            else if (e.Key == Key.D4 || e.Key == Key.NumPad4)
            {
                (DataContext as CalculatorManager).Engine.AddDigit("4");
            }
            else if (e.Key == Key.D5 || e.Key == Key.NumPad5)
            {
                (DataContext as CalculatorManager).Engine.AddDigit("5");
            }
            else if (e.Key == Key.D6 || e.Key == Key.NumPad6)
            {
                (DataContext as CalculatorManager).Engine.AddDigit("6");
            }
            else if (e.Key == Key.D7 || e.Key == Key.NumPad7)
            {
                (DataContext as CalculatorManager).Engine.AddDigit("7");
            }
            else if (e.Key == Key.D8 || e.Key == Key.NumPad8)
            {
                (DataContext as CalculatorManager).Engine.AddDigit("8");
            }
            else if (e.Key == Key.D9 || e.Key == Key.NumPad9)
            {
                (DataContext as CalculatorManager).Engine.AddDigit("9");
            }
            else if (e.Key == Key.Add)
            {
                (DataContext as CalculatorManager).Engine.AddOperator("+");
            }
            else if (e.Key == Key.Subtract)
            {
                (DataContext as CalculatorManager).Engine.AddOperator("-");
            }
            else if (e.Key == Key.Multiply)
            {
                (DataContext as CalculatorManager).Engine.AddOperator("*");
            }
            else if (e.Key == Key.Divide)
            {
                (DataContext as CalculatorManager).Engine.AddOperator("/");
            }
            else if (e.Key == Key.Enter)
            {
                (DataContext as CalculatorManager).Engine.Calculate();
            }
            else if (e.Key == Key.Back)
            {
                (DataContext as CalculatorManager).Engine.ClearLastDigit();
            }
            else if (e.Key == Key.Delete)
            {
                (DataContext as CalculatorManager).Engine.ClearInput();
            }

            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.C:
                        (DataContext as CalculatorManager).CopyToClipboard();
                        break;
                    case Key.V:
                        (DataContext as CalculatorManager).PasteFromClipboard();
                        break;
                    case Key.X:
                        (DataContext as CalculatorManager).CutToClipboard();
                        break;
                    default:
                        break;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void Button_Click_Operator(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button != null)
            {
                (DataContext as CalculatorManager).Engine.AddOperator(button.Content.ToString());
            }
        }

        private void Button_Click_Memory_Store(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.MemoryStore();
        }

        private void Button_Click_Memory_Clear(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.MemoryClear();
        }

        private void Button_Click_Memory_Retrieve(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.MemoryRecall();
        }

        private void Button_Click_Memory_Subtract(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.MemoryMinus();
        }

        private void Button_Click_Memory_Add(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.MemoryPlus();
        }
        private void Button_Click_Operator_Mul(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.AddOperator("*");
        }

        private void Button_Click_Operator_Div(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.AddOperator("/");
        }

        private void Button_Click_Operator_Pow(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.AddOperator("^");
        }

        private void Button_Click_Operator_Sqrt(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.AddOperator("√");
        }

        private void Button_Click_Operator_One_Over(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.AddOperator("1/");
        }

        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.ClearInput();
        }

        private void Button_Click_Clear_Last(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.ClearLastInput();
        }

        private void Button_Click_Clear_Last_Digit(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.ClearLastDigit();
        }

        private void Button_Click_Equal(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.Calculate();
        }

        private void Button_Click_Add_Digit(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button != null)
            {
                (DataContext as CalculatorManager).Engine.AddDigit(button.Content.ToString());
            }
        }

        private void Button_Click_Add_Dot(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.AddDecimalPoint();
        }

        private void SelectStandard_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).IsStandardMode = true;
        }

        private void SelectProgrammer_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).IsStandardMode = false;
        }

        private void Enable_or_DisableDigitGrouping_Click(object sender, RoutedEventArgs e)
        {
            // Handle SubOption 3 click
            (DataContext as CalculatorManager).Engine.UpdateDigitGrouping();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            // Handle SubOption 4 click
            (DataContext as CalculatorManager).CopyToClipboard();
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            // Handle SubOption 5 click
            (DataContext as CalculatorManager).CutToClipboard();
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            // Handle SubOption 6 click
            (DataContext as CalculatorManager).PasteFromClipboard();
        }

        private void Empty_Click(object sender, RoutedEventArgs e)
        {
            // Handle SubOption 7 click


        }
    }
}