using NUnit.Framework;

public class ShapeModuleCacheTests
{
    [SetUp]
    public void SetUp() => ShapeModuleCache.Reset();

    [Test]
    public void SetShape_PersistsShapeName()
    {
        ShapeModuleCache.data.shapeName = "ShapeCube";
        Assert.AreEqual("ShapeCube", ShapeModuleCache.data.shapeName);
    }

    [Test]
    public void SetBehaviour_PersistsBehaviourName()
    {
        ShapeModuleCache.data.behaviourName = "Spin";
        Assert.AreEqual("Spin", ShapeModuleCache.data.behaviourName);
    }

    [Test]
    public void Reset_ClearsAllFields()
    {
        ShapeModuleCache.data.shapeName     = "ShapeSphere";
        ShapeModuleCache.data.behaviourName = "Bounce";
        ShapeModuleCache.Reset();
        Assert.IsNull(ShapeModuleCache.data.shapeName);
        Assert.IsNull(ShapeModuleCache.data.behaviourName);
    }

    [Test]
    public void ResetBehaviour_PreservesShapeNameAndClearsBehaviour()
    {
        ShapeModuleCache.data.shapeName     = "ShapeCylinder";
        ShapeModuleCache.data.behaviourName = "Move";
        ShapeModuleCache.ResetBehaviour();
        Assert.AreEqual("ShapeCylinder", ShapeModuleCache.data.shapeName);
        Assert.IsNull(ShapeModuleCache.data.behaviourName);
    }

    [Test]
    public void DataProperty_NeverReturnsNull()
    {
        Assert.IsNotNull(ShapeModuleCache.data);
    }

    [Test]
    public void ResetThenSet_WorksCorrectly()
    {
        ShapeModuleCache.data.shapeName     = "ShapeCube";
        ShapeModuleCache.data.behaviourName = "Spin";
        ShapeModuleCache.Reset();
        ShapeModuleCache.data.shapeName     = "ShapePyramid";
        ShapeModuleCache.data.behaviourName = "Scale";
        Assert.AreEqual("ShapePyramid", ShapeModuleCache.data.shapeName);
        Assert.AreEqual("Scale",        ShapeModuleCache.data.behaviourName);
    }
}
