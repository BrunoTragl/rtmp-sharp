
#define AKUTILS_DEBUG

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.Generic;



public static class AKUtils
{
#if AKUTILS_DEBUG
    private static void LogMessage(string tag, int stackIndex, string arguments, string message)
    {
#if DEBUG
        var frame = new System.Diagnostics.StackFrame(stackIndex, true);

        var thread = System.Threading.Thread.CurrentThread;
        var threadInfo = "<" + thread.ManagedThreadId.ToString("000") + (string.IsNullOrEmpty(thread.Name) ? "" : ("=" + thread.Name)) + ">";
#if GG_PLATFORM_IOS
        var dateInfo = "";
#else
        var dateInfo = "[" + DateTime.Now.ToString("HH:mm:ss,fff") + "]";
#endif
        var backrefInfo = new string(' ', 32) + " in " + frame.GetFileName() + ":" + frame.GetFileLineNumber();
        var methodInfo = frame.GetMethod().ReflectedType.Name + "." + frame.GetMethod().Name + "(" + arguments + ")";

        var msg = threadInfo + " " + dateInfo + " " + methodInfo + " " + message + " " + backrefInfo;

#if GG_PLATFORM_ANDROID
        Android.Util.Log.Debug(tag, msg);
#elif GG_PLATFORM_IOS
        Console.WriteLine(tag + " " + msg);
#else
        Debug.WriteLine(tag + " " + msg);
#endif
#endif // DEBUG
    }

    public static void Trace(params object[] objs)
    {
#if DEBUG
        LogMessage("#TRACE#", 2, string.Join(", ", objs.Select(it => it == null ? "null" : it.ToString())), null);
#endif
    }

    public static void TraceUp(params object[] objs)
    {
#if DEBUG
        var args = string.Join(", ", objs.Select(it => it == null ? "null" : it.ToString()));
        LogMessage("#TRACE#", 2, args, null);
        LogMessage("#TRACE#", 2 + 1, "^", null);
        LogMessage("#TRACE#", 2 + 2, "^", null);
#endif
    }

    public static void Assert(bool condinion, params object[] objs)
    {
#if DEBUG
        if (!condinion) {
            LogMessage("#ASSERT#", 2, null, string.Join(", ", objs.Select(it => it == null ? "null" : it.ToString())));
            System.Diagnostics.Debugger.Break();
        }
#endif
    }

    public static void Validate()
    {
#if DEBUG
        Assert(Thread.CurrentThread.ManagedThreadId == 1);
#endif
    }
#endif // AKUTILS_DEBUG

    public static string JoinStrings<T>(this IEnumerable<T> self, string glue = "")
    {
        return string.Join(glue, self.Select(it => "" + it));
    } 

    public static string ToHexString(this IntPtr self, int bytesCount, string glue = " ")
    {
        if (self == IntPtr.Zero)
            return "<IntPtr.Zero>";
        var bytes = new byte[bytesCount];
        Marshal.Copy(self, bytes, 0, bytesCount);
        return bytes.ToHexString(bytesCount, glue);
    }

    public static string ToHexString(this byte[] self, string glue = " ")
    {
        if (self == null)
            return "<null>";
        return self.ToHexString(self.Length, glue);
    }

    public static string ToHexString(this byte[] self, int bytesCount, string glue = " ")
    {
        if (self == null)
            return "<null>";
        return self.Take(bytesCount).Select(it => it.ToString("x2")).JoinStrings(glue);
    }


    public static string ToUtfString(this byte[] self, bool onlyPrintable = true)
    {
        if (self == null)
            return "<null>";
        if (self.All(it => 0x30 <= it && it < 0x80))
            return System.Text.Encoding.UTF8.GetString(self);
        return "<raw>";
    }


}

