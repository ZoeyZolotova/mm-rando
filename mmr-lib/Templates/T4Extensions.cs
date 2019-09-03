using System.Collections.Generic;

namespace System.CodeDom.Compiler
{
    internal class CompilerErrorCollection : List<CompilerError>
    {
    }

    internal class CompilerError
    {
        public string ErrorText { get; set; }

        public bool IsWarning { get; set; }
    }
}