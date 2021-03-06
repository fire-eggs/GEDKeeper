﻿/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2017 by Sergey V. Zhdanovskih.
 *
 *  This file is part of "GEDKeeper".
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Reflection;
using System.Runtime.InteropServices;

using GKCommon;
using GKCore.Interfaces;

[assembly: AssemblyTitle("GKDataQualityPlugin")]
[assembly: AssemblyDescription("GEDKeeper DataQuality plugin")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("GEDKeeper")]
[assembly: AssemblyCopyright("Copyright © 2017 by Sergey V. Zhdanovskih")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

namespace GKDataQualityPlugin
{
    public enum CLS
    {
        /* 00 */ LSID_Title,
        /* 01 */ LSID_1,
        /* 02 */ LSID_2,
        /* 03 */ LSID_3,
        /* 04 */ LSID_4,
        /* 05 */ LSID_5
    }

    public sealed class Plugin : BaseObject, IPlugin, IWidget
    {
        private string fDisplayName = "GKDataQualityPlugin";
        private IHost fHost;
        private ILangMan fLangMan;

        public string DisplayName { get { return fDisplayName; } }
        public IHost Host { get { return fHost; } }
        public ILangMan LangMan { get { return fLangMan; } }
        public IImage Icon { get { return null; } }

        private DataQualityWidget fForm;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CloseForm();
            }
            base.Dispose(disposing);
        }

        internal void CloseForm()
        {
            if (fForm != null) {
                fForm = null;
            }
        }

        public void Execute()
        {
            if (!fHost.IsWidgetActive(this)) {
                fForm = new DataQualityWidget(this);
                fForm.Show();
            } else {
                fForm.Close();
            }
        }

        public void OnHostClosing(HostClosingEventArgs eventArgs) {}
        public void OnHostActivate() {}
        public void OnHostDeactivate() {}

        public void OnLanguageChange()
        {
            try
            {
                fLangMan = fHost.CreateLangMan(this);
                fDisplayName = fLangMan.LS(CLS.LSID_Title);

                if (fForm != null) fForm.SetLang();
            }
            catch (Exception ex)
            {
                Logger.LogWrite("GKDataQualityPlugin.OnLanguageChange(): " + ex.Message);
            }
        }

        public bool Startup(IHost host)
        {
            bool result = true;
            try
            {
                fHost = host;
            }
            catch (Exception ex)
            {
                Logger.LogWrite("GKDataQualityPlugin.Startup(): " + ex.Message);
                result = false;
            }
            return result;
        }

        public bool Shutdown()
        {
            bool result = true;
            try
            {
                CloseForm();
            }
            catch (Exception ex)
            {
                Logger.LogWrite("GKDataQualityPlugin.Shutdown(): " + ex.Message);
                result = false;
            }
            return result;
        }

        #region IWidget common

        void IWidget.WidgetInit(IHost host) {}

        void IWidget.BaseChanged(IBaseWindow baseWin)
        {
            if (fForm != null) {
                fForm.BaseChanged(baseWin);
            }
        }

        void IWidget.BaseClosed(IBaseWindow baseWin)
        {
            if (fForm != null) {
                fForm.BaseChanged(null);
            }
        }

        void IWidget.BaseRenamed(IBaseWindow baseWin, string oldName, string newName) {}
        void IWidget.WidgetEnable() {}

        #endregion
    }
}
