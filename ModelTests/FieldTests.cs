using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model2048;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model2048_Tests
{
    [TestClass]
    public class FieldTests
    {
        int CountNonZeroValuesInFieldArray(int[,] arr)
        {
            int result = 0;
            for (int i = 0; i < arr.GetLength(0); i++)
                for (int j = 0; j < arr.GetLength(1); j++)
                    if (arr[i, j] != 0)
                        result++;
            return result;
        }
        bool IsCellsListsEqual(List<Coordinates> lst1, List<Coordinates> lst2)
        {
            if (lst1.Count == lst2.Count)
            {
                foreach (Coordinates element in lst1)
                    if (!lst2.Contains(element))
                        return false;
            }
            else
                return false;
            return true;
        }
        [TestMethod]
        public void SingleParamConstructorTest()
        {
            Field field = new Field(3);
            Assert.AreEqual(3, field.HSize);
            Assert.AreEqual(3, field.VSize);
            Assert.IsNotNull(field.ForTestsOnly_FieldArray);
            Assert.AreEqual(0, CountNonZeroValuesInFieldArray(field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void TwoParamsConstructorTest()
        {
            Field field = new Field(3, 2);
            Assert.AreEqual(3, field.HSize);
            Assert.AreEqual(2, field.VSize);
            Assert.IsNotNull(field.ForTestsOnly_FieldArray);
            Assert.AreEqual(0, CountNonZeroValuesInFieldArray(field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ClearTest()
        {
            Field field = new Field(3);
            int[,] fieldData = { { 2, 0, 16 }, { 0, 0, 4 }, { 2, 2, 2048 } };
            field.ForTestsOnly_FieldArray = fieldData;
            field.Clear();
            Assert.AreEqual(0, CountNonZeroValuesInFieldArray(field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void HaveCellWithValueTest_True()
        {
            Field field = new Field(3);
            int[,] fieldData = { { 2, 0, 16 }, { 0, 0, 4 }, { 2, 2, 2048 } };
            field.ForTestsOnly_FieldArray = fieldData;
            Assert.IsTrue(field.HaveCellWithValue(2));
            Assert.IsTrue(field.HaveCellWithValue(4));
            Assert.IsTrue(field.HaveCellWithValue(16));
            Assert.IsTrue(field.HaveCellWithValue(2048));
            Assert.IsTrue(field.HaveCellWithValue(0));
        }
        [TestMethod]
        public void HaveCellWithValueTest_False()
        {
            Field field = new Field(3);
            int[,] fieldData = { { 2, 0, 16 }, { 0, 0, 4 }, { 2, 2, 2048 } };
            field.ForTestsOnly_FieldArray = fieldData;
            Assert.IsFalse(field.HaveCellWithValue(8));
            Assert.IsFalse(field.HaveCellWithValue(32));
        }
        [TestMethod]
        public void FindEmptyCellsTest()
        {
            Field field = new Field(3);
            int[,] fieldData = { { 2, 0, 16 }, { 0, 0, 4 }, { 2, 2, 2048 } };
            field.ForTestsOnly_FieldArray = fieldData;
            List<Coordinates> expected = new List<Coordinates>();
            expected.Add(new Coordinates(0, 1));
            expected.Add(new Coordinates(1, 0));
            expected.Add(new Coordinates(1, 1));
            Assert.IsTrue(IsCellsListsEqual(expected, field.FindEmptyCells()));
        }

    }
}
