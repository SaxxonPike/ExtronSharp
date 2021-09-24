using System;
using System.Linq;

namespace ExtronSharp.Devices
{
    public static class ExCommand
    {
        public static string ParseResponseFragment(string response, string fragment) =>
            response?[(response.IndexOf(fragment, StringComparison.OrdinalIgnoreCase) + fragment.Length)..]
                .Split(' ')[0];

        public static string[] ParseResponseFragments(string response, params string[] fragments) =>
            response == null
                ? null
                : fragments.Select(fragment => ParseResponseFragment(response, fragment)).ToArray();
    }
}