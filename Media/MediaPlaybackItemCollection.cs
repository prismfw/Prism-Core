/*
Copyright (C) 2018  Prism Framework Team

This file is part of the Prism Framework.

The Prism Framework is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

The Prism Framework is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Prism.Native;

namespace Prism.Media
{
    [DebuggerDisplay("Count = {Count}")]
    internal class MediaPlaybackItemCollection : IList<MediaPlaybackItem>
    {
        public int Count
        {
            get { return nativeObject.Items.Count; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsReadOnly
        {
            get { return false; }
        }

        public MediaPlaybackItem this[int index]
        {
            get { return (MediaPlaybackItem)ObjectRetriever.GetAgnosticObject(nativeObject.Items[index]); }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                nativeObject.Items[index] = ObjectRetriever.GetNativeObject(value);
            }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeMediaPlaybackList nativeObject;

        internal MediaPlaybackItemCollection(INativeMediaPlaybackList parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            nativeObject = parent;
        }
        
        public void Add(MediaPlaybackItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            nativeObject.Items.Add(ObjectRetriever.GetNativeObject(item));
        }

        /// <summary>
        /// Removes all menu items from the collection.
        /// </summary>
        public void Clear()
        {
            nativeObject.Items.Clear();
        }
        
        public bool Contains(MediaPlaybackItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return nativeObject.Items.Contains(ObjectRetriever.GetNativeObject(item));
        }

        public void CopyTo(MediaPlaybackItem[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            for (int i = 0; i < nativeObject.Items.Count; i++)
            {
                array[arrayIndex + i] = (MediaPlaybackItem)ObjectRetriever.GetAgnosticObject(nativeObject.Items[i]);
            }
        }
        
        public int IndexOf(MediaPlaybackItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return nativeObject.Items.IndexOf(ObjectRetriever.GetNativeObject(item));
        }

        public void Insert(int index, MediaPlaybackItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (index == nativeObject.Items.Count)
            {
                nativeObject.Items.Add(ObjectRetriever.GetNativeObject(item));
            }
            else
            {
                nativeObject.Items.Insert(index, ObjectRetriever.GetNativeObject(item));
            }
        }
        
        public bool Remove(MediaPlaybackItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            int count = nativeObject.Items.Count;
            nativeObject.Items.Remove(ObjectRetriever.GetNativeObject(item));
            return count > nativeObject.Items.Count;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= nativeObject.Items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            nativeObject.Items.RemoveAt(index);
        }
        
        public IEnumerator<MediaPlaybackItem> GetEnumerator()
        {
            return new MediaPlaybackItemEnumerator(nativeObject.Items.Count > 0 ? nativeObject.Items.GetEnumerator() : null);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new MediaPlaybackItemEnumerator(nativeObject.Items.Count > 0 ? nativeObject.Items.GetEnumerator() : null);
        }

        private class MediaPlaybackItemEnumerator : IEnumerator<MediaPlaybackItem>, IEnumerator
        {
            public MediaPlaybackItem Current
            {
                get { return (MediaPlaybackItem)ObjectRetriever.GetAgnosticObject(nativeEnumerator?.Current); }
            }

            object IEnumerator.Current
            {
                get { return ObjectRetriever.GetAgnosticObject(nativeEnumerator?.Current); }
            }

#if !DEBUG
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
            private readonly IEnumerator nativeEnumerator;

            public MediaPlaybackItemEnumerator(IEnumerator nativeEnumerator)
            {
                this.nativeEnumerator = nativeEnumerator;
            }

            public void Dispose()
            {
                (nativeEnumerator as IDisposable)?.Dispose();
            }

            public bool MoveNext()
            {
                return nativeEnumerator != null && nativeEnumerator.MoveNext();
            }

            public void Reset()
            {
                nativeEnumerator?.Reset();
            }
        }
    }
}
