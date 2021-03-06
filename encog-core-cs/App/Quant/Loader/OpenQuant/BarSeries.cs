﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Bar = Encog.App.Quant.Loader.OpenQuant.Data.Data.Bar;
using BarData = Encog.App.Quant.Loader.OpenQuant.Data.Data.BarData;
using BarSlycing = Encog.App.Quant.Loader.OpenQuant.Data.Data.BarSlycing;


using System.Drawing;
namespace Encog.App.Quant.Loader.OpenQuant
{

    // Fields
    [Serializable]
    public class DataArray : IEnumerable
    {


        protected double fDivisor;
        protected ArrayList fList;
        protected int fStopRecurant;

        // Methods
        [MethodImpl(MethodImplOptions.NoInlining)]
        public DataArray()
        {
            this.fList = new ArrayList();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Add(Data.Data.IDataObject obj)
        {
            this.fList.Add(obj);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Clear()
        {
            this.fList.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified obj].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified obj]; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool Contains(Data.Data.IDataObject obj)
        {
            return this.fList.Contains(obj);
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool Contains(Bar bar)
        {
            return this.fList.Contains(bar);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public IEnumerator GetEnumerator()
        {
            return this.fList.GetEnumerator();
        }

        /// <summary>
        /// Gets the index for a certain datetime.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public int GetIndex(DateTime datetime)
        {
            if (this.Count != 0)
            {
                DateTime dateTime = this[0].DateTime;
                DateTime time2 = this[this.Count - 1].DateTime;
                if ((dateTime <= datetime) && (time2 >= datetime))
                {
                    return this.GetIndex(datetime, 0, this.Count - 1);
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets the index for a certain date time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="index1">The index1.</param>
        /// <param name="index2">The index2.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public int GetIndex(DateTime datetime, int index1, int index2)
        {
            int num4;
            long ticks = this[index1].DateTime.Ticks;
            long num2 = this[index2].DateTime.Ticks;
            long num3 = datetime.Ticks;
            if (num2 != ticks)
            {
                num4 = index1 + ((int)((index2 - index1) * ((num3 - ticks) / (num2 - ticks))));
            }
            else
            {
                num4 = (index1 + index2) / 2;
            }
            Data.Data.IDataObject obj2 = this[num4];
            if (obj2.DateTime == datetime)
            {
                return num4;
            }
            if (((index2 - num4) < this.fStopRecurant) || ((num4 - index1) < this.fStopRecurant))
            {
                for (int i = index2; i >= index1; i--)
                {
                    obj2 = this[i];
                    if (obj2.DateTime < datetime)
                    {
                        return i;
                    }
                }
                return -1;
            }
            int num6 = (int)(((double)(index2 - index1)) / this.fDivisor);
            int num7 = Math.Max(index1, num4 - num6);
            obj2 = this[num7];
            if (obj2.DateTime > datetime)
            {
                return this.GetIndex(datetime, index1, num7);
            }
            int num8 = Math.Min(index2, num4 + num6);
            obj2 = this[num8];
            if (obj2.DateTime < datetime)
            {
                return this.GetIndex(datetime, num8, index2);
            }
            return this.GetIndex(datetime, num7, num8);
        }

        /// <summary>
        /// Inserts an item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="obj">The obj.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Insert(int index, Data.Data.IDataObject obj)
        {
            this.fList.Insert(index, obj);
        }

        /// <summary>
        /// Inserts an item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="bar">The bar.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Insert(int index, Bar bar)
        {
            this.fList.Insert(index, bar);
        }

        /// <summary>
        /// Removes the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Remove(Data.Data.IDataObject obj)
        {
            this.fList.Remove(obj);
        }


        /// <summary>
        /// Removes the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Remove(Bar bar)
        {
            this.fList.Remove(bar);
        }
        /// <summary>
        /// Removes an object at the specified index in the array of objects.
        /// </summary>
        /// <param name="index">The index.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void RemoveAt(int index)
        {
            this.fList.RemoveAt(index);
        }
        // Properties
        /// <summary>
        /// Gets the number of object in the array.
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return this.fList.Count;
            }
        }
        /// <summary>
        /// Gets the <see cref="Encog.App.Quant.Loader.OpenQuant.Data.Data.IDataObject"/> at the specified index.
        /// </summary>
        public Data.Data.IDataObject this[int index]
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return (this.fList[index] as Data.Data.IDataObject);
            }
        }

        /// <summary>
        /// Adds the specified bar.
        /// </summary>
        /// <param name="bar">The bar.</param>
        public void Add(Bar bar)
        {
            this.Add(bar);
        }
    }

}


 

