using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Dentaku
{
    class Calculator : INotifyPropertyChanged 
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
        static readonly int MaxInputableNumber = 16;

        //指数表記に切り替える閾値
        static readonly long ThresholdValue = 10_000_000_000_000_000;

        //演算子ボタンが押下されているかどうか
        public bool IsPushedOperator { get; private set; } = false;

        //演算子ボタン押下後、数字ボタンが押されたかどうか
        public bool IsPushedNumber { get; private set; } = false;


        //保持する値（結果）
        private decimal _pooledNumber;

        public decimal PooledNumber
        {
            get => _pooledNumber;
            private set
            {
                _pooledNumber = value;
                NotifyChanged(nameof(PooledNumber));
            }
        }

        //入力した値の保存に使用
        private decimal _queueNumber = 0;

        //入力中の値
        private string _currentNumber;

        public string CurrentNumber
        {
            get
            {
                var parsedNumber = decimal.Parse($"{_currentNumber,16}");


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
                CurrentNumber.Remove(CurrentNumber.Length - 1);
            }
        }
        
        //符号切り替え
        public void ChangeSign()
        {
            if (CurrentNumber != "0" && CurrentNumber.First() != '-')
            {
                if (CurrentNumber.First() != '-')
                {
                    CurrentNumber += '-';
                }
                else
                {
                    CurrentNumber = CurrentNumber.Substring(1);
                }
            }
        }

        //入力中の値を保存する
        public void SaveNumber()
        {
            IsPushedOperator = true;

            PooledNumber = decimal.Parse(CurrentNumber);
            _queueNumber = decimal.Parse(CurrentNumber);
            _currentNumber = "0";
        }

        //通常の四則演算の実行用
        public void Run(Func<decimal, decimal, decimal> func)
        {
            IsPushedNumber = false;
            IsPushedOperator = false;

            _queueNumber = decimal.Parse(CurrentNumber);
            PooledNumber = func(PooledNumber, _queueNumber);
            CurrentNumber = PooledNumber.ToString();
        }

        //小数点ボタン押下時
        public void AddDecimalPoint()
        {
            CurrentNumber = _currentNumber.Contains('.') ? _currentNumber : _currentNumber + '.';
        }

        //パーセンテージ押下時
        public void CalculatePercentage()
        {
            CurrentNumber = (PooledNumber * (decimal.Parse(_currentNumber) / 100)).ToString();
        }

        //√ボタン押下時
        public void CalculateSquareRoot()
        {
            CurrentNumber = (Math.Sqrt(double.Parse(_currentNumber))).ToString();
        }
        
        //逆数ボタン押下時
        public void CalculateReverseNumber()
        {
            CurrentNumber = (1 / decimal.Parse(_currentNumber)).ToString();
        }

        //2乗ボタン押下時
        public void CalculateSquare()
        {
            CurrentNumber = Math.Pow(double.Parse(_currentNumber), 2).ToString();
        }

        //イコールボタン押下時
        public void Equal(Func<decimal, decimal, decimal> func)
        {
            IsPushedNumber = false;

            _queueNumber = _currentNumber == "0" ? _queueNumber : decimal.Parse(_currentNumber);

            PooledNumber = func(PooledNumber, _queueNumber);

            CurrentNumber = PooledNumber.ToString();
            _currentNumber = "0";
        }
    }
}
