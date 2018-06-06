﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace Nhea.Logging
{
    public static class ExceptionDetailBuilder
    {
        private const string FileNameString = "File Name\t:\t";
        private const string ClassNameString = "Class Name\t:\t";
        private const string MethodNameString = "Method Name\t:\t";
        private const string LineNumberString = "Line Number\t:\t";

        public static void Build(Exception exception, out string exceptionDetail, out string exceptionData)
        {
            string fileName;
            Build(exception, out exceptionDetail, out exceptionData, out fileName);
        }

        public static void Build(Exception exception, out string exceptionDetail, out string exceptionData, out string fileName)
        {
            if (exception != null)
            {
                exceptionData = String.Empty;
                fileName = String.Empty;

                string exceptionMessage = String.Empty;
                StringBuilder exceptionStackTrace = new StringBuilder();

                while (exception != null)
                {
                    exceptionStackTrace.Insert(0, Environment.NewLine);
                    exceptionStackTrace.Insert(0, Environment.NewLine);

                    if (exception.Data.Count > 0)
                    {
                        foreach (DictionaryEntry entry in exception.Data)
                        {
                            if (entry.Key != null && entry.Value != null)
                            {
                                exceptionData = String.Concat(exceptionData, entry.Key.ToString() + " : " + entry.Value.ToString() + Environment.NewLine);
                            }
                        }
                    }

                    exceptionStackTrace.Insert(0, exception.StackTrace);
                    exceptionStackTrace.Insert(0, Environment.NewLine);

                    StackTrace trace = new StackTrace(exception, true);

                    if (trace != null)
                    {
                        StackFrame[] frames = trace.GetFrames();

                        if (frames != null)
                        {
                            foreach (StackFrame frame in frames)
                            {
                                string lineNumber = LineNumberString + frame.GetFileLineNumber().ToString() + Environment.NewLine;
                                exceptionStackTrace.Insert(0, lineNumber);

                                MethodBase methodBase = frame.GetMethod();

                                if (methodBase != null)
                                {
                                    string methodName = MethodNameString + methodBase.Name + Environment.NewLine;
                                    exceptionStackTrace.Insert(0, methodName);

                                    if (methodBase.DeclaringType != null)
                                    {
                                        string className = ClassNameString + methodBase.DeclaringType.FullName + Environment.NewLine;
                                        exceptionStackTrace.Insert(0, className);
                                    }
                                }

                                string file = frame.GetFileName();

                                if (!String.IsNullOrEmpty(file))
                                {
                                    fileName = file;

                                    exceptionStackTrace.Insert(0, FileNameString + file);
                                    exceptionStackTrace.Insert(0, Environment.NewLine);
                                }
                            }
                        }
                    }

                    exceptionStackTrace.Insert(0, Environment.NewLine);
                    exceptionStackTrace.Insert(0, exception.Message);

                    exception = exception.InnerException;
                }

                exceptionDetail = exceptionMessage + Environment.NewLine + Environment.NewLine + exceptionStackTrace;
                exceptionDetail = exceptionDetail.Trim('\r', '\n');
            }
            else
            {
                exceptionDetail = null;
                exceptionData = null;
                fileName = null;
            }
        }
    }
}