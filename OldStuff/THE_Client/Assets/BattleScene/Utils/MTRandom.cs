using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class MTRandom
{
    private const int N = 634;
    private long[] _mt;
    private int _index;

    public MTRandom()
    {
        this._mt = new long[N];
        this._index = 0;
    }
     
    public void init(long seed=19650218)
    {
        this._mt[0] = seed;
        for (int i=1;i<N;i++)
        {
            this._mt[i] = 0xffffffff & (0x6c078965 * (this._mt[i - 1] ^ (this._mt[i - 1] >> 30)) + i);
        }
    }

    private void nextState()
    {
        for (int i=0;i<N;i++)
        {
            long y = (this._mt[i] & 0x80000000) + (this._mt[(i + 1) % N] & 0x7fffffff);
            this._mt[i] = this._mt[(i + 397) % N] ^ (y >> 1);
            if (y % 2 != 0)
            {
                this._mt[i] ^= 0x9908b0df;
            }
        }
    }

    public long getNext()
    {
        if ( this._index == 0 )
        {
            this.nextState();
        }
        long y = this._mt[this._index];
        y ^= (y >> 11);
        y ^= ((y << 7) & 0x9d2c5680);
        y ^= ((y << 15) & 0xefc60000);
        y ^= (y >> 18);
        this._index = (this._index + 1) % 624;
        return y;
    }

    public double getNextDouble()
    {
        return (double)this.getNext() / 0xffffffff;
    }

    /// <summary>
    /// 获取min到max之间的随机整数N，[min,max]
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public int getNext(int min,int max)
    {
        return (int)(this.getNext() % (max - min + 1)) + min;
    }
}

