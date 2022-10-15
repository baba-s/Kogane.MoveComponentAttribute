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

![icon465](https://user-images.githubusercontent.com/6134875/195975848-532f8bc2-d984-4f8f-9ba2-ea15b5de1c58.gif)

```csharp
using Kogane;
using UnityEngine;

[MoveComponentDown( typeof( Rigidbody2D ) )]
public sealed class Example : MonoBehaviour
{
}
```

![icon466](https://user-images.githubusercontent.com/6134875/195975851-df0ef821-6744-4704-8c26-a9c80e82610e.gif)
