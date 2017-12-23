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
        private Func<decimal, decimal, decimal> _basicExpression;

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
            var buttonText = ((Label)(buttonInViewBox.Child)).Content.ToString();

            if (_calculator.IsPushedNumber && _calculator.IsPushedOperator)
            {
                _calculator.Run();
            }

            _calculator.Run(buttonText);

        }

        //特殊演算ボタン押下時
        public void SpecialOperationClicked(object sender,RoutedEventArgs e)
        {
            var pushedButton = ((Button)sender).Content.ToString();


        }


        public void EqualButtonClicked(object sender, RoutedEventArgs e) => _calculator.Equal();

        public void DecimalPointClicked(object sender, RoutedEventArgs e) => _calculator.AddDecimalPoint();

        public void BackSpaceClickd(object sender, RoutedEventArgs e) => _calculator.BackErase();

        public void ClearClicked(object sender, RoutedEventArgs e) => _calculator.Clear();

        public void ClearEndClicked(object sender, RoutedEventArgs e) => _calculator.ClearEnd();

        public void PlusOrMinusButtonClicked(object sender, RoutedEventArgs e) => _calculator.ChangeSign();

        public void PercentageClicked(object sender, RoutedEventArgs e) => _calculator.CalculatePercentage();

        public void SquareRootClicked(object sender, RoutedEventArgs e) => _calculator.CalculateSquareRoot();

        public void SquareClicked(object sender, RoutedEventArgs e) => _calculator.CalculateSquare();

        public void ReverseClicked(object sender, RoutedEventArgs e) => _calculator.CalculateReverseNumber();
    }
}
