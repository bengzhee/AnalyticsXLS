using System.Runtime.InteropServices;

namespace QuantLibrary.Maths
{
    public class NL2SOL
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void errorFunction(int n, int p, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)][In, Out] double[] parameters, ref int nf, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)][In, Out] double[] observations);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool constraintsFunction(int p, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)][In] double[] parameters);
        public NL2SOL(int nParameters, int nObservations)
        {
            m_nParameters = nParameters;
            m_nObservations = nObservations;
            m_n_work_integer = 60 + m_nParameters;
            m_n_work_double = 93 + m_nObservations * m_nParameters + 3 * m_nObservations + m_nParameters * (3 * m_nParameters + 33) / 2;
            m_work_integer = new int[m_n_work_integer];
            m_work_double = new double[m_n_work_double];
        }
        public int Solve(Func<double[], double[]> func, ref double[] parameters, Func<double[], bool> hadConstraintsViolated)
        {
            m_func = func;
            m_constraints = hadConstraintsViolated;
            errorFunction err = funcErr;
            constraintsFunction constr = funcConstr;
            IntPtr p_func = Marshal.GetFunctionPointerForDelegate(err);
            IntPtr p_constr = hadConstraintsViolated == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(constr);
            m_work_integer[0] = 0;
            int status = nl2sno(m_nObservations, m_nParameters, parameters, p_func, m_work_integer, m_work_double, m_n_work_integer, m_n_work_double, p_constr);
            return status;
        }
        private void funcErr(int n, int p, double[] parameters, ref int nf, double[] observations)
        {
            double[] tmp = m_func(parameters);
            for (int i = 0; i < n; ++i)
                observations[i] = tmp[i];
        }
        private bool funcConstr(int p, double[] parameters)
        {
            return m_constraints(parameters);
        }
        private int m_nParameters;
        private int m_nObservations;
        private int m_n_work_integer;
        private int m_n_work_double;
        private int[] m_work_integer;
        private double[] m_work_double;
        Func<double[], double[]> m_func;
        Func<double[], bool> m_constraints;
        [DllImport("MV_Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nl2sno")] //
        private static extern int nl2sno(int n, int p, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)][In, Out] double[] x, IntPtr calcr, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6)][In, Out] int[] iv, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 7)][In, Out] double[] v, int n_iv, int n_v, IntPtr calcc);
        //public static extern int nl2sno(int n, int p, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In, Out] double[] x, IntPtr calcr, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6)] [In, Out] int[] iv, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 7)] [In, Out] double[] v, int n_iv, int n_v, IntPtr calcc);
        //private static extern int nl2sno(int n, int p, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In, Out] double[] x                , [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6)] [In, Out] int[] iv, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 7)] [In, Out] double[] v, int n_iv, int n_v);
    }
}
