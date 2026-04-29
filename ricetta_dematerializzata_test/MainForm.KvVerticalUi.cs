using ricetta_dematerializzata.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ricetta_dematerializzata_test_ui
{
    public partial class MainForm
    {
        private bool _kvUiInitialized;

        private ListView? _kvLvInputP;
        private Button? _kvBtnAddInputP;
        private Button? _kvBtnEditInputP;
        private Button? _kvBtnDeleteInputP;

        private ListView? _kvLvInputE;
        private Button? _kvBtnAddInputE;
        private Button? _kvBtnEditInputE;
        private Button? _kvBtnDeleteInputE;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (_kvUiInitialized) return;

            InitializeKvVerticalUi();
            _kvUiInitialized = true;
        }

        private void InitializeKvVerticalUi()
        {
            SetupPrescrittoreInputList();
            SetupErogatoreInputList();

            LoadListFromInput(_txtInputP, _kvLvInputP!);
            LoadListFromInput(_txtInputE, _kvLvInputE!);

            _txtInputP.TextChanged += (s, e) => LoadListFromInput(_txtInputP, _kvLvInputP!);
            _txtInputE.TextChanged += (s, e) => LoadListFromInput(_txtInputE, _kvLvInputE!);
        }

        private void SetupPrescrittoreInputList()
        {
            lblInputP.Text = "Input (lista key/value)";

            _kvLvInputP = CreateListView();
            _kvLvInputP.Location = new Point(10, 158);
            _tabPrescrittore.Controls.Add(_kvLvInputP);

            _kvBtnAddInputP = CreateSideButton("➕ Aggiungi", new Point(860, 158), BtnAddInputP_Click);
            _kvBtnEditInputP = CreateSideButton("✏️ Modifica", new Point(860, 194), BtnEditInputP_Click);
            _kvBtnDeleteInputP = CreateSideButton("🗑 Elimina", new Point(860, 230), BtnDeleteInputP_Click);

            _tabPrescrittore.Controls.Add(_kvBtnAddInputP);
            _tabPrescrittore.Controls.Add(_kvBtnEditInputP);
            _tabPrescrittore.Controls.Add(_kvBtnDeleteInputP);

            _txtInputP.Visible = false;

            _btnChiamaP.Location = new Point(10, 450);
            _btnDebugSoapP.Location = new Point(178, 450);
            lblOutputP.Location = new Point(10, 498);
            _txtOutputP.Location = new Point(10, 520);
            _txtOutputP.Size = new Size(1005, 100);
        }

        private void SetupErogatoreInputList()
        {
            lblInputE.Text = "Input (lista key/value)";

            _kvLvInputE = CreateListView();
            _kvLvInputE.Location = new Point(10, 158);
            _tabErogatore.Controls.Add(_kvLvInputE);

            _kvBtnAddInputE = CreateSideButton("➕ Aggiungi", new Point(860, 158), BtnAddInputE_Click);
            _kvBtnEditInputE = CreateSideButton("✏️ Modifica", new Point(860, 194), BtnEditInputE_Click);
            _kvBtnDeleteInputE = CreateSideButton("🗑 Elimina", new Point(860, 230), BtnDeleteInputE_Click);

            _tabErogatore.Controls.Add(_kvBtnAddInputE);
            _tabErogatore.Controls.Add(_kvBtnEditInputE);
            _tabErogatore.Controls.Add(_kvBtnDeleteInputE);

            _txtInputE.Visible = false;

            _btnChiamaE.Location = new Point(10, 450);
            _btnDebugSoapE.Location = new Point(178, 450);
            lblOutputE.Location = new Point(10, 498);
            _txtOutputE.Location = new Point(10, 520);
            _txtOutputE.Size = new Size(1005, 100);
        }

        private static ListView CreateListView()
        {
            var lv = new ListView
            {
                Size = new Size(840, 280),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                MultiSelect = false,
                HideSelection = false
            };

            lv.Columns.Add("Chiave", 260);
            lv.Columns.Add("Valore", 560);

            return lv;
        }

        private static Button CreateSideButton(string text, Point location, EventHandler onClick)
        {
            var btn = new Button
            {
                Text = text,
                Location = location,
                Size = new Size(155, 30)
            };
            btn.Click += onClick;
            return btn;
        }

        private static void LoadListFromInput(TextBox source, ListView list)
        {
            list.BeginUpdate();
            try
            {
                list.Items.Clear();
                var dict = ParserKV.Parse(source.Text);

                foreach (var kv in dict)
                {
                    var item = new ListViewItem(kv.Key);
                    item.SubItems.Add(kv.Value);
                    list.Items.Add(item);
                }
            }
            finally
            {
                list.EndUpdate();
            }
        }

        private static void SaveListToInput(ListView list, TextBox target)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (ListViewItem item in list.Items)
            {
                var key = item.Text?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(key)) continue;

                var value = item.SubItems.Count > 1 ? item.SubItems[1].Text : string.Empty;
                dict[key] = value ?? string.Empty;
            }

            target.Text = ParserKV.Build(dict);
        }

        private void BtnAddInputP_Click(object? sender, EventArgs e)
            => AddOrEditItem(_kvLvInputP!, _txtInputP, null);

        private void BtnEditInputP_Click(object? sender, EventArgs e)
            => EditSelectedItem(_kvLvInputP!, _txtInputP);

        private void BtnDeleteInputP_Click(object? sender, EventArgs e)
            => DeleteSelectedItem(_kvLvInputP!, _txtInputP);

        private void BtnAddInputE_Click(object? sender, EventArgs e)
            => AddOrEditItem(_kvLvInputE!, _txtInputE, null);

        private void BtnEditInputE_Click(object? sender, EventArgs e)
            => EditSelectedItem(_kvLvInputE!, _txtInputE);

        private void BtnDeleteInputE_Click(object? sender, EventArgs e)
            => DeleteSelectedItem(_kvLvInputE!, _txtInputE);

        private void EditSelectedItem(ListView list, TextBox target)
        {
            if (list.SelectedItems.Count == 0)
            {
                MessageBox.Show("Seleziona una riga da modificare.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AddOrEditItem(list, target, list.SelectedItems[0]);
        }

        private void DeleteSelectedItem(ListView list, TextBox target)
        {
            if (list.SelectedItems.Count == 0)
            {
                MessageBox.Show("Seleziona una riga da eliminare.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            list.Items.Remove(list.SelectedItems[0]);
            SaveListToInput(list, target);
        }

        private void AddOrEditItem(ListView list, TextBox target, ListViewItem? existing)
        {
            var currentKey = existing?.Text ?? string.Empty;
            var currentValue = existing != null && existing.SubItems.Count > 1 ? existing.SubItems[1].Text : string.Empty;

            var currentChildren = new List<KeyValuePair<string, string>>();
            if (!string.IsNullOrWhiteSpace(currentKey) && IsArrayParentKey(currentKey))
                currentChildren = ExtractChildren(list, currentKey);

            if (!TryShowKeyValueDialog(currentKey, currentValue, currentChildren, out var key, out var value, out var children))
                return;

            var oldParentKey = existing?.Text;
            if (!string.IsNullOrWhiteSpace(oldParentKey))
                RemoveChildren(list, oldParentKey);

            ListViewItem parentItem;
            if (existing == null)
            {
                parentItem = new ListViewItem(key);
                parentItem.SubItems.Add(value);
                list.Items.Add(parentItem);
            }
            else
            {
                parentItem = existing;
                parentItem.Text = key;
                if (parentItem.SubItems.Count > 1) parentItem.SubItems[1].Text = value;
                else parentItem.SubItems.Add(value);
            }

            if (children.Count > 0)
                UpsertChildren(list, key, children);

            SaveListToInput(list, target);
        }

        private bool TryShowKeyValueDialog(
            string key,
            string value,
            List<KeyValuePair<string, string>> initialChildren,
            out string resultKey,
            out string resultValue,
            out List<KeyValuePair<string, string>> children)
        {
            resultKey = string.Empty;
            resultValue = string.Empty;
            children = new List<KeyValuePair<string, string>>();

            using (var dlg = new Form())
            using (var lblKey = new Label())
            using (var txtKey = new TextBox())
            using (var lblValue = new Label())
            using (var txtValue = new TextBox())
            using (var chkArray = new CheckBox())
            using (var lblNodes = new Label())
            using (var lvNodes = new ListView())
            using (var btnAddNode = new Button())
            using (var btnEditNode = new Button())
            using (var btnDeleteNode = new Button())
            using (var btnDuplicateNode = new Button())
            using (var btnOk = new Button())
            using (var btnCancel = new Button())
            {
                dlg.Text = "Gestione coppia key/value";
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.ClientSize = new Size(620, 390);

                lblKey.Text = "Chiave";
                lblKey.Location = new Point(12, 15);
                lblKey.Size = new Size(70, 20);

                txtKey.Location = new Point(85, 12);
                txtKey.Size = new Size(520, 22);
                txtKey.Text = key;

                lblValue.Text = "Valore";
                lblValue.Location = new Point(12, 47);
                lblValue.Size = new Size(70, 20);

                txtValue.Location = new Point(85, 44);
                txtValue.Size = new Size(520, 22);
                txtValue.Text = value;

                chkArray.Text = "Valore array (con nodi figli)";
                chkArray.Location = new Point(85, 74);
                chkArray.Size = new Size(260, 22);

                lblNodes.Text = "Nodi figli";
                lblNodes.Location = new Point(12, 105);
                lblNodes.Size = new Size(70, 20);

                lvNodes.Location = new Point(85, 102);
                lvNodes.Size = new Size(430, 220);
                lvNodes.View = View.Details;
                lvNodes.FullRowSelect = true;
                lvNodes.GridLines = true;
                lvNodes.MultiSelect = false;
                lvNodes.HideSelection = false;
                lvNodes.Columns.Add("Nodo", 180);
                lvNodes.Columns.Add("Valore", 230);

                btnAddNode.Text = "➕ Nodo";
                btnAddNode.Location = new Point(525, 102);
                btnAddNode.Size = new Size(80, 28);

                btnEditNode.Text = "✏️ Nodo";
                btnEditNode.Location = new Point(525, 136);
                btnEditNode.Size = new Size(80, 28);

                btnDeleteNode.Text = "🗑 Nodo";
                btnDeleteNode.Location = new Point(525, 170);
                btnDeleteNode.Size = new Size(80, 28);

                btnDuplicateNode.Text = "📄 Nodo";
                btnDuplicateNode.Location = new Point(525, 204);
                btnDuplicateNode.Size = new Size(80, 28);

                btnOk.Text = "OK";
                btnOk.Location = new Point(444, 340);
                btnOk.Size = new Size(75, 28);
                btnOk.DialogResult = DialogResult.OK;

                btnCancel.Text = "Annulla";
                btnCancel.Location = new Point(530, 340);
                btnCancel.Size = new Size(75, 28);
                btnCancel.DialogResult = DialogResult.Cancel;

                foreach (var node in initialChildren)
                {
                    var item = new ListViewItem(node.Key);
                    item.SubItems.Add(node.Value);
                    lvNodes.Items.Add(item);
                }

                chkArray.Checked = IsArrayParentKey(key) || initialChildren.Count > 0;

                void RefreshArrayUi()
                {
                    var enabled = chkArray.Checked;
                    lblNodes.Enabled = enabled;
                    lvNodes.Enabled = enabled;
                    btnAddNode.Enabled = enabled;
                    btnEditNode.Enabled = enabled;
                    btnDeleteNode.Enabled = enabled;
                    btnDuplicateNode.Enabled = enabled;
                    txtValue.ReadOnly = enabled;
                    if (enabled) txtValue.Text = "ARRAY";
                }

                chkArray.CheckedChanged += (s, e) => RefreshArrayUi();

                btnAddNode.Click += (s, e) =>
                {
                    if (!TryShowSimpleNodeDialog(string.Empty, string.Empty, out var nodeKey, out var nodeValue)) return;
                    var item = new ListViewItem(nodeKey);
                    item.SubItems.Add(nodeValue);
                    lvNodes.Items.Add(item);
                };

                btnEditNode.Click += (s, e) =>
                {
                    if (lvNodes.SelectedItems.Count == 0) return;
                    var sel = lvNodes.SelectedItems[0];
                    var oldNodeKey = sel.Text;
                    var oldNodeValue = sel.SubItems.Count > 1 ? sel.SubItems[1].Text : string.Empty;
                    if (!TryShowSimpleNodeDialog(oldNodeKey, oldNodeValue, out var nodeKey, out var nodeValue)) return;
                    sel.Text = nodeKey;
                    if (sel.SubItems.Count > 1) sel.SubItems[1].Text = nodeValue;
                    else sel.SubItems.Add(nodeValue);
                };

                btnDeleteNode.Click += (s, e) =>
                {
                    if (lvNodes.SelectedItems.Count == 0) return;
                    lvNodes.Items.Remove(lvNodes.SelectedItems[0]);
                };

                btnDuplicateNode.Click += (s, e) =>
                {
                    if (lvNodes.SelectedItems.Count == 0) return;
                    var sel = lvNodes.SelectedItems[0];
                    var nodeKey = sel.Text;
                    var nodeValue = sel.SubItems.Count > 1 ? sel.SubItems[1].Text : string.Empty;

                    var newNodeKey = nodeKey;
                    var suffix = 2;
                    while (lvNodes.Items.Cast<ListViewItem>().Any(i =>
                        string.Equals(i.Text, newNodeKey, StringComparison.OrdinalIgnoreCase)))
                    {
                        newNodeKey = nodeKey + "_" + suffix;
                        suffix++;
                    }

                    var dup = new ListViewItem(newNodeKey);
                    dup.SubItems.Add(nodeValue);
                    lvNodes.Items.Add(dup);
                    dup.Selected = true;
                };

                dlg.Controls.AddRange(new Control[]
                {
                    lblKey, txtKey, lblValue, txtValue, chkArray,
                    lblNodes, lvNodes, btnAddNode, btnEditNode, btnDeleteNode, btnDuplicateNode,
                    btnOk, btnCancel
                });
                dlg.AcceptButton = btnOk;
                dlg.CancelButton = btnCancel;

                RefreshArrayUi();

                if (dlg.ShowDialog(this) != DialogResult.OK) return false;

                var newKey = txtKey.Text.Trim();
                if (string.IsNullOrWhiteSpace(newKey))
                {
                    MessageBox.Show("La chiave è obbligatoria.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (chkArray.Checked && lvNodes.Items.Count == 0)
                {
                    MessageBox.Show("Per un array inserire almeno un nodo figlio.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                resultKey = newKey;
                resultValue = chkArray.Checked ? "ARRAY" : txtValue.Text;

                foreach (ListViewItem item in lvNodes.Items)
                {
                    var k = item.Text.Trim();
                    if (string.IsNullOrWhiteSpace(k)) continue;
                    var v = item.SubItems.Count > 1 ? item.SubItems[1].Text : string.Empty;
                    children.Add(new KeyValuePair<string, string>(k, v));
                }

                return true;
            }
        }

        private bool TryShowSimpleNodeDialog(string key, string value, out string resultKey, out string resultValue)
        {
            resultKey = string.Empty;
            resultValue = string.Empty;

            using (var dlg = new Form())
            using (var lblKey = new Label())
            using (var txtKey = new TextBox())
            using (var lblValue = new Label())
            using (var txtValue = new TextBox())
            using (var btnOk = new Button())
            using (var btnCancel = new Button())
            {
                dlg.Text = "Nodo figlio";
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.ClientSize = new Size(480, 145);

                lblKey.Text = "Nodo";
                lblKey.Location = new Point(12, 15);
                lblKey.Size = new Size(70, 20);

                txtKey.Location = new Point(85, 12);
                txtKey.Size = new Size(380, 22);
                txtKey.Text = key;

                lblValue.Text = "Valore";
                lblValue.Location = new Point(12, 47);
                lblValue.Size = new Size(70, 20);

                txtValue.Location = new Point(85, 44);
                txtValue.Size = new Size(380, 22);
                txtValue.Text = value;

                btnOk.Text = "OK";
                btnOk.Location = new Point(309, 100);
                btnOk.Size = new Size(75, 28);
                btnOk.DialogResult = DialogResult.OK;

                btnCancel.Text = "Annulla";
                btnCancel.Location = new Point(390, 100);
                btnCancel.Size = new Size(75, 28);
                btnCancel.DialogResult = DialogResult.Cancel;

                dlg.Controls.AddRange(new Control[] { lblKey, txtKey, lblValue, txtValue, btnOk, btnCancel });
                dlg.AcceptButton = btnOk;
                dlg.CancelButton = btnCancel;

                if (dlg.ShowDialog(this) != DialogResult.OK) return false;

                var newKey = txtKey.Text.Trim();
                if (string.IsNullOrWhiteSpace(newKey))
                {
                    MessageBox.Show("Il nome nodo è obbligatorio.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                resultKey = newKey;
                resultValue = txtValue.Text;
                return true;
            }
        }

        private static bool IsArrayParentKey(string key)
            => string.Equals(key, "ElencoDettagliPrescrizioni", StringComparison.OrdinalIgnoreCase)
            || string.Equals(key, "ElencoDettagliPrescrInviiErogato", StringComparison.OrdinalIgnoreCase);

        private static List<KeyValuePair<string, string>> ExtractChildren(ListView list, string parentKey)
        {
            var prefix = parentKey + "_";
            var children = new List<KeyValuePair<string, string>>();

            foreach (ListViewItem item in list.Items)
            {
                var key = item.Text ?? string.Empty;
                if (!key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) continue;

                var childName = key.Substring(prefix.Length);
                var value = item.SubItems.Count > 1 ? item.SubItems[1].Text : string.Empty;
                children.Add(new KeyValuePair<string, string>(childName, value));
            }

            return children;
        }

        private static void RemoveChildren(ListView list, string parentKey)
        {
            var prefix = parentKey + "_";
            var toDelete = new List<ListViewItem>();

            foreach (ListViewItem item in list.Items)
            {
                var key = item.Text ?? string.Empty;
                if (key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    toDelete.Add(item);
            }

            foreach (var item in toDelete)
                list.Items.Remove(item);
        }

        private static void UpsertChildren(ListView list, string parentKey, List<KeyValuePair<string, string>> children)
        {
            var grouped = children
                .Where(c => !string.IsNullOrWhiteSpace(c.Key))
                .GroupBy(c => c.Key.Trim(), StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.Last().Value, StringComparer.OrdinalIgnoreCase);

            foreach (var kv in grouped)
            {
                var fullKey = parentKey + "_" + kv.Key;
                ListViewItem? existing = null;

                foreach (ListViewItem item in list.Items)
                {
                    if (string.Equals(item.Text, fullKey, StringComparison.OrdinalIgnoreCase))
                    {
                        existing = item;
                        break;
                    }
                }

                if (existing == null)
                {
                    var item = new ListViewItem(fullKey);
                    item.SubItems.Add(kv.Value);
                    list.Items.Add(item);
                }
                else
                {
                    if (existing.SubItems.Count > 1) existing.SubItems[1].Text = kv.Value;
                    else existing.SubItems.Add(kv.Value);
                }
            }
        }
    }
}
