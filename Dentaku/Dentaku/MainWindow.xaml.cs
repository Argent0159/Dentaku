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

namespace Dentaku
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private Calculator _calculator;
        private Func<double, double, double> _basicExpression;
        // private Func<double, double> _singleExpression;

        public MainWindow()
        {
            InitializeComponent();
            _calculator = new Calculator();

            ValueDisplay.DataContext = _calculator;
            CalculationDisplay.DataContext = _calculator;

        }

        public void NumberButtonClicked(object sender,RoutedEventArgs e)
        {
            var pushedButton = (Button)sender;
            var viewBox = (Viewbox)(pushedButton.Content);
            var label = (Label)(viewBox.Child);
            var inputNumber = label.Content.ToString();

            _calculator.InputNumber(inputNumber);
        }

        //通常の四則演算ボタン押下時
        public void OperatorButtonClicked(object sender,RoutedEventArgs e)
        {
            var pushedButton = (Button)sender;
            var buttonInViewBox = (Viewbox)(pushedButton.Content);
            var buttonText = ((Label)(buttonInViewBox.Child)).Content;

            if (_calculator.IsPushedNumber && _calculator.IsPushedOperator)
            {
                _calculator.Run(_basicExpression);
            }

            switch (buttonText)
            {
                case "＋":
                    _basicExpression = (x, y) => x + y;
                    break;
                case "－":
                    _basicExpression = (x, y) => x - y;
                    break;
                case "÷":
                    _basicExpression = (x, y) => x / y;
                    break;
                case "×":
                    _basicExpression = (x, y) => x * y;
                    break;
            }

            _calculator.SaveNumber();
            //_calculator.IsPushedOperator = true;
        }

        //特殊演算ボタン押下時
        public void SpecialOperationClicked(object sender,RoutedEventArgs e)
        {
            var pushedButton = ((Button)sender).Content.ToString();


        }


        public void EqualButtonClicked(object sender, RoutedEventArgs e)
        {
            if (_basicExpression != null)
            {
                _calculator.Equal(_basicExpression);
            }
        }

        public void DecimalPointClicked(object sender, RoutedEventArgs e) => _calculator.AddDecimalPoint();

        public void BackSpaceClickd(object sender, RoutedEventArgs e) => _calculator.BackErase();

        public void ClearClicked(object sender, RoutedEventArgs e) => _calculator.Clear();

        public void ClearEndClicked(object sender, RoutedEventArgs e) => _calculator.ClearEnd();

        public void PlusOrMinusButtonClicked(object sender, RoutedEventArgs e) => _calculator.ChangeSign();

        
    }
}
