using System;
using System.Reflection;

namespace ComputingMath.Parsing
{
    public class Function
    {
        public virtual double evaluate(double[] x)
        {
            return 0.0;
        }

        public virtual double evaluate(double x)
        {
            return 0.0;
        }
    }

    public class FunctionCompiling
    {
        private Function myFunction = null;
        private int n = 1;

        public FunctionCompiling(string expression)
            : this(expression, 1)
        {
        }

        public FunctionCompiling(string expression, int n)
        {
            this.n = n;

            if (n > 0)
            {
                if (n == 1)
                {
                    this.initOneDimension(expression);
                }
                else
                {
                    this.initDimensional(expression);
                }
            }
            else
            {
                throw new ParsingErrorException("Incorrect dimension were set.");
            }
        }

        private void initDimensional(string expression)
        {
            Microsoft.CSharp.CSharpCodeProvider cp = new Microsoft.CSharp.CSharpCodeProvider();
            System.CodeDom.Compiler.ICodeCompiler ic = cp.CreateCompiler();
            System.CodeDom.Compiler.CompilerParameters cpar = new System.CodeDom.Compiler.CompilerParameters();

            cpar.GenerateInMemory = true;
            cpar.GenerateExecutable = false;
            cpar.ReferencedAssemblies.Add("system.dll");
            cpar.ReferencedAssemblies.Add("ComputingMath.dll");

            string src = "using System;" +
             "class myclass : ComputingMath.Parsing.Function" +
             "{" +
             "public myclass(){}" +
             "public override double evaluate(double[] x)" +
             "{" +
             "return " + expression + ";" +
             "}" +
             "}";

            System.CodeDom.Compiler.CompilerResults cr = ic.CompileAssemblyFromSource(cpar, src);

            foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
            {
                throw new ParsingErrorException(ce.ErrorText);
            }

            if ((cr.Errors.Count == 0) && (cr.CompiledAssembly != null))
            {
                Type ObjType = cr.CompiledAssembly.GetType("myclass");
                try
                {
                    if (ObjType != null)
                    {
                        myFunction = (Function)Activator.CreateInstance(ObjType);
                    }
                }
                catch (Exception exception)
                {
                    throw new ParsingErrorException("Unexpected error due to parsing function!", exception);
                }
            }
            else
            {
                throw new ParsingErrorException("Error parsing function!");
            }
        }

        private void initOneDimension(string expression)
        {
            Microsoft.CSharp.CSharpCodeProvider cp = new Microsoft.CSharp.CSharpCodeProvider();
            System.CodeDom.Compiler.ICodeCompiler ic = cp.CreateCompiler();
            System.CodeDom.Compiler.CompilerParameters cpar = new System.CodeDom.Compiler.CompilerParameters();

            cpar.GenerateInMemory = true;
            cpar.GenerateExecutable = false;
            cpar.ReferencedAssemblies.Add("system.dll");
            cpar.ReferencedAssemblies.Add("ComputingMath.dll");

            string src = "using System;" +
             "class myclass : ComputingMath.Parsing.Function" +
             "{" +
             "public myclass(){}" +
             "public override double evaluate(double x)" +
             "{" +
             "return " + expression + ";" +
             "}" +
             "}";

            System.CodeDom.Compiler.CompilerResults cr = ic.CompileAssemblyFromSource(cpar, src);

            foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
            {
                throw new ParsingErrorException(ce.ErrorText);
            }

            if ((cr.Errors.Count == 0) && (cr.CompiledAssembly != null))
            {
                Type ObjType = cr.CompiledAssembly.GetType("myclass");
                try
                {
                    if (ObjType != null)
                    {
                        myFunction = (Function)Activator.CreateInstance(ObjType);
                    }
                }
                catch (Exception exception)
                {
                    throw new ParsingErrorException("Unexpected error due to parsing function!", exception);
                }
            }
            else
            {
                throw new ParsingErrorException("Error parsing function!");
            }
        }

        public double evaluate(double[] x)
        {
            double result = 0.0;
            if (n == 1)
            {
                result = myFunction.evaluate(x[0]);
            }
            else
            {
                result = myFunction.evaluate(x);
            }

            return result;
        }

        public double evaluate(double x)
        {
            if (this.n == 1)
            {
                return myFunction.evaluate(x);
            }
            else
            {
                throw new ParsingErrorException("Using evaluate function for incorrect dimension.");
            }
        }
    }
}
