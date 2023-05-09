using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysProgLAB1_18240_18450
{
    internal class Cache
    {
        private static int _TIME_OUT = 200;
        private int _size;
        private Dictionary<string, string> _dictionary;
        private Queue<string> _keyQueue;
        private ReaderWriterLockSlim _cacheLock=new ReaderWriterLockSlim();

        public Cache(int size = 10)
        {
            _size = size;
            _dictionary = new Dictionary<string, string>(_size);
            _keyQueue = new Queue<string>(_size);
        }
        public string CitajIzKesa(string key)
        {
            try
            {
                _cacheLock.EnterReadLock();
                Console.WriteLine($"Iz kesa pribavljen response koji odgovara kljucu: {key}.");
                return _dictionary[key];
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }

        }

        public bool SadrziKljuc (string key)
        {
            try
            {
                _cacheLock.EnterReadLock();
                return _dictionary.ContainsKey(key);
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }

        public void UpisiUKes(string key,string value)
        {
            bool locked = false;
            try
            {
                locked = _cacheLock.TryEnterWriteLock(_TIME_OUT);
                if (locked)
                {
                    if(_dictionary.Count==_size)
                    {
                        string key2 = _keyQueue.Dequeue();
                        _dictionary.Remove(key2);
                        Console.WriteLine("Kes je pun!");
                        Console.WriteLine("Iz kesa po FIFO algoritmu izbacen response koji odgovara kljucu: " + key2);
                    }
                    _dictionary.Add(key, value);
                    _keyQueue.Enqueue(key);
                    Console.WriteLine($"U kes upisan response koji odgovara kljucu: {key}.");
                }
            }
            finally
            {
                if(locked)
                    _cacheLock.ExitWriteLock();
            }
            
        }
    }
}
