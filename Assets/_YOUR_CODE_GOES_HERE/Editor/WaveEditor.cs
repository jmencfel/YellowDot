
using UnityEditor;
public class WaveEditor : EditorWindow
{ 
    [MenuItem("Waves/Generate from Scene view")]
    public static void Generate()
    {
        FindObjectOfType<WavesManager>().GenerateWave();
    }
}







