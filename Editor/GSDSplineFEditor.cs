#if UNITY_EDITOR
#region "Imports"
using UnityEditor;
#endregion


[CustomEditor(typeof(GSDSplineF))]
public class GSDSplineFEditor : Editor
{
    protected GSDSplineF splineF { get { return (GSDSplineF) target; } }


    public override void OnInspectorGUI()
    {
        //Intentionally left empty.
    }
}
#endif