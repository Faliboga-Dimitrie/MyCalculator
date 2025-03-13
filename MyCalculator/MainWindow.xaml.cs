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
            (DataContext as CalculatorManager).Engine.AddOperator("pow");
        }

        private void Button_Click_Operator_Sqrt(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.AddOperator("sqrt");
        }

        private void Button_Click_Operator_One_Over(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.AddOperator("1/x");
        }

        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.ClearInput();
        }

        private void Button_Click_Clear_Last(object sender, RoutedEventArgs e)
        {
            (DataContext as CalculatorManager).Engine.ClearLastInput();
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
    }
}