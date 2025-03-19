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
        private bool _isStandardMode = true;
        private string _userClipboard = string.Empty;

        private Visibility _mainGridVisibility = Visibility.Visible;
        private Visibility _memoryGridVisibility = Visibility.Collapsed;

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public CalculatorEngine Engine
        {
            get { return _engine; }
        }

        public bool IsStandardMode { get => _isStandardMode; set => _isStandardMode = value; }

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
