using System;
using Unity.Entities;

[Serializable]
public struct MoveSpeed : IComponentData
{
	public float ValueX;
	public float ValueZ;

	public void switchDirection (float Position)
	{
		if (Position * ValueX > 0f)
		{
			ValueX = -ValueX;
		}
	}
}
