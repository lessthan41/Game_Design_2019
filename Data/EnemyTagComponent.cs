using System;
using Unity.Entities;

// enemy tag (declare that it is a enemy and can be shot by player bullet)
[Serializable]
public struct EnemyTag : IComponentData { }
