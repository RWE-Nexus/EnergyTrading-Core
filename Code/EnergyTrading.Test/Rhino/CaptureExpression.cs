namespace EnergyTrading.Test.Rhino
{
    using System;
    using System.Collections.Generic;

    using global::Rhino.Mocks;

    /// <summary>
    /// Captures arguments for Rhino Mocks calls.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CaptureExpression<T>
        where T: class
    {
        private readonly T stub;

        public CaptureExpression(T stub)
        {
            this.stub = stub;
        }

        /// <summary>
        /// Capture arguments for a one parameter method.
        /// </summary>
        /// <typeparam name="U">Type of the parameter.</typeparam>
        /// <param name="methodExpression"></param>
        /// <returns></returns>
        public IList<U> Args<U>(Action<T, U> methodExpression)
        {
            var captured = new List<U>();

            Action<U> captureArg = captured.Add;
            Action<T> stubArg = x => methodExpression(x, default(U));

            stub.Stub(stubArg).IgnoreArguments().Do(captureArg);

            return captured;
        }

        /// <summary>
        /// Capture arguments for a two parameter method.
        /// </summary>
        /// <typeparam name="U1"></typeparam>
        /// <typeparam name="U2"></typeparam>
        /// <param name="methodExpression"></param>
        /// <returns></returns>
        public IList<Tuple<U1, U2>> Args<U1, U2>(Action<T, U1, U2> methodExpression)
        {
            var captured = new List<Tuple<U1, U2>>();

            Action<U1, U2> captureArg = (x, y) => captured.Add(Tuple.Create(x, y));
            Action<T> stubArg = x => methodExpression(x, default(U1), default(U2));

            stub.Stub(stubArg).IgnoreArguments().Do(captureArg);

            return captured; 
        }

        /// <summary>
        /// Capture arguments for a three parameter method.
        /// </summary>
        /// <typeparam name="U1"></typeparam>
        /// <typeparam name="U2"></typeparam>
        /// <typeparam name="U3"></typeparam>
        /// <param name="methodExpression"></param>
        /// <returns></returns>
        public IList<Tuple<U1, U2, U3>> Args<U1, U2, U3>(Action<T, U1, U2, U3> methodExpression)
        {
            var captured = new List<Tuple<U1, U2, U3>>();

            Action<U1, U2, U3> captureArg = (x, y, z) => captured.Add(Tuple.Create(x, y, z));
            Action<T> stubArg = x => methodExpression(x, default(U1), default(U2), default(U3));

            stub.Stub(stubArg).IgnoreArguments().Do(captureArg);

            return captured;
        }

        /// <summary>
        /// Capture arguments for a four parameter method.
        /// </summary>
        /// <typeparam name="U1"></typeparam>
        /// <typeparam name="U2"></typeparam>
        /// <typeparam name="U3"></typeparam>
        /// <typeparam name="U4"></typeparam> 
        /// <param name="methodExpression"></param>
        /// <returns></returns>
        public IList<Tuple<U1, U2, U3, U4>> Args<U1, U2, U3, U4>(Action<T, U1, U2, U3, U4> methodExpression)
        {
            var captured = new List<Tuple<U1, U2, U3, U4>>();

            Action<U1, U2, U3, U4> captureArg = (x, y, z, a) => captured.Add(Tuple.Create(x, y, z, a));
            Action<T> stubArg = x => methodExpression(x, default(U1), default(U2), default(U3), default(U4));

            stub.Stub(stubArg).IgnoreArguments().Do(captureArg);

            return captured;
        }

        /// <summary>
        /// Capture arguments for a one parameter function.
        /// </summary>
        /// <typeparam name="U">Type of the parameter.</typeparam>
        /// <typeparam name="R">Return type of the function.</typeparam>
        /// <param name="funcExpression"></param>
        /// <param name="returnArg">Return value for the function, defaults to the default value for R</param>
        /// <returns></returns>
        public IList<U> FuncArgs<U, R>(Func<T, U, R> funcExpression, R returnArg = default(R))
        {
            var captured = new List<U>();

            Func<U, R> captureArg = (x) => { captured.Add(x); return returnArg; };
            Action<T> stubArg = x => funcExpression(x, default(U));

            stub.Stub(stubArg).IgnoreArguments().Do(captureArg);

            return captured;
        }

        /// <summary>
        /// Capture arguments for a two parameter function.
        /// </summary>
        /// <typeparam name="U1"></typeparam>
        /// <typeparam name="U2"></typeparam>
        /// <typeparam name="R">Return type of the function.</typeparam>
        /// <param name="funcExpression"></param>
        /// <param name="returnArg">Return value for the function, defaults to the default value for R</param>
        /// <returns></returns>
        public IList<Tuple<U1, U2>> FuncArgs<U1, U2, R>(Func<T, U1, U2, R> funcExpression, R returnArg = default(R))
        {
            var captured = new List<Tuple<U1, U2>>();

            Func<U1, U2, R> captureArg = (x, y) => { captured.Add(Tuple.Create(x, y)); return returnArg; };
            Action<T> stubArg = x => funcExpression(x, default(U1), default(U2));

            stub.Stub(stubArg).IgnoreArguments().Do(captureArg);

            return captured;
        }

        /// <summary>
        /// Capture arguments for a three parameter function.
        /// </summary>
        /// <typeparam name="U1"></typeparam>
        /// <typeparam name="U2"></typeparam>
        /// <typeparam name="U3"></typeparam>        
        /// <typeparam name="R">Return type of the function.</typeparam>
        /// <param name="funcExpression"></param>
        /// <param name="returnArg">Return value for the function, defaults to the default value for R</param>
        /// <returns></returns>
        public IList<Tuple<U1, U2, U3>> FuncArgs<U1, U2, U3, R>(Func<T, U1, U2, U3, R> funcExpression, R returnArg = default(R))
        {
            var captured = new List<Tuple<U1, U2, U3>>();

            Func<U1, U2, U3, R> captureArg = (x, y, z) => { captured.Add(Tuple.Create(x, y, z)); return returnArg; };
            Action<T> stubArg = x => funcExpression(x, default(U1), default(U2), default(U3));

            stub.Stub(stubArg).IgnoreArguments().Do(captureArg);

            return captured;
        }

        /// <summary>
        /// Capture arguments for a four parameter function.
        /// </summary>
        /// <typeparam name="U1"></typeparam>
        /// <typeparam name="U2"></typeparam>
        /// <typeparam name="U3"></typeparam>
        /// <typeparam name="U4"></typeparam>         
        /// <typeparam name="R">Return type of the function.</typeparam>
        /// <param name="funcExpression"></param>
        /// <param name="returnArg">Return value for the function, defaults to the default value for R</param>
        /// <returns></returns>
        public IList<Tuple<U1, U2, U3, U4>> FuncArgs<U1, U2, U3, U4, R>(Func<T, U1, U2, U3, U4, R> funcExpression, R returnArg = default(R))
        {
            var captured = new List<Tuple<U1, U2, U3, U4>>();

            Func<U1, U2, U3, U4, R> captureArg = (x, y, z, a) => { captured.Add(Tuple.Create(x, y, z, a)); return returnArg; };
            Action<T> stubArg = x => funcExpression(x, default(U1), default(U2), default(U3), default(U4));

            stub.Stub(stubArg).IgnoreArguments().Do(captureArg);

            return captured;
        }

        /// <summary>
        /// Capture arguments for a four parameter function.
        /// </summary>
        /// <typeparam name="U1"></typeparam>
        /// <typeparam name="U2"></typeparam>
        /// <typeparam name="U3"></typeparam>
        /// <typeparam name="U4"></typeparam>         
        /// <typeparam name="U5"></typeparam>         
        /// <typeparam name="R">Return type of the function.</typeparam>
        /// <param name="funcExpression"></param>
        /// <param name="returnArg">Return value for the function, defaults to the default value for R</param>
        /// <returns></returns>
        public IList<Tuple<U1, U2, U3, U4, U5>> FuncArgs<U1, U2, U3, U4, U5, R>(Func<T, U1, U2, U3, U4, U5, R> funcExpression, R returnArg = default(R))
        {
            var captured = new List<Tuple<U1, U2, U3, U4, U5>>();

            Func<U1, U2, U3, U4, U5, R> captureArg = (x, y, z, a, b) => { captured.Add(Tuple.Create(x, y, z, a, b)); return returnArg; };
            Action<T> stubArg = x => funcExpression(x, default(U1), default(U2), default(U3), default(U4), default(U5));

            stub.Stub(stubArg).IgnoreArguments().Do(captureArg);

            return captured;
        }
    }
}
