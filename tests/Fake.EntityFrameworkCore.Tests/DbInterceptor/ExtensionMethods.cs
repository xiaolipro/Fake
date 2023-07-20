using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DbInterceptor
{
    //
    // 摘要:
    //     Common extension methods to use only in this project.
    public static class ExtensionMethods
    {
        private static readonly JsonSerializerSettings defaultSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.None,
            ContractResolver = new DefaultContractResolver(),
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK"
        };

        private static readonly JsonSerializerSettings htmlEscapeSettings = new JsonSerializerSettings
        {
            StringEscapeHandling = StringEscapeHandling.EscapeHtml,
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.None,
            ContractResolver = new DefaultContractResolver(),
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK"
        };

        //
        // 摘要:
        //     Answers true if this String is either null or empty.
        //
        // 参数:
        //   value:
        //     The string to check.
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        //
        // 摘要:
        //     Answers true if this String is neither null or empty.
        //
        // 参数:
        //   value:
        //     The string to check.
        public static bool HasValue([NotNullWhen(true)] this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        //
        // 摘要:
        //     Chops off a string at the specified length and accounts for smaller length
        //
        // 参数:
        //   s:
        //     The string to truncate.
        //
        //   maxLength:
        //     The length to truncate to.
        public static string Truncate(this string s, int maxLength)
        {
            if (s == null || s!.Length <= maxLength)
            {
                return s;
            }

            return s!.Substring(0, maxLength);
        }

        //
        // 摘要:
        //     Checks if a string contains another one. Why the hell isn't this in the BCL already?
        //
        // 参数:
        //   s:
        //     The string to check for presence in.
        //
        //   value:
        //     The value to check presence of.
        //
        //   comparison:
        //     The System.StringComparison to use when comparing.
        //
        // 返回结果:
        //     Whether value is contained in s.
        public static bool Contains(this string s, string value, StringComparison comparison)
        {
            return s.IndexOf(value, comparison) >= 0;
        }

        //
        // 摘要:
        //     Removes trailing / characters from a path and leaves just one
        //
        // 参数:
        //   input:
        //     The string to ensure a trailing slash on.
        public static string EnsureTrailingSlash(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            return Regex.Replace(input, "/+$", string.Empty) + "/";
        }

        //
        // 摘要:
        //     Converts a List{Guid} into a JSON representation
        //
        // 参数:
        //   guids:
        //     The GUIDs to convert.
        //
        // 返回结果:
        //     A JSON representation of the GUIDs.
        public static string ToJson(this List<Guid> guids)
        {
            if (guids == null || guids!.Count == 0)
            {
                return "[]";
            }

            StringBuilder stringBuilder = new StringBuilder("[");
            for (int i = 0; i < guids!.Count; i++)
            {
                stringBuilder.Append('"').Append(guids![i].ToString()).Append('"');
                if (i < guids!.Count - 1)
                {
                    stringBuilder.Append(',');
                }
            }

            stringBuilder.Append(']');
            return stringBuilder.ToString();
        }


        //
        // 摘要:
        //     Serializes o to a JSON string.
        //
        // 参数:
        //   o:
        //     The instance to serialize.
        //
        // 返回结果:
        //     The resulting JSON object as a string.
        [return: NotNullIfNotNull("o")]
        public static string ToJson(this object o)
        {
            if (o == null)
            {
                return null;
            }

            return JsonConvert.SerializeObject(o, defaultSettings);
        }

        //
        // 摘要:
        //     Deserializes s to an object of type T.
        //
        // 参数:
        //   s:
        //     The string to deserialize.
        //
        // 类型参数:
        //   T:
        //     The type to deserialize to.
        //
        // 返回结果:
        //     The object resulting from the given string.
        public static T FromJson<T>(this string s) where T : class
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(s, defaultSettings);
        }

        //
        // 摘要:
        //     System.Collections.Generic.Dictionary`2 equivalent of ConcurrentDictionary's
        //     .TryRemove();
        //
        // 参数:
        //   dict:
        //     The dictionary to attempt removal from.
        //
        //   key:
        //     The key to attempt removal of.
        //
        //   value:
        //     The value found (if it was found) from the dictionary.
        //
        // 类型参数:
        //   TKey:
        //     The type of the keys in the dictionary.
        //
        //   TValue:
        //     The type of the values in the dictionary.
        //
        // 返回结果:
        //     Whether the key was removed.
        public static bool TryRemove<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, [NotNullWhen(true)] out TValue value)
        {
            value = default(TValue);
            if (dict != null && dict.TryGetValue(key, out value))
            {
                return dict.Remove(key);
            }

            return false;
        }
    }
}
