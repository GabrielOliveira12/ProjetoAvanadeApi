namespace Test;

[TestClass]
public class SampleTest
{
    [TestMethod]
    public void SampleTestMethod()
    {
        var expected = true;
        
        var actual = true;
        
        Assert.AreEqual(expected, actual);
    }
}
