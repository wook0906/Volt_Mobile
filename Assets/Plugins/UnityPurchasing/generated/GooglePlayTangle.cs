#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("EMA7ub1YgcWMmrM5hAL154G3HSRh01BzYVxXWHvXGdemXFBQUFRRUgIB4Xz56JpFhuKpJwp57T5LwfMl01BeUWHTUFtT01BQUZf4b4ltSjy8o6qdq/ZGWbgpJn1m2Ng93CRZh49XVwYQYJKopw+cqf6tiEokPCN26itYttB86lGIwVgKpsnfD5yKjrIKYi7bnI3E3jbTAVxJz4kHQf4Dhqa27UQXh6uQhHexGlM2wXIwORhV/BrBW8k5Iuutj4eurx+QVk9QHOOWp3bqXqXn8YP0mFvkTxdqeBDbWB4fiICYGaJwktq1IoIHm2/FbuHsmRrUZQPManmmH20BrygxMy0J3BBQOmuAxZWFvp6fEzlhqWw/q9MVH3TMF1yTx6ZfUFNSUFFQ");
        private static int[] order = new int[] { 7,7,8,7,8,13,9,11,13,9,13,13,12,13,14 };
        private static int key = 81;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
