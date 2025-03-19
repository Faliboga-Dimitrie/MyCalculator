using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyCalculator
{
    class CalculatorEngine : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private double _result;
        private string curentUserInput = string.Empty;
        private string display = string.Empty;
        private List<string> UserInput = new List<string>();

        private ObservableCollection<double> _memoryValue = new ObservableCollection<double>();
        private double _currentValue;
        private int _memoryIndex;

        private bool _isDigitGrouping = true;

        public void MemoryStore()
        {
            MemoryValue = Result;
            MemoryValues.Add(MemoryValue);
            MemoryIndex = MemoryValues.Count - 1;
            OnPropertyChanged(nameof(MemoryValues));
        }

        public void MemoryRecall()
        {
            Result = MemoryValue;
            UserInput.Clear();
            CurrentInput = MemoryValue.ToString();
            Display = MemoryValue.ToString();
            UserInput.Add(MemoryValue.ToString());
        }

        public void MemoryPlus()
        {
            MemoryValue += Result;
            if (MemoryValues.Count > 0)
            {
                MemoryValues[MemoryIndex] = MemoryValue;
            }
            else
            {
                MemoryValues.Add(MemoryValue);
                MemoryIndex = MemoryValues.Count - 1;
            }
            OnPropertyChanged(nameof(MemoryValues));
        }

        public void MemoryMinus()
        {
            MemoryValue -= Result;
            if (MemoryValues.Count > 0)
            {
                MemoryValues[MemoryIndex] = MemoryValue;
            }
            else
            {
                MemoryValues.Add(MemoryValue);
                MemoryIndex = MemoryValues.Count - 1;
            }
            OnPropertyChanged(nameof(MemoryValues));
        }

        public void MemoryClear()
        {
            MemoryValue = 0;
            MemoryValues.Clear();
            MemoryIndex = MemoryValues.Count - 1;
            OnPropertyChanged(nameof(MemoryValues));
        }

        public void MemoryIndexChanged(int index)
        {
            MemoryIndex = index;
            MemoryValue = MemoryValues[index];
            OnPropertyChanged(nameof(MemoryValue));
        }

        public CalculatorEngine()
        {
            _result = 0;
        }

        private void UpdateDisplay()
        {
            if (IsDigitGrouping)
            {
                Display = string.Join(" ", UserInput.Select(FormatNumberIfPossible));
            }
            else
            {
                Display = string.Join(" ", UserInput);
            }
        }

        public void AddDigit(string argNumber)
        {
            CurrentInput += argNumber;

            if (UserInput.Count > 0 && !IsBinaryOperator(UserInput[UserInput.Count - 1]) && !IsUnaryOperator(UserInput[UserInput.Count - 1]))
            {
                UserInput[UserInput.Count - 1] = CurrentInput;
            }
            else
            {
                UserInput.Add(CurrentInput);
            }

            CalculateCascade();

            UpdateDisplay();
        }

        public void AddDecimalPoint()
        {
            if (!CurrentInput.Contains("."))
            {
                CurrentInput += ".";
                if(UserInput.Count > 0 && !IsBinaryOperator(UserInput[UserInput.Count - 1]) && !IsUnaryOperator(UserInput[UserInput.Count - 1]))
                {
                    UserInput[UserInput.Count - 1] = CurrentInput;
                }
                else
                {
                    UserInput.Add(CurrentInput);
                }
                UpdateDisplay();
            }
        }

        private string FormatNumberIfPossible(string input)
        {
            if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
            {
                return number.ToString("N0", CultureInfo.CurrentCulture);
            }
            return input;
        }

        public string Display
        {
            get { return display; }
            set
            {
                display = value;
                OnPropertyChanged(nameof(Display));
            }
        }

        public string CurrentInput
        {
            get { return curentUserInput; }
            set
            {
                curentUserInput = value;
            }
        }

        public double Result
        {
            get { return _result; }
            private set
            {
                _result = value;
                OnPropertyChanged(nameof(DisplayResult));
            }
        }

        public double MemoryValue
        {
            get => _currentValue;
            set
            {
                _currentValue = value;
                OnPropertyChanged(nameof(MemoryValue));
            }
        }

        public int MemoryIndex { get; set; }

        public bool IsDigitGrouping
        {
            get => _isDigitGrouping;
            set
            {
                _isDigitGrouping = value;
                OnPropertyChanged(nameof(DisplayResult));
            }
        }

        public ObservableCollection<double> MemoryValues
        {
            get => _memoryValue;
            set
            {
                _memoryValue = value;
                OnPropertyChanged(nameof(MemoryValues));
            }
        }

        public string DisplayResult
        {
            get { 

                if (_result != (int)_result || !IsDigitGrouping)
                {
                    return _result.ToString();
                }
                return _result.ToString("N0"); 
            }
        }

        public void UpdateDigitGrouping()
        {
            IsDigitGrouping = !IsDigitGrouping;
            UpdateDisplay();
        }

        public void ClearInput()
        {
            UserInput.Clear();
            CurrentInput = string.Empty;
            Display = 0.ToString();
            Result = 0;
        }

        public void ClearLastInput()
        {
            if (UserInput.Count > 0)
            {
                UserInput.RemoveAt(UserInput.Count - 1);
                UpdateDisplay();
            }
            else 
            { 
                Display = 0.ToString();
            }

            CurrentInput = string.Empty;
            Result = 0;
        }

        public void ClearLastDigit()
        {
            if (CurrentInput.Length > 0)
            {
                CurrentInput = CurrentInput.Remove(CurrentInput.Length - 1);
                if(UserInput.Count > 0 && !IsBinaryOperator(UserInput[UserInput.Count - 1]) && !IsUnaryOperator(UserInput[UserInput.Count - 1]))
                {
                    UserInput[UserInput.Count - 1] = CurrentInput;
                }
                else
                {
                    UserInput.Add(CurrentInput);
                }

                if (CurrentInput.Length == 0)
                {
                    UserInput.RemoveAt(UserInput.Count - 1);
                }
                else
                {
                    CalculateCascade();
                }
                UpdateDisplay();
            }
        }

        private void CalculateCascade()
        {
            string infix = string.Join(" ", UserInput);
            string postfix = ConvertToPostfix(infix);
            Result = EvaluatePostfix(postfix);
        }

        public void Calculate()
        {
            string infix = string.Join(" ", UserInput);
            string postfix = ConvertToPostfix(infix);
            Result = EvaluatePostfix(postfix);
            UserInput.Clear();
            Display = string.Empty;
            CurrentInput = string.Empty;
        }

        private bool CheckOperatorAdd(string argOperator)
        {

            if (argOperator == "√" || argOperator == "1/")
            {
                return true;
            }

            if (UserInput.Count > 0 && IsBinaryOperator(UserInput[UserInput.Count - 1]))
            {
                return false;
            }
            return true;
        }

        public void AddOperator(string argOperator)
        {
            if(UserInput.Count == 0 && CurrentInput == string.Empty && argOperator != "-" && !IsUnaryOperator(argOperator))
            {
                return;
            }
            else if (CurrentInput == string.Empty && argOperator == "-")
            {
                CurrentInput = "-";
                Display += " -";
                return;
            }

            if (CurrentInput == string.Empty)
            {
                if (!CheckOperatorAdd(argOperator))
                {
                    return;
                }
                Display += argOperator;
            }
            else
            {
                if (!CheckOperatorAdd(argOperator))
                {
                    return;
                }
                Display += " " + argOperator + " ";
            }
            UserInput.Add(argOperator);
            CurrentInput = string.Empty;
        }

        private static string ConvertToPostfix(string infix)
        {
            var output = new List<string>();
            var operators = new Stack<string>();
            var tokens = infix.Split(' ');

            for (int i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                if (token == "") 
                    continue;

                if (double.TryParse(token, out _))
                {
                    output.Add(token);
                }
                //else if (token == "(")
                //{
                //    operators.Push(token);
                //}
                //else if (token == ")")
                //{
                //    while (operators.Count > 0 && operators.Peek() != "(")
                //    {
                //        output.Add(operators.Pop());
                //    }
                //    operators.Pop(); 
                //}
                else 
                {
                    bool isUnary = (i == 0 || IsBinaryOperator(tokens[i - 1])) && (token == "sqrt" || token == "1/x" || token == "-");

                    if (isUnary)
                    {
                        operators.Push(token);
                    }
                    else
                    {
                        while (operators.Count > 0 && Precedence(token) <= Precedence(operators.Peek()))
                        {
                            output.Add(operators.Pop());
                        }
                        operators.Push(token);
                    }
                }
            }

            while (operators.Count > 0)
            {
                output.Add(operators.Pop());
            }

            return string.Join(" ", output);
        }

        private static bool IsBinaryOperator(string token)
        {
            return token == "+" || token == "-" || token == "*"|| token == "/" || token == "^" || token == "%";
        }

        private static int Precedence(string op)
        {
            switch (op)
            {
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                case "%":
                    return 2;
                case "^":
                    return 3;
                case "√":
                case "1/":
                    return 4;
                default:
                    return 0;
            }
        }

        public static double EvaluatePostfix(string postfix)
        {
            var stack = new Stack<double>();
            var tokens = postfix.Split(' ');

            foreach (var token in tokens)
            {
                if (double.TryParse(token, out double number))
                {
                    stack.Push(number);
                }
                else if (IsUnaryOperator(token))
                {
                    double operand = stack.Pop();
                    double result = PerformUnaryOperation(operand, token);
                    stack.Push(result);
                }
                else
                {
                    double rightOperand = stack.Pop();
                    double leftOperand = stack.Pop();
                    double result = PerformOperation(leftOperand, rightOperand, token);
                    stack.Push(result);
                }
            }

            return stack.Pop();
        }

        private static bool IsUnaryOperator(string token)
        {
            return token == "√" || token == "1/";
        }

        private static double PerformUnaryOperation(double operand, string op)
        {
            switch (op)
            {
                case "√":
                    return Math.Sqrt(operand);
                case "1/":
                    return 1 / operand;
                default:
                    throw new InvalidOperationException("Unsupported unary operator: " + op);
            }
        }

        private static double PerformOperation(double left, double right, string op)
        {
            switch (op)
            {
                case "+":
                    return left + right;
                case "-":
                    return left - right;
                case "*":
                    return left * right;
                case "/":
                    return left / right;
                case "^":
                    return Math.Pow(left, right);
                case "%":
                    return left *(right/100);
                default:
                    throw new InvalidOperationException("Unsupported operator: " + op);
            }
        }
    }
}
