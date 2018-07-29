using UnityEngine;

[CreateAssetMenu(fileName="DebugConfig", menuName="Debug Config")]
public class DebugConfig : ScriptableObject 
{
	public Mesh PlayerSpawnMesh;
	public Color PlayerSpawnColor = Color.magenta;
}