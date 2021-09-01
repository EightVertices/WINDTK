# WINDTK
WIND stuff :/

## What is this ?
We created it to use it with the _XNA Framework_ (_MonoGame_), The WINDTK was our first "ATTEMPT" to create a file reader/writer, it only reads ``.wxn`` files and converts the data in ``WXNFileContent``.

## How to use this shit ?
The WINDTK main syntax must be interpreted as two main objects types: __Pure objects__ or  __Impure (Typed) objects__.
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
But.... What's the diference between then ?

__Pure__: the data types are defined by the object value.

__Impure (Typed)__: the data types are defined by the object type.

### Accepted types: ``Int, String, Bool, Array_Int, Array_String, Array_Bool``
```
