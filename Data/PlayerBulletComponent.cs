using System;
using Unity.Entities;

// player bullet tag (cannot shoot player but can shoot enemy)
[Serializable]
public struct PlayerBulletTag : IComponentData {}
