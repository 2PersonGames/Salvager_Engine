using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SalvagerEngine.Storage.Files
{
    public abstract class BaseFile : IDisposable
    {
        // Constants

        public enum Result
        {
            Success,
            Failure,
            Warning,
            FileDoesNotExist,
            TimeOut
        };

        // Variables

        public int MaxTries { protected set; get; }
        public int MaxWaitTime { protected set; get; }
        public string Fullpath { private set; get; }

        // Constructors

        public BaseFile(string fullPath)
        {
            MaxTries = 20;
            Fullpath = fullPath;
            MaxWaitTime = 1000;
        }

        // Tools

        protected Result TryAction(Func<bool> action)
        {
            int count = 0;
            while (true)
            {
                try
                {
                    if (action())
                    {
                        return Result.Success;
                    }
                    else
                    {
                        return Result.Warning;
                    }
                }
                catch (Exception e)
                {
                    if (count >= MaxTries)
                    {
                        return Result.Failure;
                    }
                }
            }
        }

        // Interfaces

        public virtual void Dispose()
        {
        }
    }
}
