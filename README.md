# Result
🎲 **Monad library for Unity Game Engine.**  

The Result monad is useful to express a series of computations that might return an error object with additional information, but without force to throwing exceptions.  
There are quite a few errors that are not exceptions and should be handled in the usual manner.  

The Result mixin has two type constructors: Ok and Error.  
The Ok can be thought of as "everything went success" and the Error is used when "something has gone wrong".

## 📖 How To Install

Open Package Manager and add Git URL to this repository.  

## 📘 Getting Started

Declaration of method with return Result:

```c#  
using Scellecs;
...

public Result<int> ValidateSomeInt(int someArg) {
    if (someArg < 0) {
        return Result.Error<int>(0, "someArg is less than zero");
    }
    else if (someArg == 0) {
        return Result.Error<int>(1, "someArg is zero");
    }
    else {
        return Result.Ok(someArg);
    }
}
```

Shorter version:

```c#  
using Scellecs;
using static Scellecs.Result;
...

public Result<int> ValidateSomeInt(int someArg) {
    if (someArg < 0) {
        return Error<int>(0, "someArg is less than zero");
    }
    if (someArg == 0) {
        return Error<int>(1, "someArg is zero");
    }
    return Ok(someArg);
}
```

Some consumer method with error handling:

```c#  
public void Foo() {
    var result = this.ValidateSomeInt(-100);
    if (result.IsError) {
        var error = result.TryGetError();
        if (error.code == 0) {
            //handle error
        }
        else if (error.code == 1) {
            //handle error
        }
    }
    else {
        var value = result.TryGet();
        //some stuff
    }
}
```

Some consumer method with manual exception throwing:

```c#  
public void Bar() {
    var result = this.ValidateSomeInt(-100);
    if (result.IsError) {
        throw result.TryGetError().AsException();
    }
    else {
        var value = result.TryGet();
        //some stuff
    }
}
```

Invalid case of usage:

```c#  
public void WidePhilSpencer() {
    var result = this.ValidateSomeInt(-100);
    
    result.TryGet();      //calling TryGet without IsError check, cause throw an exception
    result.TryGetError(); //calling TryGetError without IsError check, cause throw an exception
    
    if (result.IsError) {
        var value = result.TryGet(); // there is the error. getting the value cause throw an exception
    }
    else {
        var error = result.TryGetError(); //opposite situation. there is the value. getting the error cause throw an exception
        
        var value = result.TryGet();
        var duplicateValue = result.TryGet(); // getting value two in a row is impossible, an exception will be thrown
    }
}
```
> 💡 **IMPORTANT**  
> Result and Error are ref structs.  
  They can't be stored in fields, collections and anywhere except stack.  


## 📘 License

📄 [MIT License](LICENSE)

## 💬 Contacts

✉️ Telegram: [olegmrzv](https://t.me/olegmrzv)  
📧 E-Mail: [mail@olegmorozov.dev](mailto:mail@olegmorozov.dev)
