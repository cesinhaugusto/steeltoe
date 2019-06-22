﻿// Copyright 2017 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Steeltoe.Common.Util;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Steeltoe.Messaging
{
    public class MessageHeaders : IMessageHeaders
    {
        public static readonly Guid ID_VALUE_NONE = Guid.Empty;
        public static readonly string ID = "id";
        public static readonly string TIMESTAMP = "timestamp";
        public static readonly string CONTENT_TYPE = "contentType";
        public static readonly string REPLY_CHANNEL = "replyChannel";
        public static readonly string ERROR_CHANNEL = "errorChannel";

        protected readonly IDictionary<string, object> headers;

        private static readonly IIDGenerator _defaultIdGenerator = new DefaultIdGenerator();
        private static volatile IIDGenerator _idGenerator;

        public MessageHeaders(IDictionary<string, object> headers = null)
        : this(headers, null, null)
        {
        }

        public MessageHeaders(IDictionary<string, object> headers, Guid? id, long? timestamp)
        {
            this.headers = headers != null ? new Dictionary<string, object>(headers) : new Dictionary<string, object>();

            if (id == null)
            {
                this.headers[ID] = IdGenerator.GenerateId();
            }
            else if (id == ID_VALUE_NONE)
            {
                this.headers.Remove(ID);
            }
            else
            {
                this.headers[ID] = id;
            }

            if (timestamp == null)
            {
                this.headers[TIMESTAMP] = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }
            else if (timestamp.Value < 0)
            {
                this.headers.Remove(TIMESTAMP);
            }
            else
            {
                this.headers[TIMESTAMP] = timestamp.Value;
            }
        }

        public virtual Guid? Id
        {
            get
            {
                if (!headers.TryGetValue(ID, out object result))
                {
                    return null;
                }

                return (Guid)result;
            }
        }

        public virtual long? Timestamp
        {
            get
            {
                if (!headers.TryGetValue(TIMESTAMP, out object result))
                {
                    return null;
                }

                return (long)result;
            }
        }

        public virtual object ReplyChannel
        {
            get { return headers[REPLY_CHANNEL]; }
        }

        public virtual object ErrorChannel
        {
            get { return headers[ERROR_CHANNEL]; }
        }

        public override bool Equals(object other)
        {
            return this == other ||
                    (other is MessageHeaders && this.headers.Equals(((MessageHeaders)other).headers));
        }

        public override int GetHashCode()
        {
            return this.headers.GetHashCode();
        }

        public override string ToString()
        {
            return this.headers.ToString();
        }

        public virtual void Add(string key, object value)
        {
            throw new InvalidOperationException();
        }

        public virtual bool ContainsKey(string key)
        {
            return this.headers.ContainsKey(key);
        }

        public virtual bool Remove(string key)
        {
            throw new InvalidOperationException();
        }

        public virtual bool TryGetValue(string key, out object value)
        {
            return this.headers.TryGetValue(key, out value);
        }

        public virtual void Add(KeyValuePair<string, object> item)
        {
            throw new InvalidOperationException();
        }

        public virtual void Clear()
        {
            throw new InvalidOperationException();
        }

        public virtual bool Contains(KeyValuePair<string, object> item)
        {
            return this.headers.Contains(item);
        }

        public virtual void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            this.headers.CopyTo(array, arrayIndex);
        }

        public virtual bool Remove(KeyValuePair<string, object> item)
        {
            throw new InvalidOperationException();
        }

        public virtual IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.headers.GetEnumerator();
        }

        public virtual ICollection<string> Keys => this.headers.Keys;

        public virtual ICollection<object> Values => this.headers.Values;

        public virtual int Count => this.headers.Count;

        public virtual bool IsReadOnly => true;

        public virtual object this[string key] { get => this.headers[key]; set => throw new InvalidOperationException(); }

        public virtual T Get<T>(string key)
        {
            if (TryGetValue(key, out object val))
            {
                return (T)val;
            }

            return default(T);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.headers).GetEnumerator();
        }

        private MessageHeaders(MessageHeaders original, ISet<string> keysToIgnore)
        {
            this.headers = new Dictionary<string, object>();
            foreach (var entry in original.headers)
            {
                if (!keysToIgnore.Contains(entry.Key))
                {
                    this.headers[entry.Key] = entry.Value;
                }
            }
        }

        internal static IIDGenerator IdGenerator
        {
            get
            {
                IIDGenerator generator = _idGenerator;
                return generator != null ? generator : _defaultIdGenerator;
            }

            set
            {
                _idGenerator = value;
            }
        }

        protected internal virtual IDictionary<string, object> RawHeaders
        {
            get { return this.headers; }
        }
    }
}