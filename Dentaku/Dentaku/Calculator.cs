using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Dentaku
{
    public class Calculator : INotifyPropertyChanged 
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //プロパティ変更時の通知メソッド
        private void NotifyChanged(string PropertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));

        public Calculator()
        {
            //電卓起動時、初期値は0である
            CurrentNumber = "0";
        }

        //最大入力可能な桁数
        //static readonly int MaxInputableNumber = 16;

        //指数表記に切り替える閾値
        //static readonly long ThresholdValue = 10_000_000_000_000_000;

        //演算子ボタンが押下されているかどうか
        public bool IsPushedOperator { get; private set; } = false;

        //演算子ボタン押下後、数字ボタンが押されたかどうか
        public bool IsPushedNumber { get; private set; } = false;

        //四則演算用の式
        private Func<double, double, double> _basicExpression;

        //保持する値（結果）
        private double _pooledNumber;

        public double PooledNumber
        {
            get => _pooledNumber;
            private set
            {
                _pooledNumber = value;
                NotifyChanged(nameof(PooledNumber));
            }
        }

        //入力した値の保存に使用
        private double _queueNumber = 0;

        //入力中の値
        private string _currentNumber;

        public string CurrentNumber
        {
            get
            {
                var parsedNumber = double.Parse($"{_currentNumber,16}");


                return parsedNumber.ToString("#,0.###############");
            }
            private set
            {
                _currentNumber = value;
                NotifyChanged(nameof(CurrentNumber));
            }
        }

        //値の入力
        public void InputNumber(string Number)
        {
            CurrentNumber = _currentNumber == "0" ? Number : _currentNumber + Number;

            IsPushedNumber = true;
        }

        //値の全消去
        public void Clear()
        {
            ClearEnd();
            PooledNumber = 0;
            _basicExpression = null;

            IsPushedNumber = false;
            IsPushedOperator = false;
        }

        //入力中の値を消去
        public void ClearEnd() => CurrentNumber = "0";

        //1文字消去
        //文字列長が1なら値は0とし、そうでないなら後ろの1文字を削除する
        public void BackErase()
        {
            if (CurrentNumber.Length==1)
            {
                CurrentNumber = "0";
            }
            else
            {
                CurrentNumber = _currentNumber.Remove(_currentNumber.Length - 1);
            }
        }
        
        //符号切り替え
        public void ChangeSign()
        {
            if (CurrentNumber != "0")
            {
                if (CurrentNumber.First() != '-')
                {
                    CurrentNumber = '-' + _currentNumber;
                }
                else
                {
                    CurrentNumber = CurrentNumber.Substring(1);
                }
            }
        }

        //通常の四則演算の実行用
        public void Run()
        {
            IsPushedNumber = false;
            IsPushedOperator = false;

            _queueNumber = double.Parse(CurrentNumber);
            PooledNumber = _basicExpression?.Invoke(PooledNumber, _queueNumber) ?? PooledNumber;
            CurrentNumber = PooledNumber.ToString();
        }

        //演算子の決定
        public void Run(string Operator)
        {
            switch (Operator)
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

            IsPushedOperator = true;

            PooledNumber = double.Parse(CurrentNumber);
            _queueNumber = double.Parse(CurrentNumber);
            _currentNumber = "0";
        }

        //小数点ボタン押下時
        public void AddDecimalPoint()
        {
            CurrentNumber = _currentNumber.Contains('.') ? _currentNumber : _currentNumber + '.';
        }

        //パーセンテージ押下時
        public void CalculatePercentage()
        {
            CurrentNumber = (PooledNumber * (double.Parse(_currentNumber) / 100)).ToString();
        }

        //√ボタン押下時
        public void CalculateSquareRoot()
        {
            CurrentNumber = (Math.Sqrt(double.Parse(_currentNumber))).ToString();
        }
        
        //逆数ボタン押下時
        public void CalculateReverseNumber()
        {
            CurrentNumber = (1 / double.Parse(_currentNumber)).ToString();
        }

        //2乗ボタン押下時
        public void CalculateSquare()
        {
            CurrentNumber = Math.Pow(double.Parse(_currentNumber), 2).ToString();
        }

        //イコールボタン押下時
        public void Equal()
        {
            IsPushedNumber = false;

            _queueNumber = _currentNumber == "0" ? _queueNumber : double.Parse(_currentNumber);

            PooledNumber = _basicExpression?.Invoke(PooledNumber, _queueNumber) ?? PooledNumber;

            CurrentNumber = PooledNumber.ToString();
            _currentNumber = "0";
        }
    }
}
