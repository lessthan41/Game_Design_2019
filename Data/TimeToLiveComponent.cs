using System;
using Unity.Entities;

// entity set deadtime count automatically destroy
[Serializable]
public struct TimeToLive : IComponentData
{
	public float Value;
}
