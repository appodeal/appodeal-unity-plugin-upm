using UnityEditor;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.Checkers
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class FixProblemInstruction
    {
        private bool _checkedForResolve;

        public bool checkedForResolve
        {
            get
            {
                return _checkedForResolve;
            } 
            set
            {
                if (isAutoResolvePossible) _checkedForResolve = value;
            }
        }

        private readonly string desc;
        private readonly bool isAutoResolvePossible;

        public FixProblemInstruction(string description, bool autoResolve)
        {
            desc = description;
            isAutoResolvePossible = autoResolve;
        }

        public string getDescription()
        {
            return desc;
        }

        public bool canBeResolvedAutomatically()
        {
            return isAutoResolvePossible;
        }

        public virtual void fixProblem()
        {
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public abstract class CheckingStep
    {
        public bool done;
        public abstract string getName();
        public abstract List<FixProblemInstruction> check();
        public abstract bool isRequiredForPlatform(BuildTarget target);
    }
}
