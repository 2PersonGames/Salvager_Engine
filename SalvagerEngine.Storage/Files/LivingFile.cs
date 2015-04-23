using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SalvagerEngine.Storage.Files
{
    public class LivingFile : ItemFile
    {
        // Variables

        ReaderWriterLockSlim mAttributeLock;
        Dictionary<string, object> mAttributes;

        // Constructors

        public LivingFile(string localFilePath)
            : base(localFilePath)
        {
            mAttributes = new Dictionary<string, object>();
        }

        // Overrides

        protected override bool Load(StreamReader reader)
        {
            bool success = true;

                try
                {
                    mAttributes.Clear();
                    while (!reader.EndOfStream)
                    {
                        var lines = reader.ReadLine().Split('\t');
                        TypeDescriptor.GetConverter(Type.GetType(lines[1]));
                        mAttributes.Add(lines[0], TypeDescriptor.GetConverter(Type.GetType(lines[1])).ConvertFrom(lines[2]));
                    }
                }
                catch (Exception)
                {
                    success = false;
                }

            return success;
        }

        protected override bool Save(StreamWriter writer)
        {
            bool success = true;

            try
            {
                foreach (var key in mAttributes.Keys)
                {
                    writer.WriteLine(string.Format("{0}\t{1}\t{2}", key, mAttributes[key].GetType(), mAttributes[key]));
                }
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }

        // Accessors

        public object GetAttribute(string name)
        {
            try
            {
                mAttributeLock.EnterReadLock();
                return mAttributes[name];
            }
            finally
            {
                mAttributeLock.ExitReadLock();
            }
        }

        public T GetAttribute<T>(string name)
        {
            return (T)GetAttribute(name);
        }

        // Mutators

        public void SetAttributes(string name, object value)
        {
            try
            {
                mAttributeLock.EnterWriteLock();
                if (mAttributes.ContainsKey(name))
                {
                    mAttributes[name] = value;
                }
                else
                {
                    mAttributes.Add(name, value);
                }
            }
            finally
            {
                mAttributeLock.ExitWriteLock();
            }
        }
    }
}
