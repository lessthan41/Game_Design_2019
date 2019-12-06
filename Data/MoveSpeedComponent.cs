using System;
using Unity.Entities;

[Serializable]
public struct MoveSpeed : IComponentData
{
	public float Value;

	public void switchDirection (float Position)
	{
		if (Position * Value > 0f)
		{
			Value = -Value;
		}
	}
}
