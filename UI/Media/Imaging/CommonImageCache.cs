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
using System.Collections.Generic;
using System.Linq;

namespace Prism.UI.Media.Imaging
{
    internal class CommonImageCache : IImageCache
    {
        public int Capacity
        {
            get { return capacity; }
            set
            {
                if (value == capacity)
                {
                    return;
                }

                capacity = value;
                for (int i = capacity; i < cache.Count; i++)
                {
                    cache.Remove(cache.Keys.First());
                }
            }
        }
        private int capacity;

        public int Count
        {
            get { return cache.Count; }
        }

        private readonly Dictionary<Uri, CacheEntry> cache;

        public CommonImageCache()
        {
            cache = new Dictionary<Uri, CacheEntry>();
            capacity = 50;
        }

        public void Add(Uri sourceUri, ImageSource image, DateTime? expirationDate)
        {
            if (capacity > 0)
            {
                cache[sourceUri] = new CacheEntry(image, expirationDate);
                for (int i = capacity; i < cache.Count; i++)
                {
                    cache.Remove(cache.Keys.First(k => !k.Equals(sourceUri)));
                }
            }
        }

        public void Clear()
        {
            cache.Clear();
        }

        public bool Contains(Uri sourceUri)
        {
            return cache.ContainsKey(sourceUri);
        }

        public ImageSource GetImage(Uri sourceUri)
        {
            CacheEntry entry;
            if (cache.TryGetValue(sourceUri, out entry))
            {
                if (!entry.ExpirationDate.HasValue || entry.ExpirationDate.Value > DateTime.Now)
                {
                    return entry.Image;
                }

                cache.Remove(sourceUri);
            }

            return null;
        }

        public void Remove(Uri sourceUri)
        {
            cache.Remove(sourceUri);
        }

        public void RemoveExpired()
        {
            var expired = cache.Where(kvp => kvp.Value.ExpirationDate.HasValue && kvp.Value.ExpirationDate.Value < DateTime.Now).ToArray();
            foreach (var kvp in expired)
            {
                cache.Remove(kvp.Key);
            }
        }

        private struct CacheEntry
        {
            public readonly ImageSource Image;
            public readonly DateTime? ExpirationDate;

            public CacheEntry(ImageSource image, DateTime? expirationDate)
            {
                Image = image;
                ExpirationDate = expirationDate;
            }
        }
    }
}
