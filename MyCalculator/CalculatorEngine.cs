using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public CalculatorEngine()
        {
            _result = 0;
        }

        public void AddDigit(string argNumber)
        {
            CurrentInput += argNumber;

            if (double.TryParse(CurrentInput, out double number))
            {
                if(number != (int)number)
                {
                    Display = string.Join(" ", UserInput) + " " + number.ToString();
                }
                else
                {
                    Display = string.Join(" ", UserInput) + " " + number.ToString("N0");
                }
            }
            else
            {
                Display = CurrentInput;
            }
        }

        public void AddDecimalPoint()
        {
            if (!CurrentInput.Contains("."))
            {
                CurrentInput += ".";
                Display = string.Join(" ", UserInput) + " " + CurrentInput;
            }
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

        public string DisplayResult
        {
            get { 
                if (_result != (int)_result)
                {
                    return _result.ToString();
                }
                return _result.ToString("N0"); 
            }
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
                Display = string.Join(" ", UserInput);
            }
            else 
            { 
                Display = 0.ToString();
            }

            CurrentInput = string.Empty;
        }

        public void ClearLastDigit()
        {
            if (CurrentInput.Length > 0)
            {
                CurrentInput = CurrentInput.Remove(CurrentInput.Length - 1);
                Display = string.Join(" ", UserInput) + " " + CurrentInput;
            }
        }

        public void Calculate()
        {
            UserInput.Add(CurrentInput);
            string infix = string.Join(" ", UserInput);
            string postfix = ConvertToPostfix(infix);
            Result = EvaluatePostfix(postfix);
            UserInput.Clear();
            Display = string.Empty;
            CurrentInput = string.Empty;
        }

        private bool CheckOperatorAdd(string argOperator)
        {

            if (argOperator == "sqrt" || argOperator == "1/x")
            {
                return true;
            }

            if (UserInput.Count > 0 && IsOperator(UserInput[UserInput.Count - 1]))
            {
                return false;
            }
            return true;
        }

        public void AddOperator(string argOperator)
        {
            if(UserInput.Count == 0 && CurrentInput == string.Empty && argOperator != "-")
            {
                return;
            }
            else if (UserInput.Count == 0 && CurrentInput == string.Empty && argOperator == "-")
            {
                CurrentInput = "-";
                Display = CurrentInput;
                return;
            }

            if (CurrentInput == string.Empty)
            {
                if (!CheckOperatorAdd(argOperator))
                {
                    return;
                }
                Display += formatOperator(argOperator);
            }
            else
            {
                UserInput.Add(curentUserInput);
                if (!CheckOperatorAdd(argOperator))
                {
                    return;
                }
                Display += " " + formatOperator(argOperator) + " ";
            }
            UserInput.Add(argOperator);
            CurrentInput = string.Empty;
        }

        private static string formatOperator(string op)
        {
            switch (op)
            {
                case "pow":
                    return "^";
                case "sqrt":
                    return "√";
                case "1/x":
                    return "1/";
                default:
                    return op;
            }
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
                    bool isUnary = (i == 0 || IsOperator(tokens[i - 1])) && (token == "sqrt" || token == "1/x" || token == "-");

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

        private static bool IsOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/" || token == "pow" || token == "%";
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
                case "pow":
                    return 3;
                case "sqrt":
                case "1/x":
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
            return token == "sqrt" || token == "1/x";
        }

        private static double PerformUnaryOperation(double operand, string op)
        {
            switch (op)
            {
                case "sqrt":
                    return Math.Sqrt(operand);
                case "1/x":
                    if (operand == 0) throw new DivideByZeroException("Division by zero is not allowed.");
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
                    if (right == 0) throw new DivideByZeroException("Division by zero is not allowed.");
                    return left / right;
                case "pow":
                    return Math.Pow(left, right);
                case "%":
                    return left % right;
                default:
                    throw new InvalidOperationException("Unsupported operator: " + op);
            }
        }
    }
}
