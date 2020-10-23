﻿namespace Scellecs {
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public static class Result {
        public static Result<T> Ok<T>(T value) {
            Result<T> r;
#if DEBUG || !SCELLECS_RESULT_DISABLE_CHECKS
            r.check   = false;
            r.isError = false;
#else
            r.IsError = false;
#endif
            r.errorCode    = default;
            r.errorMessage = default;
            r.value        = value;
            return r;
        }

        public static Result<T> Error<T>(int errorCode, string errorMessage) {
            Result<T> r;
#if DEBUG || !SCELLECS_RESULT_DISABLE_CHECKS
            r.check   = false;
            r.isError = true;
#else
            r.IsError = true;
#endif
            r.value        = default;
            r.errorMessage = errorMessage;
            r.errorCode    = errorCode;
            return r;
        }
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [StructLayout(LayoutKind.Sequential)]
    public ref struct Result<T> {
#if DEBUG || !SCELLECS_RESULT_DISABLE_CHECKS
        public bool IsError {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                this.check = true;
                return this.isError;
            }
        }

        internal bool check;
        internal bool isError;
#else
        public   bool IsError;
#endif
        internal T      value;
        internal int    errorCode;
        internal string errorMessage;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T TryGet() {
#if DEBUG || !SCELLECS_RESULT_DISABLE_CHECKS
            if (this.check == false) {
                throw new NoCheckHasValueException();
            }

            if (this.isError == true) {
                throw new ResultHasNoValueException();
            }

            this.check = false;
#endif
            return this.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Error TryGetError() {
#if DEBUG || !SCELLECS_RESULT_DISABLE_CHECKS
            if (this.check == false) {
                throw new NoCheckHasValueException();
            }

            if (this.isError == false) {
                throw new ResultHasValueException();
            }

            this.check = false;
#endif

            Error error;
            error.code    = this.errorCode;
            error.message = this.errorMessage;
            return error;
        }
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [StructLayout(LayoutKind.Sequential)]
    public ref struct Error {
        public int    code;
        public string message;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ErrorException AsException() => new ErrorException(this.code, this.message);
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class NoCheckHasValueException : Exception {
        public NoCheckHasValueException() : base("Check IsError property before TryGet.") {
        }
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ResultHasNoValueException : Exception {
        public ResultHasNoValueException() : base("Result has no value. Check IsError property before TryGet.") {
        }
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ResultHasValueException : Exception {
        public ResultHasValueException() : base("Result has value. Check IsError property before TryGetError.") {
        }
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ErrorException : Exception {
        public readonly int    code;
        public readonly string message;

        public ErrorException(int code, string message) : base($"Code: {code} Message: {message}") {
            this.code    = code;
            this.message = message;
        }
    }
}

namespace Unity.IL2CPP.CompilerServices {
    using System;

    internal enum Option {
        NullChecks         = 1,
        ArrayBoundsChecks  = 2,
        DivideByZeroChecks = 3
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    internal class Il2CppSetOptionAttribute : Attribute {
        public Option Option { get; }
        public object Value  { get; }

        public Il2CppSetOptionAttribute(Option option, object value) {
            this.Option = option;
            this.Value  = value;
        }
    }
}