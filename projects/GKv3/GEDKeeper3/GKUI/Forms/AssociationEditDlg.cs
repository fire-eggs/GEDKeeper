﻿/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2017 by Sergey V. Zhdanovskih.
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
using Eto.Forms;
using GKCommon;
using GKCommon.GEDCOM;
using GKCore;
using GKCore.Options;
using GKCore.Types;
using GKCore.UIContracts;

namespace GKUI.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class AssociationEditDlg : EditorDialog, IAssociationEditDlg
    {
        private GEDCOMAssociation fAssociation;
        private GEDCOMIndividualRecord fTempInd;

        public GEDCOMAssociation Association
        {
            get { return fAssociation; }
            set {
                if (fAssociation != value) {
                    fAssociation = value;
                    UpdateView();
                }
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                string rel = cmbRelation.Text.Trim();
                if (rel != "" && GlobalOptions.Instance.Relations.IndexOf(rel) < 0)
                {
                    GlobalOptions.Instance.Relations.Add(rel);
                }

                fAssociation.Relation = cmbRelation.Text;
                fAssociation.Individual = fTempInd;

                DialogResult = DialogResult.Ok;
            }
            catch (Exception ex)
            {
                Logger.LogWrite("AssociationEditDlg.btnAccept_Click(): " + ex.Message);
                DialogResult = DialogResult.None;
            }
        }

        private void btnPersonAdd_Click(object sender, EventArgs e)
        {
            fTempInd = fBase.Context.SelectPerson(null, TargetMode.tmNone, GEDCOMSex.svNone);
            txtPerson.Text = ((fTempInd == null) ? "" : GKUtils.GetNameString(fTempInd, true, false));
        }

        public AssociationEditDlg()
        {
            InitializeComponent();

            int num = GlobalOptions.Instance.Relations.Count;
            for (int i = 0; i < num; i++)
            {
                cmbRelation.Items.Add(GlobalOptions.Instance.Relations[i]);
            }

            // SetLang()
            btnAccept.Text = LangMan.LS(LSID.LSID_DlgAccept);
            btnCancel.Text = LangMan.LS(LSID.LSID_DlgCancel);
            Title = LangMan.LS(LSID.LSID_Association);
            lblRelation.Text = LangMan.LS(LSID.LSID_Relation);
            lblPerson.Text = LangMan.LS(LSID.LSID_Person);

            btnPersonAdd.ToolTip = LangMan.LS(LSID.LSID_PersonAttachTip);
        }

        public override void UpdateView()
        {
            cmbRelation.Text = fAssociation.Relation;
            string st = ((fAssociation.Individual == null) ? "" : GKUtils.GetNameString(fAssociation.Individual, true, false));
            txtPerson.Text = st;
        }
    }
}
