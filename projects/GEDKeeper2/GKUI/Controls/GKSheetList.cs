/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2016 by Serg V. Zhdanovskih (aka Alchemist, aka Norseman).
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
using System.Drawing;
using System.Windows.Forms;

using GKCommon;
using GKCore;
using GKCore.Types;

namespace GKUI.Controls
{
    public delegate void ModifyEventHandler(object sender, ModifyEventArgs eArgs);

    public enum SheetButton
    {
        lbAdd,
        lbEdit,
        lbDelete,
        lbJump,
        lbMoveUp,
        lbMoveDown
    }

    /// <summary>
    /// 
    /// </summary>
    public class GKSheetList : ContainerControl
    {
        private static readonly object EventModify;

        private readonly ToolBarButton fBtnAdd;
        private readonly ToolBarButton fBtnDelete;
        private readonly ToolBarButton fBtnEdit;
        private readonly ToolBarButton fBtnLinkJump;
        private readonly ToolBarButton fBtnMoveUp;
        private readonly ToolBarButton fBtnMoveDown;
        private readonly ToolBar fToolBar;
        private readonly GKListView fList;

        private EnumSet<SheetButton> fButtons;
        private bool fReadOnly;

        public event ModifyEventHandler OnModify
        {
            add { base.Events.AddHandler(GKSheetList.EventModify, value); }
            remove { base.Events.RemoveHandler(GKSheetList.EventModify, value); }
        }

        public EnumSet<SheetButton> Buttons
        {
            get { return this.fButtons; }
            set { this.SetButtons(value); }
        }

        public bool ReadOnly
        {
            get { return this.fReadOnly; }
            set { this.SetReadOnly(value); }
        }

        static GKSheetList()
        {
            GKSheetList.EventModify = new object();
        }

        public GKSheetList(Control owner)
        {
            if (owner == null) {
                throw new ArgumentNullException("owner");
            }

            this.fBtnMoveDown = new ToolBarButton();
            this.fBtnMoveDown.ImageIndex = 30;
            this.fBtnMoveDown.ToolTipText = LangMan.LS(LSID.LSID_RecordMoveDown);
            this.fBtnMoveUp = new ToolBarButton();
            this.fBtnMoveUp.ImageIndex = 29;
            this.fBtnMoveUp.ToolTipText = LangMan.LS(LSID.LSID_RecordMoveUp);
            this.fBtnLinkJump = new ToolBarButton();
            this.fBtnLinkJump.ImageIndex = 28;
            this.fBtnLinkJump.ToolTipText = LangMan.LS(LSID.LSID_RecordGoto);
            this.fBtnDelete = new ToolBarButton();
            this.fBtnDelete.ImageIndex = 5;
            this.fBtnDelete.ToolTipText = LangMan.LS(LSID.LSID_MIRecordDelete);
            this.fBtnEdit = new ToolBarButton();
            this.fBtnEdit.ImageIndex = 4;
            this.fBtnEdit.ToolTipText = LangMan.LS(LSID.LSID_MIRecordEdit);
            this.fBtnAdd = new ToolBarButton();
            this.fBtnAdd.ImageIndex = 3;
            this.fBtnAdd.ToolTipText = LangMan.LS(LSID.LSID_MIRecordAdd);

            this.fToolBar = new ToolBar();
            this.fToolBar.Dock = DockStyle.Right;
            this.fToolBar.Appearance = ToolBarAppearance.Flat;
            this.fToolBar.Buttons.AddRange(new ToolBarButton[] {
                                               this.fBtnAdd,
                                               this.fBtnEdit,
                                               this.fBtnDelete,
                                               this.fBtnLinkJump,
                                               this.fBtnMoveUp,
                                               this.fBtnMoveDown});
            this.fToolBar.ImageList = MainWin.Instance.ImageList_Buttons;
            this.fToolBar.ShowToolTips = true;
            this.fToolBar.ButtonClick += this.ButtonClick;

            this.fList = new GKListView();
            this.fList.Dock = DockStyle.Fill;
            this.fList.Location = new Point(0, 0);
            this.fList.Size = new Size(500, 290);
            this.fList.HideSelection = false;
            this.fList.LabelEdit = false;
            this.fList.FullRowSelect = true;
            this.fList.View = View.Details;
            this.fList.DoubleClick += this.List_DoubleClick;
            this.fList.KeyDown += this.List_KeyDown;

            this.SuspendLayout();
            this.Controls.Add(this.fList);
            this.Controls.Add(this.fToolBar);
            this.Controls.SetChildIndex(this.fList, 1);
            this.Controls.SetChildIndex(this.fToolBar, 0);
            this.ResumeLayout(false);

            this.Dock = DockStyle.Fill;

            owner.SuspendLayout();
            owner.Controls.Add(this);
            owner.ResumeLayout(false);

            this.SetButtons(EnumSet<SheetButton>.Create(SheetButton.lbAdd, SheetButton.lbEdit, SheetButton.lbDelete));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.fList.Dispose();
                this.fBtnLinkJump.Dispose();
                this.fBtnMoveUp.Dispose();
                this.fBtnMoveDown.Dispose();
                this.fBtnDelete.Dispose();
                this.fBtnEdit.Dispose();
                this.fBtnAdd.Dispose();
                this.fToolBar.Dispose();
            }
            base.Dispose(disposing);
        }

