using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model2048;
using System;
using System.Globalization;

namespace ModelTests
{
    [TestClass]
    public class ModelTests
    {
        #region local_methods
        int CountNonZeroValuesOnField(Field field)
        {
            int result = 0;
            for (int i = 0; i < field.HSize; i++)
                for (int j = 0; j < field.VSize; j++)
                    if (field.Get(new Coordinates(i, j)) != 0)
                        result++;
            return result;
        }

        bool IsFieldArraysEqual(int[,] arr1, int[,] arr2)
        {
            if (arr1.GetLength(0) == arr2.GetLength(0) && arr1.GetLength(1) == arr2.GetLength(1))
            {
                for (int i = 0; i < arr1.GetLength(0); i++)
                    for (int j = 0; j < arr1.GetLength(1); j++)
                        if (arr1[i, j] != arr2[i, j])
                             return false;
             }
             else
                 return false;
            return true;
        }
        #endregion
        #region constructors
        [TestMethod]
        public void SingleParamConstructorTest()
        {
            Model model = new Model(3);
            Assert.AreEqual(3, model.HSize);
            Assert.AreEqual(3, model.VSize);
            Assert.AreEqual(2, CountNonZeroValuesOnField(model.ForTestsOnly_Field));
            Assert.AreEqual(0, model.Score);
        }
        [TestMethod]
        public void TwoParamsConstructorTest()
        {
            Model model = new Model(3, 2);
            Assert.AreEqual(3, model.HSize);
            Assert.AreEqual(2, model.VSize);
            Assert.AreEqual(2, CountNonZeroValuesOnField(model.ForTestsOnly_Field));
            Assert.AreEqual(0, model.Score);
        }
        #endregion
        #region IsHaveValue
        [TestMethod]
        public void IsHaveValueTest_True()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 2, 0, 16 }, { 0, 0, 4 }, { 2, 2, 2048 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            Assert.IsTrue(model.IsHaveValue(0));
            Assert.IsTrue(model.IsHaveValue(2));
            Assert.IsTrue(model.IsHaveValue(4));
            Assert.IsTrue(model.IsHaveValue(16));
            Assert.IsTrue(model.IsHaveValue(2048));
        }
        [TestMethod]
        public void IsHaveValueTest_False()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 1024, 32, 16 }, { 16, 32, 64 }, { 128, 256, 512 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            Assert.IsFalse(model.IsHaveValue(0));
            Assert.IsFalse(model.IsHaveValue(2));
            Assert.IsFalse(model.IsHaveValue(4));
            Assert.IsFalse(model.IsHaveValue(2048));
        }
        #endregion
        #region Action_Down
        [TestMethod]
        public void ActionTest_Down_NewTileGeneratedIfTilesMoved()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 2, 0, 0 }, { 0, 2, 0 }, { 0, 0, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Down);
            int newTileValue = model.Get(model.LastGeneratedTileCoords);
            Assert.IsTrue(newTileValue == 2 || newTileValue == 4);
        }
        [TestMethod]
        public void ActionTest_Down_NewTileNotGeneratedIfTilesNotMoved()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 0, 0, 2 }, { 0, 0, 2 }, { 0, 0, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Down);
            Assert.IsTrue(IsFieldArraysEqual(fieldData, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Down_WithoutMerges()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 2, 0, 4 }, { 0, 2, 0 }, { 4, 2, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Down);
            model.ForTestsOnly_Field.Set(model.LastGeneratedTileCoords, 0);
            int[,] expected = { { 0, 2, 4 }, { 0, 0, 2 }, { 0, 4, 2 } };
            Assert.IsTrue(IsFieldArraysEqual(expected, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Down_WithMerges()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 64, 0, 64 }, { 4, 2, 2 }, { 2, 2, 4 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Down);
            model.ForTestsOnly_Field.Set(model.LastGeneratedTileCoords, 0);
            int[,] expected = { { 0, 0, 128 }, { 0, 4, 4 }, { 0, 4, 4 } };
            Assert.IsTrue(IsFieldArraysEqual(expected, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Down_WithoutMoves()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 0, 0, 64 }, { 0, 4, 2 }, { 2, 8, 4 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Down);
            Assert.IsTrue(IsFieldArraysEqual(fieldData, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Down_WithTwoMerges()
        {
            Model model = new Model(4);
            int[,] fieldData = { { 2, 2, 2, 2 }, { 4, 4, 8, 8 }, { 0, 0, 0, 0}, { 0, 0, 0, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Down);
            model.ForTestsOnly_Field.Set(model.LastGeneratedTileCoords, 0);
            int[,] expected = { { 0, 0, 4, 4 }, { 0, 0, 8, 16 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
            Assert.IsTrue(IsFieldArraysEqual(expected, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Down_WithMerges_ScoresUpdated()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 2, 2, 2 }, { 4, 4, 8 }, { 0, 0, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Down);
            Assert.AreEqual(12, model.Score);
        }
        #endregion
        #region Action_Up
        [TestMethod]
        public void ActionTest_Up_NewTileGeneratedIfTilesMoved()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 0, 0, 2 }, { 0, 2, 0 }, { 0, 0, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Up);
            int newTileValue = model.Get(model.LastGeneratedTileCoords);
            Assert.IsTrue(newTileValue == 2 || newTileValue == 4);
        }
        [TestMethod]
        public void ActionTest_Up_NewTileNotGeneratedIfTilesNotMoved()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 2, 0, 0 }, { 2, 0, 0 }, { 0, 0, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Up);
            Assert.IsTrue(IsFieldArraysEqual(fieldData, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Up_WithoutMerges()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 2, 0, 4 }, { 0, 2, 0 }, { 4, 2, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Up);
            model.ForTestsOnly_Field.Set(model.LastGeneratedTileCoords, 0);
            int[,] expected = { { 2, 4, 0 }, { 2, 0, 0 }, { 4, 2, 0 } };
            Assert.IsTrue(IsFieldArraysEqual(expected, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Up_WithMerges()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 64, 0, 64 }, { 4, 2, 2 }, { 2, 2, 4 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Up);
            model.ForTestsOnly_Field.Set(model.LastGeneratedTileCoords, 0);
            int[,] expected = { { 128, 0, 0 }, { 4, 4, 0 }, { 4, 4, 0 } };
            Assert.IsTrue(IsFieldArraysEqual(expected, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Up_WithoutMoves()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 64, 0, 0 }, { 4, 2, 0 }, { 2, 8, 4 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Up);
            Assert.IsTrue(IsFieldArraysEqual(fieldData, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Up_WithTwoMerges()
        {
            Model model = new Model(4);
            int[,] fieldData = { { 2, 2, 2, 2 }, { 4, 4, 8, 8 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Up);
            model.ForTestsOnly_Field.Set(model.LastGeneratedTileCoords, 0);
            int[,] expected = { { 4, 4, 0, 0 }, { 8, 16, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
            Assert.IsTrue(IsFieldArraysEqual(expected, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Up_WithMerges_ScoresUpdated()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 2, 2, 2 }, { 4, 4, 8 }, { 0, 0, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Up);
            Assert.AreEqual(12, model.Score);
        }
        #endregion
        #region Action_Left
        [TestMethod]
        public void ActionTest_Left_NewTileGeneratedIfTilesMoved()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 2, 0, 0 }, { 0, 2, 0 }, { 0, 0, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Left);
            int newTileValue = model.Get(model.LastGeneratedTileCoords);
            Assert.IsTrue(newTileValue == 2 || newTileValue == 4);
        }
        [TestMethod]
        public void ActionTest_Left_NewTileNotGeneratedIfTilesNotMoved()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 2, 2, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Left);
            Assert.IsTrue(IsFieldArraysEqual(fieldData, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Left_WithoutMerges()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 2, 0, 4 }, { 0, 2, 0 }, { 4, 8, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Left);
            model.ForTestsOnly_Field.Set(model.LastGeneratedTileCoords, 0);
            int[,] expected = { { 2, 2, 4 }, { 4, 8, 0 }, { 0, 0, 0 } };
            Assert.IsTrue(IsFieldArraysEqual(expected, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Left_WithMerges()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 64, 2, 4 }, { 0, 2, 2 }, { 64, 4, 2 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Left);
            model.ForTestsOnly_Field.Set(model.LastGeneratedTileCoords, 0);
            int[,] expected = { { 128, 4, 4 }, { 0, 4, 4 }, { 0, 0, 0 } };
            Assert.IsTrue(IsFieldArraysEqual(expected, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Left_WithoutMoves()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 64, 8, 4 }, { 0, 4, 8 }, { 0, 16, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Left);
            Assert.IsTrue(IsFieldArraysEqual(fieldData, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Left_WithTwoMerges()
        {
            Model model = new Model(4);
            int[,] fieldData = { { 2, 4, 0, 0 }, { 2, 4, 0, 0 }, { 2, 8, 0, 0 }, { 2, 8, 0, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Left);
            model.ForTestsOnly_Field.Set(model.LastGeneratedTileCoords, 0);
            int[,] expected = { { 4, 8, 0, 0 }, { 4, 16, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
            Assert.IsTrue(IsFieldArraysEqual(expected, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Left_WithMerges_ScoresUpdated()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 2, 4, 0 }, { 2, 4, 0 }, { 0, 0, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Left);
            Assert.AreEqual(12, model.Score);
        }
        #endregion
        #region Action_Right
        [TestMethod]
        public void ActionTest_Right_NewTileGeneratedIfTilesMoved()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 2, 0, 0 }, { 0, 2, 0 }, { 0, 0, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Right);
            int newTileValue = model.Get(model.LastGeneratedTileCoords);
            Assert.IsTrue(newTileValue == 2 || newTileValue == 4);
        }
        [TestMethod]
        public void ActionTest_Right_NewTileNotGeneratedIfTilesNotMoved()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 0, 0, 0 }, { 0, 0, 0 }, { 2, 2, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Right);
            Assert.IsTrue(IsFieldArraysEqual(fieldData, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Right_WithoutMerges()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 2, 2, 0 }, { 0, 0, 2 }, { 4, 0, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Right);
            model.ForTestsOnly_Field.Set(model.LastGeneratedTileCoords, 0);
            int[,] expected = { { 0, 0, 0 }, { 2, 0, 0 }, { 4, 2, 2 } };
            Assert.IsTrue(IsFieldArraysEqual(expected, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Right_WithMerges()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 64, 2, 4 }, { 0, 2, 2 }, { 64, 4, 2 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Right);
            model.ForTestsOnly_Field.Set(model.LastGeneratedTileCoords, 0);
            int[,] expected = { { 0, 0, 0 }, { 0, 4, 4 }, { 128, 4, 4 } };
            Assert.IsTrue(IsFieldArraysEqual(expected, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Right_WithoutMoves()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 0, 0, 64 }, { 0, 4, 2 }, { 2, 8, 4 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Right);
            Assert.IsTrue(IsFieldArraysEqual(fieldData, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Right_WithTwoMerges()
        {
            Model model = new Model(4);
            int[,] fieldData = { { 2, 4, 0, 0 }, { 2, 4, 0, 0 }, { 2, 8, 0, 0 }, { 2, 8, 0, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Right);
            model.ForTestsOnly_Field.Set(model.LastGeneratedTileCoords, 0);
            int[,] expected = { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 4, 8, 0, 0 }, { 4, 16, 0, 0 } };
            Assert.IsTrue(IsFieldArraysEqual(expected, model.ForTestsOnly_Field.ForTestsOnly_FieldArray));
        }
        [TestMethod]
        public void ActionTest_Right_WithMerges_ScoresUpdated()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 2, 4, 0 }, { 2, 4, 0 }, { 0, 0, 0 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.Action(Model.Directions.Right);
            Assert.AreEqual(12, model.Score);
        }
        #endregion
        #region ClearField
        [TestMethod]
        public void ClearFieldTest()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 64, 0, 64 }, { 4, 2, 2 }, { 2, 2, 4 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            model.ClearField();
            Assert.AreEqual(2, CountNonZeroValuesOnField(model.ForTestsOnly_Field));
            Assert.AreEqual(0, model.Score);
        }
        #endregion
        #region IsHasNotMoves
        [TestMethod]
        public void IsHasNotMovesTest_True()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 2, 4, 8 }, { 16, 32, 64 }, { 128, 256, 512 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            Assert.IsTrue(model.IsHasNotMoves());
        }
        [TestMethod]
        public void IsHasNotMovesTest_False()
        {
            Model model = new Model(3);
            int[,] fieldData = { { 2, 4, 8 }, { 2, 32, 64 }, { 128, 256, 512 } };
            model.ForTestsOnly_Field.ForTestsOnly_FieldArray = fieldData;
            Assert.IsFalse(model.IsHasNotMoves());
        }
        #endregion

    }
}
