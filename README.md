# Kogane Move Component Attribute

コンポーネントの位置を指定したコンポーネントの上か下に自動で移動するエディタ拡張

## 使用例

```csharp
using Kogane;
using UnityEngine;

[MoveComponentUp( typeof( Rigidbody2D ) )]
public sealed class Example : MonoBehaviour
{
}
```

```csharp
using Kogane;
using UnityEngine;

[MoveComponentDown( typeof( Rigidbody2D ) )]
public sealed class Example : MonoBehaviour
{
}
```