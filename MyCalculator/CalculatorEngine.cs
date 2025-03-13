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
        private static int operandsCount = 0;

        public CalculatorEngine()
        {
            _result = 0;
        }

        public void AddDigit(string argNumber)
        {
            CurrentInput += argNumber;
            Display += argNumber;
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
                OnPropertyChanged(nameof(Result));
            }
        }

        public void ClearInput()
        {
            UserInput.Clear();
            CurrentInput = string.Empty;
            Result = 0;
        }

        public void ClearLastInput()
        {
            if (UserInput.Count > 0 && CurrentInput.Equals(""))
            {
                UserInput.RemoveAt(UserInput.Count - 1);
            }
            else 
            { 
                CurrentInput = string.Empty; 
            }
        }

        public void Calculate()
        {
            UserInput.Add(curentUserInput);
            string infix = string.Join(" ", UserInput);
            string postfix = ConvertToPostfix(infix);
            Result = EvaluatePostfix(postfix);
            UserInput.Clear();
            Display =string.Empty;
        }

        public void AddOperator(string argOperator)
        {
            UserInput.Add(argOperator);

            if (CurrentInput == string.Empty)
            {
                Display += formatOperator(argOperator);
            }
            else
            {
                UserInput.Add(curentUserInput);
                Display += " " + formatOperator(argOperator) + " ";
            }
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
                else if (token == "(")
                {
                    operators.Push(token);
                }
                else if (token == ")")
                {
                    while (operators.Count > 0 && operators.Peek() != "(")
                    {
                        output.Add(operators.Pop());
                    }
                    operators.Pop(); 
                }
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
            return token == "+" || token == "-" || token == "*" || token == "/" || token == "pow";
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
                default:
                    throw new InvalidOperationException("Unsupported operator: " + op);
            }
        }
    }
}
