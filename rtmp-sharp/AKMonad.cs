using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class AKMonad
{
    public class _Monad<T> where T : class
    {
        T _inner;

        public _Monad(T obj)
        {
            _inner = obj;
        }

        public _Monad<T> Do(Action<T> action)
        {
            if (_inner != null)
                action(_inner);
            return this;
        }
        
        public _Monad<TResult> Get<TResult>(Func<T, TResult> func) where TResult : class
        {
            if (_inner != null)
                return new _Monad<TResult>(func(_inner));
            return new _Monad<TResult>(default(TResult));
        }

        public _Monad<T> If(Func<T, bool> func)
        {
            if (_inner == null)
                return this;
            if (func(_inner))
                return this;
            return new _Monad<T>(default(T));
        }


        public _Monad<T> Fail(Action action)
        {
            if (_inner == null)
                action();
            return this;
        }

        public _Monad<T> Fail(Action<T> action)
        {
            if (_inner == null)
                action(_inner);
            return this;
        }

        public T Result()
        {
            return _inner;
        }

        public static implicit operator T(_Monad<T> self)
        {
            return self._inner;
        }

    }

    public static _Monad<T> ToMonad<T>(this T self) where T : class
    {
        return new _Monad<T>(self);
    }
}

