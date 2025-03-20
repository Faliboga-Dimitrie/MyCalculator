using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyCalculator
{
    class CalculatorManager : INotifyPropertyChanged
    {
        private CalculatorEngine _engine;
        private bool _isProgrammerMode = false;
        private string _userClipboard = string.Empty;

        private Visibility _mainGridVisibility = Visibility.Visible;
        private Visibility _memoryGridVisibility = Visibility.Collapsed;
        private Visibility _baseSelection = Visibility.Collapsed;
        private Visibility _hexadecimalToggle = Visibility.Collapsed;
        private Visibility _hexadecimaBaseButtons = Visibility.Collapsed;

        public Visibility MainGridVisibility
        {
            get => _mainGridVisibility;
            set
            {
                _mainGridVisibility = value;
                OnPropertyChanged(nameof(MainGridVisibility));
            }
        }

        public Visibility MemoryGridVisibility
        {
            get => _memoryGridVisibility;
            set
            {
                _memoryGridVisibility = value;
                OnPropertyChanged(nameof(MemoryGridVisibility));
            }
        }

        public Visibility BaseSelection
        {
            get => _baseSelection;
            set
            {
                _baseSelection = value;
                OnPropertyChanged(nameof(BaseSelection));
            }
        }

        public Visibility HexadecimaBaseButtons
        {
            get => _hexadecimaBaseButtons;
            set
            {
                _hexadecimaBaseButtons = value;
                OnPropertyChanged(nameof(HexadecimaBaseButtons));
            }
        }

        public Visibility HexaToggle
        {
            get => _hexadecimalToggle;
            set
            {
                _hexadecimalToggle = value;
                OnPropertyChanged(nameof(HexaToggle));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public CalculatorEngine Engine
        {
            get { return _engine; }
        }

        public bool IsProgrammerMode { get => _isProgrammerMode; set => _isProgrammerMode = value; }

        public string UserClipboard { get => _userClipboard; set => _userClipboard = value; }

        public void CopyToClipboard()
        {
            UserClipboard = Engine.CurrentInput;
        }

        public void PasteFromClipboard()
        {
            Engine.AddDigit(UserClipboard);
        }

        public void CutToClipboard()
        {
            CopyToClipboard();
            Engine.ClearLastInput();
        }

        public CalculatorManager()
        {
            _engine = new CalculatorEngine();
        }
    }
}
