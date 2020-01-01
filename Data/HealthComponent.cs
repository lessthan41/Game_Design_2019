using System;
using Unity.Entities;

// entity with this tag have health count
[Serializable]
public struct Health : IComponentData
{
	public float Value;
}
