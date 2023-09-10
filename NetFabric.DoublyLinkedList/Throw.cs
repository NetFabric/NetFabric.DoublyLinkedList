using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace NetFabric
{
    static class Throw
    {
        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ArgumentException(string paramName, string? message = default)
            => throw new ArgumentException(paramName: paramName, message: message);

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ArgumentNullException(string paramName, string? message = default)
            => throw new ArgumentNullException(paramName: paramName, message: message);

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static T ArgumentNullException<T>(string paramName, string? message = default)
            => throw new ArgumentNullException(paramName: paramName, message: message);

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void InvalidOperationException(string? message = default)
            => throw new InvalidOperationException(message);

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static T InvalidOperationException<T>(string? message = default)
            => throw new InvalidOperationException(message);

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void NotSupportedException()
            => throw new NotSupportedException();

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ArgumentOutOfRangeException(string paramName, string? message = default)
            => throw new ArgumentOutOfRangeException(paramName: paramName, message: message);
    }
}