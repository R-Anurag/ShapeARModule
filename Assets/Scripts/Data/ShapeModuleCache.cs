public static class ShapeModuleCache
{
    private static ShapeModuleData _data;
    public static ShapeModuleData data => _data ??= new ShapeModuleData();

    public static void Reset()         => _data = new ShapeModuleData();
    public static void ResetBehaviour() => data.behaviourName = null;
}
