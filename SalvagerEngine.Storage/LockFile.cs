using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SalvagerEngine.Storage
{
    public sealed class LockFile : BaseFile
    {
        // Variables

        public bool HasLock { get; private set; }

        // Constructors

        public LockFile(string pathForFileToLock, int maxWaitTime = -1)
            : base(Path.Combine(Directory.GetParent(pathForFileToLock).FullName, 
                string.Format("~{0}.lock", Path.GetFileNameWithoutExtension(pathForFileToLock))))
        {
            MaxWaitTime = maxWaitTime;
        }

        // Accessors

        public bool IsLocked()
        {
            return File.Exists(Fullpath);
        }

        // Mutators

        public Result Lock()
        {
            if (HasLock)
            {
                return Result.Success;
            }
            else
            {
                var end = MaxWaitTime < 0 ? long.MaxValue : DateTime.Now.AddMilliseconds(MaxWaitTime).Ticks;
                if (DateTime.Now.Ticks < end)
                {
                    return TryAction(delegate
                    {
                        if (!IsLocked())
                        {
                            try
                            {
                                using (var writer = new StreamWriter(Fullpath, false, Encoding.ASCII))
                                {
                                    writer.WriteLine(DateTime.Now.ToLongDateString());
                                    writer.Flush();
                                    writer.Close();
                                    return true;
                                }
                            }
                            catch
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    });
                }
                else
                {
                    return Result.TimeOut;
                }
            }
        }

        public Result Unlock()
        {
            if (HasLock)
            {
                if (IsLocked())
                {
                    return TryAction(delegate
                    {
                        File.Delete(Fullpath);
                        return !File.Exists(Fullpath);
                    });
                }
                else
                {
                    return Result.Success;
                }
            }
            else
            {
                return Result.Failure;
            }
        }

        // Overrides

        public override void Dispose()
        {
            base.Dispose();
            Unlock();
        }
    }
}