        private void SetButtons(EnumSet<SheetButton> value)
        {
            this.fButtons = value;
            this.fBtnAdd.Visible = this.fButtons.Contains(SheetButton.lbAdd);
            this.fBtnDelete.Visible = this.fButtons.Contains(SheetButton.lbDelete);
            this.fBtnEdit.Visible = this.fButtons.Contains(SheetButton.lbEdit);
            this.fBtnLinkJump.Visible = this.fButtons.Contains(SheetButton.lbJump);
            this.fBtnMoveUp.Visible = this.fButtons.Contains(SheetButton.lbMoveUp);
            this.fBtnMoveDown.Visible = this.fButtons.Contains(SheetButton.lbMoveDown);
            this.fToolBar.Visible = !this.fButtons.IsEmpty();
        }

        private void SetReadOnly(bool value)
        {
            this.fReadOnly = value;
            this.fBtnAdd.Enabled = !this.fReadOnly;
            this.fBtnDelete.Enabled = !this.fReadOnly;
            this.fBtnEdit.Enabled = !this.fReadOnly;
            this.fBtnMoveUp.Enabled = !this.fReadOnly;
            this.fBtnMoveDown.Enabled = !this.fReadOnly;

            this.fList.BackColor = (this.fReadOnly) ? SystemColors.Control : SystemColors.Window;
        }

        private void ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            if (e.Button == this.fBtnAdd)
            {
                this.ItemAdd();
            }
            else if (e.Button == this.fBtnEdit)
            {
                this.ItemEdit();
            }
            else if (e.Button == this.fBtnDelete)
            {
                this.ItemDelete();
            }
            else if (e.Button == this.fBtnLinkJump)
            {
                this.ItemJump();
            }
            else if (e.Button == this.fBtnMoveUp)
            {
                this.ItemMoveUp();
            }
            else if (e.Button == this.fBtnMoveDown)
            {
                this.ItemMoveDown();
            }
        }

        private void List_DoubleClick(object sender, EventArgs e)
        {
            this.ItemEdit();
        }

        private void List_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode) {
                    case Keys.I:
                        this.ItemAdd();
                        break;
                    case Keys.D:
                        this.ItemDelete();
                        break;
                    case Keys.Return:
                        this.ItemEdit();
                        break;
                }
            }
        }

        /*private void SheetShow(object sender, EventArgs e)
		{
			this.fList.Focus();
		}*/

        public object GetSelectedData()
        {
            object result = null;
            if (this.fList.SelectedItem() != null)
            {
                result = this.fList.SelectedItem().Data;
            }
            return result;
        }

        private void RestoreSelected(object itemData)
        {
            if (itemData != null) this.fList.SelectItem(itemData);
        }

        private void DoModify(ModifyEventArgs eArgs)
        {
            ModifyEventHandler eventHandler = (ModifyEventHandler)base.Events[GKSheetList.EventModify];
            if (eventHandler != null)
            {
                eventHandler(this, eArgs);
            }
        }

        private void ItemAdd()
        {
            if (!this.fReadOnly)
            {
                ModifyEventArgs eArgs = new ModifyEventArgs(RecordAction.raAdd, null);
                this.DoModify(eArgs);
                this.RestoreSelected(eArgs.ItemData);
            }
        }

        private void ItemEdit()
        {
            object itemData = this.GetSelectedData();
            if (!this.fReadOnly && itemData != null)
            {
                ModifyEventArgs eArgs = new ModifyEventArgs(RecordAction.raEdit, itemData);
                this.DoModify(eArgs);
                this.RestoreSelected(eArgs.ItemData);
            }
        }

        private void ItemDelete()
        {
            object itemData = this.GetSelectedData();
            if (!this.fReadOnly && itemData != null)
            {
                ModifyEventArgs eArgs = new ModifyEventArgs(RecordAction.raDelete, itemData);
                this.DoModify(eArgs);
            }
        }

        private void ItemJump()
        {
            object itemData = this.GetSelectedData();
            if (itemData != null)
            {
                ModifyEventArgs eArgs = new ModifyEventArgs(RecordAction.raJump, itemData);
                this.DoModify(eArgs);
            }
        }

        private void ItemMoveUp()
        {
            object itemData = this.GetSelectedData();
            if (!this.fReadOnly && itemData != null)
            {
                ModifyEventArgs eArgs = new ModifyEventArgs(RecordAction.raMoveUp, itemData);
                this.DoModify(eArgs);
                this.RestoreSelected(eArgs.ItemData);
            }
        }

        private void ItemMoveDown()
        {
            object itemData = this.GetSelectedData();
            if (!this.fReadOnly && itemData != null)
            {
                ModifyEventArgs eArgs = new ModifyEventArgs(RecordAction.raMoveDown, itemData);
                this.DoModify(eArgs);
                this.RestoreSelected(eArgs.ItemData);
            }
        }

        public void ClearColumns()
        {
            this.fList.Columns.Clear();
        }

        public void ResizeColumn(int columnIndex)
        {
            this.fList.ResizeColumn(columnIndex);
        }

        public void AddColumn(string caption, int width, bool autoSize)
        {
            this.fList.AddListColumn(caption, width, autoSize);
        }

        public void Columns_BeginUpdate()
        {
        }

        public void Columns_EndUpdate()
        {
        }

        public void BeginUpdate()
        {
            this.fList.BeginUpdate();
        }

        public void EndUpdate()
        {
            this.fList.EndUpdate();
        }

        public GKListItem AddItem(object itemValue, object data)
        {
            return this.fList.AddItem(itemValue, data);
        }

        public void ClearItems()
        {
            this.fList.Items.Clear();
        }

        public void SwitchSorter()
        {
            this.fList.SwitchSorter();
        }
    }
}