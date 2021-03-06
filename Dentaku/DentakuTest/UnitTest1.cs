﻿using System;
using Dentaku;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DentakuTest
{
    [TestClass]
    public class UnitTest1
    {
        //番号入力
        [TestMethod]
        public void AddNumber()
        {
            var target = new Calculator();

            target.InputNumber("1");

            Assert.AreEqual(target.CurrentNumber, "1");

            target.InputNumber("2");

            Assert.AreEqual(target.CurrentNumber, "12");
        }

        //末尾番号の消去
        [TestMethod]
        public void NumberErase()
        {
            var target = new Calculator();

            target.InputNumber("123");
            target.BackErase();

            Assert.AreEqual(target.CurrentNumber, "12");

            target.BackErase();
            target.BackErase();

            Assert.AreEqual(target.CurrentNumber, "0");
        }

        //符号の付け外し
        [TestMethod]
        public void ChangeSign()
        {
            var target = new Calculator();

            target.ChangeSign();

            Assert.AreNotEqual(target.CurrentNumber,"-0");

            target.InputNumber("123");
            target.ChangeSign();

            Assert.AreEqual(target.CurrentNumber, "-123");

            target.ChangeSign();

            Assert.AreEqual(target.CurrentNumber, "123");
        }
        

    }
}
