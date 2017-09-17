﻿using System;
using System.Reflection;
using Eto;
using Eto.Forms;
using GKCommon;
using GKCore;
using GKUI.Components;

[assembly: AssemblyTitle("GEDKeeper3.Wpf")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("GEDKeeper")]
[assembly: AssemblyCopyright("Copyright © 2009-2017 by Sergey V. Zhdanovskih")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("3.0.0.0")]

namespace GEDKeeper3.Wpf
{
    public class Program
    {
        private static void LogSysInfo()
        {
            try
            {
                //#if __MonoCS__
                //Logger.LogWrite("Mono Version: " + SysUtils.GetMonoVersion());
                //Logger.LogWrite("Desktop Type: " + SysUtils.GetDesktopType().ToString());
                //#endif

                // There should be no links to the application infrastructure
                Assembly execAssembly = Assembly.GetExecutingAssembly();
                Logger.LogWrite("CLR Version: " + execAssembly.ImageRuntimeVersion);
                Logger.LogWrite("GK Version: " + execAssembly.GetName().Version.ToString());
            }
            catch { }
        }

        [STAThread]
        public static void Main(string[] args)
        {
            EtoFormsAppHost.ConfigureBootstrap(false);
            Logger.LogInit(EtoFormsAppHost.GetLogFilename());
            LogSysInfo();

            AppHost.InitSettings();
            try
            {
                var application = new Application(Platforms.Wpf);

                var appHost = (EtoFormsAppHost)AppHost.Instance;
                appHost.Init(args, false);

                application.Run();
            } finally {
                AppHost.DoneSettings();
            }
        }
    }
}
