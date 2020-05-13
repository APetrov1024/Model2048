using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model2048;
using System.Globalization;

namespace ModelTests
{
    [TestClass]
    public class ModelTests
    {
        int CountNonZeroValuesOnField(Field field)
        {
            int result = 0;
            for (int i = 0; i < field.HSize; i++)
                for (int j = 0; j < field.VSize; j++)
                    if (field.Get(new Coordinates(i, j)) != 0)
                        result++;
            return result;
        }
        [TestMethod]
        public void SingleParamConstructorTest()
        {
            Model model = new Model(3);
            Assert.AreEqual(3, model.HSize);
            Assert.AreEqual(3, model.VSize);
            Assert.AreEqual(2, CountNonZeroValuesOnField(model.ForTestsOnly_Field));
        }
        [TestMethod]
        public void TwoParamsConstructorTest()
        {
            Model model = new Model(3, 2);
            Assert.AreEqual(3, model.HSize);
            Assert.AreEqual(2, model.VSize);
            Assert.AreEqual(2, CountNonZeroValuesOnField(model.ForTestsOnly_Field));
        }


    }
}
