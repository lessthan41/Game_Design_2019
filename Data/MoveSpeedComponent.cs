using System;
using Unity.Entities;

// move speed value (can be separate into X axis and Z axis)
[Serializable]
public struct MoveSpeed : IComponentData
{
	public float ValueX;
	public float ValueZ;

	// negate X axis speed value
	public void switchDirection (float Position)
	{
		// prevent negate for many time in Update()
		if (Position * ValueX > 0f)
		{
			ValueX = -ValueX;
		}
	}
}
