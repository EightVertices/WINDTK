# WINDTK
A library consisting of utilities like a file parser (WXN) and others.

# WXN
## What is .wxn?
It's a parser similar to XML, but with simpler syntax and better (native) compatibility with C#.
We created it to use it with the _XNA Framework_ (_MonoGame_). WXN was our first attempt to create a file reader/writer. It reads ``.wxn`` files and converts the data into ``WXNFileContent``.

## How do you use it?
The WXN main syntax must be interpreted as two main objects types: __Pure (Implicit) objects__ or  __Impure (Explicit) objects__.
Here is a sample: 
```XML
 // Pure
<Version: 1>
<SceneName: RND>
<SceneMaxObjsCount: 30>
<SceneMaxObjs: ["A", "B", 33, Uk]>

// Impure
PlayerLife<Int>: 100
PLayerName<String>: "Renan"
PlayerWeapons<Array_String>: ["Sword", "Bow", "Hammer"]
```
But... What's the diference between then?

__Pure (Implicit)__: the data types are defined by the object value.

__Impure (Explicit)__: the data types are defined by the object type.

### Native types: _Int, String, Bool, Array_Int, Array_String, Array_Bool, Vector, Array_Vector2, Array_Vector3_
